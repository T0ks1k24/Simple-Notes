using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Interfaces;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;
    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetNoteByUserId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        if (userId == null)
            return Unauthorized();

        var notes = await _noteService.GetNoteByUserIdAsync(Guid.Parse(userId));
        return Ok(notes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        
        if (userId == null)
            return Unauthorized();
        
        var note = await _noteService.GetNoteById(id ,Guid.Parse(userId));
        if (note == null)
            return Forbid("You do not have access to this note.");
        return Ok(note);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] NoteAddDto note)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;

        if (userId == null)
            return Unauthorized();
        
        await _noteService.CreateNoteAsync(note, Guid.Parse(userId));
        return Ok(); 
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteNote(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        
        if (userId == null)
            return Unauthorized();

        await _noteService.DeleteNote(id, Guid.Parse(userId));
        return Ok("Note Delete");
    }

    
    
    // [HttpPatch]
    // public async Task<IActionResult> UpdateNote([FromBody] NoteUpdateDto note)
    // {
    //     
    // }
}