import React, { useState, useEffect, useCallback, useRef } from 'react';
import { View, Text, StyleSheet, ScrollView, RefreshControl, TouchableOpacity } from 'react-native';
import { Card, ProgressBar } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';
import { useAuth } from '../auth/context';
import { progressAPI } from '../progress/api';
import { timetableAPI } from '../timetable/api';
import { studySessionsAPI } from './api';
import { StudySession } from './types';
import { enrollmentsAPI, subjectsAPI } from '../courses/api';
import { Enrollment, Topic } from '../courses/types';
import { OverallProgress } from '../progress/types';
import { TimetableEntry } from '../timetable/types';
import SessionTimer from './components/SessionTimer';
import DailyQuest from './components/DailyQuest';
import TodayClasses from './components/TodayClasses';
import RecentSessions from './components/RecentSessions';

export default function HomeScreen() {
  const navigation = useNavigation<any>();
  const { user } = useAuth();
  const [progress, setProgress] = useState<OverallProgress | null>(null);
  const [todayClasses, setTodayClasses] = useState<TimetableEntry[]>([]);
  const [recentSessions, setRecentSessions] = useState<StudySession[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [refreshing, setRefreshing] = useState(false);

  const [sessionActive, setSessionActive] = useState(false);
  const [sessionSubjectId, setSessionSubjectId] = useState<string | null>(null);
  const [sessionTopicId, setSessionTopicId] = useState<string | null>(null);
  const [sessionStartTime, setSessionStartTime] = useState<Date | null>(null);
  const [elapsedSeconds, setElapsedSeconds] = useState(0);
  const [topics, setTopics] = useState<Topic[]>([]);
  const [showSubjectPicker, setShowSubjectPicker] = useState(false);
  const [showTopicPicker, setShowTopicPicker] = useState(false);
  const [sessionNotes, setSessionNotes] = useState('');
  const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

  const [todayMinutes, setTodayMinutes] = useState(0);

  const loadData = useCallback(async () => {
    try {
      const [progressRes, timetableRes, sessionsRes, enrollRes] = await Promise.all([
        progressAPI.get(),
        timetableAPI.getAll(),
        studySessionsAPI.getAll(),
        enrollmentsAPI.getAll(),
      ]);

      setProgress(progressRes.data);
      setEnrollments(enrollRes.data);

      const todayDow = new Date().getDay();
      const todayEntries = (timetableRes.data as TimetableEntry[]).filter(e => e.dayOfWeek === todayDow);
      setTodayClasses(todayEntries);

      const sessions = sessionsRes.data as StudySession[];
      setRecentSessions(sessions.slice(0, 5));

      const todayStart = new Date();
      todayStart.setHours(0, 0, 0, 0);
      const todayMins = sessions
        .filter(s => new Date(s.startedAt) >= todayStart)
        .reduce((sum, s) => sum + s.durationMinutes, 0);
      setTodayMinutes(todayMins);
    } catch (error) {
      console.error('Failed to load home data:', error);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  useEffect(() => {
    if (sessionActive && sessionStartTime) {
      timerRef.current = setInterval(() => {
        setElapsedSeconds(Math.floor((Date.now() - sessionStartTime.getTime()) / 1000));
      }, 1000);
    }
    return () => {
      if (timerRef.current) clearInterval(timerRef.current);
    };
  }, [sessionActive, sessionStartTime]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  const onSelectSubject = async (subjectId: string) => {
    setSessionSubjectId(subjectId);
    setSessionTopicId(null);
    setShowSubjectPicker(false);
    try {
      const res = await subjectsAPI.getTopics(subjectId);
      setTopics(res.data as Topic[]);
    } catch { setTopics([]); }
  };

  const startSession = () => {
    if (!sessionSubjectId) return;
    const now = new Date();
    setSessionStartTime(now);
    setElapsedSeconds(0);
    setSessionActive(true);
  };

  const stopSession = async () => {
    if (!sessionStartTime || !sessionSubjectId) return;
    if (timerRef.current) clearInterval(timerRef.current);
    setSessionActive(false);

    const endedAt = new Date();
    const durationMinutes = Math.max(1, Math.round((endedAt.getTime() - sessionStartTime.getTime()) / 60000));

    try {
      await studySessionsAPI.create({
        subjectId: sessionSubjectId,
        topicId: sessionTopicId ?? undefined,
        startedAt: sessionStartTime.toISOString(),
        endedAt: endedAt.toISOString(),
        durationMinutes,
        notes: sessionNotes || undefined,
      });
      setSessionSubjectId(null);
      setSessionTopicId(null);
      setSessionStartTime(null);
      setElapsedSeconds(0);
      setSessionNotes('');
      setTopics([]);
      await loadData();
    } catch (error) {
      console.error('Failed to save session:', error);
    }
  };

  const level = progress?.level ?? 1;
  const totalXP = progress?.totalXP ?? 0;
  const streak = progress?.currentStreak ?? 0;
  const dailyGoal = user?.dailyGoalMinutes ?? 60;
  const dailyProgress = Math.min((todayMinutes / dailyGoal) * 100, 100);
  const selectedSubject = enrollments.find(e => e.subjectId === sessionSubjectId);
  const selectedTopic = topics.find(t => t.id === sessionTopicId);

  const getLevelProgress = () => {
    const currentLevelXP = (level - 1) * 500;
    const nextLevelXP = level * 500;
    return ((totalXP - currentLevelXP) / (nextLevelXP - currentLevelXP)) * 100;
  };

  return (
    <ScrollView
      style={styles.container}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
    >
      <Card style={styles.levelCard}>
        <Card.Content>
          <View style={styles.levelHeader}>
            <View>
              <Text style={styles.levelText}>Level {level}</Text>
              <Text style={styles.xpText}>{totalXP} XP</Text>
            </View>
            <Text style={styles.levelIcon}>🎮</Text>
          </View>
          <ProgressBar progress={getLevelProgress() / 100} color="#fbbf24" style={styles.xpBar} />
          <Text style={styles.xpNextLevel}>
            {500 - (totalXP % 500)} XP to Level {level + 1}
          </Text>
        </Card.Content>
      </Card>

      <Card style={styles.streakCard}>
        <Card.Content style={styles.streakContent}>
          <View style={styles.streakLeft}>
            <Ionicons name="flame" size={36} color="#ff6b35" />
            <View>
              <Text style={styles.streakNumber}>{streak}</Text>
              <Text style={styles.streakLabel}>Day Streak</Text>
            </View>
          </View>
          <View style={styles.streakRight}>
            <Text style={styles.streakMotivation}>
              {streak === 0
                ? 'Start studying to build your streak!'
                : streak < 7
                  ? 'Keep it going! 🔥'
                  : 'Unstoppable! 🏆'}
            </Text>
            <View style={styles.statBadge}>
              <Ionicons name="trophy" size={16} color="#fbbf24" />
              <Text style={styles.badgeText}>{enrollments.length} subjects</Text>
            </View>
          </View>
        </Card.Content>
      </Card>

      <View style={styles.quickAccessRow}>
        <TouchableOpacity style={styles.quickAccessBtn} onPress={() => navigation.navigate('Timetable')}>
          <Ionicons name="calendar-outline" size={22} color="#0ea5e9" />
          <Text style={styles.quickAccessText}>Timetable</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.quickAccessBtn} onPress={() => navigation.navigate('AITutor')}>
          <Ionicons name="sparkles-outline" size={22} color="#0ea5e9" />
          <Text style={styles.quickAccessText}>AI Tutor</Text>
        </TouchableOpacity>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>📚 Study Session</Text>
        <SessionTimer
          sessionActive={sessionActive}
          sessionSubjectId={sessionSubjectId}
          selectedSubject={selectedSubject}
          selectedTopic={selectedTopic}
          elapsedSeconds={elapsedSeconds}
          topics={topics}
          enrollments={enrollments}
          showSubjectPicker={showSubjectPicker}
          showTopicPicker={showTopicPicker}
          onSetShowSubjectPicker={setShowSubjectPicker}
          onSetShowTopicPicker={setShowTopicPicker}
          onSelectSubject={onSelectSubject}
          onSelectTopic={(id) => { setSessionTopicId(id); setShowTopicPicker(false); }}
          onStart={startSession}
          onStop={stopSession}
        />
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>⚡ Daily Quest</Text>
        <DailyQuest
          dailyGoal={dailyGoal}
          todayMinutes={todayMinutes}
          dailyProgress={dailyProgress}
          sessionActive={sessionActive}
        />
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Today's Classes</Text>
        <TodayClasses todayClasses={todayClasses} />
      </View>

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
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Recent Study Sessions</Text>
        <RecentSessions recentSessions={recentSessions} />
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  levelCard: { margin: 16, marginBottom: 8, backgroundColor: '#0c4a6e' },
  levelHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 },
  levelText: { fontSize: 24, fontWeight: 'bold', color: '#fff' },
  xpText: { fontSize: 14, color: '#94a3b8', marginTop: 4 },
  levelIcon: { fontSize: 48 },
  xpBar: { height: 8, borderRadius: 4, backgroundColor: '#0284c7' },
  xpNextLevel: { fontSize: 12, color: '#94a3b8', marginTop: 8, textAlign: 'center' },
  streakCard: { marginHorizontal: 16, marginBottom: 8, backgroundColor: '#0ea5e9' },
  streakContent: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  streakLeft: { flexDirection: 'row', alignItems: 'center', gap: 12 },
  streakNumber: { fontSize: 28, fontWeight: 'bold', color: '#fff' },
  streakLabel: { fontSize: 12, color: '#e2e8f0' },
  streakRight: { alignItems: 'flex-end', gap: 8 },
  streakMotivation: { fontSize: 14, fontWeight: '600', color: '#fff' },
  statBadge: { flexDirection: 'row', alignItems: 'center', backgroundColor: 'rgba(255,255,255,0.2)', paddingHorizontal: 12, paddingVertical: 6, borderRadius: 16, gap: 6 },
  badgeText: { color: '#fff', fontSize: 12, fontWeight: '600' },
  quickAccessRow: { flexDirection: 'row', marginHorizontal: 16, marginBottom: 8, gap: 12 },
  quickAccessBtn: { flex: 1, flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 8, backgroundColor: '#fff', paddingVertical: 14, borderRadius: 12, elevation: 1, shadowColor: '#000', shadowOffset: { width: 0, height: 1 }, shadowOpacity: 0.05, shadowRadius: 2 },
  quickAccessText: { fontSize: 14, fontWeight: '600', color: '#334155' },
  section: { marginHorizontal: 16, marginBottom: 24 },
  sectionTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: 12, color: '#1e293b' },
  statsContainer: { flexDirection: 'row', gap: 12 },
  statCard: { flex: 1 },
  statContent: { alignItems: 'center' },
  statNumber: { fontSize: 24, fontWeight: 'bold', color: '#1e293b', marginTop: 8 },
  statLabel: { fontSize: 12, color: '#64748b' },
});
