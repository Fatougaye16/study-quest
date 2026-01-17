import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, Platform, Modal } from 'react-native';
import { Button, Card, TextInput, Dialog, Portal, Checkbox } from 'react-native-paper';
import { Picker } from '@react-native-picker/picker';
import DateTimePicker from '@react-native-community/datetimepicker';
import { getCourses, getStudyPlans, saveStudyPlans, addXP, unlockAchievement } from '../utils/storage';
import { Course, StudyPlan, StudyTopic } from '../types';

export default function StudyPlanScreen() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [studyPlans, setStudyPlans] = useState<StudyPlan[]>([]);
  const [showDialog, setShowDialog] = useState(false);
  const [selectedCourse, setSelectedCourse] = useState('');
  const [topics, setTopics] = useState<string>('');
  const [duration, setDuration] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [showStartDatePicker, setShowStartDatePicker] = useState(false);
  const [showEndDatePicker, setShowEndDatePicker] = useState(false);
  const [tempStartDate, setTempStartDate] = useState(new Date());
  const [tempEndDate, setTempEndDate] = useState(new Date());

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const loadedCourses = await getCourses();
    const loadedPlans = await getStudyPlans();
    setCourses(loadedCourses);
    setStudyPlans(loadedPlans);
    if (loadedCourses.length > 0) {
      setSelectedCourse(loadedCourses[0].id);
    }
  };

  const generateStudyPlan = async () => {
    if (!selectedCourse || !topics || !duration || !startDate || !endDate) {
      Alert.alert('Error', 'Please fill in all fields');
      return;
    }

    const course = courses.find(c => c.id === selectedCourse);
    if (!course) return;

    const topicList = topics.split('\n').filter(t => t.trim());
    const topicDuration = parseInt(duration);

    const studyTopics: StudyTopic[] = topicList.map((topic, index) => ({
      id: `${Date.now()}_${index}`,
      name: topic.trim(),
      duration: topicDuration,
      completed: false,
    }));

    const newPlan: StudyPlan = {
      id: Date.now().toString(),
      courseId: course.id,
      courseName: course.name,
      topics: studyTopics,
      startDate,
      endDate,
    };

    const updatedPlans = [...studyPlans, newPlan];
    await saveStudyPlans(updatedPlans);
    setStudyPlans(updatedPlans);
    setShowDialog(false);
    resetForm();
    Alert.alert('Success', 'Study plan created! 📚');
  };

  const toggleTopicComplete = async (planId: string, topicId: string) => {
    const updatedPlans = studyPlans.map(plan => {
      if (plan.id === planId) {
        return {
          ...plan,
          topics: plan.topics.map(topic =>
            topic.id === topicId
              ? { 
                  ...topic, 
                  completed: !topic.completed,
                  studiedDate: !topic.completed ? new Date().toISOString() : undefined
                }
              : topic
          ),
        };
      }
      return plan;
    });

    const plan = updatedPlans.find(p => p.id === planId);
    const topic = plan?.topics.find(t => t.id === topicId);
    
    // Award XP when completing a topic
    if (topic && topic.completed) {
      const xpGained = Math.floor(topic.duration / 10) * 10; // 10 XP per 10 minutes
      const result = await addXP(xpGained);
      
      if (result.leveledUp) {
        Alert.alert(
          '🎉 Level Up!',
          `Congratulations! You've reached Level ${result.level}!\n\nYou earned ${xpGained} XP for completing this topic!`,
          [{ text: 'Awesome!', style: 'default' }]
        );
      } else {
        Alert.alert(
          '✨ Topic Complete!',
          `Great work! You earned ${xpGained} XP!\n\nKeep going to level up!`,
          [{ text: 'Nice!', style: 'default' }]
        );
      }
      
      // Check if this unlocks an achievement
      await unlockAchievement('3');
    }

    await saveStudyPlans(updatedPlans);
    setStudyPlans(updatedPlans);
  };

  const deletePlan = async (planId: string) => {
    Alert.alert(
      'Delete Plan',
      'Are you sure you want to delete this study plan?',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Delete',
          style: 'destructive',
          onPress: async () => {
            const updatedPlans = studyPlans.filter(plan => plan.id !== planId);
            await saveStudyPlans(updatedPlans);
            setStudyPlans(updatedPlans);
          },
        },
      ]
    );
  };

  const resetForm = () => {
    setTopics('');
    setDuration('');
    setStartDate('');
    setEndDate('');
    if (courses.length > 0) {
      setSelectedCourse(courses[0].id);
    }
  };

  const getProgress = (plan: StudyPlan) => {
    const completed = plan.topics.filter(t => t.completed).length;
    const total = plan.topics.length;
    return { completed, total, percentage: (completed / total) * 100 };
  };

  return (
    <View style={styles.container}>
      <ScrollView style={styles.scrollView}>
        {studyPlans.length === 0 ? (
          <View style={styles.emptyState}>
            <Text style={styles.emptyText}>No study plans yet!</Text>
            <Text style={styles.emptySubtext}>
              Create a plan to organize your studies
            </Text>
          </View>
        ) : (
          studyPlans.map(plan => {
            const progress = getProgress(plan);
            return (
              <Card key={plan.id} style={styles.planCard}>
                <Card.Content>
                  <View style={styles.planHeader}>
                    <Text style={styles.courseName}>{plan.courseName}</Text>
                    <Button
                      mode="text"
                      onPress={() => deletePlan(plan.id)}
                      icon="delete"
                      textColor="#ef4444"
                      compact
                    >
                      Delete
                    </Button>
                  </View>

                  <Text style={styles.dateRange}>
                    {plan.startDate} → {plan.endDate}
                  </Text>

                  <View style={styles.progressBar}>
                    <View
                      style={[
                        styles.progressFill,
                        { width: `${progress.percentage}%` },
                      ]}
                    />
                  </View>

                  <Text style={styles.progressText}>
                    {progress.completed} of {progress.total} topics completed
                  </Text>

                  <View style={styles.topicsList}>
                    {plan.topics.map(topic => (
                      <View key={topic.id} style={styles.topicItem}>
                        <Checkbox
                          status={topic.completed ? 'checked' : 'unchecked'}
                          onPress={() => toggleTopicComplete(plan.id, topic.id)}
                        />
                        <View style={styles.topicInfo}>
                          <Text
                            style={[
                              styles.topicName,
                              topic.completed && styles.topicCompleted,
                            ]}
                          >
                            {topic.name}
                          </Text>
                          <Text style={styles.topicDuration}>
                            {topic.duration} minutes
                            {topic.studiedDate && (
                              <Text style={styles.studiedDate}>
                                {' '}• Completed {new Date(topic.studiedDate).toLocaleDateString()}
                              </Text>
                            )}
                          </Text>
                        </View>
                      </View>
                    ))}
                  </View>
                </Card.Content>
              </Card>
            );
          })
        )}
      </ScrollView>

      <Button
        mode="contained"
        onPress={() => setShowDialog(true)}
        style={styles.addButton}
        icon="plus"
      >
        Create Study Plan
      </Button>

      <Portal>
        <Dialog visible={showDialog} onDismiss={() => setShowDialog(false)}>
          <Dialog.Title>Create Study Plan</Dialog.Title>
          <Dialog.ScrollArea>
            <ScrollView>
              <Dialog.Content>
                <Text style={styles.label}>Course</Text>
                <View style={styles.pickerContainer}>
                  <Picker
                    selectedValue={selectedCourse}
                    onValueChange={setSelectedCourse}
                  >
                    {courses.map(course => (
                      <Picker.Item key={course.id} label={course.name} value={course.id} />
                    ))}
                  </Picker>
                </View>

                <TextInput
                  label="Topics (one per line)"
                  value={topics}
                  onChangeText={setTopics}
                  multiline
                  numberOfLines={6}
                  style={styles.input}
                  mode="outlined"
                  placeholder="Introduction&#10;Chapter 1&#10;Chapter 2"
                />

                <TextInput
                  label="Study Duration per Topic (minutes)"
                  value={duration}
                  onChangeText={setDuration}
                  keyboardType="numeric"
                  style={styles.input}
                  mode="outlined"
                />

                <Text style={styles.label}>Start Date</Text>
                <Button
                  mode="outlined"
                  onPress={() => setShowStartDatePicker(true)}
                  style={styles.dateButton}
                  icon="calendar"
                >
                  {startDate || 'Select Start Date'}
                </Button>

                <Text style={styles.label}>End Date</Text>
                <Button
                  mode="outlined"
                  onPress={() => setShowEndDatePicker(true)}
                  style={styles.dateButton}
                  icon="calendar"
                >
                  {endDate || 'Select End Date'}
                </Button>

                <Text style={styles.hint}>
                  💡 The app will help you track your progress through each topic!
                </Text>
              </Dialog.Content>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions>
            <Button onPress={() => setShowDialog(false)}>Cancel</Button>
            <Button onPress={generateStudyPlan}>Create</Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>

      <Modal
        visible={showStartDatePicker}
        transparent
        animationType="fade"
        onRequestClose={() => setShowStartDatePicker(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={styles.datePickerContainer}>
            {showStartDatePicker && (
              <DateTimePicker
                value={tempStartDate}
                mode="date"
                display="spinner"
                onChange={(event, selectedDate) => {
                  if (selectedDate) {
                    setTempStartDate(selectedDate);
                    setStartDate(selectedDate.toISOString().split('T')[0]);
                  }
                }}
              />
            )}
            <View style={styles.datePickerButtons}>
              <Button onPress={() => setShowStartDatePicker(false)} textColor="#64748b">
                Cancel
              </Button>
              <Button onPress={() => setShowStartDatePicker(false)} mode="contained" buttonColor="#8b5cf6">
                Done
              </Button>
            </View>
          </View>
        </View>
      </Modal>

      <Modal
        visible={showEndDatePicker}
        transparent
        animationType="fade"
        onRequestClose={() => setShowEndDatePicker(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={styles.datePickerContainer}>
            {showEndDatePicker && (
              <DateTimePicker
                value={tempEndDate}
                mode="date"
                display="spinner"
                onChange={(event, selectedDate) => {
                  if (selectedDate) {
                    setTempEndDate(selectedDate);
                    setEndDate(selectedDate.toISOString().split('T')[0]);
                  }
                }}
              />
            )}
            <View style={styles.datePickerButtons}>
              <Button onPress={() => setShowEndDatePicker(false)} textColor="#64748b">
                Cancel
              </Button>
              <Button onPress={() => setShowEndDatePicker(false)} mode="contained" buttonColor="#8b5cf6">
                Done
              </Button>
            </View>
          </View>
        </View>
      </Modal>
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
  emptyState: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 32,
    marginTop: 100,
  },
  emptyText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#64748b',
    marginBottom: 8,
  },
  emptySubtext: {
    fontSize: 14,
    color: '#94a3b8',
  },
  planCard: {
    marginBottom: 16,
  },
  planHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  courseName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#1e293b',
  },
  dateRange: {
    fontSize: 14,
    color: '#64748b',
    marginBottom: 12,
  },
  progressBar: {
    height: 8,
    backgroundColor: '#e2e8f0',
    borderRadius: 4,
    overflow: 'hidden',
    marginBottom: 8,
  },
  progressFill: {
    height: '100%',
    backgroundColor: '#10b981',
  },
  progressText: {
    fontSize: 12,
    color: '#64748b',
    marginBottom: 16,
  },
  topicsList: {
    gap: 8,
  },
  topicItem: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 4,
  },
  topicInfo: {
    flex: 1,
  },
  topicName: {
    fontSize: 14,
    color: '#1e293b',
  },
  topicCompleted: {
    textDecorationLine: 'line-through',
    color: '#94a3b8',
  },
  topicDuration: {
    fontSize: 12,
    color: '#64748b',
  },
  studiedDate: {
    fontSize: 12,
    color: '#8b5cf6',
    fontWeight: '600',
  },
  addButton: {
    margin: 16,
    backgroundColor: '#8b5cf6',
  },
  label: {
    fontSize: 14,
    fontWeight: '600',
    marginTop: 8,
    marginBottom: 4,
    color: '#1e293b',
  },
  pickerContainer: {
    borderWidth: 1,
    borderColor: '#cbd5e1',
    borderRadius: 4,
    marginBottom: 12,
  },
  dateButton: {
    marginBottom: 12,
    borderColor: '#cbd5e1',
  },
  input: {
    marginBottom: 12,
  },
  hint: {
    fontSize: 12,
    color: '#ec4899',
    fontStyle: 'italic',
    marginTop: 8,
  },
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  datePickerContainer: {
    backgroundColor: '#ffffff',
    borderRadius: 12,
    padding: 16,
    width: '90%',
    maxWidth: 400,
  },
  datePickerButtons: {
    flexDirection: 'row',
    justifyContent: 'flex-end',
    gap: 8,
    marginTop: 16,
  },
});
