using BackendAPI.Core;
using BackendAPI.Data;
using BackendAPI.DTOs;
using BackendAPI.DTOs.AccountDtos;
using BackendAPI.Models;
using BackendAPI.Response;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Controllers;
public class AccountController : BaseApiController
{
    private readonly DataContext _dataContext;
    private readonly IAccountServices _accountServices;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(DataContext dataContext, IAccountServices accountServices, UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
    {
        _dataContext = dataContext;
        _accountServices = accountServices;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _roleManager = roleManager;
    }

    [HttpGet("GetAllUserService")]
    public async Task<ActionResult> ShowAllUser()
    {
        return HandleResult(await _accountServices.AllUsers());
    }

    [HttpPost("AddRoleService")]
    public async Task<IActionResult> AddRole(AddRoleUserDto dto)
    {
        var validator = new AddRoleValidator();
        var resultvalidate = validator.Validate(dto);
        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change Email is Emtry", Errors = errors });
        }

        return HandleResult(await _accountServices.AddRoleAsync(dto));
    }

    [HttpPost("loginService")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var LoginValidator = new LoginDtoValidator();
        var LoginValidationResult = LoginValidator.Validate(dto);

        if (!LoginValidationResult.IsValid)
        {
            var errors = LoginValidationResult.Errors.Select(error => error.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Login Error", Errors = errors });
        }

        return HandleResult(await _accountServices.LoginAsync(dto));
    }

    [HttpPost("registerService")]
    public async Task<object> Register(RegisterDto registerDto)
    {
        var validator = new RegisterDtoValidator(_dataContext);
        var resultvalidate = validator.Validate(registerDto);

        if (!resultvalidate.IsValid)
        {
            var errors = resultvalidate.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(new { Message = "Validation Change Password is Emtry", Errors = errors });
        }

        return HandleResult(await _accountServices.RegisterAsync(registerDto));
    }

    //[HttpDelete("DeleteUserService")]
    [HttpPost("DeleteUserService")]
    public async Task<ActionResult> DeleteUser(string userId)
    {
        return HandleResult(await _accountServices.DeleteAsync(userId));
    }

    [HttpPost("AddProfileUser")]
    public async Task<ActionResult> AddProfileUser(AddProfileUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.userId);
        if(user == null)
        {
            return HandleResult(Result<string>.Failure("User Null."));
        }

        if (!string.IsNullOrEmpty(dto.firstName))
        {
            user.FirstName = dto.firstName;
        }

        if (!string.IsNullOrEmpty(dto.lastName))
        {
            user.LastName = dto.lastName;
        }

        if (!string.IsNullOrEmpty(dto.phoneNumber))
        {
            user.PhoneNumber = dto.phoneNumber;
        }

        if (dto.agencyId != null)
        {
            user.AgencyId = dto.agencyId;
        }

        await _dataContext.SaveChangesAsync();
        return HandleResult(Result<string>.Success("UpdateSuccess"));
    }

    [HttpGet("SearchUserByUserId")]
    public async Task<ActionResult> SearchUserByUserId(string userId)
    {
        var search = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == userId);

        if(search == null)
        {
            return HandleResult(Result<string>.Failure("Not Found User"));
        }

        return HandleResult(Result<object>.Success(search));
    }



    [HttpGet("GetRemainingRolesByUserId")]
    public async Task<ActionResult<Result<List<RoleInfo>>>> GetRemainingRolesByUserId(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound(Result<List<RoleInfo>>.Failure("User not found."));

        var userRoles = await _userManager.GetRolesAsync(user);

        var allRoles = await _roleManager.Roles.ToListAsync();

        var remainingRoles = allRoles
            .Where(role => !userRoles.Contains(role.Name))
            .Select(role => new RoleInfo
            {
                Id = role.Id,
                ConcurrencyStamp = role.ConcurrencyStamp
            })
        .ToList();
        return Ok(Result<List<RoleInfo>>.Success(remainingRoles));
    }


    [HttpGet("CreateAdminIfNoUser")]
    public async Task<ActionResult> CreateAdminIfNoUser()
    {
        var users = await _userManager.Users.ToListAsync();

        if (users.Count == 0)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@kru.ac.th",
                FirstName = "",
                LastName = "",
                ProfileImage = "",
                EmailConfirmed = true,
            };
            var result = await _userManager.CreateAsync(adminUser, "!A@we1235");

            if (result != null)
            {
                await _userManager.AddToRoleAsync(adminUser, "Administrator");

                return Ok("Admin user created successfully.");
            }
            else
            {
                return BadRequest("Failed to create admin user.");
            }
        }

        return Ok("have Users already.");
    }


    [HttpGet("GetEmailUserByUserName")]
    public async Task<ActionResult> GetEmailUserByUserName(string username)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.UserName == username);

        var result = new
        {
            user = new
            {
                user.Email
            }
        };
        return Ok(result);

    }


    [HttpPost("TurnOnOffUser")]
    public async Task<ActionResult> TurnOnOffUser(TurnOnOffAccountDto dto)
    {
        var account = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (account == null)
        {
            HandleResult(Result<string>.Failure("Not Found Account"));
        }

        if (dto.StatusOnOff == 0)
        {
            account.StatusOnOff = 0;
        }
        else
        {
            account.StatusOnOff = 1;
        }
        await _dataContext.SaveChangesAsync();
        return Ok(account);
    }


    [HttpPost("DeleteRoleUser")]
    public async Task<ActionResult> DeleteRoleUser(DeleteRoleUserDto dto)
    {
        var role = await _dataContext.Roles.FirstOrDefaultAsync(x => x.Name == dto.roleName);
        if (role == null)
            return HandleResult(Result<string>.Failure("Cannot Found Role"));

        var user = await _userManager.FindByIdAsync(dto.userId);
        if (user == null)
            return HandleResult(Result<string>.Failure("Cannot Found User"));

        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
        if (result.Succeeded)
            return HandleResult(Result<string>.Success("Successfully User removed from role"));
        else
            return HandleResult(Result<string>.Failure("Failed to remove user from role"));
    }







    //[HttpGet("SearUserandAgencyByUserId")]
    //public async Task<ActionResult> SearhUserandAgencyByUserId(string userId)


}
