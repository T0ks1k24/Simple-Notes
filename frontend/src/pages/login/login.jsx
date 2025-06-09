import React, { useState } from "react";
import styles from "./login.module.css";
import { login } from "../../services/auth_api";
import { useNavigate } from 'react-router-dom';


export default function Login({ onLoginSuccess }) {
  const [form, setForm] = useState({ email: "", password: "" });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const data = await login(form);

      if(onLoginSuccess){
        onLoginSuccess(data)
      }

      // // Save localStorage
      // localStorage.setItem("accessToken", data.token);
      // localStorage.setItem("refreshToken", data.refreshToken);
      navigate("/");
    } catch (err) {
      setError(err.message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.wrapper}>
      <div className={styles.card}>
        <h2 className={styles.title}>Welcome back</h2>

        {error && <p className={styles.error}>{error}</p>}

        <form className={styles.form} onSubmit={handleSubmit}>
          <input
            type="email"
            name="email"
            placeholder="Email"
            className={styles.input}
            value={form.email}
            onChange={handleChange}
            required
            disabled={loading}
            autoComplete="username"
          />
          <input
            type="password"
            name="password"
            placeholder="Password"
            className={styles.input}
            value={form.password}
            onChange={handleChange}
            required
            disabled={loading}
            autoComplete="current-password"
          />
          <button
            type="submit"
            className={styles.button}
            disabled={loading}
          >
            {loading ? "Logging in..." : "Login"}
          </button>
        </form>

        <button
          onClick={() => navigate("/register")}
          className={styles.linkButton}
          disabled={loading}
        >
          Don’t have an account? Register
        </button>
      </div>
    </div>
  );
}
