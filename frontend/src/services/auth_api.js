import axios from "axios";

const api = axios.create({
  baseURL: 'http://localhost:5090/api/account',
  headers: {
    'Content-Type': 'application/json'
  }
});

export async function login({email, password}) {
  try {
    const response = await api.post('/login', {email, password});

    const {token, refreshToken }= response.data;
    
    if (!token || !refreshToken) {
      throw new Error('Token not received from server');
    }

    localStorage.setItem('token', token);
    localStorage.setItem('refreshToken', refreshToken)

    return response.data;
  } catch (error) {
    if (error.response?.data) {
      const errData = error.response.data;
      throw new Error(errData.detail || 'Login failed');
    } else {
      throw new Error('Network error or invalid response');
    }
  }
}



export async function register({name, email, password}) {
  try{
    const response = await api.post('/register', {name, email, password});

    return response.data;
  } catch (error) {
    if (error.response?.data) {
      const errData = error.response.data;
      throw new Error(errData.detail || 'Register failed');
    } else {
      throw new Error('Network error or invalid response');
    }
  }
}
  
  