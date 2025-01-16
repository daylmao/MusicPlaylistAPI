using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Validators
{
    public class UserUpdateValidator : AbstractValidator<UserUpdateDTO>
    {
        public UserUpdateValidator()
        {

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("{PropertyName} cannot exceed 100 characters.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MinimumLength(6).WithMessage("{PropertyName} must be at least 6 characters");

            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
