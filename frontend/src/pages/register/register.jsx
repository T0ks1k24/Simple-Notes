import React, { useState } from "react";
import styles from "./register.module.css";
import { register, login } from "../../services/auth_api";
import { useNavigate } from 'react-router-dom';

export default function Register() {
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    const name = e.target.name.value.trim();
    const email = e.target.name.value.trim();
    const password = e.target.password.value;

    try {
      await register({ name, email, password });
      await login({ email, password });
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className={styles.wrapper}>
      <div className={styles.card}>
        <h2 className={styles.title}>Create your account</h2>
        <form className={styles.form} onSubmit={handleSubmit}>
          <input
            type="text"
            name="name"
            placeholder="Username"
            className={styles.input}
            required
          />
          <input
            type="email"
            name="email"
            placeholder="Email"
            className={styles.input}
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            className={styles.input}
            required
          />
          {error && <div className={styles.error}>{error}</div>}
          <button type="submit" className={styles.button}>
            Register
          </button>
          
        </form>
        <button
  onClick={() => navigate("/login")}
  className={styles.linkButton}
>
  Already have an account? Login
</button>

      </div>
    </div>
  );
}
