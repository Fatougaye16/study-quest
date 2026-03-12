import api from '../../shared/api/client';

export const profileAPI = {
  get: () => api.get('/api/profile'),

  update: (body: {
    firstName?: string;
    lastName?: string;
    grade?: number;
    dailyGoalMinutes?: number;
  }) => api.put('/api/profile', body),

  registerDevice: (token: string, platform: string) =>
    api.post('/api/profile/device-token', { token, platform }),
};
