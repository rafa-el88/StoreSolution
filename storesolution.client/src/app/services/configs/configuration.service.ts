import { Injectable, inject } from '@angular/core';
import { Subject } from 'rxjs';

import { AppTranslationService } from './translation.service';
import { ThemeManager } from './theme-manager';
import { LocalStoreManager } from './local-store-manager.service';
import { DBkeys } from './db-keys';
import { Utilities } from '../helper/utilities';
import { environment } from '../../../environments/environment';

interface UserConfiguration {
  language: string | null;
  homeUrl: string | null;
  themeId: number | null;
  showDashboardNotifications: boolean | null;
}

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  private localStorage = inject(LocalStoreManager);
  private translationService = inject(AppTranslationService);
  private themeManager = inject(ThemeManager);

  constructor() {
    this.loadLocalChanges();
  }

  set language(value: string | null) {
    this._language = value;
    this.saveToLocalStore(value, DBkeys.LANGUAGE);
    this.translationService.changeLanguage(value);
  }
  get language(): string {
    return this._language ?? ConfigurationService.defaultLanguage;
  }

  set themeId(value: number) {
    value = +value;
    this._themeId = value;
    this.saveToLocalStore(value, DBkeys.THEME_ID);
    this.themeManager.installTheme(this.themeManager.getThemeByID(value));
  }
  get themeId() {
    return this._themeId ?? ConfigurationService.defaultThemeId;
  }

  set homeUrl(value: string | null) {
    this._homeUrl = value;
    this.saveToLocalStore(value, DBkeys.HOME_URL);
  }
  get homeUrl(): string {
    return this._homeUrl ?? ConfigurationService.defaultHomeUrl;
  }

  set showDashboardNotifications(value: boolean) {
    this._showDashboardNotifications = value;
    this.saveToLocalStore(value, DBkeys.SHOW_DASHBOARD_NOTIFICATIONS);
  }
  get showDashboardNotifications() {
    return this._showDashboardNotifications != null ? this._showDashboardNotifications : ConfigurationService.defaultShowDashboardNotifications;
  }

  public static readonly appVersion = '9.19.0';

  public static readonly defaultLanguage = 'pt';
  public static readonly defaultHomeUrl = '/';
  public static readonly defaultThemeId = 1;
  public static readonly defaultShowDashboardNotifications = true;


  public baseUrl = environment.baseUrl ?? Utilities.baseUrl();
  public fallbackBaseUrl = environment.fallbackBaseUrl;

  private _language: string | null = null;
  private _homeUrl: string | null = null;
  private _themeId: number | null = null;
  private _showDashboardNotifications: boolean | null = null;
  private onConfigurationImported: Subject<void> = new Subject<void>();

  configurationImported$ = this.onConfigurationImported.asObservable();

  private loadLocalChanges() {
    if (this.localStorage.exists(DBkeys.LANGUAGE)) {
      this._language = this.localStorage.getDataObject<string>(DBkeys.LANGUAGE);
      this.translationService.changeLanguage(this._language);
    } else {
      this.resetLanguage();
    }

    if (this.localStorage.exists(DBkeys.THEME_ID)) {
      this._themeId = this.localStorage.getDataObject<number>(DBkeys.THEME_ID);
      this.themeManager.installTheme(this.themeManager.getThemeByID(this._themeId as number));
    } else {
      this.resetTheme();
    }

    if (this.localStorage.exists(DBkeys.HOME_URL)) {
      this._homeUrl = this.localStorage.getDataObject<string>(DBkeys.HOME_URL);
    }

    if (this.localStorage.exists(DBkeys.SHOW_DASHBOARD_NOTIFICATIONS)) {
      this._showDashboardNotifications = this.localStorage.getDataObject<boolean>(DBkeys.SHOW_DASHBOARD_NOTIFICATIONS);
    }

    if (this.localStorage.exists(DBkeys.SHOW_DASHBOARD_WIDGET)) {
      this._showDashboardNotifications = this.localStorage.getDataObject<boolean>(DBkeys.SHOW_DASHBOARD_WIDGET);
    }
  }

  private saveToLocalStore(data: unknown, key: string) {
    setTimeout(() => this.localStorage.savePermanentData(data, key));
  }

  public import(jsonValue: string | null) {
    this.clearLocalChanges();

    if (jsonValue) {
      const importValue: UserConfiguration = Utilities.JsonTryParse(jsonValue);

      if (importValue.language != null) {
        this.language = importValue.language;
      }

      if (importValue.themeId != null) {
        this.themeId = importValue.themeId;
      }

      if (importValue.homeUrl != null) {
        this.homeUrl = importValue.homeUrl;
      }

      if (importValue.showDashboardNotifications != null) {
        this.showDashboardNotifications = importValue.showDashboardNotifications;
      }
    }

    this.onConfigurationImported.next();
  }

  public export(changesOnly = true): string {
    const exportValue: UserConfiguration = {
      language: changesOnly ? this._language : this.language,
      themeId: changesOnly ? this._themeId : this.themeId,
      homeUrl: changesOnly ? this._homeUrl : this.homeUrl,
      showDashboardNotifications: changesOnly ? this._showDashboardNotifications : this.showDashboardNotifications
    };

    return JSON.stringify(exportValue);
  }

  public clearLocalChanges() {
    this._language = null;
    this._themeId = null;
    this._homeUrl = null;
    this._showDashboardNotifications = null;

    this.localStorage.deleteData(DBkeys.LANGUAGE);
    this.localStorage.deleteData(DBkeys.THEME_ID);
    this.localStorage.deleteData(DBkeys.HOME_URL);
    this.localStorage.deleteData(DBkeys.SHOW_DASHBOARD_NOTIFICATIONS);
    this.localStorage.deleteData(DBkeys.SHOW_DASHBOARD_BANNER);
    this.localStorage.deleteData(DBkeys.SHOW_DASHBOARD_STATISTICS);
    this.localStorage.deleteData(DBkeys.SHOW_DASHBOARD_TODO);
    this.localStorage.deleteData(DBkeys.SHOW_DASHBOARD_WIDGET);

    this.resetLanguage();
    this.resetTheme();
    this.clearUserConfigKeys();
  }

  private resetLanguage() {
    const language = this.translationService.useBrowserLanguage();

    if (language) {
      this._language = language;
    } else {
      this._language = this.translationService.useDefaultLanguage();
    }
  }

  private resetTheme() {
    this.themeManager.installTheme();
    this._themeId = null;
  }


  private addKeyToUserConfigKeys(configKey: string) {
    const configKeys = this.localStorage.getDataObject<string[]>(DBkeys.USER_CONFIG_KEYS) ?? [];

    if (!configKeys.includes(configKey)) {
      configKeys.push(configKey);
      this.localStorage.savePermanentData(configKeys, DBkeys.USER_CONFIG_KEYS);
    }
  }

  private clearUserConfigKeys() {
    const configKeys = this.localStorage.getDataObject<string[]>(DBkeys.USER_CONFIG_KEYS);

    if (configKeys != null && configKeys.length > 0) {
      for (const key of configKeys) {
        this.localStorage.deleteData(key);
      }

      this.localStorage.deleteData(DBkeys.USER_CONFIG_KEYS);
    }
  }

  public saveConfiguration(data: unknown, configKey: string) {
    this.addKeyToUserConfigKeys(configKey);
    this.localStorage.savePermanentData(data, configKey);
  }

  public getConfiguration<T>(configKey: string, isDateType = false) {
    return this.localStorage.getDataObject<T>(configKey, isDateType);
  }
}
