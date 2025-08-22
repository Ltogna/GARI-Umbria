export class DettagliProtocollo {
    idProtocol: number;
    tipoPrescrizione: number;
    qtaDose: number;
    udmDose: number;
    durataTrattamento: number;

    constructor(idProtocol: number, tipoPrescrizione: number, qtaDose: number, udmDose: number, durataTrattamento: number) {
        this.idProtocol = idProtocol;
        this.tipoPrescrizione = tipoPrescrizione;
        this.qtaDose = qtaDose;
        this.udmDose = udmDose;
        this.durataTrattamento = durataTrattamento;
    }
}