import { BaseCodeDescr } from "app/Model/baseClass/baseCodeDescr";
import {AGRODATAINIZIO} from '../../CostantiPersonalizzate';

export abstract class AvversitaGruppo extends BaseCodeDescr {
    classType: string;
    For_Veg_Av_Cod: number;
    formulatiXAllegatiNormative_IDRiga: number;
    dataSmaltimentoScorte: Date;

    constructor(codice: number) {
        super(codice);
        this.classType = 'Avversita'
    }
}
