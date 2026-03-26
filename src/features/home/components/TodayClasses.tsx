import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import XCard from '../../../shared/components/XCard';
import { TimetableEntry } from '../../timetable/types';

interface Props {
  todayClasses: TimetableEntry[];
}

export default function TodayClasses({ todayClasses }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;
  const formatTime = (t: string) => t.slice(0, 5);

  return (
    <>
      {todayClasses.length > 0 ? (
        todayClasses.map((entry) => (
          <XCard key={entry.id} style={[styles.classCard, { borderLeftColor: entry.subjectColor }]}>
            <Text style={[styles.courseName, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>
              {entry.subjectName}
            </Text>
            <Text style={[styles.classTime, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
              {formatTime(entry.startTime)} - {formatTime(entry.endTime)}
            </Text>
            {entry.location && (
              <View style={styles.locationRow}>
                <Feather name="map-pin" size={12} color={colors.textTertiary} />
                <Text style={[styles.location, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>
                  {entry.location}
                </Text>
              </View>
            )}
          </XCard>
        ))
      ) : (
        <Text style={[styles.emptyText, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>
          No classes scheduled for today
        </Text>
      )}
    </>
  );
}

const styles = StyleSheet.create({
  classCard: { marginBottom: 8, borderLeftWidth: 4 },
  courseName: { fontSize: 16 },
  classTime: { fontSize: 14, marginTop: 4 },
  locationRow: { flexDirection: 'row', alignItems: 'center', gap: 4, marginTop: 4 },
  location: { fontSize: 12 },
  emptyText: { textAlign: 'center', fontSize: 14, fontStyle: 'italic', padding: 16 },
});
