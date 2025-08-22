import { Component } from '@angular/core';
import { PanelBarExpandEvent } from '@progress/kendo-angular-layout';
import { KendoWindowsService, WindowArgs, WindowTypes } from 'app/Service';
import { AttivazioneConfigurazioneAlgoritmiCartografici, AuthDispatcherClient, EntitaAlgoritmoCartografico, GisClient, Gis_Sat_Sentinel_Overlay, MascheraLayerRaster, RichiestaAbacoType, RichiestaAbacoUrl_In, RichiestaSignedUrl_In, RispostaStandard_1OfElencoUrlFirmati, UrlFirmato } from 'app/Service/api.service';
import { combineLatest, filter, first, firstValueFrom, forkJoin, interval, map, merge, Observable, of, pairwise, race, startWith, Subject, switchMap, take, tap, withLatestFrom } from 'rxjs';
import { enum_LayerElementiGraficiStd } from '../GIS-enum/GIS-layer-elementi-grafici';
import { GoogleMapService } from '../google-map/google-map.service';
import { DEFAULT_CHART_WIDTH } from './GIS-analisi-mappe-satellitari-grafici/GIS-analisi-mappe-satellitari-grafici.component';
import { GISAnalisiMappeSatellitariWindowService, MappeSatellitariData } from './GIS-analisi-mappe-satellitari-window.service';
import { Tile } from '../utils/mercator.utils';
import { RasterParameterVisualizationLayer, RasterParameterVisualizationType } from '../services/raster-overlay.service';
import { FeatureService } from '../services/feature.service';
import { enum_Security_Attivita } from 'app/Model/TipiEnumerativi';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { enum_FeatureProperty } from '../GIS-enum/GIS-feature';
import { enum_TipologiaLayer } from '../GIS-enum/GIS-tipologia-layer';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import { TranslocoService } from '@jsverse/transloco';
import { DialogRef } from '@progress/kendo-angular-dialog';
import { SatelliteLocalDataLoader } from './satellite-local-data-loader';
import { SatelliteGlobalDataLoader } from './satellite-global-data-loader';
import { GiasMessageService } from 'app/Service/gias-message.service';
import { NotificationRef } from '@progress/kendo-angular-notification';
import { GeoJSONAgroGisProp, GeoJson_Feature_New } from 'app/Model/GIS/GisDataReadRval_New';

const SDAC_DEFAULT = 53;

@Component({
  standalone: false,
  selector: 'gis-analisi-mappe-satellitari-window',
  templateUrl: './GIS-analisi-mappe-satellitari-window.component.html',
  styleUrls: ['./GIS-analisi-mappe-satellitari-window.component.css']
})
export class GISAnalisiMappeSatellitariWindowComponent {
  selectedTabSubject = new Subject<PanelBarExpandEvent>();

  windowArgs$: Observable<WindowArgs>;
  title$: Observable<string>;
  selectedTab$: Observable<number>;
  mapInfo$: Observable<boolean>;
  loader$: Observable<any>;
  mapClick$: Observable<boolean>;

  private lastWidth: number;
  private dialogRef: DialogRef | null = null;
  private notificationRef: NotificationRef;
  private satelliteDataAlgorithmCod: number;

