const api = axios.create({
  baseURL: 'http://localhost:5090/api/note',
  headers: {
    'Content-Type': 'application/json'
  }
});