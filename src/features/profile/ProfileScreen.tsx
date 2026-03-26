import React, { useState, useEffect, useCallback } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, TouchableOpacity, RefreshControl } from 'react-native';
import { TextInput, Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../../shared/theme';
import { useAuth } from '../auth/context';
import { profileAPI } from './api';
import { progressAPI } from '../progress/api';
import { OverallProgress } from '../progress/types';
import AfricanPattern from '../../shared/components/AfricanPattern';

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

  const { theme } = useTheme();
  const colors = theme.colors;

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
      style={[styles.container, { backgroundColor: colors.background }]}
      refreshControl={<RefreshControl refreshing={refreshing} onRefresh={onRefresh} colors={[colors.primary]} />}
    >
      {/* ── Hero Header ── */}
      <View style={[styles.header, { backgroundColor: colors.primary }]}>
        <AfricanPattern variant="header" color="#FFFFFF" />
        <View style={styles.avatarRing}>
          <View style={[styles.avatar, { backgroundColor: colors.primary }]}>
            <Text style={[styles.avatarText, { color: colors.card, fontFamily: theme.fonts.headingBold }]}>
              {(user?.firstName?.[0] ?? '').toUpperCase()}
              {(user?.lastName?.[0] ?? '').toUpperCase()}
            </Text>
          </View>
        </View>
        <Text style={[styles.name, { color: colors.card, fontFamily: theme.fonts.headingBold }]}>{user?.firstName} {user?.lastName}</Text>
        <Text style={[styles.phone, { color: colors.primary + '20' }]}>{user?.phoneNumber}</Text>
        {memberSince ? <Text style={[styles.memberSince, { color: colors.primary + '80' }]}>Member since {memberSince}</Text> : null}

        {/* Level Badge */}
        {progress && (
          <View style={styles.levelBadge}>
            <Feather name="star" size={14} color={colors.secondary} />
            <Text style={[styles.levelText, { color: colors.card, fontFamily: theme.fonts.headingBold }]}>Level {progress.level}</Text>
            <View style={styles.xpPill}>
              <Text style={[styles.xpText, { color: colors.card, fontFamily: theme.fonts.headingBold }]}>{progress.totalXP} XP</Text>
            </View>
          </View>
        )}
      </View>

      {/* ── Stats Strip ── */}
      <View style={[styles.statsStrip, { backgroundColor: colors.card, shadowColor: colors.primary }]}>
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: colors.secondary + '20' }]}>
            <Feather name="zap" size={20} color={colors.secondary} />
          </View>
          <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{progress?.currentStreak ?? 0}</Text>
          <Text style={[styles.statLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Day Streak</Text>
        </View>
        <View style={[styles.statDivider, { backgroundColor: colors.background }]} />
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: colors.primary + '20' }]}>
            <Feather name="book" size={20} color={colors.primary} />
          </View>
          <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{progress?.totalSessions ?? 0}</Text>
          <Text style={[styles.statLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Sessions</Text>
        </View>
        <View style={[styles.statDivider, { backgroundColor: colors.background }]} />
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: colors.gamification.xp + '20' }]}>
            <Feather name="clock" size={20} color={colors.gamification.xp} />
          </View>
          <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{studyHours}h</Text>
          <Text style={[styles.statLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Studied</Text>
        </View>
        <View style={[styles.statDivider, { backgroundColor: colors.background }]} />
        <View style={styles.statItem}>
          <View style={[styles.statIcon, { backgroundColor: colors.accent + '20' }]}>
            <Feather name="book-open" size={20} color={colors.accent} />
          </View>
          <Text style={[styles.statNumber, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>{progress?.subjectsEnrolled ?? 0}</Text>
          <Text style={[styles.statLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Subjects</Text>
        </View>
      </View>

      {/* ── Personal Details ── */}
      <View style={styles.section}>
        <View style={styles.sectionHeader}>
          <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Personal Details</Text>
          {!editing && (
            <TouchableOpacity onPress={startEditing} style={[styles.editButton, { backgroundColor: colors.primary + '20' }]}>
              <Feather name="edit-2" size={14} color={colors.primary} />
              <Text style={[styles.editButtonText, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>Edit</Text>
            </TouchableOpacity>
          )}
        </View>

        {editing ? (
          <View style={[styles.card, { backgroundColor: colors.card }]}>
              <Text style={[styles.label, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>First Name</Text>
              <TextInput
                value={firstName}
                onChangeText={setFirstName}
                mode="outlined"
                outlineColor={colors.border}
                activeOutlineColor={colors.primary}
                style={[styles.input, { backgroundColor: colors.card }]}
              />

              <Text style={[styles.label, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Last Name</Text>
              <TextInput
                value={lastName}
                onChangeText={setLastName}
                mode="outlined"
                outlineColor={colors.border}
                activeOutlineColor={colors.primary}
                style={[styles.input, { backgroundColor: colors.card }]}
              />

              <Text style={[styles.label, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Grade</Text>
              <TouchableOpacity
                style={[styles.pickerButton, { borderColor: colors.border, backgroundColor: colors.card }]}
                onPress={() => setShowGradePicker(!showGradePicker)}
              >
                <Text style={[styles.pickerValue, { color: colors.text }]}>Grade {grade}</Text>
                <Feather name={showGradePicker ? 'chevron-up' : 'chevron-down'} size={20} color={colors.textSecondary} />
              </TouchableOpacity>
              {showGradePicker && (
                <View style={[styles.gradeOptions, { borderColor: colors.border, backgroundColor: colors.card }]}>
                  {GRADE_OPTIONS.map(g => (
                    <TouchableOpacity
                      key={g}
                      style={[styles.gradeOption, { borderBottomColor: colors.background }, grade === g && { backgroundColor: colors.primary + '10' }]}
                      onPress={() => { setGrade(g); setShowGradePicker(false); }}
                    >
                      <Text style={[styles.gradeOptionText, { color: colors.text }, grade === g && { color: colors.primary, fontFamily: theme.fonts.bodySemiBold }]}>
                        Grade {g}
                      </Text>
                    </TouchableOpacity>
                  ))}
                </View>
              )}

              <Text style={[styles.label, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Daily Study Goal (minutes)</Text>
              <TextInput
                value={dailyGoal}
                onChangeText={setDailyGoal}
                keyboardType="numeric"
                mode="outlined"
                outlineColor={colors.border}
                activeOutlineColor={colors.primary}
                style={[styles.input, { backgroundColor: colors.card }]}
              />

              <View style={styles.formActions}>
                <Button mode="outlined" onPress={cancelEditing} textColor={colors.textSecondary} style={[styles.cancelBtn, { borderColor: colors.border }]}>
                  Cancel
                </Button>
                <Button
                  mode="contained"
                  onPress={handleSave}
                  buttonColor={colors.primary}
                  loading={saving}
                  disabled={saving}
                  style={styles.saveBtn}
                >
                  Save
                </Button>
              </View>
          </View>
        ) : (
          <View style={[styles.card, { backgroundColor: colors.card }]}>
            <View style={[styles.infoRow, { borderBottomColor: colors.background }]}>
              <View style={[styles.infoIcon, { backgroundColor: colors.primary + '20' }]}>
                <Feather name="user" size={18} color={colors.primary} />
              </View>
              <View style={styles.infoContent}>
                <Text style={[styles.infoLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Full Name</Text>
                <Text style={[styles.infoValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{user?.firstName} {user?.lastName}</Text>
              </View>
            </View>

            <View style={[styles.infoRow, { borderBottomColor: colors.background }]}>
              <View style={[styles.infoIcon, { backgroundColor: colors.gamification.xp + '20' }]}>
                <Feather name="phone" size={18} color={colors.gamification.xp} />
              </View>
              <View style={styles.infoContent}>
                <Text style={[styles.infoLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Phone Number</Text>
                <Text style={[styles.infoValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{user?.phoneNumber}</Text>
              </View>
            </View>

            <View style={[styles.infoRow, { borderBottomColor: colors.background }]}>
              <View style={[styles.infoIcon, { backgroundColor: colors.accent + '20' }]}>
                <Feather name="award" size={18} color={colors.accent} />
              </View>
              <View style={styles.infoContent}>
                <Text style={[styles.infoLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Grade</Text>
                <Text style={[styles.infoValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Grade {user?.grade}</Text>
              </View>
            </View>

            <View style={[styles.infoRow, { borderBottomColor: colors.background }]}>
              <View style={[styles.infoIcon, { backgroundColor: colors.secondary + '20' }]}>
                <Feather name="flag" size={18} color={colors.secondary} />
              </View>
              <View style={styles.infoContent}>
                <Text style={[styles.infoLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>Daily Study Goal</Text>
                <Text style={[styles.infoValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{user?.dailyGoalMinutes} minutes / day</Text>
              </View>
            </View>

            <View style={[styles.infoRow, { borderBottomWidth: 0 }]}>
              <View style={[styles.infoIcon, { backgroundColor: colors.secondary + '20' }]}>
                <Feather name="shield" size={18} color={colors.secondary} />
              </View>
              <View style={styles.infoContent}>
                <Text style={[styles.infoLabel, { color: colors.textTertiary, fontFamily: theme.fonts.bodyMedium }]}>OTP Verification</Text>
                <View style={styles.otpRow}>
                  <Text style={[styles.infoValue, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>{user?.isOtpEnabled ? 'Enabled' : 'Disabled'}</Text>
                  <View style={[styles.otpBadge, user?.isOtpEnabled ? { backgroundColor: colors.gamification.xp + '20' } : styles.otpOff]}>
                    <View style={[styles.otpDot, user?.isOtpEnabled ? { backgroundColor: colors.gamification.xp } : styles.otpDotOff]} />
                    <Text style={[styles.otpBadgeText, { fontFamily: theme.fonts.headingBold }, user?.isOtpEnabled ? { color: colors.gamification.xp } : styles.otpTextOff]}>
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
        <Text style={[styles.sectionTitle, { color: colors.text, fontFamily: theme.fonts.headingBold }]}>Account</Text>
        <View style={[styles.card, { marginTop: 12, backgroundColor: colors.card }]}>
          <TouchableOpacity style={styles.menuRow} onPress={handleLogout}>
            <View style={[styles.infoIcon, { backgroundColor: '#fef2f2' }]}>
              <Feather name="log-out" size={18} color="#ef4444" />
            </View>
            <Text style={[styles.menuLabel, { fontFamily: theme.fonts.bodySemiBold }]}>Log Out</Text>
            <Feather name="chevron-right" size={18} color={colors.textTertiary} />
          </TouchableOpacity>
        </View>
      </View>

      <Text style={[styles.version, { color: colors.textTertiary }]}>XamXam v1.0</Text>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },

  /* Header */
  header: {
    alignItems: 'center',
    paddingTop: 48,
    paddingBottom: 28,
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
    alignItems: 'center',
    justifyContent: 'center',
  },
  avatarText: { fontSize: 32 },
  name: { fontSize: 24, letterSpacing: 0.3 },
  phone: { fontSize: 14, marginTop: 4 },
  memberSince: { fontSize: 12, marginTop: 2 },
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
  levelText: { fontSize: 14 },
  xpPill: {
    backgroundColor: 'rgba(255,255,255,0.25)',
    paddingHorizontal: 8,
    paddingVertical: 2,
    borderRadius: 10,
    marginLeft: 2,
  },
  xpText: { fontSize: 12 },

  /* Stats Strip */
  statsStrip: {
    flexDirection: 'row',
    marginHorizontal: 16,
    marginTop: -18,
    borderRadius: 18,
    paddingVertical: 18,
    paddingHorizontal: 8,
    elevation: 4,
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
  statNumber: { fontSize: 18 },
  statLabel: { fontSize: 11, marginTop: 2 },
  statDivider: { width: 1, marginVertical: 4 },

  /* Sections */
  section: { marginHorizontal: 16, marginTop: 24 },
  sectionHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  sectionTitle: { fontSize: 16, textTransform: 'uppercase', letterSpacing: 0.5 },
  editButton: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 5,
    paddingHorizontal: 14,
    paddingVertical: 7,
    borderRadius: 20,
  },
  editButtonText: { fontSize: 13 },

  /* Card */
  card: {
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
  infoLabel: { fontSize: 12 },
  infoValue: { fontSize: 15, marginTop: 2 },
  otpRow: { flexDirection: 'row', alignItems: 'center', gap: 8, marginTop: 2 },
  otpBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
    paddingHorizontal: 8,
    paddingVertical: 3,
    borderRadius: 10,
  },
  otpOn: {},
  otpOff: { backgroundColor: '#fee2e2' },
  otpDot: { width: 6, height: 6, borderRadius: 3 },
  otpDotOn: {},
  otpDotOff: { backgroundColor: '#ef4444' },
  otpBadgeText: { fontSize: 10 },
  otpTextOn: {},
  otpTextOff: { color: '#ef4444' },

  /* Menu Row */
  menuRow: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 14,
    gap: 14,
  },
  menuLabel: { flex: 1, fontSize: 15, color: '#ef4444' },

  /* Edit Form */
  label: { fontSize: 13, marginBottom: 4, marginTop: 14, marginHorizontal: 14 },
  input: { marginHorizontal: 10 },
  pickerButton: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    borderWidth: 1,
    borderRadius: 8,
    paddingHorizontal: 14,
    paddingVertical: 12,
    marginHorizontal: 14,
  },
  pickerValue: { fontSize: 15 },
  gradeOptions: {
    borderWidth: 1,
    borderRadius: 8,
    marginTop: 4,
    overflow: 'hidden',
    marginHorizontal: 14,
  },
  gradeOption: { paddingHorizontal: 14, paddingVertical: 12, borderBottomWidth: 1 },
  gradeOptionSelected: {},
  gradeOptionText: { fontSize: 15 },
  gradeOptionTextSelected: {},
  formActions: { flexDirection: 'row', justifyContent: 'flex-end', gap: 12, marginTop: 20, marginBottom: 10, marginHorizontal: 14 },
  cancelBtn: {},
  saveBtn: {},
  version: { textAlign: 'center', fontSize: 12, marginTop: 32, marginBottom: 40 },
});
