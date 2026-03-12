import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import { WeeklyStudyDay } from '../types';

interface Props {
  weeklyStudy: WeeklyStudyDay[];
}

export default function WeeklyChart({ weeklyStudy }: Props) {
  if (weeklyStudy.length === 0) return null;

  const weekMax = Math.max(1, ...weeklyStudy.map(d => d.minutes));

  return (
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
  );
}

const styles = StyleSheet.create({
  section: { marginHorizontal: 16, marginBottom: 20 },
  sectionTitle: { fontSize: 17, fontWeight: 'bold', color: '#1e293b', marginBottom: 10 },
  chartCard: { backgroundColor: '#fff' },
  chartBars: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-end', height: 130, paddingTop: 16 },
  barCol: { flex: 1, alignItems: 'center', justifyContent: 'flex-end' },
  barMinutes: { fontSize: 10, color: '#64748b', marginBottom: 4, fontWeight: '600' },
  bar: { width: 24, borderRadius: 6 },
  barLabel: { fontSize: 11, color: '#94a3b8', marginTop: 6, fontWeight: '500' },
});
