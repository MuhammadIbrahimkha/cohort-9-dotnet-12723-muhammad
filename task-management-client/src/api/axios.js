import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7xxx/api', // replace with your actual API port
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;