import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { useColorScheme } from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { lightColors, darkColors, ThemeColors } from './colors';
import { fonts, fontSizes, lineHeights } from './typography';
import { spacing, radii } from './spacing';
import { shadows } from './shadows';

const THEME_KEY = '@xamxam_theme';

export interface Theme {
  colors: ThemeColors;
  fonts: typeof fonts;
  fontSizes: typeof fontSizes;
  lineHeights: typeof lineHeights;
  spacing: typeof spacing;
  radii: typeof radii;
  shadows: typeof shadows;
  isDark: boolean;
}

interface ThemeContextType {
  theme: Theme;
  isDark: boolean;
  toggleTheme: () => void;
  setDarkMode: (dark: boolean) => void;
}

const ThemeContext = createContext<ThemeContextType | undefined>(undefined);

export function useTheme(): ThemeContextType {
  const ctx = useContext(ThemeContext);
  if (!ctx) throw new Error('useTheme must be used within ThemeProvider');
  return ctx;
}

export const ThemeProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const systemScheme = useColorScheme();
  const [isDark, setIsDark] = useState(systemScheme === 'dark');
  const [loaded, setLoaded] = useState(false);

  useEffect(() => {
    AsyncStorage.getItem(THEME_KEY).then((val) => {
      if (val === 'dark') setIsDark(true);
      else if (val === 'light') setIsDark(false);
      setLoaded(true);
    });
  }, []);

  const toggleTheme = useCallback(() => {
    setIsDark((prev) => {
      const next = !prev;
      AsyncStorage.setItem(THEME_KEY, next ? 'dark' : 'light');
      return next;
    });
  }, []);

  const setDarkMode = useCallback((dark: boolean) => {
    setIsDark(dark);
    AsyncStorage.setItem(THEME_KEY, dark ? 'dark' : 'light');
  }, []);

  const theme: Theme = {
    colors: isDark ? darkColors : lightColors,
    fonts,
    fontSizes,
    lineHeights,
    spacing,
    radii,
    shadows,
    isDark,
  };

  if (!loaded) return null;

  return (
    <ThemeContext.Provider value={{ theme, isDark, toggleTheme, setDarkMode }}>
      {children}
    </ThemeContext.Provider>
  );
};
