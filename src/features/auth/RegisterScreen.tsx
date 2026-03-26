import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, KeyboardAvoidingView, Platform, Image } from 'react-native';
import { TextInput, Button, Switch } from 'react-native-paper';
import { useAuth } from './context';
import { useTheme } from '../../shared/theme';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function RegisterScreen({ navigation }: any) {
  const { register } = useAuth();
  const { theme } = useTheme();
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
    } catch (error: any) {
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Registration failed. Please try again.';
      Alert.alert('Registration Failed', msg);
    } finally {
      setLoading(false);
    }
  };

  const colors = theme.colors;

  return (
    <KeyboardAvoidingView style={[styles.container, { backgroundColor: colors.background }]} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <AfricanPattern variant="screen-bg" color={colors.primary} width={400} height={800} />
      <ScrollView contentContainerStyle={styles.scrollContent} keyboardShouldPersistTaps="handled">
        <View style={styles.header}>
          <Image
            source={require('../../../assets/xamxam.png')}
            style={styles.logo}
            resizeMode="contain"
          />
          <Text style={[styles.title, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>Join XamXam</Text>
          <Text style={[styles.subtitle, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>Yombal Gestu</Text>
        </View>

        <View style={[styles.form, { backgroundColor: colors.card, borderRadius: theme.radii.lg }, theme.shadows.md]}>
          <View style={styles.row}>
            <TextInput
              label="First Name *"
              value={firstName}
              onChangeText={setFirstName}
              style={[styles.input, styles.halfInput, { backgroundColor: colors.card }]}
              mode="outlined"
              outlineColor={colors.border}
              activeOutlineColor={colors.primary}
              textColor={colors.text}
            />
            <TextInput
              label="Last Name *"
              value={lastName}
              onChangeText={setLastName}
              style={[styles.input, styles.halfInput, { backgroundColor: colors.card }]}
              mode="outlined"
              outlineColor={colors.border}
              activeOutlineColor={colors.primary}
              textColor={colors.text}
            />
          </View>

          <TextInput
            label="Phone Number *"
            value={phoneNumber}
            onChangeText={setPhoneNumber}
            keyboardType="phone-pad"
            style={[styles.input, { backgroundColor: colors.card }]}
            mode="outlined"
            outlineColor={colors.border}
            activeOutlineColor={colors.primary}
            textColor={colors.text}
            left={<TextInput.Icon icon="phone" />}
          />

          <TextInput
            label="Grade"
            value={grade}
            onChangeText={setGrade}
            keyboardType="numeric"
            style={[styles.input, { backgroundColor: colors.card }]}
            mode="outlined"
            outlineColor={colors.border}
            activeOutlineColor={colors.primary}
            textColor={colors.text}
            left={<TextInput.Icon icon="school" />}
          />

          <TextInput
            label="Password *"
            value={password}
            onChangeText={setPassword}
            secureTextEntry={!showPassword}
            style={[styles.input, { backgroundColor: colors.card }]}
            mode="outlined"
            outlineColor={colors.border}
            activeOutlineColor={colors.primary}
            textColor={colors.text}
            left={<TextInput.Icon icon="lock" />}
            right={<TextInput.Icon icon={showPassword ? 'eye-off' : 'eye'} onPress={() => setShowPassword(!showPassword)} />}
          />

          <TextInput
            label="Confirm Password *"
            value={confirmPassword}
            onChangeText={setConfirmPassword}
            secureTextEntry={!showPassword}
            style={[styles.input, { backgroundColor: colors.card }]}
            mode="outlined"
            outlineColor={colors.border}
            activeOutlineColor={colors.primary}
            textColor={colors.text}
            left={<TextInput.Icon icon="lock-check" />}
          />

          <View style={[styles.otpRow, { borderTopColor: colors.borderLight }]}>
            <View style={styles.otpInfo}>
              <Text style={[styles.otpLabel, { color: colors.text, fontFamily: theme.fonts.bodySemiBold }]}>Enable OTP Security</Text>
              <Text style={[styles.otpHint, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>Require SMS code on every login</Text>
            </View>
            <Switch value={enableOtp} onValueChange={setEnableOtp} color={colors.primary} />
          </View>

          <Button
            mode="contained"
            onPress={handleRegister}
            loading={loading}
            disabled={loading}
            style={[styles.registerButton, { borderRadius: theme.radii.md }]}
            buttonColor={colors.primary}
            labelStyle={[styles.registerButtonLabel, { fontFamily: theme.fonts.bodySemiBold }]}
          >
            Create Account
          </Button>

          <Button
            mode="text"
            onPress={() => navigation.goBack()}
            textColor={colors.primary}
            style={styles.backButton}
            labelStyle={{ fontFamily: theme.fonts.bodySemiBold }}
          >
            Already have an account? Log in
          </Button>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollContent: { flexGrow: 1, justifyContent: 'center', padding: 24 },
  header: { alignItems: 'center', marginBottom: 24 },
  logo: { width: 64, height: 64, marginBottom: 8 },
  title: { fontSize: 28 },
  subtitle: { fontSize: 14, marginTop: 4 },
  form: { padding: 24 },
  row: { flexDirection: 'row', gap: 12 },
  input: { marginBottom: 12 },
  halfInput: { flex: 1 },
  otpRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingVertical: 12, marginBottom: 16, borderTopWidth: 1 },
  otpInfo: { flex: 1 },
  otpLabel: { fontSize: 14 },
  otpHint: { fontSize: 12, marginTop: 2 },
  registerButton: { paddingVertical: 4 },
  registerButtonLabel: { fontSize: 16 },
  backButton: { marginTop: 12 },
});
