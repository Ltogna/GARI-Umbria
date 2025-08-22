/* eslint-disable */
import {DatePipe} from "@angular/common";
import {Injectable} from "@angular/core";
import {FormControl, FormGroup} from "@angular/forms";
import {TranslocoService} from "@jsverse/transloco";
import {Lavorazione} from "app/Model/attivita/Lavorazione";
import {RilevamentoDiMagazzino} from "app/Model/attivita/RilevamentoDiMagazzino";
import {FERTILIZZANTI, FORMULATI, INSETTI, SEMENTI} from "app/Model/CostantiPersonalizzate";
import {BufferZone} from "app/Model/metaschema/BufferZone";
import {Soglia} from "app/Model/metaschema/Soglia";
import {UnitaDiMisura} from "app/Model/metaschema/UnitaDiMisura";
import {
    enum_doseQuantitaTotale,
    enum_LAVCOD, enum_PUARegolamenti_Tipo,
    enum_TipoMezzo,
    enum_TipoOperazioneDB,
    enum_UnitaMisura,
    Tipo_Polverulento
} from "app/Model/TipiEnumerativi";
import {
    DropdownListAvversita, DropdownListDisciplinare,
    DropdownListMagazzino,
    enum_Problema_DettaglioProdotto,
    MultiColumnComboboxDose_Etichetta,
    MultiColumnComboboxFertilizzazione,
    MultiColumnComboboxSemina,
    MultiColumnComboboxTrattamento,
    Sezione_Prodotto_Fertilizzanti,
    Sezione_Prodotto_Formulati,
    Sezione_Prodotto_Sementi
} from "../../quaderno-di-campagna-form/quaderno-di-campagna-form.model";
import {QdCService} from "../qdc.service";
import {UnitaDiMisuraService} from "../../../../Service/Metaschema/UnitaDiMisura.service";
import {FunzioniComuniService} from "../../../../Service/FunzioniComuni.service";
import {Epoca} from "../../../../Model/metaschema/Epoca";
import {Disciplinare} from "../../../../Model/metaschema/Disciplinari";
import {BaseCodeDescr} from "../../../../Model/baseClass/baseCodeDescr";
import {PrincipioAttivo} from "../../../../Model/metaschema/PrincipioAttivo";
import {Fabbricato} from "../../../../Model/anagrafiche/Fabbricato";
import {CentroAziendale} from "../../../../Model/anagrafiche/CentroAziendale";
import {cloneDeep} from "lodash";

@Injectable()

export class GridDosiProdottiService {

    constructor(
        private qdcservice: QdCService,
        public datepipe: DatePipe,
        private translocoService: TranslocoService,
        private unitadimisuraservice: UnitaDiMisuraService,
        private funzionicomuniservice: FunzioniComuniService
    ) {  }


