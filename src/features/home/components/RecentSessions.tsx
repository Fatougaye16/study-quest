import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { StudySession } from '../types';

interface Props {
  recentSessions: StudySession[];
}

export default function RecentSessions({ recentSessions }: Props) {
  return (
    <>
      {recentSessions.length > 0 ? (
        recentSessions.map((session) => (
          <Card key={session.id} style={styles.sessionCard}>
            <Card.Content>
              <View style={styles.sessionHeader}>
                <Text style={styles.sessionCourse}>{session.subjectName}</Text>
                <Ionicons name="checkmark-circle" size={20} color="#10b981" />
              </View>
              <Text style={styles.sessionTopic}>{session.topicName || 'General study'}</Text>
              <Text style={styles.sessionDuration}>{session.durationMinutes} minutes</Text>
            </Card.Content>
          </Card>
        ))
      ) : (
        <Text style={styles.emptyText}>Start your first study session!</Text>
      )}
    </>
  );
}

const styles = StyleSheet.create({
  sessionCard: { marginBottom: 8 },
  sessionHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  sessionCourse: { fontSize: 14, fontWeight: '600', color: '#0284c7' },
  sessionTopic: { fontSize: 16, color: '#1e293b', marginTop: 4 },
  sessionDuration: { fontSize: 12, color: '#64748b', marginTop: 4 },
  emptyText: { textAlign: 'center', color: '#94a3b8', fontSize: 14, fontStyle: 'italic', padding: 16 },
});
