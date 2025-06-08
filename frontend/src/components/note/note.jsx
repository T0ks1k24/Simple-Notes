import React, { useState } from 'react';
import styles from './note.module.css';

export default function Note({ note, onUpdate }) {
  const [isEditing, setIsEditing] = useState(false);
  const [title, setTitle] = useState(note.title);
  const [content, setContent] = useState(note.content);

  const handleSave = () => {
    const updatedNote = {
      ...note,
      title: title.trim(),
      content: content.trim(),
      updatedAt: new Date().toISOString(),
    };
    onUpdate(updatedNote);
    setIsEditing(false);
  };

  const handleCancel = () => {
    setTitle(note.title);
    setContent(note.content);
    setIsEditing(false);
  };

  return (
    <div className={styles.note}>
      <input
        className={styles.titleInput}
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Title"
        disabled={!isEditing}
      />
      <textarea
        className={styles.contentInput}
        value={content}
        onChange={(e) => setContent(e.target.value)}
        placeholder="Content"
        disabled={!isEditing}
      />
      <small className={styles.updatedAt}>
        Updated: {new Date(note.updatedAt).toLocaleString()}
      </small>
      <div className={styles.actions}>
        {isEditing ? (
          <>
            <button onClick={handleSave} className={styles.saveBtn}>Save</button>
            <button onClick={handleCancel} className={styles.cancelBtn}>Cancel</button>
          </>
        ) : (
          <button
            onClick={() => setIsEditing(true)}
            className={styles.editBtn}
            title="Edit note"
          >
            Edit
          </button>
        )}
      </div>
    </div>
  );
}
