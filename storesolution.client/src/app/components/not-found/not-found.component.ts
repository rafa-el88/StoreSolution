import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/configs/animations';

@Component({
    selector: 'app-not-found',
    templateUrl: './not-found.component.html',
    styleUrl: './not-found.component.scss',
    animations: [fadeInOut],
    imports: [RouterLink, TranslateModule]
})
export class NotFoundComponent {
}
