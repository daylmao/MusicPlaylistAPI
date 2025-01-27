using AutoMapper;
using MusicPlaylistAPI.Core.Application.DTOs.Email;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Application.Interfaces.Repository;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlaylistAPI.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
            var found = await _userRepository.GetByIdAsync(id);
            if (found == null)  return null;

            return _mapper.Map<UserDTO>(found);
        }
        public async Task<UserDTO> CreateAsync(UserInsertDTO insert)
        {
            var newInfo = _mapper.Map<User>(insert);
            if (newInfo == null) return null;

            newInfo.CreateAt = DateTime.UtcNow;
            await _userRepository.CreateAsync(newInfo);
            await _emailService.SendEmailAsync(new EmailDTO
            {
                To = insert.Email,
                Subject = "Welcome to our platform",
                Body = $@"
                   <div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px; padding: 20px; background-color: #f9f9f9;'>
                        <h1 style='color: #007BFF; font-size: 24px; margin-bottom: 20px; text-align: center;'>User Created Successfully</h1>
                        <p style='font-size: 16px; margin-bottom: 20px;'>We’re pleased to inform you that a new user has been added to your system. Here are the details:</p>
                        <div style='font-size: 16px; background-color: #fff; padding: 15px; border: 1px solid #ddd; border-radius: 5px; margin-bottom: 20px;'>
                            <strong>User Name:</strong> <span style='color: #555;'>{insert.Name} {insert.LastName}</span>
                        </div>
                        <p style='font-size: 14px; margin-bottom: 20px;'>
                            If you did not initiate this action or have any concerns, please <a href='mailto:aspnetpruebas@gmail.com' style='color: #007BFF;'>contact our support team</a>.
                        </p>
                        <hr style='border: none; border-top: 1px solid #e0e0e0; margin: 20px 0;'>
                        <p style='font-size: 12px; color: #888; text-align: center;'>This is an automated email. Please do not reply to this message.</p>
                    </div>"
        });
            return _mapper.Map<UserDTO>(newInfo);
        }

        public async Task<UserDTO> UpdateAsync(Guid id, UserUpdateDTO update)
        {
            var found = await _userRepository.GetByIdAsync(id);
            if (found == null) return null;

            var updateInfo = _mapper.Map(update,found);
            await _userRepository.UpdateAsync(updateInfo);
            return _mapper.Map<UserDTO>(updateInfo);
        }
    }
}
