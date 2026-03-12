import api from '../../shared/api/client';

export const progressAPI = {
  get: () => api.get('/api/progress'),
  getAchievements: () => api.get('/api/progress/achievements'),
  weekly: () => api.get('/api/progress/weekly'),
  streakCalendar: () => api.get('/api/progress/streak-calendar'),
};
