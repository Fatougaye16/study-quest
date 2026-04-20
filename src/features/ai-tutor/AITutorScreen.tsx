import React, { useState, useEffect, useCallback } from 'react';
import {
  View, Text, StyleSheet, ScrollView, TouchableOpacity,
  RefreshControl, Alert,
} from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useNavigation, useRoute } from '@react-navigation/native';
import { useTheme } from '../../shared/theme';
import { aiAPI } from './api';
import { SummarizeResponse, FlashcardItem, QuizQuestionItem, ExplainResponse } from './types';
import { enrollmentsAPI, subjectsAPI } from '../courses/api';
import { Enrollment, Topic } from '../courses/types';
import SummarizeView from './components/SummarizeView';
import ExplainView from './components/ExplainView';
import FlashcardsView from './components/FlashcardsView';
import QuizView from './components/QuizView';
import StudyPlanGenView from './components/StudyPlanGenView';
import UploadContentSheet from '../courses/components/UploadContentSheet';
import AfricanPattern from '../../shared/components/AfricanPattern';

type Feature = 'summarize' | 'explain' | 'flashcards' | 'quiz' | 'studyPlan' | null;

export default function AITutorScreen() {
  const { theme } = useTheme();
  const colors = theme.colors;
  const navigation = useNavigation<any>();
  const route = useRoute<any>();
  const params = route.params as {
    subjectId?: string;
    subjectName?: string;
    topicId?: string;
    topicName?: string;
    feature?: Feature;
  } | undefined;

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
  const [showUpload, setShowUpload] = useState(false);

  const loadEnrollments = useCallback(async () => {
    try {
      const { data } = await enrollmentsAPI.getAll();
      setEnrollments(data);
    } catch (e) {
      console.error('Failed to load enrollments:', e);
    }
  }, []);

  useEffect(() => { loadEnrollments(); }, [loadEnrollments]);

  // Auto-fill from route params after enrollments load
  useEffect(() => {
    if (!params?.subjectId || enrollments.length === 0) return;
    if (selectedSubject === params.subjectId) return;
    setSelectedSubject(params.subjectId);
  }, [params?.subjectId, enrollments]);

  // Auto-select topic and feature after topics load
  useEffect(() => {
    if (!params?.topicId || topics.length === 0) return;
    if (selectedTopic === params.topicId) return;
    const match = topics.find(t => t.id === params.topicId);
    if (match) {
      setSelectedTopic(params.topicId);
      if (params.feature) {
        resetResults();
        setActiveFeature(params.feature);
      }
    }
  }, [params?.topicId, params?.feature, topics]);

  // Auto-expand feature when subject-only params (no topic) and feature is studyPlan
  useEffect(() => {
    if (params?.subjectId && !params?.topicId && params?.feature === 'studyPlan' && selectedSubject) {
      resetResults();
      setActiveFeature('studyPlan');
    }
  }, [params?.feature, selectedSubject]);

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
      navigation.navigate('StudyPlan');
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Study plan generation failed';
      Alert.alert('Study Plan Error', msg);
    } finally { setLoading(false); }
  };

  const handleSelectAnswer = (answer: string) => {
    if (answeredQuestions.has(currentQuestion)) return;
    setSelectedAnswer(answer);
    const correct = quizQuestions[currentQuestion].correctAnswer.trim().toLowerCase() === answer.trim().toLowerCase();
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

  const FEATURES: { key: Feature; icon: React.ComponentProps<typeof Feather>['name']; label: string; color: string; desc: string; needsTopic: boolean }[] = [
    { key: 'summarize', icon: 'file-text', label: 'Summarize', color: colors.primary, desc: 'WASSCE-focused summary with key points', needsTopic: true },
    { key: 'explain', icon: 'book', label: 'AI Tutor', color: colors.primary, desc: 'Get WASSCE-aligned explanations & exam tips', needsTopic: true },
    { key: 'flashcards', icon: 'layers', label: 'Flashcards', color: colors.accent, desc: 'Generate WASSCE revision flashcards', needsTopic: true },
    { key: 'quiz', icon: 'help-circle', label: 'Quiz', color: colors.secondary, desc: 'Practice WASSCE-style questions', needsTopic: true },
    { key: 'studyPlan', icon: 'calendar', label: 'AI Study Plan', color: colors.gamification.xp, desc: 'Generate a WASSCE exam preparation plan', needsTopic: false },
  ];

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
      >
        <View style={styles.heroBanner}>
          <AfricanPattern variant="header" color={colors.accent} />
          <Feather name="star" size={32} color={colors.accent} />
          <Text style={[styles.heroTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>WASSCE AI Study Tools</Text>
          <Text style={[styles.heroSubtitle, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
            Select a subject and topic, then choose a tool below
          </Text>
        </View>

        <Text style={[styles.sectionLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Subject</Text>
        <TouchableOpacity
          style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.card }]}
          onPress={() => { setShowSubjectPicker(!showSubjectPicker); setShowTopicPicker(false); }}
        >
          <Text style={[{ fontSize: 16, fontFamily: theme.fonts.bodyMedium }, selectedSubject ? { color: colors.text } : { color: colors.textTertiary }]}>
            {selectedSubject
              ? enrollments.find(e => e.subjectId === selectedSubject)?.subjectName
              : 'Select an enrolled subject'}
          </Text>
          <Feather name={showSubjectPicker ? 'chevron-up' : 'chevron-down'} size={20} color={colors.textTertiary} />
        </TouchableOpacity>
        {showSubjectPicker && (
          <View style={[styles.optionList, { borderColor: colors.border, backgroundColor: colors.card }]}>
            {enrollments.map(e => (
              <TouchableOpacity
                key={e.subjectId}
                style={[styles.optionItem, { borderBottomColor: colors.border }, selectedSubject === e.subjectId && { backgroundColor: colors.primary + '10' }]}
                onPress={() => { setSelectedSubject(e.subjectId); setShowSubjectPicker(false); resetResults(); setActiveFeature(null); }}
              >
                <View style={[styles.optionDot, { backgroundColor: e.subjectColor || colors.primary }]} />
                <Text style={[{ flex: 1, fontSize: 15, color: colors.text, fontFamily: theme.fonts.body }, selectedSubject === e.subjectId && { fontFamily: theme.fonts.bodySemiBold, color: colors.primary }]}>
                  {e.subjectName}
                </Text>
                {selectedSubject === e.subjectId && <Feather name="check" size={18} color={colors.primary} />}
              </TouchableOpacity>
            ))}
            {enrollments.length === 0 && (
              <View style={{ padding: 16, alignItems: 'center' }}>
                <Text style={[styles.optionEmpty, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>No enrolled subjects yet.</Text>
                <TouchableOpacity
                  style={[styles.enrollButton, { backgroundColor: colors.primary }]}
                  onPress={() => navigation.navigate('Courses')}
                  activeOpacity={0.7}
                >
                  <Feather name="plus-circle" size={18} color="#fff" />
                  <Text style={[{ fontSize: 14, fontFamily: theme.fonts.bodySemiBold, color: '#fff', marginLeft: 8 }]}>Enroll in a Subject</Text>
                </TouchableOpacity>
              </View>
            )}
          </View>
        )}

        {selectedSubject && (
          <>
            <Text style={[styles.sectionLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Topic</Text>
            <TouchableOpacity
              style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.card }]}
              onPress={() => { setShowTopicPicker(!showTopicPicker); setShowSubjectPicker(false); }}
            >
              <Text style={[{ fontSize: 16, fontFamily: theme.fonts.bodyMedium }, selectedTopic ? { color: colors.text } : { color: colors.textTertiary }]}>
                {selectedTopic ? selectedTopicName : 'Select a topic'}
              </Text>
              <Feather name={showTopicPicker ? 'chevron-up' : 'chevron-down'} size={20} color={colors.textTertiary} />
            </TouchableOpacity>
            {showTopicPicker && (
              <View style={[styles.optionList, { borderColor: colors.border, backgroundColor: colors.card }]}>
                {topics.map(t => (
                  <TouchableOpacity
                    key={t.id}
                    style={[styles.optionItem, { borderBottomColor: colors.border }, selectedTopic === t.id && { backgroundColor: colors.primary + '10' }]}
                    onPress={() => { setSelectedTopic(t.id); setShowTopicPicker(false); resetResults(); }}
                  >
                    <Text style={[{ flex: 1, fontSize: 15, color: colors.text, fontFamily: theme.fonts.body }, selectedTopic === t.id && { fontFamily: theme.fonts.bodySemiBold, color: colors.primary }]}>
                      {t.name}
                    </Text>
                    {selectedTopic === t.id && <Feather name="check" size={18} color={colors.primary} />}
                  </TouchableOpacity>
                ))}
                {topics.length === 0 && (
                  <Text style={[styles.optionEmpty, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>No topics available for this subject.</Text>
                )}
              </View>
            )}
          </>
        )}

        {selectedSubject && selectedTopic && (() => {
          const topicObj = topics.find(t => t.id === selectedTopic);
          return topicObj && topicObj.noteCount === 0 ? (
            <View style={[styles.uploadBanner, { backgroundColor: colors.accent + '18', borderColor: colors.accent + '40' }]}>
              <Feather name="upload-cloud" size={22} color={colors.accent} />
              <View style={{ flex: 1, marginLeft: 12 }}>
                <Text style={[{ fontSize: 14, fontFamily: theme.fonts.bodySemiBold, color: colors.text }]}>No content uploaded yet</Text>
                <Text style={[{ fontSize: 12, fontFamily: theme.fonts.body, color: colors.textSecondary, marginTop: 2 }]}>Upload notes for better AI results</Text>
              </View>
              <TouchableOpacity
                style={[styles.uploadBannerBtn, { backgroundColor: colors.accent }]}
                onPress={() => setShowUpload(true)}
                activeOpacity={0.7}
              >
                <Text style={[{ fontSize: 13, fontFamily: theme.fonts.bodySemiBold, color: '#fff' }]}>Upload</Text>
              </TouchableOpacity>
            </View>
          ) : null;
        })()}

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
                      { borderLeftColor: f.color, backgroundColor: colors.card },
                      isActive && { borderColor: colors.primary, elevation: 3, shadowOpacity: 0.1 },
                      disabled && styles.featureCardDisabled,
                    ]}
                    onPress={() => !disabled && toggleFeature(f.key)}
                    activeOpacity={disabled ? 1 : 0.7}
                  >
                    <View style={[styles.featureIconWrap, { backgroundColor: f.color + '18' }]}>
                      <Feather name={f.icon} size={24} color={f.color} />
                    </View>
                    <View style={styles.featureInfo}>
                      <Text style={[styles.featureLabel, { color: colors.text, fontFamily: theme.fonts.bodyBold }, disabled && { color: colors.border }]}>{f.label}</Text>
                      <Text style={[styles.featureDesc, { color: colors.textSecondary, fontFamily: theme.fonts.body }, disabled && { color: colors.border }]}>{f.desc}</Text>
                    </View>
                    <Feather name={isActive ? 'chevron-up' : 'chevron-right'} size={20} color={disabled ? colors.border : colors.textTertiary} />
                  </TouchableOpacity>

                  {isActive && renderFeatureContent(f.key)}
                </View>
              );
            })}
          </View>
        )}

        {!selectedSubject && (
          <View style={styles.emptyState}>
            <Feather name="star" size={64} color={colors.border} />
            <Text style={[styles.emptyText, { color: colors.textSecondary, fontFamily: theme.fonts.headingBold }]}>Select a subject above to get started</Text>
            <Text style={[styles.emptySubtext, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>AI tools will help you study smarter</Text>
            {enrollments.length === 0 && (
              <TouchableOpacity
                style={[styles.enrollButton, { backgroundColor: colors.primary, marginTop: 20 }]}
                onPress={() => navigation.navigate('Courses')}
                activeOpacity={0.7}
              >
                <Feather name="plus-circle" size={18} color="#fff" />
                <Text style={[{ fontSize: 15, fontFamily: theme.fonts.bodySemiBold, color: '#fff', marginLeft: 8 }]}>Enroll in a Subject</Text>
              </TouchableOpacity>
            )}
          </View>
        )}

        <View style={{ height: 40 }} />
      </ScrollView>

      {showUpload && (
        <UploadContentSheet
          visible={showUpload}
          onClose={() => setShowUpload(false)}
          topicId={selectedTopic}
          topicName={selectedTopicName}
          onSuccess={() => {
            setShowUpload(false);
            // Refresh topics to update note count
            if (selectedSubject) {
              (async () => {
                try {
                  const { data } = await subjectsAPI.getTopics(selectedSubject);
                  setTopics(data);
                } catch (e) { console.error(e); }
              })();
            }
          }}
        />
      )}
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
  container: { flex: 1 },
  scrollView: { flex: 1, padding: 16 },

  heroBanner: { alignItems: 'center', paddingVertical: 24, marginBottom: 8 },
  heroTitle: { fontSize: 26, marginTop: 8 },
  heroSubtitle: { fontSize: 14, marginTop: 4, textAlign: 'center' },

  sectionLabel: { fontSize: 14, marginBottom: 8, marginTop: 12 },

  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 2, borderRadius: 12, paddingHorizontal: 16, paddingVertical: 16, minHeight: 56 },
  optionList: { borderWidth: 2, borderRadius: 12, overflow: 'hidden', marginBottom: 8 },
  optionItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingVertical: 14, borderBottomWidth: 1 },
  optionDot: { width: 10, height: 10, borderRadius: 5, marginRight: 12 },
  optionEmpty: { padding: 16, fontStyle: 'italic', textAlign: 'center' },

  featureGrid: { marginTop: 20, gap: 10 },
  featureCard: { flexDirection: 'row', alignItems: 'center', borderRadius: 16, padding: 16, borderLeftWidth: 4, elevation: 1, shadowColor: '#000', shadowOffset: { width: 0, height: 1 }, shadowOpacity: 0.05, shadowRadius: 3 },
  featureCardDisabled: { opacity: 0.5 },
  featureIconWrap: { width: 44, height: 44, borderRadius: 12, justifyContent: 'center', alignItems: 'center', marginRight: 14 },
  featureInfo: { flex: 1 },
  featureLabel: { fontSize: 16 },
  featureDesc: { fontSize: 12, marginTop: 2 },

  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 60 },
  emptyText: { fontSize: 18, marginTop: 16 },
  emptySubtext: { fontSize: 14, marginTop: 4 },

  uploadBanner: { flexDirection: 'row', alignItems: 'center', borderRadius: 14, padding: 14, marginTop: 16, borderWidth: 1.5 },
  uploadBannerBtn: { paddingHorizontal: 16, paddingVertical: 8, borderRadius: 10 },
  enrollButton: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 20, paddingVertical: 12, borderRadius: 12, marginTop: 12 },
});
