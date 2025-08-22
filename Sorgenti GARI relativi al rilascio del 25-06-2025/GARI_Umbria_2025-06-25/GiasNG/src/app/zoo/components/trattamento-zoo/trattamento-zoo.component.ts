import { Component, ElementRef, Inject, OnDestroy, ViewChild } from "@angular/core";
import { TrattamentoZooFormService } from "./service/trattamento-zoo-form.service";
import { animate, style, transition, trigger } from "@angular/animations";
import { GiasDialogService } from "app/Service/gias-dialog.service";
import { TranslocoService } from "@jsverse/transloco";
import { ObjParametriAgendaService } from "app/Service/obj-parametri-agenda.service";
import { CapoAnimaleCDC } from "app/Model/attivita/centri_di_costo/CapoAnimaleCDC";
import { CapoAnimale } from "app/Service/api.service";
import { RisorsaProdotto } from "app/Model/attivita/risorse/RisorsaProdotto";
import { CentroAziendale, CentroDiCosto, Job_PK, OperazioniZooClient, PrescrizioniClient, RispostaStandard } from "app/Service/net-core6-api.service";
import { GestioneRichiesteService } from "app/Service/gestione-richieste.service";
import { ActivatedRoute, Router } from "@angular/router";
import { cloneDeep } from "lodash";
import { GridProdottiSomministrazioneComponent } from "./griglie-trattamento/griglia-prodotti-somministrazione/grid-prodotti-somministrazione.component";
import { GridCapiAnimaliComponent } from "./griglie-trattamento/griglia-capi-animali/grid-capi-animali.component";
import { catchError, Observable, of, Subject, takeUntil } from "rxjs";
import { enum_LAVCOD, enum_PagineGiasNG } from "app/Model/TipiEnumerativi";
import { FunzioniComuniService } from "app/Service/FunzioniComuni.service";
import { DettagliProtocollo } from "./model/dettagli-protocollo.model";
import { DettaglioRegistroSomministrazioni } from "app/Model/attivita/dettagli/DettaglioRegistroSomministrazioni";
import { Risorsa } from "app/Model/attivita/risorse/Risorsa";
import { Prodotto } from "app/Model/attivita/risorse/Prodotto";
import { ConversionService } from "app/Service/conversion.service";
import { PrescriptionActivity } from "./model/prescription-activity.model";
import { Job } from "app/Model/attivita/Job";
import { enum_TypeTab_Zootecnia } from "app/zoo/models/tipi-enumerativi-zoo";
import { Enum_DBTypeOperation, GiasDropDownTemplateService, GiasMultiSelectTemplateService, LOADING_TOKEN, LoadingService } from "gias-ui-kit";
// import { LOADING_TOKEN, LoadingService } from 'gias-ui-kit';

@Component({
    selector: 'app-trattamento-zoo',
    templateUrl: './trattamento-zoo.component.html',
    standalone: false,
    styleUrls: ['./trattamento-zoo.component.css'],
    animations: [
        trigger('fadeInOut', [
          transition(':enter', [ // Quando l'elemento entra
            style({ opacity: 0 }), // Stato iniziale
            animate('300ms ease-in', style({ opacity: 1 })), // Transizione
          ]),
          transition(':leave', [ // Quando l'elemento esce
            animate('300ms ease-out', style({ opacity: 0 })) // Transizione inversa
          ]),
        ]),
      ],
    providers: [GiasDropDownTemplateService, GiasMultiSelectTemplateService, TrattamentoZooFormService]
})
export class TrattamentoZooComponent implements OnDestroy {

    @ViewChild(GridProdottiSomministrazioneComponent) prodottiSomministrazioneComponent!: GridProdottiSomministrazioneComponent;
    @ViewChild(GridCapiAnimaliComponent) capiAnimaliComponent!: GridCapiAnimaliComponent;

    openInIFrame: boolean = false;

    //di default, poniamo che arriviamo dai protocolli
    typeTab_Zootecnia: enum_TypeTab_Zootecnia = enum_TypeTab_Zootecnia.Prescriptions;

