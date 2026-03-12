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
