using Microsoft.AspNetCore.Mvc;
using MusicPlaylistAPI.Core.Application.DTOs.Song;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using FluentValidation;

namespace MusicPlaylistAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;
        private readonly IValidator<SongInsertDTO> _insertSongValidator;
        private readonly IValidator<SongUpdateDTO> _updateSongValidator;

        public SongController(
            ISongService songService,
            IValidator<SongInsertDTO> insertSongValidator,
            IValidator<SongUpdateDTO> updateSongValidator)
        {
            _songService = songService;
            _insertSongValidator = insertSongValidator;
            _updateSongValidator = updateSongValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetAll()
        {
            var songs = await _songService.GetAllAsync();
            return Ok(songs);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SongDTO>> GetById(Guid id)
        {
            var song = await _songService.GetByIdAsync(id);
            return song == null ? NotFound() : Ok(song);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SongDTO>>> GetByTitleAndArtist([FromQuery] string? title, [FromQuery] string? artist)
        {
            if (string.IsNullOrWhiteSpace(title) && string.IsNullOrWhiteSpace(artist))
            {
                return BadRequest("At least one of 'title' or 'artist' must be provided");
            }
            var songs = await _songService.GetByTitleAndArtist(title, artist);

            if (songs == null || !songs.Any())
            {
                return NotFound("No songs found matching the provided filters.");
            }
            return Ok(songs);
        }

        [HttpPost]
        public async Task<ActionResult<SongDTO>> Create([FromBody] SongInsertDTO insertDto)
        {
            var validation = await _insertSongValidator.ValidateAsync(insertDto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var createdSong = await _songService.CreateAsync(insertDto);
            if (createdSong == null)
                return BadRequest("Error creating the song");

            return CreatedAtAction(nameof(GetById), new { id = createdSong.SongId}, createdSong);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SongDTO>> Update(Guid id, [FromBody] SongUpdateDTO updateDto)
        {
            var validation = await _updateSongValidator.ValidateAsync(updateDto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var updatedSong = await _songService.UpdateAsync(id, updateDto);
            return updatedSong == null ? NotFound() : Ok(updatedSong);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedSong = await _songService.DeleteAsync(id);
            return deletedSong == null ? NotFound() : Ok(deletedSong);
        }
    }
}
