using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Domain.Entities
{
    public class Playlist
    {
        public Guid PlaylistId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
