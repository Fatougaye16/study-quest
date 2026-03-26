export const lightColors = {
  primary: '#BF4A0A',
  primaryLight: '#DD6B20',
  primaryDark: '#9C3D08',
  secondary: '#2B6CB0',
  secondaryLight: '#4299E1',
  accent: '#B7791F',
  accentLight: '#D69E2E',

  background: '#F7FAFC',
  surface: '#FFFFFF',
  card: '#FFFFFF',
  border: '#CBD5E0',
  borderLight: '#E2E8F0',

  text: '#1A202C',
  textSecondary: '#2D3748',
  textTertiary: '#718096',
  textInverse: '#FFFFFF',

  success: '#38A169',
  error: '#E53E3E',
  warning: '#D69E2E',
  info: '#3182CE',

  streak: '#D69E2E',
  xp: '#BF4A0A',
  level: '#2B6CB0',

  gamification: {
    streak: '#D69E2E',
    xp: '#BF4A0A',
    level: '#2B6CB0',
  },

  tabActive: '#BF4A0A',
  tabInactive: '#718096',
  headerBg: '#BF4A0A',
  headerText: '#FFFFFF',

  shadow: '#000000',
};

export const darkColors: typeof lightColors = {
  primary: '#ED8936',
  primaryLight: '#F6AD55',
  primaryDark: '#DD6B20',
  secondary: '#4299E1',
  secondaryLight: '#63B3ED',
  accent: '#ECC94B',
  accentLight: '#F6E05E',

  background: '#0F172A',
  surface: '#1E293B',
  card: '#1E293B',
  border: '#334155',
  borderLight: '#2D3748',

  text: '#F1F5F9',
  textSecondary: '#CBD5E0',
  textTertiary: '#718096',
  textInverse: '#1A202C',

  success: '#48BB78',
  error: '#FC8181',
  warning: '#ECC94B',
  info: '#63B3ED',

  streak: '#ECC94B',
  xp: '#ED8936',
  level: '#4299E1',

  gamification: {
    streak: '#ECC94B',
    xp: '#ED8936',
    level: '#4299E1',
  },

  tabActive: '#ED8936',
  tabInactive: '#718096',
  headerBg: '#1E293B',
  headerText: '#F1F5F9',

  shadow: '#000000',
};

export type ThemeColors = typeof lightColors;
