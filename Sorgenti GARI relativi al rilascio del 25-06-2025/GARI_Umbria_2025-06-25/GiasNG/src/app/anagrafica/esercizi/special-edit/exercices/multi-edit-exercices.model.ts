import {IListViewItem} from 'gias-kendo-grid';
import {IntervalloTemporale} from '../../../../Model/anagrafiche/IntervalloTemporale';
import {AGRODATAFINE, AGRODATAINIZIO} from '../../../../Model/CostantiPersonalizzate';
import {Esercizio} from '../../../../Model/anagrafiche/Esercizio';

export class AgriculturalExcercice implements IListViewItem {
  description: string;
  key: string | number;
  error: boolean;
  success: boolean;
  warning: boolean;
  piva: string;
  progettoCod: number;
  idReg: number;
  appezza: number;
  saCod: number;
  validity: IntervalloTemporale;

  constructor(
    key: string | number,
    description: string,
    piva: string,
    progettoCod: number,
    idReg: number = 0,
    appezza: number = 0,
    saCod: number = 0,
    validity: IntervalloTemporale = new IntervalloTemporale(AGRODATAINIZIO, AGRODATAFINE),
    error: boolean = false,
    success: boolean = false,
    warning: boolean = false
  ) {
    this.key = key;
    this.description = description;
    this.piva = piva;
    this.error = error;
    this.success = success;
    this.warning = warning;
    this.progettoCod = progettoCod;
    this.idReg = idReg;
    this.appezza = appezza;
    this.saCod = saCod;
    this.validity = validity;
  }

  toEsercizio(): Esercizio {
    let exe: Esercizio = new Esercizio(this.progettoCod, this.description);
    exe.validita = this.validity;
    exe.impiantoPK = {
      codice: this.idReg,
      appezzamentoPK: {
        codice: this.appezza,
        centroAziendalePK: {
          codice: this.saCod,
          partitaIva: this.piva
        }
      }
    };

    return exe;
  }
}
