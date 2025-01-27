using MusicPlaylistAPI.Core.Application.DTOs.Account.Authenticate;

namespace MusicPlaylistAPI.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AuthenticateResponseDTO> AuthenticateAsync(AuthenticateRequestDTO request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request, string origin);
        Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request, string origin);
        Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request, string token);
        Task SignOut();
    }
}