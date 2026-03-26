import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import XCard from '../../../shared/components/XCard';
import { StudySession } from '../types';

interface Props {
  recentSessions: StudySession[];
}

export default function RecentSessions({ recentSessions }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <>
      {recentSessions.length > 0 ? (
        recentSessions.map((session) => (
          <XCard key={session.id} style={styles.sessionCard}>
            <View style={styles.sessionHeader}>
              <Text style={[styles.sessionCourse, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>
                {session.subjectName}
              </Text>
              <Feather name="check-circle" size={20} color={colors.gamification.xp} />
            </View>
            <Text style={[styles.sessionTopic, { color: colors.text, fontFamily: theme.fonts.body }]}>
              {session.topicName || 'General study'}
            </Text>
            <Text style={[styles.sessionDuration, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
              {session.durationMinutes} minutes
            </Text>
          </XCard>
        ))
      ) : (
        <Text style={[styles.emptyText, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>
          Start your first study session!
        </Text>
      )}
    </>
  );
}

const styles = StyleSheet.create({
  sessionCard: { marginBottom: 8 },
  sessionHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  sessionCourse: { fontSize: 14 },
  sessionTopic: { fontSize: 16, marginTop: 4 },
  sessionDuration: { fontSize: 12, marginTop: 4 },
  emptyText: { textAlign: 'center', fontSize: 14, fontStyle: 'italic', padding: 16 },
});
