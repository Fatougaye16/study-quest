import api from '../api/client';

export const downloadsAPI = {
  pastPaper: (paperId: string) =>
    api.get(`/api/downloads/past-paper/${paperId}`, { responseType: 'arraybuffer' }),

  notes: (topicId: string) =>
    api.get(`/api/downloads/notes/${topicId}`, { responseType: 'arraybuffer' }),

  flashcards: (topicId: string) =>
    api.get(`/api/downloads/flashcards/${topicId}`, { responseType: 'arraybuffer' }),

  studyPlan: (planId: string) =>
    api.get(`/api/downloads/study-plan/${planId}`, { responseType: 'arraybuffer' }),
};
