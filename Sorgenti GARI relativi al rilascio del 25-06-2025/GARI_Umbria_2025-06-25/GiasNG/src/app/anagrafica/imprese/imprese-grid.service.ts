/* eslint-disable */
import { Inject, Injectable, Injector, Renderer2 } from '@angular/core';
import {
  AggregateSettings,
  CommandsColumnSettings,
  CommandsDropDownEvents,
  CommandsDropDownSettings,
  RemoveMultipleRowsParams,
  ToolbarSettings
} from 'gias-kendo-grid';
import { CELL_TYPES } from 'gias-ui-kit';
import {
  DropdownListItem,
  DropdownListWithForm, EditingMode,
  KendoGridColumn, LoaderType, ModelEntry, RendererGridEvent
} from 'gias-kendo-grid';
import { AbstractGridConfigService, HttpAction } from 'gias-kendo-grid';
import { from, Observable, of } from 'rxjs';
import { ImpresaKendoServerResult, ImpresaModel } from './imprese.model';
import { ConfigTemplate } from 'gias-kendo-grid';
import { catchError, filter, map, switchMap, take, takeUntil, tap } from 'rxjs/operators';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { GiasIstatService, ObjParametriAgenda } from 'gias-ui-kit';
import { MasterService } from 'app/Service/master.service';
import { AGRODATAFINE, AGRODATAINIZIO, COD_FORNITORE, SMARTPHONE_WIDTH } from 'app/Model/CostantiPersonalizzate';
import { ImpresaEditService } from './impresa-edit/impresa-edit.service';
import { Impresa } from 'app/Model/anagrafiche/Impresa';
import { PivaValidatorService } from './piva-validator.service';
import { RowClassArgs } from '@progress/kendo-angular-grid';
import { DeleteMessageService } from 'app/Service/delete-message.service';
import { enum_CodiciAnagrafe, enum_Security_Attivita } from 'app/Model/TipiEnumerativi';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { GiasMessageService } from 'app/Service/gias-message.service';
import { AnagraficaService } from '../anagrafica.service';
import { TranslocoService } from '@jsverse/transloco';
import { FormGroup, Validators } from '@angular/forms';
import { CodiciAnagrafeValori } from 'app/Model/anagrafiche/CodiciAnagrafeValori';
import { ImpreseFactoryService, IMPRESE_SERVICE_TOKEN } from 'app/Service/ServiceFactory/imprese.factory.service';
import { RisorseUmane } from 'app/Model/anagrafiche/RisorseUmane';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import { RapportoContabile } from "./impresa-edit/contatti-edit/contatti-edit.service";
import { ConfigurazioneSitiService } from '../../Service/configurazione-siti.service';
import { GruppoRaccolta } from "../../Model/metaschema/GruppoRaccolta";
import { GridCommandItem } from "../../menu-agenda/components/utils";
import { AnagraficaBusinessLogicService } from "../services/anagrafica-business-logic.service";
import { GruppiRaccoltaService } from "../../Service/GruppiRaccolta/gruppi-raccolta.service";
import { Contatto } from 'app/Model/anagrafiche/Contatto';
import { ImpresaPadre } from 'app/Model/anagrafiche/ImpresaPadre';
import { IntervalloTemporale } from '../../Model/anagrafiche/IntervalloTemporale';
import { BudgetService } from 'app/Service/Budget/budget.service';

@Injectable()
export class ImpreseGridHttpService extends AbstractGridConfigService<ImpresaKendoServerResult> {
  gridId = 'ImpreseGridHttpService';
  loader = LoaderType.SERVICE;
  editingMode = EditingMode.IN_LINE;
  rowId = 'chiave';
  cmdColumn = new CommandsColumnSettings({ editBtn: false, infoBtn: false, removeBtn: false, onDisableInfoBtn: () => false });

  aggregates = new AggregateSettings({
    enabled: true,
    descriptors: [
      { field: 'Superficie_Catastale', aggregate: 'sum', format: 'n4' },
      { field: 'Superficie_Convenzionale', aggregate: 'sum', format: 'n4' },
      { field: 'Superficie_Conversione', aggregate: 'sum', format: 'n4' },
      { field: 'Superficie_Biologico', aggregate: 'sum', format: 'n4' },
      { field: 'Superficie_Totale', aggregate: 'sum', format: 'n4' }
    ]
  });

  objParametriAgenda: ObjParametriAgenda;

  originalEditedLine: any;
  // toolbar = new ToolbarSettings(true, true);
  // views = new GridCustomizations({enabled: true});

