import React, { useEffect, useState } from "react";
import styles from "./home.module.css";
// import App from '../../App';
import NoteList from "../../components/noteList/noteList";
import { GetNote } from "../../services/note_api";


export default function Home() {
  const [notes, setNotes] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchNotes() {
      try {
        const data = await GetNote();
        setNotes(data);
      } catch(err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchNotes();
  }, []);

  if (loading) return <div className={styles.loading}>Loading...</div>;
  if (error) return <div className={styles.error}>Error: {error}</div>

  return (
    <main className={styles.container}>
      <h1 className={styles.title}>My Notes</h1>
      {notes.length === 0 ? <p>No notes</p> : <NoteList initialNotes={notes} />}
    </main>
  );
}