    signal: Subject<void> = new Subject();

    constructor(
                public trattamentoFormService: TrattamentoZooFormService,
                private agendaService: ObjParametriAgendaService,
                private giasDialogService: GiasDialogService,
                private zooService: OperazioniZooClient,
                private conversionService: ConversionService,
                private prescriptionsService: PrescrizioniClient,
                private router: Router,
                private route: ActivatedRoute,
                private gestioneRichieste: GestioneRichiesteService,
                private elementRef: ElementRef,
                @Inject(LOADING_TOKEN) private loadingService: LoadingService,
                private transloco: TranslocoService
    ) { 

        this.initializePageData();
        this.handleQueryParams();
    }

    /**
     * Initializes the page data by retrieving parameters from the agenda service and setting up the form.
      * It also handles the case when the page is opened in "Info" mode, disabling certain form fields.
     */
    private initializePageData(): void {

      this.trattamentoFormService.objParametriAgenda = this.agendaService.getObjParamValue();

      if (this.trattamentoFormService.objParametriAgenda.GenericObj_string) {

          const objParams = JSON.parse(this.trattamentoFormService.objParametriAgenda.GenericObj_string);
          const attivita = objParams.hasOwnProperty('Somministrazioni') ? objParams.Somministrazioni[0] : objParams;
          this.trattamentoFormService.attivitaDaChiamante = this.conversionService.ConversionDateInObject(attivita);

          this.trattamentoFormService.formTrattamentoZoo.get('Data').patchValue(this.trattamentoFormService.attivitaDaChiamante?.inizio ?? new Date(), { emitEvent: false });

          //se apro in Info, disabilito tutto
          if (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Read 
              || this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Update) {
            this.trattamentoFormService.formTrattamentoZoo.get('Data').disable({ emitEvent: false });
            this.trattamentoFormService.formTrattamentoZoo.get('Raggruppamento').disable({ emitEvent: false });
          }

          this.trattamentoFormService.formTrattamentoZoo.get('CentroAziendale').disable({ emitEvent: false });
          this.trattamentoFormService.formTrattamentoZoo.get('Stalla').disable({ emitEvent: false });

          const dettaglioSomm = this.trattamentoFormService.attivitaDaChiamante.risorse.find(item => item.classType == 'DettaglioRegistroSomministrazioni');

          this.trattamentoFormService.dettagliProtocollo = new DettagliProtocollo(objParams.Id_Ricetta, objParams.Tipo_Prescrizione, dettaglioSomm.quantitaTotaleReale, dettaglioSomm.unitaDiMisura.codice, dettaglioSomm.durataTrattamento);
      }

      this.trattamentoFormService.loadCentriAziendaliDDL();

    }

    private handleQueryParams(): void {
        this.route.queryParams.pipe(takeUntil(this.signal))
            .subscribe(params => {
                if (params.seFrame == 1) {
                    this.openInIFrame = true;
                }
                if (params.t_Tab) {
                    this.typeTab_Zootecnia = parseInt(params.t_Tab) as enum_TypeTab_Zootecnia;
                    this.disableFormFields();
                }
            });
    }

    private disableFormFields(): void {
      if (this.typeTab_Zootecnia == enum_TypeTab_Zootecnia.FuturePrescriptions) {
          this.trattamentoFormService.formTrattamentoZoo.get('Data').disable({ emitEvent: false });
          this.trattamentoFormService.formTrattamentoZoo.get('Raggruppamento').disable({ emitEvent: false });
      }
    }


    ngOnDestroy(): void {
      this.signal.next();
      this.signal.complete();
    }

    SalvaEditTrattamentoZoo() {
      
      if (!this.isFormValid()) {
        this.trattamentoFormService.formTrattamentoZoo.markAllAsTouched();
        return;
      }
      if (!this.preSaveCheck()) {
        return;
      }

      const param = [this.getActivityToSave()];

      this.loadingService.set_isLoading({ isLoading: true, component: this.elementRef });

      this.saveTrattamentoZoo(param);

    }

