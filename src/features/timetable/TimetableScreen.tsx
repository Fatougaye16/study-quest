import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, TouchableOpacity, RefreshControl } from 'react-native';
import { Button, Card, TextInput, Dialog, Portal } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { timetableAPI } from './api';
import { TimetableEntry } from './types';
import { enrollmentsAPI } from '../courses/api';
import { Enrollment } from '../courses/types';

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
    <View style={styles.container}>
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
      >
        {DAYS.map(({ label, value }) => {
          const entries = getEntriesForDay(value);
          return (
            <View key={value} style={styles.daySection}>
              <Text style={styles.dayTitle}>{label}</Text>
              {entries.length > 0 ? (
                entries.map(entry => (
                  <Card
                    key={entry.id}
                    style={[styles.slotCard, { borderLeftColor: entry.subjectColor }]}
                    onLongPress={() => handleDeleteEntry(entry.id)}
                  >
                    <Card.Content>
                      <Text style={styles.courseName}>{entry.subjectName}</Text>
                      <Text style={styles.time}>
                        {formatTime(entry.startTime)} - {formatTime(entry.endTime)}
                      </Text>
                      {entry.location && <Text style={styles.location}>📍 {entry.location}</Text>}
                    </Card.Content>
                  </Card>
                ))
              ) : (
                <Text style={styles.emptyDay}>No classes scheduled</Text>
              )}
            </View>
          );
        })}
      </ScrollView>

      <Button mode="contained" onPress={() => setShowDialog(true)} style={styles.addButton} icon="plus">
        Add Class
      </Button>

      <Portal>
        <Dialog visible={showDialog} onDismiss={() => setShowDialog(false)} style={styles.dialog}>
          <Dialog.Title>
            <Text style={styles.dialogTitleText}>📅 Add Class to Timetable</Text>
          </Dialog.Title>
          <Dialog.ScrollArea>
            <ScrollView>
              <Dialog.Content>
                <Text style={styles.formLabel}>Select Subject *</Text>
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

                <View style={styles.timeRow}>
                  <View style={styles.timeColumn}>
                    <Text style={styles.formLabel}>Start Time *</Text>
                    <TextInput
                      value={startTime}
                      onChangeText={setStartTime}
                      style={styles.timeInput}
                      mode="outlined"
                      placeholder="09:00"
                      outlineColor="#e2e8f0"
                      activeOutlineColor="#0ea5e9"
                    />
                  </View>
                  <View style={styles.timeSeparator}>
                    <Ionicons name="arrow-forward" size={20} color="#94a3b8" />
                  </View>
                  <View style={styles.timeColumn}>
                    <Text style={styles.formLabel}>End Time *</Text>
                    <TextInput
                      value={endTime}
                      onChangeText={setEndTime}
                      style={styles.timeInput}
                      mode="outlined"
                      placeholder="10:30"
                      outlineColor="#e2e8f0"
                      activeOutlineColor="#0ea5e9"
                    />
                  </View>
                </View>

                <Text style={styles.formLabel}>Day of the Week *</Text>
                <TouchableOpacity
                  style={styles.pickerButton}
                  onPress={() => setShowDayPicker(!showDayPicker)}
                >
                  <Text style={styles.pickerButtonText}>{dayName(selectedDay)}</Text>
                  <Ionicons name={showDayPicker ? 'chevron-up' : 'chevron-down'} size={20} color="#94a3b8" />
                </TouchableOpacity>
                {showDayPicker && (
                  <View style={styles.optionList}>
                    {DAYS.map(d => (
                      <TouchableOpacity
                        key={d.value}
                        style={[styles.optionItem, selectedDay === d.value && styles.optionItemSelected]}
                        onPress={() => { setSelectedDay(d.value); setShowDayPicker(false); }}
                      >
                        <Text style={[styles.optionText, selectedDay === d.value && styles.optionTextSelected]}>
                          {d.label}
                        </Text>
                        {selectedDay === d.value && <Ionicons name="checkmark" size={18} color="#0ea5e9" />}
                      </TouchableOpacity>
                    ))}
                  </View>
                )}

                <Text style={styles.formLabel}>Location (optional)</Text>
                <TextInput
                  value={location}
                  onChangeText={setLocation}
                  style={styles.timeInput}
                  mode="outlined"
                  placeholder="e.g., Room 201"
                  outlineColor="#e2e8f0"
                  activeOutlineColor="#0ea5e9"
                />
              </Dialog.Content>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={() => setShowDialog(false)} textColor="#64748b">Cancel</Button>
            <Button
              onPress={handleAddEntry}
              mode="contained"
              buttonColor="#0ea5e9"
              style={styles.submitButton}
              loading={saving}
              disabled={saving}
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
  container: { flex: 1, backgroundColor: '#f8fafc' },
  scrollView: { flex: 1, padding: 16 },
  daySection: { marginBottom: 24 },
  dayTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', marginBottom: 8 },
  slotCard: { marginBottom: 8, borderLeftWidth: 4 },
  courseName: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  time: { fontSize: 14, color: '#64748b', marginTop: 4 },
  location: { fontSize: 12, color: '#94a3b8', marginTop: 4 },
  emptyDay: { color: '#94a3b8', fontSize: 14, fontStyle: 'italic' },
  addButton: { margin: 16, backgroundColor: '#0ea5e9' },
  dialog: { borderRadius: 20, maxHeight: '90%' },
  dialogTitleText: { fontSize: 24, fontWeight: 'bold', color: '#1e293b' },
  formLabel: { fontSize: 14, fontWeight: '600', color: '#1e293b', marginBottom: 8, marginTop: 16 },
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
  timeRow: { flexDirection: 'row', alignItems: 'center', marginTop: 16 },
  timeColumn: { flex: 1 },
  timeSeparator: { paddingHorizontal: 12, paddingTop: 24 },
  timeInput: { backgroundColor: '#ffffff' },
  dialogActions: { paddingHorizontal: 24, paddingVertical: 20, gap: 8 },
  submitButton: { borderRadius: 10 },
});
