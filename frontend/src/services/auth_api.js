import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5090/api/Account",
  headers: { "Content-Type": "application/json" },
  timeout: 10000,
});

function extractErrorMessage(error) {
  if (error.response?.data) {
    return error.response.data.detail || JSON.stringify(error.response.data);
  } else if (error.message) {
    return error.message;
  }
  return "Unknown error";
}

export async function login({ email, password }) {
  try {
    const response = await api.post("/login", { email, password });
    const { accesstoken: accessToken, refreshToken } = response.data;

    if (!accessToken || !refreshToken) {
      throw new Error("Token not received from server");
    }

    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
    return response.data;
  } catch (error) {
    throw new Error(extractErrorMessage(error));
  }
}

export async function register({ name, email, password }) {
  try {
    const response = await api.post("/register", { name, email, password });
    return response.data;
  } catch (error) {
    throw new Error(extractErrorMessage(error));
  }
}
