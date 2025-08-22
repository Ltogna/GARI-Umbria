/* eslint-disable */
import { BaseCodeDescr } from '../baseClass/baseCodeDescr';

export class RapportoContabile extends BaseCodeDescr {
    cliente: boolean;
    fornitore: boolean;
    dipendente: boolean;
    terzista: boolean;
    legale: boolean;
    agente: boolean;
    consulente: boolean;
    flag_cancellazione: boolean;

    constructor(codice: number) {
        super(codice);
        this.flag_cancellazione = false;
    }
}
