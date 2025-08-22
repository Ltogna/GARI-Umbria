import {GeoJson_Feature_New, GeoJSONAgroGisProp} from '../../Model/GIS/GisDataReadRval_New';
import {enum_FeatureGeometryType} from '../GIS-enum/GIS-feature';

export class GisFeaturesUtils {


  public static isGeoJsonFeatureMultipolygon(feature: GeoJson_Feature_New<GeoJSONAgroGisProp>): boolean {
    return (feature.geometry.type === 'MultiPolygon');
  }

  public static isGoogleMapsFeatureMultipolygon(feature: google.maps.Data.Feature): boolean {
    return (feature.getGeometry().getType() === 'MultiPolygon');
  }


  public static isGeoJsonFeaturePolygonWithInnerBounds(feature: GeoJson_Feature_New<GeoJSONAgroGisProp>): boolean {
    return (feature.geometry.type === 'Polygon' && ((feature.geometry.coordinates as object[]).length > 1));
  }

  public static isGoogleMapsFeaturePolygonWithInnerBounds(feature: google.maps.Data.Feature): boolean {
    const g: google.maps.Data.Geometry = feature.getGeometry();
    return (g.getType() === enum_FeatureGeometryType.Polygon && (g as google.maps.Data.Polygon).getArray().length > 1);
  }

}
