import { Component } from "@angular/core";
import { TranslateModule } from "@ngx-translate/core";
import { NgxDatatableModule } from "@siemens/ngx-datatable";
import { fadeInOut } from "../../../services/animations";

@Component({
  selector: 'app-subscribers',
  templateUrl: './subscribers.component.html',
  styleUrl: './subscribers.component.scss',
  standalone: true,
  animations: [fadeInOut],
  imports: [TranslateModule, NgxDatatableModule]
})

export class SubscribersComponent {
}
