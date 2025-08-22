import { effect, Injectable, Injector, OnDestroy } from "@angular/core";
import { LeggiGiacenzeZooDto, OperazioniZooClient } from "app/Service/net-core6-api.service";
import { AbstractGridConfigService, CommandsColumnSettings, CommandsDropDownSettings, ConfigTemplate, EditingMode, GiasKendoGridComponent, GridPublicService, HttpAction, KendoGridColumn, KendoGridModel, KendoGridRow, KendoServerResult, LoaderType, SelectableSettings } from "gias-kendo-grid";
import { Observable, catchError, map, of, Subject, takeUntil, merge } from "rxjs";
import { CapiAnimaliConfigService, CapoAnimale } from "./capi-animali-config.service";
import { CellClickEvent, CellCloseEvent, SelectionEvent } from "@progress/kendo-angular-grid";
import { RisorsaPersona } from "app/Model/attivita/risorse/RisorsaPersona";
import { enum_Tipo_RisorsaUmana, enum_UdM_Dose } from "app/zoo/models/tipi-enumerativi-zoo";
import { GiasMessageService } from "app/Service/gias-message.service";
import { TrattamentoZooFormService } from "../../../service/trattamento-zoo-form.service";
import { enum_UnitaMisura } from "app/Model/TipiEnumerativi";
import { Enum_DBTypeOperation } from "gias-ui-kit";

export class GridCapiAnimaliServerResult extends KendoServerResult {
    constructor(rows: KendoGridRow[], cols: KendoGridColumn[], model: KendoGridModel) {
        super(model, cols, rows);
    }
}

@Injectable()
export class GridCapiAnimaliService extends AbstractGridConfigService<GridCapiAnimaliServerResult> implements OnDestroy {
    
    editingMode: EditingMode = EditingMode.IN_CELL;
    loader: LoaderType = LoaderType.SERVICE;

    rowId: string = 'idCapiAnimali';
    gridId: string = 'GridCapiAnimaliId';

    signal: Subject<void> = new Subject();

    CFproprietario: string = "";

    isEditing: boolean = true;

    totaleFarmacoDaSomministrare: number = 0;
    isTotEditable: boolean = false;

    public numericFormat: string;

    constructor(injector: Injector,
                public gridpublicService: GridPublicService,
                public trattamentoFormService: TrattamentoZooFormService,
                private giasMessageService: GiasMessageService,
                private capiAnimaliConfigService: CapiAnimaliConfigService,
                private zooService: OperazioniZooClient
    ) {

        super(injector, ConfigTemplate.DefaultTemplate);

        this.resizable.autoFitColumns = true;

        this.cmdColumn = new CommandsColumnSettings({
            editBtn: false,
            infoBtn: false,
            removeBtn: false,
            onDisableInfoBtn: () => false,
            width: 10
        });

        this.cmdDropDown = new CommandsDropDownSettings({
            removeBtn: false, infoBtn: false
        });

        
        merge(
              this.trattamentoFormService.ddlSelectionSubject,
              this.trattamentoFormService.ddlSelectionRaggruppamentoSubject
        ).pipe(
                takeUntil(this.signal)
        ).subscribe(value => {
            this.gridpublicService.refresh(true);
        });

        if (!this.trattamentoFormService.isInfoMode) {
            this.selectable.selectable = new SelectableSettings({
                checkboxOnly: true,
                enabled: true
            });

            this.selectable.preselectedRows.selectionChangeFn = this.selectionChangeFn;
            this.selectable.columnSettings.showSelectAll = (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB !== Enum_DBTypeOperation.Update);
            this.selectable.shouldShowCheckbox = true;
            this.selectable.columnSettings.title = ' ';
        }

        this.groups.groupable.enabled = false;

        this.onCellClick = (event: CellClickEvent) => {
            if(event.column.field === 'Quantita') {
                this.totaleFarmacoDaSomministrare -= event.dataItem['Quantita'];
            }
        };


        this.onCellClose = (event: CellCloseEvent) => {
            if (event.column.field === "Quantita") {
                this.totaleFarmacoDaSomministrare += event.dataItem['Quantita'];
            }
        };

        //verifico che mi sia stato passato il proprietario
        let risorsa: RisorsaPersona = this.trattamentoFormService.attivitaDaChiamante?.risorse.find(item => item.classType == 'RisorsaPersona' && item?.risorsaUmana?.contatto?.tipo === enum_Tipo_RisorsaUmana.Proprietario) ?? null;
        this.CFproprietario = risorsa?.risorsaUmana?.contatto?.codiceFiscale ?? "";

        effect(() => { 
            //effect per gestire la signal changeDrugSelected
            const value = this.trattamentoFormService.changeDrugSelected();
            this.calculateNumericFormat(value.udm);
            if (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB !== Enum_DBTypeOperation.Update)
                this.deselezionaCapiAnimali();
        });

    }