    private saveTrattamentoZoo(param: PrescriptionActivity[]) {

      let obsResponse: Observable<RispostaStandard>;

      //occorre distinguere se stiamo salvando la prima somministrazione, una modifica oppure una somministrazione futura
      if (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Write) {
        if (this.typeTab_Zootecnia == enum_TypeTab_Zootecnia.Prescriptions) 
              obsResponse = this.zooService.operazioniZooScriviFirstSomministrazioniDaProtocollo(this.trattamentoFormService.dettagliProtocollo.idProtocol, param);
        else 
          if (this.typeTab_Zootecnia == enum_TypeTab_Zootecnia.FuturePrescriptions)
            obsResponse = this.zooService.operazioniZooConfermaSomministrazioneFutura(param[0]); 
      } else 
        if (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Update)
          obsResponse = this.zooService.operazioniZooModificaSomministrazione(param[0]);

      obsResponse.pipe(catchError(error => {
        this.loadingService.set_isLoading({ isLoading: false, component: this.elementRef });
        this.giasDialogService.baseError("", FunzioniComuniService.getResponseError(error, this.transloco, 'SiÈVerificatoUnErroreDuranteLaFaseDiSalvat'), false);
        return of(null);
      })).subscribe(async r => {
        
        this.loadingService.set_isLoading({ isLoading: false, component: this.elementRef });

        if (r.RispostaOK) {
          this.giasDialogService.baseSuccess('SalvataggioAvvenutoConSuccesso', '');
          this.gestioneRichieste.gestionePassaggioStessoSito(enum_PagineGiasNG.Pagina_Menu_Zoo).then(resp => {
            this.router.navigate([resp]);
          });
        }
        
      });
    }


    private isFormValid(): boolean {
      return this.trattamentoFormService.formTrattamentoZoo.valid || this.trattamentoFormService.formTrattamentoZoo.disabled;
    }

    /**
     * Performs a pre-save validation check to ensure that the required conditions
     * are met before proceeding with the save operation.
     *
     * @returns {boolean} - Returns `true` if the validation passes, otherwise `false`.
     *
     * The method performs the following checks:
     * 1. Ensures that at least one item is selected in both `prodottiSomministrazioneComponent` 
     *    and `capiAnimaliComponent`. If either is empty, an error dialog is displayed.
     * 2. Ensures that the total quantity to be administered (`quantitaTotaleDaSomministrare`) 
     *    does not exceed the available stock (`quantitaInGiacenza`). If it does, an error dialog is displayed.
     *
     * Error messages are displayed using the `giasDialogService` with translations provided by `transloco`.
     */
    private preSaveCheck(): boolean {
      const arrayFarmaci = this.prodottiSomministrazioneComponent?.gridSelectedRows ?? [];
      const arrayCapiAnimali = this.capiAnimaliComponent?.gridSelectedRows ?? [];

      if (arrayFarmaci.length == 0 || arrayCapiAnimali.length == 0) {
        const message: string = (arrayFarmaci.length == 0) ? 'zoo.SelezionareAlmenoUnFarmaco' : 'zoo.SelezionareAlmenoUnCapoAnimale';
        this.giasDialogService.baseError("",
          this.transloco.translate(message),
          false
        );
        return false;
      }

      let quantitaInGiacenza = arrayFarmaci.reduce((acc, item) => acc + item.Qta, 0);
      let quantitaTotaleDaSomministrare = arrayCapiAnimali.reduce((acc, item) => acc + item.Quantita, 0);

      quantitaInGiacenza = parseFloat(quantitaInGiacenza.toFixed(4));
      quantitaTotaleDaSomministrare = parseFloat(quantitaTotaleDaSomministrare.toFixed(4));

      // verifico che le quantità inserite sulle righe checkate non eccedano la giacenza
      if (quantitaTotaleDaSomministrare > quantitaInGiacenza) {
        this.giasDialogService.baseError("",
          this.transloco.translate('zoo.LaQuantitaDaSomministrareEccedeLaGiacenza'),
          false
        );
        return false;
      }
      return true;
    }

