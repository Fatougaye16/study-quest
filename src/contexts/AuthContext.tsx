import React, { createContext, useContext, useState, useEffect, useCallback } from 'react';
import { authAPI, setTokens, clearTokens, getAccessToken, profileAPI } from '../services/api';
import { StudentResponse, AuthResponse, LoginOtpRequiredResponse } from '../types';

interface AuthState {
  user: StudentResponse | null;
  isLoading: boolean;
  isAuthenticated: boolean;
}

interface AuthContextType extends AuthState {
  login: (phoneNumber: string, password: string) => Promise<{ otpRequired?: boolean; phoneNumber?: string }>;
  register: (body: {
    phoneNumber: string;
    password: string;
    firstName: string;
    lastName: string;
    grade?: number;
    enableOtp?: boolean;
  }) => Promise<void>;
  verifyOtp: (phoneNumber: string, otpCode: string) => Promise<void>;
  logout: () => Promise<void>;
  refreshProfile: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
};

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<StudentResponse | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // On mount, check if we have a cached token and load profile
  useEffect(() => {
    (async () => {
      try {
        const token = await getAccessToken();
        if (token) {
          const { data } = await profileAPI.get();
          setUser(data);
        }
      } catch {
        await clearTokens();
      } finally {
        setIsLoading(false);
      }
    })();
  }, []);

  const handleAuthResponse = async (data: AuthResponse) => {
    await setTokens(data.accessToken, data.refreshToken);
    setUser(data.student);
  };

  const login = useCallback(async (phoneNumber: string, password: string) => {
    const { data } = await authAPI.login(phoneNumber, password);

    // If OTP is required, the response shape is { otpRequired, message, phoneNumber }
    if (data.otpRequired) {
      return { otpRequired: true, phoneNumber: data.phoneNumber as string };
    }

    // Normal login — tokens returned
    await handleAuthResponse(data as AuthResponse);
    return {};
  }, []);

  const register = useCallback(async (body: {
    phoneNumber: string;
    password: string;
    firstName: string;
    lastName: string;
    grade?: number;
    enableOtp?: boolean;
  }) => {
    const { data } = await authAPI.register(body);
    await handleAuthResponse(data as AuthResponse);
  }, []);

  const verifyOtp = useCallback(async (phoneNumber: string, otpCode: string) => {
    const { data } = await authAPI.verifyOtp(phoneNumber, otpCode);
    await handleAuthResponse(data as AuthResponse);
  }, []);

  const logout = useCallback(async () => {
    try {
      await authAPI.logout();
    } catch { /* ignore errors on logout */ }
    await clearTokens();
    setUser(null);
  }, []);

  const refreshProfile = useCallback(async () => {
    try {
      const { data } = await profileAPI.get();
      setUser(data);
    } catch { /* ignore */ }
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        isLoading,
        isAuthenticated: !!user,
        login,
        register,
        verifyOtp,
        logout,
        refreshProfile,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
