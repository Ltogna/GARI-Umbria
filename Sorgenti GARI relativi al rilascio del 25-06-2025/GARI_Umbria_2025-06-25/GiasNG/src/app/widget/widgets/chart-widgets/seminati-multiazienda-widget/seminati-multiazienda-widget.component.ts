import { Component, Input, OnInit } from "@angular/core";
import { AxisLabelContentArgs } from "@progress/kendo-angular-charts";
import { WidgetStatisticheClient, Widget_Statistics_IN } from "app/Service/net-core6-api.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";
import { finalize } from "rxjs";
import {TranslocoService} from "@jsverse/transloco";

@Component({
    standalone: false,
    selector: 'app-seminati-multiazienda-widget',
    templateUrl: './seminati-multiazienda-widget.component.html',
    styleUrls: ['./seminati-multiazienda-widget.component.scss']
  })
export class SeminatiMultiAziendaWidgetComponent implements OnInit {

  @Input() codice: string | null = null;
  unit = "";
  chartPie = false;
  loading = true;
  data = [];

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

        switch(this.codice) {

          case 'AziendeSeminateMulti':

            this.widgetsClient
            .widgetStatisticheGetFarmersHarvestSowingData(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
                    const dataRisposta = JSON.parse(data.RispostaStringa);
                    i = 0;
                    dataRisposta.forEach((val) =>  {
                      this.data.push({'Superficie': val["TOTAL_FARMERS"], 'Udm_Sim': null, 'Veg_Cod': i, 'Veg_Des': this.transloco.translate("AziendeAbbrMappate") });
                      i++;
                      this.data.push({'Superficie': val["FARMERS_SOWINGCOUNT"], 'Udm_Sim': null, 'Veg_Cod': i, 'Veg_Des': this.transloco.translate("AziendeAbbrSeminate")});
                      i++;
                      this.data.push({'Superficie': val["FARMERS_HARVESTCOUNT"], 'Udm_Sim': null, 'Veg_Cod': i, 'Veg_Des':this.transloco.translate("AziendeAbbrRaccolte") });
                    });
                  });

          break;

          case 'SeminatiRaccoltiMulti':

            this.widgetsClient
            .widgetStatisticheGetPlotsHarvestSowingData(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
                    const dataRisposta = JSON.parse(data.RispostaStringa);
                    this.unit = "Ha";
                    i = 0;
                    dataRisposta.forEach((val) =>  {
                      this.data.push({'Superficie': val["TOTAL_HA"], 'Udm_Sim': null, 'Veg_Cod': i, 'Veg_Des':  this.transloco.translate("HaTotali")});
                      i++;
                      this.data.push({'Superficie': val["HA_SOWING"], 'Udm_Sim': null, 'Veg_Cod': i, 'Veg_Des':  this.transloco.translate("HaSeminati")});
                      i++;
                      this.data.push({'Superficie': val["HA_HARVES"], 'Udm_Sim': null, 'Veg_Cod': i, 'Veg_Des':  this.transloco.translate("HaRaccolti")});
                    });
                  });

          break;

        }

      } else {
        this.loading = false;
      }

  }

  labelContent = (e: AxisLabelContentArgs): string => {
    return `${e.value} ${e.dataItem.Udm_Sim ?? this.unit}`;
  };

}
