using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TokenService tokenService,DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _dataContext = dataContext;
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

            if (loginDto == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "Invalid request. Please provide valid credentials." });
            }


            var check = await _userManager.FindByNameAsync(loginDto.Username);

            if (check == null || !await _userManager.CheckPasswordAsync(check, loginDto.Password))
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "404", Message = "Invalid username or password. Please try again." });
            }

            if(check.UserName == loginDto.Username)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseReport { Status = "404", Message = "Invalid password. Please try again." });

            }

            if (check.EmailConfirmed == false)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "404", Message = "Please confirm your email for the first login." });
            }
            else
            {
                var userid = await _userManager.GetUserIdAsync(check);
                var userDto = new UserDto
                {
                    Email = check.Email,
                    Token = await _tokenService.GenerateToken(check),
                    username = loginDto.Username,
                    userid = userid,
                    //ProfileImage = check.ProfileImage,
                };
                return Ok(userDto);
            }
        }

        [HttpPost("register")]
        public async Task<object> RegisterAsync(RegisterDto registerDto)
        {
            var validator = new RegisterDtoValidator(_dataContext);
            var resultvalidate = validator.Validate(registerDto);

            if (resultvalidate.IsValid)
            {
                var check = await _userManager.FindByEmailAsync(registerDto.Email);
                if (check != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "404", Message = "This e-mail has already been used." });
                }
                var roleExists = await _roleManager.RoleExistsAsync(registerDto.Role);
                if (!roleExists)
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

                //Random random = new Random();
                //int randomNumber = random.Next(1000, 9999);
                //string token = randomNumber.ToString();

                //_memoryCache.Set("Token", token, TimeSpan.FromDays(1));

                //await _userManager.UpdateAsync(createuser);

                //if (!string.IsNullOrEmpty(token))
                //{
                //    // ส่งอีเมล์ยืนยันอีเมล์ไปยังผู้ใช้
                //    await SendEmailConfirmationEmail(createuser.Email, token);
                //}
                //else
                //{
                //    return StatusCode(StatusCodes.Status400BadRequest, new ResponseReport { Status = "400", Message = "ไม่ได้รับค่า emailConfirmationUrl ที่ถูกต้อง" });
                //}
                return StatusCode(StatusCodes.Status201Created, new ResponseReport { Status = "201", Message = "Create Successfully" });
            }
            else
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
            }
        }



        [HttpPost("ChangePassword")]
        public async Task<object> ChangePassword(ChangePasswordDto dto)
        {
            var validator = new ChangePasswordValidator();
            var resultvalidate = validator.Validate(dto);

            if (resultvalidate.IsValid)
            {
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
            else
            {
                var errors = resultvalidate.Errors.Select(x=>x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
            }
        }

        [HttpPost("ChangeEmail")]
        public async Task<object> ChangeUserEmailAsync(ChangeUserEmailDto dto)
        {
            var validator = new ChangeEmailValidator();
            var resultvalidate = validator.Validate(dto);

            if(resultvalidate.IsValid)
            {
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
            else
            {
                var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });

            }
        }






    }
}
