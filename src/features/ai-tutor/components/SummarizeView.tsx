import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { SummarizeResponse } from '../types';

interface Props {
  loading: boolean;
  summary: SummarizeResponse | null;
  onSummarize: () => void;
}

export default function SummarizeView({ loading, summary, onSummarize }: Props) {
  return (
    <View style={styles.featureContent}>
      <Button
        mode="contained" buttonColor="#0ea5e9" onPress={onSummarize}
        loading={loading} disabled={loading} icon="text-box-search-outline"
        style={styles.actionButton}
      >
        {summary ? 'Regenerate Summary' : 'Generate Summary'}
      </Button>

      {summary && (
        <View style={styles.resultCard}>
          <Text style={styles.resultTitle}>Summary</Text>
          <Text style={styles.resultText}>{summary.summary}</Text>

          {summary.keyPoints.length > 0 && (
            <>
              <Text style={[styles.resultTitle, { marginTop: 16 }]}>Key Points</Text>
              {summary.keyPoints.map((point: string, i: number) => (
                <View key={i} style={styles.bulletRow}>
                  <Ionicons name="checkmark-circle" size={18} color="#10b981" />
                  <Text style={styles.bulletText}>{point}</Text>
                </View>
              ))}
            </>
          )}
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  featureContent: { backgroundColor: '#fff', borderRadius: 14, padding: 16, marginTop: 4, marginBottom: 4, borderWidth: 1, borderColor: '#f1f5f9' },
  actionButton: { marginTop: 12, borderRadius: 10 },
  resultCard: { marginTop: 16, backgroundColor: '#f8fafc', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#e2e8f0' },
  resultTitle: { fontSize: 16, fontWeight: '700', color: '#1e293b', marginBottom: 8 },
  resultText: { fontSize: 14, color: '#475569', lineHeight: 22 },
  bulletRow: { flexDirection: 'row', alignItems: 'flex-start', gap: 10, marginTop: 8 },
  bulletText: { flex: 1, fontSize: 14, color: '#475569', lineHeight: 20 },
});
