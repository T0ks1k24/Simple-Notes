import {StrictMode} from 'react';
import {createRoot} from 'react-dom/client';
import './index.css';
import App from './App';
import Register from './pages/register/register';
import Login from './pages/login/login';
import Home from './pages/home/home';
import {createBrowserRouter, RouterProvider} from "react-router-dom";

const router = createBrowserRouter([
  {path: "/", element: <Home/>},
  {path: "/App", element: <App/>},
  {path: "/register", element: <Register/>},
  {path: "/login", element: <Login/>}
])

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);


