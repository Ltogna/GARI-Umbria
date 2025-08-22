/* eslint-disable */
import { Impresa } from 'app/Model/anagrafiche/Impresa';
import { ModelEntry, KendoGridModel, KendoServerResult, KendoGridColumn,
    KendoGridRow, JsonKendoResult } from 'gias-kendo-grid';
import { KendoCentroModel } from '../centri/centri.models';

export class ImpresaModel extends KendoGridModel {
    chiave: ModelEntry;
    rag_soc: ModelEntry;
    piva: ModelEntry;
    Codice_Fiscale: ModelEntry;
    Codice_Cuaa: ModelEntry;
    Codice_Socio: ModelEntry;
    Contratto_Produzione: ModelEntry;
    Indirizzo: ModelEntry;
    Tecnico_Referente: ModelEntry;
    Cooperativa_Referente: ModelEntry;
    Superficie_Totale: ModelEntry;
    Superficie_Tare: ModelEntry;
    SAU_Totale: ModelEntry;

    Pro_Cod_Istat: ModelEntry;
    Prov: ModelEntry;
    Com_Cod_Istat: ModelEntry;
    Com: ModelEntry;
    Stato: ModelEntry;
    Stato_Cod: ModelEntry;
    frz_des: ModelEntry;
    ind_des: ModelEntry;
    Cap: ModelEntry;

    Piva_Padre: ModelEntry;
    codice_iscrizione_libro_soci: ModelEntry;
    data_iscrizione_libro_soci: ModelEntry;
    Rag_Soc_Padre: ModelEntry;
    Num_Padri: ModelEntry;

    Validita_Inizio: ModelEntry;
    Validita_Fine: ModelEntry;
    Data_Creazione: ModelEntry;
    Data_Modifica: ModelEntry;
    Utente_Creazione: ModelEntry;
    Utente_Modifica: ModelEntry;
    Attivo: ModelEntry;
    Provincia: ModelEntry;
}

export class KendoImpresaRow {
    chiave: string;
    rag_soc: string;
    piva: string;
    Codice_Fiscale: string;
    Codice_Cuaa: string;
    Codice_Socio: string;
    Contratto_Produzione: string;
    Indirizzo: string;
    Tecnico_Referente: string;
    Cooperativa_Referente: string;
    Superficie_Totale: number;
    Superficie_Tare: number;
    SAU_Totale: number;

    Pro_Cod_Istat: string;
    Prov: string;
    Com_Cod_Istat: string;
    Com: string;
    Stato: string;
    Stato_Cod: string;
    frz_des: string;
    ind_des: string;

    Piva_Padre: string;
    Rag_Soc_Padre: string;
    Num_Padri: string;

    Validita_Inizio: Date;
    Validita_Fine: Date;
    Data_Creazione: Date;
    Data_Modifica: Date;
    Utente_Creazione: string;
    Utente_Modifica: string;
}


export class ImpresaKendoServerResult extends KendoServerResult {
    constructor(model, columns, rows) {
        super(model, columns, rows);
    }
}
