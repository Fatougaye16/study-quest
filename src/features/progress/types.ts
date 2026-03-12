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

export interface WeeklyStudyDay {
  dayLabel: string;
  date: string;
  minutes: number;
}

export interface StreakCalendar {
  year: number;
  month: number;
  studiedDays: number[];
}
