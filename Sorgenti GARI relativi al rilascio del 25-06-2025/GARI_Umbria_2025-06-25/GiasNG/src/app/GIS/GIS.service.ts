/* eslint-disable */
import {Injectable} from '@angular/core';
import {SharedDataService} from './services/shared-data.service';
import {GoogleMapGeoJsonService} from 'app/GIS/google-map/google-map-geojson.service';
import {LayerService} from './services/layer.service';
import {FeatureService} from './services/feature.service';
import {objTreeNode, TreeGisService} from 'app/Utility/Template/kendo-tree/services/tree-gis.service';
import {GisClient, Obj_SalvaGrafica, SalvaNuovoElementoGraficoDaChiaveAlberoConAppezza_In, TipologiaLayer} from 'app/Service/api.service';
import {TranslocoService} from '@jsverse/transloco';
import {GiasDialogService} from 'app/Service/gias-dialog.service';
import {Subject} from 'rxjs';
import {WKTService} from './services/wkt.service';
import {GISModality} from './GIS-enum/GIS-feature';
import {enum_TipoNodo, enum_TipoOperazioneDB} from 'app/Model/TipiEnumerativi';
import {enum_LayerElementiGraficiStd} from './GIS-enum/GIS-layer-elementi-grafici';
import {WmsService} from './services/wms.service';
import {enum_TipologiaLayer} from './GIS-enum/GIS-tipologia-layer';
import {GeoJsonFilterService} from './services/geojson-filter.service';
import {GisDataReadParam} from 'app/Model/GIS/GisDataReadParam';
import {FiltroTemporale} from 'app/Model/GIS/FiltroTemporale';
import {UtilizzoTerreno} from 'app/Model/metaschema/utilizzi/UtilizzoTerreno';
import {Varieta} from 'app/Model/metaschema/utilizzi/Varieta';
import {GeoJsonFilterServiceParam} from 'app/Model/GIS/GeoJsonFilterServiceParam';
import {enum_OrigineChiamataFilterService, enum_OrigineChiamataLoadGeoJson} from './GIS-enum/GIS-origine-chiamata';
import {ObjParametriAgendaService} from 'app/Service/obj-parametri-agenda.service';
import {FunzioniComuniService} from 'app/Service/FunzioniComuni.service';
import {getServiceIdAndLog} from 'app/Service/utils';
import {GisToolbarService} from './GIS-toolbar/gis-toolbar.service';
import {WindowTypes} from 'app/Service';
import {CentraMappa} from 'app/Model/GIS/Utility';
import {GeoJson_Feature_New, GeoJSONAgroGisProp} from 'app/Model/GIS/GisDataReadRval_New';

@Injectable()
export class GisService {

  private signal = new Subject<void>();

  private tipoLayerSelezionatoPrecedente = "";

  private serviceId = null;

  constructor(
    private sharedDataService: SharedDataService,
    private layerService: LayerService,
    private googleMapGeoJsonService: GoogleMapGeoJsonService,
    private gisClient: GisClient,
    private treeGisService: TreeGisService,
    private featureService: FeatureService,
    private translocoService: TranslocoService,
    private giasDialogService: GiasDialogService,
    private wktService: WKTService,
    // private giasMessageService: GiasMessageService,
    private wmsService: WmsService,
    private geoJsonFilterService: GeoJsonFilterService,
    private objParametriAgendaService: ObjParametriAgendaService,
    private funzioniComuniService: FunzioniComuniService,
    private gisToolbarService: GisToolbarService
  ) {
    this.serviceId = getServiceIdAndLog('GisService','constructor');
  }

  ngOnDestroy() {
    this.signal.next();
    this.signal.complete();
  }

  public InizializzaParametriGis() {
    this.sharedDataService.setCfgGisGenerali({});
    this.sharedDataService.setCfgAlberoGisUtente({},false);
    this.sharedDataService.setTipologiaLayer([]);
  }

