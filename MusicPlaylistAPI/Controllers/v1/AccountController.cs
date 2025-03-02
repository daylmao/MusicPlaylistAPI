using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using FluentValidation;
using Asp.Versioning;
using Org.BouncyCastle.Crypto.Operators;
using MusicPlaylistAPI.Core.Application.DTOs.Account.Authenticate;
using Microsoft.AspNetCore.Identity.Data;

namespace MusicPlaylistAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequestDTO request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDTO registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest(new { error = "El cuerpo de la solicitud no puede estar vacío" });
            }

            var origin = Request.Headers["origin"];
            var result = await _accountService.RegisterAsync(registerRequest, origin);

            if (result.HasError)
            {
                return BadRequest(new { error = result.Error });
            }

            return Ok(result);
        }


        [HttpGet("confirm-email")]
        public async Task<IActionResult> RegisterAsync([FromQuery] string userId, [FromQuery] string token)
        {
            return Ok(await _accountService.ConfirmAccountAsync(userId, token));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody]ForgotPasswordRequestDTO request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ForgotPasswordAsync(request, origin));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request));
        }
    }
}
