import { Component, Input, OnInit } from "@angular/core";
import { WidgetStatisticheClient, Widget_Statistics_IN } from "app/Service/net-core6-api.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";
import { finalize, take } from "rxjs";

@Component({
    standalone: false,
    selector: 'app-columns-chart-widget',
    templateUrl: './columns-chart-widget.component.html',
    styleUrls: ['./columns-chart-widget.component.scss']
  })
export class ColumnsChartWidgetComponent implements OnInit {

    loading = true;
    data = [];
    colors = ["#4472C4", "#ED7D31", "#A5A5A5"];
    names = [];

    constructor(
      private widgetsClient: WidgetStatisticheClient,
      private gestioneMultiAziendaService: GestioneMultiAziendaService
    ) { }

    ngOnInit(): void {

        this.loading = true;

        if (this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code != "") {

            const payload = {
              Year: this.gestioneMultiAziendaService.annoSelezionatoMultiAzienda.text,
              Country: this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code
            } as Widget_Statistics_IN;

            //il formato data che ci arriva da API {CountFarmersxRegionxRange: 1462, AdminArea: 'Kilifi', RangeHACode: 1}
            this.widgetsClient
            .widgetStatisticheGetFarmerxRegionxRange(payload)
            .pipe(finalize(() => this.loading = false), take(1))
            .subscribe(data => {
                                const dataRisposta = JSON.parse(data.RispostaStringa);
                                const matrix = this.arrayToMatrix(dataRisposta, 8)
                                matrix.forEach(val => {
                                  this.data.push(this.getDataForChart(val));
                                  this.names.push(val[0].AdminArea);
                                });

            });

        } else {
            this.loading = false;
        }

    }

    private arrayToMatrix(array: any[], size: number): any[][] {
        const matrix = [];
        for (let i = 0; i < array.length; i += size) {
            matrix.push(array.slice(i, i + size));
        }
        return matrix;
    }

    getDataForChart(array: any[]): number[] {
      const values: number[] = [];

      array.forEach(item => {
        values.push(item.CountFarmersxRegionxRange);
      });

      return values;
    }

}
