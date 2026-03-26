import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity } from 'react-native';
import { Button, Dialog, Portal, TextInput, Checkbox } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
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
  const { theme } = useTheme();
  const colors = theme.colors;
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
            <Text style={[styles.dialogTitleText, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>📚 Create Study Plan</Text>
          </Dialog.Title>
          <Dialog.ScrollArea>
            <ScrollView>
              <Dialog.Content>
                <Text style={[styles.formLabel, { color: colors.text }]}>Title *</Text>
                <TextInput value={title} onChangeText={setTitle} mode="outlined" placeholder="e.g. Exam Preparation" outlineColor={colors.border} activeOutlineColor={colors.primary} style={[styles.input, { backgroundColor: colors.card }]} />

                <Text style={[styles.formLabel, { color: colors.text }]}>Subject *</Text>
                <TouchableOpacity
                  style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.card }]}
                  onPress={() => setShowSubjectPicker(!showSubjectPicker)}
                >
                  <Text style={selectedSubject ? [styles.pickerButtonText, { color: colors.text }] : [styles.pickerButtonPlaceholder, { color: colors.textTertiary }]}>
                    {selectedSubject
                      ? enrollments.find(e => e.subjectId === selectedSubject)?.subjectName
                      : 'Tap to select a subject'}
                  </Text>
                  <Feather name={showSubjectPicker ? 'chevron-up' : 'chevron-down'} size={20} color={colors.textTertiary} />
                </TouchableOpacity>
                {showSubjectPicker && (
                  <View style={[styles.optionList, { borderColor: colors.border, backgroundColor: colors.card }]}>
                    {enrollments.map(e => (
                      <TouchableOpacity
                        key={e.subjectId}
                        style={[styles.optionItem, { borderBottomColor: colors.border }, selectedSubject === e.subjectId && { backgroundColor: colors.primary + '10' }]}
                        onPress={() => { setSelectedSubject(e.subjectId); setShowSubjectPicker(false); }}
                      >
                        <View style={[styles.optionDot, { backgroundColor: e.subjectColor || colors.primary }]} />
                        <Text style={[styles.optionText, { color: colors.text }, selectedSubject === e.subjectId && { fontWeight: '600', color: colors.primary }]}>
                          {e.subjectName}
                        </Text>
                        {selectedSubject === e.subjectId && <Feather name="check" size={18} color={colors.primary} />}
                      </TouchableOpacity>
                    ))}
                    {enrollments.length === 0 && (
                      <Text style={[styles.optionEmpty, { color: colors.textTertiary }]}>No enrolled subjects. Enroll in a course first.</Text>
                    )}
                  </View>
                )}

                {topics.length > 0 && (
                  <>
                    <Text style={[styles.formLabel, { color: colors.text }]}>Topics to study ({selectedTopics.size}/{topics.length})</Text>
                    {topics.map(t => (
                      <TouchableOpacity key={t.id} style={styles.topicSelectRow} onPress={() => toggleTopic(t.id)}>
                        <Checkbox status={selectedTopics.has(t.id) ? 'checked' : 'unchecked'} color={colors.primary} />
                        <Text style={[styles.topicSelectName, { color: colors.text }]}>{t.name}</Text>
                      </TouchableOpacity>
                    ))}
                  </>
                )}

                <Text style={[styles.formLabel, { color: colors.text }]}>Duration per topic (minutes)</Text>
                <TextInput value={duration} onChangeText={setDuration} keyboardType="numeric" mode="outlined" outlineColor={colors.border} activeOutlineColor={colors.primary} style={[styles.input, { backgroundColor: colors.card }]} />

                <Text style={[styles.formLabel, { color: colors.text }]}>Start Date *</Text>
                <TouchableOpacity
                  style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.card }]}
                  onPress={() => { setPickerTarget('start'); setPickerDate(startDate ? new Date(startDate) : new Date()); setShowStartPicker(true); }}
                >
                  <Feather name="calendar" size={20} color={colors.primary} style={{ marginRight: 10 }} />
                  <Text style={startDate ? [styles.pickerButtonText, { color: colors.text }] : [styles.pickerButtonPlaceholder, { color: colors.textTertiary }]}>
                    {startDate || 'Select Start Date'}
                  </Text>
                </TouchableOpacity>

                <Text style={[styles.formLabel, { color: colors.text }]}>End Date *</Text>
                <TouchableOpacity
                  style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.card }]}
                  onPress={() => { setPickerTarget('end'); setPickerDate(endDate ? new Date(endDate) : new Date()); setShowEndPicker(true); }}
                >
                  <Feather name="calendar" size={20} color={colors.primary} style={{ marginRight: 10 }} />
                  <Text style={endDate ? [styles.pickerButtonText, { color: colors.text }] : [styles.pickerButtonPlaceholder, { color: colors.textTertiary }]}>
                    {endDate || 'Select End Date'}
                  </Text>
                </TouchableOpacity>
              </Dialog.Content>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={onDismiss} textColor={colors.textSecondary}>Cancel</Button>
            <Button onPress={handleSubmit} mode="contained" buttonColor={colors.primary} loading={saving} disabled={saving}>
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
  dialogTitleText: { fontSize: 24, fontWeight: 'bold' },
  formLabel: { fontSize: 14, fontWeight: '600', marginBottom: 8, marginTop: 16 },
  input: {},
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 2, borderRadius: 12, paddingHorizontal: 16, paddingVertical: 16, minHeight: 56 },
  pickerButtonText: { fontSize: 16, fontWeight: '500' },
  pickerButtonPlaceholder: { fontSize: 16 },
  optionList: { borderWidth: 2, borderRadius: 12, overflow: 'hidden', marginBottom: 8 },
  optionItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingVertical: 14, borderBottomWidth: 1 },
  optionDot: { width: 10, height: 10, borderRadius: 5, marginRight: 12 },
  optionText: { flex: 1, fontSize: 15 },
  optionEmpty: { padding: 16, fontStyle: 'italic', textAlign: 'center' },
  topicSelectRow: { flexDirection: 'row', alignItems: 'center', paddingVertical: 2 },
  topicSelectName: { fontSize: 14, flex: 1 },
  dialogActions: { paddingHorizontal: 24, paddingVertical: 20, gap: 8 },
});