    public selectionChangeFn = (event: SelectionEvent, component: GiasKendoGridComponent) => {

        const selectedRows = event.selectedRows;
        const deselectedRows = event.deselectedRows;

        if (component.rows.every(row => row['Selected']))
            this.totaleFarmacoDaSomministrare = 0;

        selectedRows.forEach(row => {
            if (this.trattamentoFormService.objParametriAgenda.TipoOperazioneDB !== Enum_DBTypeOperation.Update) {
                row.dataItem['Quantita'] = this.calcolaDoseDaSomministrare(row.dataItem['Incremento_Teorico_Calcolato'], this.trattamentoFormService.dettagliProtocollo.qtaDose, this.trattamentoFormService.dettagliProtocollo.udmDose);
                this.totaleFarmacoDaSomministrare += row.dataItem['Quantita'];
            } else {
                if (!this.trattamentoFormService.attivitaDaChiamante?.centriDiCosto
                    .filter(itemCentro => itemCentro.classType == 'CapoAnimaleCDC')
                    .some(itemCentro => itemCentro.capoAnimale.codice == row.dataItem['Cod_Animale'])) {
                        component.rows.find(item => item['Matricola'] == row.dataItem['Matricola'])['Selected'] = false;
                        this.giasMessageService.warningMessage('zoo.CapiAnimaliModificaNonSelezionabili', false, true);
                } else {
                    const capoAnimale = this.trattamentoFormService.attivitaDaChiamante?.centriDiCosto.filter(itemCentro => itemCentro.classType == 'CapoAnimaleCDC')?.find(itemCentro => itemCentro.capoAnimale.codice == row.dataItem.Cod_Animale);
                    row.dataItem['Quantita'] = capoAnimale?.qtaSomministrata ?? 0;
                    this.totaleFarmacoDaSomministrare += row.dataItem['Quantita'];
                }
            }
        });

        deselectedRows.forEach(row => {
            if (this.totaleFarmacoDaSomministrare > row.dataItem['Quantita'])
                this.totaleFarmacoDaSomministrare -= row.dataItem['Quantita'];
            else
                this.totaleFarmacoDaSomministrare = 0;

            row.dataItem['Quantita'] = 0;
           
        });
    }

    calculateNumericFormat(UnitaDiMisura: string | number): void {

        if (UnitaDiMisura == null || UnitaDiMisura == undefined)
            return;

        this.numericFormat = '#.0000';
        let udm: string = '';
        
        if (typeof UnitaDiMisura === 'number') {
            switch(UnitaDiMisura) {
                    //case enum_UdM_Dose.mg_su_Kg:
                    case enum_UnitaMisura.Milligrammi:
                        udm = 'mg';
                    break;
                    case enum_UdM_Dose.ml_su_100Kg:
                    case enum_UdM_Dose.ml_su_Capo:
                    case enum_UnitaMisura.Millilitri:
                        udm = 'ml';
                    break;
                    default:
                        udm = '';
            }
        } else {
            udm = UnitaDiMisura;
        }

        this.numericFormat += ' ' + udm;
    }

