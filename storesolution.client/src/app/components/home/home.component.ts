import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../services/animations';
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  animations: [fadeInOut],
  imports: [ TranslateModule ]
})
export class HomeComponent { }

