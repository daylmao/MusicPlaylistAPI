
namespace MusicPlaylistAPI.Core.Domain.Entities
{
    public class Song
    {
        public Guid SongId { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        public string? Genre { get; set; } 
        public float Duration { get; set; }
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
    }
}
