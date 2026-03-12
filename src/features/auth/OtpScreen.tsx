import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, Alert, KeyboardAvoidingView, Platform } from 'react-native';
import { TextInput, Button } from 'react-native-paper';
import { useAuth } from './context';
import { authAPI } from './api';

export default function OtpScreen({ route }: any) {
  const { phoneNumber } = route.params;
  const { verifyOtp } = useAuth();
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

  return (
    <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <View style={styles.content}>
        <Text style={styles.icon}>🔐</Text>
        <Text style={styles.title}>Verify Your Phone</Text>
        <Text style={styles.subtitle}>
          Enter the 6-digit code sent to {maskedPhone}
        </Text>

        <TextInput
          label="OTP Code"
          value={otp}
          onChangeText={(text) => setOtp(text.replace(/[^0-9]/g, '').slice(0, 6))}
          keyboardType="number-pad"
          style={styles.input}
          mode="outlined"
          outlineColor="#e2e8f0"
          activeOutlineColor="#0ea5e9"
          maxLength={6}
          left={<TextInput.Icon icon="shield-key" />}
        />

        <Button
          mode="contained"
          onPress={handleVerify}
          loading={loading}
          disabled={loading || otp.length !== 6}
          style={styles.verifyButton}
          buttonColor="#0ea5e9"
          labelStyle={styles.verifyLabel}
        >
          Verify
        </Button>

        <Button
          mode="text"
          onPress={handleResend}
          disabled={resendCooldown > 0}
          textColor="#0ea5e9"
          style={styles.resendButton}
        >
          {resendCooldown > 0 ? `Resend code in ${resendCooldown}s` : 'Resend Code'}
        </Button>
      </View>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  content: { flex: 1, justifyContent: 'center', padding: 24 },
  icon: { fontSize: 64, textAlign: 'center', marginBottom: 16 },
  title: { fontSize: 24, fontWeight: 'bold', color: '#1e293b', textAlign: 'center' },
  subtitle: { fontSize: 14, color: '#64748b', textAlign: 'center', marginTop: 8, marginBottom: 32 },
  input: { marginBottom: 20, backgroundColor: '#fff', fontSize: 24, textAlign: 'center' },
  verifyButton: { borderRadius: 8, paddingVertical: 4 },
  verifyLabel: { fontSize: 16, fontWeight: '600' },
  resendButton: { marginTop: 16 },
});
