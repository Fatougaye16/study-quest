import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, TouchableOpacity, RefreshControl } from 'react-native';
import { TextInput, Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { useAuth } from '../auth/context';
import { profileAPI } from './api';
import { progressAPI } from '../progress/api';
import { OverallProgress } from '../progress/types';

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
  const [progress, setProgress] = useState<OverallProgress | null>(null);
  const [refreshing, setRefreshing] = useState(false);

  const loadProgress = useCallback(async () => {
    try {
      const { data } = await progressAPI.get();
      setProgress(data);
    } catch {}
  }, []);

  useEffect(() => { loadProgress(); }, [loadProgress]);

  const onRefresh = useCallback(async () => {
    setRefreshing(true);
    await Promise.all([refreshProfile(), loadProgress()]);
    setRefreshing(false);
  }, [refreshProfile, loadProgress]);

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

  const studyHours = progress ? Math.round(progress.totalStudyMinutes / 60) : 0;

  return (
    <ScrollView
      style={styles.container}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={['#0ea5e9']} />}
    >
      {/* ── Hero Header ── */}
      <View style={styles.header}>
        <View style={styles.avatarRing}>
          <View style={styles.avatar}>
            <Text style={styles.avatarText}>
              {(user?.firstName?.[0] ?? '').toUpperCase()}
              {(user?.lastName?.[0] ?? '').toUpperCase()}
            </Text>
          </View>
        </View>
        <Text style={styles.name}>{user?.firstName} {user?.lastName}</Text>
        <Text style={styles.phone}>{user?.phoneNumber}</Text>
        {memberSince ? <Text style={styles.memberSince}>Member since {memberSince}</Text> : null}

        {/* Level Badge */}
        {progress && (
          <View style={styles.levelBadge}>
            <Ionicons name="star" size={14} color="#f59e0b" />
            <Text style={styles.levelText}>Level {progress.level}</Text>
            <View style={styles.xpPill}>
              <Text style={styles.xpText}>{progress.totalXP} XP</Text>
            </View>
          </View>
        )}
      </View>

      {/* ── Stats Strip ── */}
      <View style={styles.statsStrip}>
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: '#fef3c7' }]}>
            <Ionicons name="flame" size={20} color="#f59e0b" />
          </View>
          <Text style={styles.statNumber}>{progress?.currentStreak ?? 0}</Text>
          <Text style={styles.statLabel}>Day Streak</Text>
        </View>
        <View style={styles.statDivider} />
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: '#dbeafe' }]}>
            <Ionicons name="book" size={20} color="#3b82f6" />
          </View>
          <Text style={styles.statNumber}>{progress?.totalSessions ?? 0}</Text>
          <Text style={styles.statLabel}>Sessions</Text>
        </View>
        <View style={styles.statDivider} />
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: '#d1fae5' }]}>
            <Ionicons name="time" size={20} color="#10b981" />
          </View>
          <Text style={styles.statNumber}>{studyHours}h</Text>
          <Text style={styles.statLabel}>Studied</Text>
        </View>
        <View style={styles.statDivider} />
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: '#ede9fe' }]}>
            <Ionicons name="library" size={20} color="#8b5cf6" />
          </View>
          <Text style={styles.statNumber}>{progress?.subjectsEnrolled ?? 0}</Text>
          <Text style={styles.statLabel}>Subjects</Text>
        </View>
      </View>

      {/* ── Personal Details ── */}
      <View style={styles.section}>
        <View style={styles.sectionHeader}>
          <Text style={styles.sectionTitle}>Personal Details</Text>
          {!editing && (
            <TouchableOpacity onPress={startEditing} style={styles.editButton}>
              <Ionicons name="pencil" size={14} color="#0ea5e9" />
              <Text style={styles.editButtonText}>Edit</Text>
            </TouchableOpacity>
          )}
        </View>

        {editing ? (
          <View style={styles.card}>
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
          </View>
        ) : (
          <View style={styles.card}>
            <View style={styles.infoRow}>
              <View style={[styles.infoIcon, { backgroundColor: '#e0f2fe' }]}>
                <Ionicons name="person" size={18} color="#0ea5e9" />
              </View>
              <View style={styles.infoContent}>
                <Text style={styles.infoLabel}>Full Name</Text>
                <Text style={styles.infoValue}>{user?.firstName} {user?.lastName}</Text>
              </View>
            </View>

            <View style={styles.infoRow}>
              <View style={[styles.infoIcon, { backgroundColor: '#d1fae5' }]}>
                <Ionicons name="call" size={18} color="#10b981" />
              </View>
              <View style={styles.infoContent}>
                <Text style={styles.infoLabel}>Phone Number</Text>
                <Text style={styles.infoValue}>{user?.phoneNumber}</Text>
              </View>
            </View>

            <View style={styles.infoRow}>
              <View style={[styles.infoIcon, { backgroundColor: '#ede9fe' }]}>
                <Ionicons name="school" size={18} color="#8b5cf6" />
              </View>
              <View style={styles.infoContent}>
                <Text style={styles.infoLabel}>Grade</Text>
                <Text style={styles.infoValue}>Grade {user?.grade}</Text>
              </View>
            </View>

            <View style={styles.infoRow}>
              <View style={[styles.infoIcon, { backgroundColor: '#fef3c7' }]}>
                <Ionicons name="flag" size={18} color="#f59e0b" />
              </View>
              <View style={styles.infoContent}>
                <Text style={styles.infoLabel}>Daily Study Goal</Text>
                <Text style={styles.infoValue}>{user?.dailyGoalMinutes} minutes / day</Text>
              </View>
            </View>

            <View style={[styles.infoRow, { borderBottomWidth: 0 }]}>
              <View style={[styles.infoIcon, { backgroundColor: '#fce7f3' }]}>
                <Ionicons name="shield-checkmark" size={18} color="#ec4899" />
              </View>
              <View style={styles.infoContent}>
                <Text style={styles.infoLabel}>OTP Verification</Text>
                <View style={styles.otpRow}>
                  <Text style={styles.infoValue}>{user?.isOtpEnabled ? 'Enabled' : 'Disabled'}</Text>
                  <View style={[styles.otpBadge, user?.isOtpEnabled ? styles.otpOn : styles.otpOff]}>
                    <View style={[styles.otpDot, user?.isOtpEnabled ? styles.otpDotOn : styles.otpDotOff]} />
                    <Text style={[styles.otpBadgeText, user?.isOtpEnabled ? styles.otpTextOn : styles.otpTextOff]}>
                      {user?.isOtpEnabled ? 'ON' : 'OFF'}
                    </Text>
                  </View>
                </View>
              </View>
            </View>
          </View>
        )}
      </View>

      {/* ── Account ── */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Account</Text>
        <View style={[styles.card, { marginTop: 12 }]}>
          <TouchableOpacity style={styles.menuRow} onPress={handleLogout}>
            <View style={[styles.infoIcon, { backgroundColor: '#fef2f2' }]}>
              <Ionicons name="log-out" size={18} color="#ef4444" />
            </View>
            <Text style={styles.menuLabel}>Log Out</Text>
            <Ionicons name="chevron-forward" size={18} color="#cbd5e1" />
          </TouchableOpacity>
        </View>
      </View>

      <Text style={styles.version}>Study Quest v1.0</Text>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f1f5f9' },

  /* Header */
  header: {
    alignItems: 'center',
    paddingTop: 48,
    paddingBottom: 28,
    backgroundColor: '#0ea5e9',
    borderBottomLeftRadius: 28,
    borderBottomRightRadius: 28,
  },
  avatarRing: {
    width: 100,
    height: 100,
    borderRadius: 50,
    borderWidth: 3,
    borderColor: 'rgba(255,255,255,0.35)',
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 14,
  },
  avatar: {
    width: 88,
    height: 88,
    borderRadius: 44,
    backgroundColor: '#0284c7',
    alignItems: 'center',
    justifyContent: 'center',
  },
  avatarText: { fontSize: 32, fontWeight: 'bold', color: '#fff' },
  name: { fontSize: 24, fontWeight: '800', color: '#fff', letterSpacing: 0.3 },
  phone: { fontSize: 14, color: '#e0f2fe', marginTop: 4 },
  memberSince: { fontSize: 12, color: '#7dd3fc', marginTop: 2 },
  levelBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
    marginTop: 14,
    backgroundColor: 'rgba(255,255,255,0.18)',
    paddingHorizontal: 14,
    paddingVertical: 7,
    borderRadius: 20,
  },
  levelText: { fontSize: 14, fontWeight: '700', color: '#fff' },
  xpPill: {
    backgroundColor: 'rgba(255,255,255,0.25)',
    paddingHorizontal: 8,
    paddingVertical: 2,
    borderRadius: 10,
    marginLeft: 2,
  },
  xpText: { fontSize: 12, fontWeight: '700', color: '#fff' },

  /* Stats Strip */
  statsStrip: {
    flexDirection: 'row',
    backgroundColor: '#fff',
    marginHorizontal: 16,
    marginTop: -18,
    borderRadius: 18,
    paddingVertical: 18,
    paddingHorizontal: 8,
    elevation: 4,
    shadowColor: '#0ea5e9',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.1,
    shadowRadius: 12,
  },
  statItem: { flex: 1, alignItems: 'center' },
  statIcon: {
    width: 40,
    height: 40,
    borderRadius: 12,
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 8,
  },
  statNumber: { fontSize: 18, fontWeight: '800', color: '#1e293b' },
  statLabel: { fontSize: 11, color: '#94a3b8', marginTop: 2, fontWeight: '500' },
  statDivider: { width: 1, backgroundColor: '#f1f5f9', marginVertical: 4 },

  /* Sections */
  section: { marginHorizontal: 16, marginTop: 24 },
  sectionHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  sectionTitle: { fontSize: 16, fontWeight: '700', color: '#475569', textTransform: 'uppercase', letterSpacing: 0.5 },
  editButton: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 5,
    paddingHorizontal: 14,
    paddingVertical: 7,
    backgroundColor: '#e0f2fe',
    borderRadius: 20,
  },
  editButtonText: { fontSize: 13, color: '#0ea5e9', fontWeight: '700' },

  /* Card */
  card: {
    backgroundColor: '#fff',
    borderRadius: 18,
    padding: 4,
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.05,
    shadowRadius: 8,
  },

  /* Info Rows */
  infoRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 14,
    borderBottomWidth: 1,
    borderBottomColor: '#f1f5f9',
    gap: 14,
  },
  infoIcon: {
    width: 40,
    height: 40,
    borderRadius: 12,
    alignItems: 'center',
    justifyContent: 'center',
  },
  infoContent: { flex: 1 },
  infoLabel: { fontSize: 12, color: '#94a3b8', fontWeight: '500' },
  infoValue: { fontSize: 15, color: '#1e293b', fontWeight: '600', marginTop: 2 },
  otpRow: { flexDirection: 'row', alignItems: 'center', gap: 8, marginTop: 2 },
  otpBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
    paddingHorizontal: 8,
    paddingVertical: 3,
    borderRadius: 10,
  },
  otpOn: { backgroundColor: '#d1fae5' },
  otpOff: { backgroundColor: '#fee2e2' },
  otpDot: { width: 6, height: 6, borderRadius: 3 },
  otpDotOn: { backgroundColor: '#10b981' },
  otpDotOff: { backgroundColor: '#ef4444' },
  otpBadgeText: { fontSize: 10, fontWeight: '800' },
  otpTextOn: { color: '#10b981' },
  otpTextOff: { color: '#ef4444' },

  /* Menu Row */
  menuRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 14,
    gap: 14,
  },
  menuLabel: { flex: 1, fontSize: 15, fontWeight: '600', color: '#ef4444' },

  /* Edit Form */
  label: { fontSize: 13, fontWeight: '600', color: '#1e293b', marginBottom: 4, marginTop: 14, marginHorizontal: 14 },
  input: { backgroundColor: '#fff', marginHorizontal: 10 },
  pickerButton: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#e2e8f0',
    borderRadius: 8,
    paddingHorizontal: 14,
    paddingVertical: 12,
    backgroundColor: '#fff',
    marginHorizontal: 14,
  },
  pickerValue: { fontSize: 15, color: '#1e293b' },
  gradeOptions: {
    borderWidth: 1,
    borderColor: '#e2e8f0',
    borderRadius: 8,
    backgroundColor: '#fff',
    marginTop: 4,
    overflow: 'hidden',
    marginHorizontal: 14,
  },
  gradeOption: { paddingHorizontal: 14, paddingVertical: 12, borderBottomWidth: 1, borderBottomColor: '#f1f5f9' },
  gradeOptionSelected: { backgroundColor: '#f0f9ff' },
  gradeOptionText: { fontSize: 15, color: '#1e293b' },
  gradeOptionTextSelected: { color: '#0ea5e9', fontWeight: '600' },
  formActions: { flexDirection: 'row', justifyContent: 'flex-end', gap: 12, marginTop: 20, marginBottom: 10, marginHorizontal: 14 },
  cancelBtn: { borderColor: '#e2e8f0' },
  saveBtn: {},
  version: { textAlign: 'center', color: '#cbd5e1', fontSize: 12, marginTop: 32, marginBottom: 40 },
});