  kendoColumns: KendoGridColumn[] = [
    //new KendoGridColumn({ field: 'chiave', title: 'chiave' }, { resizable: true, editable: false } ),
    //new KendoGridColumn({ field: 'Num_Padri', title: 'N. Coop. Referenti' }, { resizable: true, editable: true }),
    new KendoGridColumn({ field: 'rag_soc', title: this.translocoService.translate('RagioneSociale') }, { resizable: true, editable: true, width: 250, validators: [Validators.required] }),
    new KendoGridColumn({ field: 'piva', title: this.translocoService.translate('PartitaIVA') }, { resizable: true, editable: true, width: 140 }), //validators: [this.pivaValidatorService.PivaValidator.bind(this)]
    new KendoGridColumn({ field: 'Codice_Cuaa', title: this.translocoService.translate('CodiceUnicoAziendaAgricolaSigla') }, { resizable: true, editable: true, width: 140 }),
    new KendoGridColumn({ field: 'Codice_Socio', title: this.translocoService.translate('CodiceSocio') }, { resizable: true, editable: true, width: 140 }),
    new KendoGridColumn({ field: 'Piva_Padre', title: this.translocoService.translate('CooperativaReferente') }, { resizable: true, editable: true, width: 250 }),
    new KendoGridColumn({ field: 'codice_iscrizione_libro_soci', title: this.translocoService.translate('NumeroLibroSoci') }, { resizable: true, editable: true, width: 140 }),
    new KendoGridColumn({ field: 'data_iscrizione_libro_soci', title: this.translocoService.translate('DataIscrizioneLibroSoci') }, { resizable: true, editable: true, width: 140, date: { defaultValue: AGRODATAFINE } }),
    //new KendoGridColumn({ field: 'Stato', title: 'Stato' }, { resizable: true, editable: true }),
    new KendoGridColumn({ field: 'Stato_Cod', title: this.translocoService.translate('Stato') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 80 }),
    new KendoGridColumn({ field: 'Pro_Cod_Istat', title: this.translocoService.translate('Provincia') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 105 }),
    //new KendoGridColumn({ field: 'Prov', title: 'Prov' }, { resizable: true, editable: true }),
    new KendoGridColumn({ field: 'Com_Cod_Istat', title: this.translocoService.translate('Comune') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 175 }),
    //new KendoGridColumn({ field: 'Com', title: 'Com' }, { resizable: true, editable: true }),

    new KendoGridColumn({ field: 'frz_des', title: this.translocoService.translate('Frazione') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 100 }),
    new KendoGridColumn({ field: 'ind_des', title: this.translocoService.translate('Indirizzo') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 205 }),
    new KendoGridColumn({ field: 'Cap', title: this.translocoService.translate('CAP') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 80 }),
    new KendoGridColumn({ field: 'GruppoRaccolta_Cod', title: this.translocoService.translate('GruppoRaccolta') }, { resizable: true, editable: true, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 80 }),

    new KendoGridColumn({ field: 'Superficie_Catastale', title: this.translocoService.translate('SuperficieCatastaleAbbr') }, { resizable: true, editable: false, width: 135, format: 'n4', numeric: { defaultValue: 0, min: 0 }, }),
    new KendoGridColumn({ field: 'Superficie_Convenzionale', title: this.translocoService.translate('SuperficieConvenzionaleAbbr') }, { resizable: true, editable: false, width: 135, format: 'n4', numeric: { defaultValue: 0, min: 0, format: 'n4' }, }),
    new KendoGridColumn({ field: 'Superficie_Conversione', title: this.translocoService.translate('SuperficieConversioneAbbr') }, { resizable: true, editable: false, width: 135, format: 'n4', numeric: { defaultValue: 0, min: 0, format: 'n4' }, }),
    new KendoGridColumn({ field: 'Superficie_Biologico', title: this.translocoService.translate('SuperficieBiologicoAbbr') }, { resizable: true, editable: false, width: 135, format: 'n4', numeric: { defaultValue: 0, min: 0, format: 'n4' }, }),
    new KendoGridColumn({ field: 'Superficie_Totale', title: this.translocoService.translate('SuperficieTotaleAbbr') }, { resizable: true, editable: false, width: 135, format: 'n4', numeric: { defaultValue: 0, min: 0, format: 'n4' }, }),
    // new KendoGridColumn({ field: 'Contratto_Produzione', title: 'Contratto Produzione' }, { resizable: true, editable: false }),
    // //new KendoGridColumn({ field: 'Indirizzo', title: 'Indirizzo' }, { resizable: true, editable: true }),
    // new KendoGridColumn({ field: 'Tecnico_Referente', title: 'Tecnico Referente' }, { resizable: true, editable: false}),
    // new KendoGridColumn({ field: 'Cooperativa_Referente', title: 'Cooperativa Referente' }, { resizable: true, editable: false}),

    //new KendoGridColumn({ field: 'Codice_Fiscale', title: 'C. F.' }, { resizable: true, editable: true }),

    new KendoGridColumn({ field: 'Validita_Inizio', title: this.translocoService.translate('Validita_Inizio') }, { resizable: true, editable: true, date: { defaultValue: AGRODATAINIZIO }, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 140 }),
    new KendoGridColumn({ field: 'Validita_Fine', title: this.translocoService.translate('Validita_Fine') }, { resizable: true, editable: true, date: { defaultValue: AGRODATAFINE }, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 140 }),
    new KendoGridColumn({ field: 'Data_Creazione', title: this.translocoService.translate('DataCreazione') }, { resizable: true, editable: false, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 140 }),
    new KendoGridColumn({ field: 'Utente_Creazione', title: this.translocoService.translate('UtenteCreazione') }, { resizable: true, editable: false, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 145 }),
    new KendoGridColumn({ field: 'Data_Modifica', title: this.translocoService.translate('DataModifica') }, { resizable: true, editable: false, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 130 }),
    new KendoGridColumn({ field: 'Utente_Modifica', title: this.translocoService.translate('UtenteModifica') }, { resizable: true, editable: false, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 135 }),
    new KendoGridColumn({ field: 'Attivo', title: this.translocoService.translate('Attivo') }, { resizable: true, editable: false, media: '(min-width: ' + SMARTPHONE_WIDTH + 'px)', width: 85 }),
    new KendoGridColumn({ field: 'IndirizzoCompleto', title: this.translocoService.translate('Indirizzo') }, { resizable: true, editable: false, media: '(max-width: ' + SMARTPHONE_WIDTH + 'px)', width: 250 }),
  ];

