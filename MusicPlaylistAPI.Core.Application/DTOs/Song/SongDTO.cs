

namespace MusicPlaylistAPI.Core.Application.DTOs.Song
{
    public class SongDTO
    {
        public Guid SongId { get; set; }
        public string? Artist { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public float Duration { get; set; }
    }
}
