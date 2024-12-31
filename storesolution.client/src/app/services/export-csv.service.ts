import { Injectable } from '@angular/core';
import { Papa } from 'ngx-papaparse';

@Injectable({
  providedIn: 'root',
})
export class ExportCsvService {
  constructor(private papa: Papa) { }

  exportToCsv(obj: any[], header: string[], fileName: string): void {

    const headerCustom = this.papa.unparse({fields: header,data:[]}, { header: true });
    const csv = this.papa.unparse(obj, { header: false, delimiter: ';' })
    const csvCustom = csv.replaceAll(',', ' ').replaceAll(';', ',').replaceAll('"','');

    // create BOM UTF-8
    var buffer = new ArrayBuffer(3);
    var dataView = new DataView(buffer);
    dataView.setUint8(0, 0xfe);
    dataView.setUint8(1, 0xbb);
    dataView.setUint8(2, 0xbf);
    var read = new Uint8Array(buffer);

    this.downloadCsv(headerCustom + csvCustom, fileName, read);
  }

  private downloadCsv(csv: string, fileName: string, read: Uint8Array): void {

    const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        
    const link = document.createElement('a');
    if (link.download !== undefined) {
      const url = URL.createObjectURL(blob);
      link.setAttribute('href', url);
      link.setAttribute('download', fileName);
      link.style.visibility = 'hidden';
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }
}
