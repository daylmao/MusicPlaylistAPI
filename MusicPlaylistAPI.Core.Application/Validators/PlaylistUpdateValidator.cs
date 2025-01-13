using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Validators
{
    public class PlaylistUpdateValidator: AbstractValidator<PlaylistUpdateDTO>
    {
        public PlaylistUpdateValidator()
        {
            RuleFor(b => b.Name)
                        .NotEmpty().WithMessage("{PropertyName} is required")
                        .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters");

            RuleFor(b => b.Description)
                        .Length(25, 100)
                        .WithMessage("{PropertyName} must be between 25 and 100 characters");

        }
    }
}
