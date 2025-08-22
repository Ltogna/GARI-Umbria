/* eslint-disable */
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { Contatto, PK } from 'app/Model/anagrafiche/Contatto';
import { Impresa } from 'app/Model/anagrafiche/Impresa';
import { RisorseUmane } from 'app/Model/anagrafiche/RisorseUmane';
import { FormeGiuridiche } from 'app/Model/metaschema/FormeGiuridiche';
import { FunzioniComuniService } from 'app/Service/FunzioniComuni.service';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { CODICI_TOKEN } from 'app/Utility/Template/codici-template/models/codici.model';
import { GiasMultiSelectTemplateService } from 'gias-ui-kit';
import { delay, lastValueFrom, map, of, Subject, Subscription, switchMap, take, tap } from 'rxjs';
import { AGRODATAFINE, AGRODATAINIZIO } from '../../../Model/CostantiPersonalizzate';
import { MasterService } from '../../../Service/master.service';
import { ObjParametriAgendaService } from '../../../Service/obj-parametri-agenda.service';
import { PivaValidatorService } from '../piva-validator.service';
import { ImpresaEditService } from './impresa-edit.service';
import { Location } from '@angular/common';
import { GiasMessageService } from 'app/Service/gias-message.service';
import { IMPRESE_SERVICE_TOKEN, ImpreseFactoryService } from 'app/Service/ServiceFactory/imprese.factory.service';
import { ImpreseServiceProvider } from 'app/Service/ServiceFactory/imprese.factory.provider';
import { RapportoContabile } from './contatti-edit/contatti-edit.service';
import { ContattiRootService } from './contatti-edit/contattiRoot.service';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import { DialogCloseResult } from '@progress/kendo-angular-dialog';
import { BaseCodeDescr } from '../../../Model/baseClass/baseCodeDescr';
import { GiasDropDownTemplateSComponent, ObjParametriAgenda } from 'gias-ui-kit';
import { GiiasMultiselectTemplateSComponent } from 'gias-ui-kit';
import { UtilityFunctions } from 'app/Utility/UtilityFunctions';
import { TranslocoService } from '@jsverse/transloco';
import { CooperativeConfigService } from './cooperative-edit/cooperative-edit-config.service';
import { ImpresaPadre } from 'app/Model/anagrafiche/ImpresaPadre';
import { GruppoRaccolta } from '../../../Model/metaschema/GruppoRaccolta';
import { GruppiRaccoltaService } from '../../../Service/GruppiRaccolta/gruppi-raccolta.service';
import { ConfigurazioneSitiService } from '../../../Service/configurazione-siti.service';
import { enum_TipoImpresaGerarchia } from "../../../Model/TipiEnumerativi";
import { LeggiVincoli, VincoliService } from 'app/Service/DPI/vincoli.service';
import { IntervalloTemporale } from 'app/Model/anagrafiche/IntervalloTemporale';
import { Vincolo } from 'app/Model/metaschema/Vincoli';
import { GiasDropDownTemplateComponent, RadioButtonValue } from 'gias-ui-kit';

@Component({
  standalone: false,
  selector: 'gias-impresa-edit',
  templateUrl: './impresa-edit.component.html',
  styleUrls: ['./impresa-edit.component.scss'],
  providers: [
    PivaValidatorService,
    { provide: CODICI_TOKEN, useClass: ImpresaEditService },
    GiasMultiSelectTemplateService,
    CooperativeConfigService,
    ImpreseServiceProvider
  ],
})

export class ImpresaEditComponent implements OnInit, OnDestroy {
  impresaSelezionata: boolean;
  objParametriAgenda: ObjParametriAgenda;
  formEnable: boolean;
  pivaEnable: boolean;

  FormeGiuridiche: FormeGiuridiche[];
  GruppiRaccolta: GruppoRaccolta[];
  DisciplinareAziendaliPredefiniti: any[];
  Tecnici: Contatto[];
  Odc: RisorseUmane[];
  Padri: ImpresaPadre[];
  ListaCertificazione: BaseCodeDescr[];
  defaultItemContatto = { primaryKey: { partitaIva: '', codice: '' }, descrizione: '' };
  defaultItem = { codice: 0, descrizione: '' };
  defaultItemDisciplinare = { value: 0, text: '' };

