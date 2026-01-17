import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, TouchableOpacity } from 'react-native';
import { Button, Card, TextInput, Dialog, Portal, Chip } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { Picker } from '@react-native-picker/picker';
import { getTimetable, saveTimetable, getCourses, addXP, unlockAchievement } from '../utils/storage';
import { TimetableSlot, Course } from '../types';

const DAYS = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

export default function TimetableScreen() {
  const [timetable, setTimetable] = useState<TimetableSlot[]>([]);
  const [courses, setCourses] = useState<Course[]>([]);
  const [showDialog, setShowDialog] = useState(false);
  const [selectedDay, setSelectedDay] = useState('Monday');
  const [selectedCourse, setSelectedCourse] = useState('');
  const [startTime, setStartTime] = useState('');
  const [endTime, setEndTime] = useState('');
  const [showCoursePicker, setShowCoursePicker] = useState(false);
  const [showDayPicker, setShowDayPicker] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const loadedTimetable = await getTimetable();
    const loadedCourses = await getCourses();
    setTimetable(loadedTimetable);
    setCourses(loadedCourses);
    setSelectedCourse('');
  };

  const handleAddSlot = async () => {
    if (!selectedCourse || !startTime || !endTime) {
      Alert.alert('Error', 'Please fill in all required fields');
      return;
    }

    const course = courses.find(c => c.id === selectedCourse);
    if (!course) return;

    const newSlot: TimetableSlot = {
      id: Date.now().toString(),
      courseId: course.id,
      courseName: course.name,
      day: selectedDay,
      startTime,
      endTime,
      color: course.color,
    };

    const updatedTimetable = [...timetable, newSlot];
    await saveTimetable(updatedTimetable);
    setTimetable(updatedTimetable);
    setShowDialog(false);
    resetForm();
    
    // Award XP and check achievement
    await addXP(20);
    await unlockAchievement('2');
    
    Alert.alert('Success! 🎉', 'Class added to timetable!\n\n+20 XP earned!');
  };

  const handleDeleteSlot = async (id: string) => {
    Alert.alert(
      'Delete Class',
      'Are you sure you want to remove this class from your timetable?',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Delete',
          style: 'destructive',
          onPress: async () => {
            const updatedTimetable = timetable.filter(slot => slot.id !== id);
            await saveTimetable(updatedTimetable);
            setTimetable(updatedTimetable);
          },
        },
      ]
    );
  };

  const resetForm = () => {
    setStartTime('');
    setEndTime('');
    setSelectedDay('Monday');
    setSelectedCourse('');
    setShowCoursePicker(false);
    setShowDayPicker(false);
  };

  const getTimetableForDay = (day: string) => {
    return timetable
      .filter(slot => slot.day === day)
      .sort((a, b) => a.startTime.localeCompare(b.startTime));
  };

  return (
    <View style={styles.container}>
      <ScrollView style={styles.scrollView}>
        {DAYS.map(day => {
          const daySlots = getTimetableForDay(day);
          return (
            <View key={day} style={styles.daySection}>
              <Text style={styles.dayTitle}>{day}</Text>
              {daySlots.length > 0 ? (
                daySlots.map(slot => (
                  <Card
                    key={slot.id}
                    style={[styles.slotCard, { borderLeftColor: slot.color }]}
                    onLongPress={() => handleDeleteSlot(slot.id)}
                  >
                    <Card.Content>
                      <Text style={styles.courseName}>{slot.courseName}</Text>
                      <Text style={styles.time}>
                        {slot.startTime} - {slot.endTime}
                      </Text>
                      {slot.location && (
                        <Text style={styles.location}>📍 {slot.location}</Text>
                      )}
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

      <Button
        mode="contained"
        onPress={() => setShowDialog(true)}
        style={styles.addButton}
        icon="plus"
      >
        Add Class
      </Button>

      <Portal>
        <Dialog visible={showDialog} onDismiss={() => setShowDialog(false)} style={styles.dialog}>
          <Dialog.Title style={styles.dialogTitle}>
            <Text style={styles.dialogTitleText}>📅 Add Class to Timetable</Text>
          </Dialog.Title>
          <Dialog.ScrollArea>
            <ScrollView>
              <Dialog.Content>
                {/* Course Selection */}
                <Text style={styles.formLabel}>Select Course *</Text>
                <TouchableOpacity 
                  style={styles.pickerButton}
                  onPress={() => setShowCoursePicker(!showCoursePicker)}
                >
                  <Text style={selectedCourse ? styles.pickerButtonText : styles.pickerButtonPlaceholder}>
                    {selectedCourse ? courses.find(c => c.id === selectedCourse)?.name : 'Tap to select a course'}
                  </Text>
                  <Ionicons name="chevron-down" size={20} color="#94a3b8" />
                </TouchableOpacity>
                {showCoursePicker && (
                  <View style={styles.pickerContainer}>
                    <Picker
                      selectedValue={selectedCourse}
                      onValueChange={(value) => {
                        setSelectedCourse(value);
                        setShowCoursePicker(false);
                      }}
                      style={styles.picker}
                    >
                      <Picker.Item label="Select a course" value="" />
                      {courses.map(course => (
                        <Picker.Item key={course.id} label={course.name} value={course.id} />
                      ))}
                    </Picker>
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
                      activeOutlineColor="#8b5cf6"
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
                      activeOutlineColor="#8b5cf6"
                    />
                  </View>
                </View>

                {/* Day Selection */}
                <Text style={styles.formLabel}>Day of the Week *</Text>
                <TouchableOpacity 
                  style={styles.pickerButton}
                  onPress={() => setShowDayPicker(!showDayPicker)}
                >
                  <Text style={styles.pickerButtonText}>{selectedDay}</Text>
                  <Ionicons name="chevron-down" size={20} color="#94a3b8" />
                </TouchableOpacity>
                {showDayPicker && (
                  <View style={styles.pickerContainer}>
                    <Picker 
                      selectedValue={selectedDay} 
                      onValueChange={(value) => {
                        setSelectedDay(value);
                        setShowDayPicker(false);
                      }}
                      style={styles.picker}
                    >
                      {DAYS.map(day => (
                        <Picker.Item key={day} label={day} value={day} />
                      ))}
                    </Picker>
                  </View>
                )}

                <View style={styles.helpBox}>
                  <Ionicons name="information-circle" size={16} color="#8b5cf6" />
                  <Text style={styles.helpText}>
                    Organize your weekly schedule and never miss a class!
                  </Text>
                </View>
              </Dialog.Content>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={() => setShowDialog(false)} textColor="#64748b">
              Cancel
            </Button>
            <Button 
              onPress={handleAddSlot}
              mode="contained"
              buttonColor="#8b5cf6"
              style={styles.submitButton}
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
  container: {
    flex: 1,
    backgroundColor: '#f8fafc',
  },
  scrollView: {
    flex: 1,
    padding: 16,
  },
  daySection: {
    marginBottom: 24,
  },
  dayTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#1e293b',
    marginBottom: 8,
  },
  slotCard: {
    marginBottom: 8,
    borderLeftWidth: 4,
  },
  courseName: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1e293b',
  },
  time: {
    fontSize: 14,
    color: '#64748b',
    marginTop: 4,
  },
  location: {
    fontSize: 12,
    color: '#94a3b8',
    marginTop: 4,
  },
  emptyDay: {
    color: '#94a3b8',
    fontSize: 14,
    fontStyle: 'italic',
  },
  addButton: {
    margin: 16,
    backgroundColor: '#8b5cf6',
  },
  label: {
    fontSize: 14,
    fontWeight: '600',
    marginTop: 12,
    marginBottom: 4,
    color: '#1e293b',
  },
  dialog: {
    borderRadius: 20,
    maxHeight: '90%',
  },
  dialogTitle: {
    paddingTop: 24,
    paddingBottom: 16,
  },
  titleContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
  },
  iconCircle: {
    width: 48,
    height: 48,
    borderRadius: 24,
    backgroundColor: '#f3e8ff',
    alignItems: 'center',
    justifyContent: 'center',
  },
  dialogTitleText: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#1e293b',
  },
  dialogScrollArea: {
    paddingHorizontal: 0,
  },
  dialogContent: {
    paddingHorizontal: 24,
    paddingBottom: 8,
  },
  fieldContainer: {
    marginBottom: 20,
  },
  labelRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
    marginBottom: 12,
  },
  fieldLabel: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1e293b',
  },
  pickerButton: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    borderWidth: 2,
    borderColor: '#e2e8f0',
    borderRadius: 12,
    backgroundColor: '#ffffff',
    paddingHorizontal: 16,
    paddingVertical: 16,
    minHeight: 56,
  },
  pickerButtonText: {
    fontSize: 16,
    color: '#1e293b',
    fontWeight: '500',
  },
  pickerButtonPlaceholder: {
    fontSize: 16,
    color: '#94a3b8',
  },
  pickerWrapper: {
    borderWidth: 2,
    borderColor: '#e2e8f0',
    borderRadius: 12,
    backgroundColor: '#ffffff',
    overflow: 'hidden',
  },
  picker: {
    height: 56,
  },
  pickerPlaceholder: {
    color: '#94a3b8',
  },
  timeContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
  },
  timeInputWrapper: {
    flex: 1,
  },
  timeLabel: {
    fontSize: 13,
    color: '#64748b',
    marginBottom: 6,
    fontWeight: '500',
  },
  timeInput: {
    backgroundColor: '#ffffff',
  },
  timeArrow: {
    paddingTop: 20,
  },
  infoBox: {
    flexDirection: 'row',
    alignItems: 'flex-start',
    backgroundColor: '#f8f4ff',
    padding: 16,
    borderRadius: 12,
    gap: 12,
    borderLeftWidth: 4,
    borderLeftColor: '#8b5cf6',
    marginTop: 8,
  },
  infoText: {
    flex: 1,
    fontSize: 14,
    color: '#581c87',
    lineHeight: 20,
  },
  dialogActions: {
    paddingHorizontal: 24,
    paddingVertical: 20,
    gap: 8,
  },
  
  formLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1e293b',
    marginBottom: 8,
    marginTop: 16,
  },
  timeRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 16,
  },
  timeColumn: {
    flex: 1,
  },
  timeSeparator: {
    paddingHorizontal: 12,
    paddingTop: 24,
  },
  pickerContainer: {
    borderWidth: 2,
    borderColor: '#e2e8f0',
    borderRadius: 12,
    backgroundColor: '#ffffff',
    overflow: 'hidden',
    marginBottom: 8,
  },
  submitButton: {
    borderRadius: 10,
  },
  helpBox: {
    flexDirection: 'row',
    alignItems: 'flex-start',
    backgroundColor: '#f8f4ff',
    padding: 12,
    borderRadius: 12,
    gap: 8,
    marginTop: 16,
    borderLeftWidth: 4,
    borderLeftColor: '#8b5cf6',
  },
  helpText: {
    flex: 1,
    fontSize: 13,
    color: '#581c87',
    lineHeight: 18,
  },
});
