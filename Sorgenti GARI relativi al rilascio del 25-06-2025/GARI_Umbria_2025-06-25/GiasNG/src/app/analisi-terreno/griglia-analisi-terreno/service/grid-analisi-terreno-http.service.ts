import { Injectable, Injector } from "@angular/core";
import { EditingMode, KendoGridColumn, KendoGridModel, KendoGridRow, KendoServerResult, LoaderType } from 'gias-kendo-grid';
import { ConfigTemplate } from 'gias-kendo-grid';
import { AbstractGridConfigService, HttpAction } from 'gias-kendo-grid';
import { Observable, catchError, map, of, skip, switchMap } from "rxjs";
import { AnalisiTerrenoService, LeggiListaAnalisiTerreno } from "./analisi-terreno.service";
import { GridPublicService } from 'gias-kendo-grid';
import { ObjParametriAgendaService } from "app/Service/obj-parametri-agenda.service";
import { TranslocoService } from "@jsverse/transloco";
import { CommandsColumnSettings, CommandsDropDownSettings, DeletionMode, DettagliColumnSettings, SelectableSettings } from 'gias-kendo-grid';
import { GridCommandItem, QdCRow } from "app/menu-agenda/components/utils";
import { enum_visiteGridCommands } from "app/visite/utils";
import { enum_AnalisiTerrenoGridCommands } from "app/analisi-terreno/utils";
import { AnalisiDocumentsService } from "./analisi-documents.service";
import { SelectionEvent } from "@progress/kendo-angular-grid";
import { GiasKendoGridComponent } from 'gias-kendo-grid';
import { AnalisiTerreno, Campione, ScriviAnalisiTerreno, ScriviListaAnalisiTerreno } from "app/Service/api.service";
import { IntervalloTemporale } from "app/Model/anagrafiche/IntervalloTemporale";
import { PermessiUtenteService } from "app/Service/permessi-utente.service";
import { enum_Security_Attivita } from "app/Model/TipiEnumerativi";
import { ActivatedRoute, Router } from "@angular/router";
import { AnalisiTerrenoFormService } from "./analisi-terreno-form.service";
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { FormArray } from "@angular/forms";
import { AGRODATAFINE, AGRODATAINIZIO } from "app/Model/CostantiPersonalizzate";

export class GridAnalisiTerrenoServerResult extends KendoServerResult {
    constructor(rows: KendoGridRow[], cols: KendoGridColumn[], model: KendoGridModel) {
        super(model, cols, rows);
    }
}

@Injectable()
export class GridAnalisiTerrenoHttpService extends AbstractGridConfigService<GridAnalisiTerrenoServerResult> {

    gridId = 'AnalisiTerrenoGridId';
    rowId = 'idAnalisiTerrenoGrid';

    loader: LoaderType = LoaderType.SERVICE;
    editingMode: EditingMode = EditingMode.IN_LINE;

    param: LeggiListaAnalisiTerreno;
    pivaAttuale: string;

    selectedRows = [];

    permessoEdit: boolean = false;

