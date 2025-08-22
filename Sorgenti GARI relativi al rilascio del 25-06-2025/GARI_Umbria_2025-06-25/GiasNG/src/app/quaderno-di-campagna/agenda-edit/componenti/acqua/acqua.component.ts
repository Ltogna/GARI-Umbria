/* eslint-disable */
import { Component, OnInit } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';
import { Lavorazione } from 'app/Model/attivita/Lavorazione';
import { enum_LAVCOD } from 'app/Model/TipiEnumerativi';
import { QdCService } from '../../service/qdc.service';
import {MisceleService} from "../../service/miscele.service";
import {ObjParametriAgendaService} from "../../../../Service/obj-parametri-agenda.service";
import {Enum_DBTypeOperation} from 'gias-ui-kit';
import {Acqua} from "../../quaderno-di-campagna-form/quaderno-di-campagna-form.model";
import {TranslocoService} from "@jsverse/transloco";
import {RibaltamentoTypes} from "../../../../menu-agenda/components/utils";
import { Tipo } from 'app/Model/attivita/centri_di_costo/CentroDiCosto';

@Component({
    standalone: false,
    selector: 'app-acqua',
    templateUrl: './acqua.component.html',
    styleUrls: ['./acqua.component.scss']
})
export class AcquaComponent implements OnInit {
    constructor(public parent: FormGroupDirective,
                public qdcservice: QdCService,
                private misceleservice: MisceleService,
                private objParametriAgendaService: ObjParametriAgendaService,
                private translocoService: TranslocoService) { }

    ngOnInit(): void {

        //Imposto in automatico l'acqua dalla taratura ugello della macchina appena vengono mostrate le textbox e sono in scrittura ma non durante un ribaltamento
        if(this.qdcservice.mostraAcqua() && this.objParametriAgendaService.getObjParamValue().TipoOperazioneDB === Enum_DBTypeOperation.Write && this.qdcservice.TipoRibaltamento === RibaltamentoTypes.Nessuno){

            this.qdcservice.Imposta_VolumiAcqua(0,null,"").then(risp=>{
                //Triggero il ricalcolo dell'acqua
                if(risp){
                    this.misceleservice.calcoloMiscele('changeAcqua_Ha',null,0);
                }
            });

        }

    }

    changeAcqua_Ha(value){
        this.misceleservice.calcoloMiscele('changeAcqua_Ha',null,0);
    }

    changeAcqua_Tot(value){
        this.misceleservice.calcoloMiscele('changeAcqua_Tot',null,0);
    }

    componi_descrizione_Acqua(){
        let descrizione = ""

        if(this.qdcservice.EsisteProdottoPolverulento())
            descrizione += this.translocoService.translate("TrattamentoPolverulento");

        return descrizione;
    }

    getUdM(): string {
        return this.qdcservice.getCentroDiCostoTipo() == Tipo.ProdottoDaTrattare ? 'q' : 'Ha';
    }

    getProvenienza(): string {
        if (this.qdcservice.getCentroDiCostoTipo() == Tipo.ProdottoDaTrattare) {
            return this.qdcservice.AcquaForm.get('Acqua_Ha').value > 0
                ? this.qdcservice.obj_Acqua_Provenienza.Descrizione
                : '';
        }

        return this.qdcservice.obj_Acqua_Provenienza.Descrizione;
    }
}
