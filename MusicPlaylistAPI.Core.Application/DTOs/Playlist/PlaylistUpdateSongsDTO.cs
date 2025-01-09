using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.Playlist
{
    public class PlaylistUpdateSongsDTO
    {
        public ICollection<Guid>? Songs { get; set; }
    }
}
