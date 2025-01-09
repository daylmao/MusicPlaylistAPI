using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.Song
{
    public class SongInsertDTO
    {
        public string? Artist { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public float Duration { get; set; }
    }
}
