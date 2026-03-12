export interface TimetableEntry {
  id: string;
  subjectId: string;
  subjectName: string;
  subjectColor: string;
  dayOfWeek: number;
  startTime: string;
  endTime: string;
  location?: string;
}
