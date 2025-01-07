using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Domain.Entities
{
    public class Song
    {
        public Guid SongId { get; set; }
        public string? Title { get; set; }
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public float Duration { get; set; }
        public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
