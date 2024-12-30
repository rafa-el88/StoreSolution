import { Injectable, inject } from '@angular/core';
import { AuthService } from './auth.service';
import { CustomerEndpoint } from './customer-endpoint.service';
import { Customer } from '../models/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private authService = inject(AuthService);
  private customerEndpoint = inject(CustomerEndpoint);

  getCustomers(page?: number, pageSize?: number) {
    return this.customerEndpoint.getCustomerEndpoint<Customer[]>(page, pageSize);
  }
}
