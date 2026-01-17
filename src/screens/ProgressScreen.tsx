import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, ScrollView, Dimensions, TouchableOpacity, Alert } from 'react-native';
import { Card, ProgressBar as PaperProgressBar, Button } from 'react-native-paper';
import { Ionicons } from '@expo/vector-icons';
import { getCourses, getStudyPlans, getStudySessions, getUserProfile, unlockAchievement } from '../utils/storage';
import { Course, StudyPlan, StudySession, Progress, UserProfile } from '../types';

export default function ProgressScreen() {
  const [courses, setCourses] = useState<Course[]>([]);
  const [studyPlans, setStudyPlans] = useState<StudyPlan[]>([]);
  const [studySessions, setStudySessions] = useState<StudySession[]>([]);
  const [progress, setProgress] = useState<Progress[]>([]);
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [totalHours, setTotalHours] = useState(0);
  const [overallProgress, setOverallProgress] = useState(0);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    const loadedCourses = await getCourses();
    const loadedPlans = await getStudyPlans();
    const loadedSessions = await getStudySessions();
    const userProfile = await getUserProfile();

    setCourses(loadedCourses);
    setStudyPlans(loadedPlans);
    setStudySessions(loadedSessions);
    setProfile(userProfile);

    calculateProgress(loadedCourses, loadedPlans, loadedSessions);
    checkAchievements(loadedCourses, loadedPlans, loadedSessions, userProfile);
  };

  const checkAchievements = async (coursesList: Course[], plansList: StudyPlan[], sessionsList: StudySession[], userProfile: UserProfile) => {
    // Check various achievements
    if (coursesList.length >= 1) await unlockAchievement('1');
    if (coursesList.length >= 5) await unlockAchievement('8');
    if (plansList.length >= 1) await unlockAchievement('3');
    
    const completedSessions = sessionsList.filter(s => s.completed).length;
    if (completedSessions >= 5) await unlockAchievement('4');
    
    const completedTopics = plansList.reduce((sum, plan) => 
      sum + plan.topics.filter(t => t.completed).length, 0
    );
    if (completedTopics >= 10) await unlockAchievement('7');
    
    const totalHours = sessionsList.filter(s => s.completed)
      .reduce((sum, s) => sum + s.duration, 0) / 60;
    if (totalHours >= 10) await unlockAchievement('5');
    
    if (userProfile.streak >= 3) await unlockAchievement('11');
    if (userProfile.streak >= 7) await unlockAchievement('6');
  };

  const calculateProgress = (
    coursesList: Course[],
    plansList: StudyPlan[],
    sessionsList: StudySession[]
  ) => {
    const progressData: Progress[] = coursesList.map(course => {
      const coursePlans = plansList.filter(p => p.courseId === course.id);
      const courseSessions = sessionsList.filter(s => s.courseId === course.id);

      const totalTopics = coursePlans.reduce((sum, plan) => sum + plan.topics.length, 0);
      const completedTopics = coursePlans.reduce(
        (sum, plan) => sum + plan.topics.filter(t => t.completed).length,
        0
      );

      const completedSessions = courseSessions.filter(s => s.completed).length;
      const totalSessions = courseSessions.length;

      const hoursStudied = courseSessions
        .filter(s => s.completed)
        .reduce((sum, s) => sum + s.duration, 0) / 60;

      return {
        courseId: course.id,
        courseName: course.name,
        completedSessions: completedTopics || completedSessions,
        totalSessions: totalTopics || totalSessions,
        hoursStudied,
        streak: 0, // Simplified for MVP
      };
    });

    setProgress(progressData);

    const total = progressData.reduce((sum, p) => sum + p.hoursStudied, 0);
    setTotalHours(total);

    const overall = progressData.reduce((sum, p) => {
      const percent = p.totalSessions > 0 ? (p.completedSessions / p.totalSessions) * 100 : 0;
      return sum + percent;
    }, 0) / (progressData.length || 1);
    setOverallProgress(overall);
  };

  const getMotivationalMessage = () => {
    if (overallProgress >= 75) return "You're crushing it! 🔥";
    if (overallProgress >= 50) return "Great progress! Keep going! 💪";
    if (overallProgress >= 25) return "You're making moves! 🚀";
    return "Let's get started! 🌟";
  };

  const getProgressColor = (percentage: number) => {
    if (percentage >= 75) return '#10b981';
    if (percentage >= 50) return '#f59e0b';
    if (percentage >= 25) return '#3b82f6';
    return '#94a3b8';
  };

  return (
    <ScrollView style={styles.container}>
      {/* Overall Progress Card */}
      <Card style={styles.overallCard}>
        <Card.Content>
          <Text style={styles.overallTitle}>🎮 Your Quest Progress</Text>
          <Text style={styles.motivationText}>{getMotivationalMessage()}</Text>
          
          <View style={styles.progressCircle}>
            <Text style={styles.progressPercentage}>
              {Math.round(overallProgress)}%
            </Text>
            <Text style={styles.progressLabel}>Complete</Text>
          </View>

          <View style={styles.statsRow}>
            <View style={styles.statItem}>
              <Text style={styles.statIcon}>⏱️</Text>
              <Text style={styles.statValue}>{totalHours.toFixed(1)}h</Text>
              <Text style={styles.statLabel}>Study Time</Text>
            </View>
            <View style={styles.statItem}>
              <Text style={styles.statIcon}>⚡</Text>
              <Text style={styles.statValue}>{profile?.xp || 0}</Text>
              <Text style={styles.statLabel}>Total XP</Text>
            </View>
            <View style={styles.statItem}>
              <Text style={styles.statIcon}>🏆</Text>
              <Text style={styles.statValue}>{profile?.level || 1}</Text>
              <Text style={styles.statLabel}>Level</Text>
            </View>
          </View>
        </Card.Content>
      </Card>

      {/* Course Progress */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Course Progress</Text>
        {progress.length === 0 ? (
          <Text style={styles.emptyText}>No progress data yet. Start studying!</Text>
        ) : (
          progress.map(item => {
            const percentage = item.totalSessions > 0
              ? (item.completedSessions / item.totalSessions) * 100
              : 0;
            return (
              <Card key={item.courseId} style={styles.courseCard}>
                <Card.Content>
                  <View style={styles.courseHeader}>
                    <Text style={styles.courseName}>{item.courseName}</Text>
                    <Text style={styles.coursePercentage}>
                      {Math.round(percentage)}%
                    </Text>
                  </View>

                  <PaperProgressBar
                    progress={percentage / 100}
                    color={getProgressColor(percentage)}
                    style={styles.progressBar}
                  />

                  <View style={styles.courseStats}>
                    <View style={styles.courseStat}>
                      <Text style={styles.courseStatValue}>
                        {item.completedSessions}/{item.totalSessions}
                      </Text>
                      <Text style={styles.courseStatLabel}>Sessions</Text>
                    </View>
                    <View style={styles.courseStat}>
                      <Text style={styles.courseStatValue}>
                        {item.hoursStudied.toFixed(1)}h
                      </Text>
                      <Text style={styles.courseStatLabel}>Studied</Text>
                    </View>
                  </View>
                </Card.Content>
              </Card>
            );
          })
        )}
      </View>

      {/* Achievements */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>🏆 Achievements Unlocked</Text>
        <Text style={styles.achievementCount}>
          {profile?.achievements.filter(a => a.unlocked).length || 0} / {profile?.achievements.length || 12}
        </Text>
        <View style={styles.achievementsGrid}>
          {profile?.achievements.map((achievement) => (
            <TouchableOpacity 
              key={achievement.id}
              onPress={() => {
                Alert.alert(
                  achievement.unlocked ? `${achievement.icon} ${achievement.title}` : '🔒 Locked',
                  achievement.unlocked 
                    ? `${achievement.description}\n\n+${achievement.xpReward} XP earned!`
                    : `${achievement.description}\n\nKeep going to unlock this!`,
                  [{ text: 'OK' }]
                );
              }}
            >
              <Card style={[
                styles.achievementCard,
                !achievement.unlocked && styles.achievementLocked
              ]}>
                <Card.Content style={styles.achievementContent}>
                  <Text style={styles.achievementIcon}>
                    {achievement.unlocked ? achievement.icon : '🔒'}
                  </Text>
                  <Text style={[
                    styles.achievementTitle,
                    !achievement.unlocked && styles.achievementTitleLocked
                  ]}>
                    {achievement.title}
                  </Text>
                  {achievement.unlocked && (
                    <Text style={styles.achievementXP}>+{achievement.xpReward} XP</Text>
                  )}
                </Card.Content>
              </Card>
            </TouchableOpacity>
          ))}
        </View>
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f8fafc',
  },
  overallCard: {
    margin: 16,
    backgroundColor: '#fff',
  },
  overallTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#1e293b',
    textAlign: 'center',
  },
  motivationText: {
    fontSize: 16,
    color: '#8b5cf6',
    textAlign: 'center',
    marginTop: 4,
    marginBottom: 20,
  },
  progressCircle: {
    width: 140,
    height: 140,
    borderRadius: 70,
    backgroundColor: '#f1f5f9',
    alignSelf: 'center',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 24,
    borderWidth: 8,
    borderColor: '#ec4899',
  },
  progressPercentage: {
    fontSize: 36,
    fontWeight: 'bold',
    color: '#ec4899',
  },
  progressLabel: {
    fontSize: 12,
    color: '#64748b',
    marginTop: 4,
  },
  statsRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
  },
  statItem: {
    alignItems: 'center',
  },
  statIcon: {
    fontSize: 32,
  },
  statValue: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#1e293b',
    marginTop: 8,
  },
  statLabel: {
    fontSize: 12,
    color: '#64748b',
    marginTop: 4,
  },
  section: {
    marginHorizontal: 16,
    marginBottom: 24,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#1e293b',
    marginBottom: 4,
  },
  achievementCount: {
    fontSize: 14,
    color: '#ec4899',
    fontWeight: '600',
    marginBottom: 12,
  },
  achievementsGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 12,
  },
  achievementCard: {
    width: (Dimensions.get('window').width - 56) / 2,
  },
  achievementLocked: {
    opacity: 0.5,
    backgroundColor: '#f1f5f9',
  },
  achievementContent: {
    alignItems: 'center',
    padding: 12,
  },
  achievementIcon: {
    fontSize: 40,
    marginBottom: 8,
  },
  achievementTitle: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1e293b',
    textAlign: 'center',
  },
  achievementTitleLocked: {
    color: '#94a3b8',
  },
  achievementXP: {
    fontSize: 12,
    color: '#fbbf24',
    fontWeight: 'bold',
    marginTop: 4,
  },
  achievementDesc: {
    fontSize: 11,
    color: '#64748b',
    textAlign: 'center',
    marginTop: 4,
  },
});
