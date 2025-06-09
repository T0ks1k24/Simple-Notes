using System.Collections.Immutable;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Models;

namespace Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly INoteListRepository _noteListRepository;

    public NoteService(INoteRepository noteRepository, INoteListRepository noteListRepository)
    {
        _noteRepository = noteRepository;
        _noteListRepository = noteListRepository;
    }
    
    public async Task CreateNoteAsync(NoteAddDto noteDto, Guid userId)
    {
        var newNote = new Note
        {
            Id = Guid.NewGuid(),
            Title = noteDto.Title,
            Content = noteDto.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var newListNote = new NoteList
        {
            Note = newNote,
            UserId = userId,
            AccessLevel = AccessLevel.Root
        };
        
        await _noteRepository.AddNoteWithAccessAsync(newNote, newListNote);
    }

    public async Task<List<NoteDto>> GetNoteByUserIdAsync(Guid userId)
    {
        var notes = await _noteListRepository.GetNoteByUserId(userId);

        var noteDtos = notes.Select(note => new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            UpdatedAt = note.UpdatedAt
        }).ToList();
        
        return noteDtos;
    }

    public async Task UpdateNote(Guid id, NoteUpdateDto noteUpdateDto)
    {
        var noteToUpdate = await _noteRepository.GetNoteById(id);

        if (noteToUpdate == null)
            throw new Exception("Error UpdateNote Method NoteService");
    
        if (noteUpdateDto.Title != null)
            noteToUpdate.Title = noteUpdateDto.Title;
        if (noteUpdateDto.Content != null)
            noteToUpdate.Content = noteUpdateDto.Content;
    
        noteToUpdate.UpdatedAt = DateTime.UtcNow;
        
        await _noteRepository.UpdateNote(noteToUpdate);
    }
    
    public async Task DeleteNote(Guid noteId, Guid userId)
    {
        if (noteId == null || userId == null)
            throw new Exception("Error DeleteNote NoteService");
        
        await _noteListRepository.DeleteNote(noteId, userId);
    }

    public async Task<NoteDto?> GetNoteById(Guid noteId, Guid userId)
    {
        var note = await _noteRepository.GetNoteByIdWithAccessAsync(noteId, userId);
        if (note == null)
            throw new Exception("Error GetNoteById NoteService");

        var noteDto = new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            UpdatedAt = note.UpdatedAt
        };

        return noteDto;
    }
}
