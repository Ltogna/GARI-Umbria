import { Component, OnInit } from "@angular/core";
import { FunzioniComuniService } from "app/Service/FunzioniComuni.service";
import { WidgetStatisticheClient } from "app/Service/net-core6-api.service";
import { GestioneMultiAziendaService } from "app/widget-config/gestione-multiazienda.service";


@Component({
    standalone: false,
    selector: 'app-paese-multiazienda-widget',
    templateUrl: './paese-multiazienda-widget.component.html',
    styleUrls: ['./paese-multiazienda-widget.component.scss']
  })
export class PaeseMultiAziendaComponent implements OnInit {

    constructor(
        private widgetsClient: WidgetStatisticheClient,
        private funzioniComuni: FunzioniComuniService,
        private gestioneMultiAziendaService: GestioneMultiAziendaService
    ) { }

    loading = false;
    public areaList = [];
    public yearList = [];
    selectedCountry;
    selectedYear;

    ngOnInit(): void {

        //caricati nel widgetsComponent
        this.areaList = this.gestioneMultiAziendaService.countriesList;
        this.selectedCountry = this.gestioneMultiAziendaService.nazioneSelezionataMultiAzienda;

        this.yearList = this.gestioneMultiAziendaService.yearsList;
        this.selectedYear = this.gestioneMultiAziendaService.annoSelezionatoMultiAzienda;

    }

    onDDLValueChangeCountry(value: any) {
        this.gestioneMultiAziendaService.setNazioneMultiAzienda(value);
    }

    onDDLValueChangeYear(value: any) {
        this.gestioneMultiAziendaService.setAnnoMultiAzienda(value);
    }

}
