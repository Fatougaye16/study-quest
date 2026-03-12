import api from '../../shared/api/client';

export const aiAPI = {
  summarize: (topicId: string, content?: string, grade?: number) =>
    api.post('/api/ai/summarize', { topicId, content, grade }),

  flashcards: (topicId: string, content?: string, count?: number) =>
    api.post('/api/ai/flashcards', { topicId, content, count }),

  quiz: (topicId: string, difficulty?: number, questionCount?: number) =>
    api.post('/api/ai/quiz', { topicId, difficulty, questionCount }),

  explain: (topicId: string, specificQuestion?: string, grade?: number) =>
    api.post('/api/ai/explain', { topicId, specificQuestion, grade }),

  studyPlan: (subjectId: string, topicIds?: string[], durationDays?: number) =>
    api.post('/api/ai/study-plan', { subjectId, topicIds, durationDays }),
};
