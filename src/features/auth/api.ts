import api from '../../shared/api/client';

export const authAPI = {
  register: (body: {
    phoneNumber: string;
    password: string;
    firstName: string;
    lastName: string;
    grade?: number;
    enableOtp?: boolean;
  }) => api.post('/api/auth/register', body),

  login: (phoneNumber: string, password: string) =>
    api.post('/api/auth/login', { phoneNumber, password }),

  requestOtp: (phoneNumber: string) =>
    api.post('/api/auth/request-otp', { phoneNumber }),

  verifyOtp: (phoneNumber: string, otpCode: string) =>
    api.post('/api/auth/verify-otp', { phoneNumber, otpCode }),

  refresh: (refreshToken: string) =>
    api.post('/api/auth/refresh', { refreshToken }),

  logout: () => api.post('/api/auth/logout'),
};
