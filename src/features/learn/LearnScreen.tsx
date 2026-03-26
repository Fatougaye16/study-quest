import React, { useState, useEffect, useCallback, useRef } from 'react';
import { View, Text, ScrollView, TouchableOpacity, RefreshControl, SafeAreaView } from 'react-native';
import { Card } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';
import { useTheme } from '../../shared/theme';
import { enrollmentsAPI, subjectsAPI } from '../courses/api';
import { Enrollment, Topic } from '../courses/types';
import { studyPlansAPI } from '../study-plan/api';
import { studySessionsAPI } from '../home/api';
import SessionTimer from '../home/components/SessionTimer';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function LearnScreen() {
  const { theme } = useTheme();
  const colors = theme.colors;
  const navigation = useNavigation<any>();

  const FEATURES = [
    { key: 'Courses', icon: 'book' as const, color: colors.primary, label: 'My Subjects', subtitle: 'Enroll, browse topics & notes' },
    { key: 'Timetable', icon: 'calendar' as const, color: colors.accent, label: 'Timetable', subtitle: 'View & manage your schedule' },
    { key: 'StudyPlan', icon: 'list' as const, color: colors.gamification.xp, label: 'Study Plans', subtitle: 'Create & track study plans' },
    { key: 'AITutor', icon: 'star' as const, color: colors.secondary, label: 'AI Tutor', subtitle: 'Quiz, flashcards & more' },
  ];
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
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.background }}>
    <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
    <ScrollView
      style={{ flex: 1 }}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
    >
      {/* Session Timer */}
      <View style={{ marginHorizontal: 16, marginTop: 20 }}>
        <Text style={{ fontSize: 18, fontWeight: 'bold', marginBottom: 12, color: colors.text, fontFamily: theme.fonts.headingBold }}>📚 Study Session</Text>
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
      <View style={{ marginHorizontal: 16, marginTop: 20 }}>
        <Text style={{ fontSize: 18, fontWeight: 'bold', marginBottom: 12, color: colors.text, fontFamily: theme.fonts.headingBold }}>Explore</Text>
        <View style={{ flexDirection: 'row', flexWrap: 'wrap', gap: 12 }}>
          {FEATURES.map(({ key, icon, color, label }) => (
            <TouchableOpacity
              key={key}
              style={{
                width: '47%', backgroundColor: colors.card, borderRadius: 16, padding: 20,
                alignItems: 'center', elevation: 2, shadowColor: '#000',
                shadowOffset: { width: 0, height: 2 }, shadowOpacity: 0.06, shadowRadius: 6,
              }}
              activeOpacity={0.7}
              onPress={() => navigation.navigate(key)}
            >
              <View style={{ width: 56, height: 56, borderRadius: 28, justifyContent: 'center', alignItems: 'center', marginBottom: 12, backgroundColor: color + '18' }}>
                <Feather name={icon} size={28} color={color} />
              </View>
              <Text style={{ fontSize: 15, fontWeight: '700', color: colors.text, marginBottom: 4, fontFamily: theme.fonts.headingBold }}>{label}</Text>
              <Text style={{ fontSize: 12, color: colors.textSecondary, textAlign: 'center', fontFamily: theme.fonts.body }}>{getSubtitle(key)}</Text>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      <View style={{ height: 32 }} />
    </ScrollView>
    </SafeAreaView>
  );
}


