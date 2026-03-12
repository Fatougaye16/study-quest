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
