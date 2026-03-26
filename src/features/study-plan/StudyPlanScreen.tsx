import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, RefreshControl } from 'react-native';
import { Button } from 'react-native-paper';
import { useTheme } from '../../shared/theme';
import { studyPlansAPI } from './api';
import { StudyPlan } from './types';
import { enrollmentsAPI } from '../courses/api';
import { Enrollment, Topic } from '../courses/types';
import PlanCard from './components/PlanCard';
import CreatePlanDialog from './components/CreatePlanDialog';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function StudyPlanScreen() {
  const { theme } = useTheme();
  const colors = theme.colors;
  const [plans, setPlans] = useState<StudyPlan[]>([]);
  const [enrollments, setEnrollments] = useState<Enrollment[]>([]);
  const [refreshing, setRefreshing] = useState(false);
  const [showDialog, setShowDialog] = useState(false);
  const [saving, setSaving] = useState(false);
  const [expandedPlans, setExpandedPlans] = useState<Set<string>>(new Set());

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

  const handleCreate = async (data: {
    subjectId: string;
    title: string;
    startDate: string;
    endDate: string;
    topics: Topic[];
    selectedTopicIds: Set<string>;
    duration: number;
  }) => {
    if (!data.subjectId || !data.title || !data.startDate || !data.endDate || data.selectedTopicIds.size === 0) {
      Alert.alert('Error', 'Please fill in all fields and select at least one topic');
      return;
    }

    const topicArray = data.topics.filter(t => data.selectedTopicIds.has(t.id));
    const start = new Date(data.startDate);
    const items = topicArray.map((topic, i) => {
      const scheduled = new Date(start);
      scheduled.setDate(scheduled.getDate() + i);
      return {
        topicId: topic.id,
        scheduledDate: scheduled.toISOString(),
        durationMinutes: data.duration,
      };
    });

    setSaving(true);
    try {
      await studyPlansAPI.create({
        subjectId: data.subjectId,
        title: data.title,
        startDate: new Date(data.startDate).toISOString(),
        endDate: new Date(data.endDate).toISOString(),
        items,
      });
      await loadData();
      setShowDialog(false);
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

  const togglePlanExpanded = (planId: string) => {
    setExpandedPlans(prev => {
      const next = new Set(prev);
      if (next.has(planId)) next.delete(planId);
      else next.add(planId);
      return next;
    });
  };

  return (
    <View style={[styles.container, { backgroundColor: colors.background }]}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
      <ScrollView
        style={styles.scrollView}
        refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
      >
        {plans.length === 0 ? (
          <View style={styles.emptyState}>
            <Text style={[styles.emptyText, { color: colors.textSecondary }]}>No study plans yet!</Text>
            <Text style={[styles.emptySubtext, { color: colors.textTertiary }]}>Create a plan to organize your studies</Text>
          </View>
        ) : (
          plans.map(plan => (
            <PlanCard
              key={plan.id}
              plan={plan}
              isExpanded={expandedPlans.has(plan.id)}
              onToggleExpand={() => togglePlanExpanded(plan.id)}
              onToggleItem={handleToggleItem}
              onDelete={handleDelete}
            />
          ))
        )}
      </ScrollView>

      <Button mode="contained" onPress={() => setShowDialog(true)} style={[styles.addButton, { backgroundColor: colors.primary }]} icon="plus">
        Create Study Plan
      </Button>

      <CreatePlanDialog
        visible={showDialog}
        enrollments={enrollments}
        saving={saving}
        onDismiss={() => setShowDialog(false)}
        onCreate={handleCreate}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollView: { flex: 1, padding: 16 },
  emptyState: { alignItems: 'center', justifyContent: 'center', padding: 32, marginTop: 100 },
  emptyText: { fontSize: 20, fontWeight: 'bold', marginBottom: 8 },
  emptySubtext: { fontSize: 14 },
  addButton: { margin: 16 },
});
