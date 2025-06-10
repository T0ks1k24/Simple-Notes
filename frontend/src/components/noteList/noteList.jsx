import React, { useState } from "react";
import Note from "../note/note";
import styles from "./noteList.module.css";
import { DeleteNote } from "../../services/note_api";
import ButtonAddNote from "../../components/buttonAddNote/buttonAddNote";

export default function NoteList({ initialNotes }) {
  const [notes, setNotes] = useState(initialNotes);

  async function handleDelete(id) {
    try {
      await DeleteNote(id);
      setNotes((prevNotes) => prevNotes.filter((note) => note.id !== id));
    } catch (error) {
      alert("Error when deleting a note");
    }
  }

  async function handleNoteCreated(newNote) {
    setNotes((prev) => [newNote, ...prev]);
    window.location.reload();
  }

  return (
    <div className={styles.noteList}>
      <ButtonAddNote onNoteCreated={handleNoteCreated} />

      {notes
        .filter((note) => note.id !== undefined && note.id !== null)
        .map((note) => (
          <Note
            key={note.id}
            note={note}
            onDelete={handleDelete}
            onUpdate={() => {}}
          />
        ))}
    </div>
  );
}