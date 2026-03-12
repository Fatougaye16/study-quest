import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Card, Checkbox } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { StudyPlan } from '../types';

interface Props {
  plan: StudyPlan;
  isExpanded: boolean;
  onToggleExpand: () => void;
  onToggleItem: (planId: string, itemId: string) => void;
  onDelete: (planId: string) => void;
}

const formatDate = (iso: string) => new Date(iso).toLocaleDateString();

export default function PlanCard({ plan, isExpanded, onToggleExpand, onToggleItem, onDelete }: Props) {
  const completedCount = plan.items.filter(i => i.isCompleted).length;

  return (
    <Card style={styles.planCard}>
      <TouchableOpacity activeOpacity={0.7} onPress={onToggleExpand}>
        <Card.Content style={styles.planHeaderRow}>
          <View style={styles.planHeaderLeft}>
            <View style={styles.planTitleRow}>
              <Text style={styles.planTitle} numberOfLines={1}>{plan.title}</Text>
              {plan.isAIGenerated && <Text style={styles.aiBadge}>🤖</Text>}
            </View>
            <Text style={styles.subjectName}>{plan.subjectName}</Text>
            <View style={styles.planMeta}>
              <Text style={styles.planMetaText}>{completedCount}/{plan.items.length} done</Text>
              <Text style={styles.planMetaDot}>•</Text>
              <Text style={styles.planMetaText}>{Math.round(plan.completionPercentage)}%</Text>
            </View>
            <View style={styles.progressBar}>
              <View style={[styles.progressFill, { width: `${plan.completionPercentage}%` }]} />
            </View>
          </View>
          <Ionicons name={isExpanded ? 'chevron-up' : 'chevron-down'} size={22} color="#0ea5e9" />
        </Card.Content>
      </TouchableOpacity>

      {isExpanded && (
        <Card.Content style={styles.planExpandedContent}>
          <Text style={styles.dateRange}>
            {formatDate(plan.startDate)} → {formatDate(plan.endDate)}
          </Text>

          <View style={styles.topicsList}>
            {plan.items.map(item => (
              <TouchableOpacity
                key={item.id}
                style={styles.topicItem}
                onPress={() => onToggleItem(plan.id, item.id)}
              >
                <Checkbox
                  status={item.isCompleted ? 'checked' : 'unchecked'}
                  onPress={() => onToggleItem(plan.id, item.id)}
                  color="#0ea5e9"
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

          <TouchableOpacity style={styles.deleteRow} onPress={() => onDelete(plan.id)}>
            <Ionicons name="trash-outline" size={16} color="#ef4444" />
            <Text style={styles.deleteText}>Delete Plan</Text>
          </TouchableOpacity>
        </Card.Content>
      )}
    </Card>
  );
}

const styles = StyleSheet.create({
  planCard: { marginBottom: 12 },
  planHeaderRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  planHeaderLeft: { flex: 1, marginRight: 8 },
  planTitleRow: { flexDirection: 'row', alignItems: 'center', gap: 6 },
  planTitle: { fontSize: 16, fontWeight: 'bold', color: '#1e293b', flexShrink: 1 },
  aiBadge: { fontSize: 14 },
  subjectName: { fontSize: 13, color: '#0ea5e9', fontWeight: '600', marginTop: 2 },
  planMeta: { flexDirection: 'row', alignItems: 'center', marginTop: 4, gap: 4 },
  planMetaText: { fontSize: 12, color: '#64748b' },
  planMetaDot: { fontSize: 12, color: '#cbd5e1' },
  progressBar: { height: 6, backgroundColor: '#e2e8f0', borderRadius: 3, overflow: 'hidden', marginTop: 8 },
  progressFill: { height: '100%', backgroundColor: '#10b981' },
  planExpandedContent: { paddingTop: 4, borderTopWidth: 1, borderTopColor: '#f1f5f9' },
  dateRange: { fontSize: 13, color: '#64748b', marginBottom: 8 },
  topicsList: { gap: 2 },
  topicItem: { flexDirection: 'row', alignItems: 'center', paddingVertical: 2 },
  topicInfo: { flex: 1 },
  topicName: { fontSize: 14, color: '#1e293b' },
  topicCompleted: { textDecorationLine: 'line-through', color: '#94a3b8' },
  topicDuration: { fontSize: 12, color: '#64748b' },
  deleteRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 6, paddingVertical: 10, marginTop: 8, borderTopWidth: 1, borderTopColor: '#f1f5f9' },
  deleteText: { fontSize: 13, color: '#ef4444', fontWeight: '600' },
});
