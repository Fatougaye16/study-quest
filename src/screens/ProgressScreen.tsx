import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Dimensions, TouchableOpacity, Alert, RefreshControl } from 'react-native';
import { Card, ProgressBar as PaperProgressBar } from 'react-native-paper';
import { progressAPI } from '../services/api';
import { OverallProgress, SubjectProgress, Achievement } from '../types';
import { useAuth } from '../contexts/AuthContext';

export default function ProgressScreen() {
  const { user } = useAuth();
  const [progress, setProgress] = useState<OverallProgress | null>(null);
  const [achievements, setAchievements] = useState<Achievement[]>([]);
  const [refreshing, setRefreshing] = useState(false);

  const loadData = useCallback(async () => {
    try {
      const [progRes, achRes] = await Promise.all([
        progressAPI.get(),
        progressAPI.getAchievements(),
      ]);
      setProgress(progRes.data);
      setAchievements(achRes.data);
    } catch (error) {
      console.error('Failed to load progress:', error);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  const getMotivationalMessage = () => {
    if (!progress) return "Let's get started! 🌟";
    const avg = progress.subjectProgress.length > 0
      ? progress.subjectProgress.reduce((s, p) => s + p.completionPercentage, 0) / progress.subjectProgress.length
      : 0;
    if (avg >= 75) return "You're crushing it! 🔥";
    if (avg >= 50) return "Great progress! Keep going! 💪";
    if (avg >= 25) return "You're making moves! 🚀";
    return "Let's get started! 🌟";
  };

  const getProgressColor = (pct: number) => {
    if (pct >= 75) return '#10b981';
    if (pct >= 50) return '#f59e0b';
    if (pct >= 25) return '#3b82f6';
    return '#94a3b8';
  };

  const totalHours = progress ? (progress.totalStudyMinutes / 60) : 0;
  const unlockedCount = achievements.filter(a => a.isUnlocked).length;

  return (
    <ScrollView
      style={styles.container}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#8b5cf6']} />}
    >
      <Card style={styles.overallCard}>
        <Card.Content>
          <Text style={styles.overallTitle}>🎮 Your Quest Progress</Text>
          <Text style={styles.motivationText}>{getMotivationalMessage()}</Text>

          <View style={styles.progressCircle}>
            <Text style={styles.progressPercentage}>
              {progress?.level ?? 1}
            </Text>
            <Text style={styles.progressLabel}>Level</Text>
          </View>

          <View style={styles.statsRow}>
            <View style={styles.statItem}>
              <Text style={styles.statIcon}>⏱️</Text>
              <Text style={styles.statValue}>{totalHours.toFixed(1)}h</Text>
              <Text style={styles.statLabel}>Study Time</Text>
            </View>
            <View style={styles.statItem}>
              <Text style={styles.statIcon}>⚡</Text>
              <Text style={styles.statValue}>{progress?.totalXP ?? 0}</Text>
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

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Subject Progress</Text>
        {(!progress || progress.subjectProgress.length === 0) ? (
          <Text style={styles.emptyText}>No progress data yet. Start studying!</Text>
        ) : (
          progress.subjectProgress.map((item: SubjectProgress) => (
            <Card key={item.subjectId} style={[styles.courseCard, { borderLeftColor: item.subjectColor, borderLeftWidth: 4 }]}>
              <Card.Content>
                <View style={styles.courseHeader}>
                  <Text style={styles.courseName}>{item.subjectName}</Text>
                  <Text style={styles.coursePercentage}>{Math.round(item.completionPercentage)}%</Text>
                </View>

                <PaperProgressBar
                  progress={item.completionPercentage / 100}
                  color={getProgressColor(item.completionPercentage)}
                  style={styles.progressBar}
                />

                <View style={styles.courseStats}>
                  <View style={styles.courseStat}>
                    <Text style={styles.courseStatValue}>{item.completedTopics}/{item.totalTopics}</Text>
                    <Text style={styles.courseStatLabel}>Topics</Text>
                  </View>
                  <View style={styles.courseStat}>
                    <Text style={styles.courseStatValue}>{(item.totalStudyMinutes / 60).toFixed(1)}h</Text>
                    <Text style={styles.courseStatLabel}>Studied</Text>
                  </View>
                  <View style={styles.courseStat}>
                    <Text style={styles.courseStatValue}>Lv.{item.level}</Text>
                    <Text style={styles.courseStatLabel}>{item.xp} XP</Text>
                  </View>
                </View>
              </Card.Content>
            </Card>
          ))
        )}
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>🏆 Achievements</Text>
        <Text style={styles.achievementCount}>{unlockedCount} / {achievements.length}</Text>
        <View style={styles.achievementsGrid}>
          {achievements.map((ach) => (
            <TouchableOpacity
              key={ach.type}
              onPress={() => {
                Alert.alert(
                  ach.isUnlocked ? `${ach.icon} ${ach.title}` : '🔒 Locked',
                  ach.isUnlocked
                    ? `${ach.description}\n\n+${ach.xpReward} XP earned!`
                    : `${ach.description}\n\nKeep going to unlock this!`,
                  [{ text: 'OK' }]
                );
              }}
            >
              <Card style={[styles.achievementCard, !ach.isUnlocked && styles.achievementLocked]}>
                <Card.Content style={styles.achievementContent}>
                  <Text style={styles.achievementIcon}>{ach.isUnlocked ? ach.icon : '🔒'}</Text>
                  <Text style={[styles.achievementTitle, !ach.isUnlocked && styles.achievementTitleLocked]}>
                    {ach.title}
                  </Text>
                  {ach.isUnlocked && <Text style={styles.achievementXP}>+{ach.xpReward} XP</Text>}
                </Card.Content>
              </Card>
            </TouchableOpacity>
          ))}
        </View>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  overallCard: { margin: 16, backgroundColor: '#fff' },
  overallTitle: { fontSize: 20, fontWeight: 'bold', color: '#1e293b', textAlign: 'center' },
  motivationText: { fontSize: 16, color: '#8b5cf6', textAlign: 'center', marginTop: 4, marginBottom: 20 },
  progressCircle: { width: 140, height: 140, borderRadius: 70, backgroundColor: '#f1f5f9', alignSelf: 'center', justifyContent: 'center', alignItems: 'center', marginBottom: 24, borderWidth: 8, borderColor: '#ec4899' },
  progressPercentage: { fontSize: 36, fontWeight: 'bold', color: '#ec4899' },
  progressLabel: { fontSize: 12, color: '#64748b', marginTop: 4 },
  statsRow: { flexDirection: 'row', justifyContent: 'space-around' },
  statItem: { alignItems: 'center' },
  statIcon: { fontSize: 32 },
  statValue: { fontSize: 20, fontWeight: 'bold', color: '#1e293b', marginTop: 8 },
  statLabel: { fontSize: 12, color: '#64748b', marginTop: 4 },
  section: { marginHorizontal: 16, marginBottom: 24 },
  sectionTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', marginBottom: 4 },
  emptyText: { color: '#94a3b8', fontStyle: 'italic', marginTop: 8 },
  courseCard: { marginBottom: 12 },
  courseHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  courseName: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  coursePercentage: { fontSize: 16, fontWeight: 'bold', color: '#8b5cf6' },
  progressBar: { height: 8, borderRadius: 4, marginBottom: 12 },
  courseStats: { flexDirection: 'row', justifyContent: 'space-around' },
  courseStat: { alignItems: 'center' },
  courseStatValue: { fontSize: 14, fontWeight: '600', color: '#1e293b' },
  courseStatLabel: { fontSize: 12, color: '#64748b', marginTop: 2 },
  achievementCount: { fontSize: 14, color: '#ec4899', fontWeight: '600', marginBottom: 12 },
  achievementsGrid: { flexDirection: 'row', flexWrap: 'wrap', gap: 12 },
  achievementCard: { width: (Dimensions.get('window').width - 56) / 2 },
  achievementLocked: { opacity: 0.5, backgroundColor: '#f1f5f9' },
  achievementContent: { alignItems: 'center', padding: 12 },
  achievementIcon: { fontSize: 40, marginBottom: 8 },
  achievementTitle: { fontSize: 14, fontWeight: '600', color: '#1e293b', textAlign: 'center' },
  achievementTitleLocked: { color: '#94a3b8' },
  achievementXP: { fontSize: 12, color: '#fbbf24', fontWeight: 'bold', marginTop: 4 },
});
