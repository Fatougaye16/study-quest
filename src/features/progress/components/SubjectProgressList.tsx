import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card, ProgressBar as PaperProgressBar } from 'react-native-paper';
import { useTheme } from '../../../shared/theme';
import { SubjectProgress } from '../types';

interface Props {
  subjects: SubjectProgress[];
}

export default function SubjectProgressList({ subjects }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  const getProgressColor = (pct: number) => {
    if (pct >= 75) return colors.gamification.xp;
    if (pct >= 50) return colors.secondary;
    if (pct >= 25) return colors.primary;
    return colors.textTertiary;
  };

  return (
    <View style={styles.section}>
      <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>📘 Subject Progress</Text>
      {subjects.length === 0 ? (
        <Text style={[styles.emptyText, { color: colors.textTertiary }]}>No progress data yet. Start studying!</Text>
      ) : (
        subjects.map((item) => (
          <Card key={item.subjectId} style={[styles.subjectCard, { borderLeftColor: item.subjectColor, backgroundColor: colors.card }]}>
            <Card.Content>
              <View style={styles.subjectHeader}>
                <Text style={[styles.subjectName, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{item.subjectName}</Text>
                <Text style={[styles.subjectPct, { color: getProgressColor(item.completionPercentage), fontFamily: theme.fonts.headingBold }]}>
                  {Math.round(item.completionPercentage)}%
                </Text>
              </View>
              <PaperProgressBar
                progress={item.completionPercentage / 100}
                color={getProgressColor(item.completionPercentage)}
                style={styles.progressBar}
              />
              <View style={styles.subjectStats}>
                <View style={styles.subjectStat}>
                  <Text style={[styles.subjectStatVal, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{item.completedTopics}/{item.totalTopics}</Text>
                  <Text style={[styles.subjectStatLbl, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Topics</Text>
                </View>
                <View style={styles.subjectStat}>
                  <Text style={[styles.subjectStatVal, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{(item.totalStudyMinutes / 60).toFixed(1)}h</Text>
                  <Text style={[styles.subjectStatLbl, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Studied</Text>
                </View>
                <View style={styles.subjectStat}>
                  <Text style={[styles.subjectStatVal, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Lv.{item.level}</Text>
                  <Text style={[styles.subjectStatLbl, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>{item.xp} XP</Text>
                </View>
              </View>
            </Card.Content>
          </Card>
        ))
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  section: { marginHorizontal: 16, marginBottom: 20 },
  sectionTitle: { fontSize: 17, marginBottom: 10 },
  emptyText: { fontStyle: 'italic', marginTop: 4 },
  subjectCard: { marginBottom: 12, borderLeftWidth: 4 },
  subjectHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  subjectName: { fontSize: 15, flex: 1 },
  subjectPct: { fontSize: 15 },
  progressBar: { height: 8, borderRadius: 4, marginBottom: 12 },
  subjectStats: { flexDirection: 'row', justifyContent: 'space-around' },
  subjectStat: { alignItems: 'center' },
  subjectStatVal: { fontSize: 13 },
  subjectStatLbl: { fontSize: 11, marginTop: 2 },
});
