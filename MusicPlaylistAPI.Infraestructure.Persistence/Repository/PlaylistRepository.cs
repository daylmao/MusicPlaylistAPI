using Microsoft.EntityFrameworkCore;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Domain.Entities;
using MusicPlaylistAPI.Infraestructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Infraestructure.Persistence.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly PlaylistContext _context;
        private readonly ISongRepository _songRepository;

        public PlaylistRepository(PlaylistContext context, ISongRepository songRepository)
        {
            _context = context;
            _songRepository = songRepository;
        }

        public async Task<IEnumerable<Playlist>> GetAllAsync() => await _context.Playlists.AsNoTracking().ToListAsync();

        public async Task<Playlist> GetByIdAsync(Guid id)
        {
            return await _context.Playlists
            .Include(p => p.Songs) 
            .FirstOrDefaultAsync(p => p.PlaylistId == id);
        }

        public async Task<Playlist> UpdatePlaylistSongs(Guid playlistId, List<Guid> songIds)
        {
            var playlist = await GetByIdAsync(playlistId);

            if (playlist == null)
            {
                return null; 
            }

            var songs = await _songRepository.GetByIdsAsync(songIds);

            var existingSongIds = new HashSet<Guid>(playlist.Songs.Select(p => p.SongId));

            var newSongs = songs.Where(song => !existingSongIds.Contains(song.SongId)).ToList();

            if (newSongs.Any())
            {
                playlist.Songs.AddRange(newSongs);
            }

            await SaveChangesAsync();

            return playlist;
        }

        public async Task InsertAsync(Playlist entity)
        {
            await _context.Playlists.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Playlist entity)
        {
            _context.Playlists.Attach(entity);
            _context.Playlists.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }
        public async Task DeleteAsync(Playlist entity)
        {
            _context.Playlists.Remove(entity);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
