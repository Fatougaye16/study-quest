import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, RefreshControl, TouchableOpacity, ActivityIndicator } from 'react-native';
import { Button, Card, Dialog, Portal } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { useNavigation } from '@react-navigation/native';
import { subjectsAPI, enrollmentsAPI } from './api';
import { Subject, Enrollment, Topic, Note, NoteSourceType } from './types';
import UploadContentSheet from './components/UploadContentSheet';

export default function CoursesScreen() {
  const navigation = useNavigation<any>();
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
    <View style={styles.container}>
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
      >
        {enrollments.length === 0 ? (
          <View style={styles.emptyState}>
            <Text style={styles.emptyIcon}>📚</Text>
            <Text style={styles.emptyText}>No subjects enrolled yet!</Text>
            <Text style={styles.emptySubtext}>Browse available subjects to get started</Text>
          </View>
        ) : (
          enrollments.map(enrollment => {
            const isExpanded = expandedSubject === enrollment.subjectId;
            return (
              <Card
                key={enrollment.id}
                style={[styles.courseCard, { borderTopColor: enrollment.subjectColor }]}
              >
                <Card.Content>
                  <View style={styles.courseHeader}>
                    <View style={styles.courseInfo}>
                      <Text style={styles.courseName}>{enrollment.subjectName}</Text>
                      <Text style={styles.courseCode}>Grade {enrollment.grade}</Text>
                      <Text style={styles.enrollDate}>
                        Enrolled {new Date(enrollment.enrolledAt).toLocaleDateString()}
                      </Text>
                    </View>
                    <View style={[styles.colorDot, { backgroundColor: enrollment.subjectColor }]} />
                  </View>

                  <TouchableOpacity
                    style={styles.topicsToggle}
                    onPress={() => toggleSubjectTopics(enrollment.subjectId)}
                    activeOpacity={0.7}
                  >
                    <Ionicons name="library-outline" size={18} color="#0ea5e9" />
                    <Text style={styles.topicsToggleText}>
                      {isExpanded ? 'Hide Topics' : 'View Topics & Notes'}
                    </Text>
                    <Ionicons
                      name={isExpanded ? 'chevron-up' : 'chevron-down'}
                      size={18}
                      color="#0ea5e9"
                    />
                  </TouchableOpacity>

                  {isExpanded && (
                    <View style={styles.topicsContainer}>
                      {loadingTopics ? (
                        <ActivityIndicator size="small" color="#0ea5e9" style={{ padding: 16 }} />
                      ) : topics.length === 0 ? (
                        <Text style={styles.noContent}>No topics available yet.</Text>
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
                                <View style={styles.topicOrderBadge}>
                                  <Text style={styles.topicOrderText}>{topic.order}</Text>
                                </View>
                                <View style={styles.topicInfo}>
                                  <Text style={styles.topicName}>{topic.name}</Text>
                                  {topic.description ? (
                                    <Text style={styles.topicDesc} numberOfLines={1}>{topic.description}</Text>
                                  ) : null}
                                  <View style={styles.topicMeta}>
                                    <Ionicons name="document-text-outline" size={12} color="#94a3b8" />
                                    <Text style={styles.topicMetaText}>{topic.noteCount} notes</Text>
                                    <Ionicons name="help-circle-outline" size={12} color="#94a3b8" style={{ marginLeft: 8 }} />
                                    <Text style={styles.topicMetaText}>{topic.questionCount} questions</Text>
                                  </View>
                                </View>
                                <Ionicons
                                  name={isTopicOpen ? 'chevron-up' : 'chevron-forward'}
                                  size={16}
                                  color="#94a3b8"
                                />
                              </TouchableOpacity>

                              {isTopicOpen && (
                                <View style={styles.notesContainer}>
                                  <TouchableOpacity
                                    style={styles.uploadButton}
                                    onPress={() => { setUploadTopicId(topic.id); setUploadTopicName(topic.name); }}
                                    activeOpacity={0.7}
                                  >
                                    <Ionicons name="cloud-upload-outline" size={16} color="#0ea5e9" />
                                    <Text style={styles.uploadButtonText}>Upload Content</Text>
                                  </TouchableOpacity>
                                  {loadingNotes ? (
                                    <ActivityIndicator size="small" color="#0ea5e9" style={{ padding: 12 }} />
                                  ) : notes.length === 0 ? (
                                    <Text style={styles.noContent}>No notes for this topic yet.</Text>
                                  ) : (
                                    notes.map(note => (
                                      <View key={note.id} style={styles.noteItem}>
                                        <View style={styles.noteHeader}>
                                          <Ionicons name={getSourceIcon(note.sourceType)} size={16} color={getSourceColor(note.sourceType)} />
                                          <Text style={styles.noteTitle} numberOfLines={1}>{note.title}</Text>
                                          {note.isOfficial && (
                                            <View style={[styles.aiBadge, { backgroundColor: '#d1fae5' }]}>
                                              <Ionicons name="shield-checkmark" size={10} color="#10b981" />
                                              <Text style={[styles.aiBadgeText, { color: '#10b981' }]}>Official</Text>
                                            </View>
                                          )}
                                          {note.isAIGenerated && (
                                            <View style={styles.aiBadge}>
                                              <Ionicons name="sparkles" size={10} color="#0ea5e9" />
                                              <Text style={styles.aiBadgeText}>AI</Text>
                                            </View>
                                          )}
                                          {note.sourceType !== NoteSourceType.Manual && (
                                            <View style={[styles.aiBadge, { backgroundColor: '#fef3c7' }]}>
                                              <Text style={[styles.aiBadgeText, { color: '#d97706' }]}>{getSourceLabel(note.sourceType)}</Text>
                                            </View>
                                          )}
                                        </View>
                                        {note.originalFileName && (
                                          <Text style={styles.fileName}>{note.originalFileName}</Text>
                                        )}
                                        <Text style={styles.noteContent} numberOfLines={4}>{note.content}</Text>
                                        <Text style={styles.noteDate}>
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
                      buttonColor="#0ea5e9"
                      icon="creation"
                      compact
                      style={styles.aiButton}
                    >
                      AI Tools
                    </Button>
                    <Button
                      mode="outlined"
                      onPress={() => handleUnenroll(enrollment.id)}
                      style={styles.deleteButton}
                      textColor="#ef4444"
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
        style={styles.addButton}
        icon="plus"
      >
        Browse Subjects
      </Button>

      <Portal>
        <Dialog visible={showBrowse} onDismiss={() => setShowBrowse(false)} style={styles.dialog}>
          <Dialog.Title>
            <Text style={styles.dialogTitleText}>📚 Available Subjects</Text>
          </Dialog.Title>
          <Dialog.ScrollArea style={{ maxHeight: 400 }}>
            <ScrollView>
              {unenrolledSubjects.length === 0 ? (
                <Text style={styles.allEnrolled}>You're enrolled in all available subjects!</Text>
              ) : (
                unenrolledSubjects.map(subject => (
                  <View key={subject.id} style={styles.browseItem}>
                    <View style={styles.browseInfo}>
                      <View style={styles.browseHeader}>
                        <View style={[styles.colorDotSmall, { backgroundColor: subject.color }]} />
                        <Text style={styles.browseName}>{subject.name}</Text>
                      </View>
                      <Text style={styles.browseDesc} numberOfLines={2}>{subject.description}</Text>
                      <Text style={styles.browseTopics}>{subject.topicCount} topics</Text>
                    </View>
                    <Button
                      mode="contained"
                      onPress={() => handleEnroll(subject.id)}
                      loading={enrolling === subject.id}
                      disabled={enrolling !== null}
                      buttonColor="#0ea5e9"
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
            <Button onPress={() => setShowBrowse(false)} textColor="#64748b">Close</Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>

      <UploadContentSheet
        visible={uploadTopicId !== null}
        topicId={uploadTopicId ?? ''}
        topicName={uploadTopicName}
        onClose={() => setUploadTopicId(null)}
        onSuccess={() => {
          if (uploadTopicId) toggleTopicNotes(uploadTopicId);
        }}
      />
    </View>
  );
}

function getSourceIcon(sourceType: number): any {
  switch (sourceType) {
    case NoteSourceType.Pdf: return 'document-attach';
    case NoteSourceType.Document: return 'document-text';
    case NoteSourceType.Image: return 'image';
    default: return 'create';
  }
}

function getSourceColor(sourceType: number): string {
  switch (sourceType) {
    case NoteSourceType.Pdf: return '#ef4444';
    case NoteSourceType.Document: return '#f59e0b';
    case NoteSourceType.Image: return '#10b981';
    default: return '#0ea5e9';
  }
}

function getSourceLabel(sourceType: number): string {
  switch (sourceType) {
    case NoteSourceType.Pdf: return 'PDF';
    case NoteSourceType.Document: return 'DOC';
    case NoteSourceType.Image: return 'IMG';
    default: return '';
  }
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  scrollView: { flex: 1, padding: 16 },
  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 80 },
  emptyIcon: { fontSize: 64, marginBottom: 16 },
  emptyText: { fontSize: 20, fontWeight: 'bold', color: '#64748b', marginBottom: 8 },
  emptySubtext: { fontSize: 14, color: '#94a3b8' },
  courseCard: { marginBottom: 12, borderTopWidth: 4 },
  courseHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' },
  courseInfo: { flex: 1 },
  courseName: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', marginBottom: 4 },
  courseCode: { fontSize: 14, color: '#64748b', marginBottom: 2 },
  enrollDate: { fontSize: 12, color: '#94a3b8' },
  colorDot: { width: 16, height: 16, borderRadius: 8, marginTop: 4 },
  topicsToggle: { flexDirection: 'row', alignItems: 'center', gap: 8, marginTop: 14, paddingVertical: 10, paddingHorizontal: 12, backgroundColor: '#f0f9ff', borderRadius: 10 },
  topicsToggleText: { flex: 1, fontSize: 14, fontWeight: '600', color: '#0ea5e9' },
  topicsContainer: { marginTop: 8, borderLeftWidth: 2, borderLeftColor: '#e2e8f0', marginLeft: 8 },
  topicItem: { marginBottom: 2 },
  topicHeader: { flexDirection: 'row', alignItems: 'center', paddingVertical: 10, paddingHorizontal: 12 },
  topicOrderBadge: { width: 26, height: 26, borderRadius: 13, backgroundColor: '#e0f2fe', justifyContent: 'center', alignItems: 'center', marginRight: 10 },
  topicOrderText: { fontSize: 12, fontWeight: '700', color: '#0ea5e9' },
  topicInfo: { flex: 1 },
  topicName: { fontSize: 15, fontWeight: '600', color: '#1e293b' },
  topicDesc: { fontSize: 12, color: '#64748b', marginTop: 2 },
  topicMeta: { flexDirection: 'row', alignItems: 'center', gap: 4, marginTop: 4 },
  topicMetaText: { fontSize: 11, color: '#94a3b8' },
  notesContainer: { marginLeft: 36, marginBottom: 8 },
  uploadButton: { flexDirection: 'row', alignItems: 'center', gap: 6, backgroundColor: '#f0f9ff', borderRadius: 10, paddingVertical: 10, paddingHorizontal: 14, marginBottom: 4, borderWidth: 1, borderColor: '#bae6fd', borderStyle: 'dashed' },
  uploadButtonText: { fontSize: 13, fontWeight: '600', color: '#0ea5e9' },
  noteItem: { backgroundColor: '#fff', borderRadius: 10, padding: 12, marginTop: 6, borderWidth: 1, borderColor: '#f1f5f9' },
  noteHeader: { flexDirection: 'row', alignItems: 'center', gap: 6, marginBottom: 6, flexWrap: 'wrap' },
  noteTitle: { flex: 1, fontSize: 14, fontWeight: '600', color: '#1e293b' },
  aiBadge: { flexDirection: 'row', alignItems: 'center', gap: 3, backgroundColor: '#f0f9ff', paddingHorizontal: 6, paddingVertical: 2, borderRadius: 8 },
  aiBadgeText: { fontSize: 10, fontWeight: '700', color: '#0ea5e9' },
  fileName: { fontSize: 11, color: '#94a3b8', marginBottom: 4, fontStyle: 'italic' },
  noteContent: { fontSize: 13, color: '#475569', lineHeight: 20 },
  noteDate: { fontSize: 11, color: '#94a3b8', marginTop: 6 },
  noContent: { padding: 16, color: '#94a3b8', fontStyle: 'italic', fontSize: 13 },
  cardActions: { flexDirection: 'row', gap: 10, marginTop: 12 },
  aiButton: { flex: 1, borderRadius: 8 },
  deleteButton: { borderColor: '#ef4444' },
  addButton: { margin: 16, backgroundColor: '#0ea5e9' },
  dialog: { borderRadius: 16 },
  dialogTitleText: { fontSize: 20, fontWeight: 'bold', color: '#1e293b' },
  allEnrolled: { textAlign: 'center', color: '#94a3b8', fontStyle: 'italic', padding: 24 },
  browseItem: { flexDirection: 'row', alignItems: 'center', paddingVertical: 12, paddingHorizontal: 16, borderBottomWidth: 1, borderBottomColor: '#f1f5f9' },
  browseInfo: { flex: 1, marginRight: 12 },
  browseHeader: { flexDirection: 'row', alignItems: 'center', gap: 8, marginBottom: 4 },
  colorDotSmall: { width: 10, height: 10, borderRadius: 5 },
  browseName: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  browseDesc: { fontSize: 12, color: '#64748b', marginBottom: 4 },
  browseTopics: { fontSize: 11, color: '#0ea5e9', fontWeight: '600' },
});
