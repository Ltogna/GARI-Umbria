import { Component, Input, OnInit } from "@angular/core";
import { SeriesLabelsContentArgs } from "@progress/kendo-angular-charts";
import { IntlService } from "@progress/kendo-angular-intl";
import { WidgetStatisticheClient, Widget_Statistics_IN } from "app/Service/net-core6-api.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";
import { finalize } from "rxjs";

@Component({
    standalone: false,
    selector: 'app-colture-multiazienda-widget',
    templateUrl: './colture-multiazienda-widget.component.html',
    styleUrls: ['./colture-multiazienda-widget.component.scss']
  })
export class ColtureMultiAziendaWidget implements OnInit {

    @Input() codice: string | null = null;
    data = [];
    loading = true;
    chartPie = true;

    constructor(
        private widgetsClient: WidgetStatisticheClient,
        private gestioneMultiAziendaService: GestioneMultiAziendaService,
        private intl: IntlService
    ) {
        this.labelContent = this.labelContent.bind(this);
    }

    ngOnInit(): void {
        this.loading = true;

        if (this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code != "") {

            const payload = {
            Year: this.gestioneMultiAziendaService.annoSelezionatoMultiAzienda.text,
            Country: this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda.code
            } as Widget_Statistics_IN;

            this.widgetsClient
            .widgetStatisticheGetCropMap(payload)
            .pipe(finalize(() => this.loading = false))
            .subscribe(data => {
                    const dataRisposta = JSON.parse(data.RispostaStringa);
                    dataRisposta.forEach((val) =>  {
                        if (val["Tot_Ha_xUsage"] != -1)
                            this.data.unshift({'category': val["Usage_Des"], 'totHaUsage': val["Tot_Ha_xUsage"], 'perc': val["Perc"], 'value': val["Perc"]});
                    });
            });

        } else {
                this.loading = false;
        }
    }

    public labelContent(args: SeriesLabelsContentArgs): string {
        return `${this.intl.formatNumber(args.dataItem.totHaUsage, "n1")} Ha ${this.intl.formatNumber(args.dataItem.value, "n1")} %`;
    }

}
