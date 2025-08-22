import { AfterViewInit, Component, OnDestroy, OnInit } from "@angular/core";
import { ZooRedirectorService } from "./services/zoo-redirector.service";
import { GiasMultiSelectTemplateService } from 'gias-ui-kit';
import { GiasDropDownTemplateService } from 'gias-ui-kit';
import { enum_Security_Attivita } from "../Model/TipiEnumerativi";
import { ObjParametriAgendaService } from "../Service/obj-parametri-agenda.service";
import { FormGroup } from "@angular/forms";
import { ZooOperationsFilters, ZooOperationsFiltersForm } from "./models/zoo-operations-filters.model";
import { ReplaySubject, switchMap } from "rxjs";
import { AGRODATAFINE, AGRODATAINIZIO } from "../Model/CostantiPersonalizzate";
import { ZooFiltersHelperService } from "./services/zoo-filters-helper.service";
import { BaseCodeDescr as IBaseCodeDescr, BaseCodeDescrStr as IBaseCodeDescrStr } from "../Service/api.service";
import { CookieService } from "../Service/cookie.service";
import { SelectEvent } from "@progress/kendo-angular-layout";
import { PermessiUtenteService } from "app/Service/permessi-utente.service";

@Component({
  standalone: false,
  selector: 'app-zoo',
  templateUrl: './zoo.component.html',
  styleUrls: ['./zoo.component.scss'],
  providers: [
    ZooRedirectorService, GiasDropDownTemplateService, GiasMultiSelectTemplateService
  ]
})
export class ZooComponent implements OnInit, AfterViewInit, OnDestroy {

  protected readonly hasZooReadPermission: boolean;
  protected readonly hasProtocolsReadPermission: boolean;
  protected readonly hasTherapeuticIndicationsReadPermission: boolean;
  protected searchFilters$ = new ReplaySubject<ZooOperationsFilters>(null);
  protected filters: FormGroup<ZooOperationsFiltersForm>;
  protected centers: IBaseCodeDescr[] = [];
  protected stables: IBaseCodeDescr[] = [];
  protected zooOperations: IBaseCodeDescrStr[] = [];

  private _selectedTab: number = 0;

  constructor(
    private cookies: CookieService,
    private permissions: PermessiUtenteService,
    private agenda: ObjParametriAgendaService,
    private zooFiltersHelper: ZooFiltersHelperService,
  ) {
    this.hasZooReadPermission = this.permissions.canReadPermesso(enum_Security_Attivita.Gest_Stalle);
    this.hasProtocolsReadPermission = this.permissions.canReadPermesso(enum_Security_Attivita.ZooProtocolliTerapeutici);
    this.hasTherapeuticIndicationsReadPermission = this.permissions.canReadPermesso(enum_Security_Attivita.ZooIndicazioniTerapeutiche);
  }
  ngOnDestroy(): void {
    this.zooFiltersHelper.resetFilters$.next(true);
  }

  public get isImpresaSelezionata(): boolean {
    let isImpSel = this.currentPiva != undefined && this.currentPiva != null && this.currentPiva !== '';
    return isImpSel
  }

  private get currentPiva(): string {
    const agenda = this.agenda.getObjParamValue();
    return agenda.Piva ?? '';
  }

  ngOnInit() {
    this._selectedTab = +(this.cookies.getCookie('zooTab') ?? 0);
    if (this.isImpresaSelezionata){
      this.initFilters();
      this.readSavedFilters();
    }
  }

  ngAfterViewInit() {
    this.applyFilters(true);
  }

  public onTabSelect(event: SelectEvent): void {
    this._selectedTab = event.index;
    this.cookies.setCookie({ name: 'zooTab', value: event.index.toString(), expireDays: 10 * 365 });
    this.readSavedFilters();
    this.applyFilters(false);
  }

  public isTabSelected(index: number): boolean {
    return this._selectedTab === index;
  }

  protected applyFilters(nextValue: boolean) {
    const newData = new ZooOperationsFilters(
      this.filters.controls.center.value as number,
      this.filters.controls.stable.value as number,
      this.filters.controls.from.value as Date ?? this.zooFiltersHelper.monthStart,
      this.filters.controls.to.value as Date ?? this.zooFiltersHelper.monthEnd,
      this.filters.controls.operations.value as string[],
    );
    console.debug("applyFilters", newData);
    //if (nextValue == true){
      this.searchFilters$.next(newData);
    //}
    this.rememberFilters(newData);
  }

  private initFilters() {
    this.filters = this.zooFiltersHelper.getFiltersFormGroup();
    this.zooFiltersHelper.loadBusinessCenters(this.currentPiva)
      .subscribe(centers => this.centers = centers);
    this.zooFiltersHelper.loadStables(this.currentPiva, this.filters.controls.center.value)
      .subscribe(stables => this.stables = stables);
    this.zooFiltersHelper.loadZooOperations()
      .subscribe(operations => this.zooOperations = operations);

    this.filters.controls.center.valueChanges
      .pipe(
        switchMap(center => this.zooFiltersHelper.loadStables(this.currentPiva, center))
      )
      .subscribe(stables => { 
        this.stables = stables;
        if (stables.length == 1){
          this.filters.controls.stable.setValue(stables[0].codice);
          return;
        }
        if (stables.length == 0){
          this.stables = [{codice:0, descrizione:''}];
          this.filters.controls.stable.setValue(0);
          return;
        }
      });
  }

  private rememberFilters(newData: ZooOperationsFilters) {
    const from = newData.from.toISOString();
    const to = newData.to.toISOString();
    const operations = newData.operations;
    this.cookies.setCookie({ name: 'zoo' + this._selectedTab + 'FiltersDateStart', value: from, expireDays: 10 * 365 });
    this.cookies.setCookie({ name: 'zoo' + this._selectedTab + 'FiltersDateEnd', value: to, expireDays: 10 * 365 });
    if (operations.length) {
      this.cookies.setCookie({
        name: 'zoo' + this._selectedTab + 'FiltersOperations',
        value: operations.reduce((acc, x) => acc + "|" + x),
        expireDays: 10 * 365
      });
    } else {
      this.cookies.deleteCookie('zoo' + this._selectedTab + 'FiltersOperations');
    }
  }

  private readSavedFilters() {
    const from = this.cookies.getCookie('zoo' + this._selectedTab + 'FiltersDateStart');
    const to = this.cookies.getCookie('zoo' + this._selectedTab + 'FiltersDateEnd');
    const operations = this.cookies.getCookie('zoo' + this._selectedTab + 'FiltersOperations');
    this.filters.controls.operations.patchValue(operations.split("|").filter(x => x !== ''));
    this.filters.controls.from.patchValue(!from ? this.zooFiltersHelper.monthStart : new Date(from));
    this.filters.controls.to.patchValue(!to ? this.zooFiltersHelper.monthEnd : new Date(to));
  }

}
 