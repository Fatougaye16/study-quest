import { Platform } from 'react-native';

export const fonts = {
  heading: Platform.select({
    ios: 'Poppins_600SemiBold',
    android: 'Poppins_600SemiBold',
    default: 'Poppins_600SemiBold',
  }) as string,
  headingBold: Platform.select({
    ios: 'Poppins_700Bold',
    android: 'Poppins_700Bold',
    default: 'Poppins_700Bold',
  }) as string,
  body: Platform.select({
    ios: 'Inter_400Regular',
    android: 'Inter_400Regular',
    default: 'Inter_400Regular',
  }) as string,
  bodyMedium: Platform.select({
    ios: 'Inter_500Medium',
    android: 'Inter_500Medium',
    default: 'Inter_500Medium',
  }) as string,
  bodySemiBold: Platform.select({
    ios: 'Inter_600SemiBold',
    android: 'Inter_600SemiBold',
    default: 'Inter_600SemiBold',
  }) as string,
  bodyBold: Platform.select({
    ios: 'Inter_700Bold',
    android: 'Inter_700Bold',
    default: 'Inter_700Bold',
  }) as string,
};

export const fontSizes = {
  xs: 13,
  sm: 15,
  base: 17,
  lg: 19,
  xl: 22,
  '2xl': 26,
  '3xl': 32,
  '4xl': 38,
  '5xl': 50,
};

export const lineHeights = {
  xs: 16,
  sm: 20,
  base: 24,
  lg: 28,
  xl: 28,
  '2xl': 32,
  '3xl': 36,
  '4xl': 40,
  '5xl': 56,
};
