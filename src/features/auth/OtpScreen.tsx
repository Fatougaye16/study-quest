import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, Alert, KeyboardAvoidingView, Platform } from 'react-native';
import { TextInput, Button } from 'react-native-paper';
import { Feather } from '@expo/vector-icons';
import { useAuth } from './context';
import { authAPI } from './api';
import { useTheme } from '../../shared/theme';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function OtpScreen({ route }: any) {
  const { phoneNumber } = route.params;
  const { verifyOtp } = useAuth();
  const { theme } = useTheme();
  const [otp, setOtp] = useState('');
  const [loading, setLoading] = useState(false);
  const [resendCooldown, setResendCooldown] = useState(0);

  useEffect(() => {
    setResendCooldown(60);
  }, []);

  useEffect(() => {
    if (resendCooldown > 0) {
      const timer = setTimeout(() => setResendCooldown(resendCooldown - 1), 1000);
      return () => clearTimeout(timer);
    }
  }, [resendCooldown]);

  const handleVerify = async () => {
    if (otp.length !== 6) {
      Alert.alert('Error', 'Please enter the 6-digit code');
      return;
    }

    setLoading(true);
    try {
      await verifyOtp(phoneNumber, otp);
    } catch (error: any) {
      const msg = error.response?.data?.detail || 'Invalid or expired code. Please try again.';
      Alert.alert('Verification Failed', msg);
    } finally {
      setLoading(false);
    }
  };

  const handleResend = async () => {
    try {
      await authAPI.requestOtp(phoneNumber);
      setResendCooldown(60);
      Alert.alert('OTP Sent', 'A new code has been sent to your phone');
    } catch (error: any) {
      const msg = error.response?.data?.detail || 'Failed to resend code';
      Alert.alert('Error', msg);
    }
  };

  const maskedPhone = phoneNumber.length > 4
    ? '***' + phoneNumber.slice(-4)
    : phoneNumber;

  const colors = theme.colors;

  return (
    <KeyboardAvoidingView style={[styles.container, { backgroundColor: colors.background }]} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
      <View style={styles.content}>
        <View style={[styles.iconCircle, { backgroundColor: colors.primaryLight + '20' }]}>
          <Feather name="shield" size={40} color={colors.primary} />
        </View>
        <Text style={[styles.title, { color: colors.text, fontFamily: theme.fonts.heading }]}>Verify Your Phone</Text>
        <Text style={[styles.subtitle, { color: colors.textSecondary, fontFamily: theme.fonts.body }]}>
          Enter the 6-digit code sent to {maskedPhone}
        </Text>

        <TextInput
          label="OTP Code"
          value={otp}
          onChangeText={(text) => setOtp(text.replace(/[^0-9]/g, '').slice(0, 6))}
          keyboardType="number-pad"
          style={[styles.input, { backgroundColor: colors.card }]}
          mode="outlined"
          outlineColor={colors.border}
          activeOutlineColor={colors.primary}
          textColor={colors.text}
          maxLength={6}
          left={<TextInput.Icon icon="shield-key" />}
        />

        <Button
          mode="contained"
          onPress={handleVerify}
          loading={loading}
          disabled={loading || otp.length !== 6}
          style={[styles.verifyButton, { borderRadius: theme.radii.md }]}
          buttonColor={colors.primary}
          labelStyle={[styles.verifyLabel, { fontFamily: theme.fonts.bodySemiBold }]}
        >
          Verify
        </Button>

        <Button
          mode="text"
          onPress={handleResend}
          disabled={resendCooldown > 0}
          textColor={colors.primary}
          style={styles.resendButton}
          labelStyle={{ fontFamily: theme.fonts.bodySemiBold }}
        >
          {resendCooldown > 0 ? `Resend code in ${resendCooldown}s` : 'Resend Code'}
        </Button>
      </View>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  content: { flex: 1, justifyContent: 'center', padding: 24 },
  iconCircle: { width: 80, height: 80, borderRadius: 40, alignItems: 'center', justifyContent: 'center', alignSelf: 'center', marginBottom: 20 },
  title: { fontSize: 24, textAlign: 'center' },
  subtitle: { fontSize: 14, textAlign: 'center', marginTop: 8, marginBottom: 32 },
  input: { marginBottom: 20, fontSize: 24, textAlign: 'center' },
  verifyButton: { paddingVertical: 4 },
  verifyLabel: { fontSize: 16 },
  resendButton: { marginTop: 16 },
});
