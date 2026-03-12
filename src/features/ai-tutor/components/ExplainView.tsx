import React from 'react';
import { View, Text, StyleSheet, TextInput as RNTextInput } from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
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
  return (
    <View style={styles.featureContent}>
      <Text style={styles.inputLabel}>Ask a specific question (optional)</Text>
      <RNTextInput
        style={styles.textInput}
        placeholder={`e.g. "Why does ${topicName} matter?"`}
        placeholderTextColor="#94a3b8"
        value={question}
        onChangeText={onChangeQuestion}
        multiline
      />
      <Button
        mode="contained" buttonColor="#0284c7" onPress={onExplain}
        loading={loading} disabled={loading} icon="school"
        style={styles.actionButton}
      >
        {explainResult ? 'Ask Again' : question ? 'Ask AI Tutor' : 'Explain Topic'}
      </Button>

      {explainResult && (
        <View style={styles.resultCard}>
          <Text style={styles.resultTitle}>Explanation</Text>
          <Text style={styles.resultText}>{explainResult.explanation}</Text>

          {explainResult.examples.length > 0 && (
            <>
              <Text style={[styles.resultTitle, { marginTop: 16 }]}>Examples</Text>
              {explainResult.examples.map((ex: string, i: number) => (
                <View key={i} style={styles.bulletRow}>
                  <Ionicons name="bulb" size={18} color="#f59e0b" />
                  <Text style={styles.bulletText}>{ex}</Text>
                </View>
              ))}
            </>
          )}

          {explainResult.keyTakeaways.length > 0 && (
            <>
              <Text style={[styles.resultTitle, { marginTop: 16 }]}>Key Takeaways</Text>
              {explainResult.keyTakeaways.map((kt: string, i: number) => (
                <View key={i} style={styles.bulletRow}>
                  <Ionicons name="star" size={18} color="#0ea5e9" />
                  <Text style={styles.bulletText}>{kt}</Text>
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
  inputLabel: { fontSize: 13, fontWeight: '600', color: '#64748b', marginBottom: 6, marginTop: 8 },
  textInput: { borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#fff', paddingHorizontal: 16, paddingVertical: 12, fontSize: 15, color: '#1e293b', minHeight: 48 },
  resultCard: { marginTop: 16, backgroundColor: '#f8fafc', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#e2e8f0' },
  resultTitle: { fontSize: 16, fontWeight: '700', color: '#1e293b', marginBottom: 8 },
  resultText: { fontSize: 14, color: '#475569', lineHeight: 22 },
  bulletRow: { flexDirection: 'row', alignItems: 'flex-start', gap: 10, marginTop: 8 },
  bulletText: { flex: 1, fontSize: 14, color: '#475569', lineHeight: 20 },
});
