import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Alert, Dimensions } from 'react-native';
import { Card } from 'react-native-paper';
import { useTheme } from '../../../shared/theme';
import { Achievement } from '../types';

const SCREEN_W = Dimensions.get('window').width;

interface Props {
  achievements: Achievement[];
  unlockedCount: number;
}

export default function AchievementGrid({ achievements, unlockedCount }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <View style={styles.section}>
      <View style={styles.achHeader}>
        <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>🏆 Achievements</Text>
        <Text style={[styles.achCount, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>{unlockedCount}/{achievements.length}</Text>
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
            <Card style={[styles.achCard, { backgroundColor: colors.card }, !ach.isUnlocked && { opacity: 0.45, backgroundColor: colors.border }]}>
              <Card.Content style={styles.achContent}>
                <Text style={styles.achIcon}>{ach.isUnlocked ? ach.icon : '🔒'}</Text>
                <Text style={[styles.achTitle, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }, !ach.isUnlocked && { color: colors.textTertiary }]} numberOfLines={2}>
                  {ach.title}
                </Text>
                {ach.isUnlocked
                  ? <Text style={[styles.achXP, { color: colors.accent, fontFamily: theme.fonts.bodyBold }]}>+{ach.xpReward} XP</Text>
                  : <Text style={[styles.achLockedHint, { color: colors.textTertiary }]}>{ach.description}</Text>
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
  sectionTitle: { fontSize: 17, marginBottom: 10 },
  achHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 10 },
  achCount: { fontSize: 14 },
  achGrid: { flexDirection: 'row', flexWrap: 'wrap', gap: 10 },
  achCard: { width: (SCREEN_W - 42) / 2 },
  achContent: { alignItems: 'center', paddingVertical: 14, paddingHorizontal: 8 },
  achIcon: { fontSize: 36, marginBottom: 6 },
  achTitle: { fontSize: 13, textAlign: 'center' },
  achXP: { fontSize: 12, marginTop: 4 },
  achLockedHint: { fontSize: 10, textAlign: 'center', marginTop: 4 },
});
