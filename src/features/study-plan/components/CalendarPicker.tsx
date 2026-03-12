import React from 'react';
import { View, Text, StyleSheet, Modal, TouchableOpacity } from 'react-native';
import { Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';

interface Props {
  visible: boolean;
  pickerDate: Date;
  selectedDate: string;
  title: string;
  onChangeMonth: (delta: number) => void;
  onSelectDate: (dateStr: string) => void;
  onDone: () => void;
  onCancel: () => void;
}

export default function CalendarPicker({
  visible, pickerDate, selectedDate, title, onChangeMonth, onSelectDate, onDone, onCancel,
}: Props) {
  const year = pickerDate.getFullYear();
  const month = pickerDate.getMonth();
  const firstDay = new Date(year, month, 1).getDay();
  const daysInMonth = new Date(year, month + 1, 0).getDate();

  const rows: React.ReactNode[] = [];
  let cells: React.ReactNode[] = [];

  for (let i = 0; i < firstDay; i++) {
    cells.push(<View key={`empty-${i}`} style={styles.calCell} />);
  }
  for (let day = 1; day <= daysInMonth; day++) {
    const dateStr = `${year}-${String(month + 1).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
    const isSelected = dateStr === selectedDate;
    cells.push(
      <TouchableOpacity
        key={day}
        style={[styles.calCell, isSelected && styles.calCellSelected]}
        onPress={() => onSelectDate(dateStr)}
      >
        <Text style={[styles.calDay, isSelected && styles.calDaySelected]}>{day}</Text>
      </TouchableOpacity>,
    );
    if ((firstDay + day) % 7 === 0 || day === daysInMonth) {
      rows.push(<View key={`row-${day}`} style={styles.calRow}>{cells}</View>);
      cells = [];
    }
  }

  return (
    <Modal visible={visible} transparent animationType="fade" onRequestClose={onCancel}>
      <View style={styles.modalOverlay}>
        <View style={styles.datePickerContainer}>
          <Text style={styles.datePickerTitle}>{title}</Text>

          <View style={styles.monthNav}>
            <TouchableOpacity onPress={() => onChangeMonth(-1)}>
              <Ionicons name="chevron-back" size={24} color="#0ea5e9" />
            </TouchableOpacity>
            <Text style={styles.monthLabel}>
              {pickerDate.toLocaleDateString('en-US', { month: 'long', year: 'numeric' })}
            </Text>
            <TouchableOpacity onPress={() => onChangeMonth(1)}>
              <Ionicons name="chevron-forward" size={24} color="#0ea5e9" />
            </TouchableOpacity>
          </View>

          <View style={styles.calRow}>
            {['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'].map(d => (
              <Text key={d} style={styles.calHeader}>{d}</Text>
            ))}
          </View>

          {rows}

          <View style={styles.datePickerButtons}>
            <Button onPress={onCancel} textColor="#64748b">Cancel</Button>
            <Button onPress={onDone} mode="contained" buttonColor="#0ea5e9">Done</Button>
          </View>
        </View>
      </View>
    </Modal>
  );
}

const styles = StyleSheet.create({
  modalOverlay: { flex: 1, backgroundColor: 'rgba(0,0,0,0.5)', justifyContent: 'center', alignItems: 'center' },
  datePickerContainer: { backgroundColor: '#fff', borderRadius: 16, padding: 20, width: '92%', maxWidth: 400 },
  datePickerTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b', textAlign: 'center', marginBottom: 16 },
  monthNav: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12, paddingHorizontal: 8 },
  monthLabel: { fontSize: 16, fontWeight: '600', color: '#1e293b' },
  calRow: { flexDirection: 'row', justifyContent: 'flex-start' },
  calHeader: { width: '14.28%', textAlign: 'center', fontSize: 12, fontWeight: '600', color: '#94a3b8', paddingVertical: 6 },
  calCell: { width: '14.28%', aspectRatio: 1, justifyContent: 'center', alignItems: 'center', borderRadius: 20 },
  calCellSelected: { backgroundColor: '#0ea5e9' },
  calDay: { fontSize: 14, color: '#1e293b' },
  calDaySelected: { color: '#ffffff', fontWeight: '700' },
  datePickerButtons: { flexDirection: 'row', justifyContent: 'flex-end', gap: 8, marginTop: 16 },
});
