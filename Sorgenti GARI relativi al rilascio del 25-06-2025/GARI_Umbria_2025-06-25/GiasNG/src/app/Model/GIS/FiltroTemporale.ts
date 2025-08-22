/* eslint-disable */

import { enum_TipoFiltroTemporale, enum_OperatoreFiltroTemporale } from "app/GIS/GIS-enum/GIS-filtro-temporale";

import { AGRODATAINIZIO, AGRODATAFINE } from "../CostantiPersonalizzate";

export class FiltroTemporale {
    public TipoFiltroTemporale: enum_TipoFiltroTemporale;
    public DataInizio: Date;
    public TipoOperatoreDataInizio: enum_OperatoreFiltroTemporale;
    public DataFine: Date;
    public TipoOperatoreDataFine: enum_OperatoreFiltroTemporale;

    constructor() {
        this.TipoFiltroTemporale = enum_TipoFiltroTemporale.IntervalloTemporale;
        this.DataInizio = new Date(AGRODATAINIZIO);
        this.DataFine = new Date(AGRODATAFINE);
        this.TipoOperatoreDataInizio = enum_OperatoreFiltroTemporale.SuccessivoUguale;
        this.TipoOperatoreDataFine = enum_OperatoreFiltroTemporale.PrecedenteUguale;
    }

    setValidiDataOggi() {
        this.TipoFiltroTemporale = enum_TipoFiltroTemporale.ValidiAllaData;
        const dataOggi = new Date();
        dataOggi.setHours(0,0,0,0);
        this.DataInizio = new Date(dataOggi);
        this.DataFine = new Date(dataOggi);
        this.TipoOperatoreDataInizio = enum_OperatoreFiltroTemporale.SuccessivoUguale;
        this.TipoOperatoreDataFine = enum_OperatoreFiltroTemporale.PrecedenteUguale;
    }

    setValiditaData(
        startDate: Date, 
        endDate: Date, 
        opDateStart?: enum_OperatoreFiltroTemporale, 
        opDateEnd?: enum_OperatoreFiltroTemporale
    ): void {
        this.TipoFiltroTemporale = enum_TipoFiltroTemporale.ValidiAllaData;
        this.DataInizio = new Date(startDate);
        this.DataFine = new Date(endDate);
        this.TipoOperatoreDataInizio = opDateStart ?? enum_OperatoreFiltroTemporale.SuccessivoUguale;
        this.TipoOperatoreDataFine = opDateEnd ?? enum_OperatoreFiltroTemporale.PrecedenteUguale;
    }

    setEserciziValidiAllaData(dataValidita: Date, dataValiditaFine?: Date) {
        this.TipoFiltroTemporale = enum_TipoFiltroTemporale.EserciziValidiAllaData;
        dataValidita.setHours(0,0,0,0);
        this.DataInizio = new Date(dataValidita);
        
        if (dataValiditaFine != null) {
            dataValiditaFine.setHours(0,0,0,0);
        }

        this.DataFine = new Date(dataValiditaFine ?? dataValidita);
        this.TipoOperatoreDataInizio = enum_OperatoreFiltroTemporale.SuccessivoUguale;
        this.TipoOperatoreDataFine = enum_OperatoreFiltroTemporale.PrecedenteUguale;
    }

    setIntervalloTemporaleSingolaData(
        dataValidita: Date,
        dataValiditaFine?: Date,
        opDataInizio?: enum_OperatoreFiltroTemporale, 
        opDataFine?: enum_OperatoreFiltroTemporale
    ) {
        if (dataValiditaFine === undefined) {
            dataValiditaFine = dataValidita;
        }
        // Tipo filtro
        this.TipoFiltroTemporale = enum_TipoFiltroTemporale.IntervalloTemporale;
        dataValidita.setHours(0,0,0,0);
        // Data inizio
        this.DataInizio = this.calcolaDataInizioSingolaData(dataValidita);
        // Data fine
        this.DataFine = this.calcolaDataFineSingolaData(dataValiditaFine);
        // Operatori
        this.TipoOperatoreDataInizio = opDataInizio ?? enum_OperatoreFiltroTemporale.SuccessivoUguale;
        this.TipoOperatoreDataFine = opDataFine ?? enum_OperatoreFiltroTemporale.PrecedenteUguale;
    }

    calcolaDataInizioSingolaData(dataValidita: Date): Date {
        let dataInizio = new Date(dataValidita);
        dataInizio.setDate(dataInizio.getDate() - 10);
        return dataInizio;
    }

    calcolaDataFineSingolaData(dataValidita: Date): Date {
        let dataFine = new Date(dataValidita);
        dataFine.setDate(dataFine.getDate() + 5);
        return dataFine;
    }

    clona(): FiltroTemporale {
        let objClonato = new FiltroTemporale();

        objClonato.TipoFiltroTemporale = this.TipoFiltroTemporale,
        objClonato.DataInizio = new Date(this.DataInizio),
        objClonato.DataFine = new Date(this.DataFine),
        objClonato.TipoOperatoreDataInizio = this.TipoOperatoreDataInizio,
        objClonato.TipoOperatoreDataFine = this.TipoOperatoreDataFine

        return objClonato;
    }

}

export class FiltroTemporaleAvanzato {
    public FiltroPeriodo: FiltroTemporale;
    public FiltroSingolaData: FiltroTemporale;
}
