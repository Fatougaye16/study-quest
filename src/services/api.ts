import axios, { AxiosInstance, InternalAxiosRequestConfig, AxiosResponse } from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';

// ── API Base URL ─────────────────────────────────────────────────────
// Production: your Fly.io deployment URL
// Development: your machine's local IP (run `ipconfig` to check)
const PROD_URL = 'https://study-quest-api.fly.dev';
const DEV_URL = 'http://172.20.10.2:5197';

const BASE_URL = __DEV__ ? DEV_URL : PROD_URL;

const TOKEN_KEY = '@study_quest_token';
const REFRESH_KEY = '@study_quest_refresh';

// ────────────────────────────────────────────
// Axios instance
// ────────────────────────────────────────────
const api: AxiosInstance = axios.create({
  baseURL: BASE_URL,
  timeout: 15000,
  headers: { 'Content-Type': 'application/json' },
});

// ────────────────────────────────────────────
// Token helpers
// ────────────────────────────────────────────
export const setTokens = async (access: string, refresh: string) => {
  await AsyncStorage.multiSet([
    [TOKEN_KEY, access],
    [REFRESH_KEY, refresh],
  ]);
};

export const getAccessToken = () => AsyncStorage.getItem(TOKEN_KEY);
export const getRefreshToken = () => AsyncStorage.getItem(REFRESH_KEY);

export const clearTokens = async () => {
  await AsyncStorage.multiRemove([TOKEN_KEY, REFRESH_KEY]);
};

// ────────────────────────────────────────────
// Request interceptor – attach Bearer token
// ────────────────────────────────────────────
api.interceptors.request.use(async (config: InternalAxiosRequestConfig) => {
  const token = await getAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// ────────────────────────────────────────────
// Response interceptor – auto-refresh on 401
// ────────────────────────────────────────────
let isRefreshing = false;
let failedQueue: Array<{ resolve: (v: any) => void; reject: (e: any) => void }> = [];

const processQueue = (error: any, token: string | null = null) => {
  failedQueue.forEach(({ resolve, reject }) => {
    if (error) reject(error);
    else resolve(token);
  });
  failedQueue = [];
};

api.interceptors.response.use(
  (response: AxiosResponse) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        }).then((token) => {
          originalRequest.headers.Authorization = `Bearer ${token}`;
          return api(originalRequest);
        });
      }

      originalRequest._retry = true;
      isRefreshing = true;

      try {
        const refreshToken = await getRefreshToken();
        if (!refreshToken) throw new Error('No refresh token');

        const { data } = await axios.post(`${BASE_URL}/api/auth/refresh`, {
          refreshToken,
        });

        await setTokens(data.accessToken, data.refreshToken);
        processQueue(null, data.accessToken);
        originalRequest.headers.Authorization = `Bearer ${data.accessToken}`;
        return api(originalRequest);
      } catch (refreshError) {
        processQueue(refreshError, null);
        await clearTokens();
        throw refreshError;
      } finally {
        isRefreshing = false;
      }
    }

    return Promise.reject(error);
  },
);

// ════════════════════════════════════════════
// AUTH
// ════════════════════════════════════════════
export const authAPI = {
  register: (body: {
    phoneNumber: string;
    password: string;
    firstName: string;
    lastName: string;
    grade?: number;
    enableOtp?: boolean;
  }) => api.post('/api/auth/register', body),

  login: (phoneNumber: string, password: string) =>
    api.post('/api/auth/login', { phoneNumber, password }),

  requestOtp: (phoneNumber: string) =>
    api.post('/api/auth/request-otp', { phoneNumber }),

  verifyOtp: (phoneNumber: string, otpCode: string) =>
    api.post('/api/auth/verify-otp', { phoneNumber, otpCode }),

  refresh: (refreshToken: string) =>
    api.post('/api/auth/refresh', { refreshToken }),

  logout: () => api.post('/api/auth/logout'),
};

// ════════════════════════════════════════════
// PROFILE
// ════════════════════════════════════════════
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

// ════════════════════════════════════════════
// SUBJECTS
// ════════════════════════════════════════════
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

// ════════════════════════════════════════════
// ENROLLMENTS
// ════════════════════════════════════════════
export const enrollmentsAPI = {
  getAll: () => api.get('/api/enrollments'),

  enroll: (subjectId: string) =>
    api.post('/api/enrollments', { subjectId }),

  unenroll: (enrollmentId: string) =>
    api.delete(`/api/enrollments/${enrollmentId}`),
};

// ════════════════════════════════════════════
// TIMETABLE
// ════════════════════════════════════════════
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

// ════════════════════════════════════════════
// STUDY PLANS
// ════════════════════════════════════════════
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

// ════════════════════════════════════════════
// STUDY SESSIONS
// ════════════════════════════════════════════
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

// ════════════════════════════════════════════
// PROGRESS
// ════════════════════════════════════════════
export const progressAPI = {
  get: () => api.get('/api/progress'),
  getAchievements: () => api.get('/api/progress/achievements'),
};

// ════════════════════════════════════════════
// REMINDERS
// ════════════════════════════════════════════
export const remindersAPI = {
  getAll: () => api.get('/api/reminders'),

  create: (body: {
    title: string;
    message: string;
    scheduledAt: string;
    type?: string;
    isRecurring?: boolean;
  }) => api.post('/api/reminders', body),

  delete: (id: string) => api.delete(`/api/reminders/${id}`),
};

// ════════════════════════════════════════════
// AI
// ════════════════════════════════════════════
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

export default api;