  kendoModel: ImpresaModel = {
    chiave: new ModelEntry(CELL_TYPES.STRING, false),
    rag_soc: new ModelEntry(CELL_TYPES.STRING, true),
    piva: new ModelEntry(CELL_TYPES.STRING, true),
    Codice_Fiscale: new ModelEntry(CELL_TYPES.STRING, true),
    Codice_Cuaa: new ModelEntry(CELL_TYPES.STRING, true),
    Codice_Socio: new ModelEntry(CELL_TYPES.STRING, true),
    Contratto_Produzione: new ModelEntry(CELL_TYPES.STRING, false),
    Indirizzo: new ModelEntry(CELL_TYPES.STRING, false),
    Tecnico_Referente: new ModelEntry(CELL_TYPES.STRING, false),
    Cooperativa_Referente: new ModelEntry(CELL_TYPES.STRING, false),
    codice_iscrizione_libro_soci: new ModelEntry(CELL_TYPES.STRING, true),
    data_iscrizione_libro_soci: new ModelEntry(CELL_TYPES.DATE, true),
    Superficie_Totale: new ModelEntry(CELL_TYPES.NUMBER, false),
    Superficie_Tare: new ModelEntry(CELL_TYPES.NUMBER, false),
    SAU_Totale: new ModelEntry(CELL_TYPES.NUMBER, false),
    Pro_Cod_Istat: new ModelEntry(CELL_TYPES.DROPDOWNLIST, true),
    Prov: new ModelEntry(CELL_TYPES.STRING, true),
    Com_Cod_Istat: new ModelEntry(CELL_TYPES.DROPDOWNLIST, true),
    Com: new ModelEntry(CELL_TYPES.STRING, true),
    Stato: new ModelEntry(CELL_TYPES.STRING, false),
    Stato_Cod: new ModelEntry(CELL_TYPES.DROPDOWNLIST, true),
    frz_des: new ModelEntry(CELL_TYPES.STRING, true),
    ind_des: new ModelEntry(CELL_TYPES.STRING, true),
    Cap: new ModelEntry(CELL_TYPES.STRING, true),
    Piva_Padre: new ModelEntry(CELL_TYPES.DROPDOWNLIST, true),
    Rag_Soc_Padre: new ModelEntry(CELL_TYPES.STRING, true),
    GruppoRaccolta_Cod: new ModelEntry(CELL_TYPES.DROPDOWNLIST, true),
    GruppoRaccolta_Des: new ModelEntry(CELL_TYPES.STRING, true),
    Num_Padri: new ModelEntry(CELL_TYPES.NUMBER, false),
    Superficie_Catastale: new ModelEntry(CELL_TYPES.NUMBER, false),
    Superficie_Convenzionale: new ModelEntry(CELL_TYPES.NUMBER, false),
    Superficie_Conversione: new ModelEntry(CELL_TYPES.NUMBER, false),
    Superficie_Biologico: new ModelEntry(CELL_TYPES.NUMBER, false),
    Validita_Inizio: new ModelEntry(CELL_TYPES.DATE, true),
    Validita_Fine: new ModelEntry(CELL_TYPES.DATE, true),
    Data_Creazione: new ModelEntry(CELL_TYPES.DATETIME, false),
    Data_Modifica: new ModelEntry(CELL_TYPES.DATETIME, false),
    Utente_Creazione: new ModelEntry(CELL_TYPES.STRING, false),
    Utente_Modifica: new ModelEntry(CELL_TYPES.STRING, false),
    Attivo: new ModelEntry(CELL_TYPES.DROPDOWNLIST, false),
    IndirizzoCompleto: new ModelEntry(CELL_TYPES.STRING, false),
    Provincia: new ModelEntry(CELL_TYPES.STRING, true)
  };

  public permessoEdit: boolean;
  public permessoRemove: boolean;
  public permessoInfo: boolean;
  private permessoNuovoDocumento: boolean;
  private permessoRicercaDocumenti: boolean;

