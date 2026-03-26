import React from 'react';
import { View, ViewStyle, StyleProp, StyleSheet } from 'react-native';
import { useTheme } from '../theme';

interface XCardProps {
  children: React.ReactNode;
  style?: StyleProp<ViewStyle>;
  variant?: 'default' | 'elevated' | 'outlined';
}

export default function XCard({ children, style, variant = 'default' }: XCardProps) {
  const { theme } = useTheme();

  return (
    <View
      style={[
        styles.card,
        {
          backgroundColor: theme.colors.card,
          borderRadius: theme.radii.lg,
        },
        variant === 'elevated' && theme.shadows.md,
        variant === 'outlined' && { borderWidth: 1, borderColor: theme.colors.border },
        variant === 'default' && theme.shadows.sm,
        style,
      ]}
    >
      {children}
    </View>
  );
}

const styles = StyleSheet.create({
  card: {
    padding: 16,
    overflow: 'hidden',
  },
});
