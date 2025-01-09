using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Repository
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<IEnumerable<Entity>> GetAllAsync();
        Task<Entity> GetByIdAsync(int id);
        Task InsertAsync(Entity entity);
        Task UpdateAsync(Entity entity);
        Task DeleteAsync(Entity entity);
        Task SaveChangesAsync();

    }
}
