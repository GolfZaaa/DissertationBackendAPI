using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenService _tokenService;
        private readonly DataContext _dataContext;
        private readonly IMemoryCache _memoryCache;
        private readonly SendGridClient _sendGridClient;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService, DataContext dataContext,
             IMemoryCache memoryCache,SendGridClient sendGridClient)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _dataContext = dataContext;
            _memoryCache = memoryCache;
            _sendGridClient = sendGridClient;
        }




        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userManager.Users.ToListAsync();
            List<Object> users = new();
            foreach (var user in result)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                users.Add(new { user.UserName, userRole });
            }
            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var LoginValidator = new LoginDtoValidator();
            var LoginValidationResult = LoginValidator.Validate(loginDto);

            if (!LoginValidationResult.IsValid)
            {
                var errors = LoginValidationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Login Error", Errors = errors });
            }

            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                return NotFound(new ResponseReport { Status = "404", Message = "Invalid username or password. Please try again." });
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var loginAttempts = _dataContext.LoginAttempts.Where(x => x.UserName == loginDto.Username).ToList();

                if (loginAttempts.Count == 0)
                {
                    var newLoginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        UserName = loginDto.Username,
                        DateTimeLogin = DateTime.Now,
                        CountTimeLogin = 1
                    };
                    user.AccessFailedCount = 1;
                    _dataContext.Add(newLoginAttempt);
                    _dataContext.SaveChanges();

                    return BadRequest(new ResponseReport { Status = "400", Message = "Failed to login. You have 2 more attempts." });
                }
                else if (loginAttempts.Count == 1)
                {
                    var newLoginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        UserName = loginDto.Username,
                        DateTimeLogin = DateTime.Now,
                        CountTimeLogin = 2
                    };
                    user.AccessFailedCount = 2;
                    _dataContext.Add(newLoginAttempt);
                    _dataContext.SaveChanges();

                    return BadRequest(new ResponseReport { Status = "400", Message = "Failed to login. You have 1 more attempt." });
                }
                else if (loginAttempts.Count == 2)
                {
                    var newLoginAttempt = new LoginAttempt
                    {
                        UserId = user.Id,
                        UserName = loginDto.Username,
                        DateTimeLogin = DateTime.Now,
                        CountTimeLogin = 3
                    };
                    user.AccessFailedCount = 3;
                    _dataContext.Add(newLoginAttempt);
                    _dataContext.SaveChanges();
                    return BadRequest(new ResponseReport { Status = "400", Message = "Your account is temporarily blocked. Please login again after 1 day." });
                }
                else if (loginAttempts.Count == 3)
                {
                    var test = loginAttempts.Last();

                    if (DateTime.Now - test.DateTimeLogin >= TimeSpan.FromMinutes(1))
                    {
                        _dataContext.RemoveRange(loginAttempts);
                        _dataContext.SaveChanges();

                        var newLoginAttempt = new LoginAttempt
                        {
                            UserId = user.Id,
                            UserName = loginDto.Username,
                            DateTimeLogin = DateTime.Now,
                            CountTimeLogin = 1
                        };
                        user.AccessFailedCount = 1;
                        _dataContext.Add(newLoginAttempt);
                        _dataContext.SaveChanges();
                        return BadRequest(new ResponseReport { Status = "400", Message = "Failed to login. You have 1 more attempt." });

                    }
                    return BadRequest(new ResponseReport { Status = "400", Message = "Your account is temporarily blocked. Please login again after 5 minute." });

                }
            }

            if (!user.EmailConfirmed)
            {
                return BadRequest(new ResponseReport { Status = "404", Message = "Please confirm your email for the first login." });
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var userDto = new UserDto
            {
                Email = user.Email,
                Token = await _tokenService.GenerateToken(user),
                username = loginDto.Username,
                UserId = userId,
                // ProfileImage = user.ProfileImage,
            };

            return Ok(userDto);
        }



        [HttpPost("register")]
        public async Task<object> RegisterAsync(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator(_dataContext);
            var resultvalidate = validator.Validate(registerDto);

            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
            }

            var check = await _userManager.FindByEmailAsync(registerDto.Email);
            if (check != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "404", Message = "This e-mail has already been used." });
            }
            var Checkrole = await _roleManager.RoleExistsAsync(registerDto.Role);
            if (!Checkrole)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "404", Message = "The specified role does not exist." });
            }
            var createuser = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                EmailConfirmed = false,
            };
            var result = await _userManager.CreateAsync(createuser, registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }
            await _userManager.AddToRoleAsync(createuser, registerDto.Role);
            // สร้าง token สำหรับการยืนยันอีเมล์

            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            string token = randomNumber.ToString();

            _memoryCache.Set("Token", token, TimeSpan.FromDays(1));

            await _userManager.UpdateAsync(createuser);

            if (!string.IsNullOrEmpty(token))
            {
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                await SendEmailConfirmationEmail(createuser.Email, token);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "ไม่ได้รับค่า emailConfirmationUrl ที่ถูกต้อง" });
            }
            return StatusCode(StatusCodes.Status201Created, new ResponseReport { Status = "201", Message = "Create Successfully" });

        }

        private async Task SendEmailConfirmationEmail(string email, string token)
        {
            var cachedToken = _memoryCache.Get<string>("Token");
            if (!string.IsNullOrEmpty(cachedToken))
            {
                // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                var from = new EmailAddress("64123250113@kru.ac.th", "Golf");
                var to = new EmailAddress(email);
                var subject = "Thank you";

                var expirationTime = DateTime.Now.AddHours(24); // Assuming the token is valid for 24 hours

                var htmlContent = "<div style=\"text-align: center; background-color: #f0f0f0; padding: 20px; border-radius: 10px; box-shadow: 0 8px 12px rgba(0, 0, 0, 0.2);\">";
                htmlContent += "<img src=\"https://www.kru.ac.th/kru/assets/img/kru/logo/kru_color.png\" alt=\"Your Logo\" style=\"max-width: 130px;\" />";
                htmlContent += "<p><strong><h1 style=\"font-size:3.2em; color: #0073e6; font-family: 'Arial', sans-serif; margin-bottom: 20px;\">Confirm Your Email</h1></strong></p>";
                htmlContent += "<p style=\"font-size: 1.6em; color: #555; font-family: 'Helvetica', sans-serif;\">Thank you for registering with us!</p>";
                htmlContent += "<p style=\"font-size: 1.6em; color: #555; font-family: 'Helvetica', sans-serif;\">To complete your registration, please confirm your email address by using the following token:</p>";
                htmlContent += $"<p><strong><h1 style=\"font-size:5em; color: #ff5733; font-family: 'Times New Roman', serif; letter-spacing: 10px;\">{cachedToken}</h1></strong></p>";
                htmlContent += "<p style=\"font-size: 1.2em; color: #888;\">This token will expire on " + expirationTime.ToString("MMM dd, yyyy HH:mm tt") + " (UTC).</p>";
                htmlContent += "</div>";
                htmlContent += "<p style=\"text-align: center; font-size: 1em; color: #888; margin-top: 30px;\">If you didn't register, please ignore this email.</p>";



                var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, htmlContent, htmlContent);
                await _sendGridClient.SendEmailAsync(emailMessage);

                ConfirmUserDto confirmUserDto = new()
                {
                    Email = email,
                    EmailConfirmationToken = cachedToken
                };

            }
        }

        [HttpPost("ChangePassword")]
        public async Task<object> ChangePassword(ChangePasswordDto dto)
        {
            var validator = new ChangePasswordValidator();
            var resultvalidate = validator.Validate(dto);

            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
            }
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return Unauthorized();
            }

            var isOldPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isOldPasswordValid)
            {
                return BadRequest(new { Message = "password is incorrect" });
            }

            var changePasswordResult = await _userManager.RemovePasswordAsync(user);

            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(new { Message = "Failed to change password" });
            }

            changePasswordResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(new { Message = "Failed to change password" });
            }

            return Ok(new { Message = "Password changed successfully" });

        }

        [HttpPost("ChangeEmail")]
        public async Task<object> ChangeUserEmail(ChangeUserEmailDto dto)
        {
            var validator = new ChangeEmailValidator();
            var resultvalidate = validator.Validate(dto);

            if(!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
            }
                var user = await _userManager.FindByIdAsync(dto.UserId);

                if (user == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "User Not Found" });
                }

                if (user.Email == dto.NewEmail)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "400", Message = "The new Email is the same as the current Email you are using. Please enter a Email that is different from the current one." });
                }
                var result = await _userManager.SetEmailAsync(user, dto.NewEmail);
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "Fail to SetEmail" });
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseReport { Status = "200", Message = "Change Email Successfuly" });
            
        }

        [HttpPost("ShowAllUser")]
        public IActionResult ShowAllUser()
        {
            var users = _dataContext.Users.ToList();

            var usersWithRoles = users.Select(user => new
            {
                UserId = user.Id,
                UserNames = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result,
                EmailConfirm = user.EmailConfirmed,
                AccessLoginFailed = user.AccessFailedCount,
            }).ToList();

            return Ok(usersWithRoles);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string userid, string role)
        {
            var user = await _userManager.FindByIdAsync(userid);

            if (user == null)
            {
                return NotFound("ไม่พบผู้ใช้");
            }
            //RoleExistsAsync ตรวจสอบว่าบทบาทนี้มีอยู่หรือเปล่า 
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("บทบาทไม่มีอยู่");
            }

            // IsInRoleAsync ตรวจสอบว่าผู้ใช้มีบทบาทนี้อยู่แล้วหรือเปล่า
            if (await _userManager.IsInRoleAsync(user, role))
            {
                return BadRequest("ผู้ใช้มีบทบาทนี้อยู่แล้ว");
            }

            // เพิ่มบทบาทให้กับผู้ใช้
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                return Ok($"เพิ่มบทบาท {role} ให้กับผู้ใช้ {user.UserName} สำเร็จ");
            }
            else
            {
                return BadRequest("เกิดข้อผิดพลาดในการเพิ่มบทบาท");
            }
        }

        [HttpPost("ChangeUserName")]
        public async  Task<IActionResult> ChangeUserName (ChangeUserNameDto dto)
        {
            var validator = new ChangeUserNameValidator();
            var resultvalidate = validator.Validate(dto);

            if (!resultvalidate.IsValid)
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Username is Emtry", Errors = errors });
            }
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "User Not Found" });
            }
            if (user.UserName == dto.NewUserName)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "400", Message = "The new UserName is the same as the current UserName you are using. Please enter a UserName that is different from the current one." });
            }
            var result = await _userManager.SetUserNameAsync(user, dto.NewUserName);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "Fail to SetUserName" });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseReport { Status = "200", Message = "Change UserName Successfully, please logout and login to get token again" });
        }


    }
}