    private getActivityToSave() {
      let cloned = cloneDeep(this.trattamentoFormService.attivitaDaChiamante); //serve per fare la copia dell'oggetto
      let  attivitaToSave =  new PrescriptionActivity();

      if (!attivitaToSave.job) {
        attivitaToSave.job = {
          primaryKey: {
            codice: enum_LAVCOD.CURE_MEDICAMENTI_ANIMALI.toString(),
            classType: "Zootecnia"
          } as Job_PK
         } as Job;
      }

      if (!attivitaToSave.centroAziendale && cloned.centroAziendale) {
        attivitaToSave.centroAziendale = cloned.centroAziendale as CentroAziendale;
      }

      if (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB == Enum_DBTypeOperation.Write && this.typeTab_Zootecnia == enum_TypeTab_Zootecnia.Prescriptions)
        attivitaToSave.codice = '0';
      else {
        attivitaToSave.codice = cloned.codice;
        attivitaToSave.codiceOperazioneRicetta = cloned.codiceOperazioneRicetta;
      }

      if (!attivitaToSave.attivitaCollegate || attivitaToSave.attivitaCollegate.length == 0) {
        attivitaToSave.attivitaCollegate = new Array<PrescriptionActivity>();
        attivitaToSave.attivitaCollegate.push(new PrescriptionActivity());
      }

      attivitaToSave.fabbricatoCod = this.trattamentoFormService.formTrattamentoZoo.get('Stalla').value?.STA_NUM ?? 0;

      this.setAnimalsCDC(attivitaToSave);
      this.setDrugsResources(attivitaToSave);

      //inseriamo le date
      const dataInizio = this.trattamentoFormService.formTrattamentoZoo.get('Data').value;
      attivitaToSave.inizio = dataInizio;
      attivitaToSave.fine = new Date(dataInizio.getTime() + (( this.trattamentoFormService.dettagliProtocollo.durataTrattamento - 1 ) * 24 * 60 * 60 * 1000)); //durata in giorni

      return attivitaToSave;
    }

    /**
     * Populates the `centriDiCosto` property of the provided `PrescriptionActivity` object
     * with data derived from the selected rows in the `capiAnimaliComponent` grid.
     *
     * @param attivitaToSave - The `PrescriptionActivity` object to be updated.
     *
     * The method iterates over the selected rows in the `capiAnimaliComponent` grid
     * (if available) and creates a new `CapoAnimaleCDC` object for each row. Each
     * `CapoAnimaleCDC` object is populated with data from the grid row, including
     * information about the animal (`CapoAnimale`) and the quantity to be administered.
     * These objects are then cast to `CentroDiCosto` and added to the `centriDiCosto`
     * array of the `PrescriptionActivity` object.
     */
    private setAnimalsCDC(attivitaToSave: PrescriptionActivity) {
      const arrayCapiAnimali = this.capiAnimaliComponent?.gridSelectedRows ?? [];
      attivitaToSave.centriDiCosto = new Array<CentroDiCosto>();
      arrayCapiAnimali.forEach(item => {
        
        //la valorizzo già sopra per controllare che non ecceda la giacenza
        // quantitaTotaleDaSomministrare += item.dataItem.Quantita;
        let capoAnimaleCDC = new CapoAnimaleCDC();
        
        capoAnimaleCDC.capoAnimale = {
          validato: (item.Validato == 'No') ? false : true,
          causaleMorte: 0,
          partitaIva: item.Piva,
          codice: item.Cod_Animale,
          matricola: item.Matricola,
          flagCancellazione: false
        } as CapoAnimale;

        capoAnimaleCDC.qtaSomministrata = item.Quantita;
        attivitaToSave.centriDiCosto.push(capoAnimaleCDC as unknown as CentroDiCosto);
      });
    }

