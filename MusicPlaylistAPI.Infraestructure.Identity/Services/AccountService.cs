using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicPlaylistAPI.Core.Application.DTOs.Account.Authenticate;
using MusicPlaylistAPI.Core.Application.DTOs.Email;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using MusicPlaylistAPI.Core.Domain.Enum;
using MusicPlaylistAPI.Core.Domain.Settings;
using MusicPlaylistAPI.Infraestructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MusicPlaylistAPI.Infraestructure.Identity.Services
{
    public class AccountService : IAccountService
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;


        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
        }

        // Método para generar un token JWT (Json Web Token) para un usuario
        private async Task<JwtSecurityToken> GenerateToken(User user)
        {

            // Obtener los claims personalizados del usuario (información adicional como correo, etc.)
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Obtener los roles del usuario desde el UserManager (Roles como "Admin", "User", etc.)
            var roles = await _userManager.GetRolesAsync(user);

            // Lista para almacenar los claims de los roles del usuario
            var rolesClaim = new List<Claim>();

            // Crear un claim por cada rol que tiene el usuario
            foreach (var role in roles)
            {
                rolesClaim.Add(new Claim("roles", role));
            }

            // Definir los claims básicos para el token: 
            // El nombre de usuario (sub), un identificador único (jti), el correo electrónico y el ID del usuario
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Claim para el nombre de usuario
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Claim para un identificador único del token
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // Claim para el correo electrónico
                new Claim("UId", user.Id) // Claim para el ID del usuario
            }
            // Unir los claims definidos anteriormente con los claims personalizados (como roles y otros datos)
            .Union(userClaims)
            .Union(rolesClaim);

            // Crear una clave de seguridad para firmar el token usando la clave secreta (_jwtSettings.Key)
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Establecer las credenciales de firma utilizando el algoritmo HMAC SHA256 para garantizar la integridad del token
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            //Este es el token ya creado 
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, // Quién emite el token (por ejemplo, tu aplicación)
                audience: _jwtSettings.Audience, // A quién va dirigido el token
                claims: claims, // Los claims que contienen la información del usuario
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes), // Fecha de expiración del token
                signingCredentials: signingCredentials // Lapublic string? Email { get; set; }s credenciales para firmar el token
            );

            // Devolver el token JWT generado
            return jwtSecurityToken;
        }

        // Método para generar una cadena aleatoria que se usa como parte del refresh token
        private string RandomTokenString()
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            var randomBytes = new byte[40];

            // Llenar el arreglo con bytes aleatorios
            randomNumberGenerator.GetBytes(randomBytes);

            // Convertir los bytes aleatorios en una cadena hexadecimal
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        // Método para generar un refresh token (utilizado para obtener un nuevo JWT cuando el antiguo expira)
        private RefreshTokenDTO GenerateRefreshToken()
        {
            return new RefreshTokenDTO
            {
                // Generar un token aleatorio para el refresh token
                Token = RandomTokenString(),

                // Establecer la fecha de expiración del refresh token (7 días)
                Expires = DateTime.UtcNow.AddDays(7),

                // Establecer la fecha en que se crea el refresh token
                Created = DateTime.UtcNow
            };
        }

        // Método privado para generar un enlace para confirmar el correo
        private async Task<string> EmailUri(User user, string origin) 
        {
            //Origin es el localhost desde donde se ejectua el API, eso se obtiene desde el controller, porque puede variar
            // Genera un token único para confirmar el correo electrónico
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Codifica el token en un formato seguro para URL
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Ruta del controlador que procesará la confirmación
            var route = "User/Confirm-Email";

            // Construye la URL completa (ejemplo: https://miapp.com/User/Confirm-Email)
            var Uri = new Uri(string.Concat($"{origin}/", route));

            // Agrega los parámetros de la URL (ID de usuario y token)
            var verificationUrl = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", token);

            // Devuelve el enlace completo de confirmación
            return verificationUrl; //Resultado: https://miapp.com/User/Confirm-Email?userId=123&token=abc...

        }


        // Método privado para generar un enlace para restablecer la contraseña
        private async Task<string> ForgotPasswordUri(User user, string origin)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var route = "User/Reset-Password";

            var Uri = new Uri(string.Concat($"{origin}/", route));

            var verificationUrl = QueryHelpers.AddQueryString(Uri.ToString(), "token", token);

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

            //if (!user.EmailConfirmed)
            //{
            //    response.HasError = true;
            //    response.Error = "Account not confirmed";
            //    return response;
            //}

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

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
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

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
                UserName = request.Email,
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
        public async Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request)
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
            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

            // Intenta restablecer la contraseña del usuario
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

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
