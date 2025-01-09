using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Repository
{
    public interface ISongRepository : IGenericRepository<Song>
    {
        Task<IEnumerable<Song>> GetByTitleAndArtist(string? title, string? artist );
    }
}
