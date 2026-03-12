import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import { TimetableEntry } from '../../timetable/types';

interface Props {
  todayClasses: TimetableEntry[];
}

export default function TodayClasses({ todayClasses }: Props) {
  const formatTime = (t: string) => t.slice(0, 5);

  return (
    <>
      {todayClasses.length > 0 ? (
        todayClasses.map((entry) => (
          <Card key={entry.id} style={[styles.classCard, { borderLeftColor: entry.subjectColor }]}>
            <Card.Content>
              <Text style={styles.courseName}>{entry.subjectName}</Text>
              <Text style={styles.classTime}>
                {formatTime(entry.startTime)} - {formatTime(entry.endTime)}
              </Text>
              {entry.location && <Text style={styles.location}>📍 {entry.location}</Text>}
            </Card.Content>
          </Card>
        ))
      ) : (
        <Text style={styles.emptyText}>No classes scheduled for today 🎉</Text>
      )}
    </>
  );
}

const styles = StyleSheet.create({
  classCard: { marginBottom: 8, borderLeftWidth: 4 },
  courseName: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  classTime: { fontSize: 14, color: '#64748b', marginTop: 4 },
  location: { fontSize: 12, color: '#94a3b8', marginTop: 4 },
  emptyText: { textAlign: 'center', color: '#94a3b8', fontSize: 14, fontStyle: 'italic', padding: 16 },
});
