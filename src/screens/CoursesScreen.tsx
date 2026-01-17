import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert } from 'react-native';
import { Button, Card, TextInput, Dialog, Portal, Chip } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import * as DocumentPicker from 'expo-document-picker';
import { getCourses, saveCourses, addXP, unlockAchievement } from '../utils/storage';
import { Course, CourseFile } from '../types';

const COURSE_COLORS = ['#8b5cf6', '#ec4899', '#f59e0b', '#10b981', '#06b6d4', '#f43f5e', '#14b8a6', '#a855f7'];

export default function CoursesScreen() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [showDialog, setShowDialog] = useState(false);
  const [courseName, setCourseName] = useState('');
  const [courseCode, setCourseCode] = useState('');
  const [credits, setCredits] = useState('');
  const [instructor, setInstructor] = useState('');
  const [expandedCourse, setExpandedCourse] = useState<string | null>(null);

  useEffect(() => {
    loadCourses();
  }, []);

  const loadCourses = async () => {
    const loadedCourses = await getCourses();
    setCourses(loadedCourses);
  };

  const handleAddCourse = async () => {
    if (!courseName || !credits) {
      Alert.alert('Error', 'Please fill in Course Name and Credits');
      return;
    }

    const newCourse: Course = {
      id: Date.now().toString(),
      name: courseName,
      code: courseCode,
      credits: parseInt(credits),
      instructor,
      files: [],
      color: COURSE_COLORS[courses.length % COURSE_COLORS.length],
    };

    const updatedCourses = [...courses, newCourse];
    await saveCourses(updatedCourses);
    setCourses(updatedCourses);
    setShowDialog(false);
    resetForm();
    
    // Award XP and check achievements
    await addXP(30);
    await unlockAchievement('1');
    if (updatedCourses.length >= 5) {
      await unlockAchievement('8');
    }
    
    Alert.alert('Success! 🎉', 'Course added successfully!\n\n+30 XP earned!');
  };

  const handleDeleteCourse = async (id: string) => {
    Alert.alert(
      'Delete Course',
      'Are you sure you want to delete this course? This will also remove it from your timetable and study plans.',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Delete',
          style: 'destructive',
          onPress: async () => {
            const updatedCourses = courses.filter(course => course.id !== id);
            await saveCourses(updatedCourses);
            setCourses(updatedCourses);
          },
        },
      ]
    );
  };

  const handleUploadFile = async (courseId: string) => {
    try {
      const result = await DocumentPicker.getDocumentAsync({
        type: '*/*',
        copyToCacheDirectory: true,
      });

      if (result.canceled) {
        return;
      }

      const file = result.assets[0];
      const newFile: CourseFile = {
        id: Date.now().toString(),
        name: file.name,
        uri: file.uri,
        size: file.size || 0,
        uploadDate: new Date().toISOString(),
      };

      const updatedCourses = courses.map(course => {
        if (course.id === courseId) {
          return {
            ...course,
            files: [...course.files, newFile],
          };
        }
        return course;
      });

      await saveCourses(updatedCourses);
      setCourses(updatedCourses);
      Alert.alert('Success', 'File uploaded successfully!');
    } catch (error) {
      Alert.alert('Error', 'Failed to upload file');
    }
  };

  const handleDeleteFile = async (courseId: string, fileId: string) => {
    const updatedCourses = courses.map(course => {
      if (course.id === courseId) {
        return {
          ...course,
          files: course.files.filter(file => file.id !== fileId),
        };
      }
      return course;
    });

    await saveCourses(updatedCourses);
    setCourses(updatedCourses);
  };

  const resetForm = () => {
    setCourseName('');
    setCourseCode('');
    setCredits('');
    setInstructor('');
  };

  const formatFileSize = (bytes: number) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  };

  return (
    <View style={styles.container}>
      <ScrollView style={styles.scrollView}>
        {courses.length === 0 ? (
          <View style={styles.emptyState}>
            <Text style={styles.emptyText}>No courses yet!</Text>
            <Text style={styles.emptySubtext}>Add your first course to get started</Text>
          </View>
        ) : (
          courses.map(course => (
            <Card
              key={course.id}
              style={[styles.courseCard, { borderTopColor: course.color }]}
              onPress={() => setExpandedCourse(expandedCourse === course.id ? null : course.id)}
            >
              <Card.Content>
                <View style={styles.courseHeader}>
                  <View style={styles.courseInfo}>
                    <Text style={styles.courseName}>{course.name}</Text>
                    <Text style={styles.courseCode}>{course.code}</Text>
                    {course.instructor && (
                      <Text style={styles.instructor}>👨‍🏫 {course.instructor}</Text>
                    )}
                  </View>
                  <Chip style={[styles.creditsChip, { backgroundColor: course.color }]}>
                    {course.credits} credits
                  </Chip>
                </View>

                {expandedCourse === course.id && (
                  <View style={styles.expandedContent}>
                    <View style={styles.filesHeader}>
                      <Text style={styles.filesTitle}>
                        Course Materials ({course.files.length})
                      </Text>
                      <Button
                        mode="text"
                        onPress={() => handleUploadFile(course.id)}
                        icon="upload"
                        compact
                      >
                        Upload
                      </Button>
                    </View>

                    {course.files.length > 0 ? (
                      course.files.map(file => (
                        <View key={file.id} style={styles.fileItem}>
                          <View style={styles.fileInfo}>
                            <Text style={styles.fileName}>{file.name}</Text>
                            <Text style={styles.fileSize}>{formatFileSize(file.size)}</Text>
                          </View>
                          <Button
                            mode="text"
                            onPress={() => handleDeleteFile(course.id, file.id)}
                            icon="delete"
                            textColor="#ef4444"
                            compact
                          >
                            Delete
                          </Button>
                        </View>
                      ))
                    ) : (
                      <Text style={styles.noFiles}>No files uploaded yet</Text>
                    )}

                    <Button
                      mode="outlined"
                      onPress={() => handleDeleteCourse(course.id)}
                      style={styles.deleteButton}
                      textColor="#ef4444"
                    >
                      Delete Course
                    </Button>
                  </View>
                )}
              </Card.Content>
            </Card>
          ))
        )}
      </ScrollView>

      <Button
        mode="contained"
        onPress={() => setShowDialog(true)}
        style={styles.addButton}
        icon="plus"
      >
        Add Course
      </Button>

      <Portal>
        <Dialog visible={showDialog} onDismiss={() => setShowDialog(false)} style={styles.dialog}>
          <Dialog.Title style={styles.dialogTitle}>
            <Text style={styles.dialogTitleText}>📚 Add New Course</Text>
          </Dialog.Title>
          <Dialog.Content>
            <Text style={styles.formLabel}>Course Name *</Text>
            <TextInput
              value={courseName}
              onChangeText={setCourseName}
              style={styles.input}
              mode="outlined"
              placeholder="e.g., Advanced Mathematics"
              outlineColor="#e2e8f0"
              activeOutlineColor="#8b5cf6"
            />

            <Text style={styles.formLabel}>Course Code (optional)</Text>
            <TextInput
              value={courseCode}
              onChangeText={setCourseCode}
              style={styles.input}
              mode="outlined"
              placeholder="e.g., MATH301"
              outlineColor="#e2e8f0"
              activeOutlineColor="#8b5cf6"
              autoCapitalize="characters"
            />

            <Text style={styles.formLabel}>Credits *</Text>
            <TextInput
              value={credits}
              onChangeText={setCredits}
              keyboardType="numeric"
              style={styles.input}
              mode="outlined"
              placeholder="e.g., 3"
              outlineColor="#e2e8f0"
              activeOutlineColor="#8b5cf6"
            />

            <Text style={styles.formLabel}>Instructor (optional)</Text>
            <TextInput
              value={instructor}
              onChangeText={setInstructor}
              style={styles.input}
              mode="outlined"
              placeholder="e.g., Dr. Smith"
              outlineColor="#e2e8f0"
              activeOutlineColor="#8b5cf6"
            />
            
            <View style={styles.helpBox}>
              <Ionicons name="information-circle" size={16} color="#8b5cf6" />
              <Text style={styles.helpText}>
                Add your courses to organize your studies and track progress
              </Text>
            </View>
          </Dialog.Content>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={() => setShowDialog(false)} textColor="#64748b">
              Cancel
            </Button>
            <Button 
              onPress={handleAddCourse} 
              mode="contained"
              buttonColor="#8b5cf6"
              style={styles.submitButton}
            >
              Add Course
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
  courseCard: {
    marginBottom: 12,
    borderTopWidth: 4,
  },
  courseHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
  },
  courseInfo: {
    flex: 1,
  },
  courseName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#1e293b',
    marginBottom: 4,
  },
  courseCode: {
    fontSize: 14,
    color: '#64748b',
    marginBottom: 4,
  },
  instructor: {
    fontSize: 12,
    color: '#94a3b8',
  },
  creditsChip: {
    height: 28,
  },
  expandedContent: {
    marginTop: 16,
    paddingTop: 16,
    borderTopWidth: 1,
    borderTopColor: '#e2e8f0',
  },
  filesHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  filesTitle: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1e293b',
  },
  fileItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f1f5f9',
  },
  fileInfo: {
    flex: 1,
  },
  fileName: {
    fontSize: 14,
    color: '#1e293b',
    marginBottom: 2,
  },
  fileSize: {
    fontSize: 12,
    color: '#94a3b8',
  },
  noFiles: {
    fontSize: 14,
    color: '#94a3b8',
    fontStyle: 'italic',
    textAlign: 'center',
    paddingVertical: 12,
  },
  deleteButton: {
    marginTop: 12,
    borderColor: '#ef4444',
  },
  addButton: {
    margin: 16,
    backgroundColor: '#8b5cf6',
  },
  input: {
    marginBottom: 12,
  },
  dialog: {
    borderRadius: 16,
  },
  dialogTitle: {
    paddingTop: 24,
  },
  dialogTitleText: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#1e293b',
  },
  formLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1e293b',
    marginBottom: 8,
    marginTop: 4,
  },
  helpBox: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#f3e8ff',
    padding: 12,
    borderRadius: 8,
    marginTop: 8,
    gap: 8,
  },
  helpText: {
    flex: 1,
    fontSize: 12,
    color: '#581c87',
    lineHeight: 16,
  },
  dialogActions: {
    paddingHorizontal: 24,
    paddingBottom: 20,
  },
  submitButton: {
    borderRadius: 8,
  },
});
