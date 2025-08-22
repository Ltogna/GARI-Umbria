import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { LayerService } from 'app/GIS/services/layer.service';
import { MascheraLayerRaster, TipologiaLayer } from 'app/Service/api.service';
import { filter, map, Observable, startWith, tap } from 'rxjs';
import { DEFAULT_TOP_POSITION } from 'app/GIS/GIS-toolbar/gis-toolbar.service';
import { KendoWindowsService, WindowArgs, WindowTypes } from 'app/Service';
import { enum_LayerElementiGraficiStd } from 'app/GIS/GIS-enum/GIS-layer-elementi-grafici';
import { SMARTPHONE_WIDTH } from 'app/Model/CostantiPersonalizzate';
import { AnimationStatus, GISAnalisiMappeSatellitariWindowService } from 'app/GIS/GIS-analisi-mappe-satellitari-window/GIS-analisi-mappe-satellitari-window.service';
import { GiasMessageService } from 'app/Service/gias-message.service';
import { GiasDialogService } from 'app/Service/gias-dialog.service';
import { GISRasterConfigurationWindowService } from 'app/GIS/GIS-raster-configuration-window/GIS-raster-configuration-window.service';
import { NotificationRef } from '@progress/kendo-angular-notification';
import { TranslocoService } from '@jsverse/transloco';
import { PermessiUtenteService } from 'app/Service/permessi-utente.service';
import { applyOverlaysOnPolygons } from 'app/GIS/GIS-analisi-mappe-satellitari-window/GIS-analisi-mappe-satellitari-window.component';
import { enum_Security_Attivita } from 'app/Model/TipiEnumerativi';

const TIME_DELTA_ANIMATION_SAT = 4;

@Component({
  standalone: false,
  selector: 'gis-layer-list-item-mappe-satellitari',
  templateUrl: './GIS-layer-list-item-mappe-satellitari.component.html',
  styleUrls: ['./GIS-layer-list-item-mappe-satellitari.component.css']
})
export class GISLayerListItemMappeSatellitariComponent implements OnInit, OnDestroy {
  @Input() public dataItem: TipologiaLayer = null;

  enabled$ = this.gisAnalisiMappeSatellitariWindowService.isActive$;
  masks$ = this.gisAnalisiMappeSatellitariWindowService.masks$;
  status$: Observable<AnimationStatus>;
  lastPage$: Observable<boolean>;
  animationStatus = AnimationStatus;

  private firstLoad = false;
  private rasterActiveTitleBarRef: NotificationRef | null = null;

  constructor(
    private layerService: LayerService,
    private kendoWindowsService: KendoWindowsService,
    private gisAnalisiMappeSatellitariWindowService: GISAnalisiMappeSatellitariWindowService,
    private giasMessageService: GiasMessageService,
    private giasDialogService: GiasDialogService,
    private gisRasterConfigurationWindowService: GISRasterConfigurationWindowService,
    private translocoService: TranslocoService,
    private permessiUtenteService: PermessiUtenteService
  ) { }

  get title(): string {
    const args = this.kendoWindowsService.getWindowArgs(WindowTypes.AnalisiMappeSatellitariWindow);
    if (args?.title == null) {
      return this.dataItem.nome;
    }

    return args.title;
  }

  get isCloudy(): boolean {
    const args = this.kendoWindowsService.getWindowArgs(WindowTypes.AnalisiMappeSatellitariWindow);
    return this.gisAnalisiMappeSatellitariWindowService.isActive && args?.additionalArgs != null && args.additionalArgs.isCloudy;
  }

  get themes(): boolean {
    return this.layerService.esistonoLayerConTemi;
  }

  ngOnInit(): void {
    const item = this.dataItem;

    if (item.id !== enum_LayerElementiGraficiStd.ANALISI_MAPPE_SATELLITARI) {
      throw new Error("Layer non di tipo analisi mappe satellitari")
    }

    this.prepareAnimation();

    this.status$ = this.gisAnalisiMappeSatellitariWindowService.animationStatus$.pipe(startWith(AnimationStatus.ToBeLoaded));
    this.lastPage$ = this.gisAnalisiMappeSatellitariWindowService.overlays$.pipe(
      filter(overlays => this.firstLoad && overlays.length > 0),
      map(() => true),
      tap(() => this.firstLoad = false),
      tap(() => this.gisAnalisiMappeSatellitariWindowService.goLastAnimationStep())
    );

    const args = new WindowArgs(WindowTypes.AnalisiMappeSatellitariWindow, false, item.nome, null, Math.min(window.innerWidth, 340), 340, undefined, DEFAULT_TOP_POSITION, true, true, true, false, false, false, window.innerWidth >= SMARTPHONE_WIDTH);
    this.kendoWindowsService.override(WindowTypes.AnalisiMappeSatellitariWindow, args);
  }

