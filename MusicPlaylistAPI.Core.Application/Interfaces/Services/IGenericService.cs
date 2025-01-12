using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface IGenericService<T, TI, TU> where T : class
        where TI : class
        where TU : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid Id);
        Task<T> CreateAsync(TI Insert);
        Task<T> UpdateAsync(Guid Id, TU Update);
        Task<T> DeleteAsync(Guid Id);
    }
}
