using Domain.Entities;

namespace Application.Interfaces;

public interface INoteRepository
{
    Task<Note?> GetNoteById(Guid id);
    Task<bool> UpdateNote(Note note);
    Task<bool> DeleteNote(Guid id);
    Task AddNoteWithAccessAsync(Note note, NoteList noteList);
    Task<Note?> GetNoteByIdWithAccessAsync(Guid noteId, Guid userId);
}