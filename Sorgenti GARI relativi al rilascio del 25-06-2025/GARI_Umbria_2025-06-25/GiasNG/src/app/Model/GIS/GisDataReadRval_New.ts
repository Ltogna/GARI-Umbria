export class GisDataReadRval_New<T> {
  public myGeoJson: GeoJson_New<T>;
}

export class GeoJson_Geometry_New {
  _type: FeatureType;
  public coordinates: object;

  //get & set
  get type(): string {
    return FeatureType[this._type];
  }

  set type(value: string) {
    this._type = (<any>FeatureType)[value];
  }

  //costruttori
  constructor(type: FeatureType, data: string) {
    this._type = type;
    switch (type) {
      case FeatureType.Point: {
        let vettoreCoordinate: number[] = JSON.parse(data);
        this.coordinates = vettoreCoordinate;
        break;
      }
      case FeatureType.MultiPoint:
      case FeatureType.LineString: {
        let matriceCoordinate: number[][] = JSON.parse(data);
        this.coordinates = matriceCoordinate;
        break;
      }
      case FeatureType.MultiLineString:
      case FeatureType.Polygon: {
        let matriceCoordinate3D: number[][][] = JSON.parse(data);
        this.coordinates = matriceCoordinate3D;
        break;
      }
      case FeatureType.MultiPolygon: {
        let matriceCoordinate4D: number[][][][] = JSON.parse(data);
        this.coordinates = matriceCoordinate4D;
        break;
      }
    }
  }
}

export enum FeatureType {
  Point = 1,
  MultiPoint = 2,
  LineString = 3,
  MultiLineString = 4,
  Polygon = 5,
  MultiPolygon = 6,
  Raster = 100,
}

export class FeatureTypeUtil {
  public static fromString(type: string): FeatureType {
    switch (type) {
      case 'Point':
        return FeatureType.Point;
      case 'MultiPoint':
        return FeatureType.MultiPoint;
      case 'LineString':
        return FeatureType.LineString;
      case 'MultiLineString':
        return FeatureType.MultiLineString;
      case 'Polygon':
        return FeatureType.Polygon;
      case 'MultiPolygon':
        return FeatureType.MultiPolygon;
      case 'Raster':
        return FeatureType.Raster;
    }
    return null;
  }
}

export class GeoJson_New<T> {
  geoJsonCaricato: GeoJson_Shape_New<T>;
}

export class GeoJson_Shape_New<T> {
  type: string;
  features: GeoJson_Feature_New<T>[];
}

export class GeoJson_Feature_New<T> {
  type: string;
  geometry: GeoJson_Geometry_New;
  properties: T;
}

export class GeoJSONAgroGisProp {
  public layer: string;
  public StandardEntita_layerDiAppartenenza: number;
  public StandardEntita_layerDiAppartenenza_Des: string;
  public StandardEntita_layerDiAppartenenza_Icona32: string;
  public id: string;
  public tipoicona: string;
  public zindex: string;
  public Entita_Cod: string;
  public ParametriVisualizzazioneLayer: string;
  public veg_cod: string;
  public inserimento: string;
  public flag_gps: string;
  public etichetta: string;
  public modifica: string;
  public cancellazione: string;
  public informazioni: string;
  /** Vedi la funzione {@link FunzioniComuniService.scomponiChiaveAlbero} per avere un'idea di come sia composta. */
  public chiavealbero: string;
  public Testo: string;
  public AppIdRate: string;
  public TipologiaGML: string;
  public InOsservazione: number;
  public Colore_Primario: string;
  public Colore_Retinatura: string;
  public Trasparenza: number;
  public Area_Cod: number;
  public Entita_GUID: string;
  public GMapsZoomLevel: number;
  public TotalOriginalArea: number;
  public TotalOriginalFeatureNumber: number;
}

export class GeoJSONAgroGisPropTreeNode {
  public id: string;
  public imageUrl: string;
  public style: string;
  public text: string;
  public type: string;
  public startDate: Date;
  public endDate: Date
}
