import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import Svg, { Circle } from 'react-native-svg';
import { OverallProgress } from '../types';

const RING_SIZE = 130;
const RING_STROKE = 10;
const RING_RADIUS = (RING_SIZE - RING_STROKE) / 2;
const RING_CIRCUMFERENCE = 2 * Math.PI * RING_RADIUS;

interface Props {
  progress: OverallProgress | null;
  unlockedCount: number;
  totalAchievements: number;
}

export default function HeroCard({ progress, unlockedCount, totalAchievements }: Props) {
  const totalXP = progress?.totalXP ?? 0;
  const level = progress?.level ?? 1;
  const xpInLevel = totalXP % 500;
  const xpProgress = xpInLevel / 500;
  const ringOffset = RING_CIRCUMFERENCE * (1 - xpProgress);
  const totalHours = progress ? progress.totalStudyMinutes / 60 : 0;

  const getMotivation = () => {
    if (!progress) return "Let's get started! 🌟";
    const avg =
      progress.subjectProgress.length > 0
        ? progress.subjectProgress.reduce((s, p) => s + p.completionPercentage, 0) /
          progress.subjectProgress.length
        : 0;
    if (avg >= 75) return "You're crushing it! 🔥";
    if (avg >= 50) return 'Great progress! Keep going! 💪';
    if (avg >= 25) return "You're making moves! 🚀";
    return "Let's get started! 🌟";
  };

  return (
    <Card style={styles.heroCard}>
      <Card.Content>
        <Text style={styles.heroTitle}>🎮 Your Quest Progress</Text>
        <Text style={styles.heroMotivation}>{getMotivation()}</Text>

        <View style={styles.ringContainer}>
          <Svg width={RING_SIZE} height={RING_SIZE}>
            <Circle
              cx={RING_SIZE / 2} cy={RING_SIZE / 2} r={RING_RADIUS}
              stroke="#e2e8f0" strokeWidth={RING_STROKE} fill="none"
            />
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
  );
}

const styles = StyleSheet.create({
  heroCard: { margin: 16, backgroundColor: '#fff' },
  heroTitle: { fontSize: 20, fontWeight: 'bold', color: '#1e293b', textAlign: 'center' },
  heroMotivation: { fontSize: 14, color: '#0ea5e9', textAlign: 'center', marginTop: 4, marginBottom: 16 },
  ringContainer: { alignSelf: 'center', marginBottom: 8 },
  ringLabel: { position: 'absolute', top: 0, left: 0, right: 0, bottom: 0, justifyContent: 'center', alignItems: 'center' },
  ringLevel: { fontSize: 34, fontWeight: 'bold', color: '#0ea5e9' },
  ringText: { fontSize: 11, color: '#64748b', marginTop: -2 },
  ringXPHint: { fontSize: 12, color: '#94a3b8', textAlign: 'center', marginBottom: 20 },
  statsRow: { flexDirection: 'row', justifyContent: 'space-around' },
  statItem: { alignItems: 'center' },
  statIcon: { fontSize: 28 },
  statValue: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', marginTop: 4 },
  statLabel: { fontSize: 11, color: '#64748b', marginTop: 2 },
});
