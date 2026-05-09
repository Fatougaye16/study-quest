import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, RefreshControl, Image, TouchableOpacity } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useNavigation, CommonActions } from '@react-navigation/native';
import { useAuth } from '../auth/context';
import { useTheme } from '../../shared/theme';
import { progressAPI } from '../progress/api';
import { timetableAPI } from '../timetable/api';
import { enrollmentsAPI } from '../courses/api';
import { Enrollment } from '../courses/types';
import { OverallProgress } from '../progress/types';
import { TimetableEntry } from '../timetable/types';
import TodayClasses from './components/TodayClasses';
import XCard from '../../shared/components/XCard';
import XBadge from '../../shared/components/XBadge';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function HomeScreen() {
  const { user } = useAuth();
  const { theme } = useTheme();
  const navigation = useNavigation<any>();
  const [progress, setProgress] = useState<OverallProgress | null>(null);
  const [todayClasses, setTodayClasses] = useState<TimetableEntry[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [refreshing, setRefreshing] = useState(false);

  const loadData = useCallback(async () => {
    try {
      const [progressRes, timetableRes, enrollRes] = await Promise.all([
        progressAPI.get(),
        timetableAPI.getAll(),
        enrollmentsAPI.getAll(),
      ]);

      setProgress(progressRes.data);
      setEnrollments(enrollRes.data);

      const todayDow = new Date().getDay();
      const todayEntries = (timetableRes.data as TimetableEntry[]).filter(e => e.dayOfWeek === todayDow);
      setTodayClasses(todayEntries);
    } catch (error) {
      console.error('Failed to load home data:', error);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  const streak = progress?.currentStreak ?? 0;
  const firstName = user?.firstName ?? 'Student';
  const colors = theme.colors;

  return (
    <ScrollView
      style={[styles.container, { backgroundColor: colors.background }]}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
    >
      {/* Greeting + Streak */}
      <View style={[styles.greetingCard, { backgroundColor: colors.primary }]}>
        <AfricanPattern variant="header" color="#FFFFFF" />
        <View style={styles.greetingContent}>
          <View style={styles.greetingTopRow}>
            <View style={{ flex: 1 }}>
              <Text style={[styles.greeting, { fontFamily: theme.fonts.headingBold }]}>Hi, {firstName}! 👋</Text>
            </View>
            <Image
              source={require('../../../assets/xamxam.png')}
              style={styles.headerLogo}
              resizeMode="contain"
            />
          </View>
          <Text style={styles.greetingSub}>
            {streak === 0
              ? 'Start studying to build your streak!'
              : streak < 7
                ? 'Keep it going!'
                : "You're unstoppable!"}
          </Text>
          <View style={styles.streakRow}>
            <Feather name="zap" size={28} color={colors.accent} />
            <View>
              <Text style={[styles.streakNumber, { fontFamily: theme.fonts.headingBold }]}>{streak}</Text>
              <Text style={[styles.streakLabel, { fontFamily: theme.fonts.body }]}>Day Streak</Text>
            </View>
            <XBadge type="xp" value={`Lvl ${progress?.level ?? 1} · ${progress?.totalXP ?? 0} XP`} style={{ marginLeft: 'auto' }} />
          </View>
        </View>
      </View>

      {/* Today's Schedule */}
      <View style={styles.section}>
        <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.heading }]}>Today's Classes</Text>
        <TodayClasses todayClasses={todayClasses} />
      </View>

      {/* Quick Stats */}
      <View style={styles.section}>
        <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.heading }]}>Quick Stats</Text>
        <View style={styles.statsContainer}>
          <XCard variant="elevated" style={styles.statCard}>
            <Feather name="book-open" size={28} color={colors.primary} />
            <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{enrollments.length}</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Subjects</Text>
          </XCard>
          <XCard variant="elevated" style={styles.statCard}>
            <Feather name="check-circle" size={28} color={colors.success} />
            <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{progress?.totalSessions ?? 0}</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Sessions</Text>
          </XCard>
          <XCard variant="elevated" style={styles.statCard}>
            <Feather name="clock" size={28} color={colors.secondary} />
            <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{Math.round((progress?.totalStudyMinutes ?? 0) / 60)}</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Hours</Text>
          </XCard>
        </View>
      </View>

      {/* AI Study Tool */}
      <View style={styles.section}>
        <TouchableOpacity
          style={[styles.aiShortcut, { backgroundColor: colors.primary }]}
          activeOpacity={0.8}
          onPress={() => navigation.dispatch(
            CommonActions.navigate({
              name: 'Learn',
              params: {
                screen: 'AITutor',
                initial: false,
              },
            })
          )}
        >
          <View style={styles.aiShortcutIcon}>
            <Feather name="star" size={28} color={colors.accent} />
          </View>
          <View style={{ flex: 1 }}>
            <Text style={[styles.aiShortcutTitle, { fontFamily: theme.fonts.headingBold }]}>AI Study Tools</Text>
            <Text style={[styles.aiShortcutSub, { fontFamily: theme.fonts.body }]}>Summarize, Quiz, Flashcards & more</Text>
          </View>
          <Feather name="chevron-right" size={22} color="rgba(255,255,255,0.7)" />
        </TouchableOpacity>
      </View>

      <View style={{ height: 32 }} />
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  greetingCard: { margin: 16, marginBottom: 8, borderRadius: 16, overflow: 'hidden' },
  greetingContent: { padding: 20 },
  greetingTopRow: { flexDirection: 'row', alignItems: 'center', marginBottom: 2 },
  headerLogo: { width: 44, height: 44, borderRadius: 22 },
  greeting: { fontSize: 24, color: '#fff' },
  greetingSub: { fontSize: 14, color: 'rgba(255,255,255,0.8)', marginTop: 4, marginBottom: 16 },
  streakRow: { flexDirection: 'row', alignItems: 'center', gap: 12 },
  streakNumber: { fontSize: 28, color: '#fff' },
  streakLabel: { fontSize: 12, color: 'rgba(255,255,255,0.7)' },
  section: { marginHorizontal: 16, marginBottom: 24 },
  sectionTitle: { fontSize: 18, marginBottom: 12 },
  statsContainer: { flexDirection: 'row', gap: 12 },
  statCard: { flex: 1, alignItems: 'center', paddingVertical: 16 },
  statNumber: { fontSize: 24, marginTop: 8 },
  statLabel: { fontSize: 12 },
  aiShortcut: { flexDirection: 'row', alignItems: 'center', borderRadius: 16, padding: 16, gap: 14 },
  aiShortcutIcon: { width: 48, height: 48, borderRadius: 14, backgroundColor: 'rgba(255,255,255,0.15)', justifyContent: 'center', alignItems: 'center' },
  aiShortcutTitle: { fontSize: 17, color: '#fff' },
  aiShortcutSub: { fontSize: 12, color: 'rgba(255,255,255,0.8)', marginTop: 2 },
});
