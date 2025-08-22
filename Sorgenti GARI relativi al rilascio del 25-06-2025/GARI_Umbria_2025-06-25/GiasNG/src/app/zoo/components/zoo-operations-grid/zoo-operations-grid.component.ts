import { AfterViewInit, Component, Inject, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { ZooOperationsGridConfigService } from "./zoo-operations-grid.service";
import { faLock, faLockOpen } from "@fortawesome/free-solid-svg-icons";
import { GiasDialogService } from "../../../Service/gias-dialog.service";
import { filter, from, map, Observable, of, Subject, switchMap, take, takeUntil, tap } from "rxjs";
import { FormControl, FormGroup } from "@angular/forms";
import { BaseCodeDescr } from "../../../Model/baseClass/baseCodeDescr";
import { ZooRedirectorService } from "../../services/zoo-redirector.service";
import { ZooOperationsFilters } from "../../models/zoo-operations-filters.model";
import { ZooFiltersHelperService } from "../../services/zoo-filters-helper.service";
import { BussinessMenuAgendaService } from "../../../menu-agenda/shared_services/bussiness-logic.service";
import { ZooOperationGridFlatItem } from "../../models/zoo-operation-grid-item.model";
import { TranslocoService } from '@jsverse/transloco';
import { generateGridProviders, GiasKendoGridComponent, GRID_HTTP_TOKEN, HttpAction, KendoServerResult } from 'gias-kendo-grid';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { enum_Security_Attivita } from 'app/Model/TipiEnumerativi';

@Component({
  standalone: false,
  selector: 'zoo-operations-grid',
  templateUrl: './zoo-operations-grid.component.html',
  styleUrls: ['./zoo-operations-grid.component.css'],
  providers: [
    BussinessMenuAgendaService,
    // eslint-disable-next-line no-use-before-define
    ...generateGridProviders(ZooOperationsGridConfigService, ZooOperationsGridComponent)
  ]
})
export class ZooOperationsGridComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('addOperationTemplate') addOperationTemplate: TemplateRef<any>;
  @ViewChild('zooOpGrid') grid: GiasKendoGridComponent;
  @Input() filters$: Subject<ZooOperationsFilters>;
  @Output() editFavorites = new Subject<void>();
  protected readonly faLock = faLock;
  protected readonly faLockOpen = faLockOpen;
  protected readonly SHOW_FAVORITES_SETTINGS = true;
  protected innerForm: FormGroup;
  //protected editFavorites = false;
  protected readonly console = console;
  protected _favorites: BaseCodeDescr[];
  protected _operationTypes: BaseCodeDescr[];

  protected canEdit = true;
  protected canLockOperations = false;
  protected canUnlockOperations = false;

  private signal = new Subject<void>();
  private _lastUsedFilters: ZooOperationsFilters;

  constructor(
    private transloco: TranslocoService,
    private dialog: GiasDialogService,
    private zooRedirector: ZooRedirectorService,
    private zooHelper: ZooFiltersHelperService,
    private business: BussinessMenuAgendaService,
    private permissions: PermessiUtenteService
  ) {
    this.canEdit = this.permissions.canWritePermesso(enum_Security_Attivita.MenuZooNG);
    this.canLockOperations = this.permissions.canWritePermesso(enum_Security_Attivita.Agenda_Operazioni_Blocco);
    this.canUnlockOperations = this.permissions.canWritePermesso(enum_Security_Attivita.Agenda_Operazioni_Sblocco);
  }

  protected get operationTypes(): BaseCodeDescr[] {
    return this.innerForm.get('favorites')?.value ? this._favorites : this._operationTypes;
  }

  private get selected(): ZooOperationGridFlatItem[] {
    return this.grid.rows.filter(r => r['Selected']) as ZooOperationGridFlatItem[];
  }

  protected set operationTypes(operations: BaseCodeDescr[]) {
    this._operationTypes = operations;
  }

  ngOnInit(): void {
    this.loadOperations();
  }

  ngAfterViewInit(): void {
    this.filters$.pipe(
      takeUntil(this.signal),
      filter(x => x !== null),
      tap(x => this._lastUsedFilters = x),
      switchMap(filters => this.grid.config.read(filters))
    ).subscribe(() => this.grid.publicService.refresh(true));
  }

  ngOnDestroy(): void {
    this.signal.next();
    this.signal.complete();
  }

  public newOperation() {
    this.innerForm = new FormGroup({
      operationType: new FormControl(''),
      favorites: new FormControl(false)
    });
    this.dialog.dialogMessageObs_Result(this.transloco.translate("NuovaAttività"), this.addOperationTemplate)
      .pipe(
        take(1),
        filter(r => r['returnObj'] === true),
        map(() => this.innerForm.value.operationType),
        tap(() => this.innerForm = null),
        switchMap((operationType: number) => {
          this.zooRedirector.redirectNewOperation(operationType);
          return of(true);
        })
      ).subscribe();
  }

  public removeSelected() {
    this.preventIfEmptySelection(() => {
      const prompt = this.selected.length > 1
        ? this.transloco.translate('MultipleDeletionConfirmation', [this.selected.length])
        : this.transloco.translate('SoleDeletionConfirmation');
      this.dialog.dialogMessageObs_Result(this.transloco.translate('ActivityDeletion'), prompt)
        .pipe(
          filter(r => r['returnObj']),
          switchMap(() => this.grid.config.perform(HttpAction.REMOVE, this.selected)),
          tap(() => console.debug('After delete???')),
          switchMap(() => this.refresh())
        ).subscribe(() => console.debug('subscribe after delete???'));
    });
  }

  public lockOperation() {
    this.preventIfEmptySelection(() => {
      if (this.selected.every(i => i.Blocco_Flag === 1)) return;
      let prompt: string;
      if (this.selected.length > 1) {
        prompt = this.transloco.translate('MultipleLockConfirmation', [this.selected.length]);
      } else {
        prompt = this.transloco.translate('SoleLockConfirmation');
      }

      this.dialog.dialogMessageObs_Result(this.transloco.translate('ActivityLock'), prompt)
        .pipe(
          filter(r => r['returnObj']),
          switchMap(() => this.business.bloccaAttivitaZoo(this.selected)),
          filter(isOk => isOk),
          switchMap(() => this.refresh())
        ).subscribe();
    });
  }

  public unlockOperation() {
    this.preventIfEmptySelection(() => {
      if (this.selected.every(i => i.Blocco_Flag === 0)) return;
      let prompt: string;
      if (this.selected.length > 1) {
        prompt = this.transloco.translate('MultipleUnlockConfirmation', [this.selected.length]);
      } else {
        prompt = this.transloco.translate('SoleUnlockConfirmation');
      }

      this.dialog.dialogMessageObs_Result(this.transloco.translate('ActivityUnlock'), prompt)
        .pipe(
          filter(r => r['returnObj']),
          switchMap(() => this.business.sbloccaAttivitaZoo(this.selected)),
          filter(isOk => isOk),
          switchMap(() => this.refresh())
        ).subscribe();
    });
  }

  private preventIfEmptySelection(fun: Function) {
    if (this.selected.length > 0) {
      fun.call(this);
    } else {
      this.dialog.baseError('Errore_', 'SelezionaAttivitàPerContinuare');
    }
  }

  /**
   * Loads both all the available operations and the user's favorites ones.
   * @private
   */
  private loadOperations() {
    this.zooHelper.loadZooOperations()
      .pipe(
        map(operations => operations.map(o => new BaseCodeDescr(+o.codice, o.descrizione))),
        tap(operations => this.operationTypes = operations),
        switchMap(() => this.zooHelper.loadFavorites()),
        map(favorites => {
          let fav: BaseCodeDescr[] = [];
          for (let f of favorites) {
            const found = this._operationTypes.find(x => x.codice === +f.codice);
            fav.push(new BaseCodeDescr(+f.codice, found.descrizione));
          }
          return fav;
        })
      ).subscribe(favorites => this._favorites = favorites);
  }

  private refresh(): Observable<KendoServerResult> {
    this._lastUsedFilters.forceReload = true;
    return this.grid.config.read(this._lastUsedFilters)
      .pipe(
        tap(() => this._lastUsedFilters.forceReload = false),
        tap(newData => this.grid.publicService.refresh(true, newData))
      );
  }
}
