/* eslint-disable */
import {Injectable} from '@angular/core';
import {GisDataReadParam} from 'app/Model/GIS/GisDataReadParam';
import {FeatureType, GeoJson_Geometry_New, GeoJSONAgroGisProp, GisDataReadRval_New} from 'app/Model/GIS/GisDataReadRval_New';
import {AjaxAgronicaAPIService} from 'app/Service/ajax-agronica.api.service';
import {rispostaStandard} from 'app/Service/master.service';
import {Observable} from 'rxjs';
import Polygon = google.maps.Polygon;
import Marker = google.maps.Marker;
import Polyline = google.maps.Polyline;

@Injectable()
export class GeoJsonService {
  constructor(
    protected ajaxApiService: AjaxAgronicaAPIService
  ) {  }

  public readGeoJson(gisParams: GisDataReadParam): Observable<rispostaStandard<GisDataReadRval_New<GeoJSONAgroGisProp>>> {
    const parametri = gisParams;

    return this.ajaxApiService.ajaxAPIPost<GisDataReadParam, GisDataReadRval_New<GeoJSONAgroGisProp>>(
      'Gis/LeggiElencoEntitaGeoJson',
      parametri,
      false,
      true
    );
  }

  public polygonToGeoJson(polygon: Polygon): GeoJson_Geometry_New {
    let coordinates: number[][][] = [polygon.getPath().getArray().map(ll => {
      return [ll.lng(), ll.lat()];
    })];
    //Chiudo l'anello delle coordinate inserendo sul fondo copia della prima
    coordinates[0].push(coordinates[0][0]);

    return new GeoJson_Geometry_New(FeatureType.Polygon, JSON.stringify(coordinates));
  }

  public markerToGeoJson(marker: Marker): GeoJson_Geometry_New {
    let coordinates: number[] = [marker.getPosition().lng(), marker.getPosition().lat()];
    return new GeoJson_Geometry_New(FeatureType.Point, JSON.stringify(coordinates));
  }

  public polylineToGeoJson(polyline: Polyline) {
    let coordinates: number[][] = polyline.getPath().getArray().map(ll => {
      return [ll.lng(), ll.lat()];
    });

    return new GeoJson_Geometry_New(FeatureType.LineString, JSON.stringify(coordinates));
  }

}
