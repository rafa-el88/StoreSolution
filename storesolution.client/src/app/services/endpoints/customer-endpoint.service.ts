import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './base-endpoint.service';
import { ConfigurationService } from '../configs/configuration.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  get customersUrl() { return this.configurations.baseUrl + '/api/customer'; }

  getCustomerEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
    const endpointUrl = page && pageSize ? `${this.customersUrl}/${page}/${pageSize}` : this.customersUrl;
    return this.http.get<T>(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getCustomerEndpoint<T>(page, pageSize));
      }));
  }
}
