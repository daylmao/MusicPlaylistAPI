using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MusicPlaylistAPI.Core.Application.DTOs.Account.Authenticate;
using MusicPlaylistAPI.Core.Application.DTOs.Email;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Enum;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using System.Text;


namespace MusicPlaylistAPI.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;


        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        private async Task<string> EmailUri(User user, string origin)
        {
            // Genera un token único para confirmar el correo electrónico
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Codifica el token en un formato seguro para URL
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Ruta del controlador que procesará la confirmación
            var route = "User/Confirm-Email";

            // Construye la URL completa (ejemplo: https://miapp.com/User/Confirm-Email)
            var Uri = new Uri(string.Concat($"{origin}/", route));

            // Agrega los parámetros de la URL (ID de usuario y token)
            var verificationUrl = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", code);

            // Devuelve el enlace completo de confirmación
            return verificationUrl; //Resultado: https://miapp.com/User/Confirm-Email?userId=123&token=abc...

        }


        // Método privado para generar un enlace para restablecer la contraseña
        private async Task<string> ForgotPasswordUri(User user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var route = "User/Reset-Password";

            var Uri = new Uri(string.Concat($"{origin}/", route));

            var verificationUrl = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUrl; //Resultado: https://example.com/User/Confirm-Email?token=abcxyz
        }

        // Método para autenticar a un usuario con su correo y contraseña
        public async Task<AuthenticateResponseDTO> AuthenticateAsync(AuthenticateRequestDTO request)
        {
            var response = new AuthenticateResponseDTO()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No accounts registered with {request.Email}";
                return response;
            }

            // Intenta iniciar sesión con el usuario y la contraseña
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "Invalid credentials";
                return response;
            }

            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = "Account not confirmed";
                return response;
            }

            // Rellena los datos del usuario en la respuesta
            response.Id = user.Id;
            response.Username = user.UserName;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.Email = user.Email;

            // Obtiene y asigna los roles del usuario
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;

            return response;
        }

        // Método para registrar a un nuevo usuario
        public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request, string origin)
        {
            var response = new RegisterResponseDTO()
            {
                HasError = false
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            // Si el correo ya está registrado, devuelve un error
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = "Email is already registered";
                return response;
            }

            // Crea un nuevo objeto usuario con los datos proporcionados
            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            // Intenta crear el usuario con la contraseña proporcionada
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Asigna el rol básico al usuario
                await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());

                // Genera un enlace de confirmación de correo
                var verificationUri = await EmailUri(user, origin);

                await _emailService.SendEmailAsync(new EmailDTO
                {
                    To = user.Email,
                    Body = $"Please confirm your account visiting this link {verificationUri}",
                    Subject = "Confirm Register"
                });
            }
            else
            {
                // Si ocurre un error al registrar, devuelve un error
                response.HasError = true;
                response.Error = "An error occurred trying to register the user";
                return response;
            }

            return response;
        }

        // Método para confirmar el correo electrónico del usuario
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return "No account registered";
            }

            // Decodifica el token recibido
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Intenta confirmar el correo del usuario con el token
            var result = await _userManager.ConfirmEmailAsync(user, token);

            // Devuelve un mensaje dependiendo del resultado
            return result.Succeeded ? "Account confirmed successfully" : "An error occurred trying to confirm your account";
        }

        // Método para enviar un enlace de restablecimiento de contraseña
        public async Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request, string origin)
        {
            var response = new ForgotPasswordResponseDTO()
            {
                HasError = true
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = "No account registered yet";
                return response;
            }

            // Genera un enlace de restablecimiento de contraseña
            var verificationUri = await ForgotPasswordUri(user, origin);

            // Envía el enlace al correo del usuario
            await _emailService.SendEmailAsync(new EmailDTO
            {
                To = user.Email,
                Body = $"Please reset your password visiting this link {verificationUri}",
                Subject = "Reset password"
            });

            return response;
        }

        // Método para restablecer la contraseña del usuario
        public async Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request, string token)
        {
            var response = new ResetPasswordResponseDTO
            {
                HasError = false
            };


            var user = await _userManager.FindByEmailAsync(request.Email);


            if (user == null)
            {
                response.HasError = true;
                response.Error = "Account not registered";
                return response;
            }

            // Decodifica el token recibido
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Intenta restablecer la contraseña del usuario
            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "An error occurred trying to reset your password";
                return response;
            }


            return response;
        }

        // Método para cerrar la sesión del usuario actual
        public async Task SignOut()
        {
            // Llama al método de cierre de sesión del SignInManager
            await _signInManager.SignOutAsync();
        }
    }

}