    /**
     * Sets the drug resources for the given prescription activity.
     * 
     * This method processes the selected drugs and animals, calculates the total quantity 
     * to be administered, and updates the `risorse` property of the provided `PrescriptionActivity` 
     * object with the necessary resources and details.
     * 
     * @param attivitaToSave - The `PrescriptionActivity` object to which the drug resources will be added.
     * 
     * The method performs the following steps:
     * - Retrieves the selected drugs and animals from the respective components.
     * - Calculates the total quantity to be administered based on the selected animals.
     * - Iterates over the selected drugs and creates detailed records for administration and stock movement.
     * - Updates the `risorse` property of the `PrescriptionActivity` with the administration details 
     *   and stock movement resources.
     * - Ensures that the quantity to be deducted from stock is adjusted based on the remaining quantity 
     *   to be administered.
     */
    private setDrugsResources(attivitaToSave: PrescriptionActivity) {
      const arrayFarmaci = this.prodottiSomministrazioneComponent?.gridSelectedRows ?? [];
      const arrayCapiAnimali = this.capiAnimaliComponent?.gridSelectedRows ?? [];
      const quantitaTotaleDaSomministrare = parseFloat(arrayCapiAnimali
        .reduce((acc, item) => acc + item.Quantita, 0)
        .toFixed(4));

      attivitaToSave.risorse = new Array<Risorsa>();
      let quantitaDaScaricareRimanente = quantitaTotaleDaSomministrare; 

      for (const itemFarmaco of arrayFarmaci) {

        if (quantitaDaScaricareRimanente <= 0) {
          break;
        }

        // let rifDettaglioSomministrazione = attivitaToSave.risorse.find(itemDettaglioSomm => itemDettaglioSomm.classType == 'DettaglioRegistroSomministrazioni');
        let rifDettaglioSomministrazione = cloneDeep(this.trattamentoFormService.attivitaDaChiamante.risorse.find(itemDettaglioSomm => itemDettaglioSomm.classType == 'DettaglioRegistroSomministrazioni')) ?? new DettaglioRegistroSomministrazioni();
        let farmaco = this.trattamentoFormService.arrayDettagliProdottiSomministrazione.find(itemDettaglio => itemDettaglio.prodotto.codice == itemFarmaco.Codice && itemDettaglio.MagazziniMovimentazioni[0].Lotto == itemFarmaco.Lotto);
        
        rifDettaglioSomministrazione.quantitaTotaleReale = quantitaTotaleDaSomministrare;
        rifDettaglioSomministrazione.prodotto = new Prodotto(itemFarmaco.Codice, '');
        rifDettaglioSomministrazione.codiceAIC = itemFarmaco.CodiceAIC;
        rifDettaglioSomministrazione.unitaDiMisura = farmaco.unitaDiMisura;
        rifDettaglioSomministrazione.sospensioni = [];

        attivitaToSave.risorse.push(rifDettaglioSomministrazione);

        // let risorsaScarico = attivitaToSave.risorse.find(itemDettaglioSomm => itemDettaglioSomm.classType == 'RisorsaProdotto');
        let risorsaScarico = new RisorsaProdotto();

        risorsaScarico.prodotto = farmaco.prodotto;
        risorsaScarico.MagazziniMovimentazioni = farmaco.MagazziniMovimentazioni;

        risorsaScarico.MagazziniMovimentazioni.forEach(itemMagazzino => {
          itemMagazzino.Qta = (itemMagazzino.Qta < quantitaDaScaricareRimanente) ? itemMagazzino.Qta : quantitaDaScaricareRimanente;
          itemMagazzino.QtaTot = (itemMagazzino.QtaTot < quantitaDaScaricareRimanente) ? itemMagazzino.QtaTot : quantitaDaScaricareRimanente;
          quantitaDaScaricareRimanente -= itemMagazzino.Qta;
        });

        attivitaToSave.risorse.push(risorsaScarico);
      }
    }
}