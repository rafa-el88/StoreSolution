import { Injectable, inject } from '@angular/core';
import { AuthService } from './auth.service';
import { MovieEndpoint } from './movie-endpoint.service';
import { Movie } from '../models/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private authService = inject(AuthService);
  private movieEndpoint = inject(MovieEndpoint);

  getMovies(page?: number, pageSize?: number) {
    return this.movieEndpoint.getMoviesEndpoint<Movie[]>(page, pageSize);
  }
}