  impreseForm: FormGroup;

  TipoImpresa: Array<RadioButtonValue> = [
    { name: 'Impresa', value: enum_TipoImpresaGerarchia.Impresa, enable: true },
    { name: 'Cooperativa', value: enum_TipoImpresaGerarchia.Cooperativa, enable: true },
    { name: 'Consorzio', value: enum_TipoImpresaGerarchia.Consorzio, enable: true },
    { name: 'OP', value: enum_TipoImpresaGerarchia.OrganizzazioneProduttore, enable: true },
  ];

  data_inizio: Date = AGRODATAINIZIO;
  data_fine: Date = AGRODATAFINE;

  impresa: Impresa;
  formCaricaDati: Subscription;
  valueChangesSub: Subscription;
  mostraContatti: boolean;

  constructor(
    private masterService: MasterService,
    private objParametriAgendaService: ObjParametriAgendaService,
    @Inject(CODICI_TOKEN) private impresaEditService: ImpresaEditService,
    @Inject(IMPRESE_SERVICE_TOKEN) private impreseService: ImpreseFactoryService,
    private contattiRootService: ContattiRootService,
    private funzioniComuniService: FunzioniComuniService,
    private pivaValidatorService: PivaValidatorService,
    private giasMessageService: GiasMessageService,
    private location: Location,
    private router: Router,
    private giasDialogService: GiasDialogService,
    private transloco: TranslocoService,
    private gruppiRaccoltaservice: GruppiRaccoltaService,
    private configurazioneSitiService: ConfigurazioneSitiService,
    private vincoliService: VincoliService,
  ) { }


  ngOnInit() {
    this.impreseForm = this.impresaEditService.GetFormGroupImpresa();
    this.impreseForm.controls['partitaIva'].addAsyncValidators(this.pivaValidatorService.validate.bind(this));
    this.impreseForm.controls['partitaIva'].updateValueAndValidity();
    this.valueChangesSub = this.impreseForm.valueChanges.subscribe(form => {
      this.impresaEditService.updateForm(form);
    });
    this.initDati();
    this.contattiRootService.setSalvaInPadre(false);
    this.contattiRootService.setCreaContattiPubblici(false);
  }


  private initDati() {
    this.formCaricaDati = this.impresaEditService.caricaDati().subscribe((data) => {
      this.objParametriAgenda = data.ObjParamAgenda;
      this.impresa = data.Impresa;

      this.initImpresa(data.Impresa);
      this.FormeGiuridiche = data.FormeGiuridiche;
      this.Tecnici = data.Tecnici;
      this.Padri = data.Padri;
      this.Odc = data.Odc;
      this.abilitaDisabilitaForm();

      this.impresaSelezionata = true;
      this.masterService.setCompanyHeader(data.Impresa.ragioneSociale);
      this.mostraContatti = !this.isImpresaSuperUser();
    });
  }

  abilitaDisabilitaForm(): void {
    const operazione: Enum_DBTypeOperation = this.objParametriAgenda.TipoOperazioneDB;
    if (operazione == Enum_DBTypeOperation.Read) {
      this.formEnable = false;
      this.pivaEnable = false;
    } else if (operazione == Enum_DBTypeOperation.Update) {
      this.formEnable = true;
      this.pivaEnable = false;
    } else if (operazione == Enum_DBTypeOperation.Write) {
      this.formEnable = true;
      this.configurazioneSitiService.leggiChiave('Blocco_Inserimento_PIVA_Impresa').subscribe(r => {
        this.pivaEnable = !(r?.Valore?.toLowerCase() === 'true');
      });
    }
  }

  private initImpresa(impresa: Impresa) {
    this.impreseForm.patchValue(impresa);
    const operazione = this.objParametriAgenda.TipoOperazioneDB;
    if (operazione == Enum_DBTypeOperation.Update) {
      this.impreseForm.controls['partitaIva'].disable({ emitEvent: false });
    } else if (operazione == Enum_DBTypeOperation.Read) {
      this.impreseForm.disable({ emitEvent: false });
    } else {
      this.impreseForm.enable({ emitEvent: false });
    }
  }