  constructor(
    private kendoWindowsService: KendoWindowsService,
    private gisAnalisiMappeSatellitariWindowService: GISAnalisiMappeSatellitariWindowService,
    private googleMapService: GoogleMapService,
    private gisClient: GisClient,
    private authDispatcherClient: AuthDispatcherClient,
    private featureService: FeatureService,
    private permessiUtenteService: PermessiUtenteService,
    private giasDialogService: GiasDialogService,
    private giasMessageService: GiasMessageService,
    private translocoService: TranslocoService,
    private satelliteLocalDataLoader: SatelliteLocalDataLoader,
    private satelliteGlobalDataLoader: SatelliteGlobalDataLoader
  ) {
    this.gisClient
      .gisLeggiMaschereLayerRaster({ TipologiaLayer_Raster_Cod: +enum_TipologiaLayer.Entita, LayerElementiGrafici_Raster_Cod: +enum_LayerElementiGraficiStd.ANALISI_MAPPE_SATELLITARI })
      .pipe(tap(res => this.gisAnalisiMappeSatellitariWindowService.nextMasks(res.RispostaStringa.elencoMaschere)))
      .subscribe();

    this.windowArgs$ = this.kendoWindowsService.windowToggle$.pipe(
      filter(([windowTypes, _]) => windowTypes === WindowTypes.AnalisiMappeSatellitariWindow),
      map(([_, args]) => args)
    );

    const isActive$ = this.gisAnalisiMappeSatellitariWindowService.isActive$;
    const date$ = this.gisAnalisiMappeSatellitariWindowService.currentDate$;
    const sensor$ = this.gisAnalisiMappeSatellitariWindowService.sensor$;
    const overlays$ = this.gisAnalisiMappeSatellitariWindowService.overlays$;

    this.title$ = combineLatest([isActive$, date$, sensor$]).pipe(
      withLatestFrom(this.windowArgs$, overlays$),
      map(([[isActive, date, sensor], args, overlays]) => {
        const parsedDate = GISAnalisiMappeSatellitariWindowService.dateToString(date);
        const title = this.computeTitle(isActive, args, parsedDate, sensor);
        const overlay = overlays.find(x => new Date(x.overlay.DataRiferimento).toDateString() == date.toDateString());
        const isCloudy = overlay?.overlay.Passaggi.find(x => x.Sensore.some(x => x.CodiceSensore == sensor))?.url == '';

        const oldArgs = this.kendoWindowsService.getWindowArgs(WindowTypes.AnalisiMappeSatellitariWindow);
        oldArgs.title = title;
        oldArgs.additionalArgs = { ...oldArgs.additionalArgs, isCloudy: isCloudy };
        this.kendoWindowsService.override(WindowTypes.AnalisiMappeSatellitariWindow, oldArgs);

        return title;
      }));

    this.mapInfo$ = this.gisAnalisiMappeSatellitariWindowService
      .mapInfo$
      .pipe(
        withLatestFrom(overlays$),
        map(([value, overlays]) => {
          const map = this.googleMapService.googleMapWrapper?.data?.getMap();
          const cursor = value && (overlays.length == 0 || !overlays[0].applyToPolygon) ? 'crosshair' : '';
          map?.setOptions({ draggableCursor: cursor, });
          return value;
        })
      );

    this.mapClick$ = this.gisAnalisiMappeSatellitariWindowService.mapClickEvent$.pipe(
      map(() => true),
      startWith(false)
    );

    const tab$ = this.selectedTabSubject.asObservable().pipe(
      map(x => +x.item.id.replace('accordion-', ''))
    );

    this.selectedTab$ = merge(
      this.gisAnalisiMappeSatellitariWindowService.mapClickEvent$.pipe(map(() => 1)), // Open chart tab
      tab$
    ).pipe(startWith(0));

    const loader$ = this.gisAnalisiMappeSatellitariWindowService.externalLoadOnPolygons$
      .pipe(
        tap(() => this.gisAnalisiMappeSatellitariWindowService.nextOverlays([])),
        withLatestFrom(isActive$),
        filter(([_, active]) => active),
        withLatestFrom(this.gisAnalisiMappeSatellitariWindowService.masks$),
        switchMap(([_, masks]) => {
          const hasMasks = masks.filter(x => x.isAttivaPerUtenteCorrente).length > 0;
          const features = this.featureService.getFeatureSelezionate();
          if (hasMasks) {
            if (features.length == 0) {
              this.giasDialogService.baseInfo('', 'gis.SelezionaDeiPoligoniPerCaricareEVisualizzareIDatiEdAvviareLAnimazione', true);
            } else {
              this.giasDialogService.baseInfo('', 'gis.LAnimazionioneStaPerEssereCaricataSuiPoligoniSelezionati', true);
            }

            return this.satelliteLocalDataLoader.load();
          }

          return this.satelliteGlobalDataLoader.load();
        })
      );

    const overlaysLoader$ = loader$
      .pipe(
        withLatestFrom(
          this.gisAnalisiMappeSatellitariWindowService.dateFrom$,
          this.gisAnalisiMappeSatellitariWindowService.dateTo$,
          this.gisAnalisiMappeSatellitariWindowService.sensor$,
          this.gisAnalisiMappeSatellitariWindowService.opacity$,
          this.gisAnalisiMappeSatellitariWindowService.masks$,
          this.gisAnalisiMappeSatellitariWindowService.isActive$,
        ),
        tap(([[data, features, tiles], dateFrom, dateTo, sensor, opacity, masks, active]) => this.loadMappeSatellitariData(data, tiles, dateFrom, dateTo, sensor, features, opacity, masks, active))
      );

    const windowWidthLoader$ = this.selectedTab$.pipe(
      pairwise(),
      tap(([oldTab, newTab]) => {
        const args = this.kendoWindowsService.getWindowArgs(WindowTypes.AnalisiMappeSatellitariWindow);
        if (args == null) {
          return;
        }

        if (oldTab != 1 && newTab == 1) { // chart tab
          this.lastWidth = args.width;
          args.width = Math.max(args.width, DEFAULT_CHART_WIDTH);
        }

        if (oldTab == 1 && newTab != 1) {
          if (args.width != DEFAULT_CHART_WIDTH) { // width has been changed in chart tab
            this.lastWidth = args.width;
          }

          args.width = this.lastWidth;
        }

        this.kendoWindowsService.override(WindowTypes.AnalisiMappeSatellitariWindow, args);
      })
    );

    this.loader$ = merge(overlaysLoader$, windowWidthLoader$);
  }