  constructor(
    injector: Injector,
    @Inject(IMPRESE_SERVICE_TOKEN) private impreseService: ImpreseFactoryService,
    private impreseEditService: ImpresaEditService,
    private objParametriAgendaService: ObjParametriAgendaService,
    private istatService: GiasIstatService,
    private masterService: MasterService,
    private pivaValidatorService: PivaValidatorService,
    private renderer: Renderer2,
    private router: Router,
    private deleteMessageService: DeleteMessageService,
    private giasMessageService: GiasMessageService,
    private anagraficaService: AnagraficaService,
    private route: ActivatedRoute,
    private permessiUtenteService: PermessiUtenteService,
    private translocoService: TranslocoService,
    private budgetService: BudgetService,
    private giasDialogService: GiasDialogService,
    private gruppiRaccoltaService: GruppiRaccoltaService,
    private configurazioneSitiService: ConfigurazioneSitiService,
    private businessLogic: AnagraficaBusinessLogicService
  ) {
    super(injector, ConfigTemplate.DefaultTemplate);
    this.permessoEdit = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Anagrafica_Impresa, 2) && this.budgetService.getBudget().activeBudget == false;
    this.permessoRemove = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Anagrafica_Impresa, 2) && this.budgetService.getBudget().activeBudget == false;
    this.permessoInfo = this.permessiUtenteService.getPermesso(enum_Security_Attivita.Anagrafica_Impresa, 0);
    this.permessoRicercaDocumenti = this.permessiUtenteService.canReadPermesso(enum_Security_Attivita.Documentale_Lista);
    this.permessoNuovoDocumento = this.permessiUtenteService.canWritePermesso(enum_Security_Attivita.Documentale_Inser);

    this.handleCustomizations();
    this.handleEdits();

    // this.store.select('imprese').subscribe((data) => {
    //     this.impreseState = data;
    //     this.read();
    // });

    // this.store.dispatch(new ImpreseActions.LoadImprese());
    this.objParametriAgenda = this.budgetService.getBudget() ? this.budgetService.getParametriObjService().getObjParamValue() : this.objParametriAgendaService.getObjParamValue();

    this.pagination.gridState.sort = [
      { field: 'Selected', dir: 'desc' },
      { field: 'rag_soc', dir: 'asc' }
    ];
  }

  read(): Observable<ImpresaKendoServerResult> {
    let data = this.anagraficaService.filterData.getValue()
    let DataFiltro = AGRODATAINIZIO;
    if (data.filter) {
      DataFiltro = data.data;
    }
    this.loadingService.set_isLoading({ isLoading: true, message: '', component: this.gridPublicService.gridElRef })
    return this.impreseService.leggiImprese(this.impreseService.getCaricaTutteImprese() ? "" : this.objParametriAgenda.Piva, DataFiltro).pipe(
      catchError((err) => {
        this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef });
        return of()
      }),
      map((data) => {
        this.handleDropdowns();

        data.kendo_rows.forEach((val: any) => val.Selected = false);
        let impresaSelezionata: any = data.kendo_rows.find((el: any) => el.piva == this.objParametriAgenda.Piva);
        if (impresaSelezionata != undefined) {
          impresaSelezionata.Selected = true;
        }
        data.kendo_rows.forEach((val: any) => {
          if (val.Stato_Cod == 'IT') {
            val.IndirizzoCompleto = val.ind_des + ' - ' + val.Com + ' (' + val.Prov + ')' + ' CAP:' + val.Cap;
          } else {
            val.IndirizzoCompleto = val.ind_des + ' (' + val.frz_des + ') ZIP: ' + val.Cap;
          }
        });

        const result = new ImpresaKendoServerResult(this.kendoModel, this.kendoColumns, data.kendo_rows);
        this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef })
        return result;

      }));
  };

  override applyRendererRules(opts: RendererGridEvent): void {
    const { grid, gridElRef } = { ...opts };
    let visibleRows: [] = gridElRef.nativeElement.querySelectorAll('tbody tr');
    grid.view.forEach((el, index) => {
      if (el.Attivo === 0) {
        this.renderer.addClass(visibleRows[index], 'nonAttivo');
      }
      if (el.Attivo === 1) {
        this.renderer.removeClass(visibleRows[index], 'nonAttivo');
      }
    });
    //this.anagraficaService.filterData.next(this.anagraficaService.filterData.getValue());
  }

  perform(actionType: HttpAction, items: any): Observable<any> {
    const item = items as any;
    let impresaBase: Impresa;
    let objP = new ObjParametriAgenda();
    objP.Piva = item.piva;
    switch (actionType) {
      case HttpAction.CREATE:
        impresaBase = this.impreseEditService.GetFormGroupImpresa().getRawValue();
        impresaBase.partitaIva = item.piva;
        return of(1).pipe(
          switchMap(() => {
            return this.configurazioneSitiService.leggiChiave('Blocco_Inserimento_PIVA_Impresa')
          }),
          switchMap((conf) => {
            if (item.piva == "") {
              if (conf?.Valore?.toLowerCase() === 'true') {
                return of({ returnObj: true });
              } else {
                return this.giasDialogService.dialogMessageObs_Result(
                  this.translocoService.translate("Attenzione"),
                  this.translocoService.translate("PIVAVuotaConferma")
                );
              }
            } else {
              return of({ returnObj: true });
            }
          }),
          switchMap((resp: any) => {
            if (resp.returnObj == true) {
              let risorsaUmanaBase = new RisorseUmane
              risorsaUmanaBase.codice = 0;
              risorsaUmanaBase.rapportoContabile = new RapportoContabile(COD_FORNITORE);
              let risorseUmane = [risorsaUmanaBase]
              impresaBase.contatto_superuser = new Contatto
              impresaBase.contatto_superuser.risorseUmane = risorseUmane
              impresaBase.indirizzi[0].indirizzo.codice = 0;
              this.fillImpresaBase(impresaBase, item);
              let impresaPadre = new ImpresaPadre();
              impresaPadre.partitaIva = item.Piva_Padre;
              impresaPadre.ragioneSociale = item.Rag_Soc_Padre;
              impresaPadre.codice_iscrizione_libro_soci = item.codice_iscrizione_libro_soci;
              impresaPadre.data_iscrizione_libro_soci = item.data_iscrizione_libro_soci;
              impresaBase.impresaPadre = [impresaPadre];
              this.loadingService.set_isLoading({ isLoading: true, message: '', component: this.gridPublicService.gridElRef });
              return this.impreseService.ScriviImpresa(impresaBase, false).pipe(
                switchMap((r) => {
                  if (r.RispostaOK) {
                    this.giasMessageService.successMessage(this.translocoService.translate('Impresa') + ': ' + impresaBase.ragioneSociale + ' ' + this.translocoService.translate('CreataCorrettamente'));
                    let objP = this.objParametriAgendaService.getObjParamValue();
                    objP.Piva = r.RispostaStringa.partitaIva;
                    objP.RagSoc = r.RispostaStringa.ragioneSociale;
                    this.objParametriAgendaService.changeObjParametriAgenda(objP);
                    this.gridPublicService.refresh();
                    this.gridPublicService.refresh();
                  }
                  return of(1);
                }));
            } else {
              return of(false);
            }
          })
        );
      case HttpAction.REMOVE:
        this.loadingService.set_isLoading({ isLoading: true, message: '', component: this.gridPublicService.gridElRef });
        this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
        this.objParametriAgenda.Piva = ''
        this.objParametriAgendaService.changeObjParametriAgenda(this.objParametriAgenda);

        return this.impreseService.leggiImpresaObservable(objP).pipe(
          switchMap((r) => {
            impresaBase = r.RispostaStringa;
            impresaBase.flag_cancellazione = true;
            return this.impreseService.ScriviImpresa(impresaBase, false).pipe(
              tap((r) => {
                if (r.RispostaOK) {
                  this.deleteMessageService.impreseDeleteMsg_Succ(r);
                  let objP = this.objParametriAgendaService.getObjParamValue();
                  objP.Piva = "";
                  objP.RagSoc = "";
                  this.objParametriAgendaService.changeObjParametriAgenda(objP);
                  this.gridPublicService.refresh();
                } else {
                  this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef });
                  this.deleteMessageService.impreseDeleteMsg_Fail(r);
                }
              }), switchMap(() => of([])))
          })
        );
      case HttpAction.UPDATE:
        this.loadingService.set_isLoading({ isLoading: true, message: '', component: this.gridPublicService.gridElRef });
        return from(this.impreseService.leggiImpresa(objP)).pipe(
          switchMap((r) => {
            impresaBase = r.RispostaStringa;
            this.fillImpresaBase(impresaBase, item);
            if (this.originalEditedLine.Piva_Padre != item.Piva_Padre) {
              if (item.Piva_Padre == '') {
                if (impresaBase.impresaPadre.length > 1) {
                  const index = impresaBase.impresaPadre.findIndex((el: Impresa) => { return el.partitaIva == this.originalEditedLine.Piva_Padre });
                  if (index > -1) {
                    impresaBase.impresaPadre.splice(index, 1);
                  }
                } else {
                  this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef });
                  this.giasMessageService.errorMessage(this.translocoService.translate('ErroreSelezionaCooperativa'));
                }
              } else {
                const index = impresaBase.impresaPadre.findIndex((el: Impresa) => el.partitaIva == this.originalEditedLine.Piva_Padre);

                if (index > -1) {
                  impresaBase.impresaPadre.splice(index, 1);
                }
                let impresaPadre = new ImpresaPadre();
                impresaPadre.partitaIva = item.Piva_Padre;
                impresaPadre.ragioneSociale = item.Rag_Soc_Padre;
                impresaPadre.codice_iscrizione_libro_soci = item.codice_iscrizione_libro_soci == this.originalEditedLine.codice_iscrizione_libro_soci ? "" : item.codice_iscrizione_libro_soci;
                impresaPadre.data_iscrizione_libro_soci = item.data_iscrizione_libro_soci == this.originalEditedLine.data_iscrizione_libro_soci ? AGRODATAINIZIO : item.data_iscrizione_libro_soci;
                impresaBase.impresaPadre.push(impresaPadre);
              }
            }

            return this.impreseService.ScriviImpresa(impresaBase, false).pipe(
              map(r => r.RispostaStringa),
              tap((impresaResp) => {
                this.giasMessageService.successMessage(this.translocoService.translate('Impresa') + ': ' + impresaBase.ragioneSociale + ' ' + this.translocoService.translate('ModificataCorrettamente'))
                let objP = this.objParametriAgendaService.getObjParamValue();
                objP.Piva = impresaResp.partitaIva;
                objP.RagSoc = impresaResp.ragioneSociale;
                this.objParametriAgendaService.changeObjParametriAgenda(objP);
                this.gridPublicService.refresh();
              }))
          }));
    }
    return this.read() as any;
  }

  private fillImpresaBase(impresaBase: Impresa, item: any) {
    impresaBase.ragioneSociale = item.rag_soc;
    impresaBase.CUAA = item.Codice_Cuaa;
    impresaBase.validita.inizio = item.Validita_Inizio;
    impresaBase.validita.fine = item.Validita_Fine;
    impresaBase.indirizzi[0].indirizzo.frazione = item.frz_des;
    impresaBase.indirizzi[0].indirizzo.via = item.ind_des;
    impresaBase.indirizzi[0].indirizzo.istatComune.prov = item.Pro_Cod_Istat ?? '000';
    impresaBase.indirizzi[0].indirizzo.istatComune.com = item.Com_Cod_Istat ?? '000';
    impresaBase.indirizzi[0].indirizzo.cap = item.Cap;
    impresaBase.indirizzi[0].indirizzo.stato.codice = item.Stato_Cod;
    let padreInteressato = impresaBase.impresaPadre.find(imp => imp.partitaIva == item.Piva_Padre);
    if (padreInteressato) {
      padreInteressato.codice_iscrizione_libro_soci = item.codice_iscrizione_libro_soci;
      padreInteressato.data_iscrizione_libro_soci = item.data_iscrizione_libro_soci;
    }
    if (item.GruppoRaccolta_Cod) {
      impresaBase.gruppoRaccolta = { codice: item.GruppoRaccolta_Cod, descrizione: '' };
    } else {
      impresaBase.gruppoRaccolta = { codice: 0, descrizione: '' };
    }
    if (item.Codice_Socio != '') {
      let findIndex = impresaBase.codici.findIndex((val) => val.codiceAnagrafe.codice == enum_CodiciAnagrafe.Codice_Socio);
      if (findIndex < 0) {
        impresaBase.codici.push(this.getCodiceAnagrafeSocio(item.Codice_Socio))
      } else {
        impresaBase.codici[findIndex] = this.getCodiceAnagrafeSocio(item.Codice_Socio)
      }
    } else {
      let findIndex = impresaBase.codici.findIndex((val) => val.codiceAnagrafe.codice == enum_CodiciAnagrafe.Codice_Socio);
      if (findIndex >= 0) {
        impresaBase.codici.splice(findIndex, 1);
      }
    }
  }

  getCodiceAnagrafeSocio(codice_socio): CodiciAnagrafeValori {
    return {
      codiceAnagrafe: {
        codice: enum_CodiciAnagrafe.Codice_Socio,
        descrizione: 'codice socio',
        lunghezza: 0,
        picture: '',
        tipo: '',
        gruppo: '',
        genitore: 0,
        creatore: '',
        validita: new IntervalloTemporale(),
        flag_cancellazione: false
      },
      valore: codice_socio,
      validita: new IntervalloTemporale()
    }
  }

  onTemplateBtnClick(dataItem) {
    this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
    this.objParametriAgenda.Piva = dataItem.piva;
    this.objParametriAgenda.RagSoc = dataItem.rag_soc;
    this.objParametriAgenda.TipoOperazioneDB = Enum_DBTypeOperation.Update;
    this.objParametriAgendaService.changeObjParametriAgenda(this.objParametriAgenda);
    this.router.navigate(['Impresa-Edit'], { relativeTo: this.route });
  }

  onRowClass = (e: RowClassArgs) => {
    const isEven = e.index % 2 == 0;
    return {
      even: isEven,
      odd: !isEven
    };
  }

  handleDropdowns(): void {
    let col: KendoGridColumn;
    // Provincia
    col = this.kendoColumns.find(s => s.field === 'Pro_Cod_Istat');
    // data = this.provincie.map(cod => {
    //     return new DropdownListItem(cod.Istat_Prov, cod.Provincia_Des);
    // });
    col.ddl = new DropdownListWithForm('Istat_Prov', 'Pro_Cod_Istat', 'Provincia_Des', []);
    col.ddl.valuePrimitive = true;
    col.ddl.loadOnEdit = true;
    col.ddl.descriptionField = 'Provincia';
    col.ddl.loadFunction = this.caricaProvincie.bind(this);

    // Comune
    col = this.kendoColumns.find(s => s.field === 'Com_Cod_Istat');
    col.ddl = new DropdownListWithForm('codice', 'Com_Cod_Istat', 'descrizione', []);
    col.ddl.valuePrimitive = true;
    col.ddl.loadOnEdit = true;
    col.ddl.descriptionField = 'Com';
    col.ddl.loadFunction = this.caricaComuni.bind(this);

    // Stato
    col = this.kendoColumns.find(s => s.field === 'Stato_Cod');
    col.ddl = new DropdownListWithForm('codice', 'Stato_Cod', 'descrizione', []);
    col.ddl.valuePrimitive = true;
    col.ddl.loadOnEdit = true;
    col.ddl.descriptionField = 'Stato';
    col.ddl.loadFunction = this.caricaStati.bind(this);

    // Padre
    col = this.kendoColumns.find(s => s.field === 'Piva_Padre');
    col.ddl = new DropdownListWithForm('partitaIva', 'Piva_Padre', 'ragioneSociale', []);
    col.ddl.valuePrimitive = true;
    col.ddl.loadOnEdit = true;
    col.ddl.descriptionField = 'Rag_Soc_Padre';
    col.ddl.loadFunction = this.caricaPadri.bind(this);

    col = this.kendoColumns.find(s => s.field === 'GruppoRaccolta_Cod');
    col.ddl = new DropdownListWithForm('codice', 'GruppoRaccolta_Cod', 'descrizione', []);
    col.ddl.valuePrimitive = true;
    col.ddl.loadOnEdit = true;
    col.ddl.descriptionField = 'GruppoRaccolta_Des';
    col.ddl.loadFunction = this.caricaGruppiRaccolta.bind(this);

    // Attivo
    col = this.kendoColumns.find(s => s.field === 'Attivo');
    let data = [
      new DropdownListItem(0, 'No'),
      new DropdownListItem(1, 'Sì')
    ];
    col.ddl = new DropdownListWithForm('codice', 'Attivo', 'descrizione', data);
    col.ddl.valuePrimitive = true;
  }

  caricaComuni(dataItem: any) {
    return from(this.istatService.leggiComuni(dataItem.Pro_Cod_Istat));
  }

  caricaProvincie(dataItem: any) {
    return from(this.istatService.leggiProvincie(dataItem.Stato_Cod));
  }

  caricaStati(dataItem: any) {
    return from(this.istatService.leggiStati());
  }

  caricaPadri(dataItem: any) {
    return from(this.impreseService.leggiPadri()).pipe(tap((vals) => {
      vals = vals.map((el) => {
        if (el.CUAA != null && el.CUAA != ''){
          el.ragioneSociale = el.ragioneSociale + ' (' + el.CUAA + ')';
        }
        return el;
      });
      let impresaDefault = new Impresa();
      impresaDefault.partitaIva = '';
      impresaDefault.ragioneSociale = '';
      let index = vals.findIndex((el) => el.partitaIva == '');
      if (index < 0) {
        vals.splice(0, 0, impresaDefault);
      }
    }));
  }

  caricaGruppiRaccolta(dataItem: any) {
    let objPAgenda = this.objParametriAgendaService.getObjParamValue()
    return from(this.gruppiRaccoltaService.leggiGruppiRaccolta(objPAgenda)).pipe(tap((vals) => {

      let gruppoRaccoltaDefault = new GruppoRaccolta(0);
      gruppoRaccoltaDefault.descrizione = '';
      vals.splice(0, 0, gruppoRaccoltaDefault);
    }));
  }

  handleCustomizations(): void {
    this.selectable.selectable.checkboxOnly = false;
    this.selectable.selectable.enabled = this.permessoEdit;
    this.toolbar = new ToolbarSettings();
    this.toolbar.newItem = this.permessoEdit;
    this.toolbar.resetChanges = false;

    this.cmdColumn = new CommandsColumnSettings({
      editBtn: this.permessoEdit,
      infoBtn: false,
      removeBtn: this.permessoRemove,
    });
    this.setupDdlCommandsMenu()
    this.handleCommandEvent();

    this.resizable.autoFitColumns = true;
    // this.selectable.columnSettings.showSelectAll=true;
    // this.selectable.shouldShowCheckbox = true;
    // this.selectable.columnSettings.title=' ';
    // this.pagination.scrollingType = ScrollingMode.Virtual;
    // this.pagination.virtualScrolling.rowHeight = 50;
    // this.pagination.virtualScrolling.viewportHeight = 422;
    this.resizable.isResizable = true;
    this.selectable.selectable.enabled = true;
    this.groups.groupable.enabled = false;
    if (window.innerWidth < SMARTPHONE_WIDTH) {
      this.groups.groupable.enabled = false;
      this.views.enabled = false;
      this.cmdColumn.editBtn = false;
    }
  }

  private setupDdlCommandsMenu() {
    this.cmdDropDown = new CommandsDropDownSettings({
      inlineEditBtn: false,
      fullEditBtn: this.permessoEdit,
      infoBtn: this.permessoInfo,
      removeBtn: false,
    })
    if( this.permessoRicercaDocumenti
      && this.cmdDropDown.cmdList.findIndex(v=>v.action === this.businessLogic.commands.RICERCA_DOCUMENTI) === -1
    ){
      this.cmdDropDown.addCommand(new GridCommandItem(
        "RicercaDocumenti",
        this.businessLogic.commands.RICERCA_DOCUMENTI,
        'faAnagraficaSearchDocument'
      ));
    }
    if(this.permessoNuovoDocumento
      && this.cmdDropDown.cmdList.findIndex(v=>v.action === this.businessLogic.commands.NUOVO_ALLEGATO) === -1
    ){
      this.cmdDropDown.addCommand(new GridCommandItem(
        "AggiungiNuovoAllegato",
        this.businessLogic.commands.NUOVO_ALLEGATO,
        'faAnagraficaUploadFile'
      ));
    }
  }

  private handleCommandEvent() {
    this.gridPublicService.commandEvent
      .pipe(takeUntil(this.signal), filter(ev => !!ev))
      .subscribe(ev => {
        switch (ev.command.action) {
          case CommandsDropDownEvents.FULL_EDIT:
            this.onTemplateBtnClick(ev.dataItem);
            break;
          case this.businessLogic.commands.NUOVO_ALLEGATO:
            this.businessLogic.apriKWindowImprese(ev.dataItem, this.businessLogic.commands.NUOVO_ALLEGATO);
            break;
          case this.businessLogic.commands.RICERCA_DOCUMENTI:
            this.businessLogic.apriKWindowImprese(ev.dataItem, this.businessLogic.commands.RICERCA_DOCUMENTI);
            break;
        }
      });
  }

  private handleEdits() {
    this.handleInlineEdits();
    this.onChange();
  }

  private onChange() {
    this.gridPublicService.changeDetected.pipe(takeUntil(this.signal))
      .subscribe((event: any) => {
        if (event?.action === 'edit') {
          this.handleOnChangeEdit();
        }
        if (event?.action === 'add') {
          this.handleOnChangeAdd();
        }
      });
  }

  private handleOnChangeEdit() {
    let fb = this.gridPublicService.formGroup.getValue();
    if (fb != undefined) {
      fb.controls["piva"].disable();
      this.originalEditedLine = fb.getRawValue();
    }
  }

  private handleOnChangeAdd() {
    let fb = this.gridPublicService.formGroup.getValue();
    if (fb != undefined) {
      this.configurazioneSitiService.leggiChiave('Blocco_Inserimento_PIVA_Impresa').subscribe(r => {
        r?.Valore?.toLowerCase() === 'true' ? fb.controls['piva'].disable() : fb.controls['piva'].enable();
      });
      fb.controls['Stato_Cod'].setValue('IT');
      fb.controls['Pro_Cod_Istat'].setValue('000');
      fb.controls['Com_Cod_Istat'].setValue('000');
      let col_stato = this.kendoColumns.find(s => s.field === 'Stato_Cod');
      col_stato.ddl.reload.next(true);
      let col_prov = this.kendoColumns.find(s => s.field === 'Pro_Cod_Istat');
      col_prov.ddl.reload.next(true);
      let col_com = this.kendoColumns.find(s => s.field === 'Com_Cod_Istat');
      col_com.ddl.reload.next(true);
      this.caricaPadri(fb.getRawValue()).pipe(
        take(1),
        tap((padri) => {
          let col_padri = this.kendoColumns.find(s => s.field === 'Piva_Padre');
          col_padri.ddl.reload.next(true);
          const impresaSuperUser = padri.find(el => el.partitaIva == this.masterService.objP_server.PivaSuperUser);
          if (impresaSuperUser) {
            fb.controls['Piva_Padre'].setValue(impresaSuperUser.partitaIva);
            fb.controls['Rag_Soc_Padre'].setValue(impresaSuperUser.ragioneSociale);
          }
        })
      ).subscribe();
      this.originalEditedLine = null;
    }
  }

  private handleInlineEdits() {
    this.gridPublicService.formGroup.pipe(
      takeUntil(this.signal),
      filter((fb) => fb != undefined)
    ).subscribe((fb) => {
      fb.controls['piva'].addAsyncValidators(this.pivaValidatorService.validate.bind(this));
      fb.controls['piva'].updateValueAndValidity();
      this.listenToProvinceChange(fb);
      this.listenToStateChange(fb)
      this.listenToComChange(fb);
    });
  }

  private listenToProvinceChange(fb: FormGroup) {
    fb.controls["Pro_Cod_Istat"].valueChanges.pipe(takeUntil(this.signal)).subscribe(() => {
      let prov = fb.controls["Pro_Cod_Istat"].value;
      let stato = fb.controls["Stato_Cod"].value;
      if (prov != null && prov != "") {
        if (prov != '000') {
          fb.controls["Com_Cod_Istat"].enable();
        }
        this.istatService.leggiComuni(prov).then((comuni) => {
          this.istatService.leggiProvincie(stato).then((provincie) => {
            let provincia = provincie.find(p => p.Istat_Prov === prov)
            let com = comuni.find(s => s.codice === provincia.comuneDefault);
            let col = this.kendoColumns.find(s => s.field === 'Com_Cod_Istat');
            fb.controls["Com_Cod_Istat"].setValue(com.codice); // TODO da togliere l'array
            fb.controls["Com"].setValue(com.descrizione);          // TODO da togliere
            col.ddl.reload.next(true);
          })
        });
      }
    });
  }

  private listenToStateChange(fb: FormGroup) {
    fb.controls["Stato_Cod"].valueChanges.pipe(takeUntil(this.signal)).subscribe((value) => {
      from(this.istatService.leggiStati()).pipe(take(1), map(s => {
        let gestioneGerarchia = s.filter(t => t.codice == value)[0].gestioneGerarchia;
        let stato = fb.controls["Stato_Cod"].value;
        if (gestioneGerarchia != 1 && stato != '') {
          fb.controls["Com_Cod_Istat"].disable();
          fb.controls["Com"].disable();
          fb.controls["Pro_Cod_Istat"].disable({ emitEvent: false });
          fb.controls["Prov"].disable();
          fb.controls["Pro_Cod_Istat"].setValue('000', { emitEvent: false });
          fb.controls["Com_Cod_Istat"].setValue('000', { emitEvent: false });
          fb.controls['Cap'].setValue('00000');
          fb.controls["Com"].setValue('');
          fb.controls["Prov"].setValue('');
          fb.controls["Provincia"].setValue('');
        } else {
          fb.controls["Com_Cod_Istat"].enable();
          fb.controls["Com"].enable();
          fb.controls["Pro_Cod_Istat"].enable({ emitEvent: false });
          fb.controls["Prov"].enable();
          fb.controls["Pro_Cod_Istat"].setValue(value == 'IT' ? '000' : value + '000');
          fb.controls["Com_Cod_Istat"].setValue(value == 'IT' ? '000' : value + '000', { emitEvent: false });
          fb.controls['Cap'].setValue('00000');
          fb.controls["Com"].setValue('');
          fb.controls["Prov"].setValue('');
          fb.controls["Provincia"].setValue('');
          let col = this.kendoColumns.find(s => s.field === 'Pro_Cod_Istat');
          col.ddl.reload.next(true);
        }
      })).subscribe();
    });
  }

  private listenToComChange(fb: FormGroup) {
    fb.controls["Com_Cod_Istat"].valueChanges.pipe(takeUntil(this.signal)).subscribe(() => {
      let com = fb.controls["Com_Cod_Istat"].value;
      let prov = fb.controls["Pro_Cod_Istat"].value;
      let stato = fb.controls["Stato_Cod"].value;
      if (com != '' && com != '000' && prov != '' && prov != '000' && (stato == 'IT' || stato == '')) {
        this.istatService.leggiCAP(fb.controls["Pro_Cod_Istat"].value, fb.controls["Com_Cod_Istat"].value,)
          .then((cap) => fb.controls['Cap'].setValue(cap))
      } else {
        fb.controls['Cap'].setValue('00000');
      }
    });
  }

  override async getRemoveMultipleRowsMessage(opts: RemoveMultipleRowsParams) {
    let msg: string = this.translocoService.translate('anagrafica.Delete_Msg')
    opts.data.forEach(r => {
      msg = msg.concat('\n' + r['rag_soc']);
    })
    return msg;
  }

}
