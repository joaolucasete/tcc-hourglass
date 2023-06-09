import { HttpErrorResponse } from '@angular/common/http';
import { ResetPasswordRequest } from './../../_interfaces/resetPassword/resetPasswordDto.model';
import { ActivatedRoute } from '@angular/router';
import { PasswordConfirmationValidatorService } from './../../shared/custom-validators/password-confirmation-validator.service';
import { AuthenticationService } from './../../shared/services/authentication.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  showSuccess: boolean;
  showError: boolean;
  errorMessage: string;

  private token: string;
  private email: string;

  constructor(private authService: AuthenticationService, private passConfValidator: PasswordConfirmationValidatorService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('')
    });

    this.resetPasswordForm.get('confirm').setValidators([Validators.required,
    this.passConfValidator.validateConfirmPassword(this.resetPasswordForm.get('password'))]);

    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
  }

  public validateControl = (controlName: string) => {
    return this.resetPasswordForm.get(controlName).invalid && this.resetPasswordForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string) => {
    return this.resetPasswordForm.get(controlName).hasError(errorName)
  }

  public resetPassword = async (resetPasswordFormValue) => {
    this.showError = this.showSuccess = false;
    const resetPass = { ...resetPasswordFormValue };

    const resetPassDto: ResetPasswordRequest = {
      newPassword: resetPass.password,
      confirmNewPassword: resetPass.confirm,
      token: this.token,
      email: this.email
    }

    this.authService.resetPassword('api/v1/auth/reset-password', resetPassDto)
      .subscribe({
        next: (_) => {
          this.showSuccess = true;
        },
        error: (err: HttpErrorResponse) => {
          this.showError = true;
          this.errorMessage = err.name;
        }
      })
  }
}
