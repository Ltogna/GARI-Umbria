/* eslint-disable */
import { Injectable } from '@angular/core';
import { TranslocoService } from '@jsverse/transloco';
import { Campo } from 'app/Model/anagrafiche/Campo';
import { CentroAziendale } from 'app/Model/anagrafiche/CentroAziendale';
import { Lavorazione } from 'app/Model/attivita/Lavorazione';
import { Disciplinare } from 'app/Model/metaschema/Disciplinari';
import { DestinazioneUso } from 'app/Model/metaschema/utilizzi/DestinazioneUso';
import { Specie } from 'app/Model/metaschema/utilizzi/Specie';
import { UtilizzoTerreno } from 'app/Model/metaschema/utilizzi/UtilizzoTerreno';
import { AjaxAgronicaService } from '../ajax-agronica.service';
import {MasterService, rispostaStandard} from '../master.service';
import {Impresa} from "../../Model/anagrafiche/Impresa";
import { AjaxAgronicaAPIService } from '../ajax-agronica.api.service';
import {catchError, lastValueFrom, map, Observable, of} from 'rxjs';
import {DatiPrevisionaliColture, DatiPrevisionaliColtureRequest} from '../../Model/anagrafiche/DatiPrevisionaliColture';
import { AnagraficaClient, CentroAziendale as CentroAziendaleQdCA, Impresa as ImpresaQdCA, LeggiSpecieQdC } from '../api.service'; 

export class LeggiImpianto{
  disciplinare: Disciplinare;
  direttiva_nitrati: Disciplinare;
  centroAziendale: CentroAziendale;
  lavorazione: Lavorazione;
  lavorazioni: Lavorazione[];
  campo: Campo;
  data: Date;
  utilizzoTerreno: UtilizzoTerreno;
  consideraTerrenoNudo: boolean;
  dettagliTerrenoNudo: boolean;
  id_agenda_list: number[];
  ricetta_operazione_cod_list: number[];
  tipo_ricetta: number;
  tipo_attivita: number;
  stato: number;
  tipo_operazione_db: number;
  veg_cod: number;
  dest_cod: number;
  impresa: Impresa;
  filtra_validita_esercizi?: boolean;
}

@Injectable({
  providedIn: 'root'
})

export class ImpiantiService{

  constructor(
    private masterService: MasterService,
    private ajaxAgronicaService: AjaxAgronicaService,
    private ajaxAgronicaAPIService: AjaxAgronicaAPIService,
    private translocoService: TranslocoService,
    private anagraficaClient: AnagraficaClient
  ) {  }

  /*CaricaImpianti(p: LeggiImpianto) {

      return new Promise<string>(async (resolve, reject) => {

          const parametri: CoreWS_Generic<LeggiImpianto> = new CoreWS_Generic
          (
              this.masterService.getCoreWSGenericObjP(),
              p
          );

          const R = await this.ajaxAgronicaService.ajaxAgronicaCoreWS_Promise<string, LeggiImpianto>(this.masterService.link_CoreWS + '/Anagrafica/Reg_Impianto.asmx/CaricaGridImpianti', parametri, false);

          resolve(R.RispostaStringa);
      });
  }*/

  CaricaImpianti(p: LeggiImpianto) {
    return new Promise<string>(async (resolve, reject) => {

      this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiImpianto, string>('AnagraficaNG/CaricaGridImpianti', p, false).pipe(map(R => {
        resolve(R.RispostaStringa);
      })).subscribe();

    });
  }

  /*Leggi_SpecieVegetali_Attive_Impianti_Old(p: LeggiImpianto,flagPrimaRiga: boolean, descrizioneRigaVuota: string){
      return new Promise<Specie[] | DestinazioneUso[]>(async (resolve, reject) => {

          const List =[];

          const parametri: CoreWS_Generic<LeggiImpianto> = new CoreWS_Generic
          (
              this.masterService.getCoreWSGenericObjP(),
              p
          );

          const R = await this.ajaxAgronicaService.ajaxAgronicaCoreWS_Promise<UtilizzoTerreno[], LeggiImpianto>(this.masterService.link_CoreWS + '/Anagrafica/Reg_Impianto.asmx/Leggi_SpecieVegetali_Attive_Impianti', parametri);

          const risp=<UtilizzoTerreno[]>R.RispostaStringa;

          risp.forEach((u: any)=>{
              if(u.classType === 'Varieta'){

                  List.push(u.specie);

              } else if(u.classType === 'DestinazioneUso'){
                  List.push(u);
              }
          });

          if(flagPrimaRiga){


              const SpecieAttivePrimaRiga=new Specie(0);

              SpecieAttivePrimaRiga.descrizione=this.translocoService.translate(descrizioneRigaVuota);

              List.unshift(SpecieAttivePrimaRiga);
          }

          resolve(List);
      });
  }*/

  Leggi_SpecieVegetali_Attive_Impianti(p: LeggiImpianto,flagPrimaRiga: boolean, descrizioneRigaVuota: string){
    return new Promise<Specie[] | DestinazioneUso[]>((resolve, reject) => {
      const List =[];

      const R = this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiImpianto, UtilizzoTerreno[]>('AnagraficaNG/LeggiSpecieVegetaliAttiveImpianti', p).pipe(map(R => {
        const risp=<UtilizzoTerreno[]>R.RispostaStringa;

        risp.forEach((u: any)=>{
          if(u.classType === 'Varieta'){
            List.push(u.specie);
          } else if(u.classType === 'DestinazioneUso'){
            List.push(u);
          }
        });

        if(flagPrimaRiga){
          const SpecieAttivePrimaRiga=new Specie(-1);
          SpecieAttivePrimaRiga.descrizione=this.translocoService.translate(descrizioneRigaVuota);
          List.unshift(SpecieAttivePrimaRiga);
        }

        resolve(List);
      })).subscribe();

    });
  }

  getSpecieQdCA(centroAziendale: CentroAziendaleQdCA, data: Date, impresa: ImpresaQdCA, consideraTerrenoNudo: boolean): Promise<Specie[] | DestinazioneUso[]> {
    return lastValueFrom(this.ajaxAgronicaAPIService.ajaxAPIPost<LeggiSpecieQdC, UtilizzoTerreno[]>('Anagrafica/LeggiSpecieVegetaliQdC', {
      centroAziendale: centroAziendale,
      data: data,
      impresa: impresa,
      consideraTerrenoNudo: consideraTerrenoNudo,
      soloAttiviAllaData: true
    }).pipe(
      map(x => {
        const res = x.RispostaStringa?.map((u: any) => u.classType === 'Varieta' ?  u.specie : u) ?? [];

        const specieAttivePrimaRiga = new Specie(-1);
        specieAttivePrimaRiga.descrizione = this.translocoService.translate("NessunaSelezione");
        res.unshift(specieAttivePrimaRiga);

        return res;
      }))
    );
  }

  public readDatiPrevisionaliColture(params: DatiPrevisionaliColtureRequest): Observable<rispostaStandard<DatiPrevisionaliColture>> {
    return this.ajaxAgronicaAPIService.ajaxAPIPost<DatiPrevisionaliColtureRequest, DatiPrevisionaliColture>(
      '/AnagraficaNG/readDatiPrevisionaliColture',
      params,
      false,
      false,
      true,
      false
    );
  }

}
