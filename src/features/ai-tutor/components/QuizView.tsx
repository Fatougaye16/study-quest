import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { QuizQuestionItem } from '../types';

interface Props {
  loading: boolean;
  quizQuestions: QuizQuestionItem[];
  quizDifficulty: number | null;
  onSetDifficulty: (d: number | null) => void;
  quizCount: number;
  onSetCount: (n: number) => void;
  currentQuestion: number;
  selectedAnswer: string | null;
  quizScore: number;
  quizFinished: boolean;
  answeredQuestions: Set<number>;
  onStart: () => void;
  onSelectAnswer: (answer: string) => void;
  onNextQuestion: () => void;
  onRetry: () => void;
}

export default function QuizView({
  loading, quizQuestions, quizDifficulty, onSetDifficulty,
  quizCount, onSetCount, currentQuestion, selectedAnswer,
  quizScore, quizFinished, answeredQuestions,
  onStart, onSelectAnswer, onNextQuestion, onRetry,
}: Props) {
  return (
    <View style={styles.featureContent}>
      {quizQuestions.length === 0 && !quizFinished && (
        <>
          <Text style={styles.inputLabel}>Difficulty</Text>
          <View style={styles.chipRow}>
            {([
              { label: 'Mixed', value: null },
              { label: 'Easy', value: 1 },
              { label: 'Medium', value: 2 },
              { label: 'Hard', value: 3 },
            ] as const).map(d => (
              <TouchableOpacity
                key={d.label}
                style={[styles.chip, quizDifficulty === d.value && styles.chipSelected]}
                onPress={() => onSetDifficulty(d.value)}
              >
                <Text style={[styles.chipText, quizDifficulty === d.value && styles.chipTextSelected]}>{d.label}</Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={styles.inputLabel}>Number of questions</Text>
          <View style={styles.chipRow}>
            {[5, 10, 15].map(n => (
              <TouchableOpacity
                key={n}
                style={[styles.chip, quizCount === n && styles.chipSelected]}
                onPress={() => onSetCount(n)}
              >
                <Text style={[styles.chipText, quizCount === n && styles.chipTextSelected]}>{n}</Text>
              </TouchableOpacity>
            ))}
          </View>

          <Button
            mode="contained" buttonColor="#f59e0b" onPress={onStart}
            loading={loading} disabled={loading} icon="help-circle-outline"
            style={styles.actionButton}
          >
            Start Quiz
          </Button>
        </>
      )}

      {quizQuestions.length > 0 && !quizFinished && (
        <View style={styles.quizArea}>
          <View style={styles.quizProgress}>
            <Text style={styles.quizProgressText}>
              Question {currentQuestion + 1} of {quizQuestions.length}
            </Text>
            <Text style={styles.quizScore}>Score: {quizScore}/{answeredQuestions.size}</Text>
          </View>

          <Text style={styles.quizQuestion}>{quizQuestions[currentQuestion].question}</Text>

          {quizQuestions[currentQuestion].options.map((opt: string, i: number) => {
            const answered = answeredQuestions.has(currentQuestion);
            const isCorrect = opt === quizQuestions[currentQuestion].correctAnswer;
            const isSelected = selectedAnswer === opt || (answered && opt === quizQuestions[currentQuestion].correctAnswer);
            let bgColor = '#ffffff';
            let borderColor = '#e2e8f0';
            if (answered) {
              if (isCorrect) { bgColor = '#dcfce7'; borderColor = '#10b981'; }
              else if (selectedAnswer === opt && !isCorrect) { bgColor = '#fef2f2'; borderColor = '#ef4444'; }
            } else if (selectedAnswer === opt) {
              borderColor = '#0ea5e9';
            }
            return (
              <TouchableOpacity
                key={i}
                style={[styles.quizOption, { backgroundColor: bgColor, borderColor }]}
                onPress={() => onSelectAnswer(opt)}
                disabled={answered}
              >
                <View style={[styles.optionLetter, { borderColor }]}>
                  <Text style={styles.optionLetterText}>{String.fromCharCode(65 + i)}</Text>
                </View>
                <Text style={styles.quizOptionText}>{opt}</Text>
                {answered && isCorrect && <Ionicons name="checkmark-circle" size={22} color="#10b981" />}
                {answered && selectedAnswer === opt && !isCorrect && <Ionicons name="close-circle" size={22} color="#ef4444" />}
              </TouchableOpacity>
            );
          })}

          {answeredQuestions.has(currentQuestion) && (
            <>
              <View style={styles.explanationBox}>
                <Ionicons name="bulb" size={18} color="#f59e0b" />
                <Text style={styles.explanationText}>{quizQuestions[currentQuestion].explanation}</Text>
              </View>
              <Button
                mode="contained" buttonColor="#0ea5e9" onPress={onNextQuestion}
                style={styles.actionButton}
              >
                {currentQuestion < quizQuestions.length - 1 ? 'Next Question' : 'See Results'}
              </Button>
            </>
          )}
        </View>
      )}

      {quizFinished && (
        <View style={styles.quizResults}>
          <Ionicons
            name={quizScore >= quizQuestions.length * 0.7 ? 'trophy' : 'ribbon'}
            size={48}
            color={quizScore >= quizQuestions.length * 0.7 ? '#fbbf24' : '#0ea5e9'}
          />
          <Text style={styles.quizResultsTitle}>Quiz Complete!</Text>
          <Text style={styles.quizResultsScore}>
            {quizScore} / {quizQuestions.length}
          </Text>
          <Text style={styles.quizResultsPercent}>
            {Math.round((quizScore / quizQuestions.length) * 100)}%
          </Text>
          <Text style={styles.quizResultsMsg}>
            {quizScore >= quizQuestions.length * 0.7
              ? 'Great job! Keep up the good work! 🎉'
              : 'Keep studying — you will get there! 💪'}
          </Text>
          <Button
            mode="contained" buttonColor="#0ea5e9"
            onPress={onRetry}
            style={styles.actionButton} icon="refresh"
          >
            Try Again
          </Button>
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
  quizArea: { marginTop: 8 },
  quizProgress: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 16 },
  quizProgressText: { fontSize: 13, fontWeight: '600', color: '#64748b' },
  quizScore: { fontSize: 13, fontWeight: '700', color: '#0ea5e9' },
  quizQuestion: { fontSize: 17, fontWeight: '700', color: '#1e293b', marginBottom: 16, lineHeight: 24 },
  quizOption: { flexDirection: 'row', alignItems: 'center', borderWidth: 2, borderRadius: 12, padding: 14, marginBottom: 10, gap: 12 },
  optionLetter: { width: 32, height: 32, borderRadius: 16, borderWidth: 2, justifyContent: 'center', alignItems: 'center' },
  optionLetterText: { fontSize: 14, fontWeight: '700', color: '#64748b' },
  quizOptionText: { flex: 1, fontSize: 15, color: '#1e293b' },
  explanationBox: { flexDirection: 'row', gap: 10, backgroundColor: '#fffbeb', borderRadius: 12, padding: 14, marginTop: 8, marginBottom: 4, alignItems: 'flex-start' },
  explanationText: { flex: 1, fontSize: 13, color: '#92400e', lineHeight: 20 },
  quizResults: { alignItems: 'center', paddingVertical: 24 },
  quizResultsTitle: { fontSize: 22, fontWeight: 'bold', color: '#1e293b', marginTop: 12 },
  quizResultsScore: { fontSize: 48, fontWeight: 'bold', color: '#0ea5e9', marginTop: 8 },
  quizResultsPercent: { fontSize: 18, color: '#64748b', marginTop: 4 },
  quizResultsMsg: { fontSize: 16, color: '#475569', marginTop: 12, textAlign: 'center' },
});