  public initFullMapTrattamento(): void {
    this.googleMapGeoJsonService.modality = GISModality.Trattamento;
    const tipoLayerSelezionato = enum_TipologiaLayer.Entita;
    this.layerService.refreshLayers({ Option_Value: tipoLayerSelezionato }, undefined, true);

    //--------------------------------------------------------------------------------
    // Il codice a seguire è stato commentato in quanto ridondante.
    // Il caricamento dei poligoni da QdC avviene già tramite la funzione
    // updateGeometrySelection() di testata.component.ts
    //--------------------------------------------------------------------------------

    // let filterParam = this.geoJsonFilterService.getGeoJsonFilterServiceParam();
    // const qdcConPoligoni = this.geoJsonFilterService.getQdcConPoligoni();
    // if (filterParam.FlagLoadGeoJson && qdcConPoligoni) {
    //     this.sharedDataService.setOrigineChiamataLoadGeoJson(enum_OrigineChiamataLoadGeoJson.InizializzazioneMappaQdc);
    //     this.googleMapGeoJsonService.loadGeoJsonFilterService(filterParam);
    // }
  }

  public initFullMapAnalisiTerreno(): void {
    this.googleMapGeoJsonService.modality = GISModality.AnalisiTerreno;
    this.layerService.refreshLayers({ Option_Value: enum_TipologiaLayer.Entita }, undefined, false);
  }

  public gestisciModificaListaLayer(
    layers: TipologiaLayer[],
    modality: GISModality
  ) {

    //--------------------------------------------------------------------------------
    // Funzione eseguita ogni volta che viene aggiornata una lista di layer:
    // lettura iniziale, refresh salvataggio, cambio ordinamento, modifica
    // visibilità da impostazioni.
    //--------------------------------------------------------------------------------

    // Se lettura iniziale, devo centrare la mappa

    const elencoLayers = this.sharedDataService.getTipologiaLayer();
    let impostaCentroMappa = false
    if (elencoLayers.length === 0) {
      impostaCentroMappa = true;
    }

    this.sharedDataService.setTipologiaLayer(layers);
    let tipoLayerSelezionato = this.sharedDataService.getTipoLayerSelezionato();

    switch (modality) {
      case GISModality.Full:

        if (tipoLayerSelezionato !== this.tipoLayerSelezionatoPrecedente) {
          this.tipoLayerSelezionatoPrecedente = this.sharedDataService.getTipoLayerSelezionato();

          // Chiudo finestra tematizzazioni se aperta
          if (this.gisToolbarService.getToggleState(WindowTypes.ThemeWindow)) {
            this.gisToolbarService.themeBtnToggle(false);
          }

          this.wmsService.wmsDdlSensoreElaborazioneChange(false);
          this.sharedDataService.setOrigineChiamataLoadGeoJson(enum_OrigineChiamataLoadGeoJson.CambioTipologiaLayer);
          this.googleMapGeoJsonService.loadGeoJsonForzato(impostaCentroMappa);

        } else if (tipoLayerSelezionato === enum_TipologiaLayer.Entita) {
          // Se sono stati aggiunti nuovi layer, deve rendere visibili le rispettive feature
          if (this.sonoVisibiliNuoviLayer(layers,elencoLayers)) {
            this.googleMapGeoJsonService.showHideAllFeature(true,layers);
          }
        }
        break;
    }
  }

  private sonoVisibiliNuoviLayer(
    newElencoLayers: TipologiaLayer[],
    oldElencoLayers: TipologiaLayer[]
  ): boolean {

    let nuoviLayer = false;

    if (newElencoLayers.length > oldElencoLayers.length) {

      nuoviLayer = true;

    } else {

      for (let i = 0; i < newElencoLayers.length; i++){
        let layerIndex = oldElencoLayers.findIndex(layer => layer.id === newElencoLayers[i].id);
        if (layerIndex === -1) {
          nuoviLayer = true;
          break;
        }
      }

    }

    return nuoviLayer;

  }

  //====================================================================================================
  // FUNZIONI INTERAZIONE VISUALIZZAZIONE TOTALE
  //====================================================================================================

  impostaZoomMinimoVisualizzazioneTotale() {
    this.googleMapGeoJsonService.impostaZoomMinimoVisualizzazioneTotale();
  }

