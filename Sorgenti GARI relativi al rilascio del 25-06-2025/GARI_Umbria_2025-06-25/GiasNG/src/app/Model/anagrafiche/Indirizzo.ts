/* eslint-disable */


import { CodiciNazioniISO3166 } from '../metaschema/CodiciNazioniISO3166';
import { Istat } from '../metaschema/Istat';

export class Indirizzo {
    codice: number;
    via: string;
    frazione: string;
    istatComune: Istat;
    cap: string;
    stato: CodiciNazioniISO3166;
    note: string;
    flag_cancellazione: boolean;
}
