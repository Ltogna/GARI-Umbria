/* eslint-disable */
import { Injectable, Injector } from '@angular/core';
import { AbstractGridConfigService, HttpAction } from 'gias-kendo-grid';
import { combineLatest, Observable, of } from 'rxjs';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import {  DropdownListItem, DropdownListWithForm, EditingMode, KendoGridColumn, KendoGridModel, KendoGridRow, KendoServerResult, LoaderType } from 'gias-kendo-grid';
import { CELL_TYPES } from 'gias-ui-kit';
import { ConfigTemplate } from 'gias-kendo-grid';
import { map, take } from 'rxjs/operators';
import { ExcelSettings, PDFSettings, ToolbarSettings } from 'gias-kendo-grid';
import { MetodoProduzione } from 'app/Model/metaschema/MetodoProduzione';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { ContattiEditGridModel, ContattiEditServerResult } from './contattiGrid.models';
import { ContattiRootService } from './contattiRoot.service';
import { MasterService, rispostaStandard } from 'app/Service/master.service';
import { AjaxAgronicaService } from 'app/Service/ajax-agronica.service';
import { BaseCodeDescr } from 'app/Model/baseClass/baseCodeDescr';
import { TranslocoService } from '@jsverse/transloco';
import { AjaxAgronicaAPIService } from 'app/Service/ajax-agronica.api.service';
import { ObjParametriAgenda } from 'gias-ui-kit';



export class ContattiKendo extends KendoServerResult {
    constructor(rows: KendoGridRow[], cols: KendoGridColumn[], model: KendoGridModel) {
        super(model, cols, rows);
    }
}

@Injectable()
export class ContattiGridService extends AbstractGridConfigService<ContattiEditServerResult>{
    gridId = 'Contatti';
    rowId = 'codice';

    rapportoContabileSub: any;

    edit: boolean;
    objParametriAgenda: ObjParametriAgenda;
    toolbar = new ToolbarSettings(true, false);

    loader: LoaderType = LoaderType.SERVICE;
    editingMode: EditingMode = EditingMode.IN_CELL;

    kendoColumns: Array<KendoGridColumn>;

    kendoModel: ContattiEditGridModel = {
        descrizione: {
            editable: false,
            type: CELL_TYPES.STRING
        },
        codice: {
            editable: true,
            type: CELL_TYPES.DROPDOWNLIST
        },
        settore: {
             editable: false,
             type: CELL_TYPES.STRING
        },
        attivita: {
            editable: false,
            type: CELL_TYPES.STRING
        }
    };

    constructor(
        injector: Injector,
        protected masterService: MasterService,
        private ObjParametriAgendaService: ObjParametriAgendaService,
        protected ajaxAgronicaService: AjaxAgronicaService,
        protected ajaxAgronicaAPIService: AjaxAgronicaAPIService,
        private contattiRootService: ContattiRootService,
        private translocoService: TranslocoService
    ) {

        super(injector, ConfigTemplate.DefaultTemplate);

        this.objParametriAgenda = this.ObjParametriAgendaService.getObjParamValue();
        this.edit = true;
        if (this.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Read ||
            this.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Update) {
            this.edit = false;
        }

        // Inizializzo il campo standard se in fase di creazione
        if(this.edit){
            let startRow = {
                attivita: '',
                codice: -3,
                descrizione: "Fornitore",
                settore: ''
            }
            let startRows = [startRow]
            this.contattiRootService.setRowsContatti(startRows)
        }

        this.kendoColumns = [
            new KendoGridColumn(
                { field: "codice", title: this.translocoService.translate('RapportoContabile') },
                { resizable: true, editable: this.edit, width: 135}
            ),
            new KendoGridColumn(
                { field: 'settore', title: this.translocoService.translate('Progressivo') },
                { resizable: true, editable: this.edit, width: 135}
            ),
            new KendoGridColumn(
                { field: 'attivita', title: this.translocoService.translate('Attività') },
                { resizable: true, editable: this.edit, width: 135}
            )
        ];

        this.handleCustomizations();
    }

    perform(actionType: HttpAction, items: any): Observable<any[]> {

        switch (actionType) {
            case HttpAction.CREATE:
                break;
            case HttpAction.REMOVE:
                break;
            case HttpAction.UPDATE:
                break;
        }
        this.contattiRootService.gridRowsContattiSource.next(this.gridPublicService.value.data.rows)
        return of()
    }


    read(): Observable<ContattiEditServerResult> {

        const stream1 = this.contattiRootService.gridRowsContattiSource;

        return combineLatest([stream1, this.caricaRapportoContabileAPI()])
            .pipe(take(1), map((risultato: any[]) => {
            this.handleDropdowns(risultato[1])
            return {
                model: this.kendoModel,
                columns: this.kendoColumns,
                rows: risultato[0]
            };
        }));
    }

    handleDropdowns(ddlist: MetodoProduzione[]): void {
        const col = this.kendoColumns.find(s => s.field === 'codice');

        const data: DropdownListItem[] = ddlist.map(ddlItem => new DropdownListItem(ddlItem.codice, ddlItem.descrizione));

        col.ddl = new DropdownListWithForm('RapportoContabile', 'codice', 'descrizione', data);
        col.ddl.valuePrimitive = true;
        col.ddl.descriptionField = 'descrizione';
    }

    /*caricaRapportoContabile(): Observable<RapportoContabile> {
        const parametri: any = {
            objP: this.masterService.getCoreWSGenericObjP(),
            InData: ""
        };

        return this.ajaxAgronicaService.ajaxAgronicaCoreWS_GenericsObs<RapportoContabile, any>(
            this.masterService.link_CoreWS + '/Metaschema/Rapporti_Contabili.asmx/LeggiDropdownContattiImprese',
            parametri
        ).pipe(map((risposta: rispostaStandard<RapportoContabile>) => {

            if(risposta.RispostaOK) {
                return risposta.RispostaStringa;
            } else {
                alert("Qualcosa è andato storto.");
            }

        }));
    }*/

    caricaRapportoContabileAPI(): Observable<RapportoContabile> {
        return this.ajaxAgronicaAPIService.ajaxAPIGet<any, RapportoContabile>('MetaschemaNG/LeggiDropdownContattiImprese',
            ""
        ).pipe(map((risposta: rispostaStandard<RapportoContabile>) => {

            if(risposta.RispostaOK) {
                return risposta.RispostaStringa;
            } else {
                alert("Qualcosa è andato storto.");
            }
        }));
    }

    private handleCustomizations() {
        // Nascondo la colonna Azioni con i bottoni di Info,Modifica e Cancella
        this.cmdColumn.editBtn = false;
        this.cmdColumn.infoBtn = false;
        this.cmdColumn.removeBtn = this.edit;
        this.toolbar.newItem = this.edit;

        // this.resizable.autoFitColumns = true;
        this.resizable.isResizable = true;

        // Setting generali
        this.columnMenu.columnMenu = false;
        this.columnMenu.filterable = false;
        this.behavior.saveExternalChanges = true;
        this.behavior.excelSettings = new ExcelSettings({enabled: false});
        this.behavior.pdfSettings = new PDFSettings({enabled: false});
        this.generalSettings.reordable = false;
        this.generalSettings.performOnEdit = true;
        this.groups.groupable.enabled = false;
        this.views.enabled = false;
    }

}

export class RapportoContabile extends BaseCodeDescr
{
    cliente: boolean
    fornitore: boolean
    dipendente: boolean
    terzista: boolean
    legale: boolean
    agente: boolean
    consulente: boolean
    flag_cancellazione: boolean
}
