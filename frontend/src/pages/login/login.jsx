import React, { useState } from "react";
import styles from "./login.module.css";
import { login } from "../../services/auth_api";
import { useNavigate } from 'react-router-dom';


export default function Login({ onLoginSuccess }) {
  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      const data = await login(form);

      // Save localStorage
      localStorage.setItem("accessToken", data.token);
      localStorage.setItem("refreshToken", data.token);
      setSuccess("Login successful!");
      onLoginSuccess(data.token);
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className={styles.wrapper}>
      <div className={styles.card}>
        <h2 className={styles.title}>Welcome back</h2>

        {error && <p className={styles.error}>{error}</p>}
        {success && <p className={styles.success}>{success}</p>}

        <form className={styles.form} onSubmit={handleSubmit}>
          <input
            type="email"
            name="email"
            placeholder="Email"
            className={styles.input}
            value={form.email}
            onChange={handleChange}
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            className={styles.input}
            value={form.password}
            onChange={handleChange}
            required
          />
          <button type="submit" className={styles.button}>
            Login
          </button>
        </form>
        <button
          onClick={() => navigate("/register")}
          className={styles.linkButton}
        >
          Don’t have an account? Register
        </button>
      </div>
    </div>
  );
}
