import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectComponent, NgOptionComponent } from '@ng-select/ng-select';
import { AlertService, DialogType, MessageSeverity } from '../../../services/configs/alert.service';
import { ConfigurationService } from '../../../services/configs/configuration.service';
import { AccountService } from '../../../services/store/account.service';
import { ThemeManager } from '../../../services/configs/theme-manager';
import { Utilities } from '../../../services/helper/utilities';
import { Permissions } from '../../../models/configs/permission.model';

@Component({
    selector: 'app-user-preferences',
    templateUrl: './user-preferences.component.html',
    styleUrl: './user-preferences.component.scss',
    imports: [NgSelectComponent, FormsModule, NgOptionComponent, TranslateModule]
})
export class UserPreferencesComponent {
  private alertService = inject(AlertService);
  private accountService = inject(AccountService);
  themeManager = inject(ThemeManager);
  configurations = inject(ConfigurationService);

  reloadFromServer() {
    this.alertService.startLoadingMessage();

    this.accountService.getUserPreferences()
      .subscribe({
        next: results => {
          this.alertService.stopLoadingMessage();

          this.configurations.import(results);

          this.alertService.showMessage('Padrões carregados!', '', MessageSeverity.info);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Erro de carregamento',
            `Não é possível recuperar as preferências do usuário do servidor.\r\nErro: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        }
      });
  }

  setAsDefault() {
    this.alertService.showDialog('Tem certeza de que deseja definir a configuração atual como seus novos padrões?', DialogType.confirm,
      () => this.setAsDefaultHelper(),
      () => this.alertService.showMessage('Operação cancelada!', '', MessageSeverity.default));
  }

  setAsDefaultHelper() {
    this.alertService.startLoadingMessage('', 'Salvando novos padrões');

    this.accountService.updateUserPreferences(this.configurations.export())
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.alertService.showMessage('Novos padrões', 'Padrões de conta atualizados com sucesso', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Erro de salvamento',
            `Ocorreu um erro ao salvar os padrões de configuração.\r\nErro: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        }
      });
  }

  resetDefault() {
    this.alertService.showDialog('Tem certeza de que deseja redefinir seus padrões?', DialogType.confirm,
      () => this.resetDefaultHelper(),
      () => this.alertService.showMessage('Operação cancelada!', '', MessageSeverity.default));
  }

  resetDefaultHelper() {
    this.alertService.startLoadingMessage('', 'Redefinindo padrões');

    this.accountService.updateUserPreferences(null)
      .subscribe({
        next: () => {
          this.alertService.stopLoadingMessage();
          this.configurations.import(null);
          this.alertService.showMessage('Redefinição de padrões', 'Redefinição de padrões da conta concluída com sucesso', MessageSeverity.success);
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage('Erro de salvamento',
            `Ocorreu um erro ao redefinir os padrões de configuração.\r\nErro: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        }
      });
  }

  get canViewCustomers() {
    return this.accountService.userHasPermission(Permissions.viewUsers); 
  }

  get canViewMovies() {
    return !!true; 
  }

  get canViewOrders() {
    return !!true; 
  }
}
