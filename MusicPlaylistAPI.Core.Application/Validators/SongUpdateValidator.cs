using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.Song;

public class SongUpdateValidator : AbstractValidator<SongUpdateDTO>
{
    public SongUpdateValidator()
    {
        RuleFor(song => song.UserId)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(song => song.Artist)
            .NotEmpty().WithMessage("Artist is required")
            .MaximumLength(100).WithMessage("Artist must not exceed 100 characters");

        RuleFor(song => song.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(song => song.Genre)
            .NotEmpty().WithMessage("Genre is required")
            .Must(BeAValidGenre).WithMessage("Genre must be one of the predefined values: Pop, Rock, Jazz, Classical, Hip-Hop");

        RuleFor(song => song.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than 0");
    }

    private bool BeAValidGenre(string? genre)
    {
        var allowedGenres = new[] { "Pop", "Rock", "Jazz", "Classical", "Hip-Hop" };
        return genre != null && allowedGenres.Contains(genre);
    }
}
