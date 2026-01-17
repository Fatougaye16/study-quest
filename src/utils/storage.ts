import AsyncStorage from '@react-native-async-storage/async-storage';
import { Course, TimetableSlot, StudySession, StudyPlan, UserProfile, Achievement } from '../types';

const KEYS = {
  COURSES: '@study_quest_courses',
  TIMETABLE: '@study_quest_timetable',
  STUDY_SESSIONS: '@study_quest_sessions',
  STUDY_PLANS: '@study_quest_plans',
  USER_PROFILE: '@study_quest_profile',
};

const DEFAULT_ACHIEVEMENTS: Achievement[] = [
  { id: '1', title: 'First Steps', description: 'Create your first course', icon: '🎯', unlocked: false, xpReward: 50 },
  { id: '2', title: 'Getting Organized', description: 'Add a class to your timetable', icon: '📅', unlocked: false, xpReward: 50 },
  { id: '3', title: 'Study Planner', description: 'Create your first study plan', icon: '📚', unlocked: false, xpReward: 100 },
  { id: '4', title: 'Dedicated Student', description: 'Complete 5 study sessions', icon: '⭐', unlocked: false, xpReward: 150 },
  { id: '5', title: 'Study Marathon', description: 'Study for 10 hours total', icon: '🏃', unlocked: false, xpReward: 200 },
  { id: '6', title: 'Week Warrior', description: 'Maintain a 7-day streak', icon: '🔥', unlocked: false, xpReward: 300 },
  { id: '7', title: 'Knowledge Seeker', description: 'Complete 10 topics', icon: '🧠', unlocked: false, xpReward: 250 },
  { id: '8', title: 'Course Master', description: 'Add 5 courses', icon: '👑', unlocked: false, xpReward: 200 },
  { id: '9', title: 'Early Bird', description: 'Study before 8 AM', icon: '🌅', unlocked: false, xpReward: 100 },
  { id: '10', title: 'Night Owl', description: 'Study after 10 PM', icon: '🦉', unlocked: false, xpReward: 100 },
  { id: '11', title: 'Consistency King', description: 'Study 3 days in a row', icon: '👑', unlocked: false, xpReward: 150 },
  { id: '12', title: 'Speed Demon', description: 'Complete 5 topics in one day', icon: '⚡', unlocked: false, xpReward: 200 },
];

const DEFAULT_PROFILE: UserProfile = {
  xp: 0,
  level: 1,
  streak: 0,
  totalStudyTime: 0,
  achievements: DEFAULT_ACHIEVEMENTS,
  dailyGoal: 60,
  lastStudyDate: '',
};

// Courses
export const saveCourses = async (courses: Course[]): Promise<void> => {
  try {
    await AsyncStorage.setItem(KEYS.COURSES, JSON.stringify(courses));
  } catch (error) {
    console.error('Error saving courses:', error);
    throw error;
  }
};

export const getCourses = async (): Promise<Course[]> => {
  try {
    const data = await AsyncStorage.getItem(KEYS.COURSES);
    return data ? JSON.parse(data) : [];
  } catch (error) {
    console.error('Error getting courses:', error);
    return [];
  }
};

// Timetable
export const saveTimetable = async (slots: TimetableSlot[]): Promise<void> => {
  try {
    await AsyncStorage.setItem(KEYS.TIMETABLE, JSON.stringify(slots));
  } catch (error) {
    console.error('Error saving timetable:', error);
    throw error;
  }
};

export const getTimetable = async (): Promise<TimetableSlot[]> => {
  try {
    const data = await AsyncStorage.getItem(KEYS.TIMETABLE);
    return data ? JSON.parse(data) : [];
  } catch (error) {
    console.error('Error getting timetable:', error);
    return [];
  }
};

// Study Sessions
export const saveStudySessions = async (sessions: StudySession[]): Promise<void> => {
  try {
    await AsyncStorage.setItem(KEYS.STUDY_SESSIONS, JSON.stringify(sessions));
  } catch (error) {
    console.error('Error saving study sessions:', error);
    throw error;
  }
};

export const getStudySessions = async (): Promise<StudySession[]> => {
  try {
    const data = await AsyncStorage.getItem(KEYS.STUDY_SESSIONS);
    return data ? JSON.parse(data) : [];
  } catch (error) {
    console.error('Error getting study sessions:', error);
    return [];
  }
};

// Study Plans
export const saveStudyPlans = async (plans: StudyPlan[]): Promise<void> => {
  try {
    await AsyncStorage.setItem(KEYS.STUDY_PLANS, JSON.stringify(plans));
  } catch (error) {
    console.error('Error saving study plans:', error);
    throw error;
  }
};

export const getStudyPlans = async (): Promise<StudyPlan[]> => {
  try {
    const data = await AsyncStorage.getItem(KEYS.STUDY_PLANS);
    return data ? JSON.parse(data) : [];
  } catch (error) {
    console.error('Error getting study plans:', error);
    return [];
  }
};

// User Profile
export const saveUserProfile = async (profile: UserProfile): Promise<void> => {
  try {
    await AsyncStorage.setItem(KEYS.USER_PROFILE, JSON.stringify(profile));
  } catch (error) {
    console.error('Error saving user profile:', error);
    throw error;
  }
};

export const getUserProfile = async (): Promise<UserProfile> => {
  try {
    const data = await AsyncStorage.getItem(KEYS.USER_PROFILE);
    return data ? JSON.parse(data) : DEFAULT_PROFILE;
  } catch (error) {
    console.error('Error getting user profile:', error);
    return DEFAULT_PROFILE;
  }
};

export const addXP = async (amount: number): Promise<UserProfile> => {
  const profile = await getUserProfile();
  profile.xp += amount;
  
  // Level up calculation: level = floor(xp / 500) + 1
  const newLevel = Math.floor(profile.xp / 500) + 1;
  const leveledUp = newLevel > profile.level;
  profile.level = newLevel;
  
  await saveUserProfile(profile);
  return { ...profile, leveledUp } as any;
};

export const unlockAchievement = async (achievementId: string): Promise<boolean> => {
  const profile = await getUserProfile();
  const achievement = profile.achievements.find(a => a.id === achievementId);
  
  if (achievement && !achievement.unlocked) {
    achievement.unlocked = true;
    achievement.unlockedDate = new Date().toISOString();
    profile.xp += achievement.xpReward;
    profile.level = Math.floor(profile.xp / 500) + 1;
    await saveUserProfile(profile);
    return true;
  }
  return false;
};
