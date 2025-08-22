/* eslint-disable */
import {Component, ContentChild, Inject, OnDestroy, TemplateRef, ViewChild, ViewEncapsulation} from '@angular/core';
import {Tipo_Ricetta} from 'app/Model/attivita/Attivita';
import {GRID_HTTP_TOKEN} from 'gias-kendo-grid';
import {generateGridProviders} from 'gias-kendo-grid';
import {Observable, of, Subject, switchMap} from 'rxjs';
import {BussinessMenuAgendaService} from '../../shared_services/bussiness-logic.service';
import {RicetteGridConfig} from './ricette-grid.service';
import {RicettaRow} from '../utils';
import {enum_Security_Attivita} from "../../../Model/TipiEnumerativi";
import {PermessiUtenteService} from "../../../Service/permessi-utente.service";
import {MenuAgendaDataStore} from 'app/menu-agenda/shared_services/menu-agenda-datastore.service';
import {GiasKendoGridComponent} from 'gias-kendo-grid';
import {GiasDialogService} from 'app/Service/gias-dialog.service';
import {TranslocoService} from '@jsverse/transloco';
import {faArrowUpFromBracket, faFileExport} from '@fortawesome/free-solid-svg-icons';
import {SelectionEvent} from '@progress/kendo-angular-grid';
import {RicetteService} from './ricette.service';
import {GridPublicService} from 'gias-kendo-grid';
import {DialogCloseResult, DialogResult} from '@progress/kendo-angular-dialog';
import {HttpAction} from 'gias-kendo-grid';
import {MasterService} from 'app/Service/master.service';

@Component({
  standalone: false,
  selector: 'grid-ricette',
  templateUrl: './ricette.component.html',
  styleUrls: ['./ricette.component.css'],
  providers: [
    ...generateGridProviders(RicetteGridConfig, RicetteComponent),
    BussinessMenuAgendaService
  ],
  encapsulation: ViewEncapsulation.None,
})
export class RicetteComponent implements OnDestroy {

  /** Pulsanti Documentale */
  @ContentChild('gridCommands') gridCommands: TemplateRef<any>;
  @ContentChild('stampeRef') stampeRef: TemplateRef<any>;

  @ViewChild(GiasKendoGridComponent) gridChild: GiasKendoGridComponent;

  permessoEdit: boolean = true;
  permessoMultiCancellazione: boolean = false;
  faExport = faFileExport;
  faUpload = faArrowUpFromBracket;
  signal: Subject<void> = new Subject();

  constructor(
    @Inject(GRID_HTTP_TOKEN) private grid: RicetteGridConfig,
    private bussiness: BussinessMenuAgendaService,
    private masterService: MasterService,
    protected store: MenuAgendaDataStore,
    private transloco: TranslocoService,
    private dialogService: GiasDialogService,
    private permessiUtenteService: PermessiUtenteService,
    private ricetteService: RicetteService,
    private gridpublicService: GridPublicService,
    @Inject(GRID_HTTP_TOKEN)
    private RicetteGridConfig: RicetteGridConfig
  ) {
    this.setPermissions();
  }

  get selected(): RicettaRow[] {
    return this.store.gridDataRicette.rows.filter(r => r['Selected']) as RicettaRow[];
  }

  ngOnDestroy() {
    this.signal.next();
    this.signal.complete();
  }

  private setPermissions() {
    this.permessoEdit = this.permessiUtenteService.getPermesso(
      enum_Security_Attivita.Gest_Ricette, 2);
    this.permessoMultiCancellazione = this.permessiUtenteService.getPermesso(
      enum_Security_Attivita.ManutenzioneArchivi_MultiCancellazioneInterventi, 2);
  }

  apriRicettaDaStampare(row: RicettaRow) {
    this.grid.apriRicettaDaStampare(row.Ricetta_Cod);
  }

  apriPianoLavoriDellaRicetta(row: RicettaRow) {
    this.grid.apriPianoLavoriDaStampare(row.Ricetta_Cod);
  }

  onSelectionChange($event: SelectionEvent): void {
    const selected: RicettaRow[] = $event.selectedRows.map(x => x.dataItem);
    const deselected: RicettaRow[] = $event.deselectedRows.map(x => x.dataItem);
    this.ricetteService.select(selected, deselected);
  }

