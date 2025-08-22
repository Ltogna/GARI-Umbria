/* eslint-disable */
import { Injectable } from '@angular/core';
import { Specie } from 'app/Model/metaschema/utilizzi/Specie';
import { Varieta } from 'app/Model/metaschema/utilizzi/Varieta';
import {map, Observable, take} from 'rxjs';
import { AjaxAgronicaAPIService } from '../ajax-agronica.api.service';
import { AjaxAgronicaService } from '../ajax-agronica.service';
import { MasterService } from '../master.service';
import {SementieriParametrizzazione} from '../../Model/GIS/SementieriParametrizzazione';

export class LeggiSpecie{
  parametriSementieri: SementieriParametrizzazione;
  gruppiVegetali: number[];

  constructor(cfgSementi: SementieriParametrizzazione = undefined, grVeg: number[] = []) {
    this.parametriSementieri = cfgSementi;
    this.gruppiVegetali = grVeg;
  }
}

@Injectable({
  providedIn: 'root'
})
export class SpecieVegetaliService {
  private SpecieVegetali: Specie[] = new Array();
  private ColturePrecedenti: Specie[]= new Array();

  constructor(
    private ajaxAgronicaAPIService: AjaxAgronicaAPIService
  ) { }

  leggi_FiltroUtente(sementieriParametrizzazione?: SementieriParametrizzazione) {
    return new Promise<Specie[]>(async (resolve, reject) => {
      if (this.SpecieVegetali == undefined || this.SpecieVegetali.length == 0) {
        this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiSpecie, Specie[]>(
          'Modello/Specie',
          new LeggiSpecie(sementieriParametrizzazione),
          false
        ).pipe(map(data => {
          this.SpecieVegetali = data.RispostaStringa;
          resolve(this.SpecieVegetali);
        })).subscribe();
      } else {
        resolve(this.SpecieVegetali);
      }
    });
  }

  leggi() {
    return new Promise<Specie[]>(async (resolve, reject) => {
      if(this.ColturePrecedenti== undefined || this.ColturePrecedenti.length == 0){
        this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiSpecie, Specie[]>(
          'MetaschemaNG/LeggiSpecieVegetali',
          new LeggiSpecie(),
          false).pipe(map( R => {
          this.ColturePrecedenti = R.RispostaStringa;
          resolve(this.ColturePrecedenti);
        })).subscribe();
      }else{
        resolve(this.ColturePrecedenti);
      }
    });
  }

  leggiTutteLeSpecieAPI() {
    return this.ajaxAgronicaAPIService.ajaxAPIGet<string, Array<Specie>>('Modello/Specie', "").pipe(map(r =>{
      return r.RispostaStringa;
    }));
  }

  leggi_Da_Cultivar(cultivar: Varieta) {
    return new Promise<Specie>(async (resolve, reject) => {
      this.ajaxAgronicaAPIService.ajaxAPIPost<Varieta, Specie>(
        'MetaschemaNG/Leggi_Da_Cultivar',
        cultivar,
        false).pipe(map( R => {
        resolve(R.RispostaStringa);
      })).subscribe();
    })
  }

  leggiDaGruppiVegetali(gruppi: number[]): Observable<Specie[]> {
    const params = new LeggiSpecie();
    params.gruppiVegetali = gruppi ?? [];
    return this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiSpecie, Specie[]>(
      'MetaschemaNG/LeggiSpecieVegetali', params, false
    ).pipe(take(1), map( R => {
      return R.RispostaStringa;
    }));
  }

}
