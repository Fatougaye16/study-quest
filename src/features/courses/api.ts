import api from '../../shared/api/client';

export const subjectsAPI = {
  getAll: (grade?: number) =>
    api.get('/api/subjects', { params: grade ? { grade } : undefined }),

  getTopics: (subjectId: string) =>
    api.get(`/api/subjects/${subjectId}/topics`),

  getNotes: (topicId: string) =>
    api.get(`/api/subjects/topics/${topicId}/notes`),

  createNote: (topicId: string, title: string, content: string) =>
    api.post(`/api/subjects/topics/${topicId}/notes`, { title, content }),

  getQuestions: (topicId: string) =>
    api.get(`/api/subjects/topics/${topicId}/questions`),

  createQuestion: (topicId: string, questionText: string, answerText: string, difficulty: number) =>
    api.post(`/api/subjects/topics/${topicId}/questions`, { questionText, answerText, difficulty }),
};

export const enrollmentsAPI = {
  getAll: () => api.get('/api/enrollments'),

  enroll: (subjectId: string) =>
    api.post('/api/enrollments', { subjectId }),

  unenroll: (enrollmentId: string) =>
    api.delete(`/api/enrollments/${enrollmentId}`),
};
