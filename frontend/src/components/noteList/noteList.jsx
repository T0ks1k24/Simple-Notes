import React, { useState } from 'react';
import Note from '../note/note';
import styles from './noteList.module.css';

export default function NoteList({ initialNotes }) {
  const [notes, setNotes] = useState(initialNotes);

  const handleUpdate = (updatedNote) => {
    setNotes(notes.map(note => note.id === updatedNote.id ? updatedNote : note));
  };

  return (
    <div className={styles.noteList}>
      {notes.length === 0 && <p>No notes yet.</p>}
      {notes.map(note => (
        <Note key={note.id} note={note} onUpdate={handleUpdate} />
      ))}
    </div>
  );
}
