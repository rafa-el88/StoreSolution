import { Injectable, inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { OrderEndpoint } from '../endpoints/order-endpoint.service';
import { Order } from '../../models/store/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private authService = inject(AuthService);
  private orderEndpoint = inject(OrderEndpoint);

  getAllOrders(page?: number, pageSize?: number) {
    return this.orderEndpoint.getOrdersEndpoint<Order[]>(page, pageSize);
  }
}
