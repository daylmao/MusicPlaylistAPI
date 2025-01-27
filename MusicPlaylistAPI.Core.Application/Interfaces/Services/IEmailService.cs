using MusicPlaylistAPI.Core.Application.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDTO email);
    }
}
