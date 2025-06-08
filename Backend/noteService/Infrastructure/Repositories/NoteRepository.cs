using Application.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly AppDbContext _context;
    public NoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddNoteWithAccessAsync(Note note, NoteList noteList)
    {
        await _context.Notes.AddAsync(note);
        await _context.NoteLists.AddAsync(noteList);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Note?> GetNoteById(Guid id) =>
        await _context.Notes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    

    public async Task<bool> UpdateNote(Note note)
    {
        _context.Notes.Update(note);
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> DeleteNote(Guid id)
    {
        var note = await GetNoteById(id);
        if (note != null)
        {
            _context.Notes.Remove(note);
            return await _context.SaveChangesAsync() > 0;
        } 
        return false;
    }

    public async Task<Note?> GetNoteByIdWithAccessAsync(Guid noteId, Guid userId)
    {
        var hasAccess = await _context.NoteLists
            .AnyAsync(nl => nl.NoteId == noteId && nl.UserId == userId);

        if (!hasAccess)
            throw new UnauthorizedAccessException("You do not have access to this note.");

        return await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == noteId);
    }
}