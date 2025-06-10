import React, { useState } from "react";
import styles from "./buttonAddNote.module.css";
import { CreateNote } from "../../services/note_api";

export default function ButtonAddNote({ onNoteCreated }) {
  const [loading, setLoading] = useState(false);
  const [isFormVisible, setFormVisible] = useState(false);
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");

  async function handleSubmit(e) {
    e.preventDefault();
    if (!title.trim()) {
      alert("Enter the title of the note");
      return;
    }

    setLoading(true);
    try {
      const newNote = await CreateNote({ title, content });
      onNoteCreated(newNote);
      setTitle("");
      setContent("");
      setFormVisible(false);
    } catch (error) {
      alert("Unable to create note: " + error.message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className={styles.container}>
      {!isFormVisible ? (
        <button
          className={styles.addButton}
          onClick={() => setFormVisible(true)}
          title="Add a note"
        >
          +
        </button>
      ) : (
        <form className={styles.form} onSubmit={handleSubmit}>
          <input
            className={styles.input}
            type="text"
            placeholder="Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            disabled={loading}
            required
          />
          <textarea
            className={styles.textarea}
            placeholder="Description"
            value={content}
            onChange={(e) => setContent(e.target.value)}
            disabled={loading}
          />
          <div className={styles.actions}>
            <button type="submit" disabled={loading}>
              {loading ? "Added..." : "Add"}
            </button>
            <button
              type="button"
              onClick={() => setFormVisible(false)}
              disabled={loading}
            >
              Cancel
            </button>
          </div>
        </form>
      )}
    </div>
  );
}