  SalvaNuovoImpresa() {
    this.impreseForm.statusChanges.pipe(
      delay(300),
      take(1),
      tap((state) => {
        if (this.impreseForm.valid) {
          let impresa: Impresa = this.impreseForm.getRawValue();
          if (impresa.partitaIva == '') {
            let resp = this.giasDialogService.dialogMessageObs_Result(this.transloco.translate('Attenzione'),
              this.transloco.translate('PIVAVuotaConferma'),
              [
                { text: this.transloco.translate('Conferma'), primary: true, returnObj: true },
                { text: this.transloco.translate('Annulla'), returnObj: false }
              ],
              undefined,
              undefined,
              e => e instanceof DialogCloseResult);
            resp.pipe(take(1)).subscribe(val => {
              if ((<any>val).returnObj) {
                this.onSubmit().pipe(
                  tap((r) => {
                    if (r.RispostaOK) {
                      this.giasMessageService.successMessage(this.transloco.translate('Impresa') + ': ' + r.RispostaStringa.ragioneSociale + ' ' + this.transloco.translate('ModificataCorrettamente'));
                      this.masterService.set_isLoading({ message: '', isLoading: false });
                      this.objParametriAgenda.TipoOperazioneDB = Enum_DBTypeOperation.Write;
                      this.objParametriAgenda.Piva = '';
                      let currentUrl = this.router.url;
                      this.router.routeReuseStrategy.shouldReuseRoute = () => false;
                      this.router.onSameUrlNavigation = 'reload';
                      this.objParametriAgendaService.changeObjParametriAgenda(this.objParametriAgenda);
                      this.router.navigate([currentUrl]);
                    }
                  })
                ).subscribe();
              }
            })
          } else {
            this.onSubmit().pipe(
              tap(r => {
                this.giasMessageService.successMessage(
                  this.transloco.translate('Impresa') + ': ' + r.RispostaStringa.ragioneSociale + ' ' + this.transloco.translate('ModificataCorrettamente')
                );

                this.masterService.set_isLoading({ message: '', isLoading: false });
                this.objParametriAgenda.TipoOperazioneDB = Enum_DBTypeOperation.Write;
                this.objParametriAgenda.Piva = '';
                let currentUrl = this.router.url;
                this.router.routeReuseStrategy.shouldReuseRoute = () => false;
                this.router.onSameUrlNavigation = 'reload';
                this.objParametriAgendaService.changeObjParametriAgenda(this.objParametriAgenda);
                this.router.navigate([currentUrl]);
              })
            ).subscribe();
          }
        }
      })
    ).subscribe();

    this.impreseForm.markAllAsTouched();
    this.impreseForm.setValue(this.impreseForm.getRawValue());

  }

  SalvaImpresa() {
    this.impreseForm.statusChanges.pipe(
      delay(300),
      take(1),
      tap((state) => {
        if (this.impreseForm.valid) {
          let impresa: Impresa = this.impreseForm.getRawValue();

          if (impresa.partitaIva == '') {
            this.configurazioneSitiService.leggiChiave('Blocco_Inserimento_PIVA_Impresa').pipe(
              switchMap((conf) => {
                return of({ returnObj: (conf?.Valore?.toLowerCase() === 'true') });
              }), switchMap((conf) => {
                if (!conf.returnObj) {
                  let resp = this.giasDialogService.dialogMessageObs_Result(
                    'Attenzione',
                    this.transloco.translate('PIVAVuotaConferma'),
                    [
                      { text: this.transloco.translate('Conferma'), primary: true, returnObj: true },
                      { text: this.transloco.translate('Annulla'), returnObj: false }
                    ],
                    undefined,
                    undefined,
                    e => e instanceof DialogCloseResult
                  );
                  return resp;
                } else {
                  return of({ returnObj: true })
                }
              }), switchMap((val) => {
                if ((<any>val).returnObj) {
                  return this.onSubmit()
                } else {
                  return of(null)
                }
              }), switchMap((r) => {
                if (r.RispostaOK) {
                  this.giasMessageService.successMessage(this.transloco.translate('Impresa') + ': ' + r.RispostaStringa.ragioneSociale + ' ' + this.transloco.translate('ModificataCorrettamente'));
                  this.location.back();
                  this.masterService.set_isLoading({ message: '', isLoading: false });
                }
                return of(null)
              })
            ).subscribe();
          } else {
            this.onSubmit().pipe(
              tap((r) => {
                if (r.RispostaOK) {
                  this.giasMessageService.successMessage(this.transloco.translate('Impresa') + ': ' + r?.RispostaStringa.ragioneSociale + ' ' + this.transloco.translate('ModificataCorrettamente'));
                  this.location.back();
                  this.masterService.set_isLoading({ message: '', isLoading: false });
                }
              })
            ).subscribe();
          }
        }
      })
    ).subscribe();

    this.impreseForm.markAllAsTouched();
    this.impreseForm.setValue(this.impreseForm.getRawValue());
  }