  updateGeoJsonFilterServiceVisualizzazioneTotale() {
    let filtro = new GisDataReadParam();
    // Data
    const filtroTemporaleAvanzato = this.sharedDataService.getFiltroTemporaleAvanzato();
    filtro.filtroTemporale = filtroTemporaleAvanzato.FiltroPeriodo;
    filtro.filtroTemporaleSingolaData = filtroTemporaleAvanzato.FiltroSingolaData;
    // Piva
    filtro.piva = '';
    // Centro
    filtro.sa_cod = '0';
    // Campo
    filtro.campo_cod = '0';
    // Tipologia layer selezionata
    filtro.TipologiaLayerSelezionata = this.sharedDataService.getTipoLayerSelezionato();
    // Limiti mappa
    filtro.wktBoundaySTIntersects = this.googleMapGeoJsonService.getPolygonWktFromBoundsMappa();

    this.geoJsonFilterService.setGisDataReadParam(filtro);

    let filterServiceParam = new GeoJsonFilterServiceParam(
      enum_OrigineChiamataFilterService.Albero,
      false,
      true
    );
    this.sharedDataService.setOrigineChiamataLoadGeoJson(enum_OrigineChiamataLoadGeoJson.VisualizzazioneTotale);
    this.geoJsonFilterService.setGeoJsonFilterServiceParam(filterServiceParam);
  }

  //====================================================================================================
  // FUNZIONI INTERAZIONE QDC
  //====================================================================================================

  seUpdateGeoJsonFilterServiceQdc(date: Date, sa_cod: string, campo_cod: string, veg_cod: UtilizzoTerreno) {
    if (veg_cod && this.filtroUtilizzoTerrenoImpostato(veg_cod)) {
      this.updateGeoJsonFilterServiceQdc(date, sa_cod, campo_cod, veg_cod);
    }
  }

  filtroUtilizzoTerrenoImpostato(utilizzoTerreno: UtilizzoTerreno): boolean {
    let filtroImpostato = false;
    if (utilizzoTerreno.classType === 'Varieta') {
      if ((<Varieta>utilizzoTerreno).specie.codice > 0){
        filtroImpostato = true;
      }
    } else {
      if (utilizzoTerreno.codice > 0) {
        filtroImpostato = true;
      }
    }
    return filtroImpostato;
  }

  updateGeoJsonFilterServiceQdc(date: Date, sa_cod: string, campo_cod: string, veg_cod: UtilizzoTerreno, layer: enum_LayerElementiGraficiStd = enum_LayerElementiGraficiStd.IMPIANTI) {
    let filtro = new GisDataReadParam();
    // Data
    filtro.filtroTemporale = new FiltroTemporale();
    filtro.filtroTemporale.setEserciziValidiAllaData(date);
    filtro.filtroTemporaleSingolaData = new FiltroTemporale();
    filtro.filtroTemporaleSingolaData.setIntervalloTemporaleSingolaData(date);
    // Impresa
    filtro.piva = this.objParametriAgendaService.getObjParamValue().Piva;
    // Centro
    if (!sa_cod || sa_cod === '') {
      sa_cod = '0';
    }
    filtro.sa_cod = sa_cod;
    // Campo
    if (!campo_cod || campo_cod === '') {
      campo_cod = '0';
    }
    filtro.campo_cod = campo_cod ?? '0';
    // Utilizzo terreno
    filtro.veg_cod = veg_cod
    // Layer elementi grafici
    filtro.layerElementiGrafici_cod.push(parseInt(layer));
    // Tipologia layer selezionata
    filtro.TipologiaLayerSelezionata = enum_TipologiaLayer.Entita;
    // Set Observable per ricaricamento feature
    this.geoJsonFilterService.setGisDataReadParam(filtro);
    let filterServiceParam = new GeoJsonFilterServiceParam(
      enum_OrigineChiamataFilterService.QuadernoDiCampagna,
      true,
      true
    );
    this.geoJsonFilterService.setGeoJsonFilterServiceParam(filterServiceParam);
  }

