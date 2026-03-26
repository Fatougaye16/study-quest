import React from 'react';
import { View, Text, StyleSheet, TextInput as RNTextInput } from 'react-native';
import { Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import { ExplainResponse } from '../types';

interface Props {
  loading: boolean;
  explainResult: ExplainResponse | null;
  question: string;
  onChangeQuestion: (text: string) => void;
  onExplain: () => void;
  topicName: string;
}

export default function ExplainView({ loading, explainResult, question, onChangeQuestion, onExplain, topicName }: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <View style={[styles.featureContent, { backgroundColor: colors.card, borderColor: colors.border }]}>
      <Text style={[styles.inputLabel, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>Ask a specific question (optional)</Text>
      <RNTextInput
        style={[styles.textInput, { borderColor: colors.border, backgroundColor: colors.card, color: colors.text, fontFamily: theme.fonts.body }]}
        placeholder={`e.g. "Why does ${topicName} matter?"`}
        placeholderTextColor={colors.textTertiary}
        value={question}
        onChangeText={onChangeQuestion}
        multiline
      />
      <Button
        mode="contained" buttonColor={colors.primary} onPress={onExplain}
        loading={loading} disabled={loading} icon="school"
        style={styles.actionButton}
      >
        {explainResult ? 'Ask Again' : question ? 'Ask AI Tutor' : 'Explain Topic'}
      </Button>

      {explainResult && (
        <View style={[styles.resultCard, { backgroundColor: colors.surface, borderColor: colors.border }]}>
          <Text style={[styles.resultTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Explanation</Text>
          <Text style={[styles.resultText, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>{explainResult.explanation}</Text>

          {explainResult.examples.length > 0 && (
            <>
              <Text style={[styles.resultTitle, { color: colors.text, fontFamily: theme.fonts.headingBold, marginTop: 16 }]}>Examples</Text>
              {explainResult.examples.map((ex: string, i: number) => (
                <View key={i} style={styles.bulletRow}>
                  <Feather name="zap" size={18} color={colors.accent} />
                  <Text style={[styles.bulletText, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>{ex}</Text>
                </View>
              ))}
            </>
          )}

          {explainResult.keyTakeaways.length > 0 && (
            <>
              <Text style={[styles.resultTitle, { color: colors.text, fontFamily: theme.fonts.headingBold, marginTop: 16 }]}>Key Takeaways</Text>
              {explainResult.keyTakeaways.map((kt: string, i: number) => (
                <View key={i} style={styles.bulletRow}>
                  <Feather name="star" size={18} color={colors.primary} />
                  <Text style={[styles.bulletText, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>{kt}</Text>
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
  inputLabel: { fontSize: 13, marginBottom: 6, marginTop: 8 },
  textInput: { borderWidth: 2, borderRadius: 12, paddingHorizontal: 16, paddingVertical: 12, fontSize: 15, minHeight: 48 },
  resultCard: { marginTop: 16, borderRadius: 12, padding: 16, borderWidth: 1 },
  resultTitle: { fontSize: 16, marginBottom: 8 },
  resultText: { fontSize: 14, lineHeight: 22 },
  bulletRow: { flexDirection: 'row', alignItems: 'flex-start', gap: 10, marginTop: 8 },
  bulletText: { flex: 1, fontSize: 14, lineHeight: 20 },
});
