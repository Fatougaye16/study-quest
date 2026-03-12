import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card, ProgressBar } from 'react-native-paper';

interface Props {
  dailyGoal: number;
  todayMinutes: number;
  dailyProgress: number;
  sessionActive: boolean;
}

export default function DailyQuest({ dailyGoal, todayMinutes, dailyProgress, sessionActive }: Props) {
  return (
    <Card style={styles.goalCard}>
      <Card.Content>
        <View style={styles.goalHeader}>
          <Text style={styles.goalTitle}>Study for {dailyGoal} minutes today</Text>
          <Text style={styles.goalPercentage}>{Math.round(dailyProgress)}%</Text>
        </View>
        <ProgressBar
          progress={dailyProgress / 100}
          color={dailyProgress >= 100 ? '#10b981' : '#0ea5e9'}
          style={styles.goalBar}
        />
        {dailyProgress >= 100 ? (
          <Text style={styles.goalComplete}>🎉 Daily quest complete! +50 XP</Text>
        ) : (
          <View style={styles.goalFooter}>
            <Text style={styles.goalRemaining}>
              {Math.max(0, dailyGoal - todayMinutes)} min remaining
            </Text>
            {!sessionActive && (
              <Text style={styles.goalHint}>
                {todayMinutes > 0 ? `${todayMinutes} min studied today` : 'Start a session above!'}
              </Text>
            )}
          </View>
        )}
      </Card.Content>
    </Card>
  );
}

const styles = StyleSheet.create({
  goalCard: { borderLeftWidth: 4, borderLeftColor: '#0ea5e9' },
  goalHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  goalTitle: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  goalPercentage: { fontSize: 20, fontWeight: 'bold', color: '#0ea5e9' },
  goalBar: { height: 8, borderRadius: 4, marginBottom: 8 },
  goalComplete: { fontSize: 14, color: '#10b981', fontWeight: '600', textAlign: 'center' },
  goalRemaining: { fontSize: 13, color: '#64748b' },
  goalFooter: { gap: 2 },
  goalHint: { fontSize: 12, color: '#94a3b8', fontStyle: 'italic' },
});
