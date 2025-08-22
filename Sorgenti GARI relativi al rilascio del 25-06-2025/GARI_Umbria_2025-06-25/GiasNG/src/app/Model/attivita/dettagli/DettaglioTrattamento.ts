
import { RisorsaProdotto } from '../risorse/RisorsaProdotto';
import { AvversitaGruppo} from '../../metaschema/avversita/AvversitaGruppo';
import { PrincipioAttivo } from 'app/Model/metaschema/PrincipioAttivo';
import { BufferZone } from 'app/Model/metaschema/BufferZone';
import { DoseEtichetta } from 'app/Model/metaschema/DoseEtichetta';
import { Soglia } from 'app/Model/metaschema/Soglia';
import {Tipo_Polverulento} from "../../TipiEnumerativi";

export class DettaglioTrattamento extends RisorsaProdotto {
    avversitaGruppo: AvversitaGruppo
    principiAttivi: PrincipioAttivo[];
    tempoCarenza: number;
    bufferzone: BufferZone;
    dettaglioProdotto: number;
    dosiEtichetta: DoseEtichetta[];
    soglia: Soglia;
    descrizionePrecedente: string;
    dataSmaltimentoScorte: Date;
    classificazioni: string;
    inRevisione: string;
    dataAttoNormativo: Date;
    formulatiXAllegatiNormative_IDRiga: number;
    tipoFormulato: number;
    epocheBlocchi: string;
    dettagliDose: string;
    protezione: string;
    modalitaImpiego: string;
    polverulento: Tipo_Polverulento;
    isImpollinatore: boolean;
    durataFeromone: number;
    scadenzaFeromone: Date;

    constructor() {
        super();
        this.classType = 'DettaglioTrattamento';
    }
}
