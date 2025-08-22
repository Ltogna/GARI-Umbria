import { Injectable, OnDestroy, signal } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { TranslocoService } from "@jsverse/transloco";
import { BaseCodeDescr } from "app/Model/baseClass/baseCodeDescr";
import { OperazioniZooClient } from "app/Service/net-core6-api.service";
import { ObjParametriAgendaService } from "app/Service/obj-parametri-agenda.service";
import { Subject, map, takeUntil } from "rxjs";
import { DettagliProtocollo } from "../model/dettagli-protocollo.model";
import { Enum_DBTypeOperation, ObjParametriAgenda } from "gias-ui-kit";

export class Stalla {
    STA_NUM: number;
    STA_DES: string;
    BDN_Allev_IdFiscale: string;
    BDN_Codice_Azienda: string;

    constructor(staNum, staDes, BDNproprietario, BDNcodAzienda) {
        this.STA_NUM = staNum;
        this.STA_DES = staDes;
        this.BDN_Allev_IdFiscale = BDNproprietario;
        this.BDN_Codice_Azienda = BDNcodAzienda;
    }
}

class TrattamentoZoo {
    Data: Date;
    // Ora: Time;
    CentroAziendale: BaseCodeDescr;
    Stalla: Stalla;
    Raggruppamento: BaseCodeDescr;
}


@Injectable()
export class TrattamentoZooFormService implements OnDestroy {

    signal: Subject<void> = new Subject();

    formTrattamentoZoo: FormGroup = CreateFormGroup(this.fb, this.InitializeFormTrattamentoZoo());

    defaultCentro: BaseCodeDescr;
    defaultStalla: Stalla;
    defaultRaggruppamento: BaseCodeDescr;

    Array_CentriAziendale: Array<BaseCodeDescr> = [];
    Array_Stalle: Array<Stalla> = [];
    Array_Stalle_Raggruppamenti: Array<BaseCodeDescr> = [];

    objParametriAgenda: ObjParametriAgenda;

    dettagliProtocollo: DettagliProtocollo;
    attivitaDaChiamante: any;

    showGriglie: boolean = false;
    // drugIsSelected: boolean = false;
    ddlSelectionSubject: Subject<void> = new Subject();
    ddlSelectionRaggruppamentoSubject: Subject<void> = new Subject();
    
    private _changeDrugSelected = signal<{ codice: number | null, udm?: string }>({ codice: null });
    readonly changeDrugSelected = this._changeDrugSelected.asReadonly();

    //array delle row selezionate nelle griglie
    arrayDettagliProdottiSomministrazione: Array<any> = [];

    currentSaCod: number = 0;

    isInfoMode: boolean = false;

    constructor(
                private fb: FormBuilder,
                private agendaService: ObjParametriAgendaService,
                public transloco: TranslocoService,
                private zooService: OperazioniZooClient
            ) {

                this.objParametriAgenda = this.agendaService.getObjParamValue();

                this.isInfoMode = this.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Read;

                this.defaultCentro = new BaseCodeDescr(0, this.transloco.translate('TuttiICentriAziendali'));
                this.defaultStalla = new Stalla(0, this.transloco.translate('TutteLeStalle'), "", "");
                this.defaultRaggruppamento = new BaseCodeDescr(0, this.transloco.translate('TuttiIRaggruppamenti'));

                this.formTrattamentoZoo.get('Data').valueChanges.pipe(takeUntil(this.signal)).subscribe(value => {
                    if (this.showGriglie) {
                        // this.drugIsSelected = false;
                        this.ddlSelectionSubject.next();
                    }
                });

                this.formTrattamentoZoo.get('CentroAziendale').valueChanges.pipe(takeUntil(this.signal)).subscribe(value => {
                    this.currentSaCod = value.codice;
                    this.loadStalleDDL(this.objParametriAgenda.Piva, value.codice);
                });

                this.formTrattamentoZoo.get('Stalla').valueChanges.pipe(takeUntil(this.signal)).subscribe(value => {
                    this.loadRaggruppamenti(this.objParametriAgenda.Piva, this.currentSaCod, value.STA_NUM);
                });

                this.formTrattamentoZoo.get('Raggruppamento').valueChanges.pipe(takeUntil(this.signal)).subscribe(value => {
                    this.ddlSelectionRaggruppamentoSubject.next();
                });
    }

