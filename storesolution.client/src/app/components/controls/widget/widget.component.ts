import { Component, input } from "@angular/core";
import { fadeInOut } from "../../../services/animations";
import { Widget } from "../../../models/dashboard";
import { TranslateModule } from "@ngx-translate/core";
import { NgxDatatableModule } from "@siemens/ngx-datatable";
import { SearchBoxComponent } from "../search-box/search-box.component";

@Component({
  selector: 'app-widget',
  templateUrl: './widget.component.html',
  styleUrl: './widget.component.scss',
  standalone: true,
  animations: [fadeInOut],
  imports: [SearchBoxComponent, TranslateModule, NgxDatatableModule]
})

export class WidgetComponent {
  data = input.required<Widget>();
}
