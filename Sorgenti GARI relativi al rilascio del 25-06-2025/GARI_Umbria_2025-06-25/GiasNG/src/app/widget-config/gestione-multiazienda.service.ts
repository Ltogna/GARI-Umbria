import { Injectable } from "@angular/core";
import { enum_Security_Attivita } from "app/Model/TipiEnumerativi";
import { WidgetStatisticheClient } from "app/Service/net-core6-api.service";
import { PermessiUtenteService } from "app/Service/permessi-utente.service";
import { BehaviorSubject } from "rxjs";

class Countries_IN {
    code: string;
    descr: string;
}

@Injectable({ providedIn: 'root' })
export class GestioneMultiAziendaService {

    //questo servizio è condiviso con i due componenti widget-config e widget
    widgetsMultiAzienda: boolean = false;

    nazioneMultiAziendaSubject = new BehaviorSubject<any>(null);
    nazioneSelezionataMultiAzienda = null;

    annoMultiAziendaSubject = new BehaviorSubject<any>(null);
    annoSelezionatoMultiAzienda = null;

    countriesList = [];
    yearsList = [];

    permesso_WidgetMultiAzienda: boolean;

    constructor(
                private widgetsClientMulti: WidgetStatisticheClient,
                private permessiUtenteService: PermessiUtenteService
    ) {
            this.permesso_WidgetMultiAzienda = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Widget_MultiAzienda, 2);
    }

    getPermessoWidgetMultiAzienda() {
        return this.permesso_WidgetMultiAzienda;
    }

    setNazioneMultiAzienda(value: any) {
        this.nazioneSelezionataMultiAzienda = value;
        this.nazioneMultiAziendaSubject.next(value);
    }

    setAnnoMultiAzienda(value: any) {
        //questa funzione la richiamo quando cambio l'anno. Quando cambio l'anno
        //ricarico anche i Paesi
        this.annoSelezionatoMultiAzienda = value;

        this.getCountriesListFromAPI(value);
    }


    public getCountriesListFromAPI(value?: any) {
        this.widgetsClientMulti.widgetStatisticheGetCountries(this.annoSelezionatoMultiAzienda["text"])
        .subscribe(data => {
                    const paesi = JSON.parse(data.RispostaStringa);
                    
                    //pulisco la countriesList
                    this.countriesList.splice(0, this.countriesList.length);
                    paesi.forEach((val) =>  {
                        this.countriesList.unshift(val as Countries_IN);
                    });

                    if (this.countriesList.length > 0) {
                        if (!this.nazioneSelezionataMultiAzienda || this.countriesList.findIndex(item => item.code === this.nazioneSelezionataMultiAzienda.code) == -1)   //se lo stesso Paese di prima è presente al cambio di anno, allora scelgo lo stesso
                            this.nazioneSelezionataMultiAzienda = this.countriesList[0];
                    } else 
                        this.nazioneSelezionataMultiAzienda = {'code': "", 'descr': ""};

                    if (value)
                        this.annoMultiAziendaSubject.next(value);
        });                    
    }

}