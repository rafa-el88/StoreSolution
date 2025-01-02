import { Injectable, inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { MovieEndpoint } from '../endpoints/movie-endpoint.service';
import { Movie } from '../../models/store/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  private authService = inject(AuthService);
  private movieEndpoint = inject(MovieEndpoint);

  getMovies(page?: number, pageSize?: number) {
    return this.movieEndpoint.getMoviesEndpoint<Movie[]>(page, pageSize);
  }

  getMoviesByEvent(eventId: number, page?: number, pageSize?: number) {
    return this.movieEndpoint.getMoviesByEventIdEndpoint<Movie[]>(eventId, page, pageSize);
  }
}