    constructor(injector: Injector,
                public gridpublicService: GridPublicService,
                private analisiService: AnalisiTerrenoService,
                private objParametriAgenda: ObjParametriAgendaService,
                private analisiDocumentsService: AnalisiDocumentsService,
                private analisiFormService: AnalisiTerrenoFormService,
                private permessiUtenteService: PermessiUtenteService,
                private route: ActivatedRoute,
                private router: Router,
                public transloco: TranslocoService
    ) {
        super(injector, ConfigTemplate.DefaultTemplate);

        this.resizable.autoFitColumns = true;

        let permessoScritturaDoc = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Documentale_Inser, 2);
        let permessoLetturaDoc = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Documentale_Lista, 0);
        this.permessoEdit = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Gest_Analisi_AccessoMenu, 2);
        let permessoLetturaAnalisi = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Gest_Analisi_AccessoMenu, 0);

        this.cmdColumn = new CommandsColumnSettings({
            editBtn: this.permessoEdit,
            infoBtn: false,
            removeBtn: false,
            onDisableInfoBtn: () => false,
            width: 10
        });

        this.cmdDropDown = new CommandsDropDownSettings({
            fullEditBtn: this.permessoEdit, removeBtn: this.permessoEdit, infoBtn: permessoLetturaAnalisi
        });

        this.behavior.showDeletionConfirmation = true;
        this.behavior.deletionMode = DeletionMode.HandleSingleRowDeletionOnly;

        if (permessoScritturaDoc)
            this.cmdDropDown.addCommand(new GridCommandItem(
                "AggiungiNuovoAllegato",
                enum_AnalisiTerrenoGridCommands.NUOVO_ALLEGATO,
                'faQdCUploadFile'
            ));

        if (permessoLetturaDoc)
            this.cmdDropDown.addCommand(new GridCommandItem(
                "RicercaDocumenti",
                enum_AnalisiTerrenoGridCommands.RICERCA_DOCUMENTI,
                'faQdCSearchDocument'
            ));

        this.pivaAttuale = this.objParametriAgenda.getObjParamValue().Piva;
        this.param = new LeggiListaAnalisiTerreno(this.pivaAttuale, new Date());

        this.analisiService.filterData.pipe(skip(1)).subscribe((val) => {
            this.param.Piva = this.pivaAttuale;
            this.param.Data = val.Data ?? AGRODATAINIZIO;
            this.gridPublicService.refresh(true);
        });

        if (this.permessoEdit) {
            this.selectable.selectable = new SelectableSettings({
                checkboxOnly: true,
                enabled: true,
            });
            this.selectable.preselectedRows.selectionChangeFn = this.selectionChangeFn;

            this.selectable.preselectedRows.selectedRows = [];
            this.selectable.columnSettings.showSelectAll = true;
            this.selectable.shouldShowCheckbox = true;
            this.selectable.columnSettings.title = ' ';
        }

        // this.setButtonsVisibility();
        this.handleCommands();
    }

    public selectionChangeFn = (event: SelectionEvent, component: GiasKendoGridComponent) => {
        const selectedRows = event.selectedRows;
        const deselectedRows = event.deselectedRows;

        if (selectedRows.length > 0)
            selectedRows.forEach(item => { 
                if (!this.selectedRows.some(saveItem => saveItem.dataItem.Analisi_Testata_Cod === item.dataItem.Analisi_Testata_Cod))
                    this.selectedRows.push(item);
            });
        else 
            this.selectedRows = this.selectedRows.filter(item => !deselectedRows.some(deselectedItem => deselectedItem.dataItem.Analisi_Testata_Cod === item.dataItem.Analisi_Testata_Cod));
    }

    read(options?: any): Observable<GridAnalisiTerrenoServerResult> {
        
        this.loadingService.set_isLoading({ isLoading: true, component: this.gridPublicService.gridElRef });

        // let param = new LeggiListaAnalisiTerreno(this.objParametriAgenda.getObjParamValue().Piva, AGRODATAINIZIO);

        return this.analisiService.LeggiListaAnalisiTerreno(this.param).pipe(catchError((err) => {
            this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef });
            return of();
        }), map((r:any) => {

            this.loadingService.set_isLoading({ isLoading: false, component: this.gridPublicService.gridElRef });

            let gridListaAnalisiServerResult = new GridAnalisiTerrenoServerResult(
                this.setRowGrid(r.kendo_rows),
                this.setColumnsGrid(r.kendo_columns, r.kendo_model),
                r.kendo_model as KendoGridModel
            );

            return gridListaAnalisiServerResult;
        }));
        
    }

    setRowGrid(responseRows) {
        let rows: Array<KendoGridRow> = [];
        let indexRowAnalisiTerreno = 0;

        for (let itemOfRows of responseRows) {
            indexRowAnalisiTerreno++;
            itemOfRows.idAnalisiTerrenoGrid = indexRowAnalisiTerreno;
            rows.push(
                itemOfRows
            );
        }

        return rows;
    }

    setColumnsGrid(columnsName, modelsName): Array<KendoGridColumn> {
        let columns: Array<KendoGridColumn> = [];
        for (let itemOfColumns of columnsName) {
            columns.push(
                new KendoGridColumn({ field: itemOfColumns.field, title: itemOfColumns.title }, { resizable: true, editable: modelsName[itemOfColumns.field].editable })
            );
        }

        return columns;
    }

    perform(actionType: HttpAction, items: any): Observable<any> {

        switch(actionType) {
            case HttpAction.REMOVE:
                this.loadingService.set_isLoading({ isLoading: true, message: '', component: this.gridPublicService.gridElRef });
                return this.deleteItem(items, false).pipe(
                    switchMap((r) => { 
                        this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef });
                        this.selectedRows.splice(0, this.selectedRows.length);
                        this.gridPublicService.refresh(true);
                        return of();
                    })
                );
            break;

            case HttpAction.UPDATE:
                return this.updateAnalisi(items);
            break;

            default:
                return of();
        }
    }

    private deleteItem(items: any, proseguiInCasoDiAlert: boolean) {

        let arrParam: ScriviListaAnalisiTerreno = {
            listaAnalisiTerreno: []
        }

        if (!Array.isArray(items))
            items = [ items ];

        items.forEach(item => {
            let validitaAnalisi = new IntervalloTemporale(item.Validita_Inizio, item.Validita_Fine);

            let param = {
                Piva: this.pivaAttuale,
                AnalisiTerreno: {
                    codice: item.Analisi_Testata_Cod,
                    descrizione: item.Descrizione,
                    certificatoAnalisi: { 
                                            codice: item.Analisi_Certificato_Cod,
                                            numero_certificato: item.NumeroCertificato
                    },
                    validita: validitaAnalisi,
                    flag_cancellazione: true
                }
            };

            arrParam.listaAnalisiTerreno.push(param);
        });

        return this.analisiService.CancellaListaAnalisiTerreno(arrParam);
    }

    private updateAnalisi(item: any) {

        let validitaAnalisi = new IntervalloTemporale(item.Validita_Inizio, item.Validita_Fine);

        let param = {
            Piva: this.pivaAttuale,
            AnalisiTerreno: {
                codice: item.Analisi_Testata_Cod,
                descrizione: item.Descrizione,
                certificatoAnalisi: { 
                                        codice: item.Analisi_Certificato_Cod,
                                        numero_certificato: item.NumeroCertificato
                },
                note: item.Analisi_Testata_Note1,
                validita: validitaAnalisi
            }
        };

        return this.analisiService.ModificaAnalisiTerreno(param);
    }

    private setButtonsVisibility() {
        this.gridPublicService.openCommands.GiasSubscribe(async attivita => {
            
            if (attivita) {
                // this.cmdDropDown.removeAllCommand();
                this.cmdDropDown.removeAllCommand();

                this.cmdDropDown.addCommand(new GridCommandItem(
                    "AggiungiNuovoAllegato",
                    enum_AnalisiTerrenoGridCommands.NUOVO_ALLEGATO,
                    'faQdCUploadFile'
                ));

                if (attivita.conAllegato > 0) {
                    this.cmdDropDown.addCommand(new GridCommandItem(
                        "RicercaDocumenti",
                        enum_visiteGridCommands.RICERCA_DOCUMENTI,
                        'faQdCSearchDocument'
                    ));
                }
            }
        });
    }

    private handleCommands() {
        this.gridPublicService.commandEvent.GiasSubscribe(ev => { 
            if (!ev) return;
            switch (ev.command.action) {
                case enum_AnalisiTerrenoGridCommands.RICERCA_DOCUMENTI:
                    this.analisiDocumentsService.ApriKendoWindowRicercaDocumenti(ev.dataItem.Analisi_Testata_Cod, this.param.Piva);
                break;

                case enum_AnalisiTerrenoGridCommands.NUOVO_ALLEGATO:
                    this.analisiDocumentsService.ApriKendoWindowAggiungiNuovoAllegato(ev.dataItem.Analisi_Testata_Cod, ev.dataItem.Validita_Fine, this.param.Piva);
                break;

                case enum_AnalisiTerrenoGridCommands.MODIFICA_COMPLETA:
                case enum_AnalisiTerrenoGridCommands.INFO:
                    this.goToEditPage(ev.dataItem, ev.command.action);
                break;
            }
        });
    }

    public goToEditPage(dataItem, typeCommand: enum_AnalisiTerrenoGridCommands) {

        let objParam = this.objParametriAgenda.getObjParamValue();

        switch(typeCommand) {

            case enum_AnalisiTerrenoGridCommands.INFO:
                objParam.TipoOperazioneDB = Enum_DBTypeOperation.Read;
                this.analisiFormService.formAnalisiTerreno.disable({ emitEvent: false });
            break;

            case enum_AnalisiTerrenoGridCommands.NUOVA_ANALISI:
                objParam.TipoOperazioneDB = Enum_DBTypeOperation.Write;
                this.analisiFormService.formAnalisiTerreno.enable({ emitEvent: false });
            break;

            case enum_AnalisiTerrenoGridCommands.MODIFICA_COMPLETA:
                objParam.TipoOperazioneDB = Enum_DBTypeOperation.Update;
                this.analisiFormService.formAnalisiTerreno.enable({ emitEvent: false });
            break;
        }

        if (dataItem) {

            objParam.GenericObj_string = JSON.stringify({ Analisi_Testata_Cod: dataItem.Analisi_Testata_Cod });
            this.objParametriAgenda.changeObjParametriAgenda(objParam);

            this.router.navigate(['edit'], { relativeTo: this.route, queryParams: { ...this.route.snapshot.queryParams } });

        } else {
            objParam.GenericObj_string = '';
            this.objParametriAgenda.changeObjParametriAgenda(objParam);
            this.analisiFormService.formAnalisiTerreno.reset();
            this.analisiFormService.formAnalisiTerreno.patchValue(this.analisiFormService.InitializeFormAnalisiTerreno());
            this.router.navigate(['edit'], { relativeTo: this.route, queryParams: { ...this.route.snapshot.queryParams } });
        }
    }
}