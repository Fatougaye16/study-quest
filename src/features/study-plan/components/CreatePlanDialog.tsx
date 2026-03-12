import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity } from 'react-native';
import { Button, Dialog, Portal, TextInput, Checkbox } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { Enrollment, Topic } from '../../courses/types';
import { subjectsAPI } from '../../courses/api';
import CalendarPicker from './CalendarPicker';

interface Props {
  visible: boolean;
  enrollments: Enrollment[];
  saving: boolean;
  onDismiss: () => void;
  onCreate: (data: {
    subjectId: string;
    title: string;
    startDate: string;
    endDate: string;
    topics: Topic[];
    selectedTopicIds: Set<string>;
    duration: number;
  }) => void;
}

export default function CreatePlanDialog({ visible, enrollments, saving, onDismiss, onCreate }: Props) {
  const [selectedSubject, setSelectedSubject] = useState('');
  const [title, setTitle] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [topics, setTopics] = useState<Topic[]>([]);
  const [selectedTopics, setSelectedTopics] = useState<Set<string>>(new Set());
  const [duration, setDuration] = useState('30');
  const [showSubjectPicker, setShowSubjectPicker] = useState(false);

  const [showStartPicker, setShowStartPicker] = useState(false);
  const [showEndPicker, setShowEndPicker] = useState(false);
  const [pickerDate, setPickerDate] = useState(new Date());
  const [pickerTarget, setPickerTarget] = useState<'start' | 'end'>('start');

  useEffect(() => {
    if (!visible) {
      setTitle('');
      setStartDate('');
      setEndDate('');
      setSelectedSubject('');
      setShowSubjectPicker(false);
      setTopics([]);
      setSelectedTopics(new Set());
      setDuration('30');
    }
  }, [visible]);

  useEffect(() => {
    if (!selectedSubject) { setTopics([]); return; }
    (async () => {
      try {
        const { data } = await subjectsAPI.getTopics(selectedSubject);
        setTopics(data);
        setSelectedTopics(new Set(data.map((t: Topic) => t.id)));
      } catch (err) {
        console.error('Failed to load topics:', err);
      }
    })();
  }, [selectedSubject]);

  const toggleTopic = (topicId: string) => {
    setSelectedTopics(prev => {
      const next = new Set(prev);
      if (next.has(topicId)) next.delete(topicId);
      else next.add(topicId);
      return next;
    });
  };

  const handleSubmit = () => {
    onCreate({
      subjectId: selectedSubject,
      title,
      startDate,
      endDate,
      topics,
      selectedTopicIds: selectedTopics,
      duration: parseInt(duration) || 30,
    });
  };

  const activePickerVisible = showStartPicker || showEndPicker;
  const activeSelectedDate = pickerTarget === 'start' ? startDate : endDate;

  return (
    <>
      <Portal>
        <Dialog visible={visible} onDismiss={onDismiss} style={styles.dialog}>
          <Dialog.Title>
            <Text style={styles.dialogTitleText}>📚 Create Study Plan</Text>
          </Dialog.Title>
          <Dialog.ScrollArea>
            <ScrollView>
              <Dialog.Content>
                <Text style={styles.formLabel}>Title *</Text>
                <TextInput value={title} onChangeText={setTitle} mode="outlined" placeholder="e.g. Exam Preparation" outlineColor="#e2e8f0" activeOutlineColor="#0ea5e9" style={styles.input} />

                <Text style={styles.formLabel}>Subject *</Text>
                <TouchableOpacity
                  style={styles.pickerButton}
                  onPress={() => setShowSubjectPicker(!showSubjectPicker)}
                >
                  <Text style={selectedSubject ? styles.pickerButtonText : styles.pickerButtonPlaceholder}>
                    {selectedSubject
                      ? enrollments.find(e => e.subjectId === selectedSubject)?.subjectName
                      : 'Tap to select a subject'}
                  </Text>
                  <Ionicons name={showSubjectPicker ? 'chevron-up' : 'chevron-down'} size={20} color="#94a3b8" />
                </TouchableOpacity>
                {showSubjectPicker && (
                  <View style={styles.optionList}>
                    {enrollments.map(e => (
                      <TouchableOpacity
                        key={e.subjectId}
                        style={[styles.optionItem, selectedSubject === e.subjectId && styles.optionItemSelected]}
                        onPress={() => { setSelectedSubject(e.subjectId); setShowSubjectPicker(false); }}
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

                {topics.length > 0 && (
                  <>
                    <Text style={styles.formLabel}>Topics to study ({selectedTopics.size}/{topics.length})</Text>
                    {topics.map(t => (
                      <TouchableOpacity key={t.id} style={styles.topicSelectRow} onPress={() => toggleTopic(t.id)}>
                        <Checkbox status={selectedTopics.has(t.id) ? 'checked' : 'unchecked'} color="#0ea5e9" />
                        <Text style={styles.topicSelectName}>{t.name}</Text>
                      </TouchableOpacity>
                    ))}
                  </>
                )}

                <Text style={styles.formLabel}>Duration per topic (minutes)</Text>
                <TextInput value={duration} onChangeText={setDuration} keyboardType="numeric" mode="outlined" outlineColor="#e2e8f0" activeOutlineColor="#0ea5e9" style={styles.input} />

                <Text style={styles.formLabel}>Start Date *</Text>
                <TouchableOpacity
                  style={styles.pickerButton}
                  onPress={() => { setPickerTarget('start'); setPickerDate(startDate ? new Date(startDate) : new Date()); setShowStartPicker(true); }}
                >
                  <Ionicons name="calendar-outline" size={20} color="#0ea5e9" style={{ marginRight: 10 }} />
                  <Text style={startDate ? styles.pickerButtonText : styles.pickerButtonPlaceholder}>
                    {startDate || 'Select Start Date'}
                  </Text>
                </TouchableOpacity>

                <Text style={styles.formLabel}>End Date *</Text>
                <TouchableOpacity
                  style={styles.pickerButton}
                  onPress={() => { setPickerTarget('end'); setPickerDate(endDate ? new Date(endDate) : new Date()); setShowEndPicker(true); }}
                >
                  <Ionicons name="calendar-outline" size={20} color="#0ea5e9" style={{ marginRight: 10 }} />
                  <Text style={endDate ? styles.pickerButtonText : styles.pickerButtonPlaceholder}>
                    {endDate || 'Select End Date'}
                  </Text>
                </TouchableOpacity>
              </Dialog.Content>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={onDismiss} textColor="#64748b">Cancel</Button>
            <Button onPress={handleSubmit} mode="contained" buttonColor="#0ea5e9" loading={saving} disabled={saving}>
              Create
            </Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>

      <CalendarPicker
        visible={activePickerVisible}
        pickerDate={pickerDate}
        selectedDate={activeSelectedDate}
        title={pickerTarget === 'start' ? 'Select Start Date' : 'Select End Date'}
        onChangeMonth={(delta) => setPickerDate(new Date(pickerDate.getFullYear(), pickerDate.getMonth() + delta, 1))}
        onSelectDate={(dateStr) => {
          if (pickerTarget === 'start') setStartDate(dateStr);
          else setEndDate(dateStr);
        }}
        onDone={() => { setShowStartPicker(false); setShowEndPicker(false); }}
        onCancel={() => { setShowStartPicker(false); setShowEndPicker(false); }}
      />
    </>
  );
}

const styles = StyleSheet.create({
  dialog: { borderRadius: 20, maxHeight: '90%' },
  dialogTitleText: { fontSize: 24, fontWeight: 'bold', color: '#1e293b' },
  formLabel: { fontSize: 14, fontWeight: '600', color: '#1e293b', marginBottom: 8, marginTop: 16 },
  input: { backgroundColor: '#ffffff' },
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#ffffff', paddingHorizontal: 16, paddingVertical: 16, minHeight: 56 },
  pickerButtonText: { fontSize: 16, color: '#1e293b', fontWeight: '500' },
  pickerButtonPlaceholder: { fontSize: 16, color: '#94a3b8' },
  optionList: { borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#ffffff', overflow: 'hidden', marginBottom: 8 },
  optionItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingVertical: 14, borderBottomWidth: 1, borderBottomColor: '#f1f5f9' },
  optionItemSelected: { backgroundColor: '#f0f9ff' },
  optionDot: { width: 10, height: 10, borderRadius: 5, marginRight: 12 },
  optionText: { flex: 1, fontSize: 15, color: '#1e293b' },
  optionTextSelected: { fontWeight: '600', color: '#0ea5e9' },
  optionEmpty: { padding: 16, color: '#94a3b8', fontStyle: 'italic', textAlign: 'center' },
  topicSelectRow: { flexDirection: 'row', alignItems: 'center', paddingVertical: 2 },
  topicSelectName: { fontSize: 14, color: '#1e293b', flex: 1 },
  dialogActions: { paddingHorizontal: 24, paddingVertical: 20, gap: 8 },
});
