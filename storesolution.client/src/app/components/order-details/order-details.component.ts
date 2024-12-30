import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../services/animations';

@Component({
    selector: 'app-order-details',
    templateUrl: './order-details.component.html',
    styleUrl: './order-details.component.scss',
    animations: [fadeInOut],
    imports: [TranslateModule]
})

export class OrderDetailsComponent {
}
