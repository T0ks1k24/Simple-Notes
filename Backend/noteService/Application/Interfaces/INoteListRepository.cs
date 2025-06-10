using Domain.Entities;

namespace Application.Interfaces;

public interface INoteListRepository
{
    Task AddAsync(NoteList noteList);
    Task<List<Note>> GetNoteByUserId(Guid userId);
    Task DeleteNote(Guid noteId, Guid userId);
}