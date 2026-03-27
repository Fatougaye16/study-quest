import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
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
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <View style={[styles.featureContent, { backgroundColor: colors.card, borderColor: colors.border }]}>
      {quizQuestions.length === 0 && !quizFinished && (
        <>
          <Text style={[styles.inputLabel, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>Difficulty</Text>
          <View style={styles.chipRow}>
            {([
              { label: 'Mixed', value: null },
              { label: 'Paper 1', value: 1 },
              { label: 'Paper 2 (Structured)', value: 2 },
              { label: 'Paper 2 (Essay)', value: 3 },
            ] as const).map(d => (
              <TouchableOpacity
                key={d.label}
                style={[
                  styles.chip,
                  { borderColor: colors.border, backgroundColor: colors.card },
                  quizDifficulty === d.value && { borderColor: colors.primary, backgroundColor: colors.primary + '10' },
                ]}
                onPress={() => onSetDifficulty(d.value)}
              >
                <Text style={[
                  styles.chipText,
                  { color: colors.textSecondary, fontFamily: theme.fonts.bodyMedium },
                  quizDifficulty === d.value && { color: colors.primary, fontFamily: theme.fonts.headingBold },
                ]}>{d.label}</Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={[styles.inputLabel, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>Number of questions</Text>
          <View style={styles.chipRow}>
            {[5, 10, 15].map(n => (
              <TouchableOpacity
                key={n}
                style={[
                  styles.chip,
                  { borderColor: colors.border, backgroundColor: colors.card },
                  quizCount === n && { borderColor: colors.primary, backgroundColor: colors.primary + '10' },
                ]}
                onPress={() => onSetCount(n)}
              >
                <Text style={[
                  styles.chipText,
                  { color: colors.textSecondary, fontFamily: theme.fonts.bodyMedium },
                  quizCount === n && { color: colors.primary, fontFamily: theme.fonts.headingBold },
                ]}>{n}</Text>
              </TouchableOpacity>
            ))}
          </View>

          <Button
            mode="contained" buttonColor={colors.accent} onPress={onStart}
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
            <Text style={[styles.quizProgressText, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>
              Question {currentQuestion + 1} of {quizQuestions.length}
            </Text>
            <Text style={[styles.quizScore, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>Score: {quizScore}/{answeredQuestions.size}</Text>
          </View>

          <Text style={[styles.quizQuestion, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{quizQuestions[currentQuestion].question}</Text>

          {quizQuestions[currentQuestion].options.map((opt: string, i: number) => {
            const answered = answeredQuestions.has(currentQuestion);
            const isCorrect = opt.trim().toLowerCase() === quizQuestions[currentQuestion].correctAnswer.trim().toLowerCase();
            const isSelected = selectedAnswer === opt || (answered && opt === quizQuestions[currentQuestion].correctAnswer);
            let bgColor = colors.card;
            let borderColor = colors.border;
            if (answered) {
              if (isCorrect) { bgColor = colors.gamification.xp + '20'; borderColor = colors.gamification.xp; }
              else if (selectedAnswer === opt && !isCorrect) { bgColor = colors.error + '15'; borderColor = colors.error; }
            } else if (selectedAnswer === opt) {
              borderColor = colors.primary;
            }
            return (
              <TouchableOpacity
                key={i}
                style={[styles.quizOption, { backgroundColor: bgColor, borderColor }]}
                onPress={() => onSelectAnswer(opt)}
                disabled={answered}
              >
                <View style={[styles.optionLetter, { borderColor }]}>
                  <Text style={[styles.optionLetterText, { color: colors.textSecondary, fontFamily: theme.fonts.headingBold }]}>{String.fromCharCode(65 + i)}</Text>
                </View>
                <Text style={[styles.quizOptionText, { color: colors.text, fontFamily: theme.fonts.body }]}>{opt}</Text>
                {answered && isCorrect && <Feather name="check-circle" size={22} color={colors.gamification.xp} />}
                {answered && selectedAnswer === opt && !isCorrect && <Feather name="x-circle" size={22} color={colors.error} />}
              </TouchableOpacity>
            );
          })}

          {answeredQuestions.has(currentQuestion) && (
            <>
              <View style={[styles.explanationBox, { backgroundColor: colors.accent + '15' }]}>
                <Feather name="zap" size={18} color={colors.accent} />
                <Text style={[styles.explanationText, { color: colors.accent, fontFamily: theme.fonts.body }]}>{quizQuestions[currentQuestion].explanation}</Text>
              </View>
              <Button
                mode="contained" buttonColor={colors.primary} onPress={onNextQuestion}
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
          <Feather
            name="award"
            size={48}
            color={quizScore >= quizQuestions.length * 0.7 ? colors.accent : colors.primary}
          />
          <Text style={[styles.quizResultsTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Quiz Complete!</Text>
          <Text style={[styles.quizResultsScore, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>
            {quizScore} / {quizQuestions.length}
          </Text>
          <Text style={[styles.quizResultsPercent, { color: colors.textSecondary, fontFamily: theme.fonts.bodyMedium }]}>
            {Math.round((quizScore / quizQuestions.length) * 100)}%
          </Text>
          <Text style={[styles.quizResultsMsg, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
            {quizScore >= quizQuestions.length * 0.7
              ? 'Great job! Keep up the good work! 🎉'
              : 'Keep studying — you will get there! 💪'}
          </Text>
          <Button
            mode="contained" buttonColor={colors.primary}
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
  featureContent: { borderRadius: 16, padding: 16, marginTop: 4, marginBottom: 4, borderWidth: 1 },
  actionButton: { marginTop: 12, borderRadius: 12 },
  inputLabel: { fontSize: 13, marginBottom: 6, marginTop: 8 },
  chipRow: { flexDirection: 'row', gap: 8, marginBottom: 4 },
  chip: { paddingHorizontal: 16, paddingVertical: 10, borderRadius: 20, borderWidth: 2 },
  chipText: { fontSize: 14 },
  quizArea: { marginTop: 8 },
  quizProgress: { flexDirection: 'row', justifyContent: 'space-between', marginBottom: 16 },
  quizProgressText: { fontSize: 13 },
  quizScore: { fontSize: 13 },
  quizQuestion: { fontSize: 17, marginBottom: 16, lineHeight: 24 },
  quizOption: { flexDirection: 'row', alignItems: 'center', borderWidth: 2, borderRadius: 12, padding: 14, marginBottom: 10, gap: 12 },
  optionLetter: { width: 32, height: 32, borderRadius: 16, borderWidth: 2, justifyContent: 'center', alignItems: 'center' },
  optionLetterText: { fontSize: 14 },
  quizOptionText: { flex: 1, fontSize: 15 },
  explanationBox: { flexDirection: 'row', gap: 10, borderRadius: 12, padding: 14, marginTop: 8, marginBottom: 4, alignItems: 'flex-start' },
  explanationText: { flex: 1, fontSize: 13, lineHeight: 20 },
  quizResults: { alignItems: 'center', paddingVertical: 24 },
  quizResultsTitle: { fontSize: 22, marginTop: 12 },
  quizResultsScore: { fontSize: 48, marginTop: 8 },
  quizResultsPercent: { fontSize: 18, marginTop: 4 },
  quizResultsMsg: { fontSize: 16, marginTop: 12, textAlign: 'center' },
});
