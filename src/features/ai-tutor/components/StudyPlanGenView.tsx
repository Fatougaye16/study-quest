import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';

interface Props {
  loading: boolean;
  planDays: number;
  onSetPlanDays: (n: number) => void;
  planCreated: boolean;
  onGenerate: () => void;
}

export default function StudyPlanGenView({ loading, planDays, onSetPlanDays, planCreated, onGenerate }: Props) {
  return (
    <View style={styles.featureContent}>
      <Text style={styles.inputLabel}>Plan duration (days)</Text>
      <View style={styles.chipRow}>
        {[7, 14, 21, 30].map(n => (
          <TouchableOpacity
            key={n}
            style={[styles.chip, planDays === n && styles.chipSelected]}
            onPress={() => onSetPlanDays(n)}
          >
            <Text style={[styles.chipText, planDays === n && styles.chipTextSelected]}>{n} days</Text>
          </TouchableOpacity>
        ))}
      </View>

      <Button
        mode="contained" buttonColor="#10b981" onPress={onGenerate}
        loading={loading} disabled={loading} icon="calendar-outline"
        style={styles.actionButton}
      >
        Generate WASSCE Study Plan
      </Button>

      {planCreated && (
        <View style={styles.resultCard}>
          <Ionicons name="checkmark-circle" size={40} color="#10b981" style={{ alignSelf: 'center' }} />
          <Text style={[styles.resultTitle, { textAlign: 'center', marginTop: 8 }]}>
            Study Plan Created! 🎉
          </Text>
          <Text style={[styles.resultText, { textAlign: 'center' }]}>
            Your {planDays}-day WASSCE preparation plan is ready. Check the Study Plan tab to see it.
          </Text>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  featureContent: { backgroundColor: '#fff', borderRadius: 14, padding: 16, marginTop: 4, marginBottom: 4, borderWidth: 1, borderColor: '#f1f5f9' },
  actionButton: { marginTop: 12, borderRadius: 10 },
  inputLabel: { fontSize: 13, fontWeight: '600', color: '#64748b', marginBottom: 6, marginTop: 8 },
  chipRow: { flexDirection: 'row', gap: 8, marginBottom: 4 },
  chip: { paddingHorizontal: 16, paddingVertical: 10, borderRadius: 20, borderWidth: 2, borderColor: '#e2e8f0', backgroundColor: '#fff' },
  chipSelected: { borderColor: '#0ea5e9', backgroundColor: '#f0f9ff' },
  chipText: { fontSize: 14, color: '#64748b', fontWeight: '500' },
  chipTextSelected: { color: '#0ea5e9', fontWeight: '700' },
  resultCard: { marginTop: 16, backgroundColor: '#f8fafc', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#e2e8f0' },
  resultTitle: { fontSize: 16, fontWeight: '700', color: '#1e293b', marginBottom: 8 },
  resultText: { fontSize: 14, color: '#475569', lineHeight: 22 },
});
