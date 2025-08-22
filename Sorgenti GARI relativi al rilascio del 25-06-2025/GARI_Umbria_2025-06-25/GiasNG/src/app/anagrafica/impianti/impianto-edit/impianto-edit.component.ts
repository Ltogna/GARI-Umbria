import {Component, Input, OnDestroy, OnInit, SecurityContext, ViewChild} from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import {
  audit,
  combineLatest,
  debounceTime,
  interval,
  merge, scan, skip,
  startWith,
  Subject,
  Subscription,
  take,
  takeUntil,
  tap
} from 'rxjs';
import { GiasDropDownTemplateService, ObjParametriAgenda } from 'gias-ui-kit';
import { Enum_DBTypeOperation } from 'gias-ui-kit';
import { Specie } from 'app/Model/metaschema/utilizzi/Specie';
import { GruppoFinalita } from 'app/Model/metaschema/utilizzi/GruppoFinalita';
import { AGRODATAFINE, AGRODATAINIZIO } from 'app/Model/CostantiPersonalizzate';
import { GruppoFinalitaService } from 'app/Service/Metaschema/finalita.service';
import { FunzioniComuniService } from 'app/Service/FunzioniComuni.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { GiasDropDownTemplateSComponent } from 'gias-ui-kit';
import { GruppoVarietaleService } from 'app/Service/Metaschema/gruppoVarietale.service';
import { UtilizzoTerreno } from 'app/Model/metaschema/utilizzi/UtilizzoTerreno';
import { Varieta } from 'app/Model/metaschema/utilizzi/Varieta';
import { FormaAllevamentoService } from 'app/Service/Metaschema/formaAllevamento.service';
import { PortinnestoService } from 'app/Service/Metaschema/portinnesto.service';
import { CoperturaService } from 'app/Service/Metaschema/copertura.service';
import { IrrigazioneService } from 'app/Service/Metaschema/irrigazione.service';
import { SeminaTrapiantoService } from 'app/Service/Metaschema/seminaTrapianto.service';
import { ProvenienzaSemeService } from 'app/Service/Metaschema/provenienzaSeme.service';
import { ConduzioneService } from 'app/Service/Metaschema/conduzioneTraSuFila.service';
import { CodificaInfoAggiuntiveService } from 'app/Service/Codifiche/codifica_InfoAggiuntive.service';
import { BaseCodeDescr } from 'app/Model/baseClass/baseCodeDescr';
import { Esercizio } from 'app/Model/anagrafiche/Esercizio';
import { DatePipe } from '@angular/common';
import { GiasMessageService } from "../../../Service/gias-message.service";
import { Impianto } from 'app/Model/anagrafiche/Impianto';
import { DestinazioneUso } from "../../../Model/metaschema/utilizzi/DestinazioneUso";
import { TabStripComponent } from '@progress/kendo-angular-layout';
import { TranslocoService } from '@jsverse/transloco';
import { SharedDataService } from '../../../GIS/services/shared-data.service';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { ConversioniService } from 'app/Service/Metaschema/conversioni.service';
import { UnitaDiMisura_Alternativa } from 'app/Service/api.service';
import {NotificationRef} from '@progress/kendo-angular-notification';
import { cloneDeep } from 'lodash';

@Component({
  standalone: false,
  selector: 'app-impianto-edit',
  templateUrl: './impianto-edit.component.html',
  styleUrls: ['./impianto-edit.component.css'],
  providers:[ GiasDropDownTemplateService]
})
export class ImpiantoEditComponent implements OnInit, OnDestroy {
  @Input('appezzamento') AppezzamentoEditForm: FormGroup;
  @Input('impianto') ImpiantoEditForm: FormGroup = this.fb.group({});
  @Input() maxSup: number;

  @ViewChild('tabstripEsercizi') public stripEsercizi: TabStripComponent;

  superficieBlur$: Subject<void> = new Subject<void>();

  AGRODATA_INIZIO: Date = AGRODATAINIZIO;
  AGRODATA_FINE: Date = AGRODATAFINE;

  imagePath: SafeResourceUrl;
  imageExist: boolean = false;

  defaultItem: { codice: 0, descrizione: '' };

  edit: boolean = true;

  private lastUtilizzoTerreno: UtilizzoTerreno;
  private specie: Specie;

