import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import { useTheme } from '../../../shared/theme';
import { WeeklyStudyDay } from '../types';

interface Props {
  weeklyStudy: WeeklyStudyDay[];
}

export default function WeeklyChart({ weeklyStudy }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;
  if (weeklyStudy.length === 0) return null;

  const weekMax = Math.max(1, ...weeklyStudy.map(d => d.minutes));

  return (
    <View style={styles.section}>
      <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>📊 This Week</Text>
      <Card style={[styles.chartCard, { backgroundColor: colors.card }]}>
        <Card.Content>
          <View style={styles.chartBars}>
            {weeklyStudy.map((day) => {
              const barH = day.minutes > 0 ? Math.max(8, (day.minutes / weekMax) * 100) : 4;
              return (
                <View key={day.date} style={styles.barCol}>
                  <Text style={[styles.barMinutes, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>{day.minutes > 0 ? `${day.minutes}m` : ''}</Text>
                  <View style={[styles.bar, { height: barH, backgroundColor: day.minutes > 0 ? colors.primary : colors.border }]} />
                  <Text style={[styles.barLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>{day.dayLabel}</Text>
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
  sectionTitle: { fontSize: 17, marginBottom: 10 },
  chartCard: {},
  chartBars: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-end', height: 130, paddingTop: 16 },
  barCol: { flex: 1, alignItems: 'center', justifyContent: 'flex-end' },
  barMinutes: { fontSize: 10, marginBottom: 4 },
  bar: { width: 24, borderRadius: 6 },
  barLabel: { fontSize: 11, marginTop: 6 },
});
