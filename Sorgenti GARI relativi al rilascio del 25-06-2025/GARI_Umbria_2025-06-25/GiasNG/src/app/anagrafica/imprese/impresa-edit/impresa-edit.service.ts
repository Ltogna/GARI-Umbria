/* eslint-disable */
import { Inject, Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CodiceAnagrafe } from 'app/Model/anagrafiche/CodiceAnagrafe';
import { CodiciAnagrafeValoriChiave } from 'app/Model/anagrafiche/CodiciAnagrafeValori';
import { Impresa } from 'app/Model/anagrafiche/Impresa';
import { ImpresaPadre } from 'app/Model/anagrafiche/ImpresaPadre';
import { AGRODATAFINE, AGRODATAINIZIO } from 'app/Model/CostantiPersonalizzate';
import { AjaxAgronicaService } from 'app/Service/ajax-agronica.service';
import { ImpreseService } from 'app/Service/Anagrafica/imprese.service';
import { MasterService, rispostaStandard, RispostaStandard } from 'app/Service/master.service';
import { FormeGiurificheService } from 'app/Service/Metaschema/forme-giuridiche.service';
import { ObjParametriAgendaService } from 'app/Service/obj-parametri-agenda.service';
import { ICodiciTemplateService } from 'app/Utility/Template/codici-template/services/codici-template.service';
import { KendoGridRow } from 'gias-kendo-grid';
import { HttpAction } from 'gias-kendo-grid';
import { GridPublicService } from 'gias-kendo-grid';
import { BehaviorSubject, EMPTY, from, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ContattiGridService } from './contatti-edit/contatti-edit.service';
import { ContattiRootService } from './contatti-edit/contattiRoot.service';
import { CaricaDatiDto as ImpresaEditDto } from './imprese-codici-metadata';
import {Enum_DBTypeOperation} from 'gias-ui-kit';
import {capValidatorAziendeEdit, capValidatorAziendeGrid} from 'gias-ui-kit';
import {IntervalloTemporale} from '../../../Model/anagrafiche/IntervalloTemporale';

const itemIndex = (item: CodiciAnagrafeValoriChiave, data: CodiciAnagrafeValoriChiave[]): number => {
  for (let idx = 0; idx < data.length; idx++) {
    if (data[idx].chiave === item.chiave) {
      return idx;
    }
  }

  return -1;
};

@Injectable({ providedIn: 'root'})
export class ImpresaEditService implements ICodiciTemplateService {
  gridId = "ImpresaCodici";
  public gridPublicService: GridPublicService;

  public form: Impresa;
  public codici: CodiciAnagrafeValoriChiave[];

  constructor(private impreseService: ImpreseService,
              private parametriAgenda: ObjParametriAgendaService,
              private ajaxAgronicaService: AjaxAgronicaService,
              private masterService: MasterService,
              private router: Router,
              private fb: FormBuilder,
              private formeGiuridicheService: FormeGiurificheService,
              private contattiRootService: ContattiRootService
  ) {}

  public updateForm(form: Impresa) {
    this.form = form;
    this.form.codici = this.codici;
  }

  public updateTable(action: HttpAction, row: CodiciAnagrafeValoriChiave) {
    switch(action) {
      case HttpAction.CREATE:
        const chiave = Math.max.apply(Math, this.codici.map(function (o) {
          return o.chiave;
        }));
        row.chiave = chiave + 1;
        this.codici.push(row);
        break;
      case HttpAction.UPDATE:
        const index = itemIndex(row, this.codici);
        this.codici.splice(index, 1, row);
        break;
      case HttpAction.REMOVE:
        const _index = itemIndex(row, this.codici);
        this.codici.splice(_index, 1);
        break;
    }
    this.gridPublicService.refresh(true);
  }

  private setRowsUniqueId(rows: CodiciAnagrafeValoriChiave[]) {
    let i = 0;
    rows.forEach((row) => {
      row.chiave = i++;
    });
  }

  public postForm(): Observable<rispostaStandard<Impresa>> {
    return this.impreseService.ScriviImpresa(this.form, true);
  }

  private afterPostFormCallback(resp: RispostaStandard) {
    this.masterService.setCompanyHeader(this.form.ragioneSociale);

    this.router.navigate(['Anagrafica/Imprese']);
  }

  leggiDropdowns(): Observable<CodiceAnagrafe[]> {
    const agenda = this.parametriAgenda.getObjParamValue();
    return this.impreseService.leggiImpreseCodici(agenda);

  }

  leggiDatiTabella(): Observable<CodiciAnagrafeValoriChiave[]> {
    return of(this.codici);
  }

