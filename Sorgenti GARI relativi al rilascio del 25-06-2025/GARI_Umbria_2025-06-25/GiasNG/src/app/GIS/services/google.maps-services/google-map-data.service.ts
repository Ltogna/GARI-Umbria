import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { GoogleMapService } from '../../google-map/google-map.service';
import { GoogleMapFeatureService } from './google-map-feature.service';
import { GeoJSONAgroGisProp, GeoJson_Feature_New } from 'app/Model/GIS/GisDataReadRval_New';

@Injectable()
export class GoogleMapDataService {
    public overrideFeatureStyleEvent = new Subject<{ f: google.maps.Data.Feature, style: google.maps.Data.StyleOptions }>();

    get googleMapWrapper() {
        return this.googleMapService.googleMapWrapper;
    }

    constructor(
        private googleMapService: GoogleMapService,
        private googleMapFeatureService: GoogleMapFeatureService) { }

    public overrideFeatureStyle(f: GeoJson_Feature_New<GeoJSONAgroGisProp>, style: google.maps.Data.StyleOptions): void {
        const feature = this.googleMapFeatureService.getByid(f.properties.id);
        if (feature != null) {
            this.googleMapWrapper.data.overrideStyle(feature, style);
            this.overrideFeatureStyleEvent.next({ f: feature, style: style });
        }
    }
}
