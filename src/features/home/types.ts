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
