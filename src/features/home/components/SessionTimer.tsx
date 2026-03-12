import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal, FlatList } from 'react-native';
import { Card } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
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
      <Card style={styles.sessionTimerCard}>
        <Card.Content>
          {sessionActive ? (
            <View style={styles.timerActive}>
              <Text style={styles.timerSubject}>
                {selectedSubject?.subjectName}
                {selectedTopic ? ` — ${selectedTopic.name}` : ''}
              </Text>
              <Text style={styles.timerDisplay}>{formatElapsed(elapsedSeconds)}</Text>
              <Text style={styles.timerHint}>Session in progress...</Text>
              <TouchableOpacity style={styles.stopButton} onPress={onStop}>
                <Ionicons name="stop-circle" size={24} color="#fff" />
                <Text style={styles.stopButtonText}>Stop & Save</Text>
              </TouchableOpacity>
            </View>
          ) : (
            <View>
              <Text style={styles.pickerLabel}>Subject</Text>
              <TouchableOpacity style={styles.pickerButton} onPress={() => onSetShowSubjectPicker(true)}>
                <Text style={selectedSubject ? styles.pickerValue : styles.pickerPlaceholder}>
                  {selectedSubject?.subjectName ?? 'Select a subject'}
                </Text>
                <Ionicons name="chevron-down" size={20} color="#64748b" />
              </TouchableOpacity>

              {sessionSubjectId && topics.length > 0 && (
                <>
                  <Text style={styles.pickerLabel}>Topic (optional)</Text>
                  <TouchableOpacity style={styles.pickerButton} onPress={() => onSetShowTopicPicker(true)}>
                    <Text style={selectedTopic ? styles.pickerValue : styles.pickerPlaceholder}>
                      {selectedTopic?.name ?? 'Select a topic'}
                    </Text>
                    <Ionicons name="chevron-down" size={20} color="#64748b" />
                  </TouchableOpacity>
                </>
              )}

              <TouchableOpacity
                style={[styles.startButton, !sessionSubjectId && styles.startButtonDisabled]}
                onPress={onStart}
                disabled={!sessionSubjectId}
              >
                <Ionicons name="play-circle" size={24} color="#fff" />
                <Text style={styles.startButtonText}>Start Session</Text>
              </TouchableOpacity>
            </View>
          )}
        </Card.Content>
      </Card>

      <Modal visible={showSubjectPicker} transparent animationType="slide">
        <View style={styles.modalOverlay}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Select Subject</Text>
            <FlatList
              data={enrollments}
              keyExtractor={(item) => item.id}
              renderItem={({ item }) => (
                <TouchableOpacity style={styles.modalItem} onPress={() => onSelectSubject(item.subjectId)}>
                  <View style={[styles.colorDot, { backgroundColor: item.subjectColor }]} />
                  <Text style={styles.modalItemText}>{item.subjectName}</Text>
                </TouchableOpacity>
              )}
            />
            <TouchableOpacity style={styles.modalCancel} onPress={() => onSetShowSubjectPicker(false)}>
              <Text style={styles.modalCancelText}>Cancel</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>

      <Modal visible={showTopicPicker} transparent animationType="slide">
        <View style={styles.modalOverlay}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Select Topic</Text>
            <FlatList
              data={topics}
              keyExtractor={(item) => item.id}
              renderItem={({ item }) => (
                <TouchableOpacity
                  style={styles.modalItem}
                  onPress={() => onSelectTopic(item.id)}
                >
                  <Text style={styles.modalItemText}>{item.name}</Text>
                </TouchableOpacity>
              )}
            />
            <TouchableOpacity style={styles.modalCancel} onPress={() => onSetShowTopicPicker(false)}>
              <Text style={styles.modalCancelText}>Cancel</Text>
            </TouchableOpacity>
          </View>
        </View>
      </Modal>
    </>
  );
}

const styles = StyleSheet.create({
  sessionTimerCard: { borderLeftWidth: 4, borderLeftColor: '#0ea5e9' },
  timerActive: { alignItems: 'center', paddingVertical: 8 },
  timerSubject: { fontSize: 16, fontWeight: '600', color: '#1e293b', marginBottom: 8 },
  timerDisplay: { fontSize: 48, fontWeight: 'bold', color: '#0ea5e9', fontVariant: ['tabular-nums'] },
  timerHint: { fontSize: 12, color: '#64748b', marginTop: 4, marginBottom: 16 },
  stopButton: { flexDirection: 'row', alignItems: 'center', backgroundColor: '#ef4444', paddingHorizontal: 24, paddingVertical: 12, borderRadius: 12, gap: 8 },
  stopButtonText: { color: '#fff', fontSize: 16, fontWeight: '700' },
  pickerLabel: { fontSize: 13, color: '#64748b', marginBottom: 4, marginTop: 12 },
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 1, borderColor: '#e2e8f0', borderRadius: 8, paddingHorizontal: 12, paddingVertical: 10, backgroundColor: '#f8fafc' },
  pickerValue: { fontSize: 15, color: '#1e293b' },
  pickerPlaceholder: { fontSize: 15, color: '#94a3b8' },
  startButton: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', backgroundColor: '#10b981', paddingVertical: 12, borderRadius: 12, gap: 8, marginTop: 16 },
  startButtonDisabled: { backgroundColor: '#94a3b8' },
  startButtonText: { color: '#fff', fontSize: 16, fontWeight: '700' },
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.4)', justifyContent: 'flex-end' },
  modalContent: { backgroundColor: '#fff', borderTopLeftRadius: 20, borderTopRightRadius: 20, maxHeight: '60%', paddingTop: 16, paddingBottom: 32 },
  modalTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', textAlign: 'center', marginBottom: 12 },
  modalItem: { flexDirection: 'row', alignItems: 'center', paddingHorizontal: 20, paddingVertical: 14, borderBottomWidth: 1, borderBottomColor: '#f1f5f9', gap: 12 },
  modalItemText: { fontSize: 16, color: '#1e293b' },
  colorDot: { width: 12, height: 12, borderRadius: 6 },
  modalCancel: { paddingVertical: 14, alignItems: 'center', borderTopWidth: 1, borderTopColor: '#e2e8f0', marginTop: 8 },
  modalCancelText: { fontSize: 16, color: '#ef4444', fontWeight: '600' },
});