    signalChangeDrugSelected(codice: number, udm?: string): void {
        // if (this.objParametriAgenda.TipoOperazioneDB !== Enum_DBTypeOperation.Update)
            this._changeDrugSelected.set({ codice, udm });
    }

    ngOnDestroy(): void {
        this.signal.next();
        this.signal.complete();
    }

    InitializeFormTrattamentoZoo(vals?: Partial<TrattamentoZoo>): TrattamentoZoo {
        return {
            Data: vals?.Data ?? new Date(),
            // Ora: vals?.Ora ?? null,
            CentroAziendale: vals?.CentroAziendale ?? null,
            Stalla: vals?.Stalla ?? null,
            Raggruppamento: vals?.Raggruppamento ?? null
        };
    }

    loadCentriAziendaliDDL() {

        this.zooService.operazioniZooGetCentriAziendali(this.objParametriAgenda.Piva)
        .pipe(
            map(r => JSON.parse(r.RispostaStringa)),
            map(centers => centers.map(x => new BaseCodeDescr(x['sa_cod'], x['sa_nome'])))
        )
        .subscribe(result => {
            this.Array_CentriAziendale = result;

            if (this.attivitaDaChiamante)
                this.formTrattamentoZoo.get('CentroAziendale').patchValue(result.find(item => item.codice == this.attivitaDaChiamante.centroAziendale.primaryKey.codice));

        });

    }

    loadStalleDDL(piva: string, saCod: number) {

        this.formTrattamentoZoo.get('Stalla').patchValue(this.defaultStalla);

        if (saCod !== 0) {
            this.zooService.operazioniZooGetStalle(piva, saCod)
            .pipe(
            map(r => JSON.parse(r.RispostaStringa))
            )
            .subscribe(result => {
                this.Array_Stalle = result;

                //se arrivo con l'oggetto parametrizzato
                if (this.attivitaDaChiamante) 
                    this.formTrattamentoZoo.get('Stalla').patchValue(result.find(item => item.STA_NUM == this.attivitaDaChiamante.fabbricatoCod));
            });
        } else {
            this.Array_Stalle.splice(0, this.Array_Stalle.length);
        }
    }

    loadRaggruppamenti(piva: string, saCod: number, stalla: number) {

        this.formTrattamentoZoo.get('Raggruppamento').patchValue(this.defaultRaggruppamento, { emitEvent: false });
        
        if (stalla !== 0) {
            // this.reloadGridParametri();
            if (!this.showGriglie)
                this.showGriglie = true;
            else
                this.ddlSelectionSubject.next();

            this.zooService.operazioniZooGetRaggruppamenti(piva, saCod, stalla)
            .pipe(
            map(r => JSON.parse(r.RispostaStringa)),
            map(stables => stables.map(x => new BaseCodeDescr(x['Raggruppamento_Cod'], x['Raggruppamento_Des']))),
            )
            .subscribe(result => {
                this.Array_Stalle_Raggruppamenti = result;
            });
        } else {
            this.showGriglie = false;
            this.Array_Stalle_Raggruppamenti.splice(0, this.Array_Stalle_Raggruppamenti.length);
        }
    }

    reloadGridParametri() {
        this.showGriglie = false;
        setTimeout(() => this.showGriglie = true, 0);
    }
    
}

function CreateFormGroup(fb: FormBuilder, mask: TrattamentoZoo) {

    return fb.group({
        Data: new FormControl(mask.Data),
        // Ora: new FormControl(mask.Ora),
        CentroAziendale: new FormControl(mask.CentroAziendale, [Validators.required]),
        Stalla: new FormControl(mask.Stalla, [Validators.required]),
        Raggruppamento: new FormControl(mask.Raggruppamento)
    });

}