import { Component, inject, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../services/animations';
import { Utilities } from '../../services/utilities';
import { SearchBoxComponent } from '../controls/search-box/search-box.component';
import { Customer } from '../../models/customer.model';
import { AppTranslationService } from '../../services/app-translation.service';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';
import { AlertService, MessageSeverity } from '../../services/alert.service';
import { CustomerService } from '../../services/customer.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.scss',
  animations: [fadeInOut],
  imports: [SearchBoxComponent, TranslateModule, NgxDatatableModule]
})
export class CustomersComponent implements OnInit {
  private translationService = inject(AppTranslationService);
  private alertService = inject(AlertService);
  private customerService = inject(CustomerService);

  rows: Customer[] = [];
  rowsCache: Customer[] = [];
  showDatatable = false;
  columns: TableColumn[] = [];
  loadingIndicator = false;


  ngOnInit() {

    const gT = (key: string) => this.translationService.getTranslation(key);
    this.columns = [
      { prop: 'id', name: '#', width: 10 },
      { prop: 'name', name: gT('customers.management.Name'), width: 50 },
      { prop: 'email', name: gT('customers.management.Email'), width: 20 },
      { prop: 'phoneNumber', name: gT('customers.management.PhoneNumber'), width: 20 }
    ];
    if (this.canManageCustomers) {
      this.columns.push({
        name: '',
        width: 160,
        cellTemplate: false,
        resizeable: false,
        canAutoResize: false,
        sortable: false,
        draggable: false
      });
    }
    this.loadData();
  }

  loadData() {
    this.alertService.startLoadingMessage();
    this.loadingIndicator = true;
    this.customerService.getCustomers()
      .subscribe({
        next: results => this.onDataLoadSuccessful(results),
        error: error => this.onDataLoadFailed(error)
      });
  }

  onDataLoadSuccessful(customer: Customer[]) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.rowsCache = [...customer];
    this.rows = customer;
  }

  onDataLoadFailed(error: HttpErrorResponse) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.alertService.showStickyMessage('Erro de carregamento',
      `Não é possível recuperar clientes do servidor.\r\nErro: "${Utilities.getHttpResponseMessage(error)}"`,
      MessageSeverity.error, error);
  }

  ngAfterViewInit() {
    setTimeout(() => this.showDatatable = true);
  }

  onSearchChanged(value: string) {
    this.rows = this.rowsCache.filter(r =>
      Utilities.searchArray(value, false, r.name, r.email));
  }
  get canManageCustomers() {
    return !!true;
  }
}
