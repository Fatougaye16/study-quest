import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import { SummarizeResponse } from '../types';

interface Props {
  loading: boolean;
  summary: SummarizeResponse | null;
  onSummarize: () => void;
}

export default function SummarizeView({ loading, summary, onSummarize }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <View style={[styles.featureContent, { backgroundColor: colors.card, borderColor: colors.border }]}>
      <Button
        mode="contained" buttonColor={colors.primary} onPress={onSummarize}
        loading={loading} disabled={loading} icon="text-box-search-outline"
        style={styles.actionButton}
      >
        {summary ? 'Regenerate Summary' : 'Generate Summary'}
      </Button>

      {summary && (
        <View style={[styles.resultCard, { backgroundColor: colors.surface, borderColor: colors.border }]}>
          <Text style={[styles.resultTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Summary</Text>
          <Text style={[styles.resultText, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>{summary.summary}</Text>

          {summary.keyPoints.length > 0 && (
            <>
              <Text style={[styles.resultTitle, { color: colors.text, fontFamily: theme.fonts.headingBold, marginTop: 16 }]}>Key Points</Text>
              {summary.keyPoints.map((point: string, i: number) => (
                <View key={i} style={styles.bulletRow}>
                  <Feather name="check-circle" size={18} color={colors.gamification.xp} />
                  <Text style={[styles.bulletText, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>{point}</Text>
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
  featureContent: { borderRadius: 16, padding: 16, marginTop: 4, marginBottom: 4, borderWidth: 1 },
  actionButton: { marginTop: 12, borderRadius: 12 },
  resultCard: { marginTop: 16, borderRadius: 12, padding: 16, borderWidth: 1 },
  resultTitle: { fontSize: 16, marginBottom: 8 },
  resultText: { fontSize: 14, color: '#475569', lineHeight: 22 },
  bulletRow: { flexDirection: 'row', alignItems: 'flex-start', gap: 10, marginTop: 8 },
  bulletText: { flex: 1, fontSize: 14, color: '#475569', lineHeight: 20 },
});
