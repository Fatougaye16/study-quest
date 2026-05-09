import api from '../../shared/api/client';
import { CreatePastPaperRequest, AddQuestionsRequest } from './types';

export const questionBankAPI = {
  getPapers: (params?: { subjectId?: string; year?: number; examType?: string }) =>
    api.get('/api/question-bank/papers', { params }),

  getQuestions: (paperId: string, params?: { topicId?: string; difficulty?: number }) =>
    api.get(`/api/question-bank/papers/${paperId}/questions`, { params }),

  createPaper: (body: CreatePastPaperRequest) =>
    api.post('/api/question-bank/papers', body),

  addQuestions: (paperId: string, body: AddQuestionsRequest) =>
    api.post(`/api/question-bank/papers/${paperId}/questions`, body),

  deletePaper: (paperId: string) =>
    api.delete(`/api/question-bank/papers/${paperId}`),
};
