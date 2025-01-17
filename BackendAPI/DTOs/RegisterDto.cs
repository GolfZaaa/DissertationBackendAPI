﻿using BackendAPI.Data;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BackendAPI.DTOs;

public class RegisterDto : LoginDto
{
    public string Email { get; set; }
    public string Role { get; set; }


}

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator(DataContext dataContext)
    {
        var Roles = dataContext.Roles.Select(x=>x.Name).ToList();
        RuleFor(register => register.Email).NotEmpty().WithMessage("Email is empty");
        RuleFor(register => register.Username).NotEmpty().WithMessage("Username is empty");
        RuleFor(register => register.Password).NotEmpty().WithMessage("Password is empty");
        RuleFor(register => register.Role).NotEmpty().WithMessage("Role is empty").Must(Roles.Contains).WithMessage("Incorrect position");
    }

}