  public async onInviaAdApp(item: any) {
    if (item.actionName !== 'InviaDatiAPP' || !this.gridChild || !this.gridChild.rows) return;
    let multi: string[] = this.gridChild.rows.filter(r => r['Selected'])
      .filter(row => row['Raccoglitore_Cod'] !== '0')
      .map(r => r['Raccoglitore_Cod']);

    if (multi.length)
      this.gridChild.rows.filter(r => multi.includes(r['Raccoglitore_Cod']))
        .forEach(r => r['Selected'] = true);

    let selected = this.gridChild.rows.filter(r => r['Selected']);

    if (!selected.length) {
      this.dialogService.baseError('', 'SelezionareAlmenoUnaRicetta', true);
      return;
    }
    if (selected.some(r => r['in_uso'] === '1')) {
      let trKey = selected.length === 1 ? 'ImpossibileInviareRicettaInUso' : 'ImpossibileInviareRicetteInUso';
      this.dialogService.baseError('', trKey, true);
      return;
    }

    if (selected.some(r => r['Invia_App'] === '1')) {
      let trKey = selected.length === 1 ? 'RicettaGiaInviataAllApp' : 'RicetteGiaInviateAllApp';
      this.dialogService.baseError('', trKey, true);
      return;
    }

    let resp = await this.dialogService.baseWarning(
      this.transloco.translate('InviaDatiAPP'),
      this.transloco.translate('ImpossibileModificareRicetteDopoConferma'),
      false
    )

    if (resp['returnObj']) {
      // Nella funzione lato serve uso il campo APP_Ricetta_Operazione_ID
      // come identificativo della ricetta
      this.masterService.set_isLoading({isLoading: true});

      this.bussiness.inviaRicettaApp(selected as RicettaRow[]).then(r => {
        this.masterService.set_isLoading({isLoading: false});
        //Ricarico la grid se la chiamata è andata a buon fine
        if (r.RispostaOK) {
          this.grid.applicaFiltri = true;
          this.gridpublicService.refresh(true);
          if (r.ErroriGias.length > 0) {
            this.dialogService.baseError('', r.ErroriGias[0].messaggio, false);
          } else {
            if (selected.length > 1) {
              this.dialogService.baseSuccess('', 'Ricette inviate correttamente all\'APP', false);
            } else {
              this.dialogService.baseSuccess('', 'Ricetta inviata correttamente all\'APP', false);
            }
          }
        }
      });
    }

  }

  public removeSelected() {
    this.validateSelected().pipe(switchMap(canDelete => {
      if (canDelete) {
        return  this.selected.length > 1
        ? this.transloco.translate('MultipleDeletionConfirmation', [this.selected.length])
            : this.transloco.translate('SoleDeletionConfirmation');
      }
      return of(null);
    })).subscribe(prompt => {
      if (!!prompt && prompt !== '')
        this.showDeletionConfirm(prompt)
      else if (prompt === '')
        this.RicetteGridConfig.perform(HttpAction.REMOVE, this.selected, true)
          .GiasSubscribe(r => null);
    })
  }

  private showDeletionConfirm(prompt: string) {
    this.dialogService.dialogMessageObs_Result(
      this.transloco.translate('ActivityDeletion'), prompt
    ).GiasSubscribe((R: DialogResult) => {
      if (R instanceof DialogCloseResult) return;
      if (R['returnObj']) {
        this.RicetteGridConfig.perform(HttpAction.REMOVE, this.selected, true)
          .GiasSubscribe(r => null);
      }
    })
  }

  public onNuovo() {
    this.bussiness.creaNuovaOperazione(Tipo_Ricetta.Standard_Destinazioni);
  }

  private error(content: string) {
    this.dialogService.baseError('Errore', content);
  }

  private validateSelected(): Observable<boolean> {
    const selected = this.selected;
    if (!selected.length) {
      this.error('SelezionareAlmenoUnOperazione');
      return of(false);
    }
    const inUso = selected.filter(r => r.in_uso === '1');
    if (inUso.length > 0) {
      return this.messaggioWarnInUso();
    }
    return of(true)
  }

  private messaggioWarnInUso() {
    const inUso = this.selected.filter(r => r.in_uso === '1');
    const obs = new Subject<boolean>();
    const complete = (val: boolean) => {
      obs.next(val);
      obs.complete();
    }

    if (inUso.length === this.selected.length) {
      this.dialogService.baseError(
        '', this.transloco.translate('ErroreCancellazioneRicetteSalvateAgenda'), false
      );
      complete(false);
    } else {
      let messaggio = this.transloco.translate('AttenzioneRicetteSalvateAgenda');
      messaggio += "\n" + this.transloco.translate('Ricette') + ": ";
      messaggio += inUso.map(e => '- ' + e.Descrizione_Unica)
        .reduce((a, b) => a + '<BR/>' + b)
      this.dialogService.baseWarning('', messaggio, false).then(resp => {
        if (resp['returnObj'])
          inUso.forEach(r => r['Selected'] = false);
        complete(resp['returnObj']);
      });
    }

    return obs;
  }
}
