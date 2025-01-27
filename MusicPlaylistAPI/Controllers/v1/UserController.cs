using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicPlaylistAPI.Core.Application.DTOs.User;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using FluentValidation;
using Asp.Versioning;

namespace MusicPlaylistAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserInsertDTO> _userInsertValidator;
        private readonly IValidator<UserUpdateDTO> _userUpdateValidator;

        public UserController(
            IUserService userService,
            IMapper mapper,
            IValidator<UserInsertDTO> userInsertValidator,
            IValidator<UserUpdateDTO> userUpdateValidator)
        {
            _userService = userService;
            _mapper = mapper;
            _userInsertValidator = userInsertValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] UserInsertDTO userInsertDTO)
        {
            if (userInsertDTO == null)
                return BadRequest("User data is null");

            var validationResult = await _userInsertValidator.ValidateAsync(userInsertDTO);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var createdUser = await _userService.CreateAsync(userInsertDTO);
            if (createdUser == null)
                return BadRequest("User could not be created");

            return CreatedAtAction(nameof(GetById), new { id = createdUser.UserId }, createdUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (userUpdateDTO == null)
                return BadRequest("User data is null");

            var validationResult = await _userUpdateValidator.ValidateAsync(userUpdateDTO);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var updatedUser = await _userService.UpdateAsync(id, userUpdateDTO);
            if (updatedUser == null)
                return NotFound($"User not found");

            return Ok(updatedUser);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound($"User not found");

            return Ok(user);
        }
    }
}
