import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import Svg, { Circle } from 'react-native-svg';
import { useTheme } from '../../../shared/theme';
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
  const { theme } = useTheme();
  const colors = theme.colors;
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
    <Card style={[styles.heroCard, { backgroundColor: colors.card }]}>
      <Card.Content>
        <Text style={[styles.heroTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>🎮 Your Quest Progress</Text>
        <Text style={[styles.heroMotivation, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>{getMotivation()}</Text>

        <View style={styles.ringContainer}>
          <Svg width={RING_SIZE} height={RING_SIZE}>
            <Circle
              cx={RING_SIZE / 2} cy={RING_SIZE / 2} r={RING_RADIUS}
              stroke={colors.border} strokeWidth={RING_STROKE} fill="none"
            />
            <Circle
              cx={RING_SIZE / 2} cy={RING_SIZE / 2} r={RING_RADIUS}
              stroke={colors.primary} strokeWidth={RING_STROKE} fill="none"
              strokeDasharray={`${RING_CIRCUMFERENCE}`}
              strokeDashoffset={ringOffset}
              strokeLinecap="round"
              rotation="-90" origin={`${RING_SIZE / 2}, ${RING_SIZE / 2}`}
            />
          </Svg>
          <View style={styles.ringLabel}>
            <Text style={[styles.ringLevel, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>{level}</Text>
            <Text style={[styles.ringText, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Level</Text>
          </View>
        </View>
        <Text style={[styles.ringXPHint, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>{xpInLevel} / 500 XP to next level</Text>

        <View style={styles.statsRow}>
          <View style={styles.statItem}>
            <Text style={styles.statIcon}>⏱️</Text>
            <Text style={[styles.statValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{totalHours.toFixed(1)}h</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Study Time</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statIcon}>⚡</Text>
            <Text style={[styles.statValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{totalXP}</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Total XP</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statIcon}>🔥</Text>
            <Text style={[styles.statValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{progress?.currentStreak ?? 0}</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Streak</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statIcon}>📚</Text>
            <Text style={[styles.statValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{progress?.totalSessions ?? 0}</Text>
            <Text style={[styles.statLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Sessions</Text>
          </View>
        </View>
      </Card.Content>
    </Card>
  );
}

const styles = StyleSheet.create({
  heroCard: { margin: 16 },
  heroTitle: { fontSize: 20, textAlign: 'center' },
  heroMotivation: { fontSize: 14, textAlign: 'center', marginTop: 4, marginBottom: 16 },
  ringContainer: { alignSelf: 'center', marginBottom: 8 },
  ringLabel: { position: 'absolute', top: 0, left: 0, right: 0, bottom: 0, justifyContent: 'center', alignItems: 'center' },
  ringLevel: { fontSize: 34 },
  ringText: { fontSize: 11, marginTop: -2 },
  ringXPHint: { fontSize: 12, textAlign: 'center', marginBottom: 20 },
  statsRow: { flexDirection: 'row', justifyContent: 'space-around' },
  statItem: { alignItems: 'center' },
  statIcon: { fontSize: 28 },
  statValue: { fontSize: 18, marginTop: 4 },
  statLabel: { fontSize: 11, marginTop: 2 },
});
