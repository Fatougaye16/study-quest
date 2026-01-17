export interface Course {
  id: string;
  name: string;
  code: string;
  credits: number;
  instructor?: string;
  files: CourseFile[];
  color: string;
}

export interface CourseFile {
  id: string;
  name: string;
  uri: string;
  size: number;
  uploadDate: string;
}

export interface TimetableSlot {
  id: string;
  courseId: string;
  courseName: string;
  day: string;
  startTime: string;
  endTime: string;
  location?: string;
  color: string;
}

export interface StudySession {
  id: string;
  courseId: string;
  courseName: string;
  topic: string;
  duration: number; // in minutes
  completed: boolean;
  date: string;
  notes?: string;
}

export interface StudyPlan {
  id: string;
  courseId: string;
  courseName: string;
  topics: StudyTopic[];
  startDate: string;
  endDate: string;
}

export interface StudyTopic {
  id: string;
  name: string;
  duration: number;
  completed: boolean;
  scheduledDate?: string;
  studiedDate?: string;
}

export interface Progress {
  courseId: string;
  courseName: string;
  completedSessions: number;
  totalSessions: number;
  hoursStudied: number;
  streak: number;
}

export interface UserProfile {
  xp: number;
  level: number;
  streak: number;
  totalStudyTime: number;
  achievements: Achievement[];
  dailyGoal: number;
  lastStudyDate: string;
}

export interface Achievement {
  id: string;
  title: string;
  description: string;
  icon: string;
  unlocked: boolean;
  unlockedDate?: string;
  xpReward: number;
}
