using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public ICollection<Playlist>? Playlists { get; set; }
    }
}
