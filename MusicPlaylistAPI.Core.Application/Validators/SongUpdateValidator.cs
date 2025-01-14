using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.Song;

public class SongUpdateValidator : AbstractValidator<SongUpdateDTO>
{
    public SongUpdateValidator()
    {
        RuleFor(song => song.UserId)
            .NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(song => song.Artist)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");

        RuleFor(song => song.Title)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters");

        RuleFor(song => song.Genre)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .Must(BeAValidGenre).WithMessage("{PropertyName} must be one of the predefined values: Pop, Rock, Jazz, Classical, Hip-Hop");

        RuleFor(song => song.Duration)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }

    private bool BeAValidGenre(string? genre)
    {
        var allowedGenres = new[] { "Pop", "Rock", "Jazz", "Classical", "Hip-Hop" };
        return genre != null && allowedGenres.Contains(genre);
    }
}
