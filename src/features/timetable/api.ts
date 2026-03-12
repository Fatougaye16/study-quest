import api from '../../shared/api/client';

export const timetableAPI = {
  getAll: () => api.get('/api/timetable'),

  create: (body: {
    subjectId: string;
    dayOfWeek: number;
    startTime: string;
    endTime: string;
    location?: string;
  }) => api.post('/api/timetable', body),

  update: (entryId: string, body: {
    subjectId: string;
    dayOfWeek: number;
    startTime: string;
    endTime: string;
    location?: string;
  }) => api.put(`/api/timetable/${entryId}`, body),

  delete: (entryId: string) =>
    api.delete(`/api/timetable/${entryId}`),
};
