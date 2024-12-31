import { Injectable, inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { CustomerEndpoint } from '../endpoints/customer-endpoint.service';
import { Customer } from '../../models/store/customer.model';

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
