using FluentValidation;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Validators
{
    public class PlaylistUpdateSongsValidator: AbstractValidator<PlaylistUpdateSongsDTO>
    {
        public PlaylistUpdateSongsValidator()
        {
            RuleFor(x => x.PlaylistId)
                .NotEmpty().WithMessage("Playlist ID is required");

            RuleFor(x => x.Songs)
                .Must(songs => songs == null || songs.Any()).WithMessage("At least one song must be provided")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Songs)
                        .NotEmpty().WithMessage("Songs list cannot be empty");
                });
        }
    }
}
