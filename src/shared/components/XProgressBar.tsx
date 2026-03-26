import React from 'react';
import { View, StyleSheet, ViewStyle } from 'react-native';
import { useTheme } from '../theme';

interface XProgressBarProps {
  progress: number; // 0 to 1
  color?: string;
  trackColor?: string;
  height?: number;
  style?: ViewStyle;
}

export default function XProgressBar({
  progress,
  color,
  trackColor,
  height = 8,
  style,
}: XProgressBarProps) {
  const { theme } = useTheme();
  const barColor = color ?? theme.colors.primary;
  const bgColor = trackColor ?? theme.colors.borderLight;
  const clampedProgress = Math.min(1, Math.max(0, progress));

  return (
    <View style={[styles.track, { backgroundColor: bgColor, height, borderRadius: height / 2 }, style]}>
      <View
        style={[
          styles.fill,
          {
            backgroundColor: barColor,
            width: `${clampedProgress * 100}%`,
            height,
            borderRadius: height / 2,
          },
        ]}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  track: {
    width: '100%',
    overflow: 'hidden',
  },
  fill: {
    position: 'absolute',
    left: 0,
    top: 0,
  },
});