  private signal$: Subject<void> = new Subject();
  private caricamentoFormCompletato: Subject<void> = new Subject();

  private objParametriAgenda: ObjParametriAgenda;
  private subDDL: Subscription;

  private readonly FASE_INPRODUZIONE: number = 102;

  constructor(
    private gruppoFinalitaService: GruppoFinalitaService,
    private fb: FormBuilder,
    private objParametriAgendaService: ObjParametriAgendaService,
    private fcService: FunzioniComuniService,
    private irrrigazioneService: IrrigazioneService,
    private gruppoVarietaleService: GruppoVarietaleService,
    private formaAllevamentoService: FormaAllevamentoService,
    private portinnestoService: PortinnestoService,
    private datePipe: DatePipe,
    private seminaTrapiantoService: SeminaTrapiantoService,
    private provenienzaSemeService: ProvenienzaSemeService,
    private conduzioneService: ConduzioneService,
    private codificaInfoAggiuntiveService: CodificaInfoAggiuntiveService,
    private coperturaService: CoperturaService,
    private _sanitizer: DomSanitizer,
    private giasMessageService: GiasMessageService,
    private transloco: TranslocoService,
    private permessiUtenteService: PermessiUtenteService,
    private sharedDataService: SharedDataService,
    private conversioniService: ConversioniService
  ) {  }

