using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.DTOs.User
{
    public class UserInsertDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
