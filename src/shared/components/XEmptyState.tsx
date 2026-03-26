import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../theme';
import AfricanPattern from './AfricanPattern';

interface XEmptyStateProps {
  icon?: keyof typeof Feather.glyphMap;
  title: string;
  message?: string;
}

export default function XEmptyState({ icon = 'inbox', title, message }: XEmptyStateProps) {
  const { theme } = useTheme();

  return (
    <View style={[styles.container, { backgroundColor: theme.colors.card, borderRadius: theme.radii.lg }]}>
      <AfricanPattern variant="empty-state" color={theme.colors.primary} />
      <Feather name={icon} size={48} color={theme.colors.textTertiary} />
      <Text style={[styles.title, { color: theme.colors.text, fontFamily: theme.fonts.heading }]}>{title}</Text>
      {message && (
        <Text style={[styles.message, { color: theme.colors.textSecondary, fontFamily: theme.fonts.body }]}>
          {message}
        </Text>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    padding: 40,
    alignItems: 'center',
    justifyContent: 'center',
    minHeight: 200,
    overflow: 'hidden',
  },
  title: {
    fontSize: 18,
    marginTop: 16,
    textAlign: 'center',
  },
  message: {
    fontSize: 14,
    marginTop: 8,
    textAlign: 'center',
  },
});
