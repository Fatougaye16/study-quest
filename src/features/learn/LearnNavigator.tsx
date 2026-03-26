import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useTheme } from '../../shared/theme';
import LearnScreen from './LearnScreen';
import CoursesScreen from '../courses/CoursesScreen';
import TimetableScreen from '../timetable/TimetableScreen';
import StudyPlanScreen from '../study-plan/StudyPlanScreen';
import AITutorScreen from '../ai-tutor/AITutorScreen';

const Stack = createNativeStackNavigator();

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
      <Stack.Screen name="AITutor" component={AITutorScreen} options={{ title: 'AI Tutor', ...headerOptions }} />
    </Stack.Navigator>
  );
}