    getRigaGridDosiProdotti(
        DosiProdotti: Array<any>,
        index: number,
        ProdottiForm: FormGroup,
        Prodotto: MultiColumnComboboxTrattamento | MultiColumnComboboxFertilizzazione | MultiColumnComboboxSemina,
        Centro_Aziendale: CentroAziendale,
        Operazione: Lavorazione,
        Data: Date,
        flagTipoDose: number,
        flagDoseQuantitaTotale:number,
        Dose_Acqua: number,
        ddl_dosi: MultiColumnComboboxDose_Etichetta[],
        EpocaDPI: Epoca,
        EpocaFertilizzazione: Epoca,
        Utilizza_Direttiva_Nitrati: boolean,
        Direttiva_Nitrati: Disciplinare,
        Opzioni_Semina: BaseCodeDescr,
        Modalita_Applicazione: BaseCodeDescr,
        riga_vuota: boolean,
        riga_salvata: boolean,
        nuova_riga_da_btn: boolean
    ): FormGroup {

        //Compone una nuova riga in base alla categoria di magazzino del prodotto
        //Se viene richiamata quando clicco sul bottone Inserisci Prodotto devo passare ProdottiForm valorizzato
        //Mentre quando sono in fase di lettura di una nuova Attivita Prodotto deve essere valorizzato
        //Se index è -1 allora ricavo l'indice per la nuova riga altrimenti vuol dire che sono in modifica e non genero il nuovo indice

        let Row: FormGroup;
        let RowIndex:number = index;
        let lav_cod: number = + Operazione.primaryKey.codice;

        let elem_cod = this.qdcservice.getCategoria_Magazzino(lav_cod);

        if(RowIndex === -1){
            if(DosiProdotti && DosiProdotti.length > 0){
                RowIndex = ( Math.max.apply(Math, DosiProdotti.map(function(o) { return o.DosiProdottiGridrowId; }))) + 1;
            }else{
                RowIndex = 0;
            }
        }

        let Prodotto_Selezionato: MultiColumnComboboxTrattamento | MultiColumnComboboxFertilizzazione | MultiColumnComboboxSemina = null;
        let Fabbricato_Cod = 0;
        let Fabbricato_Des = this.translocoService.translate("MagazzinoNonSelezionato");
        let Fr_Cod = 0;
        let Fr_Des = "";

        //Questo può essere solo kg o l
        let UdM_Magazzino: UnitaDiMisura = null;

        //Quando vado in modifica della riga sarà questa l'udm che utilizzerò
        //per impostare il valore nella ddl(non posso usare UdM_Indicata perché avrà il simbolo è concatenato  con hl o ha)
        let UdM: UnitaDiMisura = null;
        let UdM_Indicata: UnitaDiMisura = null;
        let Udm_Cod: string = "";
        let Udm_Des: string = "";
        let Magazzino_del_Prodotto_Selezionato: DropdownListMagazzino = null;
        let Magazzino_Agenzia: Fabbricato = null;
        let Magazzino_Esterno: Fabbricato = null;
        let Dose_Ha: number = 0;
        let Dose_Hl: number = 0;
        let DoseTot_Ha: number = 0;
        let Lotto: string = "";
        let Avversita: DropdownListAvversita = null;
        let Av_Cod: number = 0;
        let Av_Des: string = "";
        let Soglia_Avversita: Soglia = null;
        let Soglia_Des: string = "";
        let Dose_Etichetta: MultiColumnComboboxDose_Etichetta = null;
        let Dosi_Etichetta: Array<MultiColumnComboboxDose_Etichetta> = [];
        let Dosi_Etichetta_Des: string = "";
        let BufferZone: BufferZone = null;
        let BufferMin: number = 0;
        let BufferMax: number = 0;
        let Polverulento: Tipo_Polverulento = Tipo_Polverulento.NonPolverulento;
        let Carenza: number = 0;
        let Efficienza: number = 0;
        let N: number = 0;
        let N_Utile: number = 0;
        let P: number = 0;
        let K: number = 0;
        let Cu: number = 0;
        let Cod_Articolo: string = "";
        let Prima_Data: string = "";
        let Sup_Calcolata_Per_Frazionamento: number = 0;
        let Piva: string = "";
        let Sa_Cod: number = 0;
        let Visualizza_Solo_Prodotti_in_Giacenza: boolean = true;
        let Visualizza_Magazzini_Agenzie: boolean = false;
        let Visualizza_Magazzini_Esterni: boolean = false;
        let PrincipiAttivi: PrincipioAttivo[] = [];

        let N_Percentuale_X_Prodotto: number = 0;

        let problemi_DettaglioProdotto: Array<enum_Problema_DettaglioProdotto> = [];

        let problema_DettaglioProdotto_da_risolvere: enum_Problema_DettaglioProdotto = enum_Problema_DettaglioProdotto.Nessuno;

        let original_row_value: object = null;

        /*
        * Se mi arrivano più problemi_DettaglioTrattamento devo ricondurli tutti a un unico problema:
        * - Prodotto_Non_Corretto non imposto il prodotto nella combo
        * - Prodotto_Ambiguo non imposto il prodotto nella combo ma faccio partire la ricerca dei prodotti con la descrizione sua che fa da filtro
        * - Avversita_Non_Corretta, Avversita_Ambigua, Avversita_Non_Valorizzata non imposto l'avversita nella combo
        * */

        if(ProdottiForm && ProdottiForm.get("Prodotto").getRawValue()){
          problemi_DettaglioProdotto = ProdottiForm.get("Prodotto").getRawValue().Tutti_Problemi_DettaglioProdotto;
        }else if(Prodotto){
          problemi_DettaglioProdotto = Prodotto.Tutti_Problemi_DettaglioProdotto;
        }


        if(problemi_DettaglioProdotto && problemi_DettaglioProdotto.length > 0){

          if(elem_cod === FORMULATI){

            if(!this.qdcservice.Mostra_Avversita_Prima_Dei_Prodotti){
              if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Formulato_Non_Corretto)){
                problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Formulato_Non_Corretto;
              }else{
                if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Formulato_Ambiguo)){
                  problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Formulato_Ambiguo;
                }else{
                  if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Avversita_Non_Corretta) ||
                    problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Avversita_Non_Valorizzata)){
                    problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Avversita_Non_Corretta;
                  }else if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Avversita_Ambigua)){
                    problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Avversita_Ambigua;
                  }
                }
              }
            }else{
              if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Avversita_Non_Corretta) ||
                problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Avversita_Non_Valorizzata)){
                problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Avversita_Non_Corretta;
              }else{
                if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Avversita_Ambigua)){
                  problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Avversita_Ambigua;
                }else{
                  if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Formulato_Non_Corretto)){
                    problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Formulato_Non_Corretto;
                  }else if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Formulato_Ambiguo)){
                    problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Formulato_Ambiguo;
                  }
                }
              }
            }

          }else if(elem_cod === FERTILIZZANTI){
            if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Fertilizzante_Non_Corretto)){
              problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Fertilizzante_Non_Corretto;
            }else if(problemi_DettaglioProdotto.includes(enum_Problema_DettaglioProdotto.Fertilizzante_Ambiguo)){
              problema_DettaglioProdotto_da_risolvere = enum_Problema_DettaglioProdotto.Fertilizzante_Ambiguo;
            }
          }

        }

        if(!riga_vuota){
            if(ProdottiForm){

                //TODO Da verificare bene fatto così perchè sembra che il valore del getrawvalue venga passatoi per riferimento e non funziona bene perchè dopo lo ricambio il valore
                //let ProdottiFormValue: Sezione_Prodotto_Formulati | Sezione_Prodotto_Fertilizzanti | Sezione_Prodotto_Sementi  = JSON.parse(JSON.stringify(ProdottiForm.getRawValue()))
                let ProdottiFormValue: Sezione_Prodotto_Formulati | Sezione_Prodotto_Fertilizzanti | Sezione_Prodotto_Sementi  = ProdottiForm.getRawValue();

                if(ProdottiFormValue){

                    if(ProdottiFormValue.Prodotto){
                        Prodotto_Selezionato = ProdottiFormValue.Prodotto;
                        Fr_Cod = ProdottiFormValue.Prodotto.prodotto.codice;
                        Fr_Des = ProdottiFormValue.Prodotto.prodotto.descrizione;

                        Magazzino_Agenzia = this.getMagazzino_Agenzia(ProdottiFormValue.Prodotto.MagazziniMovimentazioni);

                        let Magazzino_Esterno_con_Lotto = this.qdcservice.getMagazzino_Esterno_con_Lotto(ProdottiFormValue.Prodotto.MagazziniMovimentazioni);

                        if(Magazzino_Esterno_con_Lotto && Magazzino_Esterno_con_Lotto.Magazzino_Esterno){
                          Magazzino_Esterno = Magazzino_Esterno_con_Lotto.Magazzino_Esterno;
                        }
                    }


                    if(ProdottiFormValue.Magazzino_del_Prodotto_Selezionato){
                        Magazzino_del_Prodotto_Selezionato = ProdottiFormValue.Magazzino_del_Prodotto_Selezionato;
                        Piva = Magazzino_del_Prodotto_Selezionato.primaryKey.centroAziendalePK.partitaIva;
                        Sa_Cod = Magazzino_del_Prodotto_Selezionato.primaryKey.centroAziendalePK.codice;
                        Fabbricato_Cod = ProdottiFormValue.Magazzino_del_Prodotto_Selezionato.primaryKey.codice;
                        Fabbricato_Des = ProdottiFormValue.Magazzino_del_Prodotto_Selezionato.descrizione;
                    }


                    if(ProdottiFormValue.UdM){
                        UdM_Indicata = new UnitaDiMisura(ProdottiFormValue.UdM.codice,ProdottiFormValue.UdM.descrizione,ProdottiFormValue.UdM.simbolo);
                        UdM = new UnitaDiMisura(UdM_Indicata?.codice,UdM_Indicata.descrizione,UdM_Indicata.simbolo);
                        UdM_Indicata.simbolo = this.getSimboloUnitaDiMisuraIndicata(UdM_Indicata, ProdottiFormValue.flagTipoDose);
                        Udm_Cod = this.setUdm_Cod_for_ddl(UdM_Indicata.codice.toString(),UdM_Indicata.simbolo);
                        Udm_Des = UdM_Indicata.simbolo;
                        UdM_Magazzino = this.getUnitaDiMisura(UdM);
                    }

                    Dose_Ha = ProdottiFormValue.Dose_Ha;
                    Dose_Hl = ProdottiFormValue.Dose_Hl;
                    DoseTot_Ha = ProdottiFormValue.DoseTot_Ha;
                    Lotto = ProdottiFormValue.Lotto;
                    Visualizza_Solo_Prodotti_in_Giacenza = ProdottiFormValue.Visualizza_Solo_Prodotti_in_Giacenza;
                    Visualizza_Magazzini_Agenzie = ProdottiFormValue.Visualizza_Magazzini_Agenzie;
                    Visualizza_Magazzini_Esterni = ProdottiFormValue.Visualizza_Magazzini_Esterni;

                    switch(elem_cod){
                        case FORMULATI:
                        case INSETTI:

                            ProdottiFormValue = ProdottiFormValue as Sezione_Prodotto_Formulati;

                            if(ProdottiFormValue.Avversita){
                                Avversita = ProdottiFormValue.Avversita;
                                Av_Cod = ProdottiFormValue.Avversita.codice;
                                Av_Des = ProdottiFormValue.Avversita.descrizione;
                            }

                            if(elem_cod === FORMULATI){
                                if(ProdottiFormValue.Soglia_Avversita){
                                    Soglia_Avversita = ProdottiFormValue.Soglia_Avversita;
                                    Soglia_Des = ProdottiFormValue.Soglia_Avversita.descrizione;
                                }

                                //Controllo se ci sono delle altre dosi collegate a quella scelta dall'utente
                                if(ProdottiFormValue.Dose_Etichetta){
                                    Dosi_Etichetta = this.getDosi_Etichetta(ddl_dosi,ProdottiFormValue.Dose_Etichetta);
                                    Dose_Etichetta = ProdottiFormValue.Dose_Etichetta;
                                }

                                Dosi_Etichetta_Des = this.getDescrizioniDosiEtichetta(Dosi_Etichetta);

                                if(ProdottiFormValue.Prodotto?.bufferzone){
                                    BufferZone = ProdottiFormValue.Prodotto?.bufferzone;
                                    BufferMin = ProdottiFormValue.Prodotto?.bufferzone.minimo;
                                    BufferMax =  ProdottiFormValue.Prodotto?.bufferzone.massimo;
                                }

                                if(ProdottiFormValue.Prodotto?.tempoCarenza)
                                    Carenza = ProdottiFormValue.Prodotto?.tempoCarenza;

                                Prima_Data = this.qdcservice.getDataPrimaRaccoltaUtile(Carenza,Data);

                                if(ProdottiFormValue.Prodotto)
                                    Polverulento = ProdottiFormValue.Prodotto.polverulento;

                                if(ProdottiFormValue.Prodotto?.principiAttivi)
                                    PrincipiAttivi = ProdottiFormValue.Prodotto?.principiAttivi;
                            }
                            break;
                        case FERTILIZZANTI:

                            ProdottiFormValue = ProdottiFormValue as Sezione_Prodotto_Fertilizzanti;
                            Efficienza = ProdottiFormValue.Efficienza;
                            N = ProdottiFormValue.N;
                            N_Utile = ProdottiFormValue.N_Utile;
                            P = ProdottiFormValue.P;
                            K = ProdottiFormValue.K;
                            Cu = ProdottiFormValue.Cu;
                            N_Percentuale_X_Prodotto = ProdottiFormValue.N_Percentuale_X_Prodotto;

                            break;
                        case SEMENTI:
                            ProdottiFormValue = ProdottiFormValue as Sezione_Prodotto_Sementi;

                            if(ProdottiFormValue.Prodotto){
                                Cod_Articolo = ProdottiFormValue.Prodotto.codArticolo;
                            }

                            Sup_Calcolata_Per_Frazionamento = ProdottiFormValue.Sup_Calcolata;

                            break;
                    }

                  original_row_value = cloneDeep(ProdottiFormValue.Original_Row_Value);

                }

            } else if(Prodotto) {

                //TODO Da verificare bene fatto così perchè sembra che il valore del getrawvalue venga passatoi per riferimento e non funziona bene perchè dopo lo ricambio il valore
                //Prodotto_Selezionato = JSON.parse(JSON.stringify(Prodotto));

                Prodotto_Selezionato = Prodotto;

                if(problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Formulato_Ambiguo ||
                  problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Formulato_Non_Corretto){

                    if(!this.qdcservice.Mostra_Avversita_Prima_Dei_Prodotti){

                        if(problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Formulato_Ambiguo){

                            Prodotto = this.qdcservice.setCodDescrMultiColumnComboboxProdotto(FORMULATI,Prodotto);

                            Prodotto_Selezionato = Prodotto;
                        }else if(problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Formulato_Non_Corretto){
                            Prodotto_Selezionato = this.qdcservice.Obj_Empty_MultiColumnComboboxTrattamento;
                        }
                    }else{
                        Prodotto_Selezionato = this.qdcservice.Obj_Empty_MultiColumnComboboxTrattamento;
                    }
                }

                if(problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Fertilizzante_Ambiguo ||
                  problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Fertilizzante_Non_Corretto){

                  if(problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Fertilizzante_Ambiguo){

                    Prodotto = this.qdcservice.setCodDescrMultiColumnComboboxProdotto(FERTILIZZANTI,Prodotto);

                    Prodotto_Selezionato = Prodotto;
                  }else if(problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Fertilizzante_Non_Corretto){
                    Prodotto_Selezionato = this.qdcservice.Obj_Empty_MultiColumnComboboxFertilizzazione;
                  }
                }

                const MagazziniMovimentazioni: Array<RilevamentoDiMagazzino> = Prodotto?.MagazziniMovimentazioni;

                if(MagazziniMovimentazioni && MagazziniMovimentazioni.length === 1){

                    const MagazzinoMovimentazione: RilevamentoDiMagazzino = MagazziniMovimentazioni[0];

                    Fabbricato_Cod = MagazzinoMovimentazione?.Magazzino?.primaryKey?.codice;
                    Fabbricato_Des = MagazzinoMovimentazione?.Magazzino?.descrizione;
                    Magazzino_del_Prodotto_Selezionato = this.qdcservice.setCodDescrDdlMagazzino(MagazzinoMovimentazione?.Magazzino as DropdownListMagazzino);
                    Piva =  MagazzinoMovimentazione?.Magazzino?.primaryKey.centroAziendalePK.partitaIva;
                    Sa_Cod = MagazzinoMovimentazione?.Magazzino?.primaryKey.centroAziendalePK.codice;
                    Lotto = MagazzinoMovimentazione?.Lotto;
                    Dose_Ha = MagazzinoMovimentazione?.doseHaIndicata;
                    Dose_Hl = MagazzinoMovimentazione?.doseHlIndicata;
                    DoseTot_Ha = this.getDosePerMagazzino(MagazzinoMovimentazione?.Qta,Prodotto?.unitaDiMisuraIndicata?.codice);
                    UdM_Magazzino = MagazzinoMovimentazione?.udm;
                    Magazzino_Agenzia = MagazzinoMovimentazione.Agenzia;

                    if(Magazzino_Agenzia){
                      Visualizza_Magazzini_Agenzie = true;
                    }

                    let Magazzino_Esterno_con_Lotto = this.qdcservice.getMagazzino_Esterno_con_Lotto(MagazziniMovimentazioni);

                    if(Magazzino_Esterno_con_Lotto && Magazzino_Esterno_con_Lotto.Magazzino_Esterno){
                      Magazzino_Esterno = Magazzino_Esterno_con_Lotto.Magazzino_Esterno;
                      Visualizza_Magazzini_Esterni = true;
                    }
                } else {
                    Dose_Ha = Prodotto?.doseHaReale;
                    Dose_Hl = Prodotto?.doseHlReale;
                    DoseTot_Ha = Prodotto?.quantitaTotaleReale;
                }

                Fr_Cod = Prodotto.prodotto.codice;
                Fr_Des = Prodotto.prodotto.descrizione;


                if(Prodotto?.unitaDiMisuraIndicata){
                    UdM_Indicata =new UnitaDiMisura(Prodotto?.unitaDiMisuraIndicata.codice,Prodotto?.unitaDiMisuraIndicata.descrizione,Prodotto?.unitaDiMisuraIndicata.simbolo);
                    UdM = new UnitaDiMisura(UdM_Indicata.codice,UdM_Indicata.descrizione,UdM_Indicata.simbolo);
                    UdM.simbolo = this.getSimboloOriginaleUnitaDiMisuraIndicata(UdM_Indicata);
                    Udm_Cod = this.setUdm_Cod_for_ddl(Prodotto?.unitaDiMisuraIndicata?.codice.toString(),Prodotto?.unitaDiMisuraIndicata?.simbolo);
                    Udm_Des = Prodotto?.unitaDiMisuraIndicata?.simbolo;
                }

                switch(elem_cod){
                    case FORMULATI:
                    case INSETTI:

                        Prodotto = Prodotto as MultiColumnComboboxTrattamento;


                        if((!this.qdcservice.Mostra_Avversita_Prima_Dei_Prodotti && problema_DettaglioProdotto_da_risolvere !== enum_Problema_DettaglioProdotto.Nessuno) ||
                            (this.qdcservice.Mostra_Avversita_Prima_Dei_Prodotti && (problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Avversita_Ambigua || problema_DettaglioProdotto_da_risolvere === enum_Problema_DettaglioProdotto.Avversita_Non_Corretta))){

                            Avversita = null;

                            Av_Cod = 0;

                            Av_Des = "";
                        }else{
                            if(Prodotto?.avversitaGruppo){
                            Avversita = this.qdcservice.setCodDescrDdlAvversita(Prodotto?.avversitaGruppo as DropdownListAvversita,lav_cod);
                                Av_Cod = Prodotto?.avversitaGruppo?.codice;
                                Av_Des = Prodotto?.avversitaGruppo?.descrizione;
                            }
                        }

                        if(elem_cod === FORMULATI){
                            Soglia_Avversita = Prodotto?.soglia;
                            Soglia_Des = Prodotto?.soglia?.descrizione;

                            if(Prodotto?.dosiEtichetta && Prodotto?.dosiEtichetta.length > 0){
                                Dosi_Etichetta = Prodotto?.dosiEtichetta as MultiColumnComboboxDose_Etichetta[];
                                Dose_Etichetta =  Prodotto?.dosiEtichetta[0] as MultiColumnComboboxDose_Etichetta;
                            }

                            Dosi_Etichetta_Des = this.getDescrizioniDosiEtichetta(Dosi_Etichetta);

                            if(Prodotto.bufferzone){
                                BufferZone = Prodotto.bufferzone;
                                BufferMin = Prodotto.bufferzone.minimo;
                                BufferMax = Prodotto.bufferzone.massimo;
                            }

                            Carenza = Prodotto.tempoCarenza;
                            Prima_Data = this.qdcservice.getDataPrimaRaccoltaUtile(Carenza,Data);
                            Polverulento = Prodotto.polverulento;
                            PrincipiAttivi = Prodotto.principiAttivi;

                        }

                        break;
                    case FERTILIZZANTI:
                        Prodotto = Prodotto as MultiColumnComboboxFertilizzazione;
                        Efficienza = Prodotto.efficienza;
                        N = Prodotto.N;
                        N_Utile = this.getN_Utile(Efficienza, N);
                        P = Prodotto.P;
                        K = Prodotto.K;
                        Cu = Prodotto.Cu;
                        N_Percentuale_X_Prodotto = 0;

                        break;
                    case SEMENTI:
                        Prodotto = Prodotto as MultiColumnComboboxSemina;
                        Cod_Articolo = Prodotto.codArticolo;
                        break;
                }
            }
        }

        Row = new FormGroup({
            'Categoria_Magazzino': new FormControl(elem_cod),
            'Prodotto': new FormControl(Prodotto_Selezionato),
            'Centro_Aziendale': new FormControl(Centro_Aziendale),
            'Piva': new FormControl(Piva),
            'Sa_Cod': new FormControl(Sa_Cod),
            'Fabbricato_Cod': new FormControl(Fabbricato_Cod),
            'Fabbricato_Des': new FormControl(Fabbricato_Des),
            'Fr_Cod': new FormControl(Fr_Cod),
            'Fr_Des': new FormControl(Fr_Des),
            'UdM': new FormControl(UdM),
            'Operazione' : new FormControl(Operazione),
            'UdM_Magazzino':  new FormControl(UdM_Magazzino),
            'UdM_Indicata': new FormControl(UdM_Indicata),
            'Udm_Cod': new FormControl(Udm_Cod),
            'Udm_Des': new FormControl(Udm_Des),
            'Udm_Cod_Trasformato': new FormControl(0),
            'Magazzino_del_Prodotto_Selezionato': new FormControl(Magazzino_del_Prodotto_Selezionato),
            'Dose_Ha': new FormControl(this.funzionicomuniservice.roundNumber(Dose_Ha,this.qdcservice.Obj_Default_Numeric_Settings.decimals)),
            'Dose_Hl': new FormControl(this.funzionicomuniservice.roundNumber(Dose_Hl,this.qdcservice.Obj_Default_Numeric_Settings.decimals)),
            'DoseTot_Ha': new FormControl(this.funzionicomuniservice.roundNumber(DoseTot_Ha,this.qdcservice.Obj_Default_Numeric_Settings.decimals)),
            'Ha_Hl': new FormControl(0),
            'Dose_QtaTot': new FormControl(0),
            'For_Veg_Cod': new FormControl(0),
            'Dose_Etichetta_Max': new FormControl(0),
            'Dose_Etichetta_Value': new FormControl(0),
            'Limite_Numero_Trattamenti': new FormControl(0),
            'Limite_Numero_Trattamenti_UDM_SIM': new FormControl(0),
            'Limite_Numero_Trattamenti_UDM_COD': new FormControl(0),
            'IntervalloTrattamenti_Min': new FormControl(0),
            'IntervalloTrattamenti_Max': new FormControl(0),
            'strPA_COD': new FormControl(""),
            'strPA_COD_Pesi': new FormControl(""),
            'strCLTOSS_COD': new FormControl(""),
            'MgO': new FormControl(0),
            'Dose_Fittizia': new FormControl(0),
            'Lotto': new FormControl(Lotto),
            'Mat_Regolamento': new FormControl(0),
            'Mat_Veg_Cod': new FormControl(0),
            'Mat_Cul_Cod': new FormControl(0),
            'Sup_Calcolata': new FormControl(this.funzionicomuniservice.roundNumber(Sup_Calcolata_Per_Frazionamento,this.qdcservice.Obj_Default_Numeric_Settings.decimals)),
            'Piva_Rif': new FormControl(""),
            'Sa_Cod_Rif': new FormControl(0),
            'ID_Agenda_Rif': new FormControl(0),
            'ID_Mov_Rif': new FormControl(0),
            'ID_Mov_Det_Rif': new FormControl(0),
            'Lav_Cod_Rif': new FormControl(0),
            'Cau_Mov_Rif': new FormControl(""),
            'Des_Rif': new FormControl(""),
            'Qta_Rif': new FormControl(0),
            'flagTipoDose': new FormControl(flagTipoDose),
            'flagDoseQuantitaTotale': new FormControl(flagDoseQuantitaTotale),
            'Dose_Acqua': new FormControl(this.funzionicomuniservice.roundNumber(Dose_Acqua,this.qdcservice.Obj_Default_Numeric_Settings.decimals)),
            'DosiProdottiGridrowId': new FormControl(RowIndex),
            'Visualizza_Magazzini_Agenzie': new FormControl(Visualizza_Magazzini_Agenzie),
            'Visualizza_Magazzini_Esterni': new FormControl(Visualizza_Magazzini_Esterni),
            'Magazzino_Agenzia': new FormControl(Magazzino_Agenzia),
            'Magazzino_Esterno': new FormControl(Magazzino_Esterno),
            'Visualizza_Solo_Prodotti_in_Giacenza': new FormControl(Visualizza_Solo_Prodotti_in_Giacenza),
            'Riga_Salvata': new FormControl(riga_salvata),
            'Nuova_Riga': new FormControl(nuova_riga_da_btn),
            'Modalita_Applicazione': new FormControl(Modalita_Applicazione),
            'Original_Row_Value': new FormControl(original_row_value),
            'Tutti_Problemi_DettaglioProdotto': new FormControl(problemi_DettaglioProdotto),
            'Problema_DettaglioProdotto_Da_Risolvere': new FormControl(problema_DettaglioProdotto_da_risolvere)
        });

        switch(elem_cod){
            case FORMULATI:
            case INSETTI:
                Row.addControl('Avversita',new FormControl(Avversita));
                Row.addControl('Av_Cod',new FormControl(Av_Cod));
                Row.addControl('Av_Gru',new FormControl(""));
                Row.addControl('Av_Des',new FormControl(Av_Des));

                if(elem_cod === FORMULATI){
                    Row.addControl('Soglia_Avversita',new FormControl(Soglia_Avversita));
                    Row.addControl('Soglia_Value',new FormControl(0));
                    Row.addControl('Soglia_Des',new FormControl(Soglia_Des));
                    Row.addControl('Dose_Etichetta',new FormControl(Dose_Etichetta));
                    Row.addControl('Dosi_Etichetta',new FormControl(Dosi_Etichetta));
                    Row.addControl('Dose_Etichetta_Des',new FormControl(Dosi_Etichetta_Des));


                    Row.addControl('BufferZone',new FormControl(BufferZone));
                    Row.addControl('BufferMin',new FormControl(this.funzionicomuniservice.roundNumber(BufferMin,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                    Row.addControl('BufferMax',new FormControl(this.funzionicomuniservice.roundNumber(BufferMax,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                    Row.addControl('strBuffer',new FormControl(this.getStrBuffer(BufferMin,BufferMax)));

                    Row.addControl('Carenza',new FormControl(Carenza));
                    Row.addControl('Prima_Raccolta',new FormControl(Prima_Data));
                    Row.addControl('Polverulento',new FormControl(Polverulento));
                    Row.addControl('EpocaDPI',new FormControl(EpocaDPI));
                    Row.addControl('PrincipiAttivi',new FormControl(PrincipiAttivi));
                }

                break;
            case FERTILIZZANTI:
                Row.addControl('Efficienza',new FormControl(Efficienza));
                Row.addControl('N',new FormControl(this.funzionicomuniservice.roundNumber(N,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                Row.addControl('N_Utile',new FormControl(this.funzionicomuniservice.roundNumber(N_Utile,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                Row.addControl('P',new FormControl(this.funzionicomuniservice.roundNumber(P,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                Row.addControl('K',new FormControl(this.funzionicomuniservice.roundNumber(K,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                Row.addControl('Cu',new FormControl(this.funzionicomuniservice.roundNumber(Cu,this.qdcservice.Obj_Default_Numeric_Settings.decimals)));
                Row.addControl('EpocaFertilizzazione',new FormControl(EpocaFertilizzazione));
                Row.addControl('Utilizza_Direttiva_Nitrati',new FormControl(Utilizza_Direttiva_Nitrati));
                Row.addControl('Direttiva_Nitrati',new FormControl(Direttiva_Nitrati));
                Row.addControl('N_Percentuale_X_Prodotto',new FormControl(N_Percentuale_X_Prodotto));

                break;
            case SEMENTI:
                Row.addControl('Cod_Articolo',new FormControl(Cod_Articolo));
                Row.addControl('Opzioni_Semina',new FormControl(Opzioni_Semina));
                break;
        }

        return Row;
    }

    //Aggiunge una riga vuota nel GriDosiProdotti per visualizzare il formGroup appena viene richiamato il qdcservice.Gestisci_Sezioni_Prodotto
    public AggiungiRigaVuotaGridDosiProdotti(disciplinare: DropdownListDisciplinare = null){
        if(!this.qdcservice.Abilitato_Inserimento_Prodotti_da_Grid_Dosi()){
            let sezioni = this.qdcservice.Sezioni_ProdottoFormArray.getRawValue();

            if(sezioni){
                sezioni.forEach(s=>{
                    if(s.DosiProdotti.length === 0 && s.Operazione.primaryKey.codice !== enum_LAVCOD.RACCOLTA){

                        let lav_cod = + (<Lavorazione>s.Operazione).primaryKey.codice;
                        let Opzioni_Semina = this.qdcservice.get_Default_Opzione_Semina(lav_cod);

                        let Utilizza_Direttiva_Nitrati: boolean = false;

                        let Direttiva_Nitrati: Disciplinare = null;

                        if(+ s.Operazione.primaryKey.codice === enum_LAVCOD.DISTRIBUZIONE_AMMENDANTI){

                            if(s.Utilizza_Direttiva_Nitrati){

                                Utilizza_Direttiva_Nitrati = true;

                                if(disciplinare && disciplinare.Codice_Concatenato !== ""){
                                    if(disciplinare.regolamentoConcimazione && disciplinare.regolamentoConcimazione.tipo === enum_PUARegolamenti_Tipo.PUA){
                                        Utilizza_Direttiva_Nitrati = true;

                                        Direttiva_Nitrati = disciplinare as Disciplinare;
                                    }
                                }
                            }
                        }else{
                            Utilizza_Direttiva_Nitrati = false;

                            Direttiva_Nitrati = null;
                        }

                        let row: FormGroup = this.getRigaGridDosiProdotti(s.DosiProdotti,-1,null,null,null,
                            s.Operazione,null,enum_TipoMezzo.Ettaro,
                            enum_doseQuantitaTotale.Dose,null,[],null,null,Utilizza_Direttiva_Nitrati,Direttiva_Nitrati,Opzioni_Semina,null,false,false, false);


                        this.qdcservice.AggiornaFormArrayGridDosiProdotti(row.getRawValue(),-1,s.Operazione,enum_TipoOperazioneDB.Scrittura,null);
                    }
                });
            }

        }
    }

    private getStrBuffer(BufferMin: number, BufferMax: number): string{

        //Costruzione stringa bufferzone
        let strBuffer: string = '';

        if (BufferMin !== 0 || BufferMax !== 0)
            strBuffer = BufferMin + " - " + BufferMax;

        return strBuffer;

    }

    private getDescrizioniDosiEtichetta(dosiEtichetta: MultiColumnComboboxDose_Etichetta[]): string{

        let DescrizioneDosiEtichetta: string = "";

        if(dosiEtichetta){
            DescrizioneDosiEtichetta = dosiEtichetta.map((d: MultiColumnComboboxDose_Etichetta) =>
                this.qdcservice.setCodDescrConcatenataMultiColumnComboboxMultiColumnComboboxDose_Etichetta(d).Descrizione_Concatenata
            ).join(", <br>");
        }

        return DescrizioneDosiEtichetta;
    }

    private getN_Utile(efficienza: number, n: number){

        let N_Utile: number = 0;

        N_Utile = efficienza * n;

        return N_Utile;
    }

    public getUnitaDiMisura(udm_indicata: UnitaDiMisura): UnitaDiMisura{
        //Ottiene l'unità di misura da salvare nella griglia
        //Replica la funzione in Trattamenti_2.ControllaSelezioneUnitadiMisura
        //In Trattamenti_2.ControllaSelezioneUnitadiMisura UdmCod è udm_indicata (udm scelta dall’utente)
        //In Trattamenti_2.ControllaSelezioneUnitadiMisura UdmCodTrasformato è udm (sempre kg o l per fitosanitari)

        let udm_simbolo: string = "";

        let udm_codice: number = 0;

        let udm_descrizione: string = "";

        if(udm_indicata){

            udm_descrizione = udm_indicata.descrizione;

            switch(udm_indicata.codice){
                case enum_UnitaMisura.Grammi:
                case enum_UnitaMisura.Quintali:
                case enum_UnitaMisura.Tonnellate:
                case enum_UnitaMisura.Milligrammi:
                    udm_codice = enum_UnitaMisura.KG;
                    udm_simbolo = "kg";
                    break;
                case enum_UnitaMisura.Millilitri:
                case enum_UnitaMisura.CentimetriCubi:
                case enum_UnitaMisura.Metri_Cubi:
                    udm_codice = enum_UnitaMisura.Litri;
                    udm_simbolo = "l";
                    break;
                case enum_UnitaMisura.KG:
                case enum_UnitaMisura.Litri:
                case enum_UnitaMisura.Unita_Seme:
                case enum_UnitaMisura.Num_Piante:
                case enum_UnitaMisura.Confezioni:
                case enum_UnitaMisura.Numero:
                case enum_UnitaMisura.Numero_Diffusori:
                case enum_UnitaMisura.UNITA:
                    udm_codice = udm_indicata.codice;
                    udm_simbolo = udm_indicata.simbolo;
                    break;
            }
        }

        return  new UnitaDiMisura(udm_codice,udm_descrizione,udm_simbolo);

    }

    public  getSimboloUnitaDiMisuraIndicata(udm_indicata: UnitaDiMisura, flagTipoDose: number): string{

        //Ottiene l'unità di misura indicata da salvare nella griglia
        //Replica la funzione in Trattamenti_2.ControllaSelezioneUnitadiMisura
        //In Trattamenti_2.ControllaSelezioneUnitadiMisura UdmCod è udm_indicata (udm scelta dall’utente)
        //In Trattamenti_2.ControllaSelezioneUnitadiMisura UdmCodTrasformato è udm (sempre kg o l per fitosanitari)

        let udm_indicata_simbolo: string = "";

        let new_udm = new UnitaDiMisura(udm_indicata.codice,udm_indicata.descrizione,udm_indicata.simbolo);

        let simbolo: string = new_udm?.simbolo;

        switch(flagTipoDose){
            case enum_TipoMezzo.Ettolitro:
                udm_indicata_simbolo = "[" + simbolo;

                if(!simbolo.includes("/hl"))
                    udm_indicata_simbolo +="/hl";

                break;
            case enum_TipoMezzo.Ettaro:
                udm_indicata_simbolo = "[" + simbolo;

                if(!simbolo.includes("/ha"))
                    udm_indicata_simbolo +="/ha";
                break;
        }

        if(!simbolo.includes("]"))
            udm_indicata_simbolo +="]";

        return  udm_indicata_simbolo;

    }

    private getSimboloOriginaleUnitaDiMisuraIndicata(udm_indicata: UnitaDiMisura): string{

        //Ottieni l'unita di misura originale che ha selezionato l'utente dalla ddl

        let udm_indicata_simbolo: string = udm_indicata.simbolo;

        if(this.qdcservice.Elenco_UdM_Radice_Con_Ha.includes(udm_indicata.codice) || this.qdcservice.Elenco_UdM_Radice_Con_Hl.includes(udm_indicata.codice)){
            udm_indicata_simbolo = udm_indicata_simbolo.replace("[", "").replace("]", "");
        }else{
            udm_indicata_simbolo = udm_indicata_simbolo.replace("[", "").replace("]", "").split("/")[0];
        }

        return  udm_indicata_simbolo;

    }

    setUdm_Cod_for_ddl(Udm_Cod: string, Udm_Des: string){

        if(Udm_Des.includes('hl'))
            Udm_Cod += "_" + enum_TipoMezzo.Ettolitro;
        else if(Udm_Des.includes('ha'))
            Udm_Cod += "_" + enum_TipoMezzo.Ettaro;

        return Udm_Cod;
    }


    public getDosePerMagazzino(doseTrasformata: number, udm_codice: number): number{
        let doseIndicata : number = 0

        switch(udm_codice){
            case enum_UnitaMisura.Grammi:
                doseIndicata = doseTrasformata * 1000;
                break;
            case enum_UnitaMisura.Milligrammi:
                doseIndicata = doseTrasformata * 1000000;
                break;
            case enum_UnitaMisura.Quintali:
                doseIndicata = doseTrasformata / 100;
                break;
            case enum_UnitaMisura.Tonnellate:
                doseIndicata = doseTrasformata / 1000;
                break;
            case enum_UnitaMisura.Millilitri:
                doseIndicata = doseTrasformata * 1000;
                break;
            case enum_UnitaMisura.CentimetriCubi:
                doseIndicata = doseTrasformata * 1000;
                break;
            case enum_UnitaMisura.Metri_Cubi:
                doseIndicata = doseTrasformata / 1000;
                break;
            case enum_UnitaMisura.Litri:
            case enum_UnitaMisura.KG:
            case enum_UnitaMisura.Unita_Seme:
            case enum_UnitaMisura.Num_Piante:
            case enum_UnitaMisura.Confezioni:
            case enum_UnitaMisura.Numero:
            case enum_UnitaMisura.Numero_Diffusori:
            case enum_UnitaMisura.UNITA:
                doseIndicata = doseTrasformata;
                break;
        }

        return doseIndicata;
    }

    //introdotta verifica dei dosaggi tra loro solo per i dosaggi dello stesso decreto ( = FormulatiXAllegatiNormative_IDRiga)
    public getDosi_Etichetta(ddl_Dosi: MultiColumnComboboxDose_Etichetta[],Dose_Selezionata: MultiColumnComboboxDose_Etichetta){
        let Dosi_Etichetta: MultiColumnComboboxDose_Etichetta[] = [];

        if(Dose_Selezionata){

            Dosi_Etichetta.push(Dose_Selezionata);

            let For_Veg_Av_Dos_Cod_selezionato = Dose_Selezionata.codice;
            let Udm_Cod_selezionata = ! Dose_Selezionata?.Udm?.codice ? 0 : Dose_Selezionata.Udm.codice;
            let FormulatiXAllegatiNormative_IDRiga_Selezionato = ! Dose_Selezionata?.FormulatiXAllegatiNormative_IDRiga ? 0 : Dose_Selezionata.FormulatiXAllegatiNormative_IDRiga;
            let Da_Epoca_selezionata = ! Dose_Selezionata?.Da_Epoca ? "" : Dose_Selezionata.Da_Epoca;
            let A_Epoca_selezionata = ! Dose_Selezionata?.A_Epoca ? "" : Dose_Selezionata.A_Epoca;

            if(ddl_Dosi && ddl_Dosi.length > 0){
                ddl_Dosi.forEach(d=>{

                    let For_Veg_Av_Dos_Cod = d.codice;
                    let Udm_Cod = ! d?.Udm?.codice ? 0 : d.Udm.codice;
                    let FormulatiXAllegatiNormative_IDRiga = ! d?.FormulatiXAllegatiNormative_IDRiga ? 0 : d.FormulatiXAllegatiNormative_IDRiga;
                    let Da_Epoca = ! d?.Da_Epoca ? "" : d.Da_Epoca;
                    let A_Epoca = ! d?.A_Epoca ? "" : d.A_Epoca;



                    if(For_Veg_Av_Dos_Cod !== For_Veg_Av_Dos_Cod_selezionato &&
                        (Udm_Cod_selezionata !== Udm_Cod) &&
                        (FormulatiXAllegatiNormative_IDRiga_Selezionato === FormulatiXAllegatiNormative_IDRiga || FormulatiXAllegatiNormative_IDRiga === 0) &&
                        (Da_Epoca_selezionata === Da_Epoca || Da_Epoca === "0") &&
                        (A_Epoca_selezionata === A_Epoca || A_Epoca === "0") &&
                        (this.unitadimisuraservice.ScomponiUdm(d.Udm).perHa_hl === 2123)) {

                        Dosi_Etichetta.push(d);

                    }
                });
            }

        }

        return Dosi_Etichetta;
    }

    public getMagazzino_Agenzia(rilevamentoMagazzino: RilevamentoDiMagazzino[]){

        let Magazzino_Agenzia: Fabbricato = null;

        if(rilevamentoMagazzino &&
            rilevamentoMagazzino.length === 1){

            if(this.qdcservice.IsMagazzinoAgenzia(rilevamentoMagazzino[0].Magazzino)){
                Magazzino_Agenzia = rilevamentoMagazzino[0].Magazzino;
            }

            if(!Magazzino_Agenzia){
                if(this.qdcservice.IsMagazzinoAgenzia(rilevamentoMagazzino[0].Agenzia)){
                    Magazzino_Agenzia = rilevamentoMagazzino[0].Agenzia;
                }
            }

        }

        return Magazzino_Agenzia;
    }

}
