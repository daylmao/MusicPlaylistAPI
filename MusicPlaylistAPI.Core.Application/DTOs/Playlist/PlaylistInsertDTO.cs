using MusicPlaylistAPI.Core.Application.DTOs.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.Playlist
{
    public class PlaylistInsertDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
    }
}
