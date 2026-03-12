import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
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
  return (
    <View style={styles.featureContent}>
      <Text style={styles.inputLabel}>Number of cards</Text>
      <View style={styles.chipRow}>
        {[5, 10, 15].map(n => (
          <TouchableOpacity
            key={n}
            style={[styles.chip, cardCount === n && styles.chipSelected]}
            onPress={() => onSetCardCount(n)}
          >
            <Text style={[styles.chipText, cardCount === n && styles.chipTextSelected]}>{n}</Text>
          </TouchableOpacity>
        ))}
      </View>

      <Button
        mode="contained" buttonColor="#0ea5e9" onPress={onGenerate}
        loading={loading} disabled={loading} icon="layers-outline"
        style={styles.actionButton}
      >
        {flashcards.length > 0 ? 'Regenerate' : 'Generate Flashcards'}
      </Button>

      {flashcards.length > 0 && (
        <View style={styles.flashcardArea}>
          <Text style={styles.cardCounter}>{currentCard + 1} / {flashcards.length}</Text>

          <TouchableOpacity style={styles.flashcard} onPress={onFlip} activeOpacity={0.8}>
            <View style={[styles.flashcardInner, flippedCards.has(currentCard) && styles.flashcardFlipped]}>
              <Text style={styles.flashcardSide}>{flippedCards.has(currentCard) ? 'ANSWER' : 'QUESTION'}</Text>
              <Text style={styles.flashcardText}>
                {flippedCards.has(currentCard) ? flashcards[currentCard].back : flashcards[currentCard].front}
              </Text>
              <Text style={styles.flashcardHint}>Tap to flip</Text>
            </View>
          </TouchableOpacity>

          <View style={styles.cardNav}>
            <TouchableOpacity
              style={[styles.cardNavBtn, currentCard === 0 && styles.cardNavBtnDisabled]}
              onPress={onPrev}
            >
              <Ionicons name="chevron-back" size={24} color={currentCard === 0 ? '#e2e8f0' : '#0ea5e9'} />
            </TouchableOpacity>
            <TouchableOpacity
              style={[styles.cardNavBtn, currentCard === flashcards.length - 1 && styles.cardNavBtnDisabled]}
              onPress={onNext}
            >
              <Ionicons name="chevron-forward" size={24} color={currentCard === flashcards.length - 1 ? '#e2e8f0' : '#0ea5e9'} />
            </TouchableOpacity>
          </View>
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
  flashcardArea: { marginTop: 16, alignItems: 'center' },
  cardCounter: { fontSize: 14, fontWeight: '600', color: '#64748b', marginBottom: 12 },
  flashcard: { width: '100%', minHeight: 200, borderRadius: 16, overflow: 'hidden' },
  flashcardInner: { flex: 1, backgroundColor: '#0ea5e9', borderRadius: 16, padding: 24, justifyContent: 'center', alignItems: 'center', minHeight: 200 },
  flashcardFlipped: { backgroundColor: '#0284c7' },
  flashcardSide: { fontSize: 11, fontWeight: '700', color: 'rgba(255,255,255,0.6)', letterSpacing: 2, marginBottom: 12 },
  flashcardText: { fontSize: 18, color: '#fff', fontWeight: '600', textAlign: 'center', lineHeight: 26 },
  flashcardHint: { fontSize: 12, color: 'rgba(255,255,255,0.5)', marginTop: 16 },
  cardNav: { flexDirection: 'row', gap: 24, marginTop: 16 },
  cardNavBtn: { width: 48, height: 48, borderRadius: 24, backgroundColor: '#fff', justifyContent: 'center', alignItems: 'center', borderWidth: 2, borderColor: '#e2e8f0' },
  cardNavBtnDisabled: { opacity: 0.4 },
});
