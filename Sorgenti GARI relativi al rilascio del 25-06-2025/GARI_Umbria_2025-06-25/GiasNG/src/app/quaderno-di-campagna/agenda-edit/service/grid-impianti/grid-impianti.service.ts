/* eslint-disable */
import { Injectable } from '@angular/core';
import { TranslocoService } from '@jsverse/transloco';
import { CellCloseEvent } from '@progress/kendo-angular-grid';
import { TooltipDirective } from '@progress/kendo-angular-tooltip';
import { FunzioniComuniService } from 'app/Service/FunzioniComuni.service';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { BehaviorSubject } from 'rxjs';
import { CalcoloSuperficiService } from '../calcolo-superfici.service';
import { QdCService } from '../qdc.service';
import { QdCTestataService } from '../testata/testata.service';
import {Lavorazione} from "../../../../Model/attivita/Lavorazione";
import {GridImpiantoSelezionatoModel} from "../../quaderno-di-campagna-form/quaderno-di-campagna-form.model";
import {MisceleService} from "../miscele.service";


@Injectable()

export class GridImpiantiService {

    public tooltip: TooltipDirective;

    // Observable per cambiare la proprietà AutoCorrect della numeric textbox
    private AutoCorrectNumericSource = new BehaviorSubject<boolean>(true);
    // Observable per cambiare la proprietà AutoCorrect della numeric textbox
    AutoCorrectNumeric = this.AutoCorrectNumericSource.asObservable();

    constructor(private qdcservice: QdCService,
                private funzionicomuniservice: FunzioniComuniService,
                private calcoloSuperfici: CalcoloSuperficiService,
                private ObjParametriAgendaService: ObjParametriAgendaService,
                private translocoService: TranslocoService,
                public testataservice: QdCTestataService,
                private misceleservice: MisceleService) {
    }
    settooltip(tooltip: TooltipDirective){
        this.tooltip=tooltip;
    }

    // Ripartizione della superficie trattata
    RipartizionaSupTrattata(grid) {

        const SupSel = this.qdcservice.SuperficiForm.get('Sup_Selezionata').value;
        const SupTratt = this.qdcservice.SuperficiForm.get('Sup_Trattata').value ?? 0;

        let attr = this.funzionicomuniservice.roundNumber(SupTratt / SupSel,this.qdcservice.Obj_Default_Numeric_Settings.decimals);

        const rows = grid.data.rows;
        const filteredData = this.kGetElementiSelezionati(rows);
        const nSel = filteredData.length;
        let somma: any = 0;
        const that=this;

        $.each(filteredData, function (idx, dataItem) {
            let daInserire;
            const app = parseFloat(dataItem.Sup_Imp);
            if (nSel - 1 !== idx) {
                daInserire = that.funzionicomuniservice.roundNumber((app * attr), that.qdcservice.Obj_Default_Numeric_Settings.decimals);
                somma += daInserire;
            } else {
                daInserire = (SupTratt.toFixed(that.qdcservice.Obj_Default_Numeric_Settings.decimals) - somma.toFixed(that.qdcservice.Obj_Default_Numeric_Settings.decimals)).toFixed(that.qdcservice.Obj_Default_Numeric_Settings.decimals);
            }

            dataItem.Sup_Imp_help =  that.funzionicomuniservice.roundNumber(+ daInserire, that.qdcservice.Obj_Default_Numeric_Settings.decimals);

        });


        return grid;

    }

    kGetElementiSelezionati(rows): Array<any>{

        if(!rows || rows.length == 0){
            return [];
        }

        return rows.filter(r=>r.Selected !== undefined && r.Selected !== null && r.Selected === true);
    }

    kCiSonoImpiantiSelezionati(rows): boolean{
        return this.kGetElementiSelezionati(rows).length > 0;
    }

