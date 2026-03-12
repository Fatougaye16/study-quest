import React, { useState, useEffect, useCallback, useRef } from 'react';
import {
  View, Text, StyleSheet, ScrollView, Dimensions, TouchableOpacity,
  Alert, RefreshControl, Animated,
} from 'react-native';
import { Card, ProgressBar as PaperProgressBar } from 'react-native-paper';
import { useFocusEffect } from '@react-navigation/native';
import Svg, { Circle } from 'react-native-svg';
import { progressAPI } from '../services/api';
import {
  OverallProgress, SubjectProgress, Achievement, WeeklyStudyDay, StreakCalendar,
} from '../types';
import { useAuth } from '../contexts/AuthContext';

const SCREEN_W = Dimensions.get('window').width;
const RING_SIZE = 130;
const RING_STROKE = 10;
const RING_RADIUS = (RING_SIZE - RING_STROKE) / 2;
const RING_CIRCUMFERENCE = 2 * Math.PI * RING_RADIUS;

const DAY_NAMES = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
const MONTH_NAMES = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

export default function ProgressScreen() {
  const { user } = useAuth();
  const [progress, setProgress] = useState<OverallProgress | null>(null);
  const [achievements, setAchievements] = useState<Achievement[]>([]);
  const [weeklyStudy, setWeeklyStudy] = useState<WeeklyStudyDay[]>([]);
  const [streakCal, setStreakCal] = useState<StreakCalendar | null>(null);
  const [refreshing, setRefreshing] = useState(false);

  // Achievement toast
  const [toastAch, setToastAch] = useState<Achievement | null>(null);
  const toastOpacity = useRef(new Animated.Value(0)).current;
  const prevUnlockedRef = useRef<Set<string>>(new Set());

  const loadData = useCallback(async () => {
    try {
      const [progRes, achRes, weekRes, calRes] = await Promise.all([
        progressAPI.get(),
        progressAPI.getAchievements(),
        progressAPI.weekly(),
        progressAPI.streakCalendar(),
      ]);
      setProgress(progRes.data);
      setWeeklyStudy(weekRes.data);
      setStreakCal(calRes.data);

      // Detect newly unlocked achievements
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

  // Auto-refresh every time the screen is focused
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

  // ── Computed values ───────────────────────────────────
  const totalXP = progress?.totalXP ?? 0;
  const level = progress?.level ?? 1;
  const xpInLevel = totalXP % 500;
  const xpProgress = xpInLevel / 500;
  const ringOffset = RING_CIRCUMFERENCE * (1 - xpProgress);

  const totalHours = progress ? (progress.totalStudyMinutes / 60) : 0;
  const unlockedCount = achievements.filter(a => a.isUnlocked).length;
  const weekMax = Math.max(1, ...weeklyStudy.map(d => d.minutes));

  const getMotivation = () => {
    if (!progress) return "Let's get started! 🌟";
    const avg = progress.subjectProgress.length > 0
      ? progress.subjectProgress.reduce((s, p) => s + p.completionPercentage, 0) / progress.subjectProgress.length
      : 0;
    if (avg >= 75) return "You're crushing it! 🔥";
    if (avg >= 50) return 'Great progress! Keep going! 💪';
    if (avg >= 25) return "You're making moves! 🚀";
    return "Let's get started! 🌟";
  };

  const getProgressColor = (pct: number) => {
    if (pct >= 75) return '#10b981';
    if (pct >= 50) return '#f59e0b';
    if (pct >= 25) return '#3b82f6';
    return '#94a3b8';
  };

  // ── Calendar grid builder ─────────────────────────────
  const buildCalendar = () => {
    if (!streakCal) return null;
    const daysInMonth = new Date(streakCal.year, streakCal.month, 0).getDate();
    const firstDow = new Date(streakCal.year, streakCal.month - 1, 1).getDay();
    const studiedSet = new Set(streakCal.studiedDays);
    const today = new Date();
    const isCurrentMonth = today.getFullYear() === streakCal.year && today.getMonth() + 1 === streakCal.month;
    const todayDay = isCurrentMonth ? today.getDate() : -1;

    const cells: React.ReactNode[] = [];
    // Empty leading cells
    for (let i = 0; i < firstDow; i++) {
      cells.push(<View key={`e${i}`} style={styles.calCell} />);
    }
    for (let d = 1; d <= daysInMonth; d++) {
      const studied = studiedSet.has(d);
      const isToday = d === todayDay;
      cells.push(
        <View
          key={d}
          style={[
            styles.calCell,
            studied && styles.calCellStudied,
            isToday && styles.calCellToday,
          ]}
        >
          <Text style={[styles.calDay, studied && styles.calDayStudied]}>{d}</Text>
        </View>,
      );
    }
    return cells;
  };

  return (
    <View style={{ flex: 1, backgroundColor: '#f8fafc' }}>
      {/* Achievement Toast */}
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
        {/* ── Level Ring & Stats ────────────────────── */}
        <Card style={styles.heroCard}>
          <Card.Content>
            <Text style={styles.heroTitle}>🎮 Your Quest Progress</Text>
            <Text style={styles.heroMotivation}>{getMotivation()}</Text>

            <View style={styles.ringContainer}>
              <Svg width={RING_SIZE} height={RING_SIZE}>
                {/* Background ring */}
                <Circle
                  cx={RING_SIZE / 2} cy={RING_SIZE / 2} r={RING_RADIUS}
                  stroke="#e2e8f0" strokeWidth={RING_STROKE} fill="none"
                />
                {/* Progress ring */}
                <Circle
                  cx={RING_SIZE / 2} cy={RING_SIZE / 2} r={RING_RADIUS}
                  stroke="#0ea5e9" strokeWidth={RING_STROKE} fill="none"
                  strokeDasharray={`${RING_CIRCUMFERENCE}`}
                  strokeDashoffset={ringOffset}
                  strokeLinecap="round"
                  rotation="-90" origin={`${RING_SIZE / 2}, ${RING_SIZE / 2}`}
                />
              </Svg>
              <View style={styles.ringLabel}>
                <Text style={styles.ringLevel}>{level}</Text>
                <Text style={styles.ringText}>Level</Text>
              </View>
            </View>
            <Text style={styles.ringXPHint}>{xpInLevel} / 500 XP to next level</Text>

            <View style={styles.statsRow}>
              <View style={styles.statItem}>
                <Text style={styles.statIcon}>⏱️</Text>
                <Text style={styles.statValue}>{totalHours.toFixed(1)}h</Text>
                <Text style={styles.statLabel}>Study Time</Text>
              </View>
              <View style={styles.statItem}>
                <Text style={styles.statIcon}>⚡</Text>
                <Text style={styles.statValue}>{totalXP}</Text>
                <Text style={styles.statLabel}>Total XP</Text>
              </View>
              <View style={styles.statItem}>
                <Text style={styles.statIcon}>🔥</Text>
                <Text style={styles.statValue}>{progress?.currentStreak ?? 0}</Text>
                <Text style={styles.statLabel}>Streak</Text>
              </View>
              <View style={styles.statItem}>
                <Text style={styles.statIcon}>📚</Text>
                <Text style={styles.statValue}>{progress?.totalSessions ?? 0}</Text>
                <Text style={styles.statLabel}>Sessions</Text>
              </View>
            </View>
          </Card.Content>
        </Card>

        {/* ── Weekly Study Chart ───────────────────── */}
        {weeklyStudy.length > 0 && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>📊 This Week</Text>
            <Card style={styles.chartCard}>
              <Card.Content>
                <View style={styles.chartBars}>
                  {weeklyStudy.map((day) => {
                    const barH = day.minutes > 0 ? Math.max(8, (day.minutes / weekMax) * 100) : 4;
                    return (
                      <View key={day.date} style={styles.barCol}>
                        <Text style={styles.barMinutes}>{day.minutes > 0 ? `${day.minutes}m` : ''}</Text>
                        <View style={[styles.bar, { height: barH, backgroundColor: day.minutes > 0 ? '#0ea5e9' : '#e2e8f0' }]} />
                        <Text style={styles.barLabel}>{day.dayLabel}</Text>
                      </View>
                    );
                  })}
                </View>
              </Card.Content>
            </Card>
          </View>
        )}

        {/* ── Streak Calendar ──────────────────────── */}
        {streakCal && (
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>
              🗓️ {MONTH_NAMES[streakCal.month - 1]} {streakCal.year}
            </Text>
            <Card style={styles.calCard}>
              <Card.Content>
                <View style={styles.calHeader}>
                  {DAY_NAMES.map(d => (
                    <Text key={d} style={styles.calHeaderDay}>{d}</Text>
                  ))}
                </View>
                <View style={styles.calGrid}>
                  {buildCalendar()}
                </View>
                <View style={styles.calLegend}>
                  <View style={styles.calLegendItem}>
                    <View style={[styles.calLegendDot, { backgroundColor: '#0ea5e9' }]} />
                    <Text style={styles.calLegendText}>Studied</Text>
                  </View>
                  <Text style={styles.calLegendText}>
                    {streakCal.studiedDays.length} day{streakCal.studiedDays.length !== 1 ? 's' : ''} this month
                  </Text>
                </View>
              </Card.Content>
            </Card>
          </View>
        )}

        {/* ── Subject Progress ────────────────────── */}
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>📘 Subject Progress</Text>
          {(!progress || progress.subjectProgress.length === 0) ? (
            <Text style={styles.emptyText}>No progress data yet. Start studying!</Text>
          ) : (
            progress.subjectProgress.map((item: SubjectProgress) => (
              <Card key={item.subjectId} style={[styles.subjectCard, { borderLeftColor: item.subjectColor }]}>
                <Card.Content>
                  <View style={styles.subjectHeader}>
                    <Text style={styles.subjectName}>{item.subjectName}</Text>
                    <Text style={[styles.subjectPct, { color: getProgressColor(item.completionPercentage) }]}>
                      {Math.round(item.completionPercentage)}%
                    </Text>
                  </View>
                  <PaperProgressBar
                    progress={item.completionPercentage / 100}
                    color={getProgressColor(item.completionPercentage)}
                    style={styles.progressBar}
                  />
                  <View style={styles.subjectStats}>
                    <View style={styles.subjectStat}>
                      <Text style={styles.subjectStatVal}>{item.completedTopics}/{item.totalTopics}</Text>
                      <Text style={styles.subjectStatLbl}>Topics</Text>
                    </View>
                    <View style={styles.subjectStat}>
                      <Text style={styles.subjectStatVal}>{(item.totalStudyMinutes / 60).toFixed(1)}h</Text>
                      <Text style={styles.subjectStatLbl}>Studied</Text>
                    </View>
                    <View style={styles.subjectStat}>
                      <Text style={styles.subjectStatVal}>Lv.{item.level}</Text>
                      <Text style={styles.subjectStatLbl}>{item.xp} XP</Text>
                    </View>
                  </View>
                </Card.Content>
              </Card>
            ))
          )}
        </View>

        {/* ── Achievements ────────────────────────── */}
        <View style={styles.section}>
          <View style={styles.achHeader}>
            <Text style={styles.sectionTitle}>🏆 Achievements</Text>
            <Text style={styles.achCount}>{unlockedCount}/{achievements.length}</Text>
          </View>
          <View style={styles.achGrid}>
            {achievements.map((ach) => (
              <TouchableOpacity
                key={ach.type}
                onPress={() => {
                  Alert.alert(
                    ach.isUnlocked ? `${ach.icon} ${ach.title}` : '🔒 Locked',
                    ach.isUnlocked
                      ? `${ach.description}\n\n+${ach.xpReward} XP earned!`
                      : `${ach.description}\n\nKeep going to unlock this!`,
                  );
                }}
              >
                <Card style={[styles.achCard, !ach.isUnlocked && styles.achCardLocked]}>
                  <Card.Content style={styles.achContent}>
                    <Text style={styles.achIcon}>{ach.isUnlocked ? ach.icon : '🔒'}</Text>
                    <Text style={[styles.achTitle, !ach.isUnlocked && styles.achTitleLocked]} numberOfLines={2}>
                      {ach.title}
                    </Text>
                    {ach.isUnlocked
                      ? <Text style={styles.achXP}>+{ach.xpReward} XP</Text>
                      : <Text style={styles.achLockedHint}>{ach.description}</Text>
                    }
                  </Card.Content>
                </Card>
              </TouchableOpacity>
            ))}
          </View>
        </View>

        <View style={{ height: 32 }} />
      </ScrollView>
    </View>
  );
}

// ═══════════════════════════════════════════════════
// Styles
// ═══════════════════════════════════════════════════
const styles = StyleSheet.create({
  container: { flex: 1 },

  // Toast
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

  // Hero
  heroCard: { margin: 16, backgroundColor: '#fff' },
  heroTitle: { fontSize: 20, fontWeight: 'bold', color: '#1e293b', textAlign: 'center' },
  heroMotivation: { fontSize: 14, color: '#0ea5e9', textAlign: 'center', marginTop: 4, marginBottom: 16 },

  // Ring
  ringContainer: { alignSelf: 'center', marginBottom: 8 },
  ringLabel: { position: 'absolute', top: 0, left: 0, right: 0, bottom: 0, justifyContent: 'center', alignItems: 'center' },
  ringLevel: { fontSize: 34, fontWeight: 'bold', color: '#0ea5e9' },
  ringText: { fontSize: 11, color: '#64748b', marginTop: -2 },
  ringXPHint: { fontSize: 12, color: '#94a3b8', textAlign: 'center', marginBottom: 20 },

  // Stats row
  statsRow: { flexDirection: 'row', justifyContent: 'space-around' },
  statItem: { alignItems: 'center' },
  statIcon: { fontSize: 28 },
  statValue: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', marginTop: 4 },
  statLabel: { fontSize: 11, color: '#64748b', marginTop: 2 },

  // Sections
  section: { marginHorizontal: 16, marginBottom: 20 },
  sectionTitle: { fontSize: 17, fontWeight: 'bold', color: '#1e293b', marginBottom: 10 },
  emptyText: { color: '#94a3b8', fontStyle: 'italic', marginTop: 4 },

  // Weekly Chart
  chartCard: { backgroundColor: '#fff' },
  chartBars: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-end', height: 130, paddingTop: 16 },
  barCol: { flex: 1, alignItems: 'center', justifyContent: 'flex-end' },
  barMinutes: { fontSize: 10, color: '#64748b', marginBottom: 4, fontWeight: '600' },
  bar: { width: 24, borderRadius: 6 },
  barLabel: { fontSize: 11, color: '#94a3b8', marginTop: 6, fontWeight: '500' },

  // Calendar
  calCard: { backgroundColor: '#fff' },
  calHeader: { flexDirection: 'row', marginBottom: 4 },
  calHeaderDay: { flex: 1, textAlign: 'center', fontSize: 11, fontWeight: '600', color: '#94a3b8' },
  calGrid: { flexDirection: 'row', flexWrap: 'wrap' },
  calCell: {
    width: (SCREEN_W - 64) / 7, height: 36,
    justifyContent: 'center', alignItems: 'center', borderRadius: 8,
  },
  calCellStudied: { backgroundColor: '#e0f2fe' },
  calCellToday: { borderWidth: 2, borderColor: '#0ea5e9' },
  calDay: { fontSize: 13, color: '#64748b' },
  calDayStudied: { color: '#0284c7', fontWeight: '700' },
  calLegend: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginTop: 10, paddingTop: 10, borderTopWidth: 1, borderTopColor: '#f1f5f9' },
  calLegendItem: { flexDirection: 'row', alignItems: 'center', gap: 6 },
  calLegendDot: { width: 10, height: 10, borderRadius: 5 },
  calLegendText: { fontSize: 12, color: '#64748b' },

  // Subject cards
  subjectCard: { marginBottom: 12, borderLeftWidth: 4 },
  subjectHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  subjectName: { fontSize: 15, fontWeight: '600', color: '#1e293b', flex: 1 },
  subjectPct: { fontSize: 15, fontWeight: 'bold' },
  progressBar: { height: 8, borderRadius: 4, marginBottom: 12 },
  subjectStats: { flexDirection: 'row', justifyContent: 'space-around' },
  subjectStat: { alignItems: 'center' },
  subjectStatVal: { fontSize: 13, fontWeight: '600', color: '#1e293b' },
  subjectStatLbl: { fontSize: 11, color: '#64748b', marginTop: 2 },

  // Achievements
  achHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 10 },
  achCount: { fontSize: 14, fontWeight: '600', color: '#0ea5e9' },
  achGrid: { flexDirection: 'row', flexWrap: 'wrap', gap: 10 },
  achCard: { width: (SCREEN_W - 42) / 2 },
  achCardLocked: { opacity: 0.45, backgroundColor: '#f1f5f9' },
  achContent: { alignItems: 'center', paddingVertical: 14, paddingHorizontal: 8 },
  achIcon: { fontSize: 36, marginBottom: 6 },
  achTitle: { fontSize: 13, fontWeight: '600', color: '#1e293b', textAlign: 'center' },
  achTitleLocked: { color: '#94a3b8' },
  achXP: { fontSize: 12, color: '#fbbf24', fontWeight: 'bold', marginTop: 4 },
  achLockedHint: { fontSize: 10, color: '#94a3b8', textAlign: 'center', marginTop: 4 },
});
