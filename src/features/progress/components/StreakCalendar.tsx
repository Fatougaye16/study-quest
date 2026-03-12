import React from 'react';
import { View, Text, StyleSheet, Dimensions } from 'react-native';
import { Card } from 'react-native-paper';
import { StreakCalendar as StreakCalendarType } from '../types';

const SCREEN_W = Dimensions.get('window').width;
const DAY_NAMES = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
const MONTH_NAMES = [
  'January', 'February', 'March', 'April', 'May', 'June',
  'July', 'August', 'September', 'October', 'November', 'December',
];

interface Props {
  streakCal: StreakCalendarType;
}

export default function StreakCalendar({ streakCal }: Props) {
  const daysInMonth = new Date(streakCal.year, streakCal.month, 0).getDate();
  const firstDow = new Date(streakCal.year, streakCal.month - 1, 1).getDay();
  const studiedSet = new Set(streakCal.studiedDays);
  const today = new Date();
  const isCurrentMonth = today.getFullYear() === streakCal.year && today.getMonth() + 1 === streakCal.month;
  const todayDay = isCurrentMonth ? today.getDate() : -1;

  const cells: React.ReactNode[] = [];
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

  return (
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
          <View style={styles.calGrid}>{cells}</View>
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
  );
}

const styles = StyleSheet.create({
  section: { marginHorizontal: 16, marginBottom: 20 },
  sectionTitle: { fontSize: 17, fontWeight: 'bold', color: '#1e293b', marginBottom: 10 },
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
});
