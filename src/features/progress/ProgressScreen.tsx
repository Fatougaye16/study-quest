import React, { useState, useCallback, useRef } from 'react';
import { View, Text, StyleSheet, ScrollView, RefreshControl, Animated } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { useAuth } from '../auth/context';
import { progressAPI } from './api';
import { studySessionsAPI } from '../home/api';
import { OverallProgress, Achievement, WeeklyStudyDay, StreakCalendar } from './types';
import HeroCard from './components/HeroCard';
import WeeklyChart from './components/WeeklyChart';
import StreakCalendarView from './components/StreakCalendar';
import SubjectProgressList from './components/SubjectProgressList';
import AchievementGrid from './components/AchievementGrid';
import DailyQuest from '../home/components/DailyQuest';

export default function ProgressScreen() {
  const { user } = useAuth();
  const [progress, setProgress] = useState<OverallProgress | null>(null);
  const [achievements, setAchievements] = useState<Achievement[]>([]);
  const [weeklyStudy, setWeeklyStudy] = useState<WeeklyStudyDay[]>([]);
  const [streakCal, setStreakCal] = useState<StreakCalendar | null>(null);
  const [todayMinutes, setTodayMinutes] = useState(0);
  const [refreshing, setRefreshing] = useState(false);

  const [toastAch, setToastAch] = useState<Achievement | null>(null);
  const toastOpacity = useRef(new Animated.Value(0)).current;
  const prevUnlockedRef = useRef<Set<string>>(new Set());

  const loadData = useCallback(async () => {
    try {
      const [progRes, achRes, weekRes, calRes, sessionsRes] = await Promise.all([
        progressAPI.get(),
        progressAPI.getAchievements(),
        progressAPI.weekly(),
        progressAPI.streakCalendar(),
        studySessionsAPI.getAll(),
      ]);
      setProgress(progRes.data);
      setWeeklyStudy(weekRes.data);
      setStreakCal(calRes.data);

      const todayStart = new Date();
      todayStart.setHours(0, 0, 0, 0);
      const sessions = sessionsRes.data as any[];
      const todayMins = sessions
        .filter((s: any) => new Date(s.startedAt) >= todayStart)
        .reduce((sum: number, s: any) => sum + s.durationMinutes, 0);
      setTodayMinutes(todayMins);

      const newAchievements: Achievement[] = achRes.data;
      const prevSet = prevUnlockedRef.current;
      if (prevSet.size > 0) {
        const newlyUnlocked = newAchievements.find(
          a => a.isUnlocked && !prevSet.has(a.type),
        );
        if (newlyUnlocked) showToast(newlyUnlocked);
      }
      prevUnlockedRef.current = new Set(
        newAchievements.filter(a => a.isUnlocked).map(a => a.type),
      );
      setAchievements(newAchievements);
    } catch (error) {
      console.error('Failed to load progress:', error);
    }
  }, []);

  useFocusEffect(
    useCallback(() => {
      loadData();
    }, [loadData]),
  );

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  const showToast = (ach: Achievement) => {
    setToastAch(ach);
    Animated.sequence([
      Animated.timing(toastOpacity, { toValue: 1, duration: 400, useNativeDriver: true }),
      Animated.delay(3000),
      Animated.timing(toastOpacity, { toValue: 0, duration: 400, useNativeDriver: true }),
    ]).start(() => setToastAch(null));
  };

  const unlockedCount = achievements.filter(a => a.isUnlocked).length;

  return (
    <View style={{ flex: 1, backgroundColor: '#f8fafc' }}>
      {toastAch && (
        <Animated.View style={[styles.toast, { opacity: toastOpacity }]}>
          <Text style={styles.toastIcon}>{toastAch.icon}</Text>
          <View style={{ flex: 1 }}>
            <Text style={styles.toastTitle}>Achievement Unlocked!</Text>
            <Text style={styles.toastDesc}>{toastAch.title} — +{toastAch.xpReward} XP</Text>
          </View>
        </Animated.View>
      )}

      <ScrollView
        style={styles.container}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
      >
        <HeroCard progress={progress} unlockedCount={unlockedCount} totalAchievements={achievements.length} />

        <View style={styles.dailyQuestSection}>
          <Text style={styles.dailyQuestTitle}>⚡ Daily Quest</Text>
          <DailyQuest
            dailyGoal={user?.dailyGoalMinutes ?? 60}
            todayMinutes={todayMinutes}
            dailyProgress={Math.min((todayMinutes / (user?.dailyGoalMinutes ?? 60)) * 100, 100)}
            sessionActive={false}
          />
        </View>

        <WeeklyChart weeklyStudy={weeklyStudy} />
        {streakCal && <StreakCalendarView streakCal={streakCal} />}
        <SubjectProgressList subjects={progress?.subjectProgress ?? []} />
        <AchievementGrid achievements={achievements} unlockedCount={unlockedCount} />
        <View style={{ height: 32 }} />
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  toast: {
    position: 'absolute', top: 50, left: 16, right: 16, zIndex: 100,
    flexDirection: 'row', alignItems: 'center', gap: 12,
    backgroundColor: '#0c4a6e', paddingHorizontal: 16, paddingVertical: 14,
    borderRadius: 14, elevation: 8,
    shadowColor: '#000', shadowOffset: { width: 0, height: 4 }, shadowOpacity: 0.2, shadowRadius: 8,
  },
  toastIcon: { fontSize: 32 },
  toastTitle: { fontSize: 14, fontWeight: 'bold', color: '#fff' },
  toastDesc: { fontSize: 13, color: '#bae6fd', marginTop: 2 },
  dailyQuestSection: { marginHorizontal: 16, marginBottom: 16 },
  dailyQuestTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: 12, color: '#1e293b' },
});
