import { Component, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { KendoWindowsService, WindowArgs, WindowTypes } from 'app/Service';
import { Cancella_In, GisClient } from 'app/Service/api.service';
import { Subject, takeUntil, tap } from 'rxjs';
import { FeatureService } from '../services/feature.service';
import { TranslocoService } from '@jsverse/transloco';
import { GisService } from '../GIS.service';
import { GoogleMapGeoJsonService } from '../google-map/google-map-geojson.service';
import { TreeGisService } from "app/Utility/Template/kendo-tree/services/tree-gis.service";
import { SharedDataService } from '../services/shared-data.service';
import { MasterService } from '../../Service/master.service';
import { GiasDialogService } from '../../Service/gias-dialog.service';
import { GeoJSONAgroGisProp, GeoJson_Feature_New } from 'app/Model/GIS/GisDataReadRval_New';

@Component({
  standalone: false,
  selector: 'gis-delete-feature-window',
  templateUrl: './GIS-delete-feature-window.component.html',
  styleUrls: ['./GIS-delete-feature-window.component.css']
})
export class GISDeleteFeatureWindowComponent implements OnDestroy {

  windowArgs: WindowArgs;
  form: FormGroup | null = null;
  featureSelezionate: GeoJson_Feature_New<GeoJSONAgroGisProp>[] = [];
  permessoPrecisionFarming = false;

  private signal = new Subject<void>();

  constructor(
    private kendoWindowsService: KendoWindowsService,
    private featureService: FeatureService,
    private gisClient: GisClient,
    private formBuilder: FormBuilder,
    private translocoService: TranslocoService,
    private gisService: GisService,
    private googleMapGeoJsonService: GoogleMapGeoJsonService,
    private treeGisService: TreeGisService,
    private sharedDataService: SharedDataService,
    protected masterService: MasterService,
    private giasDialogService: GiasDialogService
  ) {
    this.kendoWindowsService.windowToggle$
      .pipe(takeUntil(this.signal))
      .subscribe(([windowTypes, args]) => {
        if (windowTypes === WindowTypes.DeleteLayerWindow) {
          this.windowArgs = args;
        }
      });

    this.featureService.getFeatureSelezionate$()
      .pipe(takeUntil(this.signal))
      .subscribe(featureSelezionate => this.buildForm(featureSelezionate));

    this.sharedDataService.getCfgGisGenerali$()
      .pipe(takeUntil(this.signal))
      .subscribe(cfg => {
        if (cfg.permessoPrecisionFarming) {
          this.permessoPrecisionFarming = true;
        } else {
          this.permessoPrecisionFarming = false;
        }
      });

  }

  ngOnDestroy() {
    this.signal.next();
    this.signal.complete();
  }

  buildForm(featureSelezionate: GeoJson_Feature_New<GeoJSONAgroGisProp>[]): void {
    this.featureSelezionate = featureSelezionate;
    if (featureSelezionate.length === 0) {
      return;
    }

    const entita_cod = featureSelezionate.map(x => x.properties.Entita_Cod)[0];

    this.form = this.formBuilder.group({
      Elimina_DatoGiasPalm: new FormControl(false),
      Elimina_Grafica: new FormControl(false),
      Elimina_Impianto: new FormControl(false),
      Elimina_PrecisionFarming: new FormControl(false),
      Elimina_PrecisionFarmingABLine: new FormControl(false),
      Entita_Cod: new FormControl({ value: entita_cod, disabled: true }, Validators.required),
      // confirmDelete: new FormControl(false, [Validators.requiredTrue, Validators.]), // TODO GEOJSON
      confirmDelete: new FormControl(false, Validators.requiredTrue),
      Sementi: new FormControl(this.sharedDataService.getCfgSementiAsValue()?.Sementi),
      SementiMappaturaLibera: new FormControl(this.sharedDataService.getCfgSementiAsValue()?.SementiMappaturaLibera),
      DatiPassaggio: new FormControl(null),
    });
  }

  submit(): void {
    if (this.featureSelezionate.length > 1) {
      this.giasDialogService.baseError('Elimina feature', 'Eliminazione multipla non supportata');
      // this.giasMessageService.errorMessage("Eliminazione multipla non supportata");
      this.masterService.set_isLoading({ isLoading: false })
      return;
    }

    const formValue = this.form.getRawValue();
    delete formValue.confirmDelete;

    const payload = formValue as Cancella_In;
    if (!payload.Elimina_Grafica && !payload.Elimina_PrecisionFarming && !payload.Elimina_PrecisionFarmingABLine) {
      this.giasDialogService.baseError('', 'gis.SelezionaAlmenoUnOpzione', true);
      return;
    }

    const featureCancellata = this.featureService.getUltimaFeatureSelezionata();
    this.masterService.set_isLoading({ isLoading: true })
    this.gisClient
      .gisCancella(payload)
      .pipe(tap(() => {
        // this.featureService.deleteFeature(payload.Entita_Cod);
        this.kendoWindowsService.close(WindowTypes.DeleteLayerWindow);
        // Cancellazione nodo albero
        if (payload.Elimina_Impianto) {
          const chiaveAlberoFeatureCancellata = this.featureService.getChiaveAlberoCompletaByFeature(featureCancellata);
          this.treeGisService.cancellaNodoAlbero(chiaveAlberoFeatureCancellata);
        }
        // Cancellazione feature
        if (payload.Elimina_Grafica) {
          this.googleMapGeoJsonService.cancellaFeature(featureCancellata);
        }
        // NB: al momento l'opzione Elimina_Impianto è stata disabilitata;
        //     se viene ripristintata, in caso di cancellazione appezzamento occorre cancellare
        //     anche la feature dell'impianto eliminato; valutare se questo dato lo deve essere
        //     ritornato lato server.
      }))
      .subscribe({
        next: okData => this.gestioneCancellazioneCorretta(),
        error: errorData => this.gestioneCancellazioneErrata(errorData),
        complete: () => {
          console.log('gisCancella complete');
          this.masterService.set_isLoading({ isLoading: false });
          this.featureService.deleteFeature(featureCancellata.properties.id);
        }
      });
  }

  gestioneCancellazioneCorretta() {
    // const messaggio = this.translocoService.translate("gis.FeatureEliminataCorrettamente");
    // this.giasMessageService.successMessage(messaggio);
    this.giasDialogService.baseSuccess('Elimina feature', 'gis.FeatureEliminataCorrettamente');
  }

  gestioneCancellazioneErrata(errorData: any) {
    const messaggioStandard = this.translocoService.translate("gis.ErroreEliminazioneFeature");
    this.gisService.gestioneErroreGisClient(errorData, messaggioStandard);
  }

}
