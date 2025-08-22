import {Injectable, Injector} from "@angular/core";
import {
  AbstractGridConfigService,
  CommandsColumnSettings,
  CommandsDropDownSettings,
  EditingMode,
  HttpAction,
  KendoGridColumn,
  KendoServerResultImpl,
  LoaderType,
  ToolbarSettings
} from "gias-kendo-grid";
import {PermessiUtenteService} from "../../../Service/permessi-utente.service";
import {forkJoin, map, Observable, of, take, tap} from "rxjs";
import {enum_Security_Attivita} from "../../../Model/TipiEnumerativi";
import {PrescrizioniClient} from "../../../Service/net-core6-api.service";
import {ZooOperationsFilters} from "../../models/zoo-operations-filters.model";
import {ObjParametriAgendaService} from "../../../Service/obj-parametri-agenda.service";
import {ZooPrescriptionGridFlatItem, ZooPrescriptionGridModel} from "../../models/zoo-prescription-grid-item.model";
import {BaseCodeDescr} from "../../../Service/api.service";
import {ZooFiltersHelperService} from "../../services/zoo-filters-helper.service";
import {cloneDeep} from "lodash";
import {TUTTI_CENTRI_AZIENDALI} from "../../../Model/CostantiPersonalizzate";
import {LeggiPrescrizioniIndicazioni} from "../../models/leggi-prescrizioni.model";
import { ConversionService } from "gias-ui-kit";

@Injectable()
export class ZooIndicationsGridConfigService extends AbstractGridConfigService<KendoServerResultImpl> {
  editingMode = EditingMode.IN_PAGE;
  gridId = "zooIndicationsGrid";
  loader = LoaderType.SERVICE;
  rowId: string;

  private readonly _canEdit: boolean;
  private _centers: BaseCodeDescr[];
  private _stables: BaseCodeDescr[];
  private _filters: ZooOperationsFilters | null = null;
  private _gridModel = ZooPrescriptionGridModel;
  private _gridRows: ZooPrescriptionGridFlatItem[] = [];
  private _gridColumns = [
    new KendoGridColumn({field: 'IdRicetta', title: this.transloco.translate('ID')}),
    new KendoGridColumn({field: 'DataEmissione', title: this.transloco.translate('Data')}),
    new KendoGridColumn({field: 'Numero', title: this.transloco.translate('Numero')}),
    new KendoGridColumn({field: 'Note', title: this.transloco.translate('Note')}),
    new KendoGridColumn({field: 'Posologia', title: this.transloco.translate('Posologia')}),
    new KendoGridColumn({field: 'sa_nome', title: this.transloco.translate('CentroAziendale')}),
    new KendoGridColumn({field: 'STA_DES', title: this.transloco.translate('Stalla')}),
    new KendoGridColumn({field: 'ProprietarioIdFiscale', title: this.transloco.translate('Proprietario')}),
    new KendoGridColumn({field: 'VeterinarioIdFiscale', title: this.transloco.translate('Veterinario')}),
    new KendoGridColumn({field: 'DetentoreIdFiscale', title: this.transloco.translate('Detentore')}),
    new KendoGridColumn({field: 'Username_Creazione', title: this.transloco.translate('UtenteCreazione')}),
    new KendoGridColumn({ field: 'Username_Modifica', title: this.transloco.translate('UtenteModifica') }, { editable: false, width: 150 })
  ];

  constructor(
    injector: Injector,
    private agenda: ObjParametriAgendaService,
    private prescriptions: PrescrizioniClient,
    private permissions: PermessiUtenteService,
    private helper: ZooFiltersHelperService,
    private conversionService: ConversionService
  ) {
    super(injector);
    this._canEdit = this.permissions.canWritePermesso(enum_Security_Attivita.ZooProtocolliTerapeutici);
    this.handleCustomizations();
  }

  private get currentPiva(): string {
    const agenda = this.agenda.getObjParamValue();
    return agenda.Piva ?? '';
  }

  read(options?: ZooOperationsFilters): Observable<KendoServerResultImpl> {
    if (this.reuseLoadedData(options)) {
      return of(new KendoServerResultImpl(this._gridModel, this._gridColumns, this._gridRows));
    }
    this._filters = cloneDeep(options);
    this.loadingService.set_isLoading({isLoading: true, component: this.gridPublicService.gridElRef});
    this.gridPublicService
    return forkJoin([
      this.loadAdditionalData(),
      this.prescriptions.prescrizioniLeggiPrescrizioni(this.getLoadParams())
    ]).pipe(
      map(results => results[1]),
      map(r => r.RispostaOK ? JSON.parse(r.RispostaStringa) : []),
      map(r => this.conversionService.ConversionDateInObject(r)),
      tap(rows => this._gridRows = rows.filter(r =>
        new Date(r.DataEmissione) >= options.from && new Date(r.DataEmissione) <= options.to
      )),
      map(() => new KendoServerResultImpl(this._gridModel, this._gridColumns, this._gridRows)),
      tap(() => this.loadingService.set_isLoading({isLoading: false, component: this.gridPublicService.gridElRef}))
    );
  }

  perform(actionType: HttpAction, items: any, oldRow: any): Observable<any> {
    return of(null);
  }

  private handleCustomizations(): void {
    this.gridIsEditable = false;
    this.groups.groupable.enabled = false;
    this.columnMenu.kendoGridColumnChooser = true;
    this.views.enabled = true;
    this.resizable.autoFitColumns = true;
    this.resizable.isResizable = true;
    this.toolbar = new ToolbarSettings();
    this.toolbar.newItem = false;
    this.groups.groupable.enabled = false;
    this.setupCommands();
  }

  private setupCommands() {
    this.cmdColumn = new CommandsColumnSettings({editBtn: false, infoBtn: false, removeBtn: false});
    this.cmdDropDown = new CommandsDropDownSettings({fullEditBtn: false, infoBtn: false});
  }

  private getLoadParams() {
    return new LeggiPrescrizioniIndicazioni(
      this.currentPiva,
      this._filters.center,
      this._filters.stable
    );
  }

  private loadAdditionalData(): Observable<boolean> {
    if (!!this._centers && !!this._stables) return of(true);

    return forkJoin([
      this.helper.loadBusinessCenters(this.currentPiva).pipe(
        tap(c => this._centers = c),
      ),
      this.helper.loadStables(this.currentPiva, TUTTI_CENTRI_AZIENDALI).pipe(
        tap(s => this._stables = s),
      )
    ]).pipe(take(1), map(() => true));
  }
 
  /**
   * @param options the filters passed to the {@link read} function
   * @private
   * @returns `true` if the filters are not set and there are no specified options or,
   * if the new reading options are the same as the filters. `false` otherwise.
   */
  private reuseLoadedData(options: ZooOperationsFilters): Boolean {
    return (!this._filters && !options) || (!options && !!this._filters) || !options.forceReload && (
      !!this._filters && !!options
      && options.center === this._filters.center
      && options.stable === this._filters.stable
      && options.from === this._filters.from
      && options.to === this._filters.to
      && options.operations.reduce((x1, x2) => x1 + "|" + x2) === this._filters.operations.reduce((x1, x2) => x1 + "|" + x2)
    );
  }

}
