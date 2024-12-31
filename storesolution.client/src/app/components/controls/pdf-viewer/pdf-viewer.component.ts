import { Component } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { fadeInOut } from '../../../services/configs/animations';

@Component({
  selector: 'app-pdf-viewer',
  templateUrl: './pdf-viewer.component.html',
  styleUrl: './pdf-viewer.component.scss',
  animations: [fadeInOut],
  standalone: true,
  imports: [TranslateModule]
})

export class PdfViewerComponent {
}
