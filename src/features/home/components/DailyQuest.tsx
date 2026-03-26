import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import XCard from '../../../shared/components/XCard';
import XProgressBar from '../../../shared/components/XProgressBar';

interface Props {
  dailyGoal: number;
  todayMinutes: number;
  dailyProgress: number;
  sessionActive: boolean;
}

export default function DailyQuest({ dailyGoal, todayMinutes, dailyProgress, sessionActive }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;
  const complete = dailyProgress >= 100;

  return (
    <XCard style={{ borderLeftWidth: 4, borderLeftColor: complete ? colors.gamification.xp : colors.primary }}>
      <View style={styles.goalHeader}>
        <Text style={[styles.goalTitle, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>
          Study for {dailyGoal} minutes today
        </Text>
        <Text style={[styles.goalPercentage, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>
          {Math.round(dailyProgress)}%
        </Text>
      </View>
      <XProgressBar
        progress={dailyProgress / 100}
        color={complete ? colors.gamification.xp : colors.primary}
        height={8}
      />
      {complete ? (
        <View style={styles.completeRow}>
          <Feather name="award" size={16} color={colors.gamification.xp} />
          <Text style={[styles.goalComplete, { color: colors.gamification.xp, fontFamily: theme.fonts.bodySemiBold }]}>
            Daily quest complete! +50 XP
          </Text>
        </View>
      ) : (
        <View style={styles.goalFooter}>
          <Text style={[styles.goalRemaining, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
            {Math.max(0, dailyGoal - todayMinutes)} min remaining
          </Text>
          {!sessionActive && (
            <Text style={[styles.goalHint, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>
              {todayMinutes > 0 ? `${todayMinutes} min studied today` : 'Start a session above!'}
            </Text>
          )}
        </View>
      )}
    </XCard>
  );
}

const styles = StyleSheet.create({
  goalHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  goalTitle: { fontSize: 16 },
  goalPercentage: { fontSize: 20 },
  completeRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 6, marginTop: 8 },
  goalComplete: { fontSize: 14, textAlign: 'center' },
  goalRemaining: { fontSize: 13, marginTop: 8 },
  goalFooter: { gap: 2 },
  goalHint: { fontSize: 12, fontStyle: 'italic' },
});
