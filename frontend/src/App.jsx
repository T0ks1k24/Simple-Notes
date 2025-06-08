import React, { useState, useEffect } from 'react';
import Login from './pages/login/login';
import Register from './pages/register/register';
import Home from './pages/home/home';

export default function App() {
  const [page, setPage] = useState('login');

  useEffect(() => {
    const token = localStorage.getItem('accessToken')
    if (token){
      setPage('home')
    }
  }, []);

  const onLoginSuccess = () => {
    setPage('home');
  }

  const onLogout = () => {
    localStorage.removeItem('accessToken')
    setPage('login')
  }

  if ((page === 'login' || page === 'register') && localStorage.getItem('accessToken')) {
    setPage('home');
  }


  return (
    <div>
      {page === 'login' && <Login onLoginSuccess={onLoginSuccess} />}
      {page === 'register' && <Register onRegisterSuccess={() => setPage('login')} />}
      {page === 'home' && <Home onLogout={onLogout} />}
    </div>
  );
}
