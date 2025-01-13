using Microsoft.AspNetCore.Mvc;
using MusicPlaylistAPI.Core.Application.DTOs.Playlist;
using MusicPlaylistAPI.Core.Application.Interfaces.Services;
using FluentValidation;

namespace MusicPlaylistAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly IValidator<PlaylistInsertDTO> _createPlaylistValidator;
        private readonly IValidator<PlaylistUpdateDTO> _updatePlaylistValidator;

        public PlaylistController(
            IPlaylistService playlistService,
            IValidator<PlaylistInsertDTO> createPlaylistValidator,
            IValidator<PlaylistUpdateDTO> updatePlaylistValidator)
        {
            _playlistService = playlistService;
            _createPlaylistValidator = createPlaylistValidator;
            _updatePlaylistValidator = updatePlaylistValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> GetAll()
        {
            var playlists = await _playlistService.GetAllAsync();
            return Ok(playlists);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PlaylistDTO>> GetById(Guid id)
        {
            var playlist = await _playlistService.GetByIdAsync(id);
            return playlist == null
                ? NotFound("Playlist not found")
                : Ok(playlist);
        }

        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> Create([FromBody] PlaylistInsertDTO insertDto)
        {
            var validation = await _createPlaylistValidator.ValidateAsync(insertDto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var createdPlaylist = await _playlistService.CreateAsync(insertDto);
            if (createdPlaylist == null)
                return BadRequest("Error creating playlist");

            return CreatedAtAction(nameof(GetById), new { id = createdPlaylist.PlaylistId }, createdPlaylist);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<PlaylistDTO>> Update(Guid id, [FromBody] PlaylistUpdateDTO updateDto)
        {
            var existingPlaylist = await _playlistService.GetByIdAsync(id);
            if (existingPlaylist == null)
                return NotFound("Playlist not found");

            var validation = await _updatePlaylistValidator.ValidateAsync(updateDto);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var updatedPlaylist = await _playlistService.UpdateAsync(id, updateDto);
            return updatedPlaylist == null
                ? BadRequest("Error updating playlist")
                : Ok(updatedPlaylist);
        }

        [HttpPatch("{playlistId}/songs")]
        public async Task<IActionResult> UpdatePlaylistSongs(Guid playlistId, [FromBody] List<Guid> songIds)
        {
            var updatedPlaylist = await _playlistService.UpdatePlaylistSongs(playlistId, songIds);
            if (updatedPlaylist == null)
                return NotFound("Playlist not found");

            return Ok(updatedPlaylist);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedPlaylist = await _playlistService.DeleteAsync(id);
            return deletedPlaylist == null
                ? NotFound("Playlist not found")
                : Ok(new { Message = "Playlist deleted successfully", Data = deletedPlaylist });
        }
    }
}
