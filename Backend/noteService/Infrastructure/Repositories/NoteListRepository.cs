using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NoteListRepository : INoteListRepository
{
    private readonly AppDbContext _context;
    public NoteListRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(NoteList noteList)
    {
        await _context.NoteLists.AddAsync(noteList);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Note>> GetNoteByUserId(Guid userId)
    {
        var notes = await (
            from nl in _context.NoteLists
            join n in _context.Notes on nl.NoteId equals n.Id
            where nl.UserId == userId
            select n
        ).ToListAsync();

        return notes;
    }

    public async Task DeleteNote(Guid noteId, Guid userId)
    {
        var userNoteAccess = await _context.NoteLists
            .FirstOrDefaultAsync(nl => nl.NoteId == noteId && nl.UserId == userId);
        
        if(userNoteAccess == null)
            throw new UnauthorizedAccessException("You do not have access to this note.");
        
        if(userNoteAccess.AccessLevel != AccessLevel.Root)
            throw new UnauthorizedAccessException("You do not have permission to delete this note.");

        var noteListLinks = await _context.NoteLists
            .Where(nl => nl.NoteId == noteId)
            .ToListAsync();

        var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == noteId);
        
        if(note == null)
            throw new Exception("Note not found.");
        
        _context.NoteLists.RemoveRange(noteListLinks);
        _context.Notes.Remove(note);

        await _context.SaveChangesAsync();
    }
    
}