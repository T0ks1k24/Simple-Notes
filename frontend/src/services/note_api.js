import axios from "axios";

const api = axios.create({
  baseURL: `http://localhost:5083/api`,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export async function GetNote() {
  try {
    const { data } = await api.get(`/note`);
    return data;
  } catch (error) {
    const errMsg =
      error.response?.data?.detail || error.message || "Failed to fetch notes";
    throw new Error(errMsg);
  }
}

export async function GetNoteById(id) {
  try {
    const { data } = await api.get(`/note/${id}`);
    return data;
  } catch (error) {
    const errMsg =
      error.response?.data?.detail || error.message || "Failed to fetch notes";
    throw new Error(errMsg);
  }
}

export async function CreateNote({title, content}) {
  try {
    const { data } = await api.post(`/note`, {title, content});
    return data;
  } catch (error) {
    const errMsg =
      error.response?.data?.detail || error.message || "Failed to fetch notes";
    throw new Error(errMsg);
  }
}

export async function DeleteNote(id) {
  try {
    const { data } = await api.delete(`/note`,  { params: { id } });
    return data;
  } catch (error) {
    const errMsg =
      error.response?.data?.detail || error.message || "Failed to fetch notes";
    throw new Error(errMsg);
  }
}