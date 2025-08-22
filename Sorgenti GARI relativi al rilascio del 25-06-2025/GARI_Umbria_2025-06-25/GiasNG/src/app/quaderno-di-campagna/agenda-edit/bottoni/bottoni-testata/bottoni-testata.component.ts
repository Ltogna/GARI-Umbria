/* eslint-disable */
import {AfterViewInit, Component, OnInit} from '@angular/core';
import {Attivita} from 'app/Model/attivita/Attivita';
import { Lavorazione } from 'app/Model/attivita/Lavorazione';
import { Enum_SiteRedirector, enum_PagineAgenda_2010 } from 'app/Model/siti.enum';
import {enum_LAVCOD} from 'app/Model/TipiEnumerativi';
import { GestioneRichiesteService, ParametriAggiuntivi_QueryString } from 'app/Service/gestione-richieste.service';
import {  ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { QdCFormToAttivitaService } from '../../service/quaderno-di-campagna-form/quaderno-di-campagna-form-to-attivita.service';
import {QdCVisibilitaControlliTestataService} from "../../service/testata/visibilita-controlli-testata-service";
import {QdCService} from "../../service/qdc.service";
import {DpiBio, FORMULATI, NessunDpi, NessunDpiNessunaEtichetta} from "../../../../Model/CostantiPersonalizzate";
import {QdCControlliSalvataggioService} from "../../service/qdc-controlli-salvataggio.service";
import {
    Key_Parametri_Aggiuntivi, Obj_Errore_Gias_QdC,
    Parametri_Aggiuntivi_Attivita,
    Sezione_Prodotto_Fertilizzanti,
    Sezione_Prodotto_Formulati, Sezione_Prodotto_Raccolta, Sezione_Prodotto_Sementi
} from "../../quaderno-di-campagna-form/quaderno-di-campagna-form.model";
import {enum_ErroreGias_Tipo, ErroreGias, ErroreGias_Severity} from "../../../../Service/master.service";
import {TranslocoService} from "@jsverse/transloco";
import {cloneDeep} from "lodash";
import { ObjParametriAgenda } from 'gias-ui-kit';

@Component({
    standalone: false,
    selector: 'btn-bottoni-testata',
    templateUrl: './bottoni-testata.component.html',
    styleUrls: ['./bottoni-testata.component.scss']
})
export class BottoniTestataComponent implements OnInit, AfterViewInit {

    objParametriAgenda: ObjParametriAgenda;

    constructor(
        private objParametriAgendaService: ObjParametriAgendaService,
        public qdcservice: QdCService,
        private qdCFormToAttivitaService: QdCFormToAttivitaService,
        private gestionerichiesteservice: GestioneRichiesteService,
        public qdcvisibilitacontrollitestata: QdCVisibilitaControlliTestataService,
        private qdccontrollisalvataggioservice: QdCControlliSalvataggioService,
        private translocoService: TranslocoService
    ) {  }

    ngOnInit(): void {
        this.objParametriAgenda=this.objParametriAgendaService.getObjParamValue();
    }

    ngAfterViewInit() {
        this.Msg_Verifica_Conformita_Intervento_Ribaltamento();
    }

    redirectTo_Verifica_DoseConsigliataBS() {

        //Mostrato solo per il diserbo e passato solo il diserbo

        if(this.qdcservice.Sezioni_ProdottoFormArray && this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().length > 0) {

            let listErroriGias: ErroreGias[] = [];

            //Deve essere scelto un disciplinare diverso da bio,NessunDpi e NessunDPINessunaEtichetta
            let disciplinare = this.qdcservice.getDisciplinareModelValue(enum_LAVCOD.DISERBO);

            if(!disciplinare || disciplinare.codice === NessunDpi ||  disciplinare.codice === NessunDpiNessunaEtichetta || disciplinare.codice === DpiBio){

                let DisciplinariDesc = this.qdcservice.Obj_DpiBIO.descrizione +" , "+this.qdcservice.Obj_NessunDpi.descrizione + " , "+this.qdcservice.Obj_NessunDpiNessunaEtichetta.descrizione;

                listErroriGias.push(<ErroreGias>{
                    severity: ErroreGias_Severity.Bloccante,
                    tipo:  enum_ErroreGias_Tipo.Generico,
                    messaggio: this.translocoService.translate("qdc.ScegliereUnDisciplinareDiversoDa",{DisciplinariDesc})
                });

                if (listErroriGias && listErroriGias.length > 0) {
                    this.qdcservice.gestisci_ErroriGias(listErroriGias, true, true).then();

                    return;
                }
            }

            this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().forEach((s: Sezione_Prodotto_Formulati | Sezione_Prodotto_Fertilizzanti | Sezione_Prodotto_Sementi | Sezione_Prodotto_Raccolta) => {
                let lav_cod = + s.Operazione.primaryKey.codice;

                if(lav_cod === enum_LAVCOD.DISERBO){
                    this.qdccontrollisalvataggioservice.Controlla_Grid_Dosi_Prodotti(s.Categoria_Magazzino,lav_cod, s.DosiProdotti, listErroriGias, false);
                }
            });

            if (listErroriGias && listErroriGias.length > 0) {
                this.qdcservice.gestisci_ErroriGias(listErroriGias, true, true).then();

                return;
            }

            this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
            const newobjParametriAgenda: ObjParametriAgenda = cloneDeep(this.objParametriAgenda);

            newobjParametriAgenda.Id_Agenda = 0;
            newobjParametriAgenda.Pagina_Provenienza = this.qdcservice.masterService.getCurrentPageAsValue();;
            newobjParametriAgenda.Sito_Provenienza = Enum_SiteRedirector.GiasNG;

            const attivita: Attivita = cloneDeep(this.qdCFormToAttivitaService.mapQdCFormToListaAttivita(null,[]).find(
                                                    a=>+a.job.primaryKey.codice === enum_LAVCOD.DISERBO));

            newobjParametriAgenda.GenericObj_string = JSON.stringify(attivita);

            const parametriAggiuntivi: Array<ParametriAggiuntivi_QueryString> = [];

            this.gestionerichiesteservice.gestionePassaggioAltroSito_Aperto_in_Iframe(
                Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                enum_PagineAgenda_2010.Pagina_Verifica_DoseConsigliataBS,
                parametriAggiuntivi,
                newobjParametriAgenda,
                false,
                -1,
                'qdc.DettaglioDoseConsigliataDisciplinare.Text').then();

        }
    }
    redirectTo_Verifica_Sostenibilita() {

        if(this.qdcservice.Sezioni_ProdottoFormArray && this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().length > 0){

            let listErroriGias: ErroreGias[] = [];

            this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().forEach((s: Sezione_Prodotto_Formulati | Sezione_Prodotto_Fertilizzanti | Sezione_Prodotto_Sementi | Sezione_Prodotto_Raccolta) => {
                let lav_cod = + s.Operazione.primaryKey.codice;

                this.qdccontrollisalvataggioservice.Controlla_Grid_Dosi_Prodotti(s.Categoria_Magazzino,lav_cod, s.DosiProdotti, listErroriGias, false);
            });

            if(listErroriGias && listErroriGias.length > 0){
                this.qdcservice.gestisci_ErroriGias(listErroriGias,true,true).then();

                return;
            }

            this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
            let newobjParametriAgenda: ObjParametriAgenda = cloneDeep(this.objParametriAgenda);

            newobjParametriAgenda.Id_Agenda = 0;
            newobjParametriAgenda.Pagina_Provenienza = this.qdcservice.masterService.getCurrentPageAsValue();
            newobjParametriAgenda.Sito_Provenienza = Enum_SiteRedirector.GiasNG;

            let lista_Attivita_Ordinate: Attivita[] = [];

            const lista_Attivita: Attivita[] = cloneDeep(this.qdCFormToAttivitaService.mapQdCFormToListaAttivita(null,[]));

            //Per il lato server prima ordino la lista per i FORMULATI poi le altre attivita

            let Attivita_Formulati = lista_Attivita.filter(a=>this.qdcservice.getCategoria_Magazzino(+ a.job.primaryKey.codice) === FORMULATI);

            if(Attivita_Formulati && Attivita_Formulati.length > 0){
                Attivita_Formulati.forEach(a=>{
                    lista_Attivita_Ordinate.push(a);
                })
            }

            let Attivita_Non_Formulati = lista_Attivita.filter(a=>this.qdcservice.getCategoria_Magazzino(+ a.job.primaryKey.codice) !== FORMULATI);

            if(Attivita_Non_Formulati && Attivita_Non_Formulati.length > 0){
                Attivita_Non_Formulati.forEach(a=>{
                    lista_Attivita_Ordinate.push(a);
                })
            }

            newobjParametriAgenda.GenericObj_string = JSON.stringify(lista_Attivita_Ordinate);

            let ParametriAggiuntivi: Array<ParametriAggiuntivi_QueryString> = [];

            this.gestionerichiesteservice.gestionePassaggioAltroSito_Aperto_in_Iframe(
                Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                enum_PagineAgenda_2010.Pagina_reportSostenibilita,
                ParametriAggiuntivi,
                newobjParametriAgenda,
                false,
                -1,
                'qdc.DoseConsigliata').then();

        }


    }

    redirectTo_Verifica_Conformita() {

        if(this.qdcservice.Sezioni_ProdottoFormArray && this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().length > 0){

            let listErroriGias: ErroreGias[] = [];

            //Se ho una multiOperazione per alcuni lav_cod non devo fare il VerificaConformità(esempio MultiOperazione Semina + Distribuzione concime, devo escludere la Semina tra le Operazioni da passare al
            // VerificaConformita)

            let list_lav_cod_da_non_mappare: number[] = [];

            this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().forEach((s: Sezione_Prodotto_Formulati | Sezione_Prodotto_Fertilizzanti | Sezione_Prodotto_Sementi | Sezione_Prodotto_Raccolta) => {

                let lav_cod = + s.Operazione.primaryKey.codice;

                if(this.qdcvisibilitacontrollitestata.MostraBtn_VerificaConformita([s.Operazione])){
                  this.qdccontrollisalvataggioservice.Controlla_Grid_Dosi_Prodotti(s.Categoria_Magazzino,lav_cod, s.DosiProdotti, listErroriGias, false);
                }else{
                  list_lav_cod_da_non_mappare.push(lav_cod);
                }
            });

            if(listErroriGias && listErroriGias.length > 0){
                this.qdcservice.gestisci_ErroriGias(listErroriGias,true,true).then();

                return;
            }

            this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
            let newobjParametriAgenda: ObjParametriAgenda = cloneDeep(this.objParametriAgenda);

            newobjParametriAgenda.Id_Agenda = 0;
            newobjParametriAgenda.Pagina_Provenienza = this.qdcservice.masterService.getCurrentPageAsValue();
            newobjParametriAgenda.Sito_Provenienza = Enum_SiteRedirector.GiasNG;

            let listParametri_Aggiuntivi_Attivita: Parametri_Aggiuntivi_Attivita[] = [];

            let list_attivita: Attivita[] = cloneDeep(this.qdCFormToAttivitaService.mapQdCFormToListaAttivita(null,listParametri_Aggiuntivi_Attivita,false,[],list_lav_cod_da_non_mappare));

            newobjParametriAgenda.GenericObj_string = JSON.stringify(list_attivita);

            //Se sono in modifica passo anche la lista_Codici_Attivita_x_CentriAziendali
            //perchè il Verifica Conformita ha bisogno di tutti gli id_agenda

            let ParametriAggiuntivi: Array<ParametriAggiuntivi_QueryString> = [];

            let Parametri_Aggiuntivi_MultiCentro = listParametri_Aggiuntivi_Attivita.find(p=>p.key === Key_Parametri_Aggiuntivi.lista_Codici_Attivita_x_CentriAziendali);

            if(Parametri_Aggiuntivi_MultiCentro){
                ParametriAggiuntivi.push({
                    key: Parametri_Aggiuntivi_MultiCentro.key,
                    value: Parametri_Aggiuntivi_MultiCentro.value,
                    codifica: false
                });
            }

            this.gestionerichiesteservice.gestionePassaggioAltroSito_Aperto_in_Iframe(
                Enum_SiteRedirector.Sito_AgronicaAgenda_2010,
                enum_PagineAgenda_2010.Pagina_Verifica_Conformita,
                ParametriAggiuntivi,
                newobjParametriAgenda,
                false,
                -1,
                'qdc.ConformitaOperazione').then();
        }


    }


    /*
        @description
        Warning Sì/No che viene mostrato quando si sta ribaltando una Ricetta/Brogliaccio creta da APP in Agenda tranne per i casi
        in cui metto in edit i prodotti
     */
    Msg_Verifica_Conformita_Intervento_Ribaltamento(){

        let Operazioni: Array<Lavorazione> = this.qdcservice.TestataForm.get("Operazioni").getRawValue();

        if(this.qdcservice.Is_Ribaltamento_Ricetta_Da_Origine_Diversa() &&
          this.qdcvisibilitacontrollitestata.MostraBtn_VerificaConformita(Operazioni) &&
          !this.qdcservice.CheckDosiProdottiNonSalvate()){

            let listerroriGias: ErroreGias[] = [];

            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Warning,
                tipo: enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('qdc.VuoiVerificareConfIntervento')
            });

            this.qdcservice.gestisci_ErroriGias(listerroriGias,false,false,"").then((obj:Obj_Errore_Gias_QdC)=>{
                if(obj.result.returnObj){
                    this.redirectTo_Verifica_Conformita();
                }
            });
        }

    }

}
