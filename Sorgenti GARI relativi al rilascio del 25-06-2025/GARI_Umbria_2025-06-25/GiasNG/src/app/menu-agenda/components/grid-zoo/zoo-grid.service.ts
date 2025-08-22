/* eslint-disable radix */
import { Injectable, Injector, Renderer2 } from '@angular/core';
import { MenuContestualeService } from 'app/Master/menu-contestuale/menu-contestuale.service';
import { enum_Security_Attivita } from 'app/Model/TipiEnumerativi';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import {MasterService, RispostaStandard} from 'app/Service/master.service';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { isNullOrUndefined } from 'app/Service/utils';
import { EditingMode, GridInfoCommandEvent, LoaderType, RendererGridEvent } from 'gias-kendo-grid';
import { AbstractGridConfigService, HttpAction } from 'gias-kendo-grid';
import { UtilityFunctions } from 'app/Utility/UtilityFunctions';
import { map, Observable, skip, takeUntil, switchMap, throwError, take, of } from 'rxjs';
import { BussinessMenuAgendaService } from '../../shared_services/bussiness-logic.service';
import { MenuAgendaDataStore } from '../../shared_services/menu-agenda-datastore.service';
import { FiltersService } from '../filters/filters.service';
import { Gias2010Redirector } from '../grid-qdc/Gias2010Redirector.service';
import {construisciChiaviComposite, GridZooResult, lav_cod_copiabili, Link_ElimOpMultipla, ZooRow} from '../utils';
import { GridZooComponent } from './zoo.component';
import { AjaxAgronicaAPIService } from 'app/Service/ajax-agronica.api.service';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import {CaricaZoo} from "../models";
import { CustomColumnSettings } from 'gias-kendo-grid';

const zooLink = 'Agenda/CaricaZoo';

function operationIsBlocked(riga: ZooRow[]) {
    let anyOpBlocked = riga.some((row) => row.chiave_composita.split("_")[5] === '1');
    return anyOpBlocked;
}


@Injectable()
export class ZooGridService extends AbstractGridConfigService<GridZooResult>  {
    editingMode: EditingMode = EditingMode.IN_PAGE;
    loader: LoaderType = LoaderType.SERVICE;
    rowId = "chiave";
    gridId = "RicetteGridConfig";
    applicaFiltri: boolean;

    private component: GridZooComponent;

    private permessoZoo_W: boolean;

