import React from 'react';
import { TouchableOpacity, Text, ActivityIndicator, ViewStyle } from 'react-native';
import { Feather } from '@expo/vector-icons';
import { useTheme } from '../theme';
import { useDownload } from '../hooks/useDownload';

interface XDownloadButtonProps {
  type: 'past-paper' | 'notes' | 'flashcards' | 'study-plan';
  id: string;
  fileName?: string;
  label?: string;
  variant?: 'icon' | 'full';
  style?: ViewStyle;
}

export default function XDownloadButton({
  type, id, fileName, label, variant = 'full', style,
}: XDownloadButtonProps) {
  const { theme } = useTheme();
  const colors = theme.colors;
  const { download, downloading } = useDownload();

  const onPress = () => download(type, id, fileName);

  if (variant === 'icon') {
    return (
      <TouchableOpacity onPress={onPress} disabled={downloading} style={[{ padding: 8 }, style]} activeOpacity={0.7}>
        {downloading ? (
          <ActivityIndicator size="small" color={colors.primary} />
        ) : (
          <Feather name="download" size={20} color={colors.primary} />
        )}
      </TouchableOpacity>
    );
  }

  return (
    <TouchableOpacity
      onPress={onPress}
      disabled={downloading}
      activeOpacity={0.7}
      style={[{
        flexDirection: 'row', alignItems: 'center', justifyContent: 'center',
        backgroundColor: colors.primary + '12', borderRadius: theme.radii.md,
        paddingVertical: 10, paddingHorizontal: 16, gap: 8,
      }, downloading && { opacity: 0.5 }, style]}
    >
      {downloading ? (
        <ActivityIndicator size="small" color={colors.primary} />
      ) : (
        <Feather name="download" size={16} color={colors.primary} />
      )}
      <Text style={{
        fontSize: 14, fontFamily: theme.fonts.bodySemiBold, color: colors.primary,
      }}>
        {downloading ? 'Downloading...' : (label || 'Download PDF')}
      </Text>
    </TouchableOpacity>
  );
}
