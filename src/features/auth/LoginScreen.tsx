import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Alert, KeyboardAvoidingView, Platform, Image } from 'react-native';
import { TextInput, Button } from 'react-native-paper';
import { useAuth } from './context';
import { useTheme } from '../../shared/theme';
import AfricanPattern from '../../shared/components/AfricanPattern';

export default function LoginScreen({ navigation }: any) {
  const { login } = useAuth();
  const { theme } = useTheme();
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const handleLogin = async () => {
    if (!phoneNumber || !password) {
      Alert.alert('Error', 'Please enter phone number and password');
      return;
    }

    setLoading(true);
    try {
      const result = await login(phoneNumber, password);
      if (result.otpRequired) {
        navigation.navigate('OtpVerify', { phoneNumber: result.phoneNumber });
      }
    } catch (error: any) {
      const msg = error.response?.data?.detail || error.response?.data?.title || 'Login failed. Please try again.';
      Alert.alert('Login Failed', msg);
    } finally {
      setLoading(false);
    }
  };

  const colors = theme.colors;

  return (
    <KeyboardAvoidingView style={[styles.container, { backgroundColor: colors.background }]} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <ScrollView contentContainerStyle={styles.scrollContent} keyboardShouldPersistTaps="handled">
        <View style={styles.header}>
          <AfricanPattern variant="background" color={colors.primary} width={400} height={300} />
          <Image
            source={require('../../../assets/xamxam.png')}
            style={styles.logo}
            resizeMode="contain"
          />
          <Text style={[styles.title, { color: colors.primary, fontFamily: theme.fonts.headingBold }]}>XamXam</Text>
          <Text style={[styles.subtitle, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>Yombal Gestu</Text>
        </View>

        <View style={[styles.form, { backgroundColor: colors.card, borderRadius: theme.radii.lg }, theme.shadows.md]}>
          <Text style={[styles.formTitle, { color: colors.text, fontFamily: theme.fonts.heading }]}>Welcome Back!</Text>

          <TextInput
            label="Phone Number"
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
            label="Password"
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

          <Button
            mode="contained"
            onPress={handleLogin}
            loading={loading}
            disabled={loading}
            style={[styles.loginButton, { borderRadius: theme.radii.md }]}
            buttonColor={colors.primary}
            labelStyle={[styles.loginButtonLabel, { fontFamily: theme.fonts.bodySemiBold }]}
          >
            Log In
          </Button>

          <View style={styles.divider}>
            <View style={[styles.dividerLine, { backgroundColor: colors.border }]} />
            <Text style={[styles.dividerText, { color: colors.textTertiary, fontFamily: theme.fonts.body }]}>OR</Text>
            <View style={[styles.dividerLine, { backgroundColor: colors.border }]} />
          </View>

          <Button
            mode="outlined"
            onPress={() => navigation.navigate('Register')}
            style={[styles.registerButton, { borderColor: colors.primary, borderRadius: theme.radii.md }]}
            textColor={colors.primary}
            labelStyle={{ fontFamily: theme.fonts.bodySemiBold }}
          >
            Create Account
          </Button>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1 },
  scrollContent: { flexGrow: 1, justifyContent: 'center', padding: 24 },
  header: { alignItems: 'center', marginBottom: 40, overflow: 'hidden' },
  logo: { width: 80, height: 80, marginBottom: 12 },
  title: { fontSize: 32 },
  subtitle: { fontSize: 14, marginTop: 4 },
  form: { padding: 24 },
  formTitle: { fontSize: 22, marginBottom: 20, textAlign: 'center' },
  input: { marginBottom: 16 },
  loginButton: { marginTop: 8, paddingVertical: 4 },
  loginButtonLabel: { fontSize: 16 },
  divider: { flexDirection: 'row', alignItems: 'center', marginVertical: 20 },
  dividerLine: { flex: 1, height: 1 },
  dividerText: { marginHorizontal: 12, fontSize: 12 },
  registerButton: {},
});
