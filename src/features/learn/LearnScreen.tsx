import React, { useState, useEffect, useCallback, useRef } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity, RefreshControl, SafeAreaView } from 'react-native';
import { Card } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';
import { enrollmentsAPI, subjectsAPI } from '../courses/api';
import { Enrollment, Topic } from '../courses/types';
import { studyPlansAPI } from '../study-plan/api';
import { studySessionsAPI } from '../home/api';
import SessionTimer from '../home/components/SessionTimer';

const FEATURES = [
  { key: 'Courses', icon: 'book', color: '#0ea5e9', label: 'My Subjects', subtitle: 'Enroll, browse topics & notes' },
  { key: 'Timetable', icon: 'calendar', color: '#8b5cf6', label: 'Timetable', subtitle: 'View & manage your schedule' },
  { key: 'StudyPlan', icon: 'list', color: '#10b981', label: 'Study Plans', subtitle: 'Create & track study plans' },
  { key: 'AITutor', icon: 'sparkles', color: '#f59e0b', label: 'AI Tutor', subtitle: 'Quiz, flashcards & more' },
] as const;

export default function LearnScreen() {
  const navigation = useNavigation<any>();
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [planCount, setPlanCount] = useState(0);
  const [refreshing, setRefreshing] = useState(false);

  // Session timer state
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

  const loadData = useCallback(async () => {
    try {
      const [enrRes, plansRes] = await Promise.all([
        enrollmentsAPI.getAll(),
        studyPlansAPI.getAll(),
      ]);
      setEnrollments(enrRes.data);
      setPlanCount(plansRes.data.length);
    } catch (error) {
      console.error('Failed to load learn data:', error);
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
    setSessionStartTime(new Date());
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
    } catch (error) {
      console.error('Failed to save session:', error);
    }
  };

  const selectedSubject = enrollments.find(e => e.subjectId === sessionSubjectId);
  const selectedTopic = topics.find(t => t.id === sessionTopicId);

  const getSubtitle = (key: string) => {
    if (key === 'Courses') return `${enrollments.length} subject${enrollments.length !== 1 ? 's' : ''} enrolled`;
    if (key === 'StudyPlan') return `${planCount} active plan${planCount !== 1 ? 's' : ''}`;
    return FEATURES.find(f => f.key === key)?.subtitle ?? '';
  };

  return (
    <SafeAreaView style={styles.safe}>
    <ScrollView
      style={styles.container}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
    >
      {/* Session Timer */}
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

      {/* Feature Cards */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Explore</Text>
        <View style={styles.grid}>
          {FEATURES.map(({ key, icon, color, label }) => (
            <TouchableOpacity
              key={key}
              style={styles.featureCard}
              activeOpacity={0.7}
              onPress={() => navigation.navigate(key)}
            >
              <View style={[styles.iconCircle, { backgroundColor: color + '18' }]}>
                <Ionicons name={icon as any} size={28} color={color} />
              </View>
              <Text style={styles.featureLabel}>{label}</Text>
              <Text style={styles.featureSubtitle}>{getSubtitle(key)}</Text>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      <View style={{ height: 32 }} />
    </ScrollView>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  safe: { flex: 1, backgroundColor: '#f8fafc' },
  container: { flex: 1 },
  section: { marginHorizontal: 16, marginTop: 20 },
  sectionTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: 12, color: '#1e293b' },
  grid: { flexDirection: 'row', flexWrap: 'wrap', gap: 12 },
  featureCard: {
    width: '47%',
    backgroundColor: '#fff',
    borderRadius: 16,
    padding: 20,
    alignItems: 'center',
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.06,
    shadowRadius: 6,
  },
  iconCircle: {
    width: 56,
    height: 56,
    borderRadius: 28,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 12,
  },
  featureLabel: { fontSize: 15, fontWeight: '700', color: '#1e293b', marginBottom: 4 },
  featureSubtitle: { fontSize: 12, color: '#64748b', textAlign: 'center' },
});
