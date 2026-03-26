import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';

interface Props {
  loading: boolean;
  planDays: number;
  onSetPlanDays: (n: number) => void;
  planCreated: boolean;
  onGenerate: () => void;
}

export default function StudyPlanGenView({ loading, planDays, onSetPlanDays, planCreated, onGenerate }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <View style={[styles.featureContent, { backgroundColor: colors.card, borderColor: colors.border }]}>
      <Text style={[styles.inputLabel, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>Plan duration (days)</Text>
      <View style={styles.chipRow}>
        {[7, 14, 21, 30].map(n => (
          <TouchableOpacity
            key={n}
            style={[
              styles.chip,
              { borderColor: colors.border, backgroundColor: colors.card },
              planDays === n && { borderColor: colors.primary, backgroundColor: colors.primary + '10' },
            ]}
            onPress={() => onSetPlanDays(n)}
          >
            <Text
              style={[
                styles.chipText,
                { color: colors.textSecondary, fontFamily: theme.fonts.bodyMedium },
                planDays === n && { color: colors.primary, fontFamily: theme.fonts.headingBold },
              ]}
            >
              {n} days
            </Text>
          </TouchableOpacity>
        ))}
      </View>

      <Button
        mode="contained" buttonColor={colors.gamification.xp} onPress={onGenerate}
        loading={loading} disabled={loading} icon="calendar-outline"
        style={styles.actionButton}
      >
        Generate WASSCE Study Plan
      </Button>

      {planCreated && (
        <View style={[styles.resultCard, { backgroundColor: colors.surface, borderColor: colors.border }]}>
          <Feather name="check-circle" size={40} color={colors.gamification.xp} style={{ alignSelf: 'center' }} />
          <Text style={[styles.resultTitle, { textAlign: 'center', marginTop: 8, color: colors.text, fontFamily: theme.fonts.headingBold }]}>
            Study Plan Created!
          </Text>
          <Text style={[styles.resultText, { textAlign: 'center', color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
            Your {planDays}-day WASSCE preparation plan is ready. Check the Study Plan tab to see it.
          </Text>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  featureContent: { borderRadius: 16, padding: 16, marginTop: 4, marginBottom: 4, borderWidth: 1 },
  actionButton: { marginTop: 12, borderRadius: 12 },
  inputLabel: { fontSize: 13, marginBottom: 6, marginTop: 8 },
  chipRow: { flexDirection: 'row', gap: 8, marginBottom: 4 },
  chip: { paddingHorizontal: 16, paddingVertical: 10, borderRadius: 20, borderWidth: 2 },
  chipText: { fontSize: 14 },
  resultCard: { marginTop: 16, borderRadius: 12, padding: 16, borderWidth: 1 },
  resultTitle: { fontSize: 16, marginBottom: 8 },
  resultText: { fontSize: 14, lineHeight: 22 },
});
