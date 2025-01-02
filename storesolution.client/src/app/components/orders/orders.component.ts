import { Component, inject, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../services/configs/animations';
import { Utilities } from '../../services/helper/utilities';
import { AppTranslationService } from '../../services/configs/translation.service';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';
import { AlertService, MessageSeverity } from '../../services/configs/alert.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ExportCsvService } from '../../services/helper/export-csv.service';
import { OrderService } from '../../services/store/order.service';
import { Order } from '../../models/store/order.model';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss',
  animations: [fadeInOut],
  imports: [TranslateModule, NgxDatatableModule]
})
export class OrdersComponent implements OnInit {

  private translationService = inject(AppTranslationService);
  private alertService = inject(AlertService);
  private orderService = inject(OrderService);
  private exportCsv = inject(ExportCsvService);

  rows: Order[] = [];
  rowsCache: Order[] = [];
  showDatatable = false;
  columns: TableColumn[] = [];
  loadingIndicator = false;
  constructor() { }

  ngOnInit() {

    const gT = (key: string) => this.translationService.getTranslation(key);
    this.columns = [
      { prop: 'id', name: '#', width: 5 },
      { prop: 'dateStartRental', name: gT('orders.management.DateStartRental'), width: 100 },
      { prop: 'dateEndRental', name: gT('orders.management.DateEndRental'), width: 100 },
      { prop: 'dateDevolution', name: gT('orders.management.DateDevolution'), width: 100 },
      { prop: 'returnedMovie', name: gT('orders.management.ReturnedMovie'), width: 30 }

    ];
    if (this.canManageOrders) {
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
    this.orderService.getAllOrders()
      .subscribe({
        next: results => this.onDataLoadSuccessful(results),
        error: error => this.onDataLoadFailed(error)
      });
  }

  onDataLoadSuccessful(order: Order[]) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.rowsCache = [...order];
    this.rows = order;
  }

  onDataLoadFailed(error: HttpErrorResponse) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.alertService.showStickyMessage('Erro de carregamento',
      `Não é possível recuperar pedidos do servidor.\r\nErro: "${Utilities.getHttpResponseMessage(error)}"`,
      MessageSeverity.error, error);
  }

  ngAfterViewInit() {
    setTimeout(() => this.showDatatable = true);
  }

  onClickExportCsv() {
    const timestamp = new Date().getTime();
    const randomNum = Math.floor(Math.random() * 1000000);
    var nameCsv = timestamp.toString() + randomNum.toString() + "_pedidos";
    var headerCustom = ['ID', 'Data Inicio', 'Data fim', 'Data Devolução', 'Devolvido'];
    this.exportCsv.exportToCsv(this.rows, headerCustom, nameCsv);
  }

  get canManageOrders() {
    return !!true;
  }
}
