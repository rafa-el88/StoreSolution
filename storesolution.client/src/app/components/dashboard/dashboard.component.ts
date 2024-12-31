import { Component } from "@angular/core";
import { fadeInOut } from "../../services/animations";
import { WidgetComponent } from "../controls/widget/widget.component";
import { SubscribersComponent } from "./widgets/subscribers.component";
import { Widget } from "../../models/dashboard";
import { TranslateModule } from "@ngx-translate/core";
import { NgxDatatableModule } from "@siemens/ngx-datatable";

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  standalone: true,
  animations: [fadeInOut],
  imports: [TranslateModule, NgxDatatableModule, WidgetComponent]
})

export default class DashboardComponent {
  data: Widget = {
    id: 1,
    label: 'Subscribers',
    content: SubscribersComponent,
  };
}
