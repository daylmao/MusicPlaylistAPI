using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface IPlaylistService: IGenericService<PlaylistDTO,PlaylistInsertDTO,PlaylistUpdateDTO>
    {

    }
}
