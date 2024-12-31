import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../services/configs/animations';

@Component({
    selector: 'app-orders',
    templateUrl: './orders.component.html',
    styleUrl: './orders.component.scss',
    animations: [fadeInOut],
    imports: [TranslateModule]
})

export class OrdersComponent {
}
