/* eslint-disable */
import { Documento } from '../documenti/Documento';
import { Utente } from '../utente/utente';
import { IndirizzoAssociato } from './IndirizzoAssociato';
import { RisorseUmane } from './RisorseUmane';
import { RubricaVoci } from './RubricaVoci';

export class Contatto {
    tipo: string;
    fittizio: boolean;
    fisico_Giuridico: number;
    estero: boolean;
    visibilita: string;
    ragione_Sociale: string;
    sa_cod: number;
    cognome: string;
    nome: string;
    codiceFiscale: string;
    nome_Breve: string;
    data_Nascita: string;
    sesso: string;
    convenevoli: string;
    badge: string;
    indirizzi: IndirizzoAssociato[];
    rubricaVoci: RubricaVoci[];
    utente: Utente;
    risorseUmane: RisorseUmane[];
    fe_Tipologia_Contatto: string;
    fe_Rappresentante_Fiscale: string;
    fe_Pec: string;
    fe_SDI: string;
    primaryKey: PK;
    flag_cancellazione: boolean;
    documenti: Documento[];
    visibilitaPubblica: boolean;

    constructor() {
        this.flag_cancellazione = false;
        this.primaryKey = new PK('', '');
    }
}

export class PK {
    partitaIva: string;
    codice: string;

    constructor(partitaIva: string, codice: string){
        this.partitaIva = partitaIva;
        this.codice = codice;
    }
}
