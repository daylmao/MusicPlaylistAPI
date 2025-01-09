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
                        .NotEmpty().WithMessage("Name is required")
                        .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(b => b.Description)
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters");

            //falta validacion de song

            RuleFor(b => b.DateCreated)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("DateCreated cannot be in the future");
        }
    }
}
