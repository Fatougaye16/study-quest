import React, { useState, useEffect } from 'react';
import {
  View, Text, StyleSheet, Modal, TouchableOpacity, TextInput,
  ActivityIndicator, Alert, KeyboardAvoidingView, Platform, ScrollView, FlatList,
} from 'react-native';
import { Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import * as DocumentPicker from 'expo-document-picker';
import { subjectsAPI, enrollmentsAPI } from '../api';
import { Enrollment, Topic } from '../types';
import { useTheme } from '../../../shared/theme';

interface Props {
  visible: boolean;
  topicId?: string;
  topicName?: string;
  onClose: () => void;
  onSuccess: () => void;
}

type Mode = 'pickSubject' | 'pickTopic' | 'menu' | 'paste' | 'uploading';

export default function UploadContentSheet({ visible, topicId: initialTopicId, topicName: initialTopicName, onClose, onSuccess }: Props) {
  const needsPicker = !initialTopicId;
  const { theme } = useTheme();
  const colors = theme.colors;

  const [mode, setMode] = useState<Mode>(needsPicker ? 'pickSubject' : 'menu');
  const [resolvedTopicId, setResolvedTopicId] = useState(initialTopicId ?? '');
  const [resolvedTopicName, setResolvedTopicName] = useState(initialTopicName ?? '');

  // Picker state
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [pickerTopics, setPickerTopics] = useState<Topic[]>([]);
  const [loadingPicker, setLoadingPicker] = useState(false);

  const [pasteTitle, setPasteTitle] = useState('');
  const [pasteContent, setPasteContent] = useState('');
  const [uploading, setUploading] = useState(false);
  const [uploadStatus, setUploadStatus] = useState('');

  // Sync when props change (e.g. opened with a pre-selected topic)
  useEffect(() => {
    if (visible) {
      if (initialTopicId) {
        setResolvedTopicId(initialTopicId);
        setResolvedTopicName(initialTopicName ?? '');
        setMode('menu');
      } else {
        setMode('pickSubject');
        setResolvedTopicId('');
        setResolvedTopicName('');
        loadEnrollments();
      }
    }
  }, [visible, initialTopicId]);

  const loadEnrollments = async () => {
    setLoadingPicker(true);
    try {
      const { data } = await enrollmentsAPI.getAll();
      setEnrollments(data);
    } catch (e) {
      console.error('Failed to load enrollments:', e);
    } finally {
      setLoadingPicker(false);
    }
  };

  const handlePickSubject = async (subjectId: string) => {
    setLoadingPicker(true);
    try {
      const { data } = await subjectsAPI.getTopics(subjectId);
      setPickerTopics(data);
      setMode('pickTopic');
    } catch (e) {
      console.error('Failed to load topics:', e);
      setPickerTopics([]);
    } finally {
      setLoadingPicker(false);
    }
  };

  const handlePickTopic = (topic: Topic) => {
    setResolvedTopicId(topic.id);
    setResolvedTopicName(topic.name);
    setPickerTopics([]);
    setMode('menu');
  };

  const reset = () => {
    setMode(needsPicker ? 'pickSubject' : 'menu');
    setPasteTitle('');
    setPasteContent('');
    setUploading(false);
    setUploadStatus('');
    setResolvedTopicId(initialTopicId ?? '');
    setResolvedTopicName(initialTopicName ?? '');
    setPickerTopics([]);
  };

  const handleClose = () => {
    reset();
    onClose();
  };

  const handlePasteSubmit = async () => {
    if (!pasteTitle.trim() || !pasteContent.trim()) {
      Alert.alert('Missing fields', 'Please enter both a title and content.');
      return;
    }
    setUploading(true);
    setUploadStatus('Saving note...');
    try {
      await subjectsAPI.createNote(resolvedTopicId, pasteTitle.trim(), pasteContent.trim());
      Alert.alert('Success! ✅', 'Your note has been added.');
      reset();
      onSuccess();
      onClose();
    } catch (e: any) {
      const msg = e.response?.data?.detail || 'Failed to save note';
      Alert.alert('Error', msg);
    } finally {
      setUploading(false);
      setUploadStatus('');
    }
  };

  const handleFilePick = async (imageOnly: boolean) => {
    try {
      const result = await DocumentPicker.getDocumentAsync({
        type: imageOnly
          ? ['image/png', 'image/jpeg', 'image/jpg']
          : ['application/pdf', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', 'text/plain'],
        copyToCacheDirectory: true,
      });

      if (result.canceled || !result.assets?.length) return;

      const file = result.assets[0];
      setMode('uploading');
      setUploading(true);
      setUploadStatus(imageOnly ? 'Extracting text from image (OCR)...' : `Processing ${file.name}...`);

      await subjectsAPI.uploadFile(resolvedTopicId, file);

      Alert.alert('Upload Complete! ✅', `"${file.name}" has been processed and added as study material.`);
      reset();
      onSuccess();
      onClose();
    } catch (e: any) {
      const msg = e.response?.data?.detail || e.response?.data?.title || 'Upload failed. Please try again.';
      Alert.alert('Upload Error', msg);
      setMode('menu');
    } finally {
      setUploading(false);
      setUploadStatus('');
    }
  };

  return (
    <Modal visible={visible} animationType="slide" transparent onRequestClose={handleClose}>
      <KeyboardAvoidingView style={styles.overlay} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
        <View style={[styles.sheet, { backgroundColor: colors.card }]}>
          <View style={styles.header}>
            <Text style={[styles.headerTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Upload Content</Text>
            <TouchableOpacity onPress={handleClose} disabled={uploading}>
              <Feather name="x" size={24} color={colors.textSecondary} />
            </TouchableOpacity>
          </View>
          <Text style={[styles.topicLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
            {resolvedTopicName ? `Topic: ${resolvedTopicName}` : 'Select a subject and topic'}
            {resolvedTopicName && !initialTopicId ? (
              <Text style={[styles.changeTopic, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]} onPress={() => { setMode('pickSubject'); loadEnrollments(); }}> (change)</Text>
            ) : null}
          </Text>

          {mode === 'pickSubject' && (
            <View style={styles.pickerContainer}>
              <Text style={[styles.pickerTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Select a Subject</Text>
              {loadingPicker ? (
                <ActivityIndicator size="small" color={colors.primary} style={{ padding: 24 }} />
              ) : (
                <FlatList
                  data={enrollments}
                  keyExtractor={item => item.id}
                  style={{ maxHeight: 300 }}
                  renderItem={({ item }) => (
                    <TouchableOpacity style={[styles.pickerItem, { borderBottomColor: colors.border }]} onPress={() => handlePickSubject(item.subjectId)}>
                      <View style={[styles.pickerDot, { backgroundColor: item.subjectColor }]} />
                      <Text style={[styles.pickerItemText, { color: colors.text, fontFamily: theme.fonts.body }]}>{item.subjectName}</Text>
                      <Feather name="chevron-right" size={18} color={colors.textTertiary} />
                    </TouchableOpacity>
                  )}
                  ListEmptyComponent={<Text style={[styles.pickerEmpty, { color: colors.textTertiary }]}>No subjects enrolled yet.</Text>}
                />
              )}
            </View>
          )}

          {mode === 'pickTopic' && (
            <View style={styles.pickerContainer}>
              <TouchableOpacity style={styles.pickerBack} onPress={() => setMode('pickSubject')}>
                <Feather name="arrow-left" size={18} color={colors.primary} />
                <Text style={[styles.pickerBackText, { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>Back to subjects</Text>
              </TouchableOpacity>
              <Text style={[styles.pickerTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Select a Topic</Text>
              {loadingPicker ? (
                <ActivityIndicator size="small" color={colors.primary} style={{ padding: 24 }} />
              ) : (
                <FlatList
                  data={pickerTopics}
                  keyExtractor={item => item.id}
                  style={{ maxHeight: 300 }}
                  renderItem={({ item }) => (
                    <TouchableOpacity style={[styles.pickerItem, { borderBottomColor: colors.border }]} onPress={() => handlePickTopic(item)}>
                      <View style={[styles.pickerOrder, { backgroundColor: colors.primary + '15' }]}>
                        <Text style={[styles.pickerOrderText, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>{item.order}</Text>
                      </View>
                      <Text style={[styles.pickerItemText, { color: colors.text, fontFamily: theme.fonts.body }]}>{item.name}</Text>
                      <Feather name="chevron-right" size={18} color={colors.textTertiary} />
                    </TouchableOpacity>
                  )}
                  ListEmptyComponent={<Text style={[styles.pickerEmpty, { color: colors.textTertiary }]}>No topics available.</Text>}
                />
              )}
            </View>
          )}

          {mode === 'uploading' && (
            <View style={styles.uploadingContainer}>
              <ActivityIndicator size="large" color={colors.primary} />
              <Text style={[styles.uploadingText, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{uploadStatus}</Text>
              <Text style={[styles.uploadingHint, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>This may take a moment for images and large files...</Text>
            </View>
          )}

          {mode === 'menu' && !uploading && (
            <View style={styles.optionsContainer}>
              <TouchableOpacity style={[styles.optionCard, { backgroundColor: colors.surface, borderColor: colors.border }]} onPress={() => setMode('paste')} activeOpacity={0.7}>
                <View style={[styles.optionIcon, { backgroundColor: colors.primary + '20' }]}>
                  <Feather name="edit" size={28} color={colors.primary} />
                </View>
                <View style={styles.optionInfo}>
                  <Text style={[styles.optionTitle, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Paste Text</Text>
                  <Text style={[styles.optionDesc, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Type or paste study notes, syllabus content, or past paper questions</Text>
                </View>
                <Feather name="chevron-right" size={20} color={colors.textTertiary} />
              </TouchableOpacity>

              <TouchableOpacity style={[styles.optionCard, { backgroundColor: colors.surface, borderColor: colors.border }]} onPress={() => handleFilePick(false)} activeOpacity={0.7}>
                <View style={[styles.optionIcon, { backgroundColor: colors.accent + '20' }]}>
                  <Feather name="file-text" size={28} color={colors.accent} />
                </View>
                <View style={styles.optionInfo}>
                  <Text style={[styles.optionTitle, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Upload Document</Text>
                  <Text style={[styles.optionDesc, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>PDF, DOCX, or TXT files — text will be extracted automatically</Text>
                </View>
                <Feather name="chevron-right" size={20} color={colors.textTertiary} />
              </TouchableOpacity>

              <TouchableOpacity style={[styles.optionCard, { backgroundColor: colors.surface, borderColor: colors.border }]} onPress={() => handleFilePick(true)} activeOpacity={0.7}>
                <View style={[styles.optionIcon, { backgroundColor: colors.gamification.xp + '20' }]}>
                  <Feather name="image" size={28} color={colors.gamification.xp} />
                </View>
                <View style={styles.optionInfo}>
                  <Text style={[styles.optionTitle, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Upload Image</Text>
                  <Text style={[styles.optionDesc, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>Photos of textbook pages, past papers, or handwritten notes (OCR)</Text>
                </View>
                <Feather name="chevron-right" size={20} color={colors.textTertiary} />
              </TouchableOpacity>

              <View style={[styles.hintBox, { backgroundColor: colors.primary + '10' }]}>
                <Feather name="info" size={16} color={colors.primary} />
                <Text style={[styles.hintText, { color: colors.primary, fontFamily: theme.fonts.body }]}>
                  Uploaded content will be used by AI tools (summaries, quizzes, flashcards) for WAEC-accurate responses.
                </Text>
              </View>
            </View>
          )}

          {mode === 'paste' && !uploading && (
            <ScrollView style={styles.pasteContainer} keyboardShouldPersistTaps="handled">
              <Text style={[styles.inputLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Title</Text>
              <TextInput
                style={[styles.titleInput, { borderColor: colors.border, color: colors.text, backgroundColor: colors.surface }]}
                value={pasteTitle}
                onChangeText={setPasteTitle}
                placeholder="e.g. Quadratic Equations — WAEC 2024"
                placeholderTextColor={colors.textTertiary}
                maxLength={280}
              />

              <Text style={[styles.inputLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Content</Text>
              <TextInput
                style={[styles.contentInput, { borderColor: colors.border, color: colors.text, backgroundColor: colors.surface }]}
                value={pasteContent}
                onChangeText={setPasteContent}
                placeholder="Paste your study material, past paper questions, syllabus text, or notes here..."
                placeholderTextColor={colors.textTertiary}
                multiline
                textAlignVertical="top"
              />

              <View style={styles.pasteActions}>
                <Button
                  mode="outlined"
                  onPress={() => setMode('menu')}
                  style={[styles.backButton, { borderColor: colors.border }]}
                  textColor={colors.textSecondary}
                >
                  Back
                </Button>
                <Button
                  mode="contained"
                  onPress={handlePasteSubmit}
                  buttonColor={colors.primary}
                  style={styles.submitButton}
                  loading={uploading}
                  disabled={!pasteTitle.trim() || !pasteContent.trim()}
                >
                  Save Note
                </Button>
              </View>
            </ScrollView>
          )}
        </View>
      </KeyboardAvoidingView>
    </Modal>
  );
}

const styles = StyleSheet.create({
  overlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.5)',
    justifyContent: 'flex-end',
  },
  sheet: {
    borderTopLeftRadius: 24,
    borderTopRightRadius: 24,
    paddingHorizontal: 20,
    paddingBottom: 32,
    maxHeight: '90%',
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingTop: 20,
    paddingBottom: 8,
  },
  headerTitle: {
    fontSize: 20,
  },
  topicLabel: {
    fontSize: 13,
    marginBottom: 16,
  },
  changeTopic: {},
  pickerContainer: {
    paddingBottom: 8,
  },
  pickerTitle: {
    fontSize: 16,
    marginBottom: 12,
  },
  pickerItem: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
    paddingVertical: 14,
    paddingHorizontal: 4,
    borderBottomWidth: 1,
  },
  pickerDot: {
    width: 12,
    height: 12,
    borderRadius: 6,
  },
  pickerOrder: {
    width: 26,
    height: 26,
    borderRadius: 13,
    justifyContent: 'center',
    alignItems: 'center',
  },
  pickerOrderText: {
    fontSize: 12,
  },
  pickerItemText: {
    flex: 1,
    fontSize: 15,
  },
  pickerEmpty: {
    textAlign: 'center',
    fontStyle: 'italic',
    padding: 24,
  },
  pickerBack: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
    marginBottom: 12,
  },
  pickerBackText: {
    fontSize: 14,
  },
  optionsContainer: {
    gap: 12,
    paddingBottom: 8,
  },
  optionCard: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 14,
    padding: 16,
    borderWidth: 1,
  },
  optionIcon: {
    width: 48,
    height: 48,
    borderRadius: 12,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 14,
  },
  optionInfo: {
    flex: 1,
  },
  optionTitle: {
    fontSize: 16,
    marginBottom: 2,
  },
  optionDesc: {
    fontSize: 12,
    lineHeight: 17,
  },
  hintBox: {
    flexDirection: 'row',
    alignItems: 'flex-start',
    gap: 8,
    padding: 12,
    borderRadius: 10,
    marginTop: 4,
  },
  hintText: {
    flex: 1,
    fontSize: 12,
    lineHeight: 17,
  },
  uploadingContainer: {
    alignItems: 'center',
    paddingVertical: 48,
    gap: 16,
  },
  uploadingText: {
    fontSize: 16,
  },
  uploadingHint: {
    fontSize: 13,
  },
  pasteContainer: {
    maxHeight: 400,
  },
  inputLabel: {
    fontSize: 14,
    marginBottom: 6,
    marginTop: 8,
  },
  titleInput: {
    borderWidth: 1.5,
    borderRadius: 10,
    padding: 14,
    fontSize: 15,
    marginBottom: 12,
  },
  contentInput: {
    borderWidth: 1.5,
    borderRadius: 10,
    padding: 14,
    fontSize: 14,
    minHeight: 160,
    maxHeight: 240,
  },
  pasteActions: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 16,
    marginBottom: 8,
  },
  backButton: {
    flex: 1,
  },
  submitButton: {
    flex: 2,
    borderRadius: 10,
  },
});
