import { Component, Input, OnInit } from "@angular/core";
import { WidgetStatisticheClient, Widget_Statistics_IN } from "app/Service/net-core6-api.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";
import { finalize } from "rxjs";
import { TranslocoService } from "@jsverse/transloco";

@Component({
  standalone: false,
  selector: 'app-aziende-multi-pie-widget',
  templateUrl: './app-aziende-multi-pie-widget.component.html',
  styleUrls: ['./app-aziende-multi-pie-widget.component.scss']
})
export class AziendeMultiPieWidgetComponent implements OnInit {

  @Input() codice: string | null = null;
  data = [];
  loading = true;
  chartPie = true;

  constructor(
    private widgetsClient: WidgetStatisticheClient,
    private gestioneMultiAziendaService: GestioneMultiAziendaService,
    private transloco: TranslocoService,
  ) { }

  ngOnInit(): void {

    this.loading = true;

    if (this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code != "") {

      const payload = {
        Year: this.gestioneMultiAziendaService.annoSelezionatoMultiAzienda.text,
        Country: this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code
      } as Widget_Statistics_IN;

      switch (this.codice) {
        //dati di prova, nei vari case ci andrà la chiamata a endpoint
        case 'ProduzioneColtureMultiAzienda':

          this.widgetsClient
            .widgetStatisticheGetCropMap(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
              const dataRisposta = JSON.parse(data.RispostaStringa);
              dataRisposta.forEach((val) => {
                this.data.unshift({ 'Superficie': val["Tot_Ha_xUsage"] + "Ha /n" + val["Perc"], 'Udm_Sim': null, 'Veg_Cod': 0, 'Veg_Des': val["Usage_Des"] });
              });
            });

          break;

        case 'AziendeMovimentateMulti':

          this.widgetsClient
            .widgetStatisticheGetMovedMappedFarmersPriorWeek(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
              const dataRisposta = JSON.parse(data.RispostaStringa);
              dataRisposta.forEach((val) => {
                this.data.unshift({ 'Superficie': val["PERC_MOVED"], 'Udm_Sim': null, 'Veg_Cod': 0, 'Veg_Des': this.transloco.translate('AziendeMovimentate') });
                this.data.unshift({ 'Superficie': val["PERC_NOT_MOVED"], 'Udm_Sim': null, 'Veg_Cod': 1, 'Veg_Des': this.transloco.translate('AziendeNonMovimentate') });
              });
            });

          break;

        case 'AziendeMovimentateMultiInizioCampagna':

          this.widgetsClient
            .widgetStatisticheGetMovedMappedFarmersCampaignBegin(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
              const dataRisposta = JSON.parse(data.RispostaStringa);
              dataRisposta.forEach((val) => {
                this.data.unshift({ 'Superficie': val["PERC_MOVED"], 'Udm_Sim': null, 'Veg_Cod': 0, 'Veg_Des': this.transloco.translate('AziendeMovimentate') });
                this.data.unshift({ 'Superficie': val["PERC_NOT_MOVED"], 'Udm_Sim': null, 'Veg_Cod': 1, 'Veg_Des': this.transloco.translate('AziendeNonMovimentate') });
              });
            });

          break;

        case 'AziendeMappateMulti':

          this.widgetsClient
            .widgetStatisticheGetMappedFarmers(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
              const dataRisposta = JSON.parse(data.RispostaStringa);
              dataRisposta.forEach((val) => {
                this.data.unshift({ 'Superficie': val["PERC_MAPPED"], 'Udm_Sim': null, 'Veg_Cod': 0, 'Veg_Des':  this.transloco.translate('AziendeMappate') });
                this.data.unshift({ 'Superficie': val["PERC_NOT_MAPPED"], 'Udm_Sim': null, 'Veg_Cod': 1, 'Veg_Des':  this.transloco.translate('AziendeNonMappate') });
              });
            });

          break;

      }

    } else {
      this.loading = false;
    }

  }

}
