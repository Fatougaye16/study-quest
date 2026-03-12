import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, TouchableOpacity } from 'react-native';
import { Card, TextInput, Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { useAuth } from '../auth/context';
import { profileAPI } from './api';

const GRADE_OPTIONS = [9, 10, 11, 12];

export default function ProfileScreen() {
  const { user, logout, refreshProfile } = useAuth();

  const [editing, setEditing] = useState(false);
  const [firstName, setFirstName] = useState(user?.firstName ?? '');
  const [lastName, setLastName] = useState(user?.lastName ?? '');
  const [grade, setGrade] = useState(user?.grade ?? 10);
  const [dailyGoal, setDailyGoal] = useState(String(user?.dailyGoalMinutes ?? 30));
  const [saving, setSaving] = useState(false);
  const [showGradePicker, setShowGradePicker] = useState(false);

  const startEditing = () => {
    setFirstName(user?.firstName ?? '');
    setLastName(user?.lastName ?? '');
    setGrade(user?.grade ?? 10);
    setDailyGoal(String(user?.dailyGoalMinutes ?? 30));
    setEditing(true);
  };

  const cancelEditing = () => {
    setEditing(false);
    setShowGradePicker(false);
  };

  const handleSave = async () => {
    const goalNum = parseInt(dailyGoal) || 30;
    if (!firstName.trim() || !lastName.trim()) {
      Alert.alert('Error', 'First name and last name are required.');
      return;
    }
    if (goalNum < 1) {
      Alert.alert('Error', 'Daily goal must be at least 1 minute.');
      return;
    }

    setSaving(true);
    try {
      await profileAPI.update({
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        grade,
        dailyGoalMinutes: goalNum,
      });
      await refreshProfile();
      setEditing(false);
      setShowGradePicker(false);
      Alert.alert('Success', 'Profile updated!');
    } catch (error: any) {
      Alert.alert('Error', error.response?.data?.detail || 'Failed to update profile.');
    } finally {
      setSaving(false);
    }
  };

  const handleLogout = () => {
    Alert.alert('Logout', 'Are you sure you want to logout?', [
      { text: 'Cancel', style: 'cancel' },
      { text: 'Logout', style: 'destructive', onPress: logout },
    ]);
  };

  const memberSince = user?.createdAt
    ? new Date(user.createdAt).toLocaleDateString('en-US', { month: 'long', year: 'numeric' })
    : '';

  return (
    <ScrollView style={styles.container}>
      <View style={styles.header}>
        <View style={styles.avatar}>
          <Text style={styles.avatarText}>
            {(user?.firstName?.[0] ?? '').toUpperCase()}
            {(user?.lastName?.[0] ?? '').toUpperCase()}
          </Text>
        </View>
        <Text style={styles.name}>{user?.firstName} {user?.lastName}</Text>
        <Text style={styles.phone}>{user?.phoneNumber}</Text>
        {memberSince ? <Text style={styles.memberSince}>Member since {memberSince}</Text> : null}
      </View>

      <View style={styles.section}>
        <View style={styles.sectionHeader}>
          <Text style={styles.sectionTitle}>Profile Info</Text>
          {!editing && (
            <TouchableOpacity onPress={startEditing} style={styles.editButton}>
              <Ionicons name="pencil" size={16} color="#0ea5e9" />
              <Text style={styles.editButtonText}>Edit</Text>
            </TouchableOpacity>
          )}
        </View>

        {editing ? (
          <Card style={styles.card}>
            <Card.Content>
              <Text style={styles.label}>First Name</Text>
              <TextInput
                value={firstName}
                onChangeText={setFirstName}
                mode="outlined"
                outlineColor="#e2e8f0"
                activeOutlineColor="#0ea5e9"
                style={styles.input}
              />

              <Text style={styles.label}>Last Name</Text>
              <TextInput
                value={lastName}
                onChangeText={setLastName}
                mode="outlined"
                outlineColor="#e2e8f0"
                activeOutlineColor="#0ea5e9"
                style={styles.input}
              />

              <Text style={styles.label}>Grade</Text>
              <TouchableOpacity
                style={styles.pickerButton}
                onPress={() => setShowGradePicker(!showGradePicker)}
              >
                <Text style={styles.pickerValue}>Grade {grade}</Text>
                <Ionicons name={showGradePicker ? 'chevron-up' : 'chevron-down'} size={20} color="#64748b" />
              </TouchableOpacity>
              {showGradePicker && (
                <View style={styles.gradeOptions}>
                  {GRADE_OPTIONS.map(g => (
                    <TouchableOpacity
                      key={g}
                      style={[styles.gradeOption, grade === g && styles.gradeOptionSelected]}
                      onPress={() => { setGrade(g); setShowGradePicker(false); }}
                    >
                      <Text style={[styles.gradeOptionText, grade === g && styles.gradeOptionTextSelected]}>
                        Grade {g}
                      </Text>
                    </TouchableOpacity>
                  ))}
                </View>
              )}

              <Text style={styles.label}>Daily Study Goal (minutes)</Text>
              <TextInput
                value={dailyGoal}
                onChangeText={setDailyGoal}
                keyboardType="numeric"
                mode="outlined"
                outlineColor="#e2e8f0"
                activeOutlineColor="#0ea5e9"
                style={styles.input}
              />

              <View style={styles.formActions}>
                <Button mode="outlined" onPress={cancelEditing} textColor="#64748b" style={styles.cancelBtn}>
                  Cancel
                </Button>
                <Button
                  mode="contained"
                  onPress={handleSave}
                  buttonColor="#0ea5e9"
                  loading={saving}
                  disabled={saving}
                  style={styles.saveBtn}
                >
                  Save
                </Button>
              </View>
            </Card.Content>
          </Card>
        ) : (
          <Card style={styles.card}>
            <Card.Content>
              <View style={styles.infoRow}>
                <Ionicons name="person-outline" size={20} color="#0ea5e9" />
                <View style={styles.infoContent}>
                  <Text style={styles.infoLabel}>Name</Text>
                  <Text style={styles.infoValue}>{user?.firstName} {user?.lastName}</Text>
                </View>
              </View>

              <View style={styles.infoRow}>
                <Ionicons name="call-outline" size={20} color="#0ea5e9" />
                <View style={styles.infoContent}>
                  <Text style={styles.infoLabel}>Phone</Text>
                  <Text style={styles.infoValue}>{user?.phoneNumber}</Text>
                </View>
              </View>

              <View style={styles.infoRow}>
                <Ionicons name="school-outline" size={20} color="#0ea5e9" />
                <View style={styles.infoContent}>
                  <Text style={styles.infoLabel}>Grade</Text>
                  <Text style={styles.infoValue}>Grade {user?.grade}</Text>
                </View>
              </View>

              <View style={styles.infoRow}>
                <Ionicons name="time-outline" size={20} color="#0ea5e9" />
                <View style={styles.infoContent}>
                  <Text style={styles.infoLabel}>Daily Study Goal</Text>
                  <Text style={styles.infoValue}>{user?.dailyGoalMinutes} minutes</Text>
                </View>
              </View>

              <View style={[styles.infoRow, { borderBottomWidth: 0 }]}>
                <Ionicons name="shield-checkmark-outline" size={20} color="#0ea5e9" />
                <View style={styles.infoContent}>
                  <Text style={styles.infoLabel}>OTP Verification</Text>
                  <Text style={styles.infoValue}>{user?.isOtpEnabled ? 'Enabled' : 'Disabled'}</Text>
                </View>
              </View>
            </Card.Content>
          </Card>
        )}
      </View>

      <View style={styles.section}>
        <TouchableOpacity style={styles.logoutButton} onPress={handleLogout}>
          <Ionicons name="log-out-outline" size={20} color="#ef4444" />
          <Text style={styles.logoutText}>Logout</Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  header: { alignItems: 'center', paddingVertical: 32, backgroundColor: '#0ea5e9' },
  avatar: { width: 80, height: 80, borderRadius: 40, backgroundColor: '#0284c7', alignItems: 'center', justifyContent: 'center', marginBottom: 12, borderWidth: 3, borderColor: 'rgba(255,255,255,0.3)' },
  avatarText: { fontSize: 28, fontWeight: 'bold', color: '#fff' },
  name: { fontSize: 22, fontWeight: 'bold', color: '#fff' },
  phone: { fontSize: 14, color: '#e2e8f0', marginTop: 4 },
  memberSince: { fontSize: 12, color: '#7dd3fc', marginTop: 4 },
  section: { marginHorizontal: 16, marginTop: 20 },
  sectionHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 },
  sectionTitle: { fontSize: 18, fontWeight: 'bold', color: '#1e293b' },
  editButton: { flexDirection: 'row', alignItems: 'center', gap: 4, paddingHorizontal: 12, paddingVertical: 6, backgroundColor: '#f0f9ff', borderRadius: 16 },
  editButtonText: { fontSize: 13, color: '#0ea5e9', fontWeight: '600' },
  card: { borderRadius: 12 },
  infoRow: { flexDirection: 'row', alignItems: 'center', paddingVertical: 14, borderBottomWidth: 1, borderBottomColor: '#f1f5f9', gap: 14 },
  infoContent: { flex: 1 },
  infoLabel: { fontSize: 12, color: '#64748b' },
  infoValue: { fontSize: 15, color: '#1e293b', fontWeight: '500', marginTop: 2 },
  label: { fontSize: 13, fontWeight: '600', color: '#1e293b', marginBottom: 4, marginTop: 14 },
  input: { backgroundColor: '#fff' },
  pickerButton: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', borderWidth: 1, borderColor: '#e2e8f0', borderRadius: 8, paddingHorizontal: 14, paddingVertical: 12, backgroundColor: '#fff' },
  pickerValue: { fontSize: 15, color: '#1e293b' },
  gradeOptions: { borderWidth: 1, borderColor: '#e2e8f0', borderRadius: 8, backgroundColor: '#fff', marginTop: 4, overflow: 'hidden' },
  gradeOption: { paddingHorizontal: 14, paddingVertical: 12, borderBottomWidth: 1, borderBottomColor: '#f1f5f9' },
  gradeOptionSelected: { backgroundColor: '#f0f9ff' },
  gradeOptionText: { fontSize: 15, color: '#1e293b' },
  gradeOptionTextSelected: { color: '#0ea5e9', fontWeight: '600' },
  formActions: { flexDirection: 'row', justifyContent: 'flex-end', gap: 12, marginTop: 20 },
  cancelBtn: { borderColor: '#e2e8f0' },
  saveBtn: {},
  logoutButton: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 8, paddingVertical: 14, backgroundColor: '#fef2f2', borderRadius: 12, marginBottom: 40 },
  logoutText: { fontSize: 16, color: '#ef4444', fontWeight: '600' },
});
