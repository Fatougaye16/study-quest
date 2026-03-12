import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Card, ProgressBar as PaperProgressBar } from 'react-native-paper';
import { SubjectProgress } from '../types';

interface Props {
  subjects: SubjectProgress[];
}

const getProgressColor = (pct: number) => {
  if (pct >= 75) return '#10b981';
  if (pct >= 50) return '#f59e0b';
  if (pct >= 25) return '#3b82f6';
  return '#94a3b8';
};

export default function SubjectProgressList({ subjects }: Props) {
  return (
    <View style={styles.section}>
      <Text style={styles.sectionTitle}>📘 Subject Progress</Text>
      {subjects.length === 0 ? (
        <Text style={styles.emptyText}>No progress data yet. Start studying!</Text>
      ) : (
        subjects.map((item) => (
          <Card key={item.subjectId} style={[styles.subjectCard, { borderLeftColor: item.subjectColor }]}>
            <Card.Content>
              <View style={styles.subjectHeader}>
                <Text style={styles.subjectName}>{item.subjectName}</Text>
                <Text style={[styles.subjectPct, { color: getProgressColor(item.completionPercentage) }]}>
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
                  <Text style={styles.subjectStatVal}>{item.completedTopics}/{item.totalTopics}</Text>
                  <Text style={styles.subjectStatLbl}>Topics</Text>
                </View>
                <View style={styles.subjectStat}>
                  <Text style={styles.subjectStatVal}>{(item.totalStudyMinutes / 60).toFixed(1)}h</Text>
                  <Text style={styles.subjectStatLbl}>Studied</Text>
                </View>
                <View style={styles.subjectStat}>
                  <Text style={styles.subjectStatVal}>Lv.{item.level}</Text>
                  <Text style={styles.subjectStatLbl}>{item.xp} XP</Text>
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
  sectionTitle: { fontSize: 17, fontWeight: 'bold', color: '#1e293b', marginBottom: 10 },
  emptyText: { color: '#94a3b8', fontStyle: 'italic', marginTop: 4 },
  subjectCard: { marginBottom: 12, borderLeftWidth: 4 },
  subjectHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 8 },
  subjectName: { fontSize: 15, fontWeight: '600', color: '#1e293b', flex: 1 },
  subjectPct: { fontSize: 15, fontWeight: 'bold' },
  progressBar: { height: 8, borderRadius: 4, marginBottom: 12 },
  subjectStats: { flexDirection: 'row', justifyContent: 'space-around' },
  subjectStat: { alignItems: 'center' },
  subjectStatVal: { fontSize: 13, fontWeight: '600', color: '#1e293b' },
  subjectStatLbl: { fontSize: 11, color: '#64748b', marginTop: 2 },
});
