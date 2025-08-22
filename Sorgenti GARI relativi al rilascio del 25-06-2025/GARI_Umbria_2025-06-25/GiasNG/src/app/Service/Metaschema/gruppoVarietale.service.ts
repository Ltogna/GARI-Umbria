/* eslint-disable */
import { Injectable } from '@angular/core';
import { CoreWS_Generic } from 'app/Model/CoreWS/CoreWS_Generic';
import { GruppoVarietale } from 'app/Model/metaschema/GruppoVarietale';
import { Specie } from 'app/Model/metaschema/utilizzi/Specie';
import { map } from 'rxjs';
import { AjaxAgronicaAPIService } from '../ajax-agronica.api.service';
import { AjaxAgronicaService } from '../ajax-agronica.service';
import { MasterService } from '../master.service';
import {SementieriParametrizzazione} from '../../Model/GIS/SementieriParametrizzazione';
import {SharedDataService} from '../../GIS/services/shared-data.service';

export class GruppoVarietalexSpecie {
    Specie: Specie;
    GruppoVarietale: GruppoVarietale[];
}

export class LeggiGruppoVarietale {
    specie: Specie;
    ParametriSementieri: SementieriParametrizzazione
}

@Injectable({
    providedIn: 'root'
})
export class GruppoVarietaleService {
    private GruppoVarietalexSpecie: GruppoVarietalexSpecie[] = new Array();

    constructor(
        private ajaxAgronicaService: AjaxAgronicaService,
        private ajaxAgronicaAPIService: AjaxAgronicaAPIService,
        private masterService: MasterService,
        private sharedDataService: SharedDataService
    ) { }

    /*leggi_Old(specie: Specie){
        return new Promise<GruppoVarietale[]>(async (resolve, reject) => {
            if (this.GruppoVarietalexSpecie.find((el) => {
                if(el.Specie.codice == specie.codice) {
                    return el;
                }
            })  == undefined) {

                const parametri: CoreWS_Generic<LeggiGruppoVarietale> = new CoreWS_Generic(
                    this.masterService.getCoreWSGenericObjP(),
                    { specie: specie, ParametriSementieri: this.sharedDataService.getCfgSementiAsValue() } as LeggiGruppoVarietale);

                const R = await this.ajaxAgronicaService.ajaxAgronicaCoreWS_Promise<GruppoVarietale[], LeggiGruppoVarietale>(
                    this.masterService.link_CoreWS + '/Metaschema/GruppoVarietale.asmx/Leggi',
                    parametri, false);

                this.GruppoVarietalexSpecie.push({ Specie: specie, GruppoVarietale: R.RispostaStringa });

                resolve(this.GruppoVarietalexSpecie.find((el) => {
                    if (el.Specie.codice == specie.codice) {
                        return el;
                    }
                }).GruppoVarietale);

            } else {

                resolve(this.GruppoVarietalexSpecie.find((el) => {
                    if (el.Specie.codice == specie.codice) {
                        return el;
                    }
                }).GruppoVarietale);

            }
        });
    }*/

    leggi(specie: Specie){
        return new Promise<GruppoVarietale[]>(async (resolve, reject) => {
            if (this.GruppoVarietalexSpecie.find((el) => {
                if(el.Specie.codice == specie.codice) {
                    return el;
                }
            })  == undefined) {

                this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiGruppoVarietale, GruppoVarietale[]>(
                    'MetaschemaNG/LeggiGruppoVarietale',
                    {specie: specie, ParametriSementieri: null}, false).pipe(map(R => {
                        
                        this.GruppoVarietalexSpecie.push({ Specie: specie, GruppoVarietale: R.RispostaStringa });
        
                        resolve(this.GruppoVarietalexSpecie.find((el) => {
                            if (el.Specie.codice == specie.codice) {
                                return el;
                            }
                        }).GruppoVarietale);

                    })).subscribe();


            } else {

                resolve(this.GruppoVarietalexSpecie.find((el) => {
                    if (el.Specie.codice == specie.codice) {
                        return el;
                    }
                }).GruppoVarietale);

            }
        });
    }


}