  updateGeoJsonFilterServiceAnalisiTerreno(startDate: Date, endDate: Date, sa_cod: string, campo_cod: string, veg_cod: UtilizzoTerreno, layer: enum_LayerElementiGraficiStd = enum_LayerElementiGraficiStd.IMPIANTI) {
    let filtro = new GisDataReadParam();
    // Data
    filtro.filtroTemporale = new FiltroTemporale();
    filtro.filtroTemporale.setEserciziValidiAllaData(startDate, endDate);
    filtro.filtroTemporaleSingolaData = new FiltroTemporale();
    filtro.filtroTemporaleSingolaData.setIntervalloTemporaleSingolaData(startDate, endDate);
    // Impresa
    filtro.piva = this.objParametriAgendaService.getObjParamValue().Piva;
    // Centro
    if (!sa_cod || sa_cod === '') {
      sa_cod = '0';
    }
    filtro.sa_cod = sa_cod;
    // Campo
    if (!campo_cod || campo_cod === '') {
      campo_cod = '0';
    }
    filtro.campo_cod = campo_cod ?? '0';
    // Utilizzo terreno
    filtro.veg_cod = veg_cod
    // Layer elementi grafici
    filtro.layerElementiGrafici_cod.push(parseInt(layer));
    // Tipologia layer selezionata
    filtro.TipologiaLayerSelezionata = enum_TipologiaLayer.Entita;
    // Set Observable per ricaricamento feature
    this.geoJsonFilterService.setGisDataReadParam(filtro);
    let filterServiceParam = new GeoJsonFilterServiceParam(
      enum_OrigineChiamataFilterService.QuadernoDiCampagna,
      true,
      true
    );
    this.geoJsonFilterService.setGeoJsonFilterServiceParam(filterServiceParam);
  }

  public setFilterServiceQdcConPoligoni(qdcConPoligoni: boolean) {
    this.geoJsonFilterService.setQdcConPoligoni(qdcConPoligoni);
  }

  public setFilterServiceCentraMappa(objCentraMappa: CentraMappa) {
    this.geoJsonFilterService.setCentraMappa(objCentraMappa);
  }

  //====================================================================================================
  // FUNZIONI COPIA E INCOLLA FEATURE
  //====================================================================================================

  public copiaFeature() {
    let featureSelezionate = this.featureService.getFeatureSelezionate();

    if (featureSelezionate.length === 0) {
      this.giasDialogService.baseInfo('Copia feature', 'NessunElementoSelezionato');
      return;
    }

    if (featureSelezionate.length > 1) {
      this.giasDialogService.baseInfo('Copia feature', 'SelezionatoPiuDiUnElemento');
      return;
    }

    const featureSelezionata = this.featureService.getUltimaFeatureSelezionata();

    this.featureService.setFeatureDaCopiare(featureSelezionata);
    this.giasDialogService.alertMessage(this.translocoService.translate('SelezionaAlberoPerIncollare'));
  }

  public incollaFeature() {

    const featureDaCopiare = this.featureService.getFeatureDaCopiare();

    if (!featureDaCopiare) {
      this.giasDialogService.alertMessage(this.translocoService.translate('NessunElementoDaCopiare'));
      return;
    }

    const treeViewCheckedKeys =  this.sharedDataService.getTreeViewCheckedKeys();

    if (treeViewCheckedKeys.length === 0) {
      this.giasDialogService.alertMessage(this.translocoService.translate('NessunElementoSelezionatoAlbero'));
      return;
    }

    if (treeViewCheckedKeys.length > 1) {
      this.giasDialogService.alertMessage(this.translocoService.translate('SelezionatoPiuDiUnElementoAlbero'));
      return;
    }

    const lastCheckedKeyFeatureId = this.sharedDataService.getLastCheckedKeyFeatureId();
    if (lastCheckedKeyFeatureId !== null && lastCheckedKeyFeatureId !== "") {
      this.giasDialogService.alertMessage(this.translocoService.translate('EsisteGiaUnElementoGrafico'));
      return;
    }

    const objTreeNodeSelezionato = this.treeGisService.getTreeNodeAndParentFromCheckedKey(treeViewCheckedKeys[0]);

    if (!this.isAppezzamentoImpianto(objTreeNodeSelezionato.treeNode.id)) {
      this.giasDialogService.alertMessage(this.translocoService.translate('ElementoSelezionatoNonAppezzamentoImpianto'));
      return;
    }

    this.SalvaNuovoElementoGraficoDaChiaveAlberoConAppezza(featureDaCopiare, objTreeNodeSelezionato);

  }

  private isAppezzamentoImpianto(chiaveAlbero: string): boolean {
    let isAppezzamentoImpianto = false;
    let objChiaveAlbero = FunzioniComuniService.scomponiChiaveAlbero(chiaveAlbero);
    if (objChiaveAlbero.TipoNodo === enum_TipoNodo.Appezzamento ||
      this.funzioniComuniService.isTipoNodoImpianto(objChiaveAlbero.TipoNodo)) {
      isAppezzamentoImpianto = true;
    }
    return isAppezzamentoImpianto;
  }

