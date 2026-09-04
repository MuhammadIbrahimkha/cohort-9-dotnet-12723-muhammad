import api from './axios';
export const getUsers = () => api.get('/users');
export const getMe = () => api.get('/users/me');