  private computeTitle(isActive: boolean, args: WindowArgs, date: string | null, sensor: string | null): string {
    if (!isActive) {
      return args.additionalArgs?.originalTitle;
    }

    const elements = [];

    if (date != null) {
      elements.push(date);
    }

    if (sensor != null) {
      elements.push(sensor);
    }

    const title = elements.join(" - ")
    return title != '' ? title : args.additionalArgs?.originalTitle;
  }

  private loadMappeSatellitariData(overlays: Gis_Sat_Sentinel_Overlay[], tiles: Tile[], date1: Date, date2: Date, sensor: string, features: GeoJson_Feature_New<GeoJSONAgroGisProp>[], opacity: number, masks: MascheraLayerRaster[], active: boolean): void {
    if (date1 == null || date2 == null || !active) {
      this.gisAnalisiMappeSatellitariWindowService.nextOverlays([]);
      return;
    }

    const validOverlays = overlays.filter(element => {
      const date = GISAnalisiMappeSatellitariWindowService.stringToDate(element.DataRiferimento);
      // Fix time to be sure it does not effect dates
      date1.setHours(0);
      date.setHours(11);
      date2.setHours(23);
      return date >= date1 && date <= date2;
    });

    const applyToPolygon = applyOverlaysOnPolygons(this.permessiUtenteService, masks);
    if (applyToPolygon && overlays.length > 0 && validOverlays.length == 0 && features.length > 0) {
      this.notificationRef?.hide();
      this.notificationRef = this.giasMessageService.infoMessagge('gis.NonCiSonoDatiSatellitariDisponibiliNellIntervalloDiTempoSelezionato', false, true);
      this.gisAnalisiMappeSatellitariWindowService.nextOverlays([]);
      return;
    }

    if (applyToPolygon && overlays.length == 0 && features.length > 0) {
      this.gisAnalisiMappeSatellitariWindowService.nextOverlays([]);
      this.showNoFeatureDataError(features);
      return;
    }

    if (!applyToPolygon && validOverlays.length == 0) {
      this.gisAnalisiMappeSatellitariWindowService.nextOverlays([]);
      return;
    }

    this.gisClient.gisLeggiBaseUrlMappeSatellitari()
      .pipe(
        switchMap(res => {
          if (validOverlays.length == 0) {
            return of([]);
          }

          // If we have multiple dates, group them together
          const groupedOverlays = validOverlays.reduce((acc, current) => {
            const filteredCurrent = { ...current };
            filteredCurrent.Passaggi = filteredCurrent.Passaggi.filter(x => x.Sensore.find(s => s.CodiceSensore == sensor) != null);

            const existing = acc.find(x => x.DataRiferimento == current.DataRiferimento);
            if (existing == null) {
              acc.push(filteredCurrent);
            } else {
              existing.Passaggi = existing.Passaggi.concat(filteredCurrent.Passaggi);
            }
            return acc;
          }, [] as Gis_Sat_Sentinel_Overlay[]);

          const requests = groupedOverlays.map(overlay => {
            if (res.RispostaStringa.legacy_endpoint != null && res.RispostaStringa.legacy_endpoint !== '') {
              const layerParam = {} as RasterParameterVisualizationLayer;
              layerParam.type = RasterParameterVisualizationType.SATELLITE;
              layerParam.baseUrl = `${res.RispostaStringa.legacy_endpoint}/${overlay.Passaggi[0].url}/${sensor}/`;
              return this.handleVisualizationParameter([layerParam], tiles, overlay, sensor);
            }

            const layerParams: RasterParameterVisualizationLayer[] = [];
            for (const passaggio of overlay.Passaggi) {
              const layerParam = {} as RasterParameterVisualizationLayer;
              const data = res.RispostaStringa.datiEndpoint;
              layerParam.type = data.type;
              layerParam.baseUrl = data.baseUrl;
              layerParam.bucket = data.bucket;
              layerParam.obj = `${data.obj}/${passaggio.url}/${sensor}`;
              layerParams.push(layerParam);
            }

            return this.handleVisualizationParameter(layerParams, tiles, overlay, sensor);
          });

          return forkJoin(requests);
        }))
      .subscribe((result: [UrlFirmato | UrlFirmato[], Gis_Sat_Sentinel_Overlay][]) => {
        const overlays = result.map(([url, overlay]) => ({ sensor: sensor, overlay: overlay, urls: url, features: features, opacity: opacity, applyToPolygon: applyToPolygon } as MappeSatellitariData));
        if (applyToPolygon && features.length > 0 && !this.hasValidFeatures(overlays)) {
          this.showNoFeatureDataError(features);
          return;
        }

        this.gisAnalisiMappeSatellitariWindowService.nextOverlays(overlays);
      });
  }

