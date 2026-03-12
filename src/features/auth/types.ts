export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  student: StudentResponse;
}

export interface StudentResponse {
  id: string;
  phoneNumber: string;
  firstName: string;
  lastName: string;
  grade: number;
  dailyGoalMinutes: number;
  isOtpEnabled: boolean;
  createdAt: string;
}

export interface LoginOtpRequiredResponse {
  otpRequired: boolean;
  message: string;
  phoneNumber: string;
}