  public caricaDati(): Observable<ImpresaEditDto> {
    this.masterService.set_isLoading({ isLoading: true,
      message: 'Caricamento in corso' });

    const agenda = this.parametriAgenda.getObjParamValue();
    if(agenda.TipoOperazioneDB == Enum_DBTypeOperation.Write)
      agenda.Piva = '';


    const data = this.provaCaricareIDati(agenda).pipe(map((data) => {
      data.ObjParamAgenda = agenda;
      return data;
    }));

    this.masterService.set_isLoading({ isLoading: false });
    return data;
  }

  private provaCaricareIDati(agenda) {

    let promises: Promise<any>[] = [
      this.formeGiuridicheService.leggiFormeGiurifiche(0),
      this.impreseService.leggiCombo_Tecnici(agenda.Piva),
      this.impreseService.leggiCombo_OrganismiDiControllo(agenda.Piva),
      this.impreseService.leggiPadri()
    ];

    if (agenda.Piva != '') {
      promises.push(this.impreseService.leggiImpresa(agenda));
    }

    const data = from(Promise.all(promises)).pipe(map((dati): ImpresaEditDto => {
      let impresa: Impresa;
      const padri:ImpresaPadre[] = dati[3];
      if (agenda.Piva != "") {
        impresa = dati[4].RispostaStringa;
      } else {

        let padre = padri.find((el) => this.masterService.objP_server.PivaSuperUser == el.partitaIva);
        if (!padre) {
          padre = padri[0];
        }

        impresa = new Impresa()
        impresa.partitaIva = ''
        impresa.ragioneSociale = ''
        impresa.CUAA = ''
        impresa.certificazione = [];
        impresa.indirizzi = [
          {
            indirizzo: {
              codice: 0,
              via: '',
              frazione: '',
              istatComune: {
                reg: '000',
                prov: '000',
                com: '000',
                cap: '0000',
                localita: '',
                comuni_prov: '',
                validita: new IntervalloTemporale(),
                codiceBelfiore: ''
              },
              cap: '0000',
              stato: { codice: 'IT', descrizione: 'Italia', codiceNumerico: '', codiceAlpha3: '', gestioneGerarchia: 1},
              note: '',
              flag_cancellazione: false,
            },
            tipo_Indirizzo: 1,
            flag_cancellazione: false
          }
        ];
        impresa.codici = [];
        impresa.contatti = [];
        impresa.impresaPadre = [padre];
        impresa.tipo_Impresa = 1;
      }

      if(impresa.contatto_superuser != undefined) {
        this.contattiRootService.gridRowsContattiSource.next(impresa.contatto_superuser.risorseUmane.map(t => {
          let kendoRow= {
            codice: t.rapportoContabile.codice,
            settore: t.settore,
            descrizione: t.rapportoContabile.descrizione,
            attivita: t.attivita
          };
          return kendoRow;
        }));
      }
      this.form = impresa;
      this.codici = impresa.codici as CodiciAnagrafeValoriChiave[];
      this.setRowsUniqueId(this.codici);

      return {
        Impresa: impresa,
        FormeGiuridiche: dati[0],
        Tecnici: dati[1].RispostaStringa,
        Odc: dati[2].RispostaStringa,
        Padri: padri,
        ObjParamAgenda: agenda
      };
    }, catchError((err) => {
      console.log(err);
      return EMPTY;
    })));
    return data;
  }


  public GetFormGroupImpresa(): FormGroup {
    return this.fb.group({
      partitaIva: [''],
      ragioneSociale: ['', Validators.required],
      forma_Giuridica: new FormControl({ codice: 0, descrizione: '' }),
      tipo_Impresa: [1],
      codici: this.fb.array([]),
      CUAA: [''],
      indirizzi: this.fb.array([this.getIndirizzoAssociatoForm()]),
      contatti: this.fb.array([]),
      impresaPadre: new FormControl([{ ragioneSociale: '', partitaIva: '' }]),
      tecnicoReferente: new FormControl({ codice: 0, descrizione: '' }),
      organismo_di_Controllo: new FormControl({ codice: 0, descrizione: '' }),
      certificazione: new FormControl([]),
      validita: this.fb.group({
        inizio: [AGRODATAINIZIO],
        fine: [AGRODATAFINE]
      }),
      gruppoRaccolta: new FormControl({}),
      disciplinareAziendalePredefinito: new FormControl({}),
    });


  }

  private getIndirizzoAssociatoForm() {
    return this.fb.group({
      tipo_indirizzo: [1],
      indirizzo: this.getIndirizzoForm()
    });
  }


  private getIndirizzoForm() {
    return this.fb.group({
      codice: [0],
      via: [''],
      frazione: [''],
      cap: ['00000', capValidatorAziendeEdit()],
      note: ['', Validators.maxLength(50)],
      stato: new FormControl({ codice: 0, descrizione: '' }),
      istatComune: this.fb.group({
        prov: ['000'],
        com: ['000']
      })
    });
  }


}
