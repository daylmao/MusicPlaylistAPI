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
    public class UserRepository : IUserRepository
    {
        private readonly PlaylistContext _playlistContext;

        public UserRepository(PlaylistContext playlistContext)
        {
            _playlistContext = playlistContext;
        }
        public async Task<User> GetByIdAsync(Guid id) => await _playlistContext.Users.FindAsync(id);
        public async Task CreateAsync(User entity)
        {
            await _playlistContext.Users.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(User entity)
        {
            _playlistContext.Users.Attach(entity);
            _playlistContext.Users.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }
        public async Task DeleteAsync(User entity)
        {
            _playlistContext.Users.Remove(entity);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync() => await _playlistContext.SaveChangesAsync();

    }
}
