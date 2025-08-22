import { Component, OnInit } from "@angular/core";
import {TranslocoService} from "@jsverse/transloco";
import { WidgetStatisticheClient, Widget_Statistics_IN } from "app/Service/net-core6-api.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";
import { finalize } from "rxjs";

@Component({
    standalone: false,
    selector: 'app-dati-generali-farmers-widget',
    templateUrl: './dati-generali-farmers-widget.component.html',
    styleUrls: ['./dati-generali-farmers-widget.component.scss']
  })
export class DatiGeneraliFarmersComponent implements OnInit {

  loading = true;
  data = [];

  resultData: string;

  constructor(
    private widgetsClient: WidgetStatisticheClient,
    private transloco: TranslocoService,
    private gestioneMultiAziendaService: GestioneMultiAziendaService
  ) { }


  ngOnInit(): void {

    this.loading = true;

    if (this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code != "") {

        const payload = {
          Year: this.gestioneMultiAziendaService.annoSelezionatoMultiAzienda.text,
          Country: this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code
        } as Widget_Statistics_IN;

        this.widgetsClient.widgetStatisticheGetGeneralStatistics(payload)
                        .pipe(finalize(() => this.loading = false))
                          .subscribe(res => {
                                              const dataRisposta = JSON.parse(res.RispostaStringa);
                                              dataRisposta.forEach((val) =>  {
                                                                              Object.entries(val).forEach(([key, value]) => {
                                                                                  const num: number = value as number;
                                                                                  this.data.push({'descrizione': this.transloco.translate(key), 'valore': num?.toLocaleString('it-IT', { useGrouping: true })});
                                                                              });
                                              });
                                          });

    } else {
            this.loading = false;
    }
  }

}
