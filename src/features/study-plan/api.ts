import api from '../../shared/api/client';

export const studyPlansAPI = {
  getAll: () => api.get('/api/study-plans'),

  getById: (planId: string) =>
    api.get(`/api/study-plans/${planId}`),

  create: (body: {
    subjectId: string;
    title: string;
    startDate: string;
    endDate: string;
    items: Array<{ topicId: string; scheduledDate: string; durationMinutes: number }>;
  }) => api.post('/api/study-plans', body),

  toggleItem: (planId: string, itemId: string) =>
    api.put(`/api/study-plans/${planId}/items/${itemId}`),

  delete: (planId: string) =>
    api.delete(`/api/study-plans/${planId}`),
};