  private onSubmit() {
    if (this.impreseForm.valid && (this.funzioniComuniService.codiciValidiNoDate(this.impresaEditService.codici).length == 0)) {
      let impresa: Impresa = this.impreseForm.getRawValue();
      impresa.codici = this.impresaEditService.codici;
      impresa.contatto_superuser = new Contatto();

      impresa.contatto_superuser.risorseUmane = this.contattiRootService.getRowsContatti().map(r => {
        let ris_um: RisorseUmane = new RisorseUmane();
        ris_um.codice = 0;
        ris_um.rapportoContabile = new RapportoContabile(r['codice']);
        ris_um.attivita = r['attivita'];
        ris_um.settore = r['settore'];
        return ris_um;
      });

      impresa.indirizzi.forEach(i => {
        if (!i.indirizzo.istatComune.prov) {
          i.indirizzo.istatComune.prov = '000';
        }
        if (!i.indirizzo.istatComune.com) {
          i.indirizzo.istatComune.com = '000';
        }
      });

      if (this.contattiRootService.getSalvaInPadre()) {
        impresa.contatto_superuser.primaryKey = new PK(impresa.impresaPadre[0].partitaIva, impresa.partitaIva);
      } else {
        impresa.contatto_superuser.primaryKey = new PK("", impresa.partitaIva);
      }

      impresa.contatto_superuser.visibilitaPubblica = this.contattiRootService.getCreaContattiPubblici();

      this.impresaEditService.updateForm(impresa);
      this.masterService.set_isLoading({ message: '', isLoading: true });
      console.log(impresa);
      return this.impreseService.ScriviImpresa(impresa, true);
    } else {
      return of(null);
    }
  }

  ngOnDestroy(): void {
    this.formCaricaDati.unsubscribe();
    this.valueChangesSub.unsubscribe();
  }

  isImpresaSuperUser(): boolean {
    return this.impresa.partitaIva == this.masterService.objP_server.PivaSuperUser
  }

  certificazioneChanged(ddlEl: GiasDropDownTemplateSComponent | GiiasMultiselectTemplateSComponent): void {
    UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.impreseService.leggi_Certificazioni().pipe(take(1))))
  }

  gruppoRaccoltaChanged(ddlEl: GiasDropDownTemplateComponent | GiasDropDownTemplateSComponent | GiiasMultiselectTemplateSComponent): void {
    let objParams: ObjParametriAgenda = this.objParametriAgendaService.getObjParamValue();
    objParams.Data = new Date();
    UtilityFunctions.loadDropDownItems(
      <GiasDropDownTemplateSComponent>ddlEl,
      lastValueFrom(this.gruppiRaccoltaservice.leggiGruppiRaccoltaValidi(objParams).pipe(take(1)))
    );
  }

  disciplinareAziendalePredefinitoChanged(ddlEl: GiasDropDownTemplateComponent | GiasDropDownTemplateSComponent | GiiasMultiselectTemplateSComponent): void {
    let intervallo: IntervalloTemporale = new IntervalloTemporale();

    let validita: LeggiVincoli = {
      validita: intervallo
    }
    UtilityFunctions.loadDropDownItems(
      <GiasDropDownTemplateSComponent>ddlEl,
      lastValueFrom(this.vincoliService.leggiVincoliOrdered(intervallo)
        .pipe(take(1), map(x => this.adattaVincoli(x)))));
  }

  adattaVincoli(vincoli: Vincolo[]) {
    for (var elem of vincoli) {
      let codiceAdattato = this.adattaCodice(elem.codice);
      elem.codice = codiceAdattato;
    }

    return vincoli;
  }

  adattaCodice(codice: string) {
    let split = codice.split('_');
    if (split.length > 1)
      return (split[1] + '/' + split[0]);
    else
      return codice;
  }
}
