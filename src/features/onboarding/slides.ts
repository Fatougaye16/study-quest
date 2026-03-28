export interface OnboardingSlide {
  id: string;
  title: string;
  subtitle: string;
  illustrationKey: 'welcome' | 'timetable' | 'courses' | 'studyPlan' | 'aiTutor' | 'progress' | 'letsGo';
}

export const slides: OnboardingSlide[] = [
  {
    id: 'welcome',
    title: 'Welcome to XamXam!',
    subtitle: 'Your personal study companion built to help you learn smarter, not harder.',
    illustrationKey: 'welcome',
  },
  {
    id: 'timetable',
    title: 'Organize Your Week',
    subtitle: 'Set up your class timetable so you never miss a lesson. XamXam keeps your schedule in one place.',
    illustrationKey: 'timetable',
  },
  {
    id: 'courses',
    title: 'Explore Your Subjects',
    subtitle: 'Enroll in subjects, browse topics, and upload your own notes and study materials.',
    illustrationKey: 'courses',
  },
  {
    id: 'studyPlan',
    title: 'Plan Your Learning',
    subtitle: 'Generate smart study plans that break big goals into daily tasks you can actually finish.',
    illustrationKey: 'studyPlan',
  },
  {
    id: 'aiTutor',
    title: 'Meet Your AI Tutor',
    subtitle: 'Get instant summaries, quizzes, and flashcards powered by AI — like having a tutor in your pocket.',
    illustrationKey: 'aiTutor',
  },
  {
    id: 'progress',
    title: 'Track Your Growth',
    subtitle: 'Earn XP, build streaks, and unlock achievements as you study. Watch yourself level up!',
    illustrationKey: 'progress',
  },
  {
    id: 'letsGo',
    title: "Let's Go!",
    subtitle: 'You are all set. Start your learning adventure and make every study session count.',
    illustrationKey: 'letsGo',
  },
];
