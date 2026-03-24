import api from '../../shared/api/client';
import { DocumentPickerAsset } from 'expo-document-picker';

export const subjectsAPI = {
  getAll: (grade?: number) =>
    api.get('/api/subjects', { params: grade ? { grade } : undefined }),

  getTopics: (subjectId: string) =>
    api.get(`/api/subjects/${subjectId}/topics`),

  getNotes: (topicId: string) =>
    api.get(`/api/subjects/topics/${topicId}/notes`),

  createNote: (topicId: string, title: string, content: string) =>
    api.post(`/api/subjects/topics/${topicId}/notes`, { title, content }),

  uploadFile: (topicId: string, file: DocumentPickerAsset) => {
    const formData = new FormData();
    formData.append('file', {
      uri: file.uri,
      name: file.name,
      type: file.mimeType || 'application/octet-stream',
    } as any);
    return api.post(`/api/subjects/topics/${topicId}/upload`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
      timeout: 120000,
    });
  },

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
