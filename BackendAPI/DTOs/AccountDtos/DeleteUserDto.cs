using FluentValidation;

namespace BackendAPI.DTOs.AccountDtos;
    public class DeleteUserDto
    {
        public string UserId { get; set; }
    }


public class DeleteUserValidator : AbstractValidator<DeleteUserDto>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is empty");
    }
}