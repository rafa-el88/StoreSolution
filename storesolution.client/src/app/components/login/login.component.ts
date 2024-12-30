import { Component, OnInit, OnDestroy, Input, inject } from '@angular/core';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';

import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';
import { AuthService } from '../../services/auth.service';
import { ConfigurationService } from '../../services/configuration.service';
import { Utilities } from '../../services/utilities';
import { UserLogin } from '../../models/user-login.model';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss',
    imports: [FormsModule, NgClass]
})

export class LoginComponent implements OnInit, OnDestroy {
  private alertService = inject(AlertService);
  private authService = inject(AuthService);
  private configurations = inject(ConfigurationService);

  userLogin = new UserLogin();
  isLoading = false;
  formResetToggle = true;
  modalClosedCallback: (() => void) | undefined;
  loginStatusSubscription: Subscription | undefined;

  @Input()
  isModal = false;

  ngOnInit() {
    this.userLogin.rememberMe = this.authService.rememberMe;

    if (this.getShouldRedirect()) {
      this.authService.redirectLoginUser();
    } else {
      this.loginStatusSubscription = this.authService.getLoginStatusEvent().subscribe(() => {
        if (this.getShouldRedirect()) {
          this.authService.redirectLoginUser();
        }
      });
    }
  }

  ngOnDestroy() {
    this.loginStatusSubscription?.unsubscribe();
  }

  getShouldRedirect() {
    return !this.isModal && this.authService.isLoggedIn && !this.authService.isSessionExpired;
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  closeModal() {
    if (this.modalClosedCallback) {
      this.modalClosedCallback();
    }
  }

  login() {
    this.isLoading = true;
    this.alertService.startLoadingMessage('', 'Tentando fazer login...');

    this.authService.loginWithPassword(this.userLogin.userName, this.userLogin.password, this.userLogin.rememberMe)
      .subscribe({
        next: user => {
          setTimeout(() => {
            this.alertService.stopLoadingMessage();
            this.isLoading = false;
            this.reset();

            if (!this.isModal) {
              this.alertService.showMessage('Login', `Bem-vindo ${user.userName}!`, MessageSeverity.success);
            } else {
              this.alertService.showMessage('Login', `Sessão para ${user.userName} restaurada!`, MessageSeverity.success);
              setTimeout(() => {
                this.alertService.showStickyMessage('Sessão restaurada', 'Por favor, tente sua última operação novamente', MessageSeverity.default);
              }, 500);

              this.closeModal();
            }
          }, 500);
        },
        error: error => {
          this.alertService.stopLoadingMessage();

          if (Utilities.checkNoNetwork(error)) {
            this.alertService.showStickyMessage(Utilities.noNetworkMessageCaption, Utilities.noNetworkMessageDetail, MessageSeverity.error, error);
            this.offerBackendDevServer();
          } else {
            const errorMessage = Utilities.getHttpResponseMessage(error);

            if (errorMessage) {
              this.alertService.showStickyMessage('Não é possível efetuar login', this.mapLoginErrorMessage(errorMessage), MessageSeverity.error, error);
            } else {
              this.alertService.showStickyMessage('Não é possível efetuar login',
                'Ocorreu um erro ao efetuar login. Tente novamente mais tarde..\nErro: ' + Utilities.stringify(error), MessageSeverity.error, error);
            }
          }

          setTimeout(() => {
            this.isLoading = false;
          }, 500);
        }
      });
  }

  offerBackendDevServer() {
    if (Utilities.checkIsLocalHost(location.origin) && Utilities.checkIsLocalHost(this.configurations.baseUrl)) {
      this.alertService.showDialog(
        'Caro desenvolvedor!<br />' +
        'Parece que seu servidor de API da Web de backend está inacessível ou não está em execução...<br />' +
        'Você gostaria de alternar temporariamente para o servidor de API de desenvolvimento de fallback abaixo? (Ou especifique outro)', DialogType.prompt, value => {
          this.configurations.baseUrl = value as string;
          this.alertService.showStickyMessage('API alterada!', 'A API da Web de destino foi alterada para: ' + value, MessageSeverity.warn);
        },
        null,
        null,
        null,
        this.configurations.fallbackBaseUrl);
    }
  }

  mapLoginErrorMessage(error: string) {
    if (error === 'invalid_username_or_password') {
      return 'Nome de usuário ou senha inválidos';
    }

    return error;
  }

  reset() {
    this.formResetToggle = false;

    setTimeout(() => {
      this.formResetToggle = true;
    });
  }
}
