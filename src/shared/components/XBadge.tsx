import React from 'react';
import { View, Text, StyleSheet, ViewStyle } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../theme';

interface XBadgeProps {
  type: 'xp' | 'streak' | 'level' | 'achievement';
  value: string | number;
  label?: string;
  style?: ViewStyle;
}

export default function XBadge({ type, value, label, style }: XBadgeProps) {
  const { theme } = useTheme();

  const config = {
    xp: { icon: 'star' as const, color: theme.colors.xp, bg: theme.isDark ? '#3D2200' : '#FFFAF0' },
    streak: { icon: 'zap' as const, color: theme.colors.streak, bg: theme.isDark ? '#3D3200' : '#FFFFF0' },
    level: { icon: 'award' as const, color: theme.colors.level, bg: theme.isDark ? '#1A3D2A' : '#F0FFF4' },
    achievement: { icon: 'hexagon' as const, color: theme.colors.accent, bg: theme.isDark ? '#3D3200' : '#FFFFF0' },
  }[type];

  return (
    <View style={[styles.badge, { backgroundColor: config.bg, borderRadius: theme.radii.full }, style]}>
      <Feather name={config.icon} size={14} color={config.color} />
      <Text style={[styles.value, { color: config.color, fontFamily: theme.fonts.bodySemiBold }]}>{value}</Text>
      {label && <Text style={[styles.label, { color: config.color, fontFamily: theme.fonts.body }]}>{label}</Text>}
    </View>
  );
}

const styles = StyleSheet.create({
  badge: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
    paddingHorizontal: 12,
    paddingVertical: 6,
  },
  value: {
    fontSize: 13,
  },
  label: {
    fontSize: 11,
  },
});
