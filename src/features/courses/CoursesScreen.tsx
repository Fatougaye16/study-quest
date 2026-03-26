import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, RefreshControl, TouchableOpacity, ActivityIndicator } from 'react-native';
import { Button, Card, Dialog, Portal } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';
import { useTheme } from '../../shared/theme';
import { subjectsAPI, enrollmentsAPI } from './api';
import { Subject, Enrollment, Topic, Note, NoteSourceType } from './types';
import UploadContentSheet from './components/UploadContentSheet';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function CoursesScreen() {
  const navigation = useNavigation<any>();
  const { theme } = useTheme();
  const colors = theme.colors;

  const getSourceIcon = (sourceType: number): string => {
    switch (sourceType) {
      case NoteSourceType.Pdf: return 'paperclip';
      case NoteSourceType.Document: return 'file-text';
      case NoteSourceType.Image: return 'image';
      default: return 'edit';
    }
  };

  const getSourceColor = (sourceType: number): string => {
    switch (sourceType) {
      case NoteSourceType.Pdf: return colors.error;
      case NoteSourceType.Document: return colors.accent;
      case NoteSourceType.Image: return colors.success;
      default: return colors.primary;
    }
  };

  const getSourceLabel = (sourceType: number): string => {
    switch (sourceType) {
      case NoteSourceType.Pdf: return 'PDF';
      case NoteSourceType.Document: return 'DOC';
      case NoteSourceType.Image: return 'IMG';
      default: return '';
    }
  };

  const [subjects, setSubjects] = useState<Subject[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [refreshing, setRefreshing] = useState(false);
  const [showBrowse, setShowBrowse] = useState(false);
  const [enrolling, setEnrolling] = useState<string | null>(null);

  const [expandedSubject, setExpandedSubject] = useState<string | null>(null);
  const [topics, setTopics] = useState<Topic[]>([]);
  const [loadingTopics, setLoadingTopics] = useState(false);
  const [expandedTopic, setExpandedTopic] = useState<string | null>(null);
  const [notes, setNotes] = useState<Note[]>([]);
  const [loadingNotes, setLoadingNotes] = useState(false);
  const [uploadTopicId, setUploadTopicId] = useState<string | null>(null);
  const [uploadTopicName, setUploadTopicName] = useState('');
  const [showQuickUpload, setShowQuickUpload] = useState(false);

  const loadData = useCallback(async () => {
    try {
      const [subRes, enrRes] = await Promise.all([
        subjectsAPI.getAll(),
        enrollmentsAPI.getAll(),
      ]);
      setSubjects(subRes.data);
      setEnrollments(enrRes.data);
    } catch (error) {
      console.error('Failed to load subjects:', error);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    setExpandedSubject(null);
    setExpandedTopic(null);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  const enrolledSubjectIds = new Set(enrollments.map(e => e.subjectId));

  const handleEnroll = async (subjectId: string) => {
    setEnrolling(subjectId);
    try {
      await enrollmentsAPI.enroll(subjectId);
      await loadData();
      Alert.alert('Enrolled! 🎉', 'You have been enrolled in this subject.\n\n+30 XP earned!');
    } catch (error: any) {
      const msg = error.response?.data?.detail || 'Failed to enroll';
      Alert.alert('Error', msg);
    } finally {
      setEnrolling(null);
    }
  };

  const handleUnenroll = async (enrollmentId: string) => {
    Alert.alert('Unenroll', 'Are you sure you want to unenroll from this subject?', [
      { text: 'Cancel', style: 'cancel' },
      {
        text: 'Unenroll',
        style: 'destructive',
        onPress: async () => {
          try {
            await enrollmentsAPI.unenroll(enrollmentId);
            if (expandedSubject) setExpandedSubject(null);
            await loadData();
          } catch (error: any) {
            Alert.alert('Error', error.response?.data?.detail || 'Failed to unenroll');
          }
        },
      },
    ]);
  };

  const toggleSubjectTopics = async (subjectId: string) => {
    if (expandedSubject === subjectId) {
      setExpandedSubject(null);
      setExpandedTopic(null);
      return;
    }
    setExpandedSubject(subjectId);
    setExpandedTopic(null);
    setLoadingTopics(true);
    try {
      const { data } = await subjectsAPI.getTopics(subjectId);
      setTopics(data);
    } catch (e) {
      console.error('Failed to load topics:', e);
      setTopics([]);
    } finally {
      setLoadingTopics(false);
    }
  };

  const toggleTopicNotes = async (topicId: string) => {
    if (expandedTopic === topicId) {
      setExpandedTopic(null);
      return;
    }
    setExpandedTopic(topicId);
    setLoadingNotes(true);
    try {
      const { data } = await subjectsAPI.getNotes(topicId);
      setNotes(data);
    } catch (e) {
      console.error('Failed to load notes:', e);
      setNotes([]);
    } finally {
      setLoadingNotes(false);
    }
  };

  const unenrolledSubjects = subjects.filter(s => !enrolledSubjectIds.has(s.id));

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
      >
        {/* Quick Upload Button */}
        {enrollments.length > 0 && (
          <TouchableOpacity
            style={[styles.quickUploadBar, { backgroundColor: colors.primary }]}
            activeOpacity={0.7}
            onPress={() => setShowQuickUpload(true)}
          >
            <Feather name="upload-cloud" size={20} color={colors.textInverse} />
            <Text style={[styles.quickUploadText, { color: colors.textInverse, fontFamily: theme.fonts.headingBold }]}>Upload Content</Text>
            <Feather name="chevron-right" size={18} color="rgba(255,255,255,0.7)" />
          </TouchableOpacity>
        )}

        {enrollments.length === 0 ? (
          <View style={styles.emptyState}>
            <Feather name="book-open" size={64} color={colors.textTertiary} style={{ marginBottom: 16 }} />
            <Text style={[styles.emptyText, { color: colors.textSecondary, fontFamily: theme.fonts.headingBold }]}>No subjects enrolled yet!</Text>
            <Text style={[styles.emptySubtext, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>Browse available subjects to get started</Text>
          </View>
        ) : (
          enrollments.map(enrollment => {
            const isExpanded = expandedSubject === enrollment.subjectId;
            return (
              <Card
                key={enrollment.id}
                style={[styles.courseCard, { borderTopColor: enrollment.subjectColor, backgroundColor: colors.card }]}
              >
                <Card.Content>
                  <View style={styles.courseHeader}>
                    <View style={styles.courseInfo}>
                      <Text style={[styles.courseName, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{enrollment.subjectName}</Text>
                      <Text style={[styles.courseCode, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Grade {enrollment.grade}</Text>
                      <Text style={[styles.enrollDate, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>
                        Enrolled {new Date(enrollment.enrolledAt).toLocaleDateString()}
                      </Text>
                    </View>
                    <View style={[styles.colorDot, { backgroundColor: enrollment.subjectColor }]} />
                  </View>

                  <TouchableOpacity
                    style={[styles.topicsToggle, { backgroundColor: colors.primaryLight + '20' }]}
                    onPress={() => toggleSubjectTopics(enrollment.subjectId)}
                    activeOpacity={0.7}
                  >
                    <Feather name="book-open" size={18} color={colors.primary} />
                    <Text style={[styles.topicsToggleText, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>
                      {isExpanded ? 'Hide Topics' : 'View Topics & Notes'}
                    </Text>
                    <Feather
                      name={isExpanded ? 'chevron-up' : 'chevron-down'}
                      size={18}
                      color={colors.primary}
                    />
                  </TouchableOpacity>

                  {isExpanded && (
                    <View style={[styles.topicsContainer, { borderLeftColor: colors.border }]}>
                      {loadingTopics ? (
                        <ActivityIndicator size="small" color={colors.primary} style={{ padding: 16 }} />
                      ) : topics.length === 0 ? (
                        <Text style={[styles.noContent, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>No topics available yet.</Text>
                      ) : (
                        topics.map(topic => {
                          const isTopicOpen = expandedTopic === topic.id;
                          return (
                            <View key={topic.id} style={styles.topicItem}>
                              <TouchableOpacity
                                style={styles.topicHeader}
                                onPress={() => toggleTopicNotes(topic.id)}
                                activeOpacity={0.7}
                              >
                                <View style={[styles.topicOrderBadge, { backgroundColor: colors.primaryLight + '30' }]}>
                                  <Text style={[styles.topicOrderText, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>{topic.order}</Text>
                                </View>
                                <View style={styles.topicInfo}>
                                  <Text style={[styles.topicName, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{topic.name}</Text>
                                  {topic.description ? (
                                    <Text style={[styles.topicDesc, { color: colors.textSecondary, fontFamily: theme.fonts.body }]} numberOfLines={1}>{topic.description}</Text>
                                  ) : null}
                                  <View style={styles.topicMeta}>
                                    <Feather name="file-text" size={12} color={colors.textTertiary} />
                                    <Text style={[styles.topicMetaText, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>{topic.noteCount} notes</Text>
                                    <Feather name="help-circle" size={12} color={colors.textTertiary} style={{ marginLeft: 8 }} />
                                    <Text style={[styles.topicMetaText, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>{topic.questionCount} questions</Text>
                                  </View>
                                </View>
                                <Feather
                                  name={isTopicOpen ? 'chevron-up' : 'chevron-right'}
                                  size={16}
                                  color={colors.textTertiary}
                                />
                              </TouchableOpacity>

                              {isTopicOpen && (
                                <View style={styles.notesContainer}>
                                  <TouchableOpacity
                                    style={[styles.uploadButton, { backgroundColor: colors.primaryLight + '20', borderColor: colors.primaryLight }]}
                                    onPress={() => { setUploadTopicId(topic.id); setUploadTopicName(topic.name); }}
                                    activeOpacity={0.7}
                                  >
                                    <Feather name="upload-cloud" size={16} color={colors.primary} />
                                    <Text style={[styles.uploadButtonText, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>Upload Content</Text>
                                  </TouchableOpacity>
                                  {loadingNotes ? (
                                    <ActivityIndicator size="small" color={colors.primary} style={{ padding: 12 }} />
                                  ) : notes.length === 0 ? (
                                    <Text style={[styles.noContent, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>No notes for this topic yet.</Text>
                                  ) : (
                                    notes.map(note => (
                                      <View key={note.id} style={[styles.noteItem, { backgroundColor: colors.card, borderColor: colors.border }]}>
                                        <View style={styles.noteHeader}>
                                          <Feather name={getSourceIcon(note.sourceType) as any} size={16} color={getSourceColor(note.sourceType)} />
                                          <Text style={[styles.noteTitle, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]} numberOfLines={1}>{note.title}</Text>
                                          {note.isOfficial && (
                                            <View style={[styles.aiBadge, { backgroundColor: colors.success + '33' }]}>
                                              <Feather name="shield" size={10} color={colors.success} />
                                              <Text style={[styles.aiBadgeText, { color: colors.success, fontFamily: theme.fonts.headingBold }]}>Official</Text>
                                            </View>
                                          )}
                                          {note.isAIGenerated && (
                                            <View style={[styles.aiBadge, { backgroundColor: colors.primaryLight + '20' }]}>
                                              <Feather name="star" size={10} color={colors.primary} />
                                              <Text style={[styles.aiBadgeText, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>AI</Text>
                                            </View>
                                          )}
                                          {note.sourceType !== NoteSourceType.Manual && (
                                            <View style={[styles.aiBadge, { backgroundColor: colors.accent + '33' }]}>
                                              <Text style={[styles.aiBadgeText, { color: colors.accent, fontFamily: theme.fonts.headingBold }]}>{getSourceLabel(note.sourceType)}</Text>
                                            </View>
                                          )}
                                        </View>
                                        {note.originalFileName && (
                                          <Text style={[styles.fileName, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>{note.originalFileName}</Text>
                                        )}
                                        <Text style={[styles.noteContent, { color: colors.textSecondary, fontFamily: theme.fonts.body }]} numberOfLines={4}>{note.content}</Text>
                                        <Text style={[styles.noteDate, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>
                                          {new Date(note.createdAt).toLocaleDateString()}
                                        </Text>
                                      </View>
                                    ))
                                  )}
                                </View>
                              )}
                            </View>
                          );
                        })
                      )}
                    </View>
                  )}

                  <View style={styles.cardActions}>
                    <Button
                      mode="contained"
                      onPress={() => navigation.navigate('AITutor')}
                      buttonColor={colors.primary}
                      icon="creation"
                      compact
                      style={styles.aiButton}
                    >
                      AI Tools
                    </Button>
                    <Button
                      mode="outlined"
                      onPress={() => handleUnenroll(enrollment.id)}
                      style={[styles.deleteButton, { borderColor: colors.error }]}
                      textColor={colors.error}
                      compact
                    >
                      Unenroll
                    </Button>
                  </View>
                </Card.Content>
              </Card>
            );
          })
        )}
      </ScrollView>

      <Button
        mode="contained"
        onPress={() => setShowBrowse(true)}
        style={[styles.addButton, { backgroundColor: colors.primary }]}
        icon="plus"
      >
        Browse Subjects
      </Button>

      <Portal>
        <Dialog visible={showBrowse} onDismiss={() => setShowBrowse(false)} style={[styles.dialog, { backgroundColor: colors.card }]}>
          <Dialog.Title>
            <Text style={[styles.dialogTitleText, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Available Subjects</Text>
          </Dialog.Title>
          <Dialog.ScrollArea style={{ maxHeight: 400 }}>
            <ScrollView>
              {unenrolledSubjects.length === 0 ? (
                <Text style={[styles.allEnrolled, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>You're enrolled in all available subjects!</Text>
              ) : (
                unenrolledSubjects.map(subject => (
                  <View key={subject.id} style={[styles.browseItem, { borderBottomColor: colors.border }]}>
                    <View style={styles.browseInfo}>
                      <View style={styles.browseHeader}>
                        <View style={[styles.colorDotSmall, { backgroundColor: subject.color }]} />
                        <Text style={[styles.browseName, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{subject.name}</Text>
                      </View>
                      <Text style={[styles.browseDesc, { color: colors.textSecondary, fontFamily: theme.fonts.body }]} numberOfLines={2}>{subject.description}</Text>
                      <Text style={[styles.browseTopics, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>{subject.topicCount} topics</Text>
                    </View>
                    <Button
                      mode="contained"
                      onPress={() => handleEnroll(subject.id)}
                      loading={enrolling === subject.id}
                      disabled={enrolling !== null}
                      buttonColor={colors.primary}
                      compact
                    >
                      Enroll
                    </Button>
                  </View>
                ))
              )}
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions>
            <Button onPress={() => setShowBrowse(false)} textColor={colors.textSecondary}>Close</Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>

      {/* Topic-specific upload (from topic drill-down) */}
      <UploadContentSheet
        visible={uploadTopicId !== null}
        topicId={uploadTopicId ?? undefined}
        topicName={uploadTopicName}
        onClose={() => setUploadTopicId(null)}
        onSuccess={() => {
          if (uploadTopicId) toggleTopicNotes(uploadTopicId);
        }}
      />

      {/* Quick upload (no pre-selected topic — picker is inside the sheet) */}
      <UploadContentSheet
        visible={showQuickUpload}
        onClose={() => setShowQuickUpload(false)}
        onSuccess={() => {}}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollView: { flex: 1, padding: 16 },
  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 80 },
  emptyText: { fontSize: 20, marginBottom: 8 },
  emptySubtext: { fontSize: 14 },
  courseCard: { marginBottom: 12, borderTopWidth: 4, borderRadius: 16 },
  courseHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' },
  courseInfo: { flex: 1 },
  courseName: { fontSize: 18, marginBottom: 4 },
  courseCode: { fontSize: 14, marginBottom: 2 },
  enrollDate: { fontSize: 12 },
  colorDot: { width: 16, height: 16, borderRadius: 8, marginTop: 4 },
  topicsToggle: { flexDirection: 'row', alignItems: 'center', gap: 8, marginTop: 14, paddingVertical: 10, paddingHorizontal: 12, borderRadius: 12 },
  topicsToggleText: { flex: 1, fontSize: 14 },
  topicsContainer: { marginTop: 8, borderLeftWidth: 2, marginLeft: 8 },
  topicItem: { marginBottom: 2 },
  topicHeader: { flexDirection: 'row', alignItems: 'center', paddingVertical: 10, paddingHorizontal: 12 },
  topicOrderBadge: { width: 26, height: 26, borderRadius: 13, justifyContent: 'center', alignItems: 'center', marginRight: 10 },
  topicOrderText: { fontSize: 12 },
  topicInfo: { flex: 1 },
  topicName: { fontSize: 15 },
  topicDesc: { fontSize: 12, marginTop: 2 },
  topicMeta: { flexDirection: 'row', alignItems: 'center', gap: 4, marginTop: 4 },
  topicMetaText: { fontSize: 11 },
  notesContainer: { marginLeft: 36, marginBottom: 8 },
  uploadButton: { flexDirection: 'row', alignItems: 'center', gap: 6, borderRadius: 12, paddingVertical: 10, paddingHorizontal: 14, marginBottom: 4, borderWidth: 1, borderStyle: 'dashed' },
  uploadButtonText: { fontSize: 13 },
  noteItem: { borderRadius: 12, padding: 12, marginTop: 6, borderWidth: 1 },
  noteHeader: { flexDirection: 'row', alignItems: 'center', gap: 6, marginBottom: 6, flexWrap: 'wrap' },
  noteTitle: { flex: 1, fontSize: 14 },
  aiBadge: { flexDirection: 'row', alignItems: 'center', gap: 3, paddingHorizontal: 6, paddingVertical: 2, borderRadius: 8 },
  aiBadgeText: { fontSize: 10 },
  fileName: { fontSize: 11, marginBottom: 4, fontStyle: 'italic' },
  noteContent: { fontSize: 13, lineHeight: 20 },
  noteDate: { fontSize: 11, marginTop: 6 },
  noContent: { padding: 16, fontStyle: 'italic', fontSize: 13 },
  cardActions: { flexDirection: 'row', gap: 10, marginTop: 12 },
  aiButton: { flex: 1, borderRadius: 12 },
  deleteButton: {},
  addButton: { margin: 16, borderRadius: 12 },
  dialog: { borderRadius: 16 },
  dialogTitleText: { fontSize: 20 },
  allEnrolled: { textAlign: 'center', fontStyle: 'italic', padding: 24 },
  browseItem: { flexDirection: 'row', alignItems: 'center', paddingVertical: 12, paddingHorizontal: 16, borderBottomWidth: 1 },
  browseInfo: { flex: 1, marginRight: 12 },
  browseHeader: { flexDirection: 'row', alignItems: 'center', gap: 8, marginBottom: 4 },
  colorDotSmall: { width: 10, height: 10, borderRadius: 5 },
  browseName: { fontSize: 16 },
  browseDesc: { fontSize: 12, marginBottom: 4 },
  browseTopics: { fontSize: 11 },
  quickUploadBar: { flexDirection: 'row', alignItems: 'center', gap: 10, paddingVertical: 14, paddingHorizontal: 16, borderRadius: 12, marginBottom: 16 },
  quickUploadText: { flex: 1, fontSize: 15 },
});
