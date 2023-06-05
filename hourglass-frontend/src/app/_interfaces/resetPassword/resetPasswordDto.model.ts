export interface ResetPasswordRequest {
    newPassword: string;
    confirmNewPassword: string;
    email: string;
    token: string;
}