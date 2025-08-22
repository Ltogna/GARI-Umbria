/* eslint-disable */
import { UtilizzoTerreno } from "../metaschema/utilizzi/UtilizzoTerreno";
import { FiltroTemporale } from "./FiltroTemporale";
import {SementieriParametrizzazione} from './SementieriParametrizzazione';
import {ConfigurazioneAlbero} from '../../Service/api.service';

export class GisDataReadParam {
    public piva: string;
    public sa_cod: string;
    public appezza: string;
    public id_imp: string;
    public TipologiaLayerSelezionata: string;
    public wktBoundaySTIntersects: string;
    public filtroTemporale: FiltroTemporale;
    public filtroTemporaleSingolaData: FiltroTemporale;
    public campo_cod: string;
    public veg_cod: UtilizzoTerreno;
    public cfgAlbero: ConfigurazioneAlbero;
    public cfgSementi: SementieriParametrizzazione;
    public layerElementiGrafici_cod: number[] = [];

    clona(): GisDataReadParam {
        let objClonato = new GisDataReadParam();

        objClonato.piva = this.piva;
        objClonato.sa_cod = this.sa_cod;
        objClonato.TipologiaLayerSelezionata = this.TipologiaLayerSelezionata;
        objClonato.filtroTemporale = this.filtroTemporale.clona();
        objClonato.filtroTemporaleSingolaData = this.filtroTemporaleSingolaData.clona();

        return objClonato;
    }

}
