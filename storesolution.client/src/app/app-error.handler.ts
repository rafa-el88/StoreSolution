import { Injectable, ErrorHandler } from '@angular/core';

@Injectable()
export class AppErrorHandler extends ErrorHandler {
  constructor() {
    super();
  }

  override handleError(error: Error) {
    if (confirm("Erro fatal!\nOcorreu um erro não resolvido. Deseja recarregar a página para corrigir isso?\n\n" +
      `Erro: ${error.message}`)) {
      window.location.reload();
    }

    super.handleError(error);
  }
}
