import React from 'react';
import { TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useTheme } from '../../shared/theme';
import LearnScreen from './LearnScreen';
import CoursesScreen from '../courses/CoursesScreen';
import TimetableScreen from '../timetable/TimetableScreen';
import StudyPlanScreen from '../study-plan/StudyPlanScreen';
import AITutorScreen from '../ai-tutor/AITutorScreen';
import QuestionBankScreen from '../question-bank/QuestionBankScreen';

export type LearnStackParamList = {
  LearnHub: undefined;
  Courses: undefined;
  Timetable: undefined;
  StudyPlan: undefined;
  QuestionBank: undefined;
  AITutor: {
    subjectId?: string;
    subjectName?: string;
    topicId?: string;
    topicName?: string;
    feature?: 'summarize' | 'explain' | 'flashcards' | 'quiz' | 'studyPlan';
  } | undefined;
};

const Stack = createNativeStackNavigator<LearnStackParamList>();

export default function LearnNavigator() {
  const { theme } = useTheme();
  const colors = theme.colors;

  const headerOptions = {
    headerStyle: { backgroundColor: colors.primary },
    headerTintColor: colors.card,
    headerTitleStyle: { fontWeight: 'bold' as const, fontFamily: theme.fonts.headingBold },
  };

  return (
    <Stack.Navigator>
      <Stack.Screen name="LearnHub" component={LearnScreen} options={{ headerShown: false }} />
      <Stack.Screen name="Courses" component={CoursesScreen} options={{ title: 'My Subjects', ...headerOptions }} />
      <Stack.Screen name="Timetable" component={TimetableScreen} options={{ title: 'Timetable', ...headerOptions }} />
      <Stack.Screen name="StudyPlan" component={StudyPlanScreen} options={{ title: 'Study Plans', ...headerOptions }} />
      <Stack.Screen
        name="AITutor"
        component={AITutorScreen}
        options={({ navigation }) => ({
          title: 'AI Tutor',
          ...headerOptions,
          headerLeft: () => (
            <TouchableOpacity
              onPress={() => {
                if (navigation.canGoBack()) {
                  navigation.goBack();
                } else {
                  navigation.navigate('LearnHub');
                }
              }}
              style={{ marginRight: 8 }}
            >
              <Feather name="arrow-left" size={24} color={colors.card} />
            </TouchableOpacity>
          ),
        })}
      />
      <Stack.Screen name="QuestionBank" component={QuestionBankScreen} options={{ title: 'Question Bank', ...headerOptions }} />
    </Stack.Navigator>
  );
}
