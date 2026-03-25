import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, RefreshControl } from 'react-native';
import { Card } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { useAuth } from '../auth/context';
import { progressAPI } from '../progress/api';
import { timetableAPI } from '../timetable/api';
import { enrollmentsAPI } from '../courses/api';
import { Enrollment } from '../courses/types';
import { OverallProgress } from '../progress/types';
import { TimetableEntry } from '../timetable/types';
import TodayClasses from './components/TodayClasses';

export default function HomeScreen() {
  const { user } = useAuth();
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

  return (
    <ScrollView
      style={styles.container}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
    >
      {/* Greeting + Streak */}
      <Card style={styles.greetingCard}>
        <Card.Content>
          <Text style={styles.greeting}>Hi, {firstName}! 👋</Text>
          <Text style={styles.greetingSub}>
            {streak === 0
              ? 'Start studying to build your streak!'
              : streak < 7
                ? 'Keep it going! 🔥'
                : "You're unstoppable! 🏆"}
          </Text>
          <View style={styles.streakRow}>
            <Ionicons name="flame" size={32} color="#ff6b35" />
            <View>
              <Text style={styles.streakNumber}>{streak}</Text>
              <Text style={styles.streakLabel}>Day Streak</Text>
            </View>
            <View style={styles.xpBadge}>
              <Ionicons name="star" size={16} color="#fbbf24" />
              <Text style={styles.xpBadgeText}>Level {progress?.level ?? 1} · {progress?.totalXP ?? 0} XP</Text>
            </View>
          </View>
        </Card.Content>
      </Card>

      {/* Today's Schedule */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Today's Classes</Text>
        <TodayClasses todayClasses={todayClasses} />
      </View>

      {/* Quick Stats */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Quick Stats</Text>
        <View style={styles.statsContainer}>
          <Card style={styles.statCard}>
            <Card.Content style={styles.statContent}>
              <Ionicons name="book" size={32} color="#0284c7" />
              <Text style={styles.statNumber}>{enrollments.length}</Text>
              <Text style={styles.statLabel}>Subjects</Text>
            </Card.Content>
          </Card>
          <Card style={styles.statCard}>
            <Card.Content style={styles.statContent}>
              <Ionicons name="checkmark-circle" size={32} color="#10b981" />
              <Text style={styles.statNumber}>{progress?.totalSessions ?? 0}</Text>
              <Text style={styles.statLabel}>Sessions</Text>
            </Card.Content>
          </Card>
          <Card style={styles.statCard}>
            <Card.Content style={styles.statContent}>
              <Ionicons name="time" size={32} color="#8b5cf6" />
              <Text style={styles.statNumber}>{Math.round((progress?.totalStudyMinutes ?? 0) / 60)}</Text>
              <Text style={styles.statLabel}>Hours</Text>
            </Card.Content>
          </Card>
        </View>
      </View>

      <View style={{ height: 32 }} />
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  greetingCard: { margin: 16, marginBottom: 8, backgroundColor: '#0ea5e9' },
  greeting: { fontSize: 24, fontWeight: 'bold', color: '#fff' },
  greetingSub: { fontSize: 14, color: '#e0f2fe', marginTop: 4, marginBottom: 16 },
  streakRow: { flexDirection: 'row', alignItems: 'center', gap: 12 },
  streakNumber: { fontSize: 28, fontWeight: 'bold', color: '#fff' },
  streakLabel: { fontSize: 12, color: '#e2e8f0' },
  xpBadge: { marginLeft: 'auto', flexDirection: 'row', alignItems: 'center', gap: 6, backgroundColor: 'rgba(255,255,255,0.2)', paddingHorizontal: 12, paddingVertical: 6, borderRadius: 16 },
  xpBadgeText: { color: '#fff', fontSize: 12, fontWeight: '600' },
  section: { marginHorizontal: 16, marginBottom: 24 },
  sectionTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: 12, color: '#1e293b' },
  statsContainer: { flexDirection: 'row', gap: 12 },
  statCard: { flex: 1 },
  statContent: { alignItems: 'center' },
  statNumber: { fontSize: 24, fontWeight: 'bold', color: '#1e293b', marginTop: 8 },
  statLabel: { fontSize: 12, color: '#64748b' },
});
