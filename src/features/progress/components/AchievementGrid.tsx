import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Alert, Dimensions } from 'react-native';
import { Card } from 'react-native-paper';
import { Achievement } from '../types';

const SCREEN_W = Dimensions.get('window').width;

interface Props {
  achievements: Achievement[];
  unlockedCount: number;
}

export default function AchievementGrid({ achievements, unlockedCount }: Props) {
  return (
    <View style={styles.section}>
      <View style={styles.achHeader}>
        <Text style={styles.sectionTitle}>🏆 Achievements</Text>
        <Text style={styles.achCount}>{unlockedCount}/{achievements.length}</Text>
      </View>
      <View style={styles.achGrid}>
        {achievements.map((ach) => (
          <TouchableOpacity
            key={ach.type}
            onPress={() => {
              Alert.alert(
                ach.isUnlocked ? `${ach.icon} ${ach.title}` : '🔒 Locked',
                ach.isUnlocked
                  ? `${ach.description}\n\n+${ach.xpReward} XP earned!`
                  : `${ach.description}\n\nKeep going to unlock this!`,
              );
            }}
          >
            <Card style={[styles.achCard, !ach.isUnlocked && styles.achCardLocked]}>
              <Card.Content style={styles.achContent}>
                <Text style={styles.achIcon}>{ach.isUnlocked ? ach.icon : '🔒'}</Text>
                <Text style={[styles.achTitle, !ach.isUnlocked && styles.achTitleLocked]} numberOfLines={2}>
                  {ach.title}
                </Text>
                {ach.isUnlocked
                  ? <Text style={styles.achXP}>+{ach.xpReward} XP</Text>
                  : <Text style={styles.achLockedHint}>{ach.description}</Text>
                }
              </Card.Content>
            </Card>
          </TouchableOpacity>
        ))}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  section: { marginHorizontal: 16, marginBottom: 20 },
  sectionTitle: { fontSize: 17, fontWeight: 'bold', color: '#1e293b', marginBottom: 10 },
  achHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 10 },
  achCount: { fontSize: 14, fontWeight: '600', color: '#0ea5e9' },
  achGrid: { flexDirection: 'row', flexWrap: 'wrap', gap: 10 },
  achCard: { width: (SCREEN_W - 42) / 2 },
  achCardLocked: { opacity: 0.45, backgroundColor: '#f1f5f9' },
  achContent: { alignItems: 'center', paddingVertical: 14, paddingHorizontal: 8 },
  achIcon: { fontSize: 36, marginBottom: 6 },
  achTitle: { fontSize: 13, fontWeight: '600', color: '#1e293b', textAlign: 'center' },
  achTitleLocked: { color: '#94a3b8' },
  achXP: { fontSize: 12, color: '#fbbf24', fontWeight: 'bold', marginTop: 4 },
  achLockedHint: { fontSize: 10, color: '#94a3b8', textAlign: 'center', marginTop: 4 },
});
