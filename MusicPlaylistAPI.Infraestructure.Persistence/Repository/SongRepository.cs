using Microsoft.EntityFrameworkCore;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Domain.Entities;
using MusicPlaylistAPI.Infraestructure.Persistence.Context;


namespace MusicPlaylistAPI.Infraestructure.Persistence.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly PlaylistContext _context;

        public SongRepository(PlaylistContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Song>> GetAllAsync() => await _context.Songs.ToListAsync();

        public async Task<Song> GetByIdAsync(int id) => await _context.Songs.FindAsync(id);

        public async Task<IEnumerable<Song>> GetByTitleAndArtist(string? title = null, string? artist = null)
        {
            return await _context.Songs
                .Where(b => (title == null || b.Title == title) && (artist == null || b.Artist == artist)).ToListAsync();
        }
        public async Task InsertAsync(Song entity)
        {
            await _context.Songs.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Song entity)
        {
            _context.Songs.Attach(entity);
            _context.Songs.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }
        public async Task DeleteAsync(Song entity)
        {
            _context.Songs.Remove(entity);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
