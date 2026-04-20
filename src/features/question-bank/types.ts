export interface PastPaper {
  id: string;
  subjectId: string;
  subjectName: string;
  year: number;
  examType: string;
  paperNumber: number;
  title: string;
  createdByStudentId: string;
  createdAt: string;
  questionCount: number;
}

export interface PastQuestion {
  id: string;
  pastPaperId: string;
  topicId?: string;
  topicName?: string;
  questionNumber: number;
  questionText: string;
  answerText?: string;
  marks?: number;
  imageUrl?: string;
  difficulty: number;
}

export interface CreatePastPaperRequest {
  subjectId: string;
  year: number;
  examType: string;
  paperNumber: number;
  title: string;
}

export interface AddQuestionsRequest {
  questions: Array<{
    topicId?: string;
    questionNumber: number;
    questionText: string;
    answerText?: string;
    marks?: number;
    difficulty: number;
  }>;
}
