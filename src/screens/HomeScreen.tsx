import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, TouchableOpacity, Animated } from 'react-native';
import { Card, Button, ProgressBar } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { getCourses, getTimetable, getStudySessions, getUserProfile, addXP } from '../utils/storage';
import { Course, TimetableSlot, StudySession, UserProfile } from '../types';

export default function HomeScreen() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [todayClasses, setTodayClasses] = useState<TimetableSlot[]>([]);
  const [recentSessions, setRecentSessions] = useState<StudySession[]>([]);
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [dailyProgress, setDailyProgress] = useState(0);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const loadedCourses = await getCourses();
    const timetable = await getTimetable();
    const sessions = await getStudySessions();
    const userProfile = await getUserProfile();

    setCourses(loadedCourses);
    setProfile(userProfile);

    // Get today's classes
    const today = new Date().toLocaleDateString('en-US', { weekday: 'long' });
    const todaySlots = timetable.filter(slot => slot.day === today);
    setTodayClasses(todaySlots);

    // Get recent sessions
    const recent = sessions.slice(-5).reverse();
    setRecentSessions(recent);

    // Calculate daily progress
    const todaySessions = sessions.filter(s => 
      new Date(s.date).toDateString() === new Date().toDateString() && s.completed
    );
    const todayMinutes = todaySessions.reduce((sum, s) => sum + s.duration, 0);
    setDailyProgress(Math.min((todayMinutes / userProfile.dailyGoal) * 100, 100));
  };

  const getMotivationalMessage = () => {
    if (!profile) return "Welcome to Study Quest! 🎮";
    
    const level = profile.level;
    const messages = [
      `Level ${level} Scholar! You're crushing it! 🌟`,
      `${profile.xp} XP and counting! Keep going! 💪`,
      "One quest closer to mastery! 🎯",
      "Your learning adventure continues! 🚀",
      `${profile.streak} day streak! You're on fire! 🔥`,
      "Every study session is a victory! ✨",
      "You're becoming legendary! 👑",
    ];
    return messages[Math.floor(Math.random() * messages.length)];
  };

  const getLevelProgress = () => {
    if (!profile) return 0;
    const currentLevelXP = (profile.level - 1) * 500;
    const nextLevelXP = profile.level * 500;
    return ((profile.xp - currentLevelXP) / (nextLevelXP - currentLevelXP)) * 100;
  };

  return (
    <ScrollView style={styles.container}>
      {/* Level & XP Banner */}
      <Card style={styles.levelCard}>
        <Card.Content>
          <View style={styles.levelHeader}>
            <View>
              <Text style={styles.levelText}>Level {profile?.level || 1}</Text>
              <Text style={styles.xpText}>{profile?.xp || 0} XP</Text>
            </View>
            <Text style={styles.levelIcon}>🎮</Text>
          </View>
          <ProgressBar 
            progress={getLevelProgress() / 100} 
            color="#fbbf24" 
            style={styles.xpBar}
          />
          <Text style={styles.xpNextLevel}>
            {500 - ((profile?.xp || 0) % 500)} XP to Level {(profile?.level || 1) + 1}
          </Text>
        </Card.Content>
      </Card>

      {/* Motivational Banner */}
      <Card style={styles.motivationCard}>
        <Card.Content>
          <Text style={styles.motivationText}>{getMotivationalMessage()}</Text>
          <View style={styles.statsRow}>
            <View style={styles.statBadge}>
              <Ionicons name="flame" size={20} color="#ff6b35" />
              <Text style={styles.badgeText}>{profile?.streak || 0} day streak</Text>
            </View>
            <View style={styles.statBadge}>
              <Ionicons name="trophy" size={20} color="#fbbf24" />
              <Text style={styles.badgeText}>
                {profile?.achievements.filter(a => a.unlocked).length || 0} achievements
              </Text>
            </View>
          </View>
        </Card.Content>
      </Card>

      {/* Daily Goal */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>⚡ Daily Quest</Text>
        <Card style={styles.goalCard}>
          <Card.Content>
            <View style={styles.goalHeader}>
              <Text style={styles.goalTitle}>Study for {profile?.dailyGoal || 60} minutes</Text>
              <Text style={styles.goalPercentage}>{Math.round(dailyProgress)}%</Text>
            </View>
            <ProgressBar 
              progress={dailyProgress / 100} 
              color={dailyProgress >= 100 ? "#10b981" : "#ec4899"}
              style={styles.goalBar}
            />
            {dailyProgress >= 100 ? (
              <Text style={styles.goalComplete}>🎉 Daily quest complete! +50 XP</Text>
            ) : (
              <Text style={styles.goalRemaining}>
                {Math.max(0, (profile?.dailyGoal || 60) - (dailyProgress / 100 * (profile?.dailyGoal || 60)))} minutes remaining
              </Text>
            )}
          </Card.Content>
        </Card>
      </View>

      {/* Today's Classes */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Today's Classes</Text>
        {todayClasses.length > 0 ? (
          todayClasses.map((slot) => (
            <Card key={slot.id} style={[styles.classCard, { borderLeftColor: slot.color }]}>
              <Card.Content>
                <Text style={styles.courseName}>{slot.courseName}</Text>
                <Text style={styles.classTime}>
                  {slot.startTime} - {slot.endTime}
                </Text>
                {slot.location && (
                  <Text style={styles.location}>📍 {slot.location}</Text>
                )}
              </Card.Content>
            </Card>
          ))
        ) : (
          <Text style={styles.emptyText}>No classes scheduled for today 🎉</Text>
        )}
      </View>

      {/* Quick Stats */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Quick Stats</Text>
        <View style={styles.statsContainer}>
          <Card style={styles.statCard}>
            <Card.Content style={styles.statContent}>
              <Ionicons name="book" size={32} color="#6366f1" />
              <Text style={styles.statNumber}>{courses.length}</Text>
              <Text style={styles.statLabel}>Courses</Text>
            </Card.Content>
          </Card>
          <Card style={styles.statCard}>
            <Card.Content style={styles.statContent}>
              <Ionicons name="checkmark-circle" size={32} color="#10b981" />
              <Text style={styles.statNumber}>
                {recentSessions.filter(s => s.completed).length}
              </Text>
              <Text style={styles.statLabel}>Completed</Text>
            </Card.Content>
          </Card>
        </View>
      </View>

      {/* Recent Study Sessions */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Recent Study Sessions</Text>
        {recentSessions.length > 0 ? (
          recentSessions.map((session) => (
            <Card key={session.id} style={styles.sessionCard}>
              <Card.Content>
                <View style={styles.sessionHeader}>
                  <Text style={styles.sessionCourse}>{session.courseName}</Text>
                  {session.completed && (
                    <Ionicons name="checkmark-circle" size={20} color="#10b981" />
                  )}
                </View>
                <Text style={styles.sessionTopic}>{session.topic}</Text>
                <Text style={styles.sessionDuration}>{session.duration} minutes</Text>
              </Card.Content>
            </Card>
          ))
        ) : (
          <Text style={styles.emptyText}>Start your first study session!</Text>
        )}
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f8fafc',
  },
  levelCard: {
    margin: 16,
    marginBottom: 8,
    backgroundColor: '#581c87',
  },
  levelHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  levelText: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#fff',
  },
  xpText: {
    fontSize: 14,
    color: '#94a3b8',
    marginTop: 4,
  },
  levelIcon: {
    fontSize: 48,
  },
  xpBar: {
    height: 8,
    borderRadius: 4,
    backgroundColor: '#7c3aed',
  },
  xpNextLevel: {
    fontSize: 12,
    color: '#94a3b8',
    marginTop: 8,
    textAlign: 'center',
  },
  motivationCard: {
    marginHorizontal: 16,
    marginBottom: 8,
    backgroundColor: 'linear-gradient(135deg, #8b5cf6 0%, #ec4899 100%)',
    backgroundColor: '#8b5cf6',
  },
  motivationText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#fff',
    textAlign: 'center',
    marginBottom: 12,
  },
  statsRow: {
    flexDirection: 'row',
    justifyContent: 'center',
    gap: 16,
  },
  statBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: 'rgba(255,255,255,0.2)',
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 16,
    gap: 6,
  },
  badgeText: {
    color: '#fff',
    fontSize: 12,
    fontWeight: '600',
  },
  section: {
    marginHorizontal: 16,
    marginBottom: 24,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 12,
    color: '#1e293b',
  },
  goalCard: {
    borderLeftWidth: 4,
    borderLeftColor: '#ec4899',
  },
  goalHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  goalTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1e293b',
  },
  goalPercentage: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#8b5cf6',
  },
  goalBar: {
    height: 8,
    borderRadius: 4,
    marginBottom: 8,
  },
  goalComplete: {
    fontSize: 14,
    color: '#10b981',
    fontWeight: '600',
    textAlign: 'center',
  },
  goalRemaining: {
    fontSize: 12,
    color: '#64748b',
    textAlign: 'center',
  },
  classCard: {
    marginBottom: 8,
    borderLeftWidth: 4,
  },
  courseName: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1e293b',
  },
  classTime: {
    fontSize: 14,
    color: '#64748b',
    marginTop: 4,
  },
  location: {
    fontSize: 12,
    color: '#94a3b8',
    marginTop: 4,
  },
  emptyText: {
    textAlign: 'center',
    color: '#94a3b8',
    fontSize: 14,
    fontStyle: 'italic',
    padding: 16,
  },
  statsContainer: {
    flexDirection: 'row',
    gap: 12,
  },
  statCard: {
    flex: 1,
  },
  statContent: {
    alignItems: 'center',
  },
  statNumber: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#1e293b',
    marginTop: 8,
  },
  statLabel: {
    fontSize: 12,
    color: '#64748b',
  },
  sessionCard: {
    marginBottom: 8,
  },
  sessionHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  sessionCourse: {
    fontSize: 14,
    fontWeight: '600',
    color: '#6366f1',
  },
  sessionTopic: {
    fontSize: 16,
    color: '#1e293b',
    marginTop: 4,
  },
  sessionDuration: {
    fontSize: 12,
    color: '#64748b',
    marginTop: 4,
  },
});
