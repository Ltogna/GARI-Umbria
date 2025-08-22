/* eslint-disable */
import { Injectable } from '@angular/core';
import { CoreWS_Generic } from 'app/Model/CoreWS/CoreWS_Generic';
import { Irrigazione } from 'app/Model/metaschema/Irrigazione';
import { Specie } from 'app/Model/metaschema/utilizzi/Specie';
import { map } from 'rxjs';
import { AjaxAgronicaAPIService } from '../ajax-agronica.api.service';
import { AjaxAgronicaService } from '../ajax-agronica.service';
import { MasterService } from '../master.service';

export class IrrigazionexSpecie {
    Specie: Specie;
    Irrigazione: Irrigazione[];
}

export class LeggiIrrigazione{
    specie: Specie;
}

@Injectable({
    providedIn: 'root'
})
export class IrrigazioneService {
    private IrrigazionexSpecie: IrrigazionexSpecie[] = new Array();

    constructor(private ajaxAgronicaService: AjaxAgronicaService,
        private ajaxAgronicaAPIService: AjaxAgronicaAPIService,
        private masterService: MasterService) { }

    /*leggi_Old(specie: Specie): Promise<Irrigazione[]> {
        return new Promise<Irrigazione[]>(async (resolve, reject) => {
            if (specie.codice == 0) {
                resolve(new Array<Irrigazione>());
            }
            if (this.IrrigazionexSpecie.find((el) => {
                if (el.Specie.codice == specie.codice) {
                    return el;
                }
            }) == undefined) {

                const parametri: CoreWS_Generic<LeggiIrrigazione> = new CoreWS_Generic(
                    this.masterService.getCoreWSGenericObjP(),
                    { specie: specie }
                );

                const R = await this.ajaxAgronicaService.ajaxAgronicaCoreWS_Promise<Irrigazione[], LeggiIrrigazione>(
                    this.masterService.link_CoreWS + '/Metaschema/ImpiantiIrrigazioni.asmx/CaricaImpiantiIrrigazioni_Modello',
                    parametri,
                    false);

                this.IrrigazionexSpecie.push({ Specie: specie, Irrigazione: R.RispostaStringa });
                resolve(this.IrrigazionexSpecie.find((el) => {
                    if (el.Specie.codice == specie.codice) {
                        return el;
                    }
                }).Irrigazione);

            } else {

                resolve(this.IrrigazionexSpecie.find((el) => {
                    if (el.Specie.codice == specie.codice) {
                        return el;
                    }
                }).Irrigazione);

            }
        });

    }*/

    leggi(specie: Specie): Promise<Irrigazione[]> {
        return new Promise<Irrigazione[]>(async (resolve, reject) => {
            if (specie.codice == 0) {
                resolve(new Array<Irrigazione>());
            }
            if (this.IrrigazionexSpecie.find((el) => {
                if (el.Specie.codice == specie.codice) {
                    return el;
                }
            }) == undefined) {

                this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiIrrigazione, Irrigazione[]>(
                    'MetaschemaNG/CaricaImpiantiIrrigazioniModello',
                    { specie: specie },
                    false).pipe(map(R => {
                        this.IrrigazionexSpecie.push({ Specie: specie, Irrigazione: R.RispostaStringa });
                        resolve(this.IrrigazionexSpecie.find((el) => {
                            if (el.Specie.codice == specie.codice) {
                                return el;
                            }
                        }).Irrigazione);
                    })).subscribe();

            } else {

                resolve(this.IrrigazionexSpecie.find((el) => {
                    if (el.Specie.codice == specie.codice) {
                        return el;
                    }
                }).Irrigazione);

            }
        });

    }

}
