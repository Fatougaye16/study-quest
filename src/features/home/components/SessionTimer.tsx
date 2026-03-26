import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal, FlatList } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../../shared/theme';
import XCard from '../../../shared/components/XCard';
import { Enrollment, Topic } from '../../courses/types';

interface Props {
  sessionActive: boolean;
  sessionSubjectId: string | null;
  selectedSubject: Enrollment | undefined;
  selectedTopic: Topic | undefined;
  elapsedSeconds: number;
  topics: Topic[];
  enrollments: Enrollment[];
  showSubjectPicker: boolean;
  showTopicPicker: boolean;
  onSetShowSubjectPicker: (v: boolean) => void;
  onSetShowTopicPicker: (v: boolean) => void;
  onSelectSubject: (subjectId: string) => void;
  onSelectTopic: (topicId: string) => void;
  onStart: () => void;
  onStop: () => void;
}

export default function SessionTimer({
  sessionActive, sessionSubjectId, selectedSubject, selectedTopic,
  elapsedSeconds, topics, enrollments,
  showSubjectPicker, showTopicPicker,
  onSetShowSubjectPicker, onSetShowTopicPicker,
  onSelectSubject, onSelectTopic, onStart, onStop,
}: Props) {
  const { theme } = useTheme();
  const colors = theme.colors;

  const formatElapsed = (secs: number) => {
    const h = Math.floor(secs / 3600);
    const m = Math.floor((secs % 3600) / 60);
    const s = secs % 60;
    return h > 0
      ? `${h}:${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}`
      : `${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')}`;
  };

  return (
    <>
      <XCard style={{ borderLeftWidth: 4, borderLeftColor: colors.primary }}>
        {sessionActive ? (
          <View style={styles.timerActive}>
            <Text style={[styles.timerSubject, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>
              {selectedSubject?.subjectName}
              {selectedTopic ? ` — ${selectedTopic.name}` : ''}
            </Text>
            <Text style={[styles.timerDisplay, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>
              {formatElapsed(elapsedSeconds)}
            </Text>
            <Text style={[styles.timerHint, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
              Session in progress...
            </Text>
            <TouchableOpacity
              style={[styles.stopButton, { backgroundColor: colors.error }]}
              onPress={onStop}
            >
              <Feather name="stop-circle" size={24} color="#fff" />
              <Text style={[styles.stopButtonText, { fontFamily: theme.fonts.headingBold }]}>Stop & Save</Text>
            </TouchableOpacity>
          </View>
        ) : (
          <View>
            <Text style={[styles.pickerLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
              Subject
            </Text>
            <TouchableOpacity
              style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.surface }]}
              onPress={() => onSetShowSubjectPicker(true)}
            >
              <Text style={[
                selectedSubject ? { color: colors.text } : { color: colors.textTertiary },
                { fontSize: 15, fontFamily: theme.fonts.body },
              ]}>
                {selectedSubject?.subjectName ?? 'Select a subject'}
              </Text>
              <Feather name="chevron-down" size={20} color={colors.textSecondary} />
            </TouchableOpacity>

            {sessionSubjectId && topics.length > 0 && (
              <>
                <Text style={[styles.pickerLabel, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
                  Topic (optional)
                </Text>
                <TouchableOpacity
                  style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.surface }]}
                  onPress={() => onSetShowTopicPicker(true)}
                >
                  <Text style={[
                    selectedTopic ? { color: colors.text } : { color: colors.textTertiary },
                    { fontSize: 15, fontFamily: theme.fonts.body },
                  ]}>
                    {selectedTopic?.name ?? 'Select a topic'}
                  </Text>
                  <Feather name="chevron-down" size={20} color={colors.textSecondary} />
                </TouchableOpacity>
              </>
            )}

            <TouchableOpacity
              style={[
                styles.startButton,
                { backgroundColor: colors.gamification.xp },
                !sessionSubjectId && { backgroundColor: colors.textTertiary },
              ]}
              onPress={onStart}
              disabled={!sessionSubjectId}
            >
              <Feather name="play-circle" size={24} color="#fff" />
              <Text style={[styles.startButtonText, { fontFamily: theme.fonts.headingBold }]}>Start Session</Text>
            </TouchableOpacity>
          </View>
        )}
      </XCard>

      <Modal visible={showSubjectPicker} transparent animationType="slide">
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.card }]}>
            <Text style={[styles.modalTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>
              Select Subject
            </Text>
            <FlatList
              data={enrollments}
              keyExtractor={(item) => item.id}
              renderItem={({ item }) => (
                <TouchableOpacity
                  style={[styles.modalItem, { borderBottomColor: colors.border }]}
                  onPress={() => onSelectSubject(item.subjectId)}
                >
                  <View style={[styles.colorDot, { backgroundColor: item.subjectColor }]} />
                  <Text style={[styles.modalItemText, { color: colors.text, fontFamily: theme.fonts.body }]}>
                    {item.subjectName}
                  </Text>
                </TouchableOpacity>
              )}
            />
            <TouchableOpacity
              style={[styles.modalCancel, { borderTopColor: colors.border }]}
              onPress={() => onSetShowSubjectPicker(false)}
            >
              <Text style={[styles.modalCancelText, { color: colors.error, fontFamily: theme.fonts.bodySemiBold }]}>
                Cancel
              </Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>

      <Modal visible={showTopicPicker} transparent animationType="slide">
        <View style={styles.modalOverlay}>
          <View style={[styles.modalContent, { backgroundColor: colors.card }]}>
            <Text style={[styles.modalTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>
              Select Topic
            </Text>
            <FlatList
              data={topics}
              keyExtractor={(item) => item.id}
              renderItem={({ item }) => (
                <TouchableOpacity
                  style={[styles.modalItem, { borderBottomColor: colors.border }]}
                  onPress={() => onSelectTopic(item.id)}
                >
                  <Text style={[styles.modalItemText, { color: colors.text, fontFamily: theme.fonts.body }]}>
                    {item.name}
                  </Text>
                </TouchableOpacity>
              )}
            />
            <TouchableOpacity
              style={[styles.modalCancel, { borderTopColor: colors.border }]}
              onPress={() => onSetShowTopicPicker(false)}
            >
              <Text style={[styles.modalCancelText, { color: colors.error, fontFamily: theme.fonts.bodySemiBold }]}>
                Cancel
              </Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>
    </>
  );
}

const styles = StyleSheet.create({
  timerActive: { alignItems: 'center', paddingVertical: 8 },
  timerSubject: { fontSize: 16, marginBottom: 8 },
  timerDisplay: { fontSize: 48, fontVariant: ['tabular-nums'] },
  timerHint: { fontSize: 12, marginTop: 4, marginBottom: 16 },
  stopButton: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 24, paddingVertical: 12, borderRadius: 12, gap: 8 },
  stopButtonText: { color: '#fff', fontSize: 16 },
  pickerLabel: { fontSize: 13, marginBottom: 4, marginTop: 12 },
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 1, borderRadius: 12, paddingHorizontal: 12, paddingVertical: 10 },
  startButton: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', paddingVertical: 12, borderRadius: 12, gap: 8, marginTop: 16 },
  startButtonText: { color: '#fff', fontSize: 16 },
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.4)', justifyContent: 'flex-end' },
  modalContent: { borderTopLeftRadius: 20, borderTopRightRadius: 20, maxHeight: '60%', paddingTop: 16, paddingBottom: 32 },
  modalTitle: { fontSize: 18, textAlign: 'center', marginBottom: 12 },
  modalItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 20, paddingVertical: 14, borderBottomWidth: 1, gap: 12 },
  modalItemText: { fontSize: 16 },
  colorDot: { width: 12, height: 12, borderRadius: 6 },
  modalCancel: { paddingVertical: 14, alignItems: 'center', borderTopWidth: 1, marginTop: 8 },
  modalCancelText: { fontSize: 16 },
});