    read(options?: any): Observable<GridCapiAnimaliServerResult> {
        this.loadingService.set_isLoading({ isLoading: true, component: this.gridPublicService.gridElRef });

        if (this.trattamentoFormService.isInfoMode) { 

            this.totaleFarmacoDaSomministrare = 0;
            //se sono in modalità info, non faccio la chiamata al servizio, ma prendo i dati già passati
            let capiAnimaliInfo: Array<KendoGridRow> = [];

            this.trattamentoFormService.attivitaDaChiamante.centriDiCosto
            .filter(itemCentro => itemCentro.classType == 'CapoAnimaleCDC')
            .forEach(itemCentro => {
                let itemCapoAnimale = new CapoAnimale(itemCentro.capoAnimale.codice, 
                                                      itemCentro.capoAnimale.matricola, 
                                                      itemCentro.sottogruppoStalla_ingresso.nome, 
                                                      this.trattamentoFormService.formTrattamentoZoo.get('Stalla').value, 
                                                      '', 
                                                      itemCentro.capoAnimale.razza.descrizione, 
                                                      itemCentro.capoAnimale.statiAccrescimento[0].descrizione, 
                                                      itemCentro.capoAnimale.validita.inizio, 
                                                      itemCentro.capoAnimale.validita.fine, 
                                                      0, 
                                                      itemCentro.qtaSomministrata);
                capiAnimaliInfo.push(itemCapoAnimale);
                this.totaleFarmacoDaSomministrare += itemCentro.qtaSomministrata;
            });

            this.loadingService.set_isLoading({ isLoading: false, component: this.gridPublicService.gridElRef });

            return of(new GridCapiAnimaliServerResult(
                capiAnimaliInfo,
                this.capiAnimaliConfigService.kendoColumnsCapiAnimali,
                this.capiAnimaliConfigService.kendoModelCapiAnimali
            ));

        } else {

            //se sono in modalità modifica o nuova, faccio la chiamata al servizio per prendere i capi animali
            let formValues = this.trattamentoFormService.formTrattamentoZoo.getRawValue();

            let param = {
                Piva: this.trattamentoFormService.objParametriAgenda.Piva,
                CodCentro: formValues?.CentroAziendale?.codice ?? 0,
                CodStalla: formValues?.Stalla?.STA_NUM ?? 0,
                CodRaggruppamento: formValues?.Raggruppamento?.codice ?? 0,
                CodAnimale: 0,
                Matricola: '',
                DataGiacenza: formValues?.Data,
                Istantanea: true,
                MostraPesate: true,
                FiltraFornitori: true,
                ListaCodAnimali: [],
                /** Mostra data primo giorno caricamento e giorni in stalla da quella data */
                MostraGGPrimoCaricamento: false,
                CFproprietario: this.CFproprietario
            } as LeggiGiacenzeZooDto;

            return this.zooService.operazioniZooGetGiacenzeZoo(param).pipe(catchError((err) => {
                this.loadingService.set_isLoading({ isLoading: false, message: '', component: this.gridPublicService.gridElRef });
                return of();
            }), map((r:any) => {

                this.loadingService.set_isLoading({ isLoading: false, component: this.gridPublicService.gridElRef });

                let respobj = JSON.parse(r.RispostaStringa);

                this.totaleFarmacoDaSomministrare = 0;

                let gridCapiAnimaliServerResult = new GridCapiAnimaliServerResult(
                    this.setRowGrid(respobj),
                    this.capiAnimaliConfigService.kendoColumnsCapiAnimali,
                    this.capiAnimaliConfigService.kendoModelCapiAnimali
                );

                return gridCapiAnimaliServerResult;
            }));
        }
    }

