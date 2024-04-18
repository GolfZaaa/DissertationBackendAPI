using BackendAPI.Controllers;
using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BackendAPI.Services
{
    public class AccountService : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMemoryCache _memoryCache;
        private readonly SendGridClient _sendGridClient;
        private readonly TokenService _tokenService;

        public AccountService(UserManager<ApplicationUser> userManager, DataContext dataContext, RoleManager<IdentityRole> roleManager
            , IMemoryCache memoryCache, SendGridClient sendGridClient, TokenService tokenService)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _roleManager = roleManager;
            _memoryCache = memoryCache;
            _sendGridClient = sendGridClient;
            _tokenService = tokenService;
        }

        public async Task<Result<string>> AddRoleAsync(AddRoleUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return Result<string>.Failure("User not found.");

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role == null)
                return Result<string>.Failure("Role not found.");

            if (await _userManager.IsInRoleAsync(user, role.Name))
                return Result<string>.Failure("User already has this role.");

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
                return Result<string>.Success($"AddRole Success");
            else
                return Result<string>.Failure("An error occurred while adding a role.");
        }


        public async Task<Result<object>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            //เช็ค UserLogin ผิดเกิน 3 ครั้ง
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var loginAttempts = _dataContext.LoginAttempts.Where(x => x.UserName == dto.Username).ToList();

                if (loginAttempts.Count == 0)
                {
                    var newLoginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        UserName = dto.Username,
                        DateTimeLogin = DateTime.Now,
                        CountTimeLogin = 1
                    };
                    user.AccessFailedCount = 1;
                    _dataContext.Add(newLoginAttempt);
                    _dataContext.SaveChanges();

                    return Result<object>.Failure("Failed to login. You have 2 more attempts.");
                }
                else if (loginAttempts.Count == 1)
                {
                    var newLoginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        UserName = dto.Username,
                        DateTimeLogin = DateTime.Now,
                        CountTimeLogin = 2
                    };
                    user.AccessFailedCount = 2;
                    _dataContext.Add(newLoginAttempt);
                    _dataContext.SaveChanges();

                    return Result<object>.Failure("Failed to login. You have 1 more attempts.");
                }
                else if (loginAttempts.Count == 2)
                {
                    var newLoginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        UserName = dto.Username,
                        DateTimeLogin = DateTime.Now,
                        CountTimeLogin = 3
                    };
                    user.AccessFailedCount = 3;
                    _dataContext.Add(newLoginAttempt);
                    _dataContext.SaveChanges();
                    return Result<object>.Failure("Your account is temporarily blocked. Please login again after 10 Seconds.");
                }
                else if (loginAttempts.Count == 3)
                {
                    var test = loginAttempts.Last();

                    if (DateTime.Now - test.DateTimeLogin >= TimeSpan.FromSeconds(10))
                    {
                        _dataContext.RemoveRange(loginAttempts);
                        _dataContext.SaveChanges();

                        var newLoginAttempt = new LoginAttempt
                        {
                            UserId = user.Id,
                            UserName = dto.Username,
                            DateTimeLogin = DateTime.Now,
                            CountTimeLogin = 1
                        };
                        user.AccessFailedCount = 1;
                        _dataContext.Add(newLoginAttempt);
                        _dataContext.SaveChanges();
                        return Result<object>.Failure("Failed to login. You have 2 more attempts.");
                    }
                    return Result<object>.Failure("Your account is temporarily blocked. Please login again after 10 Seconds.");
                }
            }

            if (!user.EmailConfirmed)return Result<object>.Failure("Please confirm your email for the first login.");

            if (user.StatusOnOff == 0) return Result<object>.Failure("Admin block user already");

            var userId = await _userManager.GetUserIdAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = new UserDto
            {
                Token = await _tokenService.GenerateToken(user),
                UserId = userId,
                role = roles.ToArray(),
                ProfileImage = user.ProfileImage,
            };
            return Result<object>.Success(userDto);

        }

        public async Task<Result<object>> RegisterAsync(RegisterDto registerDto)
        {
            var check = await _userManager.FindByEmailAsync(registerDto.Email);
            if (check != null) return Result<object>.Failure("This e-mail has already been used.");

            var Checkrole = await _roleManager.RoleExistsAsync(registerDto.Role);
            if (!Checkrole) return Result<object>.Failure("The specified role does not exist.");

            var CheckUser = await _userManager.FindByNameAsync(registerDto.Username);
            if (CheckUser != null) return Result<object>.Failure("This user has already been used.");


            var createuser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                EmailConfirmed = false,
                FirstName = "",
                LastName = "",
                ProfileImage = "",
            };

            var result = await _userManager.CreateAsync(createuser, registerDto.Password);
            if (!result.Succeeded) return Result<object>.Failure("Register Faill.");

            await _userManager.AddToRoleAsync(createuser, registerDto.Role);
            // สร้าง token สำหรับการยืนยันอีเมล์

            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string token = randomNumber.ToString();

            _memoryCache.Set("Token", token, TimeSpan.FromDays(30));

            await _userManager.UpdateAsync(createuser);

            // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
            if (!string.IsNullOrEmpty(token)) await SendEmailConfirmationEmail(createuser.Email, token);
            else return Result<object>.Failure("The email delivery was unsuccessful.");

            return Result<object>.Success("Create Successfully");

        }

        private async Task SendEmailConfirmationEmail(string email, string token)
        {
            var cachedToken = _memoryCache.Get<string>("Token");
            if (!string.IsNullOrEmpty(cachedToken))
            {
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                var from = new EmailAddress("64123250113@kru.ac.th", "ผู้พัฒนา");
                var to = new EmailAddress(email);
                var subject = "ขอบคุณ";

                var expirationTime = DateTime.Now.AddHours(24);

                var htmlContent = "<div style=\"text-align: center; background-color: #f0f0f0; padding: 20px; border-radius: 10px; box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);\">";
                htmlContent += "<img src=\"https://ip.kru.ac.th/assets/img/kru.png\" alt=\"Your Logo\" style=\"max-width: 130px;\" />";
                htmlContent += "<p><strong><h1 style=\"font-size:2.7em; color: #0073e6; font-family: 'Arial', sans-serif; margin-bottom: 20px;\">ยืนยันอีเมลของคุณ</h1></strong></p>";
                htmlContent += "<p style=\"font-size: 1.6em; color: #555; font-family: 'Helvetica', sans-serif;\">ขอบคุณที่ลงทะเบียนกับเราครับ!</p>";
                htmlContent += "<p style=\"font-size: 1.6em; color: #555; font-family: 'Helvetica', sans-serif;\">เพื่อทำการลงทะเบียนเสร็จสิ้น กรุณายืนยันที่อยู่อีเมลของคุณโดยการใช้โทเค็นต่อไปนี้: </p>";
                htmlContent += $"<p><strong><h1 style=\"font-size:7em; color: #ff5733; font-family: 'Times New Roman', serif; letter-spacing: 10px; font-weight: bold;\">{cachedToken}</h1></strong></p>";
                htmlContent += "<p style=\"font-size: 1.2em; color: #888;\">โทเค็นนี้จะหมดอายุเมื่อ " + expirationTime.ToString(" dd MMM yyyy HH:mm tt") + " (UTC).</p>";
                htmlContent += "</div>";
                htmlContent += "<p style=\"text-align: center; font-size: 1em; color: #888; margin-top: 30px;\">หากคุณยังไม่ได้ลงทะเบียน กรุณาไม่ตอบกลับอีเมลนี้.</p>";

                var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                await _sendGridClient.SendEmailAsync(emailMessage);

                //ConfirmUserDto confirmUserDto = new()
                //{
                //    Email = email,
                //    EmailConfirmationToken = cachedToken
                //};

            }
        }

        public async Task<Result<List<object>>> AllUsers()
        {
            var users = await _dataContext.Users.ToListAsync();

            var usersWithRoles = users.Select(user => new
            {
                UserId = user.Id,
                UserNames = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result,
                EmailConfirm = user.EmailConfirmed,
                AccessLoginFailed = user.AccessFailedCount,
                ProfileImage = user.ProfileImage,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AgencyName = GetAgencyName(user.AgencyId).Result,
                StatusOnOff = user.StatusOnOff,
                //    RoleConcurrencyStamps = _userManager.GetRolesAsync(user).Result
                //.Select(role => _roleManager.Roles.Single(r => r.Name == role).ConcurrencyStamp)
                //.ToList(),
                RoleConcurrencyStamps = _userManager.GetRolesAsync(user).Result.Select(role => new
                {
                    RoleName = role,
                    ConcurrencyStamp = _roleManager.Roles.Single(r => r.Name == role).ConcurrencyStamp
                }).ToList()
            }).ToList<object>();

            return Result<List<object>>.Success(usersWithRoles);
        }

        private async Task<string> GetAgencyName(int agencyId)
        {
            var agency = await _dataContext.Agencys
                .Where(x => x.Id == agencyId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            return agency;
        }

        public async Task<Result<string>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return Result<string>.Failure("UserName not found.");

            if (await _userManager.CheckPasswordAsync(user, dto.Password))
                return Result<string>.Failure("The new Password is the same as the current Password you are using. Please enter a Password that is different from the current one");

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);

            if (!removePasswordResult.Succeeded) return Result<string>.Failure("Failed to remove existing password.");

            var changePasswordResult = await _userManager.AddPasswordAsync(user, dto.Password);

            if (!changePasswordResult.Succeeded)return Result<string>.Failure("Failed to change password.");

            return Result<string>.Success("Password changed successfully.");
        }

        public async Task<Result<string>> ChangeUserEmailAsync(ChangeUserEmailDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return Result<string>.Failure("User Not Found.");
            else if (user.Email == dto.NewEmail)
                return Result<string>.Failure("The new Email is the same as the current Email you are using. Please enter a Email that is different from the current one.");

            await _userManager.SetEmailAsync(user, dto.NewEmail);
            user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);
            return Result<string>.Success("Email Changing");
        }

        public async Task<Result<string>> ChangeUserNameAsync(ChangeUserNameDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)return Result<string>.Failure("User Not Found.");

            if (user.UserName == dto.NewUserName)
                return Result<string>.Failure("The new UserName is the same as the current UserName you are using. Please enter a UserName that is different from the current one.");
            

            var result = await _userManager.SetUserNameAsync(user, dto.NewUserName);
            if (!result.Succeeded)
                return Result<string>.Failure("Fail to SetUserName.");

            return Result<string>.Success("Change UserName Successfully.");
        }
            
        public async Task<Result<string>> ConfirmEmailUserAsync(ConfirmEmailUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Result<string>.Failure("User Not Found");

            var token = _memoryCache.Get<string>("Token");

            if (string.IsNullOrEmpty(token)) return Result<string>.Failure("Token has expired.");

            if (dto.TokenConfirm != token) return Result<string>.Failure("Token is Wrong");
            else user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);

            return Result<string>.Success("Email confirmed successfully.");
        }

        public async Task<Result<string>> DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return Result<string>.Success("Delete Successfully");
            }
            else
            {
                return Result<string>.Failure("Failed to Delete");
            }
        }


        public async Task<Result<string>> ResendOtpConfirmEmailAsync(ResendOtpConfirmEmailDto dto)
        {
            _memoryCache.Remove("Token");
            var token = GenerateOTPForConfirmEmail();
            _memoryCache.Set("Token", token, TimeSpan.FromDays(1));

            if (!string.IsNullOrEmpty(token))
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                await SendEmailConfirmationEmail(dto.Email, token);
            else
                // กรณีไม่ได้รับค่า emailConfirmationUrl ที่ถูกต้อง
                return Result<string>.Failure($"Failed to send email {dto.Email}.");

            return Result<string>.Success("Successful Resending");
        }
        private string GenerateOTPForConfirmEmail()
        {
            Random random = new Random();
            int RandomNumber = random.Next(1000, 9999);
            return RandomNumber.ToString();
        }



        //Forgot Password Start
        public async Task<Result<string>> ForgetPasswordAsync(ForgetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null) return Result<string>.Failure("User Not Found");

            if (dto.Password != dto.ConfirmPassword)
                return Result<string>.Failure("Password and confirm password do not match");

            //ทำการเช็คโดยใช้ identity Framework 
            //เช็คว่ารหัสที่จะเปลี่ยนตรงกับรหัส ณ ปัจจุบันที่ใช้อยู่รึเปล่า ถ้าใช่จะให้เข้า if Error   Start
            var CheckPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (CheckPassword) return Result<string>.Failure("New password must be different from the current password");
            //เช็คว่ารหัสที่จะเปลี่ยนตรงกับรหัส ณ ปัจจุบันที่ใช้อยู่รึเปล่า ถ้าใช่จะให้เข้า if Error   End

            var changePasswordResult = await _userManager.RemovePasswordAsync(user);

            if (!changePasswordResult.Succeeded)
                return Result<string>.Failure("Failed to change password");

            await _userManager.AddPasswordAsync(user, dto.Password);

            return Result<string>.Failure("Password changed successfully");
        }

        public async Task<Result<string>> sendOtpToForgotPasswordAsync(SendOtpToForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Result<string>.Failure("User Not Found");

            var Generate = GenerateOTPForForgotPassword();
            _memoryCache.Set("ForgotPassword", Generate, TimeSpan.FromDays(1));

            if (!string.IsNullOrEmpty(Generate))
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                await SendEmailForgotPassword(dto.Email, Generate);
            else
                return Result<string>.Failure($"Failed to send email {dto.Email}.");

            return Result<string>.Success("Successful Resending");
        }

        private async Task SendEmailForgotPassword(string email, string token)
        {
            var cachedTokenforgotpassword = _memoryCache.Get<string>("ForgotPassword");
            if (!string.IsNullOrEmpty(cachedTokenforgotpassword))
            {
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                var from = new EmailAddress("64123250113@kru.ac.th", "Golf");
                var to = new EmailAddress(email);
                var subject = "ForgotPassword";
                var expirationTime = DateTime.Now.AddHours(24);

                var htmlContent = "<div style=\"text-align: center; background-color: #f0f0f0; padding: 20px; border-radius: 10px; box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);\">";
                htmlContent += "<img src=\"https://ip.kru.ac.th/assets/img/kru.png\" alt=\"Your Logo\" style=\"max-width: 130px;\" />";
                htmlContent += "<p><strong><h1 style=\"font-size:2.7em; color: #0073e6; font-family: 'Arial', sans-serif; margin-bottom: 20px;\">Forgot Password</h1></strong></p>";
                htmlContent += "<p style=\"font-size: 1.6em; color: #555; font-family: 'Helvetica', sans-serif;\">Thank you for using website us!</p>";
                htmlContent += "<p style=\"font-size: 1.6em; color: #555; font-family: 'Helvetica', sans-serif;\">Enter the email address you used when you joined and we’ll send you instructions to reset your password.</p>";
                htmlContent += $"<p><strong><h1 style=\"font-size:7em; color: #ff5733; font-family: 'Times New Roman', serif; letter-spacing: 10px; font-weight: bold;\">{cachedTokenforgotpassword}</h1></strong></p>";
                htmlContent += "<p style=\"font-size: 1.2em; color: #888;\">This token will expire on " + expirationTime.ToString("MMM dd, yyyy HH:mm tt") + " (UTC).</p>";
                htmlContent += "</div>";
                htmlContent += "<p style=\"text-align: center; font-size: 1em; color: #888; margin-top: 30px;\">If you didn't register, please ignore this email.</p>";

                var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                await _sendGridClient.SendEmailAsync(emailMessage);

          
            }
        }

        public async Task<Result<string>> ResendOtpToForgotPasswordAsync(ResendOtpToForgotPasswordDto dto)
        {
            _memoryCache.Remove("ForgotPassword");
            var token = GenerateOTPForForgotPassword();
            _memoryCache.Set("ForgotPassword", token, TimeSpan.FromDays(1));

            if (!string.IsNullOrEmpty(token))
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                await SendEmailForgotPassword(dto.Email, token);
            else
                // กรณีไม่ได้รับค่า emailConfirmationUrl ที่ถูกต้อง
                return Result<string>.Failure($"Failed to send email {dto.Email}.");

            return Result<string>.Success("Successful Resending");
        }

        private string GenerateOTPForForgotPassword()
        {
            Random random = new Random();
            int RandomNumber = random.Next(1000, 9999);
            return RandomNumber.ToString();
        }

        public async Task<Result<string>> ConfirmEmailToForgotPasswordAsync(ConfirmForgotPasswordUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Result<string>.Failure("Not Found User");

            var token = _memoryCache.Get<string>("ForgotPassword"); // รับค่า token จาก memory cache

            if (string.IsNullOrEmpty(token))
                return Result<string>.Failure("token null.");

            if (string.IsNullOrEmpty(dto.TokenConfirm))
                return Result<string>.Failure("TokenConfirm null.");

            // เช็คว่าโทเค็นที่ผู้ใช้กรอกตรงกับโทเค็นที่เก็บในแคชไหม
            if (dto.TokenConfirm != token)
                return Result<string>.Failure("TokenConfirm Wrong.");

            return Result<string>.Failure("Confirmed token successfully go to ResetPassword.");
        }
        //Forgot Password End

    }
}
