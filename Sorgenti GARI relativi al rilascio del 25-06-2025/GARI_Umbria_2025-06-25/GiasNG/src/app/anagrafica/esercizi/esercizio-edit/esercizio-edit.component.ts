import { Component, EventEmitter, Inject, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { ControlContainer, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { TranslocoService } from '@jsverse/transloco';
import { Appezzamento } from 'app/Model/anagrafiche/Appezzamento';
import { Contatto } from 'app/Model/anagrafiche/Contatto';
import { Esercizio } from 'app/Model/anagrafiche/Esercizio';
import { Fabbricato } from 'app/Model/anagrafiche/Fabbricato';
import { Impresa } from 'app/Model/anagrafiche/Impresa';
import { Lavorazione } from 'app/Model/attivita/Lavorazione';
import { BaseCodeDescr } from 'app/Model/baseClass/baseCodeDescr';
import { BaseCodeDescrStr } from 'app/Model/baseClass/baseCodeDescrStr';
import { AGRODATAFINE, AGRODATAINIZIO } from 'app/Model/CostantiPersonalizzate';
import { enum_Impostazioni_Utenti } from 'app/Model/Impostazioni_Utenti.enum';
import { Disciplinare } from 'app/Model/metaschema/Disciplinari';
import { FaseCicloColturale } from 'app/Model/metaschema/fase';
import { FinalitaPianoConcimazione } from 'app/Model/metaschema/FinalitaPianoConcimazione';
import { Regolamenti } from 'app/Model/metaschema/Regolamenti';
import { RegolamentoConcimazione } from 'app/Model/metaschema/RegolamentoConcimazione';
import { GruppoFinalita } from 'app/Model/metaschema/utilizzi/GruppoFinalita';
import { Specie } from 'app/Model/metaschema/utilizzi/Specie';
import { UtilizzoTerreno } from 'app/Model/metaschema/utilizzi/UtilizzoTerreno';
import { Varieta } from 'app/Model/metaschema/utilizzi/Varieta';
import { ContattiService } from 'app/Service/Anagrafica/contatti.service';
import { CodificaInfoAggiuntiveService, enum_CAC_Codifica_InfoAggiuntive_ArgomentoCod } from 'app/Service/Codifiche/codifica_InfoAggiuntive.service';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import { GiasMessageService } from 'app/Service/gias-message.service';
import { DisciplinariService, LeggiDisciplinare, LeggiIAF } from 'app/Service/Metaschema/disciplinari.service';
import { GruppoFinalitaService } from 'app/Service/Metaschema/finalita.service';
import { PianoConcimazioneService } from 'app/Service/Metaschema/pianoConcimazione.service';
import { PuaService } from 'app/Service/Metaschema/pua.service';
import { RegolamentiService } from 'app/Service/Metaschema/regolamenti.service';
import { TabelleWsClientService } from 'app/Service/Metaschema/tabelleWsClient.service';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { ImpreseFactoryService, IMPRESE_SERVICE_TOKEN } from 'app/Service/ServiceFactory/imprese.factory.service';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { CODICI_TOKEN } from 'app/Utility/Template/codici-template/models/codici.model';
import { CodiciTemplateConfigService } from 'app/Utility/Template/codici-template/services/codici-template-config.service';
import { GiasDropDownTemplateSComponent, ObjParametriAgenda } from 'gias-ui-kit';
import { generateGridProviders } from 'gias-kendo-grid';
import { GiiasMultiselectTemplateSComponent } from 'gias-ui-kit';
import { GiasMultiSelectTemplateService } from 'gias-ui-kit';
import { UtilityFunctions } from 'app/Utility/UtilityFunctions';
import { isNumber } from 'lodash';
import {
  audit,
  delay,
  from,
  lastValueFrom,
  map,
  Observable,
  of,
  startWith,
  Subject,
  switchMap,
  take,
  takeUntil,
  tap
} from 'rxjs';
import { EserciziEditService } from './esercizio-edit-codici.service';
import { VincoliService } from "../../../Service/DPI/vincoli.service";
import { IntervalloTemporale } from 'app/Model/anagrafiche/IntervalloTemporale';
import { Vincolo } from "../../../Model/metaschema/Vincoli";
import { ImpresaPadre } from 'app/Model/anagrafiche/ImpresaPadre';
import { GruppoRaccolta } from '../../../Model/metaschema/GruppoRaccolta';
import { LeggiProdotti, ProdottiService } from 'app/Service/Anagrafica/prodotti.service';
import { DettaglioRaccolta } from 'app/Model/attivita/dettagli/DettaglioRaccolta';
import { GruppiRaccoltaService } from '../../../Service/GruppiRaccolta/gruppi-raccolta.service';
import { Impianto } from '../../../Model/anagrafiche/Impianto';
import { DatiPrevisionaliColtureRequest } from '../../../Model/anagrafiche/DatiPrevisionaliColture';
import { ImpiantiService } from '../../../Service/Anagrafica/impianti.service';
import { ApportoMacroelementi } from "../../../Model/metaschema/ApportoMacroelementi";
import { FiltroCalcoloNPK } from "../../../Model/filtri/filtroCalcoloNPK";
import { AppezzamentoEditService } from 'app/anagrafica/appezzamenti/appezzamenti-edit/appezzamento-edit.service';
import { UnitaDiMisura } from '../../../Model/metaschema/UnitaDiMisura';
import { enum_UnitaMisura } from '../../../Model/TipiEnumerativi';
import { UnitaDiMisuraService } from '../../../Service/Metaschema/UnitaDiMisura.service';
import { rispostaStandard } from '../../../Service/master.service';
import { IndirizzoAssociato } from '../../../Model/anagrafiche/IndirizzoAssociato';
import { CentriAziendaliService, LeggiIndirizziCentro } from '../../../Service/Anagrafica/centri.service';
import { IMPIANTI_SERVICE_TOKEN, ImpiantiFactoryService } from '../../../Service/ServiceFactory/impianti.factory.service';
import { ContributeService } from '../../../Service/Metaschema/contribute.service';
import { Contribute, ContributeType } from '../../../Model/metaschema/Contribute';
import { GiasDropDownTemplateComponent, GiasIstatService } from 'gias-ui-kit';

@Component({
  standalone: false,
  selector: 'app-esercizio-edit',
  templateUrl: './esercizio-edit.component.html',
  styleUrls: ['./esercizio-edit.component.css'],
  providers: [
    GiasMultiSelectTemplateService, { provide: CODICI_TOKEN, useClass: EserciziEditService },
    ...generateGridProviders(CodiciTemplateConfigService, EsercizioEditComponent)
  ]
})
export class EsercizioEditComponent implements OnInit, OnDestroy {
  @Input('esercizio') EsercizioEditForm: FormGroup;
  @Output() faseChangeEvent = new EventEmitter<{ fase: number, id: number, validitaInizio: Date; }>();
  @Output() validitaInizioChangeEvent = new EventEmitter<{ fase: number, id: number, validitaInizio: Date; }>();
  @ViewChild('descriptionsDdl') descriptionsDdl: TemplateRef<any>;

  descrizioneEserciziForm: FormGroup;

  signal$: Subject<void> = new Subject();
  validitaBlur$: Subject<void> = new Subject<void>();

  protected GruppiRaccolta: Array<GruppoRaccolta>;
  protected AGRODATA_INIZIO: Date = AGRODATAINIZIO;
  protected AGRODATA_FINE: Date = AGRODATAFINE;

  protected ListaCertificazioneAziendale: Array<BaseCodeDescr>;

  protected defaultItem = { codice: 0, descrizione: '' };
  protected defaultItemStr = { codice: '', descrizione: '' };
  protected defaultItemContatto = { primaryKey: { partitaIva: '', codice: '' }, descrizione: '' };

  protected defaultItemDisciplinare: Disciplinare = {
    codice: '',
    descrizione: '',
    disciplinarePubblicoPrivato: 1,
    regolamentoConcimazione: { codice: 0, descrizione: '', tipo: 0 },
    idTr: 3,
    raggruppamentiColturaliDPI: null,
    gruppoFinalita: null,
    flagProtetto: 0,
    validita: new IntervalloTemporale()
  };

  private leggiProdotti: LeggiProdotti = {
    impresa: new Impresa(),
    tipoAttivita: undefined,
    statoAttivita: undefined,
    lavorazione: undefined,
    impianti: [],
    prodottiDaTrattare: [],
    specie: undefined,
    disciplinare: undefined,
    epocaDPI: undefined,
    avversitaGruppo: undefined,
    filtroPerDescrizione: '',
    data: undefined,
    escludiGiacenzeZero: false,
    magazziniAgenzie: false,
    magazziniEsterni: false
  };

  private defaultItemVincolo: Vincolo = {
    codice: '1',
    descrizione: 'Nessuno',
    disciplinare: this.defaultItemDisciplinare,
    regolamento: { codice: 1, descrizione: '' }
  };

  private objParametriAgenda: ObjParametriAgenda;
  private lastUtilizzoTerreno: UtilizzoTerreno;
  private specie: Specie;

  private ImpiantoEditForm: FormGroup;
  private AppezzamentoEditForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private objParametriAgendaService: ObjParametriAgendaService,
    private regolamentiService: RegolamentiService,
    private disciplinariService: DisciplinariService,
    private codificaInfoAggiuntiveService: CodificaInfoAggiuntiveService,
    private controlContainer: ControlContainer,
    private contattiService: ContattiService,
    private tabelleWsClientService: TabelleWsClientService,
    private puaService: PuaService,
    private PianoConcimazioneService: PianoConcimazioneService,
    private finalitaService: GruppoFinalitaService,
    private vincoliService: VincoliService,
    private prodottiService: ProdottiService,
    private giasMessageService: GiasMessageService,
    private giasDialogService: GiasDialogService,
    @Inject(CODICI_TOKEN) private esercizioCodiciService: EserciziEditService,
    private translocoService: TranslocoService,
    @Inject(IMPRESE_SERVICE_TOKEN) private impreseService: ImpreseFactoryService,
    @Inject(IMPIANTI_SERVICE_TOKEN) private appezzamentiService: ImpiantiFactoryService,
    private permessiUtenteService: PermessiUtenteService,
    private gruppiRaccoltaservice: GruppiRaccoltaService,
    private impiantiService: ImpiantiService,
    private appezzamentoEditService: AppezzamentoEditService,
    private unitaMisuraService: UnitaDiMisuraService,
    private centriService: CentriAziendaliService,
    private istatService: GiasIstatService,
    private contributeService: ContributeService
  ) {
    this.descrizioneEserciziForm = this.fb.group({
      des: new FormControl({ codice: 0, descrizione: '' })
    });
  }

  ngOnDestroy(): void {
    this.signal$.next();
    this.signal$.complete();
  }

  async ngOnInit() {

    this.EsercizioEditForm.get('vincolo').valueChanges.subscribe((val) => {
      if (val == undefined) {
        console.log(val);
      }
    });

    (<FormGroup>this.EsercizioEditForm.controls['validita']).controls['inizio'].valueChanges.pipe(
      tap((val) => {
        console.log("change: " + val);
      })).subscribe();

    this.ImpiantoEditForm = <FormGroup>this.controlContainer.control;
    this.AppezzamentoEditForm = (<FormGroup>this.ImpiantoEditForm.parent.parent);
    this.ImpiantoEditForm.controls['utilizzoTerreno'].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val: UtilizzoTerreno) => {
      if (this.lastUtilizzoTerreno == null) {
        this.lastUtilizzoTerreno = val;
        return;
      }
      if (this.lastUtilizzoTerreno.classType != val.classType) {
        if (val.classType == 'Varieta') {
          this.specieChanged((<Varieta>val).specie);
        } else {
          this.specieChanged({ codice: 0, descrizione: '' });
        }
      } else if (val.classType == 'Varieta') {
        if ((<Varieta>this.lastUtilizzoTerreno).codice != (<Varieta>val).codice) {
          this.resetProdotto();
        }

        if ((<Varieta>this.lastUtilizzoTerreno)?.specie?.codice != (<Varieta>val)?.specie?.codice) {
          this.specieChanged((<Varieta>val).specie);
        }
      }
      this.lastUtilizzoTerreno = val;
    });

    this.esercizioCodiciService.setCodici(this.EsercizioEditForm.value.codici);
    this.EsercizioEditForm.controls["codici"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((el) => {
      this.esercizioCodiciService.setCodici(el);
    });

    this.esercizioCodiciService.currentodici.pipe(takeUntil(this.signal$)).subscribe((vals) => {
      this.EsercizioEditForm.controls['codici'].setValue(vals, { emitEvent: false });
    });

    let vincoloSelected = this.EsercizioEditForm.get('vincolo').getRawValue();
    this.leggiVincoli().then((val) => {
      if (vincoloSelected) {
        if (vincoloSelected.codice && vincoloSelected.codice != '1' && vincoloSelected.codice != '4') {
          let v = val.find((el) => {
            return el.codice.split('_')[1] == vincoloSelected.codice.split('_')[1];
          });
          if (v) {
            this.EsercizioEditForm.get('vincolo').setValue(v, { emitEvent: false });
          }
        } else {
          let v = val.find((el) => {
            return el.codice == vincoloSelected.codice;
          });
          if (v) {
            this.EsercizioEditForm.get('vincolo').setValue(v, { emitEvent: false });
          }
        }
      }
    });

    if (this.objParametriAgendaService.getObjParamValue().TipoOperazioneDB == Enum_DBTypeOperation.Write) {
      from(this.impreseService.leggiImpresa(this.objParametriAgendaService.getObjParamValue())).pipe(take(1), map(impresa => {
        if (impresa.RispostaStringa.certificazione.length != 0) {
          this.EsercizioEditForm.controls['certificazioneAziendale'].setValue(impresa.RispostaStringa.certificazione);
        }

        if (this.permessiUtenteService.getImpostazione_SuperUser(enum_Impostazioni_Utenti.SUPERUSER_Smart_NuovoImpianto_DefaultOrganismoReferente)?.Valore == '1') {
          from(this.leggiOrganismoReferente()).pipe(take(1), map(o => {
            let OrgRef = o.filter(t => t.tipo == 'azienda');
            if (OrgRef.length == 1) {
              let azienda: Contatto = OrgRef[0];
              let descrizione;
              if (azienda.ragione_Sociale != null && azienda.ragione_Sociale != "") {
                descrizione = azienda.ragione_Sociale;
              } else {
                descrizione = azienda.cognome + ' ' + azienda.nome;
              }
              this.EsercizioEditForm.controls['organismo_Referente'].setValue({ codice: azienda.primaryKey.codice, descrizione: descrizione });
            }
          })).subscribe();
        }

        if (this.permessiUtenteService.getImpostazione_SuperUser(enum_Impostazioni_Utenti.SUPERUSER_NuovoImpianto_DefaultTecnico)?.Valore == '1') {
          if (impresa.RispostaStringa.tecnicoReferente) {
            let tecnico: Contatto = impresa.RispostaStringa.tecnicoReferente;
            this.EsercizioEditForm.controls['tecnico'].setValue([{ codice: tecnico.primaryKey.codice, descrizione: tecnico.ragione_Sociale }]);
          }
        }

        if (this.permessiUtenteService.getImpostazione_SuperUser(enum_Impostazioni_Utenti.SUPERUSER_COD_ORG_REFERENTE_DA_PADRE)?.Valore == '1') {
          let padriImpresa = impresa.RispostaStringa.impresaPadre;
          if (padriImpresa.length == 1) {
            let azienda: ImpresaPadre = padriImpresa[0];
            this.EsercizioEditForm.controls['organismo_Referente'].setValue({ codice: azienda.partitaIva, descrizione: azienda.ragioneSociale });
          }
        }
      })).subscribe();
    }

    (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["tipologia"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((el) => {
      if (el.codice == 0) {
        (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["n"].setValue(0);
        (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["p2o5"].setValue(0);
        (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["k2o"].setValue(0);
        (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["mgo"].setValue(0);
      }

      this.impostaNPK();
    });

    (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["fase"].valueChanges.pipe(
      takeUntil(this.signal$),
      delay(0)
    ).subscribe(
      {
        next: (v) => {
          this.impostaNPK();
          const fase = (<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['fase'].value.codice;
          const validitaInizio: Date = (<FormGroup>this.EsercizioEditForm.controls['validita']).controls['inizio'].value;
          const id: number = this.EsercizioEditForm.controls['id'].value;
          this.faseChangeEvent.emit({ fase: fase, id: id, validitaInizio: validitaInizio });
        },
        error: (e) => console.error(e),
        complete: () => {
        }
      }
    );

    (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["pianoConcimazione"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((el) => {
      lastValueFrom(this.leggiFinalitaConcimazione().pipe(take(1))).then(val => {
        if (val.length > 0) {
          (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["tipologia"].setValue(val[0]);
        } else {
          (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["tipologia"].setValue(this.defaultItem);
        }
      })
    });

    (<FormGroup>this.EsercizioEditForm.controls['validita']).controls['inizio'].valueChanges.pipe(
      takeUntil(this.signal$),
      audit(ev => this.validitaBlur$)
    ).subscribe(val => {
      const id: number = this.EsercizioEditForm.controls['id'].value;
      const fase = (<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['fase'].value.codice;
      this.validitaInizioChangeEvent.emit({ fase: fase, id: id, validitaInizio: val });
    });

    this.EsercizioEditForm.controls["disciplinare"].valueChanges.pipe(takeUntil(this.signal$)).subscribe(async (el: Disciplinare) => {
      if (el?.regolamentoConcimazione) {
        let a = await lastValueFrom(this.puaService.leggiRegolamentoConcimazionexImpianto(this.EsercizioEditForm.controls["validita"].value).pipe(take(1)));
        const regFert = a.find((val) => val.codice == el.regolamentoConcimazione.codice);

        if (regFert != null && regFert != undefined) {
          (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["pianoConcimazione"].setValue(regFert);
        } else {
          (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["pianoConcimazione"].setValue({ codice: 0, descrizione: '' });
        }
        this.EsercizioEditForm.controls["iaf"].patchValue([]);
      }

    });

    this.EsercizioEditForm.controls["piante_Ha"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val) => {
      if (isNumber(val) && isNumber(this.ImpiantoEditForm.controls["superficie"].value)) {
        if (val > 0 && this.ImpiantoEditForm.controls["superficie"].value > 0) {
          this.EsercizioEditForm.controls['piante_Impianto'].setValue(Math.round(val * this.ImpiantoEditForm.controls["superficie"].value), { emitEvent: false });
        }
      }
    });

    this.EsercizioEditForm.controls["piante_Impianto"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val: number) => {
      if (isNumber(val) && isNumber(this.ImpiantoEditForm.controls["superficie"].value)) {
        if (val > 0 && this.ImpiantoEditForm.controls["superficie"].value > 0) {
          this.EsercizioEditForm.controls['piante_Ha'].setValue(Math.round(val / this.ImpiantoEditForm.controls["superficie"].value), { emitEvent: false });
        }
      }
    });

    this.EsercizioEditForm.controls["piante_Ha_Femmine"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val) => {
      if (isNumber(val) && isNumber(this.ImpiantoEditForm.controls["superficie"].value)) {
        if (val > 0 && this.ImpiantoEditForm.controls["superficie"].value > 0) {
          this.EsercizioEditForm.controls['Piante_Ha_Impianto_Femmine'].setValue(Math.round(val * this.ImpiantoEditForm.controls["superficie"].value), { emitEvent: false });
        }
      }
    });

    this.EsercizioEditForm.controls["Piante_Ha_Impianto_Femmine"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val: number) => {
      if (isNumber(val) && isNumber(this.ImpiantoEditForm.controls["superficie"].value)) {
        if (val > 0 && this.ImpiantoEditForm.controls["superficie"].value > 0) {
          this.EsercizioEditForm.controls['piante_Ha_Femmine'].setValue(Math.round(val / this.ImpiantoEditForm.controls["superficie"].value), { emitEvent: false });
        }
      }
    });

    this.EsercizioEditForm.controls["Piante_Ha_Maschi"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val) => {
      if (isNumber(val) && isNumber(this.ImpiantoEditForm.controls["superficie"].value)) {
        if (val > 0 && this.ImpiantoEditForm.controls["superficie"].value > 0) {
          this.EsercizioEditForm.controls['Piante_Ha_Impianto_Maschi'].setValue(Math.round(val * this.ImpiantoEditForm.controls["superficie"].value), { emitEvent: false });
        }
      }
    });

    this.EsercizioEditForm.controls["Piante_Ha_Impianto_Maschi"].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val: number) => {
      if (isNumber(val) && isNumber(this.ImpiantoEditForm.controls["superficie"].value)) {
        if (val > 0 && this.ImpiantoEditForm.controls["superficie"].value > 0) {
          this.EsercizioEditForm.controls['Piante_Ha_Maschi'].setValue(Math.round(val / this.ImpiantoEditForm.controls["superficie"].value), { emitEvent: false });
        }
      }
    });

    const esercizioInitVal: Esercizio = this.EsercizioEditForm.value;
    if (esercizioInitVal.apportiMassimiMacroelementi.pianoConcimazione?.codice > 0) {
      const RegolamentiConcimazioni = await lastValueFrom(this.puaService.leggiRegolamentoConcimazionexImpianto(this.EsercizioEditForm.controls["validita"].value).pipe(take(1)));
      const RegolamentoConcimazioneSel = RegolamentiConcimazioni.find((el) => el.codice == esercizioInitVal.apportiMassimiMacroelementi.pianoConcimazione.codice);
      if (RegolamentoConcimazioneSel) {
        (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["pianoConcimazione"].setValue(RegolamentoConcimazioneSel, { emitEvent: false });
      }
    }

    this.EsercizioEditForm.controls["vincolo"].valueChanges.pipe(
      takeUntil(this.signal$)
    ).subscribe((vincolo: Vincolo) => {
      if (vincolo) {
        this.EsercizioEditForm.controls["disciplinare"].setValue(vincolo.disciplinare);
        (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["pianoConcimazione"].setValue(vincolo.disciplinare.regolamentoConcimazione);
        this.EsercizioEditForm.controls["regolamento"].setValue(vincolo.regolamento);
      }
    })

    this.setResaTotalePrevistaSubs()

    const prodottoControl = this.EsercizioEditForm.get('prodotto');
    if (prodottoControl != null) {
      prodottoControl.valueChanges
        .pipe(
          takeUntil(this.signal$),
          tap((value: DettaglioRaccolta) => {
            const varietaControl = this.ImpiantoEditForm.controls['utilizzoTerreno'];
            this.lastUtilizzoTerreno = value.varieta;
            if (value.varieta != null && varietaControl.value?.codice == 0) {
              varietaControl.setValue(value.varieta);
            }

            const finalitaControl = this.ImpiantoEditForm.get('gruppoFinalita');
            if (value.finalita != null && finalitaControl.value?.codice == 0) {
              finalitaControl.setValue(value.finalita);
            }

            const vincoloControl = this.EsercizioEditForm.controls['vincolo'];
            if (value.regolamento?.codice != null && value.regolamento?.codice == 1 && (vincoloControl?.value?.codice == 0 || vincoloControl?.value?.codice == null || vincoloControl?.value?.codice == '4')) {
              this.leggiVincoli().then(vincoli => {
                const vincolo = vincoli.find(v => v.regolamento?.codice == value.regolamento?.codice);
                if (vincolo) {
                  this.EsercizioEditForm.get('vincolo').setValue(vincolo);
                  let metodoProduzioneControl = this.AppezzamentoEditForm.controls['metodo_Produzione'];
                  if (metodoProduzioneControl.value && metodoProduzioneControl.value.codice == 3) {
                    this.AppezzamentoEditForm.controls['metodo_Produzione'].setValue({ descrizione: 'Integrato', codice: 1 });
                  }
                }
              });
            } else if (value.regolamento?.codice != null && value.regolamento?.codice == 4 && (vincoloControl?.value == null || vincoloControl?.value?.codice != 4)) {
              this.leggiVincoli().then(vincoli => {
                const vincolo = vincoli.find(v => v.regolamento?.codice == value.regolamento?.codice);
                if (vincolo) {
                  this.EsercizioEditForm.get('vincolo').setValue(vincolo);
                  let metodoProduzioneControl = this.AppezzamentoEditForm.controls['metodo_Produzione'];
                  if (metodoProduzioneControl.value && metodoProduzioneControl.value.codice != 3) {
                    this.AppezzamentoEditForm.controls['metodo_Produzione'].setValue({ descrizione: 'Biologico', codice: 3 });
                  }
                }

              });
            }
          })
        ).subscribe();
    }

    this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
    this.disabilitacontrolli();

  }

  showDescriptionButton(): boolean {
    return this.ImpiantoEditForm.controls['algoritmoCodifica'].value !== '';
  }

  chooseDescription(): void {
    this.giasDialogService.dialogMessageObs_Result('', this.descriptionsDdl)
      .pipe(take(1))
      .subscribe((r) => {
        if (r['returnObj']) {
          this.EsercizioEditForm.controls['descrizione'].setValue(this.descrizioneEserciziForm.value.des.descrizione);
        }
      });
  }

  calcoloNPKModello(filtro: FiltroCalcoloNPK): Observable<ApportoMacroelementi> {
    if (!this.appezzamentoEditService.savings.value) {
      return this.PianoConcimazioneService.calcoloNPKModello(filtro);
    } else {
      return of(<ApportoMacroelementi>{});
    }
  }

  disabilitacontrolli(): void {
    // Se sono in Lettura disabilito tutti i controlli
    if (this.objParametriAgenda.TipoOperazioneDB === Enum_DBTypeOperation.Read) {
      this.EsercizioEditForm.disable();
    }
  }

  async openDdl(ddlEl: GiasDropDownTemplateSComponent | GiiasMultiselectTemplateSComponent) {
    switch (ddlEl.giasFormControlName) {
      case 'regolamento':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.regolamentiService.leggi().pipe(take(1))))
        break;
      case 'disciplinare':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.LeggiDisciplinari().pipe(take(1))))
        break;
      case 'iaf':
        ddlEl.listItems = await lastValueFrom(this.LeggiIAF().pipe(take(1)));
        break;
      case 'capitolato_Privato':
        //ddlEl.listItems = await this.codificaInfoAggiuntiveService.leggiCapitolatoPrivato();
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.codificaInfoAggiuntiveService.leggiCapitolatoPrivato())
        break;
      case 'residuo':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.codificaInfoAggiuntiveService.leggiResiduiDisponibili())
        break;
      case 'certificazioneAziendale':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.impreseService.leggi_Certificazioni().pipe(take(1))))
        break;
      case 'certificazioneProdotto':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.codificaInfoAggiuntiveService.leggiCertProdDisponibili())
        break;
      case 'contributi':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.esercizioCodiciService.leggiContributi())
        break;
      case 'tecnico':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.impreseService.leggiCombo_Tecnici(
          this.objParametriAgenda.Piva).then(
            risposta => {
              return risposta.RispostaStringa.map(el => {
                return { codice: el.primaryKey.codice, descrizione: el.ragione_Sociale } as BaseCodeDescrStr;
              })
            }))
        break;
      case 'organismo_Referente':
        ddlEl.listItems = (await this.leggiOrganismoReferente()).map((el: Contatto) => {
          let descrizione;
          if (el.ragione_Sociale != null && el.ragione_Sociale != "") {
            descrizione = el.ragione_Sociale;
          } else {
            descrizione = el.cognome + ' ' + el.nome;
          }
          return { codice: el.primaryKey.codice, descrizione: descrizione }
        });
        break;
      case 'licenza_Coltivazione':
        //ddlEl.listItems = await this.leggiLicenzaColtivazione();
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.leggiLicenzaColtivazione())
        break;
      case 'riferimento_Trasferimento_Dati':
        ddlEl.listItems = (await this.leggiRiferimento_Trasferimento_Dati()).map((el: Contatto) => {
          let descrizione;
          if (el.ragione_Sociale != null && el.ragione_Sociale != "") {
            descrizione = el.ragione_Sociale;
          } else {
            descrizione = el.cognome + ' ' + el.nome;
          }
          return { codice: el.primaryKey.codice, descrizione: descrizione }
        });
        break;
      case 'magazzino_Conferimento':
        ddlEl.listItems = (await this.leggiMagazzinoConferimento()).map((el: Fabbricato) => {
          return {
            codice: el.primaryKey.codice + '|' + el.primaryKey.centroAziendalePK.codice + '|' + el.primaryKey.centroAziendalePK.partitaIva,
            descrizione: el.descrizione
          }
        });
        break;
      case 'piano_Semina':
        //ddlEl.listItems = await this.codificaInfoAggiuntiveService.leggiPianiSemina();
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.codificaInfoAggiuntiveService.leggiPianiSemina())
        break;
      case 'pianoConcimazione':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.puaService.leggiRegolamentoConcimazionexImpianto(this.EsercizioEditForm.controls["validita"].value).pipe(take(1))))
        break;
      case 'tipologia':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.leggiFinalitaConcimazione()))
        break;
      case 'fase':
        //ddlEl.listItems = await this.leggiStatoImpianto();
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, new Promise((resolve, reject) => this.leggiStatoImpianto().GiasSubscribe(r => resolve(r))))
        break;
      case 'lavorazione':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, new Promise((resolve, reject) => this.leggiLavorazione().GiasSubscribe(r => resolve(r))))
        break;
      case 'specifica':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, new Promise((resolve, reject) => this.leggiSpecifica().GiasSubscribe(r => resolve(r))))
        break;
      case 'vincolo':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, this.leggiVincoli())
        break;
      case 'prodotto':
        const varieta = this.ImpiantoEditForm.controls['utilizzoTerreno'].value as Varieta;
        const finalita = this.ImpiantoEditForm.get('gruppoFinalita').value as GruppoFinalita;
        const regolamento = this.EsercizioEditForm.get('vincolo')?.value?.regolamento as Regolamenti | null;
        const impresa = new Impresa();
        impresa.partitaIva = this.objParametriAgendaService.getObjParamValue().Piva;
        if (varieta.descrizione.toLowerCase() == 'altre') {
          varieta.codice = 0;
        }
        const leggiProdotti = {
          ...this.leggiProdotti,
          impresa: impresa,
          specie: varieta.specie,
          varieta: varieta,
          finalita: finalita,
          regolamento: regolamento
        } as LeggiProdotti;
        const items = lastValueFrom(this.prodottiService.Leggi_Trasformati_Vegetali_Anagrafica_With_Default(leggiProdotti));
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, items);
        break;
      case 'des':
        UtilityFunctions.loadDropDownItems(<GiasDropDownTemplateSComponent>ddlEl, lastValueFrom(this.appezzamentiService.generateDescriptions(this.objParametriAgenda.Piva)))
        break;
      case 'acaContributes':
        UtilityFunctions.loadDropDownItems(
          <GiasDropDownTemplateSComponent>ddlEl,
          lastValueFrom(this.contributeService.read(new Contribute(0, ContributeType.ACA)).pipe(map(r => r.RispostaStringa)))
        );
        break;
    }
  }

  calcolaSuperficieClick() {
    const superficie = this.calcolaSuperficie().superficie;
    let appezzamento: Appezzamento = this.AppezzamentoEditForm.getRawValue();
    if (appezzamento.catastoAppezzamento.length > 0) {
      this.giasMessageService.warningMessage(this.translocoService.translate('NonEPossibileModificareSuperficie'));
      return;
    }
    if (superficie > 0) {
      this.ImpiantoEditForm.controls['superficie'].setValue(superficie);
      this.AppezzamentoEditForm.controls['superficie'].setValue(superficie);
      this.giasDialogService.alertMessage("Impostata superficie di " + superficie + "ha", null, 3)
    }
  }

  calcolaPianteClick(): void {
    const pianteCalcolate = this.calcolaPiante();
    if (pianteCalcolate.piante_ha > 0 && pianteCalcolate.piante_impianto > 0) {
      const currentPiante_Ha = this.EsercizioEditForm.controls['piante_Ha'].value;
      const currentPiante_Impianto = this.EsercizioEditForm.controls['piante_Impianto'].value;

      if (currentPiante_Ha == 0 && currentPiante_Impianto == 0) {
        this.assegnaValoriCalcolaPiante(pianteCalcolate.piante_ha, pianteCalcolate.piante_impianto)
        return;
      }

      if (currentPiante_Ha != pianteCalcolate.piante_ha || currentPiante_Impianto != pianteCalcolate.piante_impianto) {
        this.giasDialogService.dialogMessageObs_Result(
          this.translocoService.translate('Attenzione'),
          this.translocoService.translate('PianteDifferisconoModificare')).pipe(
            take(1),
            tap((res) => {
              if ((<any>res).returnObj) {
                this.assegnaValoriCalcolaPiante(pianteCalcolate.piante_ha, pianteCalcolate.piante_impianto)
              }
            })
          ).pipe(takeUntil(this.signal$)).subscribe();
      } else {

      }

    }
  }

  specieSelezionata(): boolean {
    var specie_specified = false;
    var val: UtilizzoTerreno = this.ImpiantoEditForm.controls['utilizzoTerreno'].value
    if (val.classType == 'Varieta') {
      if ((<Varieta>val).specie.codice > 0) {
        specie_specified = true;
      }
    }

    return specie_specified;
  }

  gruppoRaccoltaChanged(ddlEl: GiasDropDownTemplateComponent | GiasDropDownTemplateSComponent | GiiasMultiselectTemplateSComponent): void {
    let objParams: ObjParametriAgenda = this.objParametriAgendaService.getObjParamValue();
    objParams.Data = new Date();
    UtilityFunctions.loadDropDownItems(
      <GiasDropDownTemplateSComponent>ddlEl,
      lastValueFrom(this.gruppiRaccoltaservice.leggiGruppiRaccoltaValidi(objParams).pipe(take(1)))
    );
  }

  private specieChanged(specie: Specie) {
    this.specie = specie;

    if (this.specie.codice == 0) {
      this.EsercizioEditForm.get('piante_Impianto').setValue(0);
      this.EsercizioEditForm.get('piante_Ha_Femmine').setValue(0);
      this.EsercizioEditForm.get('Piante_Ha_Impianto_Femmine').setValue(0);
      this.EsercizioEditForm.get('Piante_Ha_Maschi').setValue(0);
      this.EsercizioEditForm.get('Piante_Ha_Impianto_Maschi').setValue(0);

      this.EsercizioEditForm.get('data_Semina_Trapianto_Prevista').setValue(AGRODATAINIZIO);
      this.EsercizioEditForm.get('data_Raccolta_Prevista').setValue(AGRODATAINIZIO);
      this.EsercizioEditForm.get('data_Fioritura_Prevista').setValue(AGRODATAINIZIO);
      this.EsercizioEditForm.get('resa_prevista').setValue(0);
      this.EsercizioEditForm.get('resa_totale_prevista').setValue(0);

      this.EsercizioEditForm.get('regolamento').setValue({ codice: 0, descrizione: '' });
      this.EsercizioEditForm.get('disciplinare').setValue({ codice: '0', descrizione: '' });
      this.EsercizioEditForm.get('vincolo').setValue(this.defaultItemVincolo);

      this.EsercizioEditForm.get('iaf').setValue([]);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('pianoConcimazione').setValue({ codice: 0, descrizione: '' });
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('tipologia').setValue({ codice: 0, descrizione: '' });
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('fase').setValue({ codice: 0, descrizione: '' });
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('n').setValue(0);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('p2o5').setValue(0);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('k2o').setValue(0);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('mgo').setValue(0);
      this.resetProdotto();

    } else {
      this.EsercizioEditForm.get('iaf').setValue([]);
      let vincoloSelected = this.EsercizioEditForm.get('vincolo').getRawValue();
      this.leggiVincoli().then((val) => {
        if (vincoloSelected && vincoloSelected.codice) {
          this.EsercizioEditForm.get('vincolo').setValue(vincoloSelected);
        } else {
          this.EsercizioEditForm.get('vincolo').setValue(val[0]);
        }
      })
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('pianoConcimazione').setValue({ codice: 0, descrizione: '' });
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('tipologia').setValue({ codice: 0, descrizione: '' });
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('fase').setValue({ codice: 102, descrizione: 'Impianto in Produzione' });
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('n').setValue(0);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('p2o5').setValue(0);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('k2o').setValue(0);
      this.EsercizioEditForm.get('apportiMassimiMacroelementi').get('mgo').setValue(0);
      this.resetProdotto();
    }

  }

  private LeggiDisciplinari() {
    let specie: Specie = this.ImpiantoEditForm.get('utilizzoTerreno').value.specie;
    let lavorazione: Lavorazione = new Lavorazione('0');
    let regolamento: Regolamenti = { codice: 0, descrizione: '' } //this.EsercizioEditForm.get('regolamento').value;
    let leggiDisciplinari: LeggiDisciplinare = {
      regolamento: regolamento,
      specie: specie,
      data: this.EsercizioEditForm.get('validita').get('inizio').value,
      lavorazione: lavorazione,
      privato: false
    };
    return this.disciplinariService.leggi(leggiDisciplinari);
  }

  private LeggiIAF() {
    let specie: Specie = this.ImpiantoEditForm.get('utilizzoTerreno').value.specie;
    let disciplinare: Disciplinare = this.EsercizioEditForm.get('disciplinare').value;
    let leggiIAF: LeggiIAF = {
      disciplinare: disciplinare,
      specie: specie,
      data: this.EsercizioEditForm.get('validita').get('inizio').value,
      privato: false
    };
    return this.disciplinariService.leggiIAF(leggiIAF);
  }

  private leggiOrganismoReferente() {
    const piva = this.objParametriAgendaService.getObjParamValue().Piva;
    let impresa = new Impresa();
    impresa.partitaIva = piva;
    return this.contattiService.LeggiOrganismiReferenti(impresa);
  }

  private leggiMagazzinoConferimento() {
    //const piva = this.ImpiantoEditForm.controls["primaryKey"].value.appezzamentoPK.centroAziendalePK.partitaIva;
    let impresa = new Impresa();
    impresa.partitaIva = "";
    return this.contattiService.LeggiMagazzinoConferimento(impresa);
  }

  private leggiRiferimento_Trasferimento_Dati() {
    const piva = this.ImpiantoEditForm.controls["primaryKey"].value.appezzamentoPK.centroAziendalePK.partitaIva;
    let impresa = new Impresa();
    impresa.partitaIva = piva;
    return this.contattiService.LeggiRiferimento_Trasferimento_Dati(impresa);
  }

  private leggiLicenzaColtivazione() {
    const piva = this.ImpiantoEditForm.controls["primaryKey"].value.appezzamentoPK.centroAziendalePK.partitaIva;
    return this.tabelleWsClientService.RicercaValoriParametriQualitativi(1330, piva);
  }

  private leggiFinalitaConcimazione() {
    const regolamento: RegolamentoConcimazione = (<FormGroup>this.EsercizioEditForm.controls["apportiMassimiMacroelementi"]).controls["pianoConcimazione"].value;
    const utilizzo = this.ImpiantoEditForm.controls['utilizzoTerreno'].value as Varieta;
    const specie = utilizzo.specie;
    const finalita = this.ImpiantoEditForm.controls['gruppoFinalita'].value as GruppoFinalita;
    return this.PianoConcimazioneService.leggiRegolamentoConcimazionexImpianto(regolamento, specie, finalita);
  }

  private leggiStatoImpianto() {
    return this.finalitaService.Leggi_FasiCicloColturalexSpecie({
      specie: (<Varieta>this.ImpiantoEditForm.controls['utilizzoTerreno'].value).specie,
      finalita: <GruppoFinalita>this.ImpiantoEditForm.controls['gruppoFinalita'].value
    })
  }

  private leggiLavorazione() {
    return this.codificaInfoAggiuntiveService.LeggiCAC_Codifica_InfoAggiuntive({
      Argomento_Cod: enum_CAC_Codifica_InfoAggiuntive_ArgomentoCod.Lavorazione,
      InfoAgg_Cod: "",
      Tipo_Codifica: 0
    }, true)
  }

  private leggiSpecifica() {
    return this.codificaInfoAggiuntiveService.LeggiCAC_Codifica_InfoAggiuntive({
      Argomento_Cod: enum_CAC_Codifica_InfoAggiuntive_ArgomentoCod.Specifica,
      InfoAgg_Cod: "",
      Tipo_Codifica: 0
    }, true)
  }

  private leggiVincoli() {
    let validita = new IntervalloTemporale(
      this.EsercizioEditForm.controls["validita"].value.inizio,
      this.EsercizioEditForm.controls["validita"].value.fine);

    return lastValueFrom(this.vincoliService.leggiVincoli(validita));
  }

  public calculateResaPrevista(event: Event): void {
    let impianto: Impianto = this.ImpiantoEditForm.getRawValue();
    let esercizio: Esercizio = this.EsercizioEditForm.getRawValue();
    let appezzamento = this.AppezzamentoEditForm.getRawValue();
    this.readIndirizzoCentroAziendale(appezzamento.primaryKey.centroAziendalePK.codice).pipe(
      switchMap(r => {
        if (impianto.utilizzoTerreno.classType == 'Varieta') {
          let indirizzo: IndirizzoAssociato = r.RispostaStringa[0];

          let params: DatiPrevisionaliColtureRequest = new DatiPrevisionaliColtureRequest();
          params.culCod = new BaseCodeDescr(impianto.utilizzoTerreno.codice, impianto.utilizzoTerreno.descrizione);
          params.vegCod = new BaseCodeDescr((<any>impianto.utilizzoTerreno).specie.codice, (<any>impianto.utilizzoTerreno).specie.descrizione);
          params.foralCod = new BaseCodeDescr(impianto.formaAllevamento.codice);
          params.grfiCod = new BaseCodeDescr(impianto.gruppoFinalita.codice);
          params.grvaCod = new BaseCodeDescr(impianto.gruppoVarietale.codice);
          params.portCod = new BaseCodeDescr(impianto.portinnesto.codice);
          params.reg = new BaseCodeDescrStr(indirizzo?.indirizzo?.istatComune.reg ?? '');
          params.prov = new BaseCodeDescrStr(indirizzo?.indirizzo?.istatComune.com ?? '');
          params.codiceStato = new BaseCodeDescrStr(indirizzo?.indirizzo?.stato.codice ?? '');
          params.regCod = new BaseCodeDescr(esercizio.vincolo.regolamento.codice);
          params.statoCod = new BaseCodeDescr(esercizio.apportiMassimiMacroelementi.fase.codice);
          params.validitaFine = esercizio.validita.fine;
          params.validitaInizio = esercizio.validita.inizio;
          params.parametroCod = new BaseCodeDescr(4);
          params.dettSpeciePersonalizzatoCod = new BaseCodeDescrStr(impianto.dettaglio_varieta_personalizzato.codice);

          return this.impiantiService.readDatiPrevisionaliColture(params);
        }
        return of(undefined);
      })
    ).subscribe(r => {
      if (r !== undefined) {
        if (!r.RispostaOK) {
          this.giasDialogService.baseError('appezzamento.datiPrevisione', r.Errore);
        } else {
          if (r.RispostaStringa.udm.codice !== 0) {
            this.EsercizioEditForm.controls['resa_prevista'].setValue(this.unitaMisuraService.Converti(<UnitaDiMisura>r.RispostaStringa.udm, r.RispostaStringa.valore, new UnitaDiMisura(enum_UnitaMisura.KG__HA, '')) ?? r.RispostaStringa.valore);
          } else {
            this.EsercizioEditForm.controls['resa_prevista'].setValue(r.RispostaStringa.valore);
          }
        }
      }
    });
  }

  private readIndirizzoCentroAziendale(sa_cod: number): Observable<rispostaStandard<IndirizzoAssociato[]>> {
    let indirizzo: IndirizzoAssociato;
    if (this.AppezzamentoEditForm.getRawValue()?.indirizzi?.length > 0) {
      indirizzo = this.AppezzamentoEditForm.getRawValue()?.indirizzi[0];
      return this.istatService.readRegionsByProvince(indirizzo.indirizzo.istatComune.prov).pipe(
        switchMap(r => {
          indirizzo.indirizzo.istatComune.reg = r?.codice ?? '000';
          let resp: rispostaStandard<IndirizzoAssociato[]> = new rispostaStandard<IndirizzoAssociato[]>();
          resp.RispostaStringa = [indirizzo];
          return of(resp);
        })
      );
    } else {
      let params: LeggiIndirizziCentro = new LeggiIndirizziCentro(this.objParametriAgenda.Piva, sa_cod);
      return this.centriService.readCentreAddresses(params);
    }
  }

  private assegnaValoriCalcolaPiante(piante_ha, piante_impianto) {
    this.EsercizioEditForm.controls['piante_Ha'].setValue(piante_ha, { emitEvent: false });
    this.EsercizioEditForm.controls['piante_Impianto'].setValue(piante_impianto, { emitEvent: false });
  }

  private calcolaPiante(): { piante_impianto: number, piante_ha: number } {
    const distanza_suFila = this.ImpiantoEditForm.get('su_Fila_M').value;
    const distanza_traFila = this.ImpiantoEditForm.get('tra_Fila_M').value;
    const interbina = this.ImpiantoEditForm.get('interbina').value;
    const germinabilita = this.ImpiantoEditForm.get('germinabilita').value;
    const superficie = this.ImpiantoEditForm.get('superficie').value;

    let flag = true;
    // Controllo se i campi richiesti sono stati riempiti
    if (distanza_suFila == 0 || distanza_traFila == 0) {
      this.giasMessageService.warningMessage(this.translocoService.translate('NecessarioCompilareICampi'));
      flag = false;
    }

    if (flag) {

      let denominatore;

      if (interbina > 0) {
        denominatore = Math.abs(distanza_traFila - interbina) * distanza_suFila;
      } else {
        denominatore = distanza_suFila * distanza_traFila;
      }

      let PianteHa: number;
      if (germinabilita != 0 && germinabilita != undefined && germinabilita != null) {
        PianteHa = 10000 / denominatore * (germinabilita / 100);
      } else {
        PianteHa = 10000 / denominatore;
      }

      if (PianteHa == Infinity) {
        this.ImpiantoEditForm.get('piante_Ha')?.setValue(null);
        this.ImpiantoEditForm.get('piante_Impianto')?.setValue(null);
        return;
      }
      const PianteImpianto = PianteHa * superficie;

      this.ImpiantoEditForm.get('piante_Ha')?.setValue(parseFloat(PianteHa.toFixed(0)));
      this.ImpiantoEditForm.get('piante_Impianto')?.setValue(parseFloat(PianteImpianto.toFixed(0)));
      return {
        piante_ha: parseFloat(PianteHa.toFixed(0)),
        piante_impianto: parseFloat(PianteImpianto.toFixed(0))
      }
    } else {
      this.ImpiantoEditForm.get('piante_Ha')?.setValue(null);
      this.ImpiantoEditForm.get('piante_Impianto')?.setValue(null);
      return {
        piante_ha: 0,
        piante_impianto: 0
      }
    }
  }

  private calcolaSuperficie(): { superficie: number } {
    const distanza_suFila = this.ImpiantoEditForm.get('su_Fila_M').value;
    const distanza_traFila = this.ImpiantoEditForm.get('tra_Fila_M').value;
    const interbina = this.ImpiantoEditForm.get('interbina').value;
    const germinabilita = this.ImpiantoEditForm.get('germinabilita').value;
    const numeroPiante = this.EsercizioEditForm.get('piante_Impianto').value;

    let flag = true;
    // Controllo se i campi richiesti sono stati riempiti
    if (distanza_suFila == 0 || distanza_traFila == 0) {
      this.giasMessageService.warningMessage(this.translocoService.translate('NecessarioCompilareICampixPiante'));
      flag = false;
    }

    if (numeroPiante == undefined || numeroPiante == 0) {
      this.giasMessageService.warningMessage("impostare correttamente numero piante");
      flag = false;
    }

    if (flag) {

      let denominatore;

      if (interbina > 0) {
        denominatore = Math.abs(distanza_traFila - interbina) * distanza_suFila;
      } else {
        denominatore = distanza_suFila * distanza_traFila;
      }

      let PianteHa: number;
      if (germinabilita != 0 && germinabilita != undefined && germinabilita != null) {
        PianteHa = 10000 / denominatore * (germinabilita / 100);
      } else {
        PianteHa = 10000 / denominatore;
      }

      if (PianteHa == Infinity) {
        this.EsercizioEditForm.get('piante_Ha').setValue(null);
        this.EsercizioEditForm.get('piante_Impianto').setValue(null);
        return;
      }
      const superficie = numeroPiante / PianteHa;

      this.EsercizioEditForm.get('piante_Ha').setValue(parseFloat(PianteHa.toFixed(0)));
      this.EsercizioEditForm.get('piante_Impianto').setValue(parseFloat(numeroPiante.toFixed(0)));
      return {
        superficie: parseFloat(superficie.toFixed(4))
      }
    } else {
      return {
        superficie: 0
      }
    }
  }

  private resetProdotto(): void {
    this.EsercizioEditForm.get('prodotto').setValue({ codice: 0, descrizione: '' });
  }

  public disableLotto(): boolean {
    return !!this.EsercizioEditForm && this.ImpiantoEditForm?.controls['algoritmoCodifica']?.value !== '';
  }

  private updateResaTotalePrevista(): void {
    let resa = this.EsercizioEditForm.controls['resa_prevista'].value ?? 0;
    let sup = this.ImpiantoEditForm.controls['superficie'].value ?? 0;
    this.EsercizioEditForm.controls['resa_totale_prevista'].setValue(resa * sup);
  }

  private setResaTotalePrevistaSubs(): void {
    let resa = this.EsercizioEditForm.controls['resa_prevista'];
    let sup = this.ImpiantoEditForm.controls['superficie'];

    resa.valueChanges.pipe(takeUntil(this.signal$), startWith(0)).subscribe(this.updateResaTotalePrevista.bind(this));
    sup.valueChanges.pipe(takeUntil(this.signal$), startWith(0)).subscribe(this.updateResaTotalePrevista.bind(this));
  }

  private impostaNPK() {
    this.calcoloNPKModello({
      regolamento: <RegolamentoConcimazione>(<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['pianoConcimazione'].value,
      specie: (<Varieta>this.ImpiantoEditForm.controls['utilizzoTerreno'].value).specie,
      finalita: <FinalitaPianoConcimazione>(<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['tipologia'].value,
      stato: <FaseCicloColturale>(<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['fase'].value
    }).pipe(
      takeUntil(this.signal$)
    ).subscribe(val => {
      if (val.n != undefined) {
        (<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['n'].setValue(val.n);
      }
      if (val.p2o5 != undefined) {
        (<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['p2o5'].setValue(val.p2o5);
      }
      if (val.k2o != undefined) {
        (<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['k2o'].setValue(val.k2o);
      }
      if (val.mgo != undefined) {
        (<FormGroup>this.EsercizioEditForm.controls['apportiMassimiMacroelementi']).controls['mgo'].setValue(val.mgo);
      }
    });
  }



}
