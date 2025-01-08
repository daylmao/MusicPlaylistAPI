using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Validators
{
    public class SongInsertValidator : AbstractValidator<SongInsertDTO>
    {
        public SongInsertValidator()
        {
            RuleFor(song => song.Artist)
                .NotEmpty().WithMessage("Artist is required");

            RuleFor(song => song.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters");

            RuleFor(song => song.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .Must(BeAValidGenre).WithMessage("Genre must be a valid type");

            RuleFor(song => song.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0");
        }

        private bool BeAValidGenre(string? genre)
        {
            var allowedGenres = new[] { "Pop", "Rock", "Jazz", "Classical", "Hip-Hop" };
            return genre != null && allowedGenres.Contains(genre);
        }
    }

}
