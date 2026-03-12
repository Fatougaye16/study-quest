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

export interface Enrollment {
  id: string;
  subjectId: string;
  subjectName: string;
  subjectColor: string;
  grade: number;
  enrolledAt: string;
}
