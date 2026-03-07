// ─── Auth ────────────────────────────────────
export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  student: StudentResponse;
}

export interface StudentResponse {
  id: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  grade: number;
  dailyGoalMinutes: number;
  isOtpEnabled: boolean;
  createdAt: string;
}

export interface LoginOtpRequiredResponse {
  otpRequired: boolean;
  message: string;
  phoneNumber: string;
}

// ─── Subjects ────────────────────────────────
export interface Subject {
  id: string;
  name: string;
  grade: number;
  description: string;
  color: string;
  topicCount: number;
}

export interface Topic {
  id: string;
  subjectId: string;
  name: string;
  order: number;
  description: string;
  noteCount: number;
  questionCount: number;
}

export interface Note {
  id: string;
  topicId: string;
  title: string;
  content: string;
  isAIGenerated: boolean;
  createdAt: string;
}

export interface Question {
  id: string;
  topicId: string;
  questionText: string;
  answerText: string;
  difficulty: number;
  isAIGenerated: boolean;
}

// ─── Enrollments ─────────────────────────────
export interface Enrollment {
  id: string;
  subjectId: string;
  subjectName: string;
  subjectColor: string;
  grade: number;
  enrolledAt: string;
}

// ─── Timetable ───────────────────────────────
export interface TimetableEntry {
  id: string;
  subjectId: string;
  subjectName: string;
  subjectColor: string;
  dayOfWeek: number; // 0=Sunday … 6=Saturday
  startTime: string; // "HH:mm:ss"
  endTime: string;
  location?: string;
}

// ─── Study Plans ─────────────────────────────
export interface StudyPlan {
  id: string;
  subjectId: string;
  subjectName: string;
  title: string;
  startDate: string;
  endDate: string;
  isAIGenerated: boolean;
  createdAt: string;
  items: StudyPlanItem[];
  completionPercentage: number;
}

export interface StudyPlanItem {
  id: string;
  topicId: string;
  topicName: string;
  scheduledDate: string;
  durationMinutes: number;
  isCompleted: boolean;
  completedAt?: string;
}

// ─── Study Sessions ──────────────────────────
export interface StudySession {
  id: string;
  subjectId: string;
  subjectName: string;
  topicId?: string;
  topicName?: string;
  startedAt: string;
  endedAt?: string;
  durationMinutes: number;
  notes?: string;
}

// ─── Progress ────────────────────────────────
export interface OverallProgress {
  totalXP: number;
  level: number;
  currentStreak: number;
  totalStudyMinutes: number;
  totalSessions: number;
  subjectsEnrolled: number;
  subjectProgress: SubjectProgress[];
}

export interface SubjectProgress {
  subjectId: string;
  subjectName: string;
  subjectColor: string;
  xp: number;
  level: number;
  streak: number;
  totalStudyMinutes: number;
  completedTopics: number;
  totalTopics: number;
  completionPercentage: number;
}

export interface Achievement {
  type: string;
  title: string;
  description: string;
  icon: string;
  xpReward: number;
  isUnlocked: boolean;
  unlockedAt?: string;
}

// ─── Reminders ───────────────────────────────
export interface Reminder {
  id: string;
  title: string;
  message: string;
  scheduledAt: string;
  sentAt?: string;
  type: string;
  isRecurring: boolean;
}

// ─── AI ──────────────────────────────────────
export interface SummarizeResponse {
  summary: string;
  keyPoints: string[];
}

export interface FlashcardItem {
  front: string;
  back: string;
}

export interface QuizQuestionItem {
  question: string;
  options: string[];
  correctAnswer: string;
  explanation: string;
}

export interface ExplainResponse {
  explanation: string;
  examples: string[];
  keyTakeaways: string[];
}

// ─── Navigation ──────────────────────────────
export type RootStackParamList = {
  Login: undefined;
  Register: undefined;
  OtpVerify: { phoneNumber: string };
  Main: undefined;
};
