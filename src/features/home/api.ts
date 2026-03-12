import api from '../../shared/api/client';

export const studySessionsAPI = {
  getAll: (params?: { subjectId?: string; from?: string; to?: string }) =>
    api.get('/api/study-sessions', { params }),

  create: (body: {
    subjectId: string;
    topicId?: string;
    startedAt: string;
    endedAt?: string;
    durationMinutes: number;
    notes?: string;
  }) => api.post('/api/study-sessions', body),
};
