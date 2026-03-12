import React, { useState, useEffect, useCallback } from 'react';
import {
  View, Text, StyleSheet, ScrollView, TouchableOpacity,
  RefreshControl, Alert,
} from 'react-native';
import { Ionicons } from '@expo/vector-icons';
import { aiAPI } from './api';
import { SummarizeResponse, FlashcardItem, QuizQuestionItem, ExplainResponse } from './types';
import { enrollmentsAPI, subjectsAPI } from '../courses/api';
import { Enrollment, Topic } from '../courses/types';
import SummarizeView from './components/SummarizeView';
import ExplainView from './components/ExplainView';
import FlashcardsView from './components/FlashcardsView';
import QuizView from './components/QuizView';
import StudyPlanGenView from './components/StudyPlanGenView';

type Feature = 'summarize' | 'explain' | 'flashcards' | 'quiz' | 'studyPlan' | null;

export default function AITutorScreen() {
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [selectedSubject, setSelectedSubject] = useState('');
  const [showSubjectPicker, setShowSubjectPicker] = useState(false);
  const [topics, setTopics] = useState<Topic[]>([]);
  const [selectedTopic, setSelectedTopic] = useState('');
  const [showTopicPicker, setShowTopicPicker] = useState(false);
  const [refreshing, setRefreshing] = useState(false);

  const [activeFeature, setActiveFeature] = useState<Feature>(null);
  const [loading, setLoading] = useState(false);

  const [summary, setSummary] = useState<SummarizeResponse | null>(null);

  const [explainResult, setExplainResult] = useState<ExplainResponse | null>(null);
  const [question, setQuestion] = useState('');

  const [flashcards, setFlashcards] = useState<FlashcardItem[]>([]);
  const [cardCount, setCardCount] = useState(10);
  const [flippedCards, setFlippedCards] = useState<Set<number>>(new Set());
  const [currentCard, setCurrentCard] = useState(0);

  const [quizQuestions, setQuizQuestions] = useState<QuizQuestionItem[]>([]);
  const [quizDifficulty, setQuizDifficulty] = useState<number | null>(null);
  const [quizCount, setQuizCount] = useState(5);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState<string | null>(null);
  const [quizScore, setQuizScore] = useState(0);
  const [quizFinished, setQuizFinished] = useState(false);
  const [answeredQuestions, setAnsweredQuestions] = useState<Set<number>>(new Set());

  const [planDays, setPlanDays] = useState(14);
  const [planCreated, setPlanCreated] = useState(false);

  const loadEnrollments = useCallback(async () => {
    try {
      const { data } = await enrollmentsAPI.getAll();
      setEnrollments(data);
    } catch (e) {
      console.error('Failed to load enrollments:', e);
    }
  }, []);

  useEffect(() => { loadEnrollments(); }, [loadEnrollments]);

  useEffect(() => {
    if (!selectedSubject) { setTopics([]); setSelectedTopic(''); return; }
    (async () => {
      try {
        const { data } = await subjectsAPI.getTopics(selectedSubject);
        setTopics(data);
        setSelectedTopic('');
      } catch (e) { console.error('Failed to load topics:', e); }
    })();
  }, [selectedSubject]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadEnrollments();
    setRefreshing(false);
  }, [loadEnrollments]);

  const resetResults = () => {
    setSummary(null);
    setExplainResult(null);
    setFlashcards([]);
    setFlippedCards(new Set());
    setCurrentCard(0);
    setQuizQuestions([]);
    setCurrentQuestion(0);
    setSelectedAnswer(null);
    setQuizScore(0);
    setQuizFinished(false);
    setAnsweredQuestions(new Set());
    setPlanCreated(false);
    setQuestion('');
  };

  const toggleFeature = (f: Feature) => {
    if (activeFeature === f) { setActiveFeature(null); return; }
    resetResults();
    setActiveFeature(f);
  };

  const selectedTopicName = topics.find(t => t.id === selectedTopic)?.name ?? '';

  const handleSummarize = async () => {
    if (!selectedTopic) return;
    setLoading(true);
    try {
      const { data } = await aiAPI.summarize(selectedTopic);
      setSummary(data);
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Summarize failed';
      Alert.alert('Summarize Error', msg);
    } finally { setLoading(false); }
  };

  const handleExplain = async () => {
    if (!selectedTopic) return;
    setLoading(true);
    try {
      const { data } = await aiAPI.explain(selectedTopic, question || undefined);
      setExplainResult(data);
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Explain failed';
      Alert.alert('Explain Error', msg);
    } finally { setLoading(false); }
  };

  const handleFlashcards = async () => {
    if (!selectedTopic) return;
    setLoading(true);
    setFlippedCards(new Set());
    setCurrentCard(0);
    try {
      const { data } = await aiAPI.flashcards(selectedTopic, undefined, cardCount);
      setFlashcards(data.flashcards);
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Flashcard generation failed';
      Alert.alert('Flashcards Error', msg);
    } finally { setLoading(false); }
  };

  const handleQuiz = async () => {
    if (!selectedTopic) return;
    setLoading(true);
    setCurrentQuestion(0);
    setSelectedAnswer(null);
    setQuizScore(0);
    setQuizFinished(false);
    setAnsweredQuestions(new Set());
    try {
      const { data } = await aiAPI.quiz(selectedTopic, quizDifficulty ?? undefined, quizCount);
      setQuizQuestions(data.questions);
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Quiz generation failed';
      Alert.alert('Quiz Error', msg);
    } finally { setLoading(false); }
  };

  const handleStudyPlan = async () => {
    if (!selectedSubject) return;
    setLoading(true);
    setPlanCreated(false);
    try {
      const topicIds = topics.map(t => t.id);
      await aiAPI.studyPlan(selectedSubject, topicIds.length > 0 ? topicIds : undefined, planDays);
      setPlanCreated(true);
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Study plan generation failed';
      Alert.alert('Study Plan Error', msg);
    } finally { setLoading(false); }
  };

  const handleSelectAnswer = (answer: string) => {
    if (answeredQuestions.has(currentQuestion)) return;
    setSelectedAnswer(answer);
    const correct = quizQuestions[currentQuestion].correctAnswer === answer;
    if (correct) setQuizScore(prev => prev + 1);
    setAnsweredQuestions(prev => new Set(prev).add(currentQuestion));
  };

  const handleNextQuestion = () => {
    if (currentQuestion < quizQuestions.length - 1) {
      setCurrentQuestion(prev => prev + 1);
      setSelectedAnswer(null);
    } else {
      setQuizFinished(true);
    }
  };

  const toggleFlip = () => {
    setFlippedCards(prev => {
      const next = new Set(prev);
      if (next.has(currentCard)) next.delete(currentCard);
      else next.add(currentCard);
      return next;
    });
  };

  const FEATURES: { key: Feature; icon: string; label: string; color: string; desc: string; needsTopic: boolean }[] = [
    { key: 'summarize', icon: 'document-text', label: 'Summarize', color: '#0ea5e9', desc: 'Get a concise summary with key points', needsTopic: true },
    { key: 'explain', icon: 'school', label: 'AI Tutor', color: '#0284c7', desc: 'Ask questions & get explanations', needsTopic: true },
    { key: 'flashcards', icon: 'layers', label: 'Flashcards', color: '#0ea5e9', desc: 'Generate study flashcards', needsTopic: true },
    { key: 'quiz', icon: 'help-circle', label: 'Quiz', color: '#f59e0b', desc: 'Test your knowledge', needsTopic: true },
    { key: 'studyPlan', icon: 'calendar', label: 'AI Study Plan', color: '#10b981', desc: 'Generate a personalized study plan', needsTopic: false },
  ];

  return (
    <View style={styles.container}>
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
      >
        <View style={styles.heroBanner}>
          <Ionicons name="sparkles" size={32} color="#fbbf24" />
          <Text style={styles.heroTitle}>AI Study Tools</Text>
          <Text style={styles.heroSubtitle}>
            Select a subject and topic, then choose a tool below
          </Text>
        </View>

        <Text style={styles.sectionLabel}>Subject</Text>
        <TouchableOpacity
          style={styles.pickerButton}
          onPress={() => { setShowSubjectPicker(!showSubjectPicker); setShowTopicPicker(false); }}
        >
          <Text style={selectedSubject ? styles.pickerText : styles.pickerPlaceholder}>
            {selectedSubject
              ? enrollments.find(e => e.subjectId === selectedSubject)?.subjectName
              : 'Select an enrolled subject'}
          </Text>
          <Ionicons name={showSubjectPicker ? 'chevron-up' : 'chevron-down'} size={20} color="#94a3b8" />
        </TouchableOpacity>
        {showSubjectPicker && (
          <View style={styles.optionList}>
            {enrollments.map(e => (
              <TouchableOpacity
                key={e.subjectId}
                style={[styles.optionItem, selectedSubject === e.subjectId && styles.optionItemSelected]}
                onPress={() => { setSelectedSubject(e.subjectId); setShowSubjectPicker(false); resetResults(); setActiveFeature(null); }}
              >
                <View style={[styles.optionDot, { backgroundColor: e.subjectColor || '#0ea5e9' }]} />
                <Text style={[styles.optionText, selectedSubject === e.subjectId && styles.optionTextSelected]}>
                  {e.subjectName}
                </Text>
                {selectedSubject === e.subjectId && <Ionicons name="checkmark" size={18} color="#0ea5e9" />}
              </TouchableOpacity>
            ))}
            {enrollments.length === 0 && (
              <Text style={styles.optionEmpty}>No enrolled subjects. Enroll in a course first.</Text>
            )}
          </View>
        )}

        {selectedSubject && (
          <>
            <Text style={styles.sectionLabel}>Topic</Text>
            <TouchableOpacity
              style={styles.pickerButton}
              onPress={() => { setShowTopicPicker(!showTopicPicker); setShowSubjectPicker(false); }}
            >
              <Text style={selectedTopic ? styles.pickerText : styles.pickerPlaceholder}>
                {selectedTopic ? selectedTopicName : 'Select a topic'}
              </Text>
              <Ionicons name={showTopicPicker ? 'chevron-up' : 'chevron-down'} size={20} color="#94a3b8" />
            </TouchableOpacity>
            {showTopicPicker && (
              <View style={styles.optionList}>
                {topics.map(t => (
                  <TouchableOpacity
                    key={t.id}
                    style={[styles.optionItem, selectedTopic === t.id && styles.optionItemSelected]}
                    onPress={() => { setSelectedTopic(t.id); setShowTopicPicker(false); resetResults(); }}
                  >
                    <Text style={[styles.optionText, selectedTopic === t.id && styles.optionTextSelected]}>
                      {t.name}
                    </Text>
                    {selectedTopic === t.id && <Ionicons name="checkmark" size={18} color="#0ea5e9" />}
                  </TouchableOpacity>
                ))}
                {topics.length === 0 && (
                  <Text style={styles.optionEmpty}>No topics available for this subject.</Text>
                )}
              </View>
            )}
          </>
        )}

        {selectedSubject && (
          <View style={styles.featureGrid}>
            {FEATURES.map(f => {
              const disabled = f.needsTopic && !selectedTopic;
              const isActive = activeFeature === f.key;
              return (
                <View key={f.key}>
                  <TouchableOpacity
                    style={[
                      styles.featureCard,
                      { borderLeftColor: f.color },
                      isActive && styles.featureCardActive,
                      disabled && styles.featureCardDisabled,
                    ]}
                    onPress={() => !disabled && toggleFeature(f.key)}
                    activeOpacity={disabled ? 1 : 0.7}
                  >
                    <View style={[styles.featureIconWrap, { backgroundColor: f.color + '18' }]}>
                      <Ionicons name={f.icon as any} size={24} color={f.color} />
                    </View>
                    <View style={styles.featureInfo}>
                      <Text style={[styles.featureLabel, disabled && { color: '#cbd5e1' }]}>{f.label}</Text>
                      <Text style={[styles.featureDesc, disabled && { color: '#e2e8f0' }]}>{f.desc}</Text>
                    </View>
                    <Ionicons name={isActive ? 'chevron-up' : 'chevron-forward'} size={20} color={disabled ? '#e2e8f0' : '#94a3b8'} />
                  </TouchableOpacity>

                  {isActive && renderFeatureContent(f.key)}
                </View>
              );
            })}
          </View>
        )}

        {!selectedSubject && (
          <View style={styles.emptyState}>
            <Ionicons name="sparkles-outline" size={64} color="#cbd5e1" />
            <Text style={styles.emptyText}>Select a subject above to get started</Text>
            <Text style={styles.emptySubtext}>AI tools will help you study smarter</Text>
          </View>
        )}

        <View style={{ height: 40 }} />
      </ScrollView>
    </View>
  );

  function renderFeatureContent(feature: Feature) {
    switch (feature) {
      case 'summarize':
        return <SummarizeView loading={loading} summary={summary} onSummarize={handleSummarize} />;
      case 'explain':
        return (
          <ExplainView
            loading={loading} explainResult={explainResult}
            question={question} onChangeQuestion={setQuestion}
            onExplain={handleExplain} topicName={selectedTopicName}
          />
        );
      case 'flashcards':
        return (
          <FlashcardsView
            loading={loading} flashcards={flashcards} cardCount={cardCount}
            onSetCardCount={setCardCount} currentCard={currentCard}
            flippedCards={flippedCards} onGenerate={handleFlashcards}
            onFlip={toggleFlip}
            onPrev={() => { if (currentCard > 0) setCurrentCard(prev => prev - 1); }}
            onNext={() => { if (currentCard < flashcards.length - 1) setCurrentCard(prev => prev + 1); }}
          />
        );
      case 'quiz':
        return (
          <QuizView
            loading={loading} quizQuestions={quizQuestions}
            quizDifficulty={quizDifficulty} onSetDifficulty={setQuizDifficulty}
            quizCount={quizCount} onSetCount={setQuizCount}
            currentQuestion={currentQuestion} selectedAnswer={selectedAnswer}
            quizScore={quizScore} quizFinished={quizFinished}
            answeredQuestions={answeredQuestions}
            onStart={handleQuiz} onSelectAnswer={handleSelectAnswer}
            onNextQuestion={handleNextQuestion}
            onRetry={() => { setQuizQuestions([]); setQuizFinished(false); setCurrentQuestion(0); setSelectedAnswer(null); setQuizScore(0); setAnsweredQuestions(new Set()); }}
          />
        );
      case 'studyPlan':
        return (
          <StudyPlanGenView
            loading={loading} planDays={planDays} onSetPlanDays={setPlanDays}
            planCreated={planCreated} onGenerate={handleStudyPlan}
          />
        );
      default: return null;
    }
  }
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  scrollView: { flex: 1, padding: 16 },

  heroBanner: { alignItems: 'center', paddingVertical: 24, marginBottom: 8 },
  heroTitle: { fontSize: 26, fontWeight: 'bold', color: '#1e293b', marginTop: 8 },
  heroSubtitle: { fontSize: 14, color: '#64748b', marginTop: 4, textAlign: 'center' },

  sectionLabel: { fontSize: 14, fontWeight: '600', color: '#1e293b', marginBottom: 8, marginTop: 12 },

  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#fff', paddingHorizontal: 16, paddingVertical: 16, minHeight: 56 },
  pickerText: { fontSize: 16, color: '#1e293b', fontWeight: '500' },
  pickerPlaceholder: { fontSize: 16, color: '#94a3b8' },
  optionList: { borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#fff', overflow: 'hidden', marginBottom: 8 },
  optionItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingVertical: 14, borderBottomWidth: 1, borderBottomColor: '#f1f5f9' },
  optionItemSelected: { backgroundColor: '#f0f9ff' },
  optionDot: { width: 10, height: 10, borderRadius: 5, marginRight: 12 },
  optionText: { flex: 1, fontSize: 15, color: '#1e293b' },
  optionTextSelected: { fontWeight: '600', color: '#0ea5e9' },
  optionEmpty: { padding: 16, color: '#94a3b8', fontStyle: 'italic', textAlign: 'center' },

  featureGrid: { marginTop: 20, gap: 10 },
  featureCard: { flexDirection: 'row', alignItems: 'center', backgroundColor: '#fff', borderRadius: 14, padding: 16, borderLeftWidth: 4, elevation: 1, shadowColor: '#000', shadowOffset: { width: 0, height: 1 }, shadowOpacity: 0.05, shadowRadius: 3 },
  featureCardActive: { borderColor: '#0ea5e9', elevation: 3, shadowOpacity: 0.1 },
  featureCardDisabled: { opacity: 0.5 },
  featureIconWrap: { width: 44, height: 44, borderRadius: 12, justifyContent: 'center', alignItems: 'center', marginRight: 14 },
  featureInfo: { flex: 1 },
  featureLabel: { fontSize: 16, fontWeight: '700', color: '#1e293b' },
  featureDesc: { fontSize: 12, color: '#64748b', marginTop: 2 },

  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 60 },
  emptyText: { fontSize: 18, fontWeight: 'bold', color: '#64748b', marginTop: 16 },
  emptySubtext: { fontSize: 14, color: '#94a3b8', marginTop: 4 },
});
