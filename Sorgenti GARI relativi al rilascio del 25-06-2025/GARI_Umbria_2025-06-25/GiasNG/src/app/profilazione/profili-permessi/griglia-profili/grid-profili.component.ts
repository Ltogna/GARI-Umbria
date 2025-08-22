import {Component, TemplateRef, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {generateGridProviders} from 'gias-kendo-grid';
import {GridProfiliMenuActions, GridProfiliService} from './grid-profili.service';
import {BaseCodeDescr} from 'app/Model/baseClass/baseCodeDescr';
import {TipologiaUtente} from '../../models/profili-permessi/tipologia-utente.model';
import {TipologieUtentiService} from 'app/profilazione/services/profili-permessi/tipologie-utenti.service';
import {GiasDialogService} from '../../../Service/gias-dialog.service';
import {TranslocoService} from '@jsverse/transloco';
import {filter, map, switchMap, tap} from 'rxjs';
import {enum_PaginaImpostazioni} from '../../impostazioni-utente/impostazioni.model';
import {ImpostazioniFormService} from '../../services/impostazioni/impostazioni-form.service';
import {GiasWindowsService} from 'gias-ui-kit';

@Component({
  standalone: false,
  selector: 'app-grid-profili',
  templateUrl: './grid-profili.component.html',
  styleUrls: ['./grid-profili.component.css'],
  providers: [
    ...generateGridProviders(GridProfiliService, GridProfiliComponent)
  ]
})
export class GridProfiliComponent {

  @ViewChild('profileGrid') profileGrid: any;
  @ViewChild('copyFormTemplate') copyFormTemplate: TemplateRef<any>;
  @ViewChild('impostazioniRef') impostazioniRef;

  public selected: BaseCodeDescr;
  public copyProfileForm: FormGroup = new FormGroup({
    Description: new FormControl('', Validators.required),
    copySettings: new FormControl(false)
  });

  constructor(
    private profileService: TipologieUtentiService,
    private impostazioniService: ImpostazioniFormService,
    private dialog: GiasDialogService,
    private windowService: GiasWindowsService,
    private transloco: TranslocoService,
    private router: Router,
  ) {
    this.showCopyProfileForm();
    this.showSettingsForm();
  }

  public selectProfile(event: any) {
    if (event.selectedRows?.length) {
      let profile = event.selectedRows.at(0).dataItem;
      this.selected = profile;
      this.profileService.selectedProfile$.next(profile);
    } else {
      this.profileService.selectedProfile$.next(null);
    }
  }

  /**
   * Apre il form per la modifica delle impostazioni collegate al profilo.
   * La modalità di caricamento delle impostazioni per le tipologie è la stessa che per gli utenti.
   * (Impostazioni sempre salvate nella tabella Utenti_Impostazioni)
   */
  public showSettingsForm() {
    this.profileService.profileGridEvent$.pipe(
      filter(menuAction => menuAction.event === GridProfiliMenuActions.SET_SETTINGS),
      map(menuAction => menuAction.item),
      tap(selected => this.selected = selected),
    ).subscribe(selected => {
      this.impostazioniService.USAGE_AREA = enum_PaginaImpostazioni.UTENTI;
      this.impostazioniService.utentiSelezionati = [selected.codice.toString()];
      this.windowService.open({
        title: this.transloco.translate('prof.ImpostazioniUtenteDelProfilo') + ' "' + selected.descrizione + '"',
        content: this.impostazioniRef,
        height: window.innerHeight * 0.9,
        width: window.innerWidth * 0.9,
      });
    });
  }

  public saveSettings(onlyTouched: boolean) {
    let content = onlyTouched ? 'prof.WarningSalvaModificate' : 'prof.WarningSalvaTutto';
    this.dialog.warningThen('prof.SalvataggioImpostazioni', content, true,
      () => {
        if (onlyTouched)
          this.impostazioniService.salvaImpostazioniModificate();
        else
          this.impostazioniService.salvaTutteImpostazioni();
      });
  }

  public showCopyProfileForm() {
    this.profileService.profileGridEvent$.pipe(
      filter(menuAction => menuAction.event === GridProfiliMenuActions.COPY),
      map(menuAction => menuAction.item),
      tap(selected => this.selected = selected),
      switchMap(selected => {
        const title = this.transloco.translate('prof.CopiaProfilo') + ' ' + selected.descrizione;
        this.copyProfileForm.controls['Description']
          .patchValue(selected.descrizione + ' - ' + this.transloco.translate('Copia'));
        return this.dialog.dialogMessageObs_Result(
          title, this.copyFormTemplate, undefined, undefined, undefined,
          (p) => this.copyProfileForm.invalid
        );
      }),
      filter(res => res['returnObj'] === true)
    ).subscribe(selected => this.copyProfile());
  }

  private copyProfile() {
    let original: TipologiaUtente = new TipologiaUtente(this.selected.codice, this.selected.descrizione);
    let copy = new TipologiaUtente(
      0, this.copyProfileForm.controls['Description'].value
    );
    this.profileService.copyTipologie(copy, original, this.copyProfileForm.controls['copySettings'].value);
    this.reloadComponent();
  }

  private reloadComponent() {
    let currentUrl = this.router.url;
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    this.router.onSameUrlNavigation = 'reload';
    this.router.navigate([currentUrl]);
  }

}