    private setRowGrid(responseRows): Array<KendoGridRow> {
        let rows: Array<KendoGridRow> = [];
        let indexCapiAnimali = 0;

        const centriDiCosto = this.trattamentoFormService.attivitaDaChiamante?.centriDiCosto.filter(item => item.classType === 'CapoAnimaleCDC') || [];

        // Filtra le righe se ci sono dati nei centri di costo
        const filteredRows = centriDiCosto.length > 0
            ? responseRows.filter(itemOfRows => 
                centriDiCosto.some(itemCentro => itemCentro.capoAnimale.codice === itemOfRows.Cod_Animale)
            )
            : responseRows;

        for (let itemOfRows of filteredRows) {
            indexCapiAnimali++;
            itemOfRows.idCapiAnimali = indexCapiAnimali;
            
            const capoAnimale = this.trattamentoFormService.attivitaDaChiamante?.centriDiCosto.filter(itemCentro => itemCentro.classType == 'CapoAnimaleCDC')?.find(itemCentro => itemCentro.capoAnimale.codice == itemOfRows.Cod_Animale);

            //se entro in modifica, sicuramente ci sono dei capi selezionati
            itemOfRows.Selected = (capoAnimale)? true : false;
            itemOfRows.Quantita = itemOfRows.Selected? 
                                    ((capoAnimale.qtaSomministrata == 0)? 
                                        this.calcolaDoseDaSomministrare(itemOfRows['Incremento_Teorico_Calcolato'], this.trattamentoFormService.dettagliProtocollo.qtaDose, this.trattamentoFormService.dettagliProtocollo.udmDose)
                                        : capoAnimale.qtaSomministrata)
                                    : 0;
            this.totaleFarmacoDaSomministrare += itemOfRows.Quantita;

            rows.push(itemOfRows);
        }

        rows.sort((a, b) => {
            return (b['Selected'] ? 1 : 0) - (a['Selected'] ? 1 : 0);
        });

        return rows;
    }

    private calcolaDoseDaSomministrare(pesoAnimale: number, dose: number, udmDose: enum_UdM_Dose ): number {

        let doseDaSomministrare: number = 0;
        switch (udmDose) {
            // case enum_UdM_Dose.mg_su_Kg:
            //     doseDaSomministrare = pesoAnimale * dose;
            //     break;
            case enum_UdM_Dose.ml_su_100Kg:
                doseDaSomministrare = pesoAnimale * dose / 100;
                break;
            case enum_UdM_Dose.ml_su_Capo:
                doseDaSomministrare = dose;
                break;
            case enum_UdM_Dose.n_su_Capo:
                doseDaSomministrare = dose;
                break;
            default:
                doseDaSomministrare = 0;
        }

        return doseDaSomministrare;
    }

    private deselezionaCapiAnimali() {
        let refValue = this.gridpublicService.getValue()?.data ?? null;
        this.totaleFarmacoDaSomministrare = 0;
        if (refValue) {
            refValue.rows.filter(item => item['Selected']).forEach(item => {
                    item['Selected'] = false;
                    item['Quantita'] = 0; //this.calcolaDoseDaSomministrare(item['Incremento_Teorico_Calcolato'], this.trattamentoFormService.dettagliProtocollo.qtaDose, this.trattamentoFormService.dettagliProtocollo.udmDose);
            });
        }
    }

    modificaTotEDistribuisci() {
        let qtaDaDistrubuire = this.totaleFarmacoDaSomministrare / this.gridpublicService.getValue().data.rows.filter((row: any) => row.Selected).length;
        this.gridpublicService.getValue().data.rows.filter(item => item['Selected']).forEach(item => {  
            item['Quantita'] = qtaDaDistrubuire;
        });
        this.isTotEditable = false;
        this.giasMessageService.infoMessagge('zoo.ModificaTotaleDistribuzioneCapiAnimali', false, true);
    }

    ngOnDestroy(): void {
        this.signal.next();
        this.signal.complete();
    }

    perform(actionType: HttpAction, items: any, oldRow?: any): Observable<any> {
        throw new Error("Method not implemented.");
    }

}