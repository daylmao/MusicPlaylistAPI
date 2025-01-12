using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Validators
{
    public class CreatePlaylistValidator: AbstractValidator<PlaylistInsertDTO>
    {
        public CreatePlaylistValidator()
        {
            RuleFor(b => b.Name)
             .NotEmpty().WithMessage("Name is required")
             .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

            RuleFor(b => b.Description)
                .MaximumLength(100).WithMessage("Description must not exceed 100 characters")
                .When(b => b.Description != null); 

        }
    }
}
