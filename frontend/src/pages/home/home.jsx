import React from "react";
import styles from "./home.module.css";
// import App from '../../App';
import NoteList from "../../components/noteList/noteList";

const initialNotes = [
  {
    id: 1,
    title: "First note",
    content: "Hello world!",
    updatedAt: new Date().toISOString(),
  },
  {
    id: 2,
    title: "Second note",
    content: "More text here.",
    updatedAt: new Date().toISOString(),
  },
  {
    id: 3,
    title: "Second note",
    content: "More text here.",
    updatedAt: new Date().toISOString(),
  },
  {
    id: 4,
    title: "Second note",
    content: "More text here.",
    updatedAt: new Date().toISOString(),
  },
  {
    id: 5,
    title: "Second note",
    content: "More text here.",
    updatedAt: new Date().toISOString(),
  },
];

export default function Home() {
  return (
    <main className={styles.container}>
      <h1 className={styles.title}>My Notes</h1>
      <NoteList initialNotes={initialNotes} />
    </main>
  );
}
