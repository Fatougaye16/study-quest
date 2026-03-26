import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import { FlashcardItem } from '../types';

interface Props {
  loading: boolean;
  flashcards: FlashcardItem[];
  cardCount: number;
  onSetCardCount: (n: number) => void;
  currentCard: number;
  flippedCards: Set<number>;
  onGenerate: () => void;
  onFlip: () => void;
  onPrev: () => void;
  onNext: () => void;
}

export default function FlashcardsView({
  loading, flashcards, cardCount, onSetCardCount,
  currentCard, flippedCards, onGenerate, onFlip, onPrev, onNext,
}: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  return (
    <View style={[styles.featureContent, { backgroundColor: colors.card, borderColor: colors.border }]}>
      <Text style={[styles.inputLabel, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>Number of cards</Text>
      <View style={styles.chipRow}>
        {[5, 10, 15].map(n => (
          <TouchableOpacity
            key={n}
            style={[styles.chip, { borderColor: colors.border, backgroundColor: colors.card }, cardCount === n && { borderColor: colors.primary, backgroundColor: colors.primary + '10' }]}
            onPress={() => onSetCardCount(n)}
          >
            <Text style={[{ fontSize: 14, color: colors.textSecondary, fontFamily: theme.fonts.bodyMedium }, cardCount === n && { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>{n}</Text>
          </TouchableOpacity>
        ))}
      </View>

      <Button
        mode="contained" buttonColor={colors.primary} onPress={onGenerate}
        loading={loading} disabled={loading} icon="layers-outline"
        style={styles.actionButton}
      >
        {flashcards.length > 0 ? 'Regenerate' : 'Generate Flashcards'}
      </Button>

      {flashcards.length > 0 && (
        <View style={styles.flashcardArea}>
          <Text style={[styles.cardCounter, { color: colors.textSecondary, fontFamily: theme.fonts.bodySemiBold }]}>{currentCard + 1} / {flashcards.length}</Text>

          <TouchableOpacity style={styles.flashcard} onPress={onFlip} activeOpacity={0.8}>
            <View style={[styles.flashcardInner, { backgroundColor: colors.primary }, flippedCards.has(currentCard) && { backgroundColor: colors.secondary }]}>
              <Text style={[styles.flashcardSide, { fontFamily: theme.fonts.headingBold }]}>{flippedCards.has(currentCard) ? 'ANSWER' : 'QUESTION'}</Text>
              <Text style={[styles.flashcardText, { fontFamily: theme.fonts.bodySemiBold }]}>
                {flippedCards.has(currentCard) ? flashcards[currentCard].back : flashcards[currentCard].front}
              </Text>
              <Text style={styles.flashcardHint}>Tap to flip</Text>
            </View>
          </TouchableOpacity>

          <View style={styles.cardNav}>
            <TouchableOpacity
              style={[styles.cardNavBtn, { backgroundColor: colors.card, borderColor: colors.border }, currentCard === 0 && styles.cardNavBtnDisabled]}
              onPress={onPrev}
            >
              <Feather name="chevron-left" size={24} color={currentCard === 0 ? colors.border : colors.primary} />
            </TouchableOpacity>
            <TouchableOpacity
              style={[styles.cardNavBtn, { backgroundColor: colors.card, borderColor: colors.border }, currentCard === flashcards.length - 1 && styles.cardNavBtnDisabled]}
              onPress={onNext}
            >
              <Feather name="chevron-right" size={24} color={currentCard === flashcards.length - 1 ? colors.border : colors.primary} />
            </TouchableOpacity>
          </View>
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
  flashcardArea: { marginTop: 16, alignItems: 'center' },
  cardCounter: { fontSize: 14, marginBottom: 12 },
  flashcard: { width: '100%', minHeight: 200, borderRadius: 16, overflow: 'hidden' },
  flashcardInner: { flex: 1, borderRadius: 16, padding: 24, justifyContent: 'center', alignItems: 'center', minHeight: 200 },
  flashcardSide: { fontSize: 11, color: 'rgba(255,255,255,0.6)', letterSpacing: 2, marginBottom: 12 },
  flashcardText: { fontSize: 18, color: '#fff', textAlign: 'center', lineHeight: 26 },
  flashcardHint: { fontSize: 12, color: 'rgba(255,255,255,0.5)', marginTop: 16 },
  cardNav: { flexDirection: 'row', gap: 24, marginTop: 16 },
  cardNavBtn: { width: 48, height: 48, borderRadius: 24, justifyContent: 'center', alignItems: 'center', borderWidth: 2 },
  cardNavBtnDisabled: { opacity: 0.4 },
});
