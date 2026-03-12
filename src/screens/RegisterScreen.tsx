import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, KeyboardAvoidingView, Platform } from 'react-native';
import { TextInput, Button, Switch } from 'react-native-paper';
import { useAuth } from '../contexts/AuthContext';

export default function RegisterScreen({ navigation }: any) {
  const { register } = useAuth();
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [grade, setGrade] = useState('10');
  const [enableOtp, setEnableOtp] = useState(false);
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const handleRegister = async () => {
    if (!firstName || !lastName || !phoneNumber || !password) {
      Alert.alert('Error', 'Please fill in all required fields');
      return;
    }
    if (password !== confirmPassword) {
      Alert.alert('Error', 'Passwords do not match');
      return;
    }
    if (password.length < 6) {
      Alert.alert('Error', 'Password must be at least 6 characters');
      return;
    }

    setLoading(true);
    try {
      await register({
        phoneNumber,
        password,
        firstName,
        lastName,
        grade: parseInt(grade) || 10,
        enableOtp,
      });
      // Auth context updates, navigation switches automatically
    } catch (error: any) {
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Registration failed. Please try again.';
      Alert.alert('Registration Failed', msg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <KeyboardAvoidingView style={styles.container} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <ScrollView contentContainerStyle={styles.scrollContent} keyboardShouldPersistTaps="handled">
        <View style={styles.header}>
          <Text style={styles.logo}>🚀</Text>
          <Text style={styles.title}>Join Study Quest</Text>
          <Text style={styles.subtitle}>Start your learning adventure</Text>
        </View>

        <View style={styles.form}>
          <View style={styles.row}>
            <TextInput
              label="First Name *"
              value={firstName}
              onChangeText={setFirstName}
              style={[styles.input, styles.halfInput]}
              mode="outlined"
              outlineColor="#e2e8f0"
              activeOutlineColor="#0ea5e9"
            />
            <TextInput
              label="Last Name *"
              value={lastName}
              onChangeText={setLastName}
              style={[styles.input, styles.halfInput]}
              mode="outlined"
              outlineColor="#e2e8f0"
              activeOutlineColor="#0ea5e9"
            />
          </View>

          <TextInput
            label="Phone Number *"
            value={phoneNumber}
            onChangeText={setPhoneNumber}
            keyboardType="phone-pad"
            style={styles.input}
            mode="outlined"
            outlineColor="#e2e8f0"
            activeOutlineColor="#0ea5e9"
            left={<TextInput.Icon icon="phone" />}
          />

          <TextInput
            label="Grade"
            value={grade}
            onChangeText={setGrade}
            keyboardType="numeric"
            style={styles.input}
            mode="outlined"
            outlineColor="#e2e8f0"
            activeOutlineColor="#0ea5e9"
            left={<TextInput.Icon icon="school" />}
          />

          <TextInput
            label="Password *"
            value={password}
            onChangeText={setPassword}
            secureTextEntry={!showPassword}
            style={styles.input}
            mode="outlined"
            outlineColor="#e2e8f0"
            activeOutlineColor="#0ea5e9"
            left={<TextInput.Icon icon="lock" />}
            right={<TextInput.Icon icon={showPassword ? 'eye-off' : 'eye'} onPress={() => setShowPassword(!showPassword)} />}
          />

          <TextInput
            label="Confirm Password *"
            value={confirmPassword}
            onChangeText={setConfirmPassword}
            secureTextEntry={!showPassword}
            style={styles.input}
            mode="outlined"
            outlineColor="#e2e8f0"
            activeOutlineColor="#0ea5e9"
            left={<TextInput.Icon icon="lock-check" />}
          />

          <View style={styles.otpRow}>
            <View style={styles.otpInfo}>
              <Text style={styles.otpLabel}>Enable OTP Security</Text>
              <Text style={styles.otpHint}>Require SMS code on every login</Text>
            </View>
            <Switch value={enableOtp} onValueChange={setEnableOtp} color="#0ea5e9" />
          </View>

          <Button
            mode="contained"
            onPress={handleRegister}
            loading={loading}
            disabled={loading}
            style={styles.registerButton}
            buttonColor="#0ea5e9"
            labelStyle={styles.registerButtonLabel}
          >
            Create Account
          </Button>

          <Button
            mode="text"
            onPress={() => navigation.goBack()}
            textColor="#0ea5e9"
            style={styles.backButton}
          >
            Already have an account? Log in
          </Button>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f8fafc' },
  scrollContent: { flexGrow: 1, justifyContent: 'center', padding: 24 },
  header: { alignItems: 'center', marginBottom: 24 },
  logo: { fontSize: 48, marginBottom: 8 },
  title: { fontSize: 28, fontWeight: 'bold', color: '#0ea5e9' },
  subtitle: { fontSize: 14, color: '#94a3b8', marginTop: 4 },
  form: { backgroundColor: '#fff', borderRadius: 16, padding: 24, elevation: 2 },
  row: { flexDirection: 'row', gap: 12 },
  input: { marginBottom: 12, backgroundColor: '#fff' },
  halfInput: { flex: 1 },
  otpRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingVertical: 12, marginBottom: 16, borderTopWidth: 1, borderTopColor: '#f1f5f9' },
  otpInfo: { flex: 1 },
  otpLabel: { fontSize: 14, fontWeight: '600', color: '#1e293b' },
  otpHint: { fontSize: 12, color: '#94a3b8', marginTop: 2 },
  registerButton: { borderRadius: 8, paddingVertical: 4 },
  registerButtonLabel: { fontSize: 16, fontWeight: '600' },
  backButton: { marginTop: 12 },
});
