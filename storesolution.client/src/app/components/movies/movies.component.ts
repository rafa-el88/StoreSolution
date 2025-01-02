import { Component, inject, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../services/configs/animations';
import { Utilities } from '../../services/helper/utilities';
import { Movie } from '../../models/store/movie.model';
import { AppTranslationService } from '../../services/configs/translation.service';
import { NgxDatatableModule, TableColumn } from '@siemens/ngx-datatable';
import { AlertService, MessageSeverity } from '../../services/configs/alert.service';
import { MovieService } from '../../services/store/movie.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ExportCsvService } from '../../services/helper/export-csv.service';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms'

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.scss',
  animations: [fadeInOut],
  imports: [TranslateModule, NgxDatatableModule, NgSelectModule, FormsModule]
})
export class MoviesComponent implements OnInit {

  private translationService = inject(AppTranslationService);
  private alertService = inject(AlertService);
  private movieService = inject(MovieService);
  private exportCsv = inject(ExportCsvService);

  rows: Movie[] = [];
  rowsCache: Movie[] = [];
  showDatatable = false;
  columns: TableColumn[] = [];
  loadingIndicator = false;
  selectedValue: number = 0;


  constructor() { }

  ngOnInit() {

    const gT = (key: string) => this.translationService.getTranslation(key);
    this.columns = [
      { prop: 'id', name: '#', width: 5 },
      { prop: 'title', name: gT('movies.management.Title'), width: 100 },
      { prop: 'pricePerDay', name: gT('movies.management.PricePerDay'), width: 25 },
      { prop: 'quantityCopies', name: gT('movies.management.QuantityCopies'), width: 25 },
      { prop: 'unitsInStock', name: gT('movies.management.UnitsInStock'), width: 25 }
    ];
    if (this.canManageMovies) {
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

  loadData() { }

  onSelectChange(event: any) {

    this.movieService.getMoviesByEvent(event)
      .subscribe({
        next: results => this.onDataLoadSuccessful(results),
        error: error => this.onDataLoadFailed(error)
      });
  }

  onDataLoadSuccessful(movie: Movie[]) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.rowsCache = [...movie];
    this.rows = movie;
    setTimeout(() => this.showDatatable = true);
  }

  onDataLoadFailed(error: HttpErrorResponse) {
    this.alertService.stopLoadingMessage();
    this.loadingIndicator = false;

    this.alertService.showStickyMessage('Erro de carregamento',
      `Não é possível recuperar filmes do servidor.\r\nErro: "${Utilities.getHttpResponseMessage(error)}"`,
      MessageSeverity.error, error);
  }

  ngAfterViewInit() {
    
  }

  onSearchChanged(value: string) {
    this.rows = this.rowsCache.filter(r =>
      Utilities.searchArray(value, false, r.title, r.description));
  }

  onClickExportCsv() {
    const timestamp = new Date().getTime();
    const randomNum = Math.floor(Math.random() * 1000000);
    var nameCsv = timestamp.toString() + randomNum.toString() + "_filmes";
    var headerCustom = ['ID', 'Titulo', 'Preço/dia', 'Qtd. Cópias', 'Qtd. Estoque'];
    this.exportCsv.exportToCsv(this.rows, headerCustom, nameCsv);
  }
  get canManageMovies() {
    return !!true;
  }
}
