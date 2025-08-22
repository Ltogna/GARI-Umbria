/* eslint-disable */

import { IntervalloTemporale } from './IntervalloTemporale';
import { FormeGiuridiche } from '../metaschema/FormeGiuridiche';
import { CentroAziendale } from './CentroAziendale';
import { CodiciAnagrafeValori } from './CodiciAnagrafeValori';
import { IndirizzoAssociato } from './IndirizzoAssociato';
import { Contatto } from './Contatto';
import { RisorseUmane } from './RisorseUmane';
import { BaseCodeDescr } from '../baseClass/baseCodeDescr';
import { ImpresaPadre } from './ImpresaPadre';

export class Impresa {
    partitaIva: string;
    CUAA: string;
    ragioneSociale: string;
    validita: IntervalloTemporale;
    forma_Giuridica: FormeGiuridiche;
    tipo_Impresa: number;
    centriAziendali: CentroAziendale[];
    codici: CodiciAnagrafeValori[];
    impresaPadre: ImpresaPadre[];
    certificazione: BaseCodeDescr[]
    indirizzi: IndirizzoAssociato[];
    tecnicoReferente: Contatto;
    organismo_di_Controllo: RisorseUmane;
    contatti: Contatto[];
    contatto_superuser: Contatto;
    flag_cancellazione: boolean;
    gruppoRaccolta: BaseCodeDescr;

    constructor() {
        this.flag_cancellazione = false;
    }
}