    // Funzione per l'aggiornamento della superficie Totale
    RicalcolaSuperficieTotale(rows){
        let SupTot = 0;
        // se la checkbox è chekkata

        const filteredData = this.kGetElementiSelezionati(rows);

        $.each(filteredData, function (idx, dataItem) {

            SupTot += parseFloat(dataItem.Sup_Imp);
        });

        this.qdcservice.SuperficiForm.patchValue({
            Sup_Selezionata: this.funzionicomuniservice.roundNumber(SupTot,this.qdcservice.Obj_Default_Numeric_Settings.decimals)
        });

        this.qdcservice.SuperficiForm.markAsTouched();

    }

    //@description
    //Aggiornamento della superficie Coinvolta
    // Se il flag_calcolo_miscele è false non riscattano tutti i ricalcoli dovuti al cambio della Superficie Trattata
    RicalcolaSuperficieCoinvolta(rows:any,flag_calcolo_miscele: boolean = true) {
        let SupTot = 0;
        // sel la checkbox è chekkata

        const filteredData = this.kGetElementiSelezionati(rows);

        $.each(filteredData, function (idx, dataItem) {
            SupTot += parseFloat(dataItem.Sup_Imp_help);
        });

        this.AutoCorrectNumericSource.next(false);

        this.qdcservice.SuperficiForm.patchValue({
            Sup_Trattata:this.funzionicomuniservice.roundNumber(SupTot,this.qdcservice.Obj_Default_Numeric_Settings.decimals)
        });

        this.AutoCorrectNumericSource.next(true);

        this.qdcservice.SuperficiForm.markAsTouched();

        if(flag_calcolo_miscele){
            this.misceleservice.calcoloMiscele('Sup_Trattata',null,0, this.qdcservice.SuperficiForm.get("Sup_Trattata").value);
        }

    }


    GridImpiantionCellClose(event: CellCloseEvent, inputElementRef){

        this.calcoloSuperfici.ricalcoloSuperfici_Edit_Grid_Impianti(event.dataItem,event);

        if(!event.formGroup.valid &&
            event.formGroup.get('Sup_Imp_help')) {

            this.tooltip.show(inputElementRef.numericInput.nativeElement);
        } else {
            this.tooltip.hide();
        }

        let rows = this.qdcservice.GridImpiantiPublicService.getValue().data.rows;

        this.RicalcolaSuperficieCoinvolta(rows);
    }

    EventiPostselectionChangeGridImpianti(rows: Array<any>,ImpostaDisciplinare: boolean = true){

        //Funzione richiamata sia run-time quando seleziono/deseleziono una riga sia quando
        //ottengo gli ImpiantiSelezionati dal formarray

        this.Abilita_Disabilita_Resto(rows);
        this.RicalcolaSuperficieTotale(rows);
        this.RicalcolaSuperficieCoinvolta(rows);

        let Operazioni: Array<Lavorazione> = this.qdcservice.TestataForm.get("Operazioni").getRawValue();


        //Imposto la ddl del Disciplinare solamente se è visibile
        if(ImpostaDisciplinare &&
            Operazioni &&
            Operazioni.findIndex(o=>this.qdcservice.MostraDisciplinare(o,this.qdcservice.Sezioni_ProdottoFormArray)) > -1){

            if(!this.testataservice.Array_Disciplinari){
                this.testataservice.getArray_Disciplinari().then(d=>{
                    this.qdcservice.ImpostaDisciplinare_Dagli_ImpiantiSelezionati(this.testataservice.Array_Disciplinari).then(r=>{
                        if(r){
                            this.qdcservice.GestisciColoreRigheGridImpianti();
                            this.qdcservice.AvvisoImpianti(rows);
                            this.qdcservice.Calcola_Percentuale_N_Per_Piano_Nutrizionale_Fertilizzanti_Tutti();
                        }

                    });
                });
            }else{
                this.qdcservice.ImpostaDisciplinare_Dagli_ImpiantiSelezionati(this.testataservice.Array_Disciplinari).then(r=>{
                    if(r){
                        this.qdcservice.GestisciColoreRigheGridImpianti();
                        this.qdcservice.AvvisoImpianti(rows);
                        this.qdcservice.Calcola_Percentuale_N_Per_Piano_Nutrizionale_Fertilizzanti_Tutti();
                    }

                });
            }
        }

    }

