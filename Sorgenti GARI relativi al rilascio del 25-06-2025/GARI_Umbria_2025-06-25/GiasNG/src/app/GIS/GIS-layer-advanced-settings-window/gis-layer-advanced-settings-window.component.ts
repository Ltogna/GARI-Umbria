import { Component, OnDestroy, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { TranslocoService } from '@jsverse/transloco';
import { CancelEvent, EditEvent, GridComponent, RemoveEvent, SaveEvent } from '@progress/kendo-angular-grid';
import { TextBoxComponent } from '@progress/kendo-angular-inputs';
import { KendoWindowsService, WindowArgs, WindowTypes } from 'app/Service';
import {
  AggiornaElementoGraficoPerTipoOggetto_In,
  AttivaAttributoLayer_In,
  AttributoLayer,
  AttributoLayer_In,
  AttributoLayer_In_OperazioneAttributo,
  GisClient,
  ImpostaCampoChiaveLayer_In,
  ImpostaVisualizzazioneEtichetta_In,
  LeggiImpostazioniAvanzateLayer,
  RispostaStandard_1OfLeggiImpostazioniAvanzateLayer,
  TipologiaLayer
} from 'app/Service/api.service';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import { GiasMessageService } from 'app/Service/gias-message.service';
import {catchError, finalize, Observable, of, Subscription, switchMap, tap} from 'rxjs';
import { DdlFeatureTypeElement } from '../GIS-layer-visibility-configuration-window/GIS-layer-visibility-configuration-window.component';
import { GISLayerAdvancedSettingsWindowService } from './GIS-layer-advanced-settings-window.service';
import {GoogleMapGeoJsonService} from '../google-map/google-map-geojson.service';
import {FeatureType} from '../../Model/GIS/GisDataReadRval_New';

@Component({
  standalone: false,
  selector: 'gis-layer-advanced-settings-window',
  templateUrl: './gis-layer-advanced-settings-window.component.html',
  styleUrls: ['./gis-layer-advanced-settings-window.component.css']
})
export class GISLayerAdvancedSettingsWindowComponent implements OnDestroy {
  @ViewChild('textbox') textbox: TextBoxComponent;
  @ViewChild('grid') grid: GridComponent;

  formGroup: FormGroup;
  windowArgs: WindowArgs;
  loading: boolean = false;
  attributesData: AttributoLayer[] = [];
  featureTypeElements: DdlFeatureTypeElement[] = [];
  featureTypeSelectedId: number;
  saveVisibile: boolean = false;

  private editedRowIndex: number;
  private hasEtichetta: boolean;
  private editedRow: GridComponent;
  private data: LeggiImpostazioniAvanzateLayer;
  private newAttributeName: string;
  private subscriptions: Subscription[] = [];

  constructor(
    private kendoWindowsService: KendoWindowsService,
    private gisLayerAdvancedSettingsWindowService: GISLayerAdvancedSettingsWindowService,
    private gisClient: GisClient,
    private giasMessageService: GiasMessageService,
    private transloco: TranslocoService,
    private giasDialogService: GiasDialogService,
    private googleMapGeoJsonService: GoogleMapGeoJsonService
  ) {
    this.popolaDdlFeatureTypeElements();

    this.subscriptions.push(
      this.kendoWindowsService
        .windowToggle$
        .subscribe(([windowTypes, args]) => {
          if (windowTypes === WindowTypes.LayerAdvancedSettingsWindow) {
            this.windowArgs = args;
            if(this.grid) {
              this.closeEditor(this.grid);
            }
          }
        })
    );

    this.subscribeToNewData();
  }

  private popolaDdlFeatureTypeElements(): void {
    let featureTypes: DdlFeatureTypeElement[] = [
      new DdlFeatureTypeElement(FeatureType.Polygon, this.transloco.translate('Poligono')),
      new DdlFeatureTypeElement(FeatureType.Point, this.transloco.translate('Punto')),
      new DdlFeatureTypeElement(FeatureType.LineString, this.transloco.translate('Linea'))
    ];
    this.featureTypeElements.push(...featureTypes);
  }

  ngOnDestroy(): void {
    for (const sub of this.subscriptions) {
      sub.unsubscribe();
    }

    this.gisLayerAdvancedSettingsWindowService.selectLayer(null);
  }

  cambioChiaveAttributo(item: AttributoLayer): void {
    item.CampoChiave = item.CampoChiave === '0' ? '1' : '0';
    const payload: ImpostaCampoChiaveLayer_In = {
      Impostazione: item.CampoChiave === '1',
      ProgressivoDataStruct: item.ProgressivoDataStruct
    };

    this.gisClient
      .gisImpostaCampoChiaveLayer(payload)
      .subscribe(() => this.giasMessageService.successMessage('gis.AttributoSalvaOk', false, true));
  }

  cambioAttivitaAttributo(item: AttributoLayer): void {
    item.TemaAttivo = item.TemaAttivo === '0' ? '1' : '0';
    const payload: AttivaAttributoLayer_In = {
      IdLayer: this.gisLayerAdvancedSettingsWindowService.currentLayerSelected.id,
      ProgressivoDataStruct: item.ProgressivoDataStruct,
      Attivazione: item.TemaAttivo === '1'
    };

    this.gisClient
      .gisImpostaAttivazioneAttributoLayer(payload)
      .pipe(
        catchError((a, c) => {
          this.giasMessageService.errorMessage(a.message, false, true);
          return of(JSON.parse(a.response));
        })
      ).subscribe((r) => {
      if (r.RispostaOK) {
        this.giasMessageService.successMessage('gis.AttributoSalvaOk', false, true);
      }
    });
  }

  cambioVisibilitaEtichettaAttributo(item: AttributoLayer): void {
    item.NomeAttributo = item.NomeAttributo
      .startsWith('^') ? item.NomeAttributo
      .replace('^', '') : ('^')
      .concat(item.NomeAttributo);

    const payload: ImpostaVisualizzazioneEtichetta_In = {
      Impostazione: item.NomeAttributo.startsWith('^'),
      ProgressivoDataStruct: item.ProgressivoDataStruct,
    };

    this.gisClient
      .gisImpostaVisualizzazioneEtichettaLayer(payload)
      .pipe(
        catchError((a, c) => {
          this.giasMessageService.errorMessage(a.message, false, true);
          return of(JSON.parse(a.response));
        })
      ).subscribe((r) => {
      if (r.RispostaOK) {
        this.giasMessageService.successMessage('gis.AttributoSalvaOk', false, true);

        // ricarico le feature per ottenere una descrizione aggiornata delle stesse (AppIdRate)
        this.googleMapGeoJsonService.loadGeoJsonForzato(false);
      }
    });
  }

  cambioTipoOggetto(): void {
    const payload: AggiornaElementoGraficoPerTipoOggetto_In = {
      GIS_TipoOggetto_Cod: this.featureTypeSelectedId,
      GIS_TipoOggetto_Cod_Prev: Number.parseInt(this.data.TipoGIS),
      LayerElementiGrafici_Cod: Number.parseInt(this.gisLayerAdvancedSettingsWindowService.currentLayerSelected.id)
    };

    this.gisClient
      .gisAggiornaElementoGraficoPerTipoOggetto(payload)
      .subscribe(() => {
        this.giasMessageService.successMessage('gis.AttributoSalvaOk', false, true);
        this.subscribeToNewData();
      });
  }

  change(event: any): void {
    if (event === '') {
      this.saveVisibile = false;
      return;
    }

    this.newAttributeName = event;
    this.saveVisibile = true;
  }

  newAttributeToggle(): void {
    if (!this.saveVisibile) {
      this.textbox.focus();
      return;
    }

    this.newAttributeName = this.newAttributeName.startsWith('^') ? this.newAttributeName.replace('^', '') : this.newAttributeName;
    if (this.saveVisibile && this.newAttributeName !== '') {
      this.gisClient
        .gisAttributoLayer({
          IdLayer: this.gisLayerAdvancedSettingsWindowService.currentLayerSelected.id,
          IdOperazione: AttributoLayer_In_OperazioneAttributo.INSERT,
          Nome: this.newAttributeName,
          TipoDato: 'string'
        } as AttributoLayer_In)
        .subscribe({
          next: _ => {
            this.giasDialogService.baseSuccess('Salvataggio attributo', 'gis.SalvataggioNuovoAttributoSuccesso');
            this.textbox.value = '';
            this.saveVisibile = false;
            this.subscribeToNewData();
          },
          error: _ => {
            this.giasDialogService.baseError('Salvataggio attributo', 'gis.ErroreSalvataggioAttributo');
          }
        });
    }
  }

  protected editAttributo(args: EditEvent): void {
    this.closeEditor(args.sender);
    this.hasEtichetta = args.dataItem.NomeAttributo.startsWith('^');
    this.editedRow = args.sender;

    this.formGroup = new FormGroup({
      NomeAttributo: new FormControl(args.dataItem.NomeAttributo.startsWith('^') ? args.dataItem.NomeAttributo.replace('^', '') : args.dataItem.NomeAttributo)
    });

    this.editedRowIndex = args.rowIndex;
    args.sender.editRow(args.rowIndex, this.formGroup);
  }

  protected updateAttributo(args: SaveEvent): void {
    let newName = this.formGroup.value.NomeAttributo;
    newName = newName.startsWith('^') ? newName.replace('^', '') : newName;

    this.gisClient
      .gisAttributoLayer({
        IdLayer: this.gisLayerAdvancedSettingsWindowService.currentLayerSelected.id,
        IdOperazione: AttributoLayer_In_OperazioneAttributo.UPDATE,
        ProgressivoDataStruct: args.dataItem.ProgressivoDataStruct,
        Nome: this.hasEtichetta ? ('^').concat(newName) : newName
      } as AttributoLayer_In)
      .subscribe({
        next: _ => {
          this.giasMessageService.successMessage('gis.AttributoSalvaOk', false, true)
          this.subscribeToNewData();
        }
      });

    this.closeEditor(args.sender);
  }

  protected cancelEditAttributo(args: CancelEvent): void {
    this.closeEditor(args.sender);
  }

  private closeEditor(grid: GridComponent): void {
    grid.closeRow(this.editedRowIndex);
    this.editedRowIndex = undefined;
    this.formGroup = undefined;
    this.hasEtichetta = false;
  }

  protected deleteAttributo(args: RemoveEvent): void {
    this.gisClient
      .gisAttributoLayer({
        IdLayer: this.gisLayerAdvancedSettingsWindowService.currentLayerSelected.id,
        IdOperazione: AttributoLayer_In_OperazioneAttributo.DELETE,
        ProgressivoDataStruct: args.dataItem.ProgressivoDataStruct
      } as AttributoLayer_In)
      .subscribe({
        next: _ => {
          this.giasMessageService.successMessage('gis.AttributoSalvaOk', false, true)
          this.subscribeToNewData();
        }
      });
  }

  isBloccato(): boolean {
    return this.data != undefined && this.data.Bloccato != '0';
  }

  private subscribeToNewData(): void {
    this.subscriptions.push(
      this.gisLayerAdvancedSettingsWindowService
        .layerSelected$
        .pipe(
          switchMap(() => this.loadData()),
          tap(data => this.assignData(data.RispostaStringa))
        )
        .subscribe()
    );
  }

  private assignData(data: LeggiImpostazioniAvanzateLayer): void {
    this.data = data;
    this.attributesData = this.data.ListaAttributiLayer;
    this.featureTypeSelectedId = Number.parseInt(this.data.TipoGIS);
  }

  private loadData(): Observable<RispostaStandard_1OfLeggiImpostazioniAvanzateLayer> {
    const layer: TipologiaLayer = this.gisLayerAdvancedSettingsWindowService.currentLayerSelected;
    if (layer == null) {
      return of();
    }

    this.loading = true;

    return this.gisClient
      .gisLeggiImpostazioniAvanzateLayer(+layer.id)
      .pipe(
        finalize(() => this.loading = false)
      );
  }
}
