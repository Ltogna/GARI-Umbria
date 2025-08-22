/* eslint-disable */
import { Injectable, Injector } from '@angular/core';
import { FormGroup, FormGroupDirective, Validators } from '@angular/forms';
import { TranslocoService } from '@jsverse/transloco';
import { ImpresaPadre } from 'app/Model/anagrafiche/ImpresaPadre';
import { AGRODATAINIZIO } from 'app/Model/CostantiPersonalizzate';
import { BehaviorSettings, CommandsColumnSettings, ExcelSettings, PDFSettings } from 'gias-kendo-grid';
import {  DateSettings, DropdownListItem, DropdownListWithForm, EditingMode, KendoGridColumn, LoaderType, ModelEntry } from 'gias-kendo-grid';
import { CELL_TYPES } from 'gias-ui-kit';
import { ConfigTemplate } from 'gias-kendo-grid';
import { AbstractGridConfigService, HttpAction } from 'gias-kendo-grid';
import { GridPublicService } from 'gias-kendo-grid';
import { Observable, of } from 'rxjs';
import { ContattiRootService } from '../contatti-edit/contattiRoot.service';
import { ImpresaEditService } from '../impresa-edit.service';
import { CooperativeKendoGridRow, CooperativeKendoServerResult } from './utils';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { ObjParametriAgenda } from 'gias-ui-kit';


@Injectable()
export class CooperativeConfigService extends
    AbstractGridConfigService<CooperativeKendoServerResult>{
    editingMode: EditingMode = EditingMode.IN_CELL;
    loader: LoaderType = LoaderType.SERVICE;
    rowId: string = 'chiave';
    gridId: string = 'CooperativeConfigService';
    behavior: BehaviorSettings = new BehaviorSettings({ saveExternalChanges: true });

    cmdColumn: CommandsColumnSettings = new CommandsColumnSettings({infoBtn: false, editBtn: false,  removeBtn: true })

    

    padri: ImpresaPadre[];

    cooperativeFormGroup: FormGroup;

    constructor(injector: Injector,
        private gridPubService: GridPublicService,
        protected transloco: TranslocoService,
        private contattiRootService: ContattiRootService,
        private parametriAgendaService: ObjParametriAgendaService,
        private rootFormGroup: FormGroupDirective) {
        super(injector, ConfigTemplate.DefaultTemplate);

        
        this.toolbar.newItem = true;
        this.generalSettings.performOnEdit = true;
        this.cmdColumn.editBtn = false;

        if ((<ObjParametriAgenda>this.parametriAgendaService.getObjParamValue()).TipoOperazioneDB == Enum_DBTypeOperation.Read){
            this.disableGrid();
        }

        this.columnMenu.columnMenu = false;
        this.columnMenu.filterable = false;
        this.behavior.excelSettings = new ExcelSettings({enabled: false});
        this.behavior.pdfSettings = new PDFSettings({enabled: false});
        this.generalSettings.reordable = false;
        this.generalSettings.performOnEdit = true;
        this.groups.groupable.enabled = false;
        this.views.enabled = false;

        this.cmdColumn.editBtn = false;
    }

    read(options?: any): Observable<CooperativeKendoServerResult> {
            
            //const agenda: ObjParametriAgenda = this.objParametriAgendaService.getObjParamValue();

            //this.handleDropdowns()
            let rows: CooperativeKendoGridRow[] = [];
            this.cooperativeFormGroup = this.rootFormGroup.form as FormGroup;  
            this.cooperativeFormGroup.get('impresaPadre').value.forEach(padre => {
                rows.push({ chiave: padre.partitaIva,
                    codcooperativa: padre.partitaIva,
                    cooperativa: padre.ragioneSociale,
                    numero: padre.codice_iscrizione_libro_soci,
                    data: padre.data_iscrizione_libro_soci ? padre.data_iscrizione_libro_soci : AGRODATAINIZIO,
                    flag_cancellazione: false
            })
        })
            const tableData = this.GetModel(rows);
            if ((<ObjParametriAgenda>this.parametriAgendaService.getObjParamValue()).TipoOperazioneDB == Enum_DBTypeOperation.Read){
                this.makeCellsUneditable();
            }
            return of(tableData);
    }
    
    perform(actionType: HttpAction, item: any[]): Observable<any[]> {
        let cooperativePadre: ImpresaPadre[] = this.cooperativeFormGroup.get('impresaPadre').value;
        switch(actionType){
            case HttpAction.CREATE:
                item[0].codcooperativa.data.codice_iscrizione_libro_soci = item[0].numero;
                item[0].codcooperativa.data.data_iscrizione_libro_soci = item[0].data ? item[0].data : new Date(1900, 0, 1, 0, 0, 0, 0);
                cooperativePadre.push(item[0].codcooperativa.data);
                this.cooperativeFormGroup.get('impresaPadre').patchValue(cooperativePadre);
            break;
            case HttpAction.UPDATE:
                let modifica = cooperativePadre.find(impresa => item[0].chiave == impresa.partitaIva);
                if(item[0].codcooperativa.data) {
                    for(var k in item[0].codcooperativa.data) modifica[k] = item[0].codcooperativa.data[k];
                }
                modifica.codice_iscrizione_libro_soci = item[0].numero;
                modifica.data_iscrizione_libro_soci = item[0].data ? item[0].data : new Date(1900, 0, 1, 0, 0, 0, 0); 
                this.cooperativeFormGroup.get('impresaPadre').patchValue(cooperativePadre);
            break;
            case HttpAction.REMOVE:
                cooperativePadre = cooperativePadre.filter(impresa => impresa.partitaIva != item[0].codcooperativa);
                this.cooperativeFormGroup.get('impresaPadre').patchValue(cooperativePadre);
            break;
        }
        if(cooperativePadre.length != 1) {
            this.contattiRootService.singoloPadreSource.next(false);
            this.contattiRootService.setSalvaInPadre(false);
        } else {
            this.contattiRootService.singoloPadreSource.next(true)
        }
        this.gridPubService.refresh(true);
        return of([]);
    }


    GetModel(rows: CooperativeKendoGridRow[]) {

        this.columns = [
            new KendoGridColumn(
                { field: 'codcooperativa', title: this.transloco.translate('CooperativaReferente', {}) },
                { validators: [Validators.required], width: 135}
            ),
            new KendoGridColumn(
                { field: 'numero', title: this.transloco.translate('NumeroLibroSoci', {}) },
                { /*validators: [Validators.required],*/ width: 135}
            ),
            new KendoGridColumn(
              { field: 'data', title: this.transloco.translate('DataIscrizioneLibroSoci', {}) },
              { /*validators: [Validators.required],*/ width: 135, date: new DateSettings({ defaultValue: AGRODATAINIZIO })}
          )
        ];
        this.setTipologiaDDL(this.columns[0]);

        this.model = {
            codcooperativa: new ModelEntry(CELL_TYPES.DROPDOWNLIST),
            cooperativa: new ModelEntry(CELL_TYPES.STRING, false),
            numero: new ModelEntry(CELL_TYPES.STRING),
            data: new ModelEntry(CELL_TYPES.DATE)
        }

        //this.makeCellsUneditable();

        return new CooperativeKendoServerResult(this.model, this.columns, rows);
    }

    setTipologiaDDL(col: KendoGridColumn) {
        let list: DropdownListItem[] = [];
        col.ddl = new DropdownListWithForm('partitaIva', 'codcooperativa', 'ragioneSociale', list, null);
        col.ddl.valuePrimitive = false;
        col.ddl.loadOnEdit = true;
        col.ddl.descriptionField = "cooperativa";
        col.ddl.loadFunction = this.caricaPadri.bind(this);
        
    }

    caricaPadri(selezionata: any){
        let cooperativePadre: ImpresaPadre[] = this.cooperativeFormGroup.get('impresaPadre').value;
        if(selezionata.codcooperativa){
            cooperativePadre = cooperativePadre.filter(impresa => impresa.partitaIva != selezionata.codcooperativa.id)
        }
        return of(this.padri.filter(a => !cooperativePadre.map(b=>b.partitaIva).includes(a.partitaIva)));
    }

    /*handleDropdowns(): void {
        let data;
        let col;
        // Centro
        col = this.columns.find(s => s.field === 'Sa_Cod');
        col.ddl = new DropdownListWithForm('VisibilitaPubblica', 'Sa_Cod', 'descrizione', [], new DropdownListItem(0, ''));
        col.ddl.valuePrimitive = false;
        col.validators = [Validators.required];
        col.ddl.id = 'codice';
        col.ddl.formControlValue = 'descrizione';
        col.ddl.loadOnEdit = true;
        col.ddl.descriptionField = 'Visibilita';
        col.ddl.loadFunction = this.impresePadre.bind(this);

        col = this.columns.find(s => s.field === 'CLASS_CODE');
        data = this.tipo.map(cod => new DropdownListItem(cod.CLASS_CODE, cod.CLASS_DESC));
        col.ddl = new DropdownListWithForm('Tipologia', 'CLASS_CODE', 'tipologia', data);
        col.ddl.valuePrimitive = true;
        col.validators = [Validators.required];

        col = this.kendoColumns.find(s => s.field === 'Ditta_Cod');
        data = this.marche.map(cod => new DropdownListItem(cod.codice, cod.descrizione));
        col.ddl = new DropdownListWithForm('Marche', 'Ditta_Cod', 'Ditta_Des', data);
        col.ddl.valuePrimitive = true;

    }*/

    setPadri(padri: any){
        this.padri = padri;
    }

    getPadriCount() {
        return this.padri.length
    }
}

