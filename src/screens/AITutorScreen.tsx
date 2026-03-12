import React, { useState, useEffect, useCallback } from 'react';
import {
  View, Text, StyleSheet, ScrollView, TouchableOpacity,
  RefreshControl, ActivityIndicator, TextInput as RNTextInput, Alert,
} from 'react-native';
import { Card, Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { aiAPI, subjectsAPI, enrollmentsAPI } from '../services/api';
import {
  Enrollment, Topic, SummarizeResponse, FlashcardItem,
  QuizQuestionItem, ExplainResponse,
} from '../types';

type Feature = 'summarize' | 'explain' | 'flashcards' | 'quiz' | 'studyPlan' | null;

export default function AITutorScreen() {
  // ── Selection state ───────────────────────────────────────────────
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [selectedSubject, setSelectedSubject] = useState('');
  const [showSubjectPicker, setShowSubjectPicker] = useState(false);
  const [topics, setTopics] = useState<Topic[]>([]);
  const [selectedTopic, setSelectedTopic] = useState('');
  const [showTopicPicker, setShowTopicPicker] = useState(false);
  const [refreshing, setRefreshing] = useState(false);

  // ── Active feature ────────────────────────────────────────────────
  const [activeFeature, setActiveFeature] = useState<Feature>(null);
  const [loading, setLoading] = useState(false);

  // ── Summarize ─────────────────────────────────────────────────────
  const [summary, setSummary] = useState<SummarizeResponse | null>(null);

  // ── Explain ───────────────────────────────────────────────────────
  const [explainResult, setExplainResult] = useState<ExplainResponse | null>(null);
  const [question, setQuestion] = useState('');

  // ── Flashcards ────────────────────────────────────────────────────
  const [flashcards, setFlashcards] = useState<FlashcardItem[]>([]);
  const [cardCount, setCardCount] = useState(10);
  const [flippedCards, setFlippedCards] = useState<Set<number>>(new Set());
  const [currentCard, setCurrentCard] = useState(0);

  // ── Quiz ──────────────────────────────────────────────────────────
  const [quizQuestions, setQuizQuestions] = useState<QuizQuestionItem[]>([]);
  const [quizDifficulty, setQuizDifficulty] = useState<number | null>(null);
  const [quizCount, setQuizCount] = useState(5);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [selectedAnswer, setSelectedAnswer] = useState<string | null>(null);
  const [quizScore, setQuizScore] = useState(0);
  const [quizFinished, setQuizFinished] = useState(false);
  const [answeredQuestions, setAnsweredQuestions] = useState<Set<number>>(new Set());

  // ── AI Study Plan ─────────────────────────────────────────────────
  const [planDays, setPlanDays] = useState(14);
  const [planCreated, setPlanCreated] = useState(false);

  // ── Data loading ──────────────────────────────────────────────────
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

  // ═══════════════════════════════════════════════════════════════════
  // API Calls
  // ═══════════════════════════════════════════════════════════════════

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

  // ── Quiz helpers ──────────────────────────────────────────────────
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

  // ── Flashcard helpers ─────────────────────────────────────────────
  const toggleFlip = () => {
    setFlippedCards(prev => {
      const next = new Set(prev);
      if (next.has(currentCard)) next.delete(currentCard);
      else next.add(currentCard);
      return next;
    });
  };

  // ═══════════════════════════════════════════════════════════════════
  // RENDER
  // ═══════════════════════════════════════════════════════════════════

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
        {/* ── Hero Banner ───────────────────────────────────── */}
        <View style={styles.heroBanner}>
          <Ionicons name="sparkles" size={32} color="#fbbf24" />
          <Text style={styles.heroTitle}>AI Study Tools</Text>
          <Text style={styles.heroSubtitle}>
            Select a subject and topic, then choose a tool below
          </Text>
        </View>

        {/* ── Subject Picker ────────────────────────────────── */}
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

        {/* ── Topic Picker ──────────────────────────────────── */}
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

        {/* ── Feature Cards ─────────────────────────────────── */}
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

                  {/* ── Expanded Feature Content ──────────────── */}
                  {isActive && renderFeatureContent(f.key)}
                </View>
              );
            })}
          </View>
        )}

        {/* ── Empty state ───────────────────────────────────── */}
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

  // ═══════════════════════════════════════════════════════════════════
  // Feature content renderers
  // ═══════════════════════════════════════════════════════════════════

  function renderFeatureContent(feature: Feature) {
    switch (feature) {
      case 'summarize': return renderSummarize();
      case 'explain': return renderExplain();
      case 'flashcards': return renderFlashcards();
      case 'quiz': return renderQuiz();
      case 'studyPlan': return renderStudyPlan();
      default: return null;
    }
  }

  // ── Summarize ─────────────────────────────────────────────────────
  function renderSummarize() {
    return (
      <View style={styles.featureContent}>
        <Button
          mode="contained" buttonColor="#0ea5e9" onPress={handleSummarize}
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
                {summary.keyPoints.map((point, i) => (
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

  // ── Explain (Tutor) ───────────────────────────────────────────────
  function renderExplain() {
    return (
      <View style={styles.featureContent}>
        <Text style={styles.inputLabel}>Ask a specific question (optional)</Text>
        <RNTextInput
          style={styles.textInput}
          placeholder={`e.g. "Why does ${selectedTopicName} matter?"`}
          placeholderTextColor="#94a3b8"
          value={question}
          onChangeText={setQuestion}
          multiline
        />
        <Button
          mode="contained" buttonColor="#0284c7" onPress={handleExplain}
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
                {explainResult.examples.map((ex, i) => (
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
                {explainResult.keyTakeaways.map((kt, i) => (
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

  // ── Flashcards ────────────────────────────────────────────────────
  function renderFlashcards() {
    return (
      <View style={styles.featureContent}>
        {/* Count selector */}
        <Text style={styles.inputLabel}>Number of cards</Text>
        <View style={styles.chipRow}>
          {[5, 10, 15].map(n => (
            <TouchableOpacity
              key={n}
              style={[styles.chip, cardCount === n && styles.chipSelected]}
              onPress={() => setCardCount(n)}
            >
              <Text style={[styles.chipText, cardCount === n && styles.chipTextSelected]}>{n}</Text>
            </TouchableOpacity>
          ))}
        </View>

        <Button
          mode="contained" buttonColor="#0ea5e9" onPress={handleFlashcards}
          loading={loading} disabled={loading} icon="layers-outline"
          style={styles.actionButton}
        >
          {flashcards.length > 0 ? 'Regenerate' : 'Generate Flashcards'}
        </Button>

        {flashcards.length > 0 && (
          <View style={styles.flashcardArea}>
            <Text style={styles.cardCounter}>{currentCard + 1} / {flashcards.length}</Text>

            <TouchableOpacity style={styles.flashcard} onPress={toggleFlip} activeOpacity={0.8}>
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
                onPress={() => { if (currentCard > 0) setCurrentCard(prev => prev - 1); }}
              >
                <Ionicons name="chevron-back" size={24} color={currentCard === 0 ? '#e2e8f0' : '#0ea5e9'} />
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.cardNavBtn, currentCard === flashcards.length - 1 && styles.cardNavBtnDisabled]}
                onPress={() => { if (currentCard < flashcards.length - 1) setCurrentCard(prev => prev + 1); }}
              >
                <Ionicons name="chevron-forward" size={24} color={currentCard === flashcards.length - 1 ? '#e2e8f0' : '#0ea5e9'} />
              </TouchableOpacity>
            </View>
          </View>
        )}
      </View>
    );
  }

  // ── Quiz ──────────────────────────────────────────────────────────
  function renderQuiz() {
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
                  onPress={() => setQuizDifficulty(d.value)}
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
                  onPress={() => setQuizCount(n)}
                >
                  <Text style={[styles.chipText, quizCount === n && styles.chipTextSelected]}>{n}</Text>
                </TouchableOpacity>
              ))}
            </View>

            <Button
              mode="contained" buttonColor="#f59e0b" onPress={handleQuiz}
              loading={loading} disabled={loading} icon="help-circle-outline"
              style={styles.actionButton}
            >
              Start Quiz
            </Button>
          </>
        )}

        {/* Active question */}
        {quizQuestions.length > 0 && !quizFinished && (
          <View style={styles.quizArea}>
            <View style={styles.quizProgress}>
              <Text style={styles.quizProgressText}>
                Question {currentQuestion + 1} of {quizQuestions.length}
              </Text>
              <Text style={styles.quizScore}>Score: {quizScore}/{answeredQuestions.size}</Text>
            </View>

            <Text style={styles.quizQuestion}>{quizQuestions[currentQuestion].question}</Text>

            {quizQuestions[currentQuestion].options.map((opt, i) => {
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
                  onPress={() => handleSelectAnswer(opt)}
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
                  mode="contained" buttonColor="#0ea5e9" onPress={handleNextQuestion}
                  style={styles.actionButton}
                >
                  {currentQuestion < quizQuestions.length - 1 ? 'Next Question' : 'See Results'}
                </Button>
              </>
            )}
          </View>
        )}

        {/* Quiz results */}
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
              onPress={() => { setQuizQuestions([]); setQuizFinished(false); setCurrentQuestion(0); setSelectedAnswer(null); setQuizScore(0); setAnsweredQuestions(new Set()); }}
              style={styles.actionButton} icon="refresh"
            >
              Try Again
            </Button>
          </View>
        )}
      </View>
    );
  }

  // ── AI Study Plan ─────────────────────────────────────────────────
  function renderStudyPlan() {
    return (
      <View style={styles.featureContent}>
        <Text style={styles.inputLabel}>Plan duration (days)</Text>
        <View style={styles.chipRow}>
          {[7, 14, 21, 30].map(n => (
            <TouchableOpacity
              key={n}
              style={[styles.chip, planDays === n && styles.chipSelected]}
              onPress={() => setPlanDays(n)}
            >
              <Text style={[styles.chipText, planDays === n && styles.chipTextSelected]}>{n} days</Text>
            </TouchableOpacity>
          ))}
        </View>

        <Button
          mode="contained" buttonColor="#10b981" onPress={handleStudyPlan}
          loading={loading} disabled={loading} icon="calendar-outline"
          style={styles.actionButton}
        >
          Generate AI Study Plan
        </Button>

        {planCreated && (
          <View style={styles.resultCard}>
            <Ionicons name="checkmark-circle" size={40} color="#10b981" style={{ alignSelf: 'center' }} />
            <Text style={[styles.resultTitle, { textAlign: 'center', marginTop: 8 }]}>
              Study Plan Created! 🎉
            </Text>
            <Text style={[styles.resultText, { textAlign: 'center' }]}>
              Your {planDays}-day AI-generated study plan is ready. Check the Study Plan tab to see it.
            </Text>
          </View>
        )}
      </View>
    );
  }
}

// ═════════════════════════════════════════════════════════════════════
// STYLES
// ═════════════════════════════════════════════════════════════════════

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  scrollView: { flex: 1, padding: 16 },

  // Hero
  heroBanner: { alignItems: 'center', paddingVertical: 24, marginBottom: 8 },
  heroTitle: { fontSize: 26, fontWeight: 'bold', color: '#1e293b', marginTop: 8 },
  heroSubtitle: { fontSize: 14, color: '#64748b', marginTop: 4, textAlign: 'center' },

  // Section
  sectionLabel: { fontSize: 14, fontWeight: '600', color: '#1e293b', marginBottom: 8, marginTop: 12 },

  // Picker
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

  // Feature cards
  featureGrid: { marginTop: 20, gap: 10 },
  featureCard: { flexDirection: 'row', alignItems: 'center', backgroundColor: '#fff', borderRadius: 14, padding: 16, borderLeftWidth: 4, elevation: 1, shadowColor: '#000', shadowOffset: { width: 0, height: 1 }, shadowOpacity: 0.05, shadowRadius: 3 },
  featureCardActive: { borderColor: '#0ea5e9', elevation: 3, shadowOpacity: 0.1 },
  featureCardDisabled: { opacity: 0.5 },
  featureIconWrap: { width: 44, height: 44, borderRadius: 12, justifyContent: 'center', alignItems: 'center', marginRight: 14 },
  featureInfo: { flex: 1 },
  featureLabel: { fontSize: 16, fontWeight: '700', color: '#1e293b' },
  featureDesc: { fontSize: 12, color: '#64748b', marginTop: 2 },

  // Feature content
  featureContent: { backgroundColor: '#fff', borderRadius: 14, padding: 16, marginTop: 4, marginBottom: 4, borderWidth: 1, borderColor: '#f1f5f9' },
  actionButton: { marginTop: 12, borderRadius: 10 },
  inputLabel: { fontSize: 13, fontWeight: '600', color: '#64748b', marginBottom: 6, marginTop: 8 },
  textInput: { borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#fff', paddingHorizontal: 16, paddingVertical: 12, fontSize: 15, color: '#1e293b', minHeight: 48 },

  // Chips
  chipRow: { flexDirection: 'row', gap: 8, marginBottom: 4 },
  chip: { paddingHorizontal: 16, paddingVertical: 10, borderRadius: 20, borderWidth: 2, borderColor: '#e2e8f0', backgroundColor: '#fff' },
  chipSelected: { borderColor: '#0ea5e9', backgroundColor: '#f0f9ff' },
  chipText: { fontSize: 14, color: '#64748b', fontWeight: '500' },
  chipTextSelected: { color: '#0ea5e9', fontWeight: '700' },

  // Results
  resultCard: { marginTop: 16, backgroundColor: '#f8fafc', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#e2e8f0' },
  resultTitle: { fontSize: 16, fontWeight: '700', color: '#1e293b', marginBottom: 8 },
  resultText: { fontSize: 14, color: '#475569', lineHeight: 22 },
  bulletRow: { flexDirection: 'row', alignItems: 'flex-start', gap: 10, marginTop: 8 },
  bulletText: { flex: 1, fontSize: 14, color: '#475569', lineHeight: 20 },

  // Flashcards
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

  // Quiz
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

  // Empty state
  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 60 },
  emptyText: { fontSize: 18, fontWeight: 'bold', color: '#64748b', marginTop: 16 },
  emptySubtext: { fontSize: 14, color: '#94a3b8', marginTop: 4 },
});
