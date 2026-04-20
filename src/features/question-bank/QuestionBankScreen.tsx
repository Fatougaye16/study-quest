import React, { useState, useCallback, useEffect } from 'react';
import {
  View, Text, FlatList, TouchableOpacity, RefreshControl, SafeAreaView, StyleSheet, Modal, TextInput, Alert,
} from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../shared/theme';
import { enrollmentsAPI } from '../courses/api';
import { Enrollment } from '../courses/types';
import { questionBankAPI } from './api';
import { PastPaper, PastQuestion } from './types';
import XCard from '../../shared/components/XCard';
import XButton from '../../shared/components/XButton';
import XEmptyState from '../../shared/components/XEmptyState';
import AfricanPattern from '../../shared/components/AfricanPattern';
import XDownloadButton from '../../shared/components/XDownloadButton';

const EXAM_TYPES = ['WASSCE', 'BECE', 'NECO'];
const YEARS = Array.from({ length: 15 }, (_, i) => new Date().getFullYear() - i);

export default function QuestionBankScreen() {
  const { theme } = useTheme();
  const colors = theme.colors;

  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [papers, setPapers] = useState<PastPaper[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  // Filters
  const [filterSubject, setFilterSubject] = useState<string | undefined>();
  const [filterYear, setFilterYear] = useState<number | undefined>();
  const [filterExam, setFilterExam] = useState<string | undefined>();
  const [showFilters, setShowFilters] = useState(false);

  // Question detail view
  const [selectedPaper, setSelectedPaper] = useState<PastPaper | null>(null);
  const [questions, setQuestions] = useState<PastQuestion[]>([]);
  const [questionsLoading, setQuestionsLoading] = useState(false);
  const [showAnswers, setShowAnswers] = useState<Record<string, boolean>>({});

  const loadPapers = useCallback(async () => {
    try {
      const params: any = {};
      if (filterSubject) params.subjectId = filterSubject;
      if (filterYear) params.year = filterYear;
      if (filterExam) params.examType = filterExam;
      const { data } = await questionBankAPI.getPapers(params);
      setPapers(data);
    } catch (e) {
      console.error('Failed to load papers:', e);
    } finally {
      setLoading(false);
    }
  }, [filterSubject, filterYear, filterExam]);

  const loadEnrollments = useCallback(async () => {
    try {
      const { data } = await enrollmentsAPI.getAll();
      setEnrollments(data);
    } catch (e) {
      console.error('Failed to load enrollments:', e);
    }
  }, []);

  useEffect(() => { loadEnrollments(); }, [loadEnrollments]);
  useEffect(() => { loadPapers(); }, [loadPapers]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadPapers();
    setRefreshing(false);
  }, [loadPapers]);

  const openPaper = async (paper: PastPaper) => {
    setSelectedPaper(paper);
    setQuestionsLoading(true);
    setShowAnswers({});
    try {
      const { data } = await questionBankAPI.getQuestions(paper.id);
      setQuestions(data);
    } catch {
      setQuestions([]);
    } finally {
      setQuestionsLoading(false);
    }
  };

  const toggleAnswer = (qId: string) => {
    setShowAnswers(prev => ({ ...prev, [qId]: !prev[qId] }));
  };

  const clearFilters = () => {
    setFilterSubject(undefined);
    setFilterYear(undefined);
    setFilterExam(undefined);
    setShowFilters(false);
  };

  const activeFilterCount = [filterSubject, filterYear, filterExam].filter(Boolean).length;

  const renderPaperCard = ({ item }: { item: PastPaper }) => (
    <TouchableOpacity onPress={() => openPaper(item)} activeOpacity={0.7}>
      <XCard variant="elevated" style={{ marginBottom: 12 }}>
        <View style={{ flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <View style={{ flex: 1 }}>
            <Text style={{ fontSize: 16, fontFamily: theme.fonts.headingBold, color: colors.text }}>
              {item.title}
            </Text>
            <Text style={{ fontSize: 13, fontFamily: theme.fonts.body, color: colors.textSecondary, marginTop: 2 }}>
              {item.subjectName} • {item.examType} {item.year} • Paper {item.paperNumber}
            </Text>
          </View>
          <View style={{
            backgroundColor: colors.primary + '18', borderRadius: 12,
            paddingHorizontal: 10, paddingVertical: 4, marginLeft: 8,
          }}>
            <Text style={{ fontSize: 12, fontFamily: theme.fonts.bodySemiBold, color: colors.primary }}>
              {item.questionCount} Q{item.questionCount !== 1 ? 's' : ''}
            </Text>
          </View>
        </View>
        <View style={{ flexDirection: 'row', alignItems: 'center', marginTop: 8 }}>
          <Feather name="chevron-right" size={16} color={colors.textTertiary} />
          <Text style={{ fontSize: 12, fontFamily: theme.fonts.body, color: colors.textTertiary, marginLeft: 4 }}>
            Tap to view questions
          </Text>
        </View>
      </XCard>
    </TouchableOpacity>
  );

  const renderQuestion = ({ item }: { item: PastQuestion }) => (
    <XCard variant="default" style={{ marginBottom: 10 }}>
      <View style={{ flexDirection: 'row', alignItems: 'flex-start' }}>
        <View style={{
          width: 28, height: 28, borderRadius: 14, backgroundColor: colors.primary + '18',
          justifyContent: 'center', alignItems: 'center', marginRight: 10, marginTop: 2,
        }}>
          <Text style={{ fontSize: 12, fontFamily: theme.fonts.bodySemiBold, color: colors.primary }}>
            {item.questionNumber}
          </Text>
        </View>
        <View style={{ flex: 1 }}>
          <Text style={{ fontSize: 14, fontFamily: theme.fonts.body, color: colors.text, lineHeight: 20 }}>
            {item.questionText}
          </Text>
          {item.marks != null && (
            <Text style={{ fontSize: 11, fontFamily: theme.fonts.body, color: colors.textTertiary, marginTop: 2, fontStyle: 'italic' }}>
              [{item.marks} mark{item.marks !== 1 ? 's' : ''}]
            </Text>
          )}
          {item.topicName && (
            <View style={{
              backgroundColor: colors.secondary + '15', borderRadius: 8,
              paddingHorizontal: 8, paddingVertical: 2, alignSelf: 'flex-start', marginTop: 6,
            }}>
              <Text style={{ fontSize: 11, fontFamily: theme.fonts.body, color: colors.secondary }}>
                {item.topicName}
              </Text>
            </View>
          )}
        </View>
      </View>

      {item.answerText && (
        <TouchableOpacity onPress={() => toggleAnswer(item.id)} style={{ marginTop: 10 }}>
          <View style={{
            flexDirection: 'row', alignItems: 'center',
            backgroundColor: showAnswers[item.id] ? colors.success + '12' : colors.borderLight,
            borderRadius: 8, padding: 10,
          }}>
            <Feather
              name={showAnswers[item.id] ? 'eye-off' : 'eye'}
              size={14} color={showAnswers[item.id] ? colors.success : colors.textTertiary}
            />
            <Text style={{
              fontSize: 13, fontFamily: theme.fonts.bodySemiBold, marginLeft: 6,
              color: showAnswers[item.id] ? colors.success : colors.textTertiary,
            }}>
              {showAnswers[item.id] ? 'Hide Answer' : 'Show Answer'}
            </Text>
          </View>
          {showAnswers[item.id] && (
            <Text style={{
              fontSize: 14, fontFamily: theme.fonts.body, color: colors.text,
              marginTop: 8, lineHeight: 20, paddingLeft: 10,
              borderLeftWidth: 2, borderLeftColor: colors.success,
            }}>
              {item.answerText}
            </Text>
          )}
        </TouchableOpacity>
      )}

      <View style={{
        flexDirection: 'row', justifyContent: 'flex-end', marginTop: 6,
      }}>
        {[1, 2, 3].map(d => (
          <View key={d} style={{
            width: 8, height: 8, borderRadius: 4, marginLeft: 3,
            backgroundColor: d <= item.difficulty ? colors.primary : colors.borderLight,
          }} />
        ))}
      </View>
    </XCard>
  );

  // Question detail modal
  if (selectedPaper) {
    return (
      <SafeAreaView style={{ flex: 1, backgroundColor: colors.background }}>
        <View style={{
          flexDirection: 'row', alignItems: 'center', padding: 16,
          backgroundColor: colors.card, borderBottomWidth: 1, borderBottomColor: colors.border,
        }}>
          <TouchableOpacity onPress={() => setSelectedPaper(null)} style={{ marginRight: 12 }}>
            <Feather name="arrow-left" size={24} color={colors.text} />
          </TouchableOpacity>
          <View style={{ flex: 1 }}>
            <Text style={{ fontSize: 16, fontFamily: theme.fonts.headingBold, color: colors.text }}>
              {selectedPaper.title}
            </Text>
            <Text style={{ fontSize: 12, fontFamily: theme.fonts.body, color: colors.textSecondary }}>
              {selectedPaper.subjectName} • {selectedPaper.examType} {selectedPaper.year}
            </Text>
          </View>
          <XDownloadButton
            type="past-paper" id={selectedPaper.id} variant="icon"
            fileName={`${selectedPaper.subjectName}_${selectedPaper.examType}_${selectedPaper.year}.pdf`}
          />
        </View>

        {questionsLoading ? (
          <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
            <Text style={{ color: colors.textTertiary, fontFamily: theme.fonts.body }}>Loading questions...</Text>
          </View>
        ) : questions.length === 0 ? (
          <XEmptyState icon="help-circle" title="No Questions Yet" message="Questions haven't been added to this paper yet." />
        ) : (
          <FlatList
            data={questions}
            keyExtractor={q => q.id}
            renderItem={renderQuestion}
            contentContainerStyle={{ padding: 16 }}
          />
        )}
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: colors.background }}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />

      {/* Filter Bar */}
      <View style={{
        flexDirection: 'row', alignItems: 'center', padding: 12, paddingHorizontal: 16,
        backgroundColor: colors.card, borderBottomWidth: 1, borderBottomColor: colors.border,
      }}>
        <TouchableOpacity
          onPress={() => setShowFilters(!showFilters)}
          style={{
            flexDirection: 'row', alignItems: 'center', backgroundColor: colors.primary + '12',
            borderRadius: 20, paddingHorizontal: 14, paddingVertical: 8,
          }}
        >
          <Feather name="filter" size={16} color={colors.primary} />
          <Text style={{ fontSize: 13, fontFamily: theme.fonts.bodySemiBold, color: colors.primary, marginLeft: 6 }}>
            Filters{activeFilterCount > 0 ? ` (${activeFilterCount})` : ''}
          </Text>
        </TouchableOpacity>
        {activeFilterCount > 0 && (
          <TouchableOpacity onPress={clearFilters} style={{ marginLeft: 8 }}>
            <Text style={{ fontSize: 12, fontFamily: theme.fonts.body, color: colors.error }}>Clear</Text>
          </TouchableOpacity>
        )}
      </View>

      {/* Expandable Filter Panel */}
      {showFilters && (
        <View style={{ padding: 16, backgroundColor: colors.card, borderBottomWidth: 1, borderBottomColor: colors.border }}>
          <Text style={{ fontSize: 13, fontFamily: theme.fonts.bodySemiBold, color: colors.text, marginBottom: 8 }}>Subject</Text>
          <FlatList
            horizontal showsHorizontalScrollIndicator={false}
            data={[{ subjectId: '', subjectName: 'All' } as Enrollment, ...enrollments]}
            keyExtractor={e => e.subjectId || 'all'}
            renderItem={({ item: e }) => (
              <TouchableOpacity
                onPress={() => setFilterSubject(e.subjectId || undefined)}
                style={{
                  paddingHorizontal: 14, paddingVertical: 6, borderRadius: 16, marginRight: 8,
                  backgroundColor: filterSubject === (e.subjectId || undefined) ? colors.primary : colors.borderLight,
                }}
              >
                <Text style={{
                  fontSize: 13, fontFamily: theme.fonts.body,
                  color: filterSubject === (e.subjectId || undefined) ? colors.textInverse : colors.text,
                }}>
                  {e.subjectName}
                </Text>
              </TouchableOpacity>
            )}
          />

          <Text style={{ fontSize: 13, fontFamily: theme.fonts.bodySemiBold, color: colors.text, marginTop: 12, marginBottom: 8 }}>Exam Type</Text>
          <View style={{ flexDirection: 'row', gap: 8 }}>
            {['All', ...EXAM_TYPES].map(t => (
              <TouchableOpacity
                key={t}
                onPress={() => setFilterExam(t === 'All' ? undefined : t)}
                style={{
                  paddingHorizontal: 14, paddingVertical: 6, borderRadius: 16,
                  backgroundColor: filterExam === (t === 'All' ? undefined : t) ? colors.primary : colors.borderLight,
                }}
              >
                <Text style={{
                  fontSize: 13, fontFamily: theme.fonts.body,
                  color: filterExam === (t === 'All' ? undefined : t) ? colors.textInverse : colors.text,
                }}>
                  {t}
                </Text>
              </TouchableOpacity>
            ))}
          </View>

          <Text style={{ fontSize: 13, fontFamily: theme.fonts.bodySemiBold, color: colors.text, marginTop: 12, marginBottom: 8 }}>Year</Text>
          <FlatList
            horizontal showsHorizontalScrollIndicator={false}
            data={[0, ...YEARS]}
            keyExtractor={y => String(y)}
            renderItem={({ item: y }) => (
              <TouchableOpacity
                onPress={() => setFilterYear(y === 0 ? undefined : y)}
                style={{
                  paddingHorizontal: 14, paddingVertical: 6, borderRadius: 16, marginRight: 8,
                  backgroundColor: filterYear === (y === 0 ? undefined : y) ? colors.primary : colors.borderLight,
                }}
              >
                <Text style={{
                  fontSize: 13, fontFamily: theme.fonts.body,
                  color: filterYear === (y === 0 ? undefined : y) ? colors.textInverse : colors.text,
                }}>
                  {y === 0 ? 'All' : y}
                </Text>
              </TouchableOpacity>
            )}
          />
        </View>
      )}

      {/* Papers List */}
      {loading ? (
        <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
          <Text style={{ color: colors.textTertiary, fontFamily: theme.fonts.body }}>Loading papers...</Text>
        </View>
      ) : papers.length === 0 ? (
        <View style={{ flex: 1, justifyContent: 'center', padding: 16 }}>
          <XEmptyState
            icon="file-text"
            title="No Past Papers"
            message={activeFilterCount > 0 ? 'Try adjusting your filters.' : 'Past exam papers will appear here once added.'}
          />
        </View>
      ) : (
        <FlatList
          data={papers}
          keyExtractor={p => p.id}
          renderItem={renderPaperCard}
          contentContainerStyle={{ padding: 16 }}
          refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
        />
      )}
    </SafeAreaView>
  );
}
