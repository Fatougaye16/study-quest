import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import { Card, Checkbox } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
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
  const { theme } = useTheme();
  const colors = theme.colors;
  const completedCount = plan.items.filter(i => i.isCompleted).length;

  return (
    <Card style={[styles.planCard, { backgroundColor: colors.card }]}>
      <TouchableOpacity activeOpacity={0.7} onPress={onToggleExpand}>
        <Card.Content style={styles.planHeaderRow}>
          <View style={styles.planHeaderLeft}>
            <View style={styles.planTitleRow}>
              <Text style={[styles.planTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]} numberOfLines={1}>{plan.title}</Text>
              {plan.isAIGenerated && <Text style={styles.aiBadge}>🤖</Text>}
            </View>
            <Text style={[styles.subjectName, { color: colors.primary }]}>{plan.subjectName}</Text>
            <View style={styles.planMeta}>
              <Text style={[styles.planMetaText, { color: colors.textSecondary }]}>{completedCount}/{plan.items.length} done</Text>
              <Text style={[styles.planMetaDot, { color: colors.textTertiary }]}>•</Text>
              <Text style={[styles.planMetaText, { color: colors.textSecondary }]}>{Math.round(plan.completionPercentage)}%</Text>
            </View>
            <View style={[styles.progressBar, { backgroundColor: colors.border }]}>
              <View style={[styles.progressFill, { width: `${plan.completionPercentage}%`, backgroundColor: colors.gamification.xp }]} />
            </View>
          </View>
          <Feather name={isExpanded ? 'chevron-up' : 'chevron-down'} size={22} color={colors.primary} />
        </Card.Content>
      </TouchableOpacity>

      {isExpanded && (
        <Card.Content style={[styles.planExpandedContent, { borderTopColor: colors.border }]}>
          <Text style={[styles.dateRange, { color: colors.textSecondary }]}>
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
                  color={colors.primary}
                />
                <View style={styles.topicInfo}>
                  <Text style={[styles.topicName, { color: colors.text }, item.isCompleted && { textDecorationLine: 'line-through', color: colors.textTertiary }]}>
                    {item.topicName}
                  </Text>
                  <Text style={[styles.topicDuration, { color: colors.textSecondary }]}>
                    {item.durationMinutes} min • {formatDate(item.scheduledDate)}
                    {item.completedAt ? ` • Done ${formatDate(item.completedAt)}` : ''}
                  </Text>
                </View>
              </TouchableOpacity>
            ))}
          </View>

          <TouchableOpacity style={[styles.deleteRow, { borderTopColor: colors.border }]} onPress={() => onDelete(plan.id)}>
            <Feather name="trash-2" size={16} color="#ef4444" />
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
  planTitle: { fontSize: 16, fontWeight: 'bold', flexShrink: 1 },
  aiBadge: { fontSize: 14 },
  subjectName: { fontSize: 13, fontWeight: '600', marginTop: 2 },
  planMeta: { flexDirection: 'row', alignItems: 'center', marginTop: 4, gap: 4 },
  planMetaText: { fontSize: 12 },
  planMetaDot: { fontSize: 12 },
  progressBar: { height: 6, borderRadius: 3, overflow: 'hidden', marginTop: 8 },
  progressFill: { height: '100%' },
  planExpandedContent: { paddingTop: 4, borderTopWidth: 1 },
  dateRange: { fontSize: 13, marginBottom: 8 },
  topicsList: { gap: 2 },
  topicItem: { flexDirection: 'row', alignItems: 'center', paddingVertical: 2 },
  topicInfo: { flex: 1 },
  topicName: { fontSize: 14 },
  topicDuration: { fontSize: 12 },
  deleteRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 6, paddingVertical: 10, marginTop: 8, borderTopWidth: 1 },
  deleteText: { fontSize: 13, color: '#ef4444', fontWeight: '600' },
});
