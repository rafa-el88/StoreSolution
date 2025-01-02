import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './base-endpoint.service';
import { ConfigurationService } from '../configs/configuration.service';

@Injectable({
  providedIn: 'root'
})
export class OrderEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  get ordersUrl() { return this.configurations.baseUrl + '/api/order'; }

  getOrdersEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
    const endpointUrl = page && pageSize ? `${this.ordersUrl}/${page}/${pageSize}` : this.ordersUrl;
    return this.http.get<T>(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getOrdersEndpoint<T>(page, pageSize));
      }));
  }
}
