using MusicPlaylistAPI.Core.Application.DTOs.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.Playlist
{
    public class PlaylistUpdateDTO
    {
        public Guid PlaylistId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } = null;
        public ICollection<SongDTO>? Songs { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
