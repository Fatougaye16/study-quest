import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, Modal, RefreshControl, TouchableOpacity } from 'react-native';
import { Button, Card, TextInput, Dialog, Portal, Checkbox } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { studyPlansAPI, enrollmentsAPI, subjectsAPI } from '../services/api';
import { StudyPlan, StudyPlanItem, Enrollment, Topic } from '../types';

export default function StudyPlanScreen() {
  const [plans, setPlans] = useState<StudyPlan[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [refreshing, setRefreshing] = useState(false);

  // Create dialog state
  const [showDialog, setShowDialog] = useState(false);
  const [selectedSubject, setSelectedSubject] = useState('');
  const [title, setTitle] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [topics, setTopics] = useState<Topic[]>([]);
  const [selectedTopics, setSelectedTopics] = useState<Set<string>>(new Set());
  const [duration, setDuration] = useState('30');
  const [saving, setSaving] = useState(false);
  const [showSubjectPicker, setShowSubjectPicker] = useState(false);

  // Date pickers
  const [showStartPicker, setShowStartPicker] = useState(false);
  const [showEndPicker, setShowEndPicker] = useState(false);
  const [pickerDate, setPickerDate] = useState(new Date());
  const [pickerTarget, setPickerTarget] = useState<'start' | 'end'>('start');

  const loadData = useCallback(async () => {
    try {
      const [plansRes, enrRes] = await Promise.all([
        studyPlansAPI.getAll(),
        enrollmentsAPI.getAll(),
      ]);
      setPlans(plansRes.data);
      setEnrollments(enrRes.data);
    } catch (error) {
      console.error('Failed to load study plans:', error);
    }
  }, []);

  useEffect(() => { loadData(); }, [loadData]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await loadData();
    setRefreshing(false);
  }, [loadData]);

  // Load topics when subject changes
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

  const handleCreate = async () => {
    if (!selectedSubject || !title || !startDate || !endDate || selectedTopics.size === 0) {
      Alert.alert('Error', 'Please fill in all fields and select at least one topic');
      return;
    }

    const dur = parseInt(duration) || 30;
    const start = new Date(startDate);
    const topicArray = topics.filter(t => selectedTopics.has(t.id));
    const items = topicArray.map((topic, i) => {
      const scheduled = new Date(start);
      scheduled.setDate(scheduled.getDate() + i);
      return {
        topicId: topic.id,
        scheduledDate: scheduled.toISOString(),
        durationMinutes: dur,
      };
    });

    setSaving(true);
    try {
      await studyPlansAPI.create({
        subjectId: selectedSubject,
        title,
        startDate: new Date(startDate).toISOString(),
        endDate: new Date(endDate).toISOString(),
        items,
      });
      await loadData();
      setShowDialog(false);
      resetForm();
      Alert.alert('Success! 🎉', 'Study plan created!');
    } catch (error: any) {
      Alert.alert('Error', error.response?.data?.detail || 'Failed to create plan');
    } finally {
      setSaving(false);
    }
  };

  const handleToggleItem = async (planId: string, itemId: string) => {
    try {
      await studyPlansAPI.toggleItem(planId, itemId);
      await loadData();
    } catch (error: any) {
      Alert.alert('Error', error.response?.data?.detail || 'Failed to toggle item');
    }
  };

  const handleDelete = (planId: string) => {
    Alert.alert('Delete Plan', 'Are you sure you want to delete this study plan?', [
      { text: 'Cancel', style: 'cancel' },
      {
        text: 'Delete',
        style: 'destructive',
        onPress: async () => {
          try {
            await studyPlansAPI.delete(planId);
            await loadData();
          } catch (error: any) {
            Alert.alert('Error', error.response?.data?.detail || 'Failed to delete');
          }
        },
      },
    ]);
  };

  const resetForm = () => {
    setTitle('');
    setStartDate('');
    setEndDate('');
    setSelectedSubject('');
    setShowSubjectPicker(false);
    setTopics([]);
    setSelectedTopics(new Set());
    setDuration('30');
  };

  const toggleTopic = (topicId: string) => {
    setSelectedTopics(prev => {
      const next = new Set(prev);
      if (next.has(topicId)) next.delete(topicId);
      else next.add(topicId);
      return next;
    });
  };

  const formatDate = (iso: string) => new Date(iso).toLocaleDateString();

  return (
    <View style={styles.container}>
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#8b5cf6']} />}
      >
        {plans.length === 0 ? (
          <View style={styles.emptyState}>
            <Text style={styles.emptyText}>No study plans yet!</Text>
            <Text style={styles.emptySubtext}>Create a plan to organize your studies</Text>
          </View>
        ) : (
          plans.map(plan => (
            <Card key={plan.id} style={styles.planCard}>
              <Card.Content>
                <View style={styles.planHeader}>
                  <View style={{ flex: 1 }}>
                    <Text style={styles.planTitle}>{plan.title}</Text>
                    <Text style={styles.subjectName}>{plan.subjectName}</Text>
                  </View>
                  <Button mode="text" onPress={() => handleDelete(plan.id)} icon="delete" textColor="#ef4444" compact>
                    Delete
                  </Button>
                </View>

                <Text style={styles.dateRange}>
                  {formatDate(plan.startDate)} → {formatDate(plan.endDate)}
                  {plan.isAIGenerated ? ' 🤖 AI' : ''}
                </Text>

                <View style={styles.progressBar}>
                  <View style={[styles.progressFill, { width: plan.completionPercentage + '%' }]} />
                </View>
                <Text style={styles.progressText}>
                  {plan.items.filter(i => i.isCompleted).length} of {plan.items.length} items completed ({Math.round(plan.completionPercentage)}%)
                </Text>

                <View style={styles.topicsList}>
                  {plan.items.map(item => (
                    <TouchableOpacity
                      key={item.id}
                      style={styles.topicItem}
                      onPress={() => handleToggleItem(plan.id, item.id)}
                    >
                      <Checkbox
                        status={item.isCompleted ? 'checked' : 'unchecked'}
                        onPress={() => handleToggleItem(plan.id, item.id)}
                        color="#8b5cf6"
                      />
                      <View style={styles.topicInfo}>
                        <Text style={[styles.topicName, item.isCompleted && styles.topicCompleted]}>
                          {item.topicName}
                        </Text>
                        <Text style={styles.topicDuration}>
                          {item.durationMinutes} min • {formatDate(item.scheduledDate)}
                          {item.completedAt ? ` • Done ${formatDate(item.completedAt)}` : ''}
                        </Text>
                      </View>
                    </TouchableOpacity>
                  ))}
                </View>
              </Card.Content>
            </Card>
          ))
        )}
      </ScrollView>

      <Button mode="contained" onPress={() => setShowDialog(true)} style={styles.addButton} icon="plus">
        Create Study Plan
      </Button>

      <Portal>
        <Dialog visible={showDialog} onDismiss={() => setShowDialog(false)} style={styles.dialog}>
          <Dialog.Title>
            <Text style={styles.dialogTitleText}>📚 Create Study Plan</Text>
          </Dialog.Title>
          <Dialog.ScrollArea>
            <ScrollView>
              <Dialog.Content>
                <Text style={styles.formLabel}>Title *</Text>
                <TextInput value={title} onChangeText={setTitle} mode="outlined" placeholder="e.g. Exam Preparation" outlineColor="#e2e8f0" activeOutlineColor="#8b5cf6" style={styles.input} />

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
                        <View style={[styles.optionDot, { backgroundColor: e.subjectColor || '#8b5cf6' }]} />
                        <Text style={[styles.optionText, selectedSubject === e.subjectId && styles.optionTextSelected]}>
                          {e.subjectName}
                        </Text>
                        {selectedSubject === e.subjectId && <Ionicons name="checkmark" size={18} color="#8b5cf6" />}
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
                        <Checkbox status={selectedTopics.has(t.id) ? 'checked' : 'unchecked'} color="#8b5cf6" />
                        <Text style={styles.topicSelectName}>{t.name}</Text>
                      </TouchableOpacity>
                    ))}
                  </>
                )}

                <Text style={styles.formLabel}>Duration per topic (minutes)</Text>
                <TextInput value={duration} onChangeText={setDuration} keyboardType="numeric" mode="outlined" outlineColor="#e2e8f0" activeOutlineColor="#8b5cf6" style={styles.input} />

                <Text style={styles.formLabel}>Start Date *</Text>
                <TouchableOpacity
                  style={styles.pickerButton}
                  onPress={() => { setPickerTarget('start'); setPickerDate(startDate ? new Date(startDate) : new Date()); setShowStartPicker(true); }}
                >
                  <Ionicons name="calendar-outline" size={20} color="#8b5cf6" style={{ marginRight: 10 }} />
                  <Text style={startDate ? styles.pickerButtonText : styles.pickerButtonPlaceholder}>
                    {startDate || 'Select Start Date'}
                  </Text>
                </TouchableOpacity>

                <Text style={styles.formLabel}>End Date *</Text>
                <TouchableOpacity
                  style={styles.pickerButton}
                  onPress={() => { setPickerTarget('end'); setPickerDate(endDate ? new Date(endDate) : new Date()); setShowEndPicker(true); }}
                >
                  <Ionicons name="calendar-outline" size={20} color="#8b5cf6" style={{ marginRight: 10 }} />
                  <Text style={endDate ? styles.pickerButtonText : styles.pickerButtonPlaceholder}>
                    {endDate || 'Select End Date'}
                  </Text>
                </TouchableOpacity>
              </Dialog.Content>
            </ScrollView>
          </Dialog.ScrollArea>
          <Dialog.Actions style={styles.dialogActions}>
            <Button onPress={() => setShowDialog(false)} textColor="#64748b">Cancel</Button>
            <Button onPress={handleCreate} mode="contained" buttonColor="#8b5cf6" loading={saving} disabled={saving}>
              Create
            </Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>

      {/* Shared date picker modal */}
      <Modal visible={showStartPicker || showEndPicker} transparent animationType="fade" onRequestClose={() => { setShowStartPicker(false); setShowEndPicker(false); }}>
        <View style={styles.modalOverlay}>
          <View style={styles.datePickerContainer}>
            <Text style={styles.datePickerTitle}>{pickerTarget === 'start' ? 'Select Start Date' : 'Select End Date'}</Text>

            {/* Month navigation */}
            <View style={styles.monthNav}>
              <TouchableOpacity onPress={() => setPickerDate(new Date(pickerDate.getFullYear(), pickerDate.getMonth() - 1, 1))}>
                <Ionicons name="chevron-back" size={24} color="#8b5cf6" />
              </TouchableOpacity>
              <Text style={styles.monthLabel}>
                {pickerDate.toLocaleDateString('en-US', { month: 'long', year: 'numeric' })}
              </Text>
              <TouchableOpacity onPress={() => setPickerDate(new Date(pickerDate.getFullYear(), pickerDate.getMonth() + 1, 1))}>
                <Ionicons name="chevron-forward" size={24} color="#8b5cf6" />
              </TouchableOpacity>
            </View>

            {/* Day-of-week headers */}
            <View style={styles.calRow}>
              {['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'].map(d => (
                <Text key={d} style={styles.calHeader}>{d}</Text>
              ))}
            </View>

            {/* Calendar days */}
            {(() => {
              const year = pickerDate.getFullYear();
              const month = pickerDate.getMonth();
              const firstDay = new Date(year, month, 1).getDay();
              const daysInMonth = new Date(year, month + 1, 0).getDate();
              const selectedStr = pickerTarget === 'start' ? startDate : endDate;
              const rows: React.ReactNode[] = [];
              let cells: React.ReactNode[] = [];

              for (let i = 0; i < firstDay; i++) {
                cells.push(<View key={`empty-${i}`} style={styles.calCell} />);
              }
              for (let day = 1; day <= daysInMonth; day++) {
                const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
                const isSelected = dateStr === selectedStr;
                cells.push(
                  <TouchableOpacity
                    key={day}
                    style={[styles.calCell, isSelected && styles.calCellSelected]}
                    onPress={() => {
                      if (pickerTarget === 'start') setStartDate(dateStr);
                      else setEndDate(dateStr);
                    }}
                  >
                    <Text style={[styles.calDay, isSelected && styles.calDaySelected]}>{day}</Text>
                  </TouchableOpacity>
                );
                if ((firstDay + day) % 7 === 0 || day === daysInMonth) {
                  rows.push(<View key={`row-${day}`} style={styles.calRow}>{cells}</View>);
                  cells = [];
                }
              }
              return rows;
            })()}

            <View style={styles.datePickerButtons}>
              <Button onPress={() => { setShowStartPicker(false); setShowEndPicker(false); }} textColor="#64748b">Cancel</Button>
              <Button onPress={() => { setShowStartPicker(false); setShowEndPicker(false); }} mode="contained" buttonColor="#8b5cf6">Done</Button>
            </View>
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  scrollView: { flex: 1, padding: 16 },
  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 100 },
  emptyText: { fontSize: 20, fontWeight: 'bold', color: '#64748b', marginBottom: 8 },
  emptySubtext: { fontSize: 14, color: '#94a3b8' },
  planCard: { marginBottom: 16 },
  planHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 4 },
  planTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b' },
  subjectName: { fontSize: 14, color: '#8b5cf6', fontWeight: '600' },
  dateRange: { fontSize: 14, color: '#64748b', marginBottom: 12 },
  progressBar: { height: 8, backgroundColor: '#e2e8f0', borderRadius: 4, overflow: 'hidden', marginBottom: 8 },
  progressFill: { height: '100%', backgroundColor: '#10b981' },
  progressText: { fontSize: 12, color: '#64748b', marginBottom: 16 },
  topicsList: { gap: 4 },
  topicItem: { flexDirection: 'row', alignItems: 'center', paddingVertical: 2 },
  topicInfo: { flex: 1 },
  topicName: { fontSize: 14, color: '#1e293b' },
  topicCompleted: { textDecorationLine: 'line-through', color: '#94a3b8' },
  topicDuration: { fontSize: 12, color: '#64748b' },
  addButton: { margin: 16, backgroundColor: '#8b5cf6' },
  dialog: { borderRadius: 20, maxHeight: '90%' },
  dialogTitleText: { fontSize: 24, fontWeight: 'bold', color: '#1e293b' },
  formLabel: { fontSize: 14, fontWeight: '600', color: '#1e293b', marginBottom: 8, marginTop: 16 },
  input: { backgroundColor: '#ffffff' },
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#ffffff', paddingHorizontal: 16, paddingVertical: 16, minHeight: 56 },
  pickerButtonText: { fontSize: 16, color: '#1e293b', fontWeight: '500' },
  pickerButtonPlaceholder: { fontSize: 16, color: '#94a3b8' },
  optionList: { borderWidth: 2, borderColor: '#e2e8f0', borderRadius: 12, backgroundColor: '#ffffff', overflow: 'hidden', marginBottom: 8 },
  optionItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 16, paddingVertical: 14, borderBottomWidth: 1, borderBottomColor: '#f1f5f9' },
  optionItemSelected: { backgroundColor: '#f5f3ff' },
  optionDot: { width: 10, height: 10, borderRadius: 5, marginRight: 12 },
  optionText: { flex: 1, fontSize: 15, color: '#1e293b' },
  optionTextSelected: { fontWeight: '600', color: '#8b5cf6' },
  optionEmpty: { padding: 16, color: '#94a3b8', fontStyle: 'italic', textAlign: 'center' },
  topicSelectRow: { flexDirection: 'row', alignItems: 'center', paddingVertical: 2 },
  topicSelectName: { fontSize: 14, color: '#1e293b', flex: 1 },
  dialogActions: { paddingHorizontal: 24, paddingVertical: 20, gap: 8 },
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.5)', justifyContent: 'center', alignItems: 'center' },
  datePickerContainer: { backgroundColor: '#fff', borderRadius: 16, padding: 20, width: '92%', maxWidth: 400 },
  datePickerTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', textAlign: 'center', marginBottom: 16 },
  monthNav: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12, paddingHorizontal: 8 },
  monthLabel: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  calRow: { flexDirection: 'row', justifyContent: 'flex-start' },
  calHeader: { width: '14.28%', textAlign: 'center', fontSize: 12, fontWeight: '600', color: '#94a3b8', paddingVertical: 6 },
  calCell: { width: '14.28%', aspectRatio: 1, justifyContent: 'center', alignItems: 'center', borderRadius: 20 },
  calCellSelected: { backgroundColor: '#8b5cf6' },
  calDay: { fontSize: 14, color: '#1e293b' },
  calDaySelected: { color: '#ffffff', fontWeight: '700' },
  datePickerButtons: { flexDirection: 'row', justifyContent: 'flex-end', gap: 8, marginTop: 16 },
});