  private hasValidFeatures(data: MappeSatellitariData[]): boolean {
    for (const element of data) {
      const urls = element.overlay.Passaggi.map(p => p.url);
      for (const feature of element.features) {
        const code = feature.properties.Entita_GUID;
        if (code == null || code.trim() == '') {
          continue;
        }

        for (const url of urls) {
          if (url.includes(code)) {
            return true;
          }
        }
      }
    }

    return false;
  }

  private handleVisualizationParameter(layerParams: RasterParameterVisualizationLayer[], tiles: Tile[], overlay: Gis_Sat_Sentinel_Overlay, sensor: string): Observable<[UrlFirmato[] | UrlFirmato, Gis_Sat_Sentinel_Overlay]> {
    const requests: Observable<RispostaStandard_1OfElencoUrlFirmati>[] = [];
    for (const layerParam of layerParams) {
      if (layerParam.type == RasterParameterVisualizationType.SATELLITE) {
        return this.getSatelliteRequests(layerParam, overlay);
      }

      if (layerParam.type == RasterParameterVisualizationType.PUBLIC || layerParam.type == RasterParameterVisualizationType.ABACO_PUBLIC) {
        return this.getRasterPublicRequests(tiles, layerParam, overlay, sensor);
      }

      if (layerParam.type == RasterParameterVisualizationType.PRIVATE) {
        requests.push(this.authDispatcherClient.authDispatcherRichiediSignedURL({
          elencoRichieste: tiles.map(tile => ({
            Bucket: layerParam.bucket,
            Object: layerParam.obj,
            Coords: `${tile.zoom}/${tile.x}/${tile.y}`
          }))
        } as RichiestaSignedUrl_In));
      }

      if (layerParam.type == RasterParameterVisualizationType.ABACO_PRIVATE) {
        const param = this.getColorTableParam(layerParam, sensor);
        requests.push(this.authDispatcherClient.authDispatcherRichiediAbacoURL({
          RichiestaAbacoType: RichiestaAbacoType.Satellite,
          elencoRichieste: tiles.map(tile => ({
            Bucket: layerParam.bucket,
            Object: layerParam.obj,
            Coords: `${tile.zoom}/${tile.x}/${tile.y}`
          }))
        } as RichiestaAbacoUrl_In)
          .pipe(map(res => {
            res.RispostaStringa.elencoUrlFirmati.forEach(y => y.SignedUrl = `${y.SignedUrl}&${param}`);
            return res;
          }))
        );
      }
    }

    return forkJoin(requests)
      .pipe(map(res => [res.flatMap(r => r.RispostaStringa.elencoUrlFirmati).filter(this.distinctUrls), overlay]));
  }

  private getColorTableParam(layerParam: RasterParameterVisualizationLayer, sensor: string): string {
    const obj = layerParam.obj.split('/')[0];
    return `colorTable=${layerParam.bucket}/${obj}/${sensor}.txt`;
  }

  private getSatelliteRequests(layerParam: RasterParameterVisualizationLayer, overlay: Gis_Sat_Sentinel_Overlay): Observable<[UrlFirmato, Gis_Sat_Sentinel_Overlay]> {
    const url = layerParam.baseUrl.replace(/\/$/, ''); // remove final / if present
    return of([{ Key: ``, SignedUrl: url } as UrlFirmato, overlay]);
  }