    Abilita_Disabilita_Resto(rows){

        const kkNonCiSonoImpiantiSelezionati = !this.kCiSonoImpiantiSelezionati(rows);

        this.Abilita_Disabilita_DDL('Specie', kkNonCiSonoImpiantiSelezionati);

        this.Abilita_Disabilita_DDL('Centro_Aziendale', kkNonCiSonoImpiantiSelezionati);

        this.Abilita_Disabilita_DDL('Campo', kkNonCiSonoImpiantiSelezionati);

        this.Abilita_Disabilita_DDL('Operatore_Visita', kkNonCiSonoImpiantiSelezionati);

        this.Abilita_Disabilita_DDL('Azienda_Visita', kkNonCiSonoImpiantiSelezionati);

        this.Abilita_Disabilita_DDL('Visualizza_Specie', kkNonCiSonoImpiantiSelezionati);
    }

    Abilita_Disabilita_DDL(nomeddl: string,abilita: boolean){

        if(this.qdcservice.TestataForm.get(nomeddl)){
            if (!abilita) {
                this.qdcservice.TestataForm.get(nomeddl).disable({emitEvent: false});
            } else {
                this.qdcservice.TestataForm.get(nomeddl).enable({emitEvent: false});
            }
        }

        if(this.qdcservice.TestataVisitaForm.get(nomeddl)){
            if (!abilita) {
                this.qdcservice.TestataVisitaForm.get(nomeddl).disable({emitEvent: false});
            } else {
                this.qdcservice.TestataVisitaForm.get(nomeddl).enable({emitEvent: false});
            }
        }

    }

    //replica una parte dell' inizializzazioneKendo_BufferZone della Trattamenti_2
    AggiornaGridImpiantiDopoCambioPercentualeAbbatimento(){

        let percAbb:number = this.qdcservice.OttieniPercentualeAbbattimentoDiserboDisseccamento(null,null);

        let datiGriglia = this.qdcservice.GridImpiantiPublicService.getValue().data.rows;

        let ricalcoliEffettuati = false;

        // ******************************************************************************************************
        // CALCOLI PER PERCENTUALE ABBATTIMENTO
        // ******************************************************************************************************

        if (percAbb > 0 && percAbb < 100)
        {

            for (var i = 0; i < datiGriglia.length; i++) {

                if(datiGriglia[i].Selected){
                    datiGriglia[i].Sup_Imp_help = this.calcoloSuperfici.calcoloPercentualeAbbatimento(datiGriglia[i].Sup_Imp, datiGriglia[i].Sup_Imp_help);
                }

            }

            ricalcoliEffettuati = true;
        }


        if(ricalcoliEffettuati){
            this.qdcservice.AggiornaFormArrayImpiantiSelezionati();

            this.RicalcolaSuperficieCoinvolta(datiGriglia);
        }

    }

    //Richiamo questa funzione al cambio del Prodotto per poter ricalcolare correttamente le buffer in base alla nuova
    //buffer minima del prodotto (se la buffer minima è minore di quella già presente nella grid considero quello più alta)
    AggiornaGridImpiantiXBufferzone(){

        let ricalcoliEffettuati: boolean = false;

        let ImpiantiSelezionati: Array<GridImpiantoSelezionatoModel> = JSON.parse(JSON.stringify(this.qdcservice.ImpiantiSelezionatiFormArray.getRawValue()));

        if(ImpiantiSelezionati && ImpiantiSelezionati.length > 0){
            for(let i of ImpiantiSelezionati){
                if(this.calcoloSuperfici.ricalcoloSuperfici(i)){
                    ricalcoliEffettuati = true;
                }
            }
        }


        if(ricalcoliEffettuati){

            this.qdcservice.RicaricaGridImpianti();

            let rows = this.qdcservice.GridImpiantiPublicService.getValue().data.rows;

            this.RicalcolaSuperficieCoinvolta(rows);
        }

    }


}