  ngOnDestroy(): void {
    this.gisAnalisiMappeSatellitariWindowService.nextAnimationStatus(AnimationStatus.ToBeLoaded);
    this.kendoWindowsService.close(WindowTypes.AnalisiMappeSatellitariWindow);
    this.gisAnalisiMappeSatellitariWindowService.nextIsActive(false);
    this.rasterActiveTitleBarRef?.hide();
  }

  toggleVisible($event: Event, item: TipologiaLayer, masks: MascheraLayerRaster[], visible: boolean): void {
    $event.stopPropagation();

    const rasterAlreadyOpen = this.gisRasterConfigurationWindowService.isSomeVisible();
    if (visible && rasterAlreadyOpen) {
      this.giasDialogService.baseError('', 'gis.DisattivaIlCaricamentoRasterPrimaDiAttivareAnimazioneDatiSatellitari', true);
      return;
    }

    this.layerService.toggleLayerItemVisible(item, visible);
    this.gisAnalisiMappeSatellitariWindowService.nextIsActive(visible);

    if (visible) {
      const hasMasks = masks.filter(x => x.isAttivaPerUtenteCorrente).length > 0;
      const hasPermissions = this.permessiUtenteService.getPermesso(enum_Security_Attivita.GIS_Gestione_Parametri_Maschere_Raster, 0);
      if (!hasMasks && !hasPermissions) {
        this.giasDialogService.baseError('', 'gis.NonSiHannoIPermessiPerCaricareIDatiSatellitari', true);
        return;
      }

      this.gisAnalisiMappeSatellitariWindowService.nextAnimationStatus(AnimationStatus.Loading);
      this.firstLoad = true;

      const title = this.translocoService.translate('gis.VisualizzazioneDatiSatellitariAttiva');
      const body = this.translocoService.translate('gis.ChiudiVisualizzazione');
      this.rasterActiveTitleBarRef = this.giasMessageService.customMessage(title, body, '', () => this.toggleVisible(new Event('click'), item, masks, false), false);
    } else {
      this.gisAnalisiMappeSatellitariWindowService.nextAnimationStatus(AnimationStatus.ToBeLoaded);
      this.kendoWindowsService.close(WindowTypes.AnalisiMappeSatellitariWindow);
      this.rasterActiveTitleBarRef?.hide();
    }

    this.gisAnalisiMappeSatellitariWindowService.nextExternalLoadOnPolygons(applyOverlaysOnPolygons(this.permessiUtenteService, masks));
  }

  toggleAnalisiMappeSatellitari($event: Event, item: TipologiaLayer): void {
    $event.stopPropagation();

    const args = this.kendoWindowsService.getWindowArgs(WindowTypes.AnalisiMappeSatellitariWindow);
    args.additionalArgs = { originalTitle: item.nome };
    this.kendoWindowsService.open(WindowTypes.AnalisiMappeSatellitariWindow, args, false);
  }

  mappeSatellitariStart($event: Event): void {
    $event.stopPropagation();

    // Always restart
    this.gisAnalisiMappeSatellitariWindowService.nextAnimationStep(0);
    this.gisAnalisiMappeSatellitariWindowService.nextAnimationStatus(AnimationStatus.Started);
  }

  mappeSatellitariPause($event: Event): void {
    $event.stopPropagation();

    this.gisAnalisiMappeSatellitariWindowService.nextAnimationStatus(AnimationStatus.Stopped);
  }

  mappeSatellitariNext($event: Event): void {
    $event.stopPropagation();

    this.gisAnalisiMappeSatellitariWindowService.goNextAnimationStep();
  }

  isForwardVisible(enabled: boolean, status: AnimationStatus): boolean {
    return enabled && status == AnimationStatus.Stopped || status == AnimationStatus.Loading
  }

  private prepareAnimation(): void {
    const firstOfTheYear = new Date(new Date().getFullYear(), 0, 1);
    const sixMonthsAgo = new Date();
    sixMonthsAgo.setMonth(sixMonthsAgo.getMonth() - TIME_DELTA_ANIMATION_SAT);

    this.gisAnalisiMappeSatellitariWindowService.nextDateFrom(new Date(Math.max.apply(null, [sixMonthsAgo, firstOfTheYear])));
    this.gisAnalisiMappeSatellitariWindowService.nextDateTo(new Date());
  }
}
