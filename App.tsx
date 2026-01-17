import React, { useState, useEffect } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { Provider as PaperProvider } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import SplashScreen from './src/components/SplashScreen';
import HomeScreen from './src/screens/HomeScreen';
import TimetableScreen from './src/screens/TimetableScreen';
import CoursesScreen from './src/screens/CoursesScreen';
import StudyPlanScreen from './src/screens/StudyPlanScreen';
import ProgressScreen from './src/screens/ProgressScreen';

const Tab = createBottomTabNavigator();

export default function App() {
  const [showSplash, setShowSplash] = useState(true);

  useEffect(() => {
    setTimeout(() => {
      setShowSplash(false);
    }, 3000);
  }, []);

  if (showSplash) {
    return <SplashScreen />;
  }

  return (
    <PaperProvider>
      <NavigationContainer>
        <Tab.Navigator
          screenOptions={({ route }) => ({
            tabBarIcon: ({ focused, color, size }) => {
              let iconName: keyof typeof Ionicons.glyphMap = 'home';

              if (route.name === 'Home') {
                iconName = focused ? 'home' : 'home-outline';
              } else if (route.name === 'Timetable') {
                iconName = focused ? 'calendar' : 'calendar-outline';
              } else if (route.name === 'Courses') {
                iconName = focused ? 'book' : 'book-outline';
              } else if (route.name === 'Study Plan') {
                iconName = focused ? 'list' : 'list-outline';
              } else if (route.name === 'Progress') {
                iconName = focused ? 'stats-chart' : 'stats-chart-outline';
              }

              return <Ionicons name={iconName} size={size} color={color} />;
            },
            tabBarActiveTintColor: '#8b5cf6',
            tabBarInactiveTintColor: 'gray',
            headerStyle: {
              backgroundColor: '#8b5cf6',
            },
            headerTintColor: '#fff',
            headerTitleStyle: {
              fontWeight: 'bold',
            },
          })}
        >
          <Tab.Screen name="Home" component={HomeScreen} />
          <Tab.Screen name="Timetable" component={TimetableScreen} />
          <Tab.Screen name="Courses" component={CoursesScreen} />
          <Tab.Screen name="Study Plan" component={StudyPlanScreen} />
          <Tab.Screen name="Progress" component={ProgressScreen} />
        </Tab.Navigator>
      </NavigationContainer>
    </PaperProvider>
  );
}
