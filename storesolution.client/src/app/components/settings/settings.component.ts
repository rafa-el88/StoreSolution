import { Component, inject, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';

import { fadeInOut } from '../../services/configs/animations';
import { AccountService } from '../../services/store/account.service';
import { Permissions } from '../../models/configs/permission.model';
import { UserInfoComponent } from '../controls/user-info/user-info.component';
import { UserPreferencesComponent } from '../controls/user-preferences/user-preferences.component';
import { UsersManagementComponent } from '../controls/users-management/users-management.component';
import { RolesManagementComponent } from '../controls/roles-management/roles-management.component';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrl: './settings.component.scss',
    animations: [fadeInOut],
    imports: [
        RouterLink, TranslateModule, NgbNavModule,
        UserInfoComponent, UserPreferencesComponent, UsersManagementComponent, RolesManagementComponent
    ]
})
export class SettingsComponent implements OnInit, AfterViewInit, OnDestroy {
  private router = inject(Router);
  public route = inject(ActivatedRoute);
  private accountService = inject(AccountService);

  readonly profileTab = 'profile';
  readonly preferencesTab = 'preferences';
  readonly usersTab = 'users';
  readonly rolesTab = 'roles';
  activeTab = '';
  showDatatable = false; 
  fragmentSubscription: Subscription | undefined;

  ngOnInit() {
    this.fragmentSubscription = this.route.fragment.subscribe(fragment => this.setActiveTab(fragment));
  }

  ngAfterViewInit() {
    setTimeout(() => this.showDatatable = true);
  }

  ngOnDestroy() {
    this.fragmentSubscription?.unsubscribe();
  }

  setActiveTab(fragment: string | null) {
    fragment = fragment?.toLowerCase() ?? this.profileTab;

    const canViewTab = fragment === this.profileTab || fragment === this.preferencesTab ||
      (fragment === this.usersTab && this.canViewUsers) || (fragment === this.rolesTab && this.canViewRoles);

    if (canViewTab)
      this.activeTab = fragment;
    else
      this.router.navigate([], { fragment: this.profileTab });
  }

  get canViewUsers() {
    return this.accountService.userHasPermission(Permissions.viewUsers);
  }

  get canViewRoles() {
    return this.accountService.userHasPermission(Permissions.viewRoles);
  }
}