  private SalvaNuovoElementoGraficoDaChiaveAlberoConAppezza(
    featureDaCopiare: GeoJson_Feature_New<GeoJSONAgroGisProp>,
    objTreeNodeSelezionato: objTreeNode
  ) {

    // TODO Andrea (3): gestire il passaggio diretto delle coordinate WKT
    // const featureGeometryWkt = this.wktService.geometryToWKT(featureDaCopiare);

    const featureGeometryHiddenPunti = this.wktService.geometryToHiddenPunti(featureDaCopiare);

    let flagGps = featureDaCopiare.properties.flag_gps;

    if (flagGps === undefined || flagGps === null) {
      flagGps = "0";
    }

    const payload: SalvaNuovoElementoGraficoDaChiaveAlberoConAppezza_In = {
      ChiaveAlbero: objTreeNodeSelezionato.treeNode.id,
      HiddenPuntiNuovo: featureGeometryHiddenPunti,
      Area: "",
      FlagGps: flagGps,
      TipoOperazioneDB: enum_TipoOperazioneDB.Scrittura, // Non utilizzato
      EntitaCod: 0
    };

    this.gisClient
      .gisSalvaNuovoElementoGraficoDaChiaveAlberoConAppezza(payload)
      .subscribe({
        next: okData => this.gestioneIncollaCorretto(
          okData.RispostaStringa,
          featureDaCopiare,
          objTreeNodeSelezionato
        ),
        error: errorData => this.gestioneIncollaErrato(errorData),
        complete: () => console.log('gisSalvaNuovoElementoGraficoDaChiaveAlberoConAppezza complete')
      });
  }

  gestioneIncollaCorretto(
    objSalvaGrafica: Obj_SalvaGrafica,
    featureDaCopiare: GeoJson_Feature_New<GeoJSONAgroGisProp>,
    objTreeNodeSelezionato: objTreeNode
  ) {
    let elementiGraficiSalvati = objSalvaGrafica.Lista_ElementiGrafici.length;
    if (elementiGraficiSalvati === 0) {
      // const msgErrore = this.translocoService.translate("gis.ErroreIncollaFeatureRiferimentoEntitaMancante");
      // this.giasMessageService.errorMessage(msgErrore);
      this.giasDialogService.baseError('Incolla feature', 'gis.ErroreIncollaFeatureRiferimentoEntitaMancante');
      return;
    }
    if (elementiGraficiSalvati > 1) {
      const msgWarning = this.translocoService.translate("InseritiPiuElementiGrafici");
      console.warn(msgWarning, elementiGraficiSalvati);
    }
    const primaChiaveSalvata = objSalvaGrafica.Lista_ElementiGrafici[0];
    this.googleMapGeoJsonService.inserisciFeatureIncollata(
      featureDaCopiare,
      objTreeNodeSelezionato,
      primaChiaveSalvata.Piva,
      primaChiaveSalvata.Entita_Cod
    );
    // const messaggio = this.translocoService.translate("gis.FeatureIncollataCorrettamente");
    // this.giasMessageService.successMessage(messaggio);
    this.giasDialogService.baseSuccess('Incolla feature', 'gis.FeatureIncollataCorrettamente');
    this.featureService.inizializzaFeatureDaCopiare();
  }

  gestioneIncollaErrato(errorData: any) {
    const messaggioStandard = this.translocoService.translate("gis.ErroreIncollaFeature");
    this.gestioneErroreGisClient(errorData,messaggioStandard);
  }

  gestioneErroreGisClient(errorData: any, messaggioStandard: string) {

    let messaggioServer = null;

    if (errorData !== undefined) {

      let rispostaConErrori = errorData;

      if (rispostaConErrori.ErroriGias !== undefined && rispostaConErrori.Errore !== undefined) {

        if (rispostaConErrori.ErroriGias.length > 0) {

          // Ricezione messaggio server
          messaggioServer = rispostaConErrori.ErroriGias[0].messaggio;

        } else {

          // Log Errore Risposta Standard
          if (rispostaConErrori.Errore) {
            console.warn(rispostaConErrori.Errore);
          }

        }

      } else {

        // Log Errore Generico
        console.warn(errorData);

      }

    }

    if (messaggioServer !== null && messaggioServer !== "") {
      this.giasDialogService.alertMessage(messaggioServer);
    } else {
      //this.giasMessageService.errorMessage(messaggioStandard);
      this.giasDialogService.baseError('Elimina feature', messaggioStandard, false);
    }
  }

}
