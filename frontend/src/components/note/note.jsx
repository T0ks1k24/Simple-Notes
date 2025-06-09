import React, { useState } from 'react';
import styles from './note.module.css';


export default function Note({ note, onDelete }) {
  const [title, setTitle] = useState(note.title);
  const [content, setContent] = useState(note.content);

  return (
    <div className={styles.note}>
      <input
        className={styles.titleInput}
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Title"
        disabled={true}
      />
      <textarea
        className={styles.contentInput}
        value={content}
        onChange={(e) => setContent(e.target.value)}
        placeholder="Content"
        disabled={true} 
      />
      <small className={styles.updatedAt}>
        Updated: {new Date(note.updatedAt).toLocaleString()}
      </small>
      <div className={styles.actions}>
        <button
          onClick={() => onDelete(note.id)}
          className={styles.deleteBtn}
          title="Delete note"
        >
          Delete
        </button>
      </div>
    </div>
  );
}