  ngOnInit() {

    this.objParametriAgenda = this.objParametriAgendaService.getObjParamValue();
    this.piante_calcola();
    this.disabilitacontrolli();

    this.ImpiantoEditForm.controls['superficie'].updateValueAndValidity();

    if (this.ImpiantoEditForm.controls['utilizzoTerreno'].value &&
      this.ImpiantoEditForm.controls['utilizzoTerreno'].value.classType == 'Varieta') {
      this.specie = this.ImpiantoEditForm.controls['utilizzoTerreno'].value.specie;
    }

    if (this.ImpiantoEditForm.controls['immagineBase64'].value != null &&
      this.ImpiantoEditForm.controls['immagineBase64'].value != undefined &&
      this.ImpiantoEditForm.controls['immagineBase64'].value != '') {

      this.imagePath = this._sanitizer.sanitize(
        SecurityContext.URL,
        'data:image/jpg;base64,' + this.ImpiantoEditForm.controls['immagineBase64'].value
      )
      this.imageExist = true;
    }

    this.ImpiantoEditForm.controls['utilizzoTerreno'].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val: UtilizzoTerreno) => {
      this.onChangeUtilizzoTerreno(val);
    });

    merge(
      this.ImpiantoEditForm.controls['su_Fila_M'].valueChanges,
      this.ImpiantoEditForm.controls['tra_Fila_M'].valueChanges,
      this.ImpiantoEditForm.controls['interbina'].valueChanges,
      this.ImpiantoEditForm.controls['germinabilita'].valueChanges,
      this.ImpiantoEditForm.controls['superficie'].valueChanges
    ).pipe(
      takeUntil(this.signal$)
    ).subscribe(() => {
      this.piante_calcola();
    });

    this.ImpiantoEditForm.controls['superficie'].valueChanges.pipe(
      takeUntil(this.signal$),
      skip(1),
      audit(ev => this.superficieBlur$),
      scan((a, c) => this.onEditSupImp(a, c), undefined)
    ).subscribe();

    this.ImpiantoEditForm.controls['gruppoVarietale'].valueChanges.pipe(takeUntil(this.signal$)).subscribe((el:BaseCodeDescr) => {
      this.onChangeGruppoVarietale(el);
    });

    this.ImpiantoEditForm.controls['impianto_Ibrido'].valueChanges.pipe(takeUntil(this.signal$)).subscribe((val: boolean) => {
      this.onChangeImpiantoIbrido(val);
    });

    if (this.objParametriAgenda.TipoOperazioneDB === Enum_DBTypeOperation.Read) {
      this.edit = false;
    }

    this.caricamentoFormCompletato.pipe(takeUntil(this.signal$)).subscribe(() => {
      this.onCaricamentoFormCompletato();

    });

    interval(0).pipe(take(1)).subscribe(() => this.caricamentoFormCompletato.next());

    const unitaMisuraAlternativaControl = this.ImpiantoEditForm.get('unitaMisuraAlternativa');
    const superficieAlternativaControl = this.ImpiantoEditForm.get('superficieAlternativa');

    if (unitaMisuraAlternativaControl != null && superficieAlternativaControl != null) {
      const superficieAlternativa$ = superficieAlternativaControl.valueChanges.pipe(startWith(superficieAlternativaControl.value));

      combineLatest(
        [
          unitaMisuraAlternativaControl.valueChanges.pipe(startWith(unitaMisuraAlternativaControl.value)),
          superficieAlternativa$
        ]
      ).pipe(
        takeUntil(this.signal$),
        debounceTime(250),
        tap(([udm, sup]: [UnitaDiMisura_Alternativa, number]) => {
          if (udm?.codice == null || udm?.codice == 0) {
            return;
          }

          // round result to 4th decimal
          const newValue = Math.round(sup * udm.tassoConversione * 1000) / 1000;

          const superficieImpiantoForm = this.ImpiantoEditForm.controls['superficie'];
          superficieImpiantoForm?.setValue(newValue);
          superficieImpiantoForm?.markAsTouched();

          const superficieAppezzamentoForm = this.AppezzamentoEditForm?.controls['superficie'];
          const idAppezzamentoForm = this.AppezzamentoEditForm?.controls['primaryKey']?.value;
          if (superficieAppezzamentoForm != null && superficieAppezzamentoForm.untouched && idAppezzamentoForm != null && idAppezzamentoForm.codice == 0) {
            superficieAppezzamentoForm.setValue(newValue);
          }
        })
      ).subscribe();

      superficieAlternativa$.subscribe((value: number) => {
        const superficieImpiantoForm = this.ImpiantoEditForm.controls['superficie'];
        if (value == null || value == 0) {
          superficieImpiantoForm.enable();
          return;
        }

        superficieImpiantoForm.disable();
      });
    }
  }

  ngOnDestroy() {
    this.signal$.next();
    this.signal$.complete();
    if (this.subDDL) {
      this.subDDL.unsubscribe();
    }
  }

  getFormArrayEsercizi() {
    return <FormArray>this.ImpiantoEditForm.get('esercizi');
  }

  onAddEsercizio(): void {
    const arrFormEsercizi: FormArray<any> = this.ImpiantoEditForm.controls['esercizi'] as FormArray;
    const lastFormEsercizio: FormGroup<any> = arrFormEsercizi.controls[arrFormEsercizi.length - 1] as FormGroup;
    const deepCopy: FormGroup<any> = this.fcService.cloneAbstractControl(lastFormEsercizio) as FormGroup;
    const validita_fine: Date = deepCopy.controls['validita'].value.fine as Date;

    if (validita_fine.toUTCString() == AGRODATAFINE.toUTCString()){
      this.giasMessageService.warningMessage(this.transloco.translate('PerInserireEsercizio'), true)
    } else {
      this.setNewEsercizioData(deepCopy, validita_fine);
      deepCopy.controls['codici'].setValue([]);
      deepCopy.controls['lotto'].setValue('');
      deepCopy.controls['codice'].setValue(0);
      deepCopy.controls['esercizio_Chiuso'].setValue(false); // new ex. can't be created already closed

      arrFormEsercizi.push(deepCopy);

      setTimeout(() => {
        this.stripEsercizi.selectTab(arrFormEsercizi.length - 1);
      }, 100);
    }
  }

  setNewEsercizioData(esercizio: FormGroup, dataFine: Date){
    let validita_inizio_new: Date = new Date(dataFine);
    let validita_fine_new: Date = new Date(dataFine);

    validita_inizio_new.setDate(dataFine.getDate() + 1);
    validita_fine_new.setFullYear(dataFine.getFullYear() + 1);

    (esercizio.controls['validita'] as FormGroup).controls['inizio'].setValue(validita_inizio_new);
    (esercizio.controls['validita'] as FormGroup).controls['fine'].setValue(validita_fine_new);
  }

  onRemoveEsercizioTabStrip(): void {
    let ArrTabs = this.stripEsercizi.tabs.toArray();
    let selectedIndex = ArrTabs.findIndex((el) => el.selected == true);
    const arrFormEsercizi = this.ImpiantoEditForm.controls['esercizi'] as FormArray;
    arrFormEsercizi.removeAt(selectedIndex);
    if (ArrTabs.length > selectedIndex && selectedIndex > 0) {
      this.stripEsercizi.selectTab(selectedIndex - 1);
    } else if (selectedIndex == 0) {
      this.stripEsercizi.selectTab(1);
    } else {
      this.stripEsercizi.selectTab(0);
    }

    this.updateDataInizioProduzione(undefined);
  }

  async openDdl(ddlEl: GiasDropDownTemplateSComponent): Promise<void> {
    switch (ddlEl.giasFormControlName) {
      case 'gruppoFinalita':
        ddlEl.listItems = await this.gruppoFinalitaService.leggi(this.specie, this.sharedDataService?.getCfgSementiAsValue());
        break;
      case 'gruppoVarietale':
        ddlEl.listItems = await this.gruppoVarietaleService.leggi(this.specie);
        break;
      case 'irrigazione':
        ddlEl.listItems = await this.irrrigazioneService.leggi(this.specie);
        break;
      case 'formaAllevamento':
        ddlEl.listItems = await this.formaAllevamentoService.leggi(this.specie);
        break;
      case 'portinnesto':
        ddlEl.listItems = await this.portinnestoService.leggi(this.specie);
        break;
      case 'seminaTrapianto':
        ddlEl.listItems = await this.seminaTrapiantoService.leggi(this.specie);
        break;
      case 'tecnicaConduzioneTraFila':
        ddlEl.listItems = await this.conduzioneService.leggiConduzioneTra(this.specie);
        break;
      case 'tecnicaConduzioneSuFila':
        ddlEl.listItems = await this.conduzioneService.leggiConduzioneSu(this.specie);
        break;
      case 'dettaglio_varieta_personalizzato':
        ddlEl.listItems = await this.codificaInfoAggiuntiveService.leggiDettaglioVarietaPersonalizzato();
        break;
      case 'codiceZona':
        ddlEl.listItems = await this.codificaInfoAggiuntiveService.leggiProdotti(this.objParametriAgenda.Piva);
        break;
      case 'provenienzaSeme':
        ddlEl.listItems = await this.provenienzaSemeService.leggi(this.specie);
        break;
      case 'copertura':
        ddlEl.listItems = await this.coperturaService.leggi(this.specie);
        break;
      case 'unitaMisuraAlternativa':
        ddlEl.listItems = await this.conversioniService.leggi_async(true);
        break;
    }
  }

  descrizioneEsercizio(esercizio: Esercizio): string {
    let impianto: Impianto = this.ImpiantoEditForm.getRawValue();
    let impiantoDescr: string;

    if (impianto.utilizzoTerreno.classType == "Varieta") {
      impiantoDescr = (<Varieta>impianto.utilizzoTerreno).specie.descrizione + " - " + (<Varieta>impianto.utilizzoTerreno).descrizione + " ";
    } else {
      impiantoDescr = (<DestinazioneUso>impianto.utilizzoTerreno).descrizione + " ";
    }

    if (impianto.utilizzoTerreno.codice == 0) {
      impiantoDescr = "";
    }

    let dataInizioStr = '...';
    if (this.datePipe.transform(esercizio.validita.inizio, 'longDate') != this.datePipe.transform(AGRODATAINIZIO, 'longDate')) {
      dataInizioStr = this.datePipe.transform(esercizio.validita.inizio, 'shortDate')
    }
    let dataFineStr = '...';
    if (this.datePipe.transform(esercizio.validita.fine, 'longDate') != this.datePipe.transform(AGRODATAFINE, 'longDate')) {
      dataFineStr = this.datePipe.transform(esercizio.validita.fine, 'shortDate')
    }

    return dataInizioStr + ' - ' + dataFineStr;
  }

  specieSelezionata(): boolean {
    var specie_specified = false;
    var val:UtilizzoTerreno = this.ImpiantoEditForm.controls['utilizzoTerreno'].value
    if (val.classType == 'Varieta') {
      if ((<Varieta>val).specie.codice > 0) {
        specie_specified = true;
      }
    }

    return specie_specified;
  }

  updateDataInizioProduzione(event: { fase: number, id: number, validitaInizio: Date; }): void {
    const dataInizioProduzione: Date = this.ImpiantoEditForm.controls['data_Inizio_Produzione'].value;
    const eserciziInProduzione: any[] = this.getFormArrayEsercizi().value.filter(e => e.apportiMassimiMacroelementi.fase.codice === this.FASE_INPRODUZIONE);

    if (event?.fase === this.FASE_INPRODUZIONE && event?.validitaInizio < dataInizioProduzione) {
      this.ImpiantoEditForm.controls['data_Inizio_Produzione'].setValue(event.validitaInizio);
    } else if (eserciziInProduzione.length > 0) {
      const dates = eserciziInProduzione.flatMap(e => e.validita.inizio)
        .sort((a: Date, b: Date) => a.getTime() - b.getTime());

      let date: Date = dates.shift();
      let dateCopy = cloneDeep(date);
      // dateCopy.setDate(1);
      // dateCopy.setMonth(0);

      this.ImpiantoEditForm.controls['data_Inizio_Produzione'].setValue(dateCopy ?? new Date());
    }
  }

  disableCodiceImpianto(): boolean {
    return !!this.ImpiantoEditForm && this.ImpiantoEditForm?.controls['codiceImpianto']?.value !== '' && this.ImpiantoEditForm?.controls['algoritmoCodifica']?.value !== '';
  }

  private async specieChanged(specie: Specie) {
    this.specie = specie;

    if (this.specie.codice == 0) {
      this.ImpiantoEditForm.get('gruppoFinalita').setValue({ codice: 0, descrizione: "" });
    } else {
      const finalita_arr = await this.gruppoFinalitaService.leggi(specie, this.sharedDataService?.getCfgSementiAsValue());
      let finalita: GruppoFinalita = { codice: 0, descrizione: '', specieCod: this.specie.codice };
      if (finalita_arr.length > 0) {
        let defaultFinalita: GruppoFinalita;
        if(this.permessiUtenteService.getImpostazione_Utente(201)){
          defaultFinalita = finalita_arr.find(el => el.codice == parseInt(this.permessiUtenteService.getImpostazione_Utente(201).Valore))
        }
        if(defaultFinalita) {
          finalita = defaultFinalita;
        } else {
          finalita = finalita_arr[0];
        }
      }
      this.ImpiantoEditForm.get('gruppoFinalita').setValue({ codice: finalita.codice, descrizione: finalita.descrizione });

    }

    this.ImpiantoEditForm.get('gruppoVarietale').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('irrigazione').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('formaAllevamento').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('portinnesto').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('seminaTrapianto').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('provenienzaSeme').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('tecnicaConduzioneTraFila').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('tecnicaConduzioneSuFila').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('dettaglio_varieta_personalizzato').setValue({ codice: 0, descrizione: '' });
    this.ImpiantoEditForm.get('codiceZona').setValue({ codice: '', descrizione: '' });
    this.ImpiantoEditForm.get('copertura').setValue({ codice: 0, descrizione: '' });
  }

  private piante_calcola(): void {
    const distanza_suFila = this.ImpiantoEditForm.get('su_Fila_M').value;
    const distanza_traFila = this.ImpiantoEditForm.get('tra_Fila_M').value;
    const interbina = this.ImpiantoEditForm.get('interbina').value;
    const germinabilita = this.ImpiantoEditForm.get('germinabilita').value;
    const superficie = this.ImpiantoEditForm.get('superficie').value;

    let flag = true;
    // Controllo se i campi richiesti sono stati riempiti
    if (distanza_suFila == 0) {
      flag = false;
    }
    if (distanza_traFila == 0) {
      flag = false;
    }

    if (flag) {

      let denominatore: number;

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
      const PianteImpianto: number = PianteHa * superficie;

      this.ImpiantoEditForm.get('piante_Ha')?.setValue(parseFloat(PianteHa.toFixed(0)));
      this.ImpiantoEditForm.get('piante_Impianto')?.setValue(parseFloat(PianteImpianto.toFixed(0)));
    } else {
      this.ImpiantoEditForm.get('piante_Ha')?.setValue(null);
      this.ImpiantoEditForm.get('piante_Impianto')?.setValue(null);
    }
  }

  private disabilitacontrolli(){
    // Se sono in Lettura disabilito tutti i controlli
    if(this.objParametriAgenda.TipoOperazioneDB === Enum_DBTypeOperation.Read){
      this.ImpiantoEditForm.disable();
    }

    if (this.ImpiantoEditForm.controls['codiceImpianto'].value !== '' && this.ImpiantoEditForm.controls['algoritmoCodifica'].value !== '') {
      this.ImpiantoEditForm.controls['codiceImpianto'].disable();
    }
  }

  private onChangeUtilizzoTerreno(utilizzoTerreno: UtilizzoTerreno) {
    if (this.lastUtilizzoTerreno == null) {
      this.lastUtilizzoTerreno = utilizzoTerreno;
      return;
    }
    if (this.lastUtilizzoTerreno.classType != utilizzoTerreno.classType) {
      if (utilizzoTerreno.classType == 'Varieta') {
        this.specieChanged((<Varieta>utilizzoTerreno).specie);
      } else {
        this.specieChanged({ codice: 0, descrizione: '' });
      }
    } else if (utilizzoTerreno.classType == 'Varieta') {
      if ((<Varieta>this.lastUtilizzoTerreno).specie.codice != (<Varieta>utilizzoTerreno).specie.codice) {
        this.specieChanged((<Varieta>utilizzoTerreno).specie);
      }
    }
    this.lastUtilizzoTerreno = utilizzoTerreno;
  }

  private onChangeGruppoVarietale(el: BaseCodeDescr) {
    if (el.codice < 0) {
      this.ImpiantoEditForm.controls['impianto_Ibrido'].setValue(true);
    } else {
      this.ImpiantoEditForm.controls['impianto_Ibrido'].setValue(false);
    }
  }

  private onChangeImpiantoIbrido(val: boolean) {
    if (val == false) {
      this.ImpiantoEditForm.controls['codBMBDBT_F'].setValue('');
      this.ImpiantoEditForm.controls['genetica_F'].setValue('');
      this.ImpiantoEditForm.controls['offType_F'].setValue('');
      this.ImpiantoEditForm.controls['distanzaSuFila_F'].setValue(0);
      this.ImpiantoEditForm.controls['distanzaTraFila_F'].setValue(0);
    }
  }

  private onCaricamentoFormCompletato() {
    let impianto: Impianto = this.ImpiantoEditForm.getRawValue();
    let index = impianto.esercizi.findIndex((imp: Esercizio) => imp.codice == this.objParametriAgenda.Progetto_Cod);
    if (index >= 0) {
      this.stripEsercizi.selectTab(index);
    } else {
      this.stripEsercizi.selectTab(0);
    }
  }

  private onEditSupImp(previousNotification: NotificationRef, value: number): NotificationRef {
    const result: {sup: number, readFromAppezzamento: boolean} = this.extractSuperficieGis();

    if (!!result.sup && result.sup !== 0) {
      let prefix: string;
      const diff: number = Number.parseFloat((value - result.sup).toFixed(4));
      const percentage: number = Number.parseFloat(((diff / result.sup) * 100).toFixed(4));

      if (result.readFromAppezzamento) {
        prefix = `${this.transloco.translate('SuperficiePoligonoAppezzamentoAssociato')} ${result.sup} [Ha],  `;
      } else {
        prefix = `${this.transloco.translate('SuperficiePoligonoAssociato')} ${result.sup} [Ha],  `;
      }

      const diffStr: string = `${this.transloco.translate('Differenza')}: ${diff > 0 ? '+' : ''}${diff} [Ha],  `;
      const percStr: string = `${this.transloco.translate('DifferenzaPercentuale')}: ${diff > 0 ? '+' : ''}${percentage} %`;
      if (diff >= 0.0001) {
        previousNotification?.hide();
        return this.giasMessageService.warningMessage(prefix + diffStr + percStr, false, false, undefined, 10000);
      }
    }
  }

  /** Extract the associated polygon surface and tells if it was read from the Impianto or from the Appezzamento */
  private extractSuperficieGis(): { sup: number, readFromAppezzamento: boolean } {
    let superficieGis: number = Number.parseFloat(this.ImpiantoEditForm?.controls['superficieGis']?.value?.toFixed(4));

    // check if implant doesn't have a polygon associated or if it is a new one
    const readFromAppezzamento: boolean = superficieGis === 0 || this.ImpiantoEditForm?.controls['primaryKey']?.value?.codice === 0;
    if (readFromAppezzamento) {
      superficieGis = Number.parseFloat(this.AppezzamentoEditForm?.controls['superficieGis']?.value?.toFixed(4));
    }

    return {sup: superficieGis, readFromAppezzamento: readFromAppezzamento};
  }
}
