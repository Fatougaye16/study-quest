import React, { useState, useEffect } from 'react';
import {
  View, Text, StyleSheet, Modal, TouchableOpacity, TextInput,
  ActivityIndicator, Alert, KeyboardAvoidingView, Platform, ScrollView, FlatList,
} from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import * as DocumentPicker from 'expo-document-picker';
import { subjectsAPI, enrollmentsAPI } from '../api';
import { Enrollment, Topic } from '../types';

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
        <View style={styles.sheet}>
          <View style={styles.header}>
            <Text style={styles.headerTitle}>Upload Content</Text>
            <TouchableOpacity onPress={handleClose} disabled={uploading}>
              <Ionicons name="close" size={24} color="#64748b" />
            </TouchableOpacity>
          </View>
          <Text style={styles.topicLabel}>
            {resolvedTopicName ? `Topic: ${resolvedTopicName}` : 'Select a subject and topic'}
            {resolvedTopicName && !initialTopicId ? (
              <Text style={styles.changeTopic} onPress={() => { setMode('pickSubject'); loadEnrollments(); }}> (change)</Text>
            ) : null}
          </Text>

          {mode === 'pickSubject' && (
            <View style={styles.pickerContainer}>
              <Text style={styles.pickerTitle}>Select a Subject</Text>
              {loadingPicker ? (
                <ActivityIndicator size="small" color="#0ea5e9" style={{ padding: 24 }} />
              ) : (
                <FlatList
                  data={enrollments}
                  keyExtractor={item => item.id}
                  style={{ maxHeight: 300 }}
                  renderItem={({ item }) => (
                    <TouchableOpacity style={styles.pickerItem} onPress={() => handlePickSubject(item.subjectId)}>
                      <View style={[styles.pickerDot, { backgroundColor: item.subjectColor }]} />
                      <Text style={styles.pickerItemText}>{item.subjectName}</Text>
                      <Ionicons name="chevron-forward" size={18} color="#94a3b8" />
                    </TouchableOpacity>
                  )}
                  ListEmptyComponent={<Text style={styles.pickerEmpty}>No subjects enrolled yet.</Text>}
                />
              )}
            </View>
          )}

          {mode === 'pickTopic' && (
            <View style={styles.pickerContainer}>
              <TouchableOpacity style={styles.pickerBack} onPress={() => setMode('pickSubject')}>
                <Ionicons name="arrow-back" size={18} color="#0ea5e9" />
                <Text style={styles.pickerBackText}>Back to subjects</Text>
              </TouchableOpacity>
              <Text style={styles.pickerTitle}>Select a Topic</Text>
              {loadingPicker ? (
                <ActivityIndicator size="small" color="#0ea5e9" style={{ padding: 24 }} />
              ) : (
                <FlatList
                  data={pickerTopics}
                  keyExtractor={item => item.id}
                  style={{ maxHeight: 300 }}
                  renderItem={({ item }) => (
                    <TouchableOpacity style={styles.pickerItem} onPress={() => handlePickTopic(item)}>
                      <View style={styles.pickerOrder}>
                        <Text style={styles.pickerOrderText}>{item.order}</Text>
                      </View>
                      <Text style={styles.pickerItemText}>{item.name}</Text>
                      <Ionicons name="chevron-forward" size={18} color="#94a3b8" />
                    </TouchableOpacity>
                  )}
                  ListEmptyComponent={<Text style={styles.pickerEmpty}>No topics available.</Text>}
                />
              )}
            </View>
          )}

          {mode === 'uploading' && (
            <View style={styles.uploadingContainer}>
              <ActivityIndicator size="large" color="#0ea5e9" />
              <Text style={styles.uploadingText}>{uploadStatus}</Text>
              <Text style={styles.uploadingHint}>This may take a moment for images and large files...</Text>
            </View>
          )}

          {mode === 'menu' && !uploading && (
            <View style={styles.optionsContainer}>
              <TouchableOpacity style={styles.optionCard} onPress={() => setMode('paste')} activeOpacity={0.7}>
                <View style={[styles.optionIcon, { backgroundColor: '#dbeafe' }]}>
                  <Ionicons name="create-outline" size={28} color="#0ea5e9" />
                </View>
                <View style={styles.optionInfo}>
                  <Text style={styles.optionTitle}>Paste Text</Text>
                  <Text style={styles.optionDesc}>Type or paste study notes, syllabus content, or past paper questions</Text>
                </View>
                <Ionicons name="chevron-forward" size={20} color="#94a3b8" />
              </TouchableOpacity>

              <TouchableOpacity style={styles.optionCard} onPress={() => handleFilePick(false)} activeOpacity={0.7}>
                <View style={[styles.optionIcon, { backgroundColor: '#fef3c7' }]}>
                  <Ionicons name="document-outline" size={28} color="#f59e0b" />
                </View>
                <View style={styles.optionInfo}>
                  <Text style={styles.optionTitle}>Upload Document</Text>
                  <Text style={styles.optionDesc}>PDF, DOCX, or TXT files — text will be extracted automatically</Text>
                </View>
                <Ionicons name="chevron-forward" size={20} color="#94a3b8" />
              </TouchableOpacity>

              <TouchableOpacity style={styles.optionCard} onPress={() => handleFilePick(true)} activeOpacity={0.7}>
                <View style={[styles.optionIcon, { backgroundColor: '#d1fae5' }]}>
                  <Ionicons name="image-outline" size={28} color="#10b981" />
                </View>
                <View style={styles.optionInfo}>
                  <Text style={styles.optionTitle}>Upload Image</Text>
                  <Text style={styles.optionDesc}>Photos of textbook pages, past papers, or handwritten notes (OCR)</Text>
                </View>
                <Ionicons name="chevron-forward" size={20} color="#94a3b8" />
              </TouchableOpacity>

              <View style={styles.hintBox}>
                <Ionicons name="information-circle-outline" size={16} color="#0ea5e9" />
                <Text style={styles.hintText}>
                  Uploaded content will be used by AI tools (summaries, quizzes, flashcards) for WAEC-accurate responses.
                </Text>
              </View>
            </View>
          )}

          {mode === 'paste' && !uploading && (
            <ScrollView style={styles.pasteContainer} keyboardShouldPersistTaps="handled">
              <Text style={styles.inputLabel}>Title</Text>
              <TextInput
                style={styles.titleInput}
                value={pasteTitle}
                onChangeText={setPasteTitle}
                placeholder="e.g. Quadratic Equations — WAEC 2024"
                placeholderTextColor="#94a3b8"
                maxLength={280}
              />

              <Text style={styles.inputLabel}>Content</Text>
              <TextInput
                style={styles.contentInput}
                value={pasteContent}
                onChangeText={setPasteContent}
                placeholder="Paste your study material, past paper questions, syllabus text, or notes here..."
                placeholderTextColor="#94a3b8"
                multiline
                textAlignVertical="top"
              />

              <View style={styles.pasteActions}>
                <Button
                  mode="outlined"
                  onPress={() => setMode('menu')}
                  style={styles.backButton}
                  textColor="#64748b"
                >
                  Back
                </Button>
                <Button
                  mode="contained"
                  onPress={handlePasteSubmit}
                  buttonColor="#0ea5e9"
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
    backgroundColor: '#fff',
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
    fontWeight: 'bold',
    color: '#1e293b',
  },
  topicLabel: {
    fontSize: 13,
    color: '#64748b',
    marginBottom: 16,
  },
  changeTopic: {
    color: '#0ea5e9',
    fontWeight: '600',
  },
  pickerContainer: {
    paddingBottom: 8,
  },
  pickerTitle: {
    fontSize: 16,
    fontWeight: '700',
    color: '#1e293b',
    marginBottom: 12,
  },
  pickerItem: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
    paddingVertical: 14,
    paddingHorizontal: 4,
    borderBottomWidth: 1,
    borderBottomColor: '#f1f5f9',
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
    backgroundColor: '#e0f2fe',
    justifyContent: 'center',
    alignItems: 'center',
  },
  pickerOrderText: {
    fontSize: 12,
    fontWeight: '700',
    color: '#0ea5e9',
  },
  pickerItemText: {
    flex: 1,
    fontSize: 15,
    color: '#1e293b',
  },
  pickerEmpty: {
    textAlign: 'center',
    color: '#94a3b8',
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
    color: '#0ea5e9',
    fontWeight: '600',
  },
  optionsContainer: {
    gap: 12,
    paddingBottom: 8,
  },
  optionCard: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#f8fafc',
    borderRadius: 14,
    padding: 16,
    borderWidth: 1,
    borderColor: '#f1f5f9',
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
    fontWeight: '600',
    color: '#1e293b',
    marginBottom: 2,
  },
  optionDesc: {
    fontSize: 12,
    color: '#64748b',
    lineHeight: 17,
  },
  hintBox: {
    flexDirection: 'row',
    alignItems: 'flex-start',
    gap: 8,
    backgroundColor: '#f0f9ff',
    padding: 12,
    borderRadius: 10,
    marginTop: 4,
  },
  hintText: {
    flex: 1,
    fontSize: 12,
    color: '#0369a1',
    lineHeight: 17,
  },
  uploadingContainer: {
    alignItems: 'center',
    paddingVertical: 48,
    gap: 16,
  },
  uploadingText: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1e293b',
  },
  uploadingHint: {
    fontSize: 13,
    color: '#94a3b8',
  },
  pasteContainer: {
    maxHeight: 400,
  },
  inputLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1e293b',
    marginBottom: 6,
    marginTop: 8,
  },
  titleInput: {
    borderWidth: 1.5,
    borderColor: '#e2e8f0',
    borderRadius: 10,
    padding: 14,
    fontSize: 15,
    color: '#1e293b',
    backgroundColor: '#f8fafc',
    marginBottom: 12,
  },
  contentInput: {
    borderWidth: 1.5,
    borderColor: '#e2e8f0',
    borderRadius: 10,
    padding: 14,
    fontSize: 14,
    color: '#1e293b',
    backgroundColor: '#f8fafc',
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
    borderColor: '#e2e8f0',
  },
  submitButton: {
    flex: 2,
    borderRadius: 10,
  },
});