  private getRasterPublicRequests(tiles: Tile[], layerParam: RasterParameterVisualizationLayer, overlay: Gis_Sat_Sentinel_Overlay, sensor: string): Observable<[UrlFirmato[], Gis_Sat_Sentinel_Overlay]> {
    const result = tiles.map(tile => {
      const coods = `${tile.zoom}/${tile.x}/${tile.y}`;
      const url = layerParam.baseUrl.replace(/\/$/, ''); // remove final / if present
      if (layerParam.type == RasterParameterVisualizationType.PUBLIC) {
        return { Key: coods, SignedUrl: `${url}/${coods}` } as UrlFirmato;
      }

      const param = this.getColorTableParam(layerParam, sensor);
      return { Key: coods, SignedUrl: `${url}/${coods}?${param}` } as UrlFirmato;
    });

    return of([result, overlay]);
  }

  private distinctUrls(value: UrlFirmato, index: number, array: UrlFirmato[]): boolean {
    return array.findIndex(x => x.SignedUrl == value.SignedUrl) === index;
  }

  private showNoFeatureDataError(features: GeoJson_Feature_New<GeoJSONAgroGisProp>[]): void {
    if (this.dialogRef != null) {
      return;
    }

    const codes = features.map(feature => +feature.properties.Entita_Cod);
    //const timeout = interval(20000).pipe(take(1), map(() => ({ returnObj: false })));

    forkJoin(codes.map(code => this.gisClient.gisLeggiLogEsecuzioniConfigurazione(code)))
      .pipe(
        map(responses => {
          const noFeaturesCode = [];
          for (let i = 0; i < codes.length; i++) {
            if (responses[i].RispostaStringa.elencoLogEsecuzioniConfigurazione.length == 0) {
              noFeaturesCode.push(codes[i]);
            }
          }
          return noFeaturesCode
        }),
        filter(noFeaturesCode => noFeaturesCode.length > 0),
        switchMap(noFeatureCodes => {

          const title = this.translocoService.translate('gis.QuestoPoligonoConCodiceNonHaDatiSatellitari', { codes: noFeatureCodes.join(', ') });
          this.dialogRef = this.giasDialogService.dialogMessageRef('', title);
          return this.dialogRef.result;
        })
      )
      .subscribe((result: any) => {
        if (result?.returnObj) {
          this.enableSatelliteAlgorithm(codes);
        }

        this.dialogRef.close();
        this.dialogRef = null;
      });
  }

  private async loadSatelliteAlgorithm(entityCode: number): Promise<number> {
    const response = await firstValueFrom(
      this.gisClient.gisLeggiConfigurazioniProiezioneSuLayer({
        LayerElementiGrafici_Cod: 19,
        Entita_Cod: entityCode,
        TipologiaLayer_cod: 1,
      })
    );
    return (
      response.RispostaStringa?.elencoConfigurazioni?.find(
        (config) => config.LayerAnalysisConfig_Algorithm_Cod == 1
      )?.LayerAnalysisConfig_Cod ?? SDAC_DEFAULT
    );
  }

  private async enableSatelliteAlgorithm(codes: number[]) {
    const body = {
      configurazioneProiezione_Cod: await this.loadSatelliteAlgorithm(codes[0]),
      layer_cod: 0,
      isAttivo: true,
      tipologia_layer_cod: 1,
      listaEntita: GISAnalisiMappeSatellitariWindowComponent.getEntitiesFromFeatures(codes)
    } as AttivazioneConfigurazioneAlgoritmiCartografici;

    this.gisClient
      .gisAttivaDisattivaConfigurazione(body)
      .subscribe({
        next: () => this.giasDialogService.baseSuccess('', 'OperazioneRiuscita', true),
        error: () => this.giasDialogService.baseError('', 'ErroreModifica', true)
      });
  }

  private static getEntitiesFromFeatures(codes: number[]): EntitaAlgoritmoCartografico[] {
    return codes.map(code => ({ entita_cod_1: code, entita_cod_2: 0, entita_cod_risultato: 0 } as EntitaAlgoritmoCartografico));
  }
}

export function applyOverlaysOnPolygons(permessiUtenteService: PermessiUtenteService, masks: MascheraLayerRaster[]): boolean {
  // If I have active masks (without checking any permissions), view overlays on polygons
  const hasMasks = masks.filter(x => x.isAttivaPerUtenteCorrente).length > 0;
  if (hasMasks) {
    return true;
  }

  // If I don't have permissions, view overlays on polygons (if no feature is selected, no overlay is loaded)
  const hasPermission = permessiUtenteService.getPermesso(enum_Security_Attivita.GIS_Gestione_Parametri_Maschere_Raster, 0);
  if (!hasPermission) {
    return true;
  }

  // No active mask, but I have permissions to load the whole overlay
  return false;
}
