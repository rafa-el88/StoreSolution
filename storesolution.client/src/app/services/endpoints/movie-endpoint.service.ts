import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './base-endpoint.service';
import { ConfigurationService } from '../configs/configuration.service';

@Injectable({
  providedIn: 'root'
})
export class MovieEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  get moviesUrl() { return this.configurations.baseUrl + '/api/movie'; }
  get moviesByEventUrl() { return this.configurations.baseUrl + '/api/movie/eventId'; }

  getMoviesEndpoint<T>(page?: number, pageSize?: number): Observable<T> {
    const endpointUrl = page && pageSize ? `${this.moviesUrl}/${page}/${pageSize}` : this.moviesUrl;
    console.log(endpointUrl);
    return this.http.get<T>(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getMoviesEndpoint<T>(page, pageSize));
      }));
  }

  getMoviesByEventIdEndpoint<T>(eventId: number, page?: number, pageSize?: number): Observable<T> {
    //const endpointUrl = eventId ? `${this.moviesByEventUrl}/${eventId}` : this.moviesByEventUrl;
    const endpointUrl = page && pageSize ? `${this.moviesByEventUrl}/${eventId}/${page}/${pageSize}` : `${this.moviesByEventUrl}/${eventId}`;
    console.log(endpointUrl);
    return this.http.get<T>(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getMoviesByEventIdEndpoint<T>(eventId, page, pageSize));
      }));
  }
}
