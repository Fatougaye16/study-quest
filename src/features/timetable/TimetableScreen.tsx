import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, TouchableOpacity, RefreshControl } from 'react-native';
import { Button, Card, TextInput, Dialog, Portal } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../shared/theme';
import { timetableAPI } from './api';
import { TimetableEntry } from './types';
import { enrollmentsAPI } from '../courses/api';
import { Enrollment } from '../courses/types';
import AfricanPattern from '../../shared/components/AfricanPattern';

const DAYS = [
  { label: 'Monday', value: 1 },
  { label: 'Tuesday', value: 2 },
  { label: 'Wednesday', value: 3 },
  { label: 'Thursday', value: 4 },
  { label: 'Friday', value: 5 },
  { label: 'Saturday', value: 6 },
  { label: 'Sunday', value: 0 },
];

const dayName = (dow: number) => DAYS.find(d => d.value === dow)?.label ?? '';

export default function TimetableScreen() {
  const { theme } = useTheme();
  const colors = theme.colors;
  const [timetable, setTimetable] = useState<TimetableEntry[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [showDialog, setShowDialog] = useState(false);
  const [selectedDay, setSelectedDay] = useState(1);
  const [selectedSubject, setSelectedSubject] = useState('');
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  const [location, setLocation] = useState('');
  const [showSubjectPicker, setShowSubjectPicker] = useState(false);
  const [showDayPicker, setShowDayPicker] = useState(false);
  const [refreshing, setRefreshing] = useState(false);
  const [saving, setSaving] = useState(false);

  const loadData = useCallback(async () => {
    try {
      const [ttRes, enrRes] = await Promise.all([
        timetableAPI.getAll(),
        enrollmentsAPI.getAll(),
      ]);
      setTimetable(ttRes.data);
      setEnrollments(enrRes.data);
    } catch (error) {
      console.error('Failed to load timetable:', error);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  const handleAddEntry = async () => {
    if (!selectedSubject || !startTime || !endTime) {
      Alert.alert('Error', 'Please fill in subject, start time, and end time');
      return;
    }

    setSaving(true);
    try {
      await timetableAPI.create({
        subjectId: selectedSubject,
        dayOfWeek: selectedDay,
        startTime: startTime.length === 5 ? startTime + ':00' : startTime,
        endTime: endTime.length === 5 ? endTime + ':00' : endTime,
        location: location || undefined,
      });
      await loadData();
      setShowDialog(false);
      resetForm();
      Alert.alert('Success! 🎉', 'Class added to timetable!');
    } catch (error: any) {
      Alert.alert('Error', error.response?.data?.detail || 'Failed to add class');
    } finally {
      setSaving(false);
    }
  };

  const handleDeleteEntry = (entryId: string) => {
    Alert.alert('Delete Class', 'Remove this class from your timetable?', [
      { text: 'Cancel', style: 'cancel' },
      {
        text: 'Delete',
        style: 'destructive',
        onPress: async () => {
          try {
            await timetableAPI.delete(entryId);
            await loadData();
          } catch (error: any) {
            Alert.alert('Error', error.response?.data?.detail || 'Failed to delete');
          }
        },
      },
    ]);
  };

  const resetForm = () => {
    setStartTime('');
    setEndTime('');
    setLocation('');
    setSelectedDay(1);
    setSelectedSubject('');
    setShowSubjectPicker(false);
    setShowDayPicker(false);
  };

  const formatTime = (t: string) => t.slice(0, 5);

  const getEntriesForDay = (dow: number) =>
    timetable
      .filter(e => e.dayOfWeek === dow)
      .sort((a, b) => a.startTime.localeCompare(b.startTime));

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
      >
        {DAYS.map(({ label, value }) => {
          const entries = getEntriesForDay(value);
          return (
            <View key={value} style={styles.daySection}>
              <Text style={[styles.dayTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{label}</Text>
              {entries.length > 0 ? (
                entries.map(entry => (
                  <Card
                    key={entry.id}
                    style={[styles.slotCard, { borderLeftColor: entry.subjectColor, backgroundColor: colors.card }]}
                    onLongPress={() => handleDeleteEntry(entry.id)}
                  >
                    <Card.Content>
                      <Text style={[styles.courseName, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{entry.subjectName}</Text>
                      <Text style={[styles.time, { color: colors.textSecondary }]}>
                        {formatTime(entry.startTime)} - {formatTime(entry.endTime)}
                      </Text>
                      {entry.location && <Text style={[styles.location, { color: colors.textTertiary }]}>📍 {entry.location}</Text>}
                    </Card.Content>
                  </Card>
                ))
              ) : (
                <Text style={[styles.emptyDay, { color: colors.textTertiary }]}>No classes scheduled</Text>
              )}
            </View>
          );
        })}
      </ScrollView>

      <Button mode="contained" onPress={() => setShowDialog(true)} style={[styles.addButton, { backgroundColor: colors.primary }]} icon="plus">
        Add Class
      </Button>

      <Portal>
        <Dialog visible={showDialog} onDismiss={() => setShowDialog(false)} style={[styles.dialog, { backgroundColor: colors.card }]}>
          <Dialog.Title style={styles.dialogTitle}>
            <Text style={[styles.dialogTitleText, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>📅 Add Class</Text>
          </Dialog.Title>
          <Dialog.ScrollArea style={styles.dialogScrollArea}>
            <ScrollView showsVerticalScrollIndicator={false}>
              <View style={styles.dialogFormContent}>
                <Text style={[styles.formLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Subject *</Text>
                <TouchableOpacity
                  style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.surface }]}
                  onPress={() => setShowSubjectPicker(!showSubjectPicker)}
                >
                  <Text style={selectedSubject ? [styles.pickerButtonText, { color: colors.text, fontFamily: theme.fonts.bodyMedium }] : [styles.pickerButtonPlaceholder, { color: colors.textTertiary }]}>
                    {selectedSubject
                      ? enrollments.find(e => e.subjectId === selectedSubject)?.subjectName
                      : 'Tap to select a subject'}
                  </Text>
                  <Feather name={showSubjectPicker ? 'chevron-up' : 'chevron-down'} size={20} color={colors.textTertiary} />
                </TouchableOpacity>
                {showSubjectPicker && (
                  <View style={[styles.optionList, { borderColor: colors.border, backgroundColor: colors.surface }]}>
                    {enrollments.map(e => (
                      <TouchableOpacity
                        key={e.subjectId}
                        style={[styles.optionItem, { borderBottomColor: colors.borderLight }, selectedSubject === e.subjectId && { backgroundColor: colors.primary + '15' }]}
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

                <Text style={[styles.formLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Day *</Text>
                <TouchableOpacity
                  style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.surface }]}
                  onPress={() => setShowDayPicker(!showDayPicker)}
                >
                  <Text style={[styles.pickerButtonText, { color: colors.text, fontFamily: theme.fonts.bodyMedium }]}>{dayName(selectedDay)}</Text>
                  <Feather name={showDayPicker ? 'chevron-up' : 'chevron-down'} size={20} color={colors.textTertiary} />
                </TouchableOpacity>
                {showDayPicker && (
                  <View style={[styles.optionList, { borderColor: colors.border, backgroundColor: colors.surface }]}>
                    {DAYS.map(d => (
                      <TouchableOpacity
                        key={d.value}
                        style={[styles.optionItem, { borderBottomColor: colors.borderLight }, selectedDay === d.value && { backgroundColor: colors.primary + '15' }]}
                        onPress={() => { setSelectedDay(d.value); setShowDayPicker(false); }}
                      >
                        <Text style={[styles.optionText, { color: colors.text }, selectedDay === d.value && { fontWeight: '600', color: colors.primary }]}>
                          {d.label}
                        </Text>
                        {selectedDay === d.value && <Feather name="check" size={18} color={colors.primary} />}
                      </TouchableOpacity>
                    ))}
                  </View>
                )}

                <View style={styles.timeRow}>
                  <View style={styles.timeColumn}>
                    <Text style={[styles.formLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Start *</Text>
                    <TextInput
                      value={startTime}
                      onChangeText={setStartTime}
                      style={[styles.timeInput, { backgroundColor: colors.surface }]}
                      mode="outlined"
                      placeholder="09:00"
                      outlineColor={colors.border}
                      activeOutlineColor={colors.primary}
                      textColor={colors.text}
                    />
                  </View>
                  <View style={styles.timeSeparator}>
                    <Feather name="arrow-right" size={20} color={colors.textTertiary} />
                  </View>
                  <View style={styles.timeColumn}>
                    <Text style={[styles.formLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>End *</Text>
                    <TextInput
                      value={endTime}
                      onChangeText={setEndTime}
                      style={[styles.timeInput, { backgroundColor: colors.surface }]}
                      mode="outlined"
                      placeholder="10:30"
                      outlineColor={colors.border}
                      activeOutlineColor={colors.primary}
                      textColor={colors.text}
                    />
                  </View>
                </View>

                <Text style={[styles.formLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Location</Text>
                <TextInput
                  value={location}
                  onChangeText={setLocation}
                  style={[styles.timeInput, { backgroundColor: colors.surface }]}
                  mode="outlined"
                  placeholder="e.g., Room 201"
                  outlineColor={colors.border}
                  activeOutlineColor={colors.primary}
                  textColor={colors.text}
                />
              </View>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={() => setShowDialog(false)} textColor={colors.textSecondary} labelStyle={{ fontFamily: theme.fonts.bodySemiBold }}>Cancel</Button>
            <Button
              onPress={handleAddEntry}
              mode="contained"
              buttonColor={colors.primary}
              style={styles.submitButton}
              loading={saving}
              disabled={saving}
              labelStyle={{ fontFamily: theme.fonts.bodySemiBold }}
            >
              Add Class
            </Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollView: { flex: 1, padding: 16 },
  daySection: { marginBottom: 24 },
  dayTitle: { fontSize: 18, fontWeight: 'bold', marginBottom: 8 },
  slotCard: { marginBottom: 8, borderLeftWidth: 4 },
  courseName: { fontSize: 16, fontWeight: '600' },
  time: { fontSize: 14, marginTop: 4 },
  location: { fontSize: 12, marginTop: 4 },
  emptyDay: { fontSize: 14, fontStyle: 'italic' },
  addButton: { margin: 16 },
  dialog: { borderRadius: 20, marginHorizontal: 16 },
  dialogTitle: { paddingBottom: 0 },
  dialogTitleText: { fontSize: 20 },
  dialogScrollArea: { paddingHorizontal: 0, maxHeight: 420 },
  dialogFormContent: { paddingHorizontal: 24, paddingBottom: 8 },
  formLabel: { fontSize: 14, marginBottom: 6, marginTop: 14 },
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 1, borderRadius: 10, paddingHorizontal: 14, paddingVertical: 14, minHeight: 50 },
  pickerButtonText: { fontSize: 15 },
  pickerButtonPlaceholder: { fontSize: 15 },
  optionList: { borderWidth: 1, borderRadius: 10, overflow: 'hidden', marginTop: 6 },
  optionItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 14, paddingVertical: 12, borderBottomWidth: 1 },
  optionDot: { width: 10, height: 10, borderRadius: 5, marginRight: 10 },
  optionText: { flex: 1, fontSize: 14 },
  optionEmpty: { padding: 14, fontStyle: 'italic', textAlign: 'center', fontSize: 13 },
  timeRow: { flexDirection: 'row', alignItems: 'flex-end', gap: 4 },
  timeColumn: { flex: 1 },
  timeSeparator: { paddingHorizontal: 8, paddingBottom: 14 },
  timeInput: { fontSize: 15 },
  dialogActions: { paddingHorizontal: 20, paddingVertical: 16, gap: 8 },
  submitButton: { borderRadius: 10 },
});