    constructor(
        injector: Injector,
        private filters: FiltersService,
        private dialogService: GiasDialogService,
        private menu: MenuContestualeService,
        private dataStore: MenuAgendaDataStore,
        private redirector: Gias2010Redirector,
        private renderer: Renderer2,
        private bizz: BussinessMenuAgendaService,
        private permessiUtenteService: PermessiUtenteService,
        private ajaxAgronicaAPIService: AjaxAgronicaAPIService,
        private objParametriAgendaService: ObjParametriAgendaService,
        private masterService: MasterService
    ) {
        super(injector);

        this.customColumn = new CustomColumnSettings({showColumn: true,title: this.transloco.translate("Comandi"),useCustomColumnCellTemplate: true});

        this.applyGridSettings();

        this.handleFilters();

        this.permessoZoo_W = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Gest_Stalle, 2);
    }

    get warningDialogRef() {
        return this.component.warningDialogRef;
    }
    set warningDialogMsg(val: string) {
        this.component.warningDialogMsg = val;
    }

    public SetComponentRef(component: GridZooComponent) {
        this.component = component;
    }

    override applyRendererRules(opts: RendererGridEvent): void {
        const { grid, gridElRef } = { ...opts };
        let rows: [] = grid.data['data'];
        let domElems = gridElRef.nativeElement.querySelectorAll('tbody tr');

        this.nascondiPulsantiZoo(this.renderer, domElems, rows);
    }

    public read(options?: any): Observable<GridZooResult> {
        const data = this.dataStore.gridDataZoo;
        if(!isNullOrUndefined(data) && !this.applicaFiltri && !this.menu.pendingWorkDone) {
            return of(data);
        }
        this.applicaFiltri = false;


        const params = (...args) => {
            const filters = this.filters.getFiltriZoo();
            let agendaService = args[0];
            const master = args[1];

            let agenda = agendaService.getObjParamValue();
            return {
                filtro: JSON.stringify({
                    TipoGriglia: '2',
                    ...filters
                }),
                piva: agenda.Piva,
                objParam_server: master.ObjParametri_Server,
                objParam_utenti: master.ObjParametri_Utenti
            };
        };

        const params_NG = (...args): CaricaZoo => {
            const filters = this.filters.getFiltriZoo();
            let agendaService = args[0];
            const master = args[1];

            let agenda = agendaService.getObjParamValue();

            const obj = <CaricaZoo>{
              filtro: JSON.stringify({
                TipoGriglia: '2',
                ...filters
              }),
              piva: agenda.Piva,
              variabiliInSessione_NG: this.masterService.variabiliInSessione
            };

            return obj;
        };

        this.isLoading(true);
        return this.ajaxAgronicaAPIService.ajaxAPIPost<any, any>(zooLink, params_NG(this.objParametriAgendaService, this.masterService))
            .pipe(map(r => {
                let result = r.RispostaStringa;
                this.dataStore.gridDataZoo =  {
                    model: result.kendo_model,
                    columns: result.kendo_columns,
                    rows: result.kendo_rows
                };
                this.isLoading(false);

                this.menu.markPendingWorkAsDone();
                return this.dataStore.gridDataZoo;
            }));
    }

    public perform(actionType: HttpAction, items: ZooRow[] | ZooRow): Observable<any[]> {
        let daCancellare = this.getArrayFrom(items);

        if(actionType === HttpAction.REMOVE) {
            return this.deleteItem(daCancellare);
        }
    }

    public nascondiPulsantiZoo(renderer: any, domElems: any[], elems: any[]) {
        elems.forEach((dataItem: ZooRow, index: number) => {
            let currDomRow = domElems[index];

            UtilityFunctions.setStyle(renderer, currDomRow, '.editBtn', 'display', (this.permessoZoo_W ? 'block' : 'none'));
            UtilityFunctions.setStyle(renderer, currDomRow, '#btnDelete', 'display', (this.permessoZoo_W ? 'block' : 'none'));
            UtilityFunctions.setStyle(renderer, currDomRow, '.btnCopia', 'display', (this.permessoZoo_W ? 'block' : 'none'));

            if (this.bizz.SenzaPermessoDiModifica(dataItem) || this.bizz.OperazioneBloccata(dataItem)) {
                UtilityFunctions.setStyle(renderer, currDomRow, '.editBtn', 'display', 'none');
                UtilityFunctions.setStyle(renderer, currDomRow, '#btnDelete', 'display', 'none');
            }

            //Se il tipo di operazione non è tra le editabili, nascondo il modifica
            if (this.bizz.lavcodNonEditabile(dataItem)) {
                UtilityFunctions.setStyle(renderer, currDomRow, '.editBtn', 'display', 'none');
            }

            //Se il tipo di operazione non è tra le copiabili, nascondo il copia
            if (lav_cod_copiabili.indexOf(parseInt(dataItem.Lav_cod)) < 0) {
                UtilityFunctions.setStyle(renderer, currDomRow, '.btnCopia', 'display', 'none');
            }
        });
    }

    private deleteItem(items: ZooRow[]): Observable<any[]> {
        if (operationIsBlocked(items)) {
            this.dialogService.baseError('', "OpBloccata");
            return throwError(() => new Error());
        }

        return this.deleteZoo(items, false);
    }

    private deleteZoo(items: ZooRow[], proseguiInCasoDiAlert: boolean): Observable<any[]> {

        const params = (...args) => {
            const master = args[1];

            let chiavi = construisciChiaviComposite(items);
            return {
                strChiaviComposite: 'del_elem|' + chiavi,
                proseguiInCasoDiAlert: proseguiInCasoDiAlert
            };
        };

        return this.ajaxAgronicaAPIService.ajaxAPIPost(Link_ElimOpMultipla, params).pipe(switchMap((risposta: RispostaStandard) => {
            if (risposta.RispostaOK) {
                this.dialogService.baseSuccess('', risposta.RispostaStringa, false);
                this.dataStore.resetGridData();
                this.gridPublicService.refresh(true);
            } else {
                if(risposta.RispostaConferma) {
                    this.warningDialogMsg = risposta.Errore;
                    return this.dialogService.baseWarning('', risposta.Errore, false);
                }
            }

            return [];
        }), switchMap((result) => {
            if(result['returnObj'])
                return this.deleteZoo(items, true);
            return of([]);
        }));

        /*return this.http.post2(Link_ElimOpMultipla, params, true).pipe(switchMap((risposta: RispostaStandard) => {
            if (risposta.RispostaOK) {
                this.dialogService.baseSuccess('', risposta.RispostaStringa, false);
                this.dataStore.resetGridData();
                this.gridPublicService.refresh(true);
            } else {
                if(risposta.RispostaConferma) {
                    this.warningDialogMsg = risposta.Errore;
                    return this.dialogService.baseWarning('', risposta.Errore, false);
                }
            }

            return [];
        }), switchMap((result) => {
            if(result['returnObj'])
                return this.deleteZoo(items, true);
            return of([]);
        }));*/
    }

    private handleFilters() {

        this.gridPublicService.changeDetected.pipe(takeUntil(this.signal))
            .subscribe((event: GridInfoCommandEvent) => {
                if(this.editBtnIsHidden(event.dataItem) && event.action === 'edit')
                    return;

                switch(event.action) {
                    case 'info':
                    case 'edit':
                        this.redirector.reindirizzaAiDettagli(event.action,event.dataItem);
                    break;
                }
            });

        this.filters.subscribeToFiltersChange().pipe(takeUntil(this.signal), skip(1)).subscribe(() => {
            this.applicaFiltri = true;
            this.gridPublicService.refresh(true);
        });
    }

    private editBtnIsHidden(dataItem: ZooRow) {
        return this.bizz.lavcodNonEditabile(dataItem)
            || this.bizz.SenzaPermessoDiModifica(dataItem);
    }

    private applyGridSettings() {
        this.behavior.showDeletionConfirmation = true;
    }

    private getArrayFrom(items: ZooRow | ZooRow[]): ZooRow[] {
        if(!Array.isArray(items)) {
            return [items];
        }
        return items;
    }

}

