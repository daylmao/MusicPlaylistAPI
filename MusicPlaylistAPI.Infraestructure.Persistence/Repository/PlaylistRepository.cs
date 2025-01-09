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

        public PlaylistRepository(PlaylistContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Playlist>> GetAllAsync() => await _context.Playlists.ToListAsync();

        public async Task<Playlist> GetByIdAsync(int id) => await _context.Playlists.FindAsync(id);

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
