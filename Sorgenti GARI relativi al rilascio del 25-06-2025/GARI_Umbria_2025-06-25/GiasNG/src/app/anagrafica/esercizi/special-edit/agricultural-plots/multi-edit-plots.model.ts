import {IListViewItem} from 'gias-kendo-grid';
import {IntervalloTemporale} from '../../../../Model/anagrafiche/IntervalloTemporale';
import {AGRODATAFINE, AGRODATAINIZIO} from '../../../../Model/CostantiPersonalizzate';
import {Appezzamento, PKAppezzamento} from '../../../../Model/anagrafiche/Appezzamento';

export class AgriculturalPlot implements IListViewItem {
  description: string;
  key: string | number;
  error: boolean;
  success: boolean;
  warning: boolean;
  piva: string;
  saCod: number;
  appezza: number;
  idReg: number;
  progettoCod: number;
  validity: IntervalloTemporale;

  constructor(
    key: string | number,
    description: string,
    piva: string,
    saCod: number,
    appezza: number,
    idReg: number = 0,
    progettoCod: number = 0,
    validity: IntervalloTemporale = new IntervalloTemporale(AGRODATAINIZIO, AGRODATAFINE),
    error: boolean = false,
    success: boolean = false,
    warning: boolean = false
  ) {
    this.key = key;
    this.description = description;
    this.piva = piva;
    this.saCod = saCod;
    this.appezza = appezza;
    this.error = error;
    this.success = success;
    this.warning = warning;
    this.idReg = idReg;
    this.progettoCod = progettoCod;
    this.validity = validity;
  }

  toAppezzamento(): Appezzamento {
    const plotPK: PKAppezzamento = {
      codice: this.appezza,
      centroAziendalePK: {
        partitaIva: this.piva,
        codice: this.saCod
      }
    };

    let plot: Appezzamento = new Appezzamento(plotPK);
    plot.validita = this.validity;
    plot.descrizione = this.description;

    return plot;
  }
}
