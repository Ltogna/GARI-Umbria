import {Injectable, Optional} from "@angular/core";
import {TranslocoService} from "@jsverse/transloco";
import {FERTILIZZANTI, FORMULATI, INSETTI, SEMENTI} from 'app/Model/CostantiPersonalizzate';
import {AgendaService, Controllo_Inserimento_Dose_Prodotto} from "app/Service/Agenda/Agenda.service";
import {enum_ErroreGias_Tipo, ErroreGias, ErroreGias_Severity, RispostaStandard} from "app/Service/master.service";
import {GridImpiantiService} from "../grid-impianti/grid-impianti.service";
import {QdCProdottiService} from "../prodotti.service";
import {QdCService} from "../qdc.service";
import {QdCFormToAttivitaService} from "../quaderno-di-campagna-form/quaderno-di-campagna-form-to-attivita.service";
import {Lavorazione} from "../../../../Model/attivita/Lavorazione";
import {enum_LAVCOD, enum_PUARegolamenti_Tipo, enum_TipoOperazioneDB} from "../../../../Model/TipiEnumerativi";
import {FormGroup} from "@angular/forms";
import {
  Acqua,
  CodiciXOperazione,
  Key_Parametri_Aggiuntivi,
  Key_Parametri_Aggiuntivi_Attivita,
  Key_Parametri_Aggiuntivi_ControlloDosi,
  Parametri_Aggiuntivi_ControllaDosi,
  Sezione_Prodotto_Sementi
} from '../../quaderno-di-campagna-form/quaderno-di-campagna-form.model';
import {Fabbricato} from "../../../../Model/anagrafiche/Fabbricato";
import {AvversitaGruppo} from "../../../../Model/metaschema/avversita/AvversitaGruppo";
import {QdCDettagliFormulatiService} from "../prodotti/dettagli-formulati.service";
import {QdCFertilizzantiService} from "../prodotti/fertilizzanti.service";
import {RowGridProdottiDaTrattare} from "app/Service/api.service";
import {ProdottoDaTrattareCDC} from "app/Model/attivita/centri_di_costo/ProdottoDaTrattareCDC";
import {cloneDeep} from "lodash";
import {RibaltamentoTypes} from "../../../../menu-agenda/components/utils";
import {Attivita} from "../../../../Model/attivita/Attivita";
import {QdCFormulatiService} from "../prodotti/formulati.service";


@Injectable()

export class GridDosiProdottiControlliService{
    constructor(private prodottiservice: QdCProdottiService,
                @Optional() private qdcdettagliformulatiservice: QdCDettagliFormulatiService,
                @Optional() private qdcfertilizzantiservice: QdCFertilizzantiService,
                @Optional() private qdcformulatiservice: QdCFormulatiService,
                private qdcservice: QdCService,
                private translocoService: TranslocoService,
                private gridimpiantiservice: GridImpiantiService,
                private qdcformtoattivitaservice: QdCFormToAttivitaService,
                private agendaservice: AgendaService){}

    ControllaSeInserireDoseProdotto(gridDosiProdottiRows: Array<any>, RigaDaInserire: any,FormProdotto: FormGroup,Tipo_Operazione_DB: enum_TipoOperazioneDB,DosiProdottiGridrowId: number): Promise<ErroreGias[]>{

        //TODO qui aggiungere controllo che ci deve essere il prodotto selezionato e le dosi valorizzate
        // TODO Ricontrolla tutti i valori:
        // in  nella trattamenti_2.Dosi_Inserisci
        return new Promise<ErroreGias[]>(async (resolve, reject) => {

            let listerroriGias: ErroreGias[] = [];

            if(RigaDaInserire){

                this.ControllaSelezioneProdotto(RigaDaInserire,listerroriGias);

                if(listerroriGias.length === 0)
                    this.Controlli_Per_Categoria_Magazzino(RigaDaInserire,gridDosiProdottiRows,listerroriGias);

                if(listerroriGias.length === 0){

                    let acqua: Acqua = this.qdcservice.AcquaForm.getRawValue();

                    this.Controlla_Acqua_Dose_HL(acqua.Acqua_Ha,RigaDaInserire,true,false,listerroriGias);

                    if(listerroriGias.length === 0){
                        this.Controlla_Acqua_Dose_HL(acqua.Acqua_Tot,RigaDaInserire,true,false,listerroriGias);
                    }
                }


                if(listerroriGias.length === 0)
                    this.Controlla_Dosi(RigaDaInserire,listerroriGias);

                //Controlli in base all'impostazione utente o imprese impostazioni

                if(listerroriGias.length === 0){
                    if(this.prodottiservice.controlli_Giacenze_Lotti.txt_Lotto.obbligatorio &&
                        (!RigaDaInserire.Lotto || RigaDaInserire.Lotto === "")){

                        listerroriGias.push(<ErroreGias>{
                            severity: ErroreGias_Severity.Bloccante,
                            tipo:  enum_ErroreGias_Tipo.Generico,
                            messaggio: this.translocoService.translate('qdc.LottoObbligatorioSecondoImpostazione')
                        });
                    }

                    if(this.prodottiservice.controlli_Giacenze_Lotti.ddl_UdM.obbligatorio &&
                        (!RigaDaInserire.UdM_Indicata || RigaDaInserire.UdM_Indicata.codice === 0)){

                        listerroriGias.push(<ErroreGias>{
                            severity: ErroreGias_Severity.Bloccante,
                            tipo:  enum_ErroreGias_Tipo.Generico,
                            messaggio: this.translocoService.translate('qdc.SelezionareUdM')
                        });
                    }

                    if(this.prodottiservice.controlli_Giacenze_Lotti.ddl_Magazzino.obbligatorio &&
                        (!RigaDaInserire.Magazzino_del_Prodotto_Selezionato || RigaDaInserire.Magazzino_del_Prodotto_Selezionato.primaryKey.codice === 0)){

                        listerroriGias.push(<ErroreGias>{
                            severity: ErroreGias_Severity.Bloccante,
                            tipo:  enum_ErroreGias_Tipo.Generico,
                            messaggio: this.translocoService.translate('qdc.MagazzinobbligatoriaSecondoImpostazione')
                        });
                    }
                }

                if(listerroriGias.length === 0){
                    if(!this.qdcservice.Controlla_Magazzino(RigaDaInserire)){
                        listerroriGias.push(<ErroreGias>{
                            severity: ErroreGias_Severity.Bloccante,
                            tipo:  enum_ErroreGias_Tipo.Generico,
                            messaggio: this.translocoService.translate("qdc.SelezionareUnMagazzinoAzienda",{nomeAzienda: this.qdcservice.objParametriAgenda.RagSoc})
                        });
                    }
                }

                //TODO per ora qui non c'è il vincolo del prodotto con magazzino e lotto diverso
                //ma dovrà essere implementato un nuovo controllo
                /*if(gridDosiProdottiRows && listerroriGias.length === 0){

                    for(const r of gridDosiProdottiRows){
                        if(r.Fr_Cod === RigaDaInserire.Fr_Cod &&
                            r.Piva === RigaDaInserire.Piva &&
                            r.Sa_Cod ===  RigaDaInserire.Sa_Cod &&
                            r.Fabbricato_Cod === RigaDaInserire.Fabbricato_Cod &&
                            r.Piva_Rif === RigaDaInserire.Piva_Rif &&
                            r.Sa_Cod_Rif === RigaDaInserire.Sa_Cod_Rif &&
                            r.Id_Agenda_Rif === RigaDaInserire.Id_Agenda_Rif &&
                            r.Id_Mov_Rif === RigaDaInserire.Id_Mov_Rif &&
                            r.Id_Mov_Det_Rif === RigaDaInserire.Id_Mov_Det_Rif &&
                            r.Lotto === RigaDaInserire.Lotto &&
                            r.DosiProdottiGridrowId !== RigaDaInserire.DosiProdottiGridrowId){

                            if(r.Categoria_Magazzino === FORMULATI){
                                listerroriGias.push(<ErroreGias>{
                                    severity: ErroreGias_Severity.Bloccante,
                                    tipo:  enum_ErroreGias_Tipo.Generico,
                                    messaggio: this.translocoService.translate('NonEConsentitoInserireUnFormulatoGiaPresen')
                                });

                                break;
                            }else if(r.Categoria_Magazzino === FERTILIZZANTI){
                                listerroriGias.push(<ErroreGias>{
                                    severity: ErroreGias_Severity.Bloccante,
                                    tipo:  enum_ErroreGias_Tipo.Generico,
                                    messaggio: this.translocoService.translate('NonEConsentitoInserireUnFertilizzanteGiaPr')
                                });

                                break;
                            }else if(r.Categoria_Magazzino === SEMENTI){
                                listerroriGias.push(<ErroreGias>{
                                    severity: ErroreGias_Severity.Bloccante,
                                    tipo:  enum_ErroreGias_Tipo.Generico,
                                    messaggio: this.translocoService.translate('NonEConsentitoInserireUnaSementePiantinaGiaPresen')
                                });

                                break;
                            }


                        }

                    }
                }*/


                //Controlli lato server
                if(listerroriGias.length === 0){

                    let listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[] = this.setParametriAggiuntivi_Controlla_Dosi(FormProdotto.get("Operazione").getRawValue());

                    listerroriGias = await this.ControlliDoseProdotto_Lato_Server(RigaDaInserire,gridDosiProdottiRows,FormProdotto,Tipo_Operazione_DB,DosiProdottiGridrowId,listParametri_Aggiuntivi_ControllaDosi);

                    resolve(listerroriGias);
                }else{
                    this.qdcservice.gestisci_ErroriGias(listerroriGias,true,true).then((res:any)=>{
                        if(res){
                            switch(res.severity){
                                case ErroreGias_Severity.Bloccante:
                                    resolve(res.list_errori_filtrati);
                                    break;
                                case ErroreGias_Severity.WarningBloccante:
                                    resolve(res.list_errori_filtrati);
                                    break;
                                case ErroreGias_Severity.Warning:
                                    if (res.result.returnObj === true) {
                                        resolve([]);
                                    }else{
                                        resolve(res.list_errori_filtrati);
                                    }
                                    break;
                            }
                        }else{
                            resolve([]);
                        }
                    });
                }
            }

        });

    }

    private  async ControlliDoseProdotto_Lato_Server(row:any,rowsInserite: Array<any>,FormProdotto: FormGroup, Tipo_Operazione_DB: enum_TipoOperazioneDB,DosiProdottiGridrowId: number,listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[]){

        let listerroriGias : ErroreGias[] = [];

        let Controllo_Inserimento_Dose_Lato_Server: Controllo_Inserimento_Dose_Prodotto = this.Componi_Obj_Controllo_Inserimento_Dose_Prodotto(
            row,rowsInserite, FormProdotto, Tipo_Operazione_DB, DosiProdottiGridrowId, listParametri_Aggiuntivi_ControllaDosi
        );

        let risp: RispostaStandard = await this.agendaservice.Controllo_Inserimento_Dose_Prodotto(Controllo_Inserimento_Dose_Lato_Server);

        if(!risp.RispostaOK && risp.ErroriGias.length > 0){
            listerroriGias = await this.gestisciTipoErrore_Warning(
                row, rowsInserite, FormProdotto, Tipo_Operazione_DB, DosiProdottiGridrowId, Controllo_Inserimento_Dose_Lato_Server.parametri_aggiuntivi_list, risp
            );
        }

        return listerroriGias;

    }

    public Componi_Obj_Controllo_Inserimento_Dose_Prodotto(row:any,rowsInserite: Array<any>,FormProdotto: FormGroup, Tipo_Operazione_DB: enum_TipoOperazioneDB,DosiProdottiGridrowId: number,listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[]):Controllo_Inserimento_Dose_Prodotto{

        let Controllo_Inserimento_Dose_Lato_Server = new Controllo_Inserimento_Dose_Prodotto();

        Controllo_Inserimento_Dose_Lato_Server.tipoAttivita = this.qdcservice.TestataForm.get("Tipo").value;
        Controllo_Inserimento_Dose_Lato_Server.statoAttivita = this.qdcservice.TestataForm.get("Stato").value;

        if(Tipo_Operazione_DB === enum_TipoOperazioneDB.Scrittura){
            Controllo_Inserimento_Dose_Lato_Server.DosiProdottiGridrowId = -1;
        }else{
            Controllo_Inserimento_Dose_Lato_Server.DosiProdottiGridrowId = DosiProdottiGridrowId;
        }

        let raccoglitore_cod: number = this.qdcservice.TestataForm.get("Raccoglitore").value;

        //Per il multicentro va bene che prenda il primo id_agenda perchè tanto c'è la lista codici_x_centri
        if(raccoglitore_cod && raccoglitore_cod > 0){
            Controllo_Inserimento_Dose_Lato_Server.id_agenda = 0;
            Controllo_Inserimento_Dose_Lato_Server.raccoglitore_cod = this.qdcservice.TestataForm.get("Raccoglitore").value;
        } else {
            Controllo_Inserimento_Dose_Lato_Server.id_agenda = + (this.qdcservice.TestataForm.get("Codici_Attivita").getRawValue() as Array<CodiciXOperazione>)[0].CodiceAttivita;
            Controllo_Inserimento_Dose_Lato_Server.raccoglitore_cod = 0;
        }

        Controllo_Inserimento_Dose_Lato_Server.DoseConsentitaDiserbo = 0;

        if(row.Categoria_Magazzino === FORMULATI && this.qdcdettagliformulatiservice.doseConsentitaDiserbo && this.qdcdettagliformulatiservice.doseConsentitaDiserbo.D_HA_Max_Diserbo)
            Controllo_Inserimento_Dose_Lato_Server.DoseConsentitaDiserbo = this.qdcdettagliformulatiservice.doseConsentitaDiserbo.D_HA_Max_Diserbo;

        Controllo_Inserimento_Dose_Lato_Server.risorsaAcqua = this.qdcformtoattivitaservice.getRisorsaAcqua(row.Operazione);

        //Scarto le proprietà delle righe impianti che possono avere dell'Html all'interno (showHTMLAsString = true) per evitare che ci siano problemi con la security
        Controllo_Inserimento_Dose_Lato_Server.row_grid_impianti = [];

        if (this.qdcservice.GridImpiantiPublicService != null) {
            let row_grid_Impianti = this.gridimpiantiservice.kGetElementiSelezionati(cloneDeep(this.qdcservice.GridImpiantiPublicService.getValue().data.rows));

            if(row_grid_Impianti && row_grid_Impianti.length > 0)
                Controllo_Inserimento_Dose_Lato_Server.row_grid_impianti = row_grid_Impianti;
        }

        Controllo_Inserimento_Dose_Lato_Server.row_grid_prodottiDaTrattare = [];
        let row_grid_prodottiDaTrattare = this.qdcservice.ProdottiDaTrattareSelezionatiFormArray.value;
        if(row_grid_prodottiDaTrattare && row_grid_prodottiDaTrattare.length > 0){
            Controllo_Inserimento_Dose_Lato_Server.row_grid_prodottiDaTrattare = this.parseProdottiDaTrattare(row_grid_prodottiDaTrattare);
        }

        Controllo_Inserimento_Dose_Lato_Server.row_grid_dosi = rowsInserite;
        Controllo_Inserimento_Dose_Lato_Server.lavorazione =  row.Operazione;
        Controllo_Inserimento_Dose_Lato_Server.row_grid_dosi_altre_operazioni = [];

        //Compongo un 'array con tutte le righe di prodotti salvate nelle altre operazioni
        if(this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().length > 0){
            this.qdcservice.Sezioni_ProdottoFormArray.getRawValue().forEach(s=>{
               if((s.Operazione as Lavorazione).primaryKey.codice !== Controllo_Inserimento_Dose_Lato_Server.lavorazione.primaryKey.codice) {
                   let DosiProdotticonRigheSalvate = this.qdcservice.DosiProdotticonRigheSalvate(null,s.Operazione);

                   if(DosiProdotticonRigheSalvate && DosiProdotticonRigheSalvate.length > 0){
                       DosiProdotticonRigheSalvate.forEach(d=>{
                           Controllo_Inserimento_Dose_Lato_Server.row_grid_dosi_altre_operazioni.push(d);
                       });
                   }
               }
            });
        }

        Controllo_Inserimento_Dose_Lato_Server.unitadiMisura = row.UdM;
        Controllo_Inserimento_Dose_Lato_Server.doseHa = row.Dose_Ha;
        Controllo_Inserimento_Dose_Lato_Server.doseHl = row.Dose_Hl;
        Controllo_Inserimento_Dose_Lato_Server.doseQ = row.Dose_Ha; // WIP post raccolta, bisogna implementare un campo specifico per la sola quantità ettaro
        Controllo_Inserimento_Dose_Lato_Server.quantitaTotale = row.DoseTot_Ha;
        Controllo_Inserimento_Dose_Lato_Server.flagTipoDose = row.flagTipoDose;
        Controllo_Inserimento_Dose_Lato_Server.flagDoseQuantitaTotale = row.flagDoseQuantitaTotale;
        Controllo_Inserimento_Dose_Lato_Server.Magazzino =  (row.Magazzino_del_Prodotto_Selezionato as Fabbricato);
        Controllo_Inserimento_Dose_Lato_Server.Lotto =  row.Lotto;

        Controllo_Inserimento_Dose_Lato_Server.data =  this.qdcservice.TestataForm.get("Data").value;
        Controllo_Inserimento_Dose_Lato_Server.disciplinare = this.qdcservice.getDisciplinareModelValue(+ (row.Operazione as Lavorazione).primaryKey.codice);

        Controllo_Inserimento_Dose_Lato_Server.utilizzoTerreno = this.qdcservice.getUtilizzoTerrenoModel();
        Controllo_Inserimento_Dose_Lato_Server.tipoRicetta = this.qdcservice.TestataForm.get("TipoRicetta").value;
        Controllo_Inserimento_Dose_Lato_Server.sup_Trattata = this.qdcservice.SuperficiForm.get("Sup_Trattata").value;
        Controllo_Inserimento_Dose_Lato_Server.qtaProdotto_Trattata = this.qdcservice.QuantitaForm.get("Qta_Trattata").value;

        //TODO Multicentro capire come fare
        Controllo_Inserimento_Dose_Lato_Server.ricetta_cod = + this.qdcservice.TestataForm.get("Codici_Attivita").getRawValue().find(c=>c.Operazione.primaryKey.codice === row.Operazione.primaryKey.codice).CodiceRicetta;

        //TODO Multicentro capire come fare
        Controllo_Inserimento_Dose_Lato_Server.ricetta_operazione_cod = + this.qdcservice.TestataForm.get("Codici_Attivita").getRawValue().find(c=>c.Operazione.primaryKey.codice === row.Operazione.primaryKey.codice).CodiceOperazioneRicetta;

        Controllo_Inserimento_Dose_Lato_Server.Piva_Rif = row.Piva_Rif;

        Controllo_Inserimento_Dose_Lato_Server.Sa_Cod_Rif = row.Sa_Cod_Rif;

        Controllo_Inserimento_Dose_Lato_Server.ID_Agenda_Rif = row.ID_Agenda_Rif;

        Controllo_Inserimento_Dose_Lato_Server.ID_Mov_Rif = row.ID_Mov_Rif;

        Controllo_Inserimento_Dose_Lato_Server.ID_Mov_Det_Rif = row.ID_Mov_Det_Rif;

        Controllo_Inserimento_Dose_Lato_Server.Lav_Cod_Rif = row.Lav_Cod_Rif;

        Controllo_Inserimento_Dose_Lato_Server.Cau_Mov_Rif = row.Cau_Mov_Rif;

        Controllo_Inserimento_Dose_Lato_Server.Des_Rif = row.Des_Rif;

        Controllo_Inserimento_Dose_Lato_Server.Qta_Rif = row.Qta_Rif;

        Controllo_Inserimento_Dose_Lato_Server.pua =  this.qdcservice.TestataForm.get("Codici_Attivita").getRawValue().find(c=>c.Operazione.primaryKey.codice === row.Operazione.primaryKey.codice).pua;

        Controllo_Inserimento_Dose_Lato_Server.Magazzino_Esterno = row.Magazzino_Esterno;

        switch(row.Categoria_Magazzino){
            case FORMULATI:
                Controllo_Inserimento_Dose_Lato_Server.dettaglioTrattamento = row.Prodotto;
                Controllo_Inserimento_Dose_Lato_Server.avversitaGruppo = (row.Avversita as AvversitaGruppo);
                Controllo_Inserimento_Dose_Lato_Server.soglia_Avversita = row.Soglia_Avversita;
                Controllo_Inserimento_Dose_Lato_Server.dosi_Etichetta = row.Dosi_Etichetta;
                break;
            case FERTILIZZANTI:
                Controllo_Inserimento_Dose_Lato_Server.dettaglioFertilizzazione = row.Prodotto;
                Controllo_Inserimento_Dose_Lato_Server.N = row.N;
                Controllo_Inserimento_Dose_Lato_Server.N_Utile = row.N_Utile;
                Controllo_Inserimento_Dose_Lato_Server.P = row.P;
                Controllo_Inserimento_Dose_Lato_Server.K = row.K;
                Controllo_Inserimento_Dose_Lato_Server.Cu = row.Cu;
                Controllo_Inserimento_Dose_Lato_Server.epocaFertilizzazione = FormProdotto.get("EpocaFertilizzazione").value;
                Controllo_Inserimento_Dose_Lato_Server.efficienza = FormProdotto.get("Efficienza").value;
                break;
            case SEMENTI:
                Controllo_Inserimento_Dose_Lato_Server.dettaglioSemina = row.Prodotto;
                Controllo_Inserimento_Dose_Lato_Server.sup_Calcolata = row.Sup_Calcolata;
                Controllo_Inserimento_Dose_Lato_Server.tipo_Semina = this.qdcservice.getOpzione_Semina_Model(row.Operazione);
                break;
            case INSETTI:
                Controllo_Inserimento_Dose_Lato_Server.dettaglioTrattamento = row.Prodotto;
                Controllo_Inserimento_Dose_Lato_Server.avversitaGruppo = (row.Avversita as AvversitaGruppo);
                break;
        }

        Controllo_Inserimento_Dose_Lato_Server.parametri_aggiuntivi_list = listParametri_Aggiuntivi_ControllaDosi;

        Controllo_Inserimento_Dose_Lato_Server.Categoria_Magazzino = row.Categoria_Magazzino;

        Controllo_Inserimento_Dose_Lato_Server.impresa = this.qdcservice.getImpresa_Model();

        Controllo_Inserimento_Dose_Lato_Server.Visualizza_Magazzini_Esterni = row.Visualizza_Magazzini_Esterni;

        //Original_Row_Value: Oggetto utilizzato per controllare le differenze tra il prodotto in ribaltamento e quello iniziale del brogliaccio
        if(this.qdcservice.TipoRibaltamento === RibaltamentoTypes.Da_Brogliaccio_ad_Agenda || this.qdcservice.TipoRibaltamento === RibaltamentoTypes.Da_Ricetta_ad_Agenda){
          Controllo_Inserimento_Dose_Lato_Server.Original_Row_Value = row.Original_Row_Value;
        }else{
          Controllo_Inserimento_Dose_Lato_Server.Original_Row_Value = null;
        }

        return Controllo_Inserimento_Dose_Lato_Server;
    }

    private ControllaSelezioneProdotto(RigaDaInserire: any,listerroriGias: ErroreGias[]){

        //Replica il ControllaSelezioneFormulato(), il Resources.AgronicaAgenda_2010.SelezionareUnFertilizzante e il Resources.AgronicaAgenda_2010.SelezionareUnaSementePiantina della Trattamenti_2
        if(!RigaDaInserire.Prodotto || RigaDaInserire.Prodotto.prodotto.codice === 0 || (RigaDaInserire.Fr_Cod === 0 && RigaDaInserire.Fr_Des === "")){

            switch(RigaDaInserire.Categoria_Magazzino){
                case FORMULATI:
                    listerroriGias.push(<ErroreGias>{
                        severity: ErroreGias_Severity.Bloccante,
                        tipo:  enum_ErroreGias_Tipo.Generico,
                        messaggio: this.translocoService.translate('SelezionareUnFormulato')
                    });
                    break;
                case FERTILIZZANTI:
                    listerroriGias.push(<ErroreGias>{
                        severity: ErroreGias_Severity.Bloccante,
                        tipo:  enum_ErroreGias_Tipo.Generico,
                        messaggio: this.translocoService.translate('SelezionareUnFertilizzante')
                    });
                    break;
                case SEMENTI:
                    listerroriGias.push(<ErroreGias>{
                        severity: ErroreGias_Severity.Bloccante,
                        tipo:  enum_ErroreGias_Tipo.Generico,
                        messaggio: this.translocoService.translate('SelezionareUnaSementePiantina')
                    });
                    break;
                case INSETTI:
                    listerroriGias.push(<ErroreGias>{
                        severity: ErroreGias_Severity.Bloccante,
                        tipo:  enum_ErroreGias_Tipo.Generico,
                        messaggio: this.translocoService.translate('SelezionareUnInsetto')
                    });
                    break;

            }
        }


        if(listerroriGias.length === 0){
            for(let s of this.qdcservice.Sezioni_ProdottoFormArray.getRawValue()){
                if(s.Operazione.primaryKey.codice !== RigaDaInserire.Operazione.primaryKey.codice){
                    for(let d of s.DosiProdotti){
                        if(d.Fr_Cod === RigaDaInserire.Fr_Cod && d.Categoria_Magazzino === RigaDaInserire.Categoria_Magazzino){
                            listerroriGias.push(<ErroreGias>{
                                severity: ErroreGias_Severity.Bloccante,
                                tipo:  enum_ErroreGias_Tipo.Generico,
                                messaggio: this.translocoService.translate('NonePossibileInserireloStessoProdottoinSezDiverse')
                            });
                            return listerroriGias;
                        }
                    }
                }
            }
        }

        return listerroriGias;

    }

    private Controlla_Dosi(RigaDaInserire: any,listerroriGias: ErroreGias[]){

        this.Controlla_Acqua_Dose_HL(RigaDaInserire.Dose_Hl,RigaDaInserire,false,true,listerroriGias);

        if(listerroriGias.length > 0){
            return;
        }

        //controllo dose
        if(RigaDaInserire.Dose_Ha <= 0){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaDoseNullaONegativa')
            });

            return;
        }



        //controlli generici sulla dose totale
        if(RigaDaInserire.DoseTot_Ha <= 0){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaDoseTotaleNullaO')
            });

            return;
        }

    }

    private Controlla_Acqua_Dose_HL(value: number,RigaDaInserire: any,flag_acqua: boolean,flag_dose_hl: boolean,listerroriGias: ErroreGias[]){

        let messaggio: string = "";

        let lav_cod = + RigaDaInserire.Operazione.primaryKey.codice;

        //CONTROLLO OBBLIGO ACQUA
        switch(lav_cod){
            case enum_LAVCOD.CONCIMAZIONE_FOGLIARE:
            case enum_LAVCOD.FERTIRRIGAZIONE:
            case enum_LAVCOD.TRATTAMENTO_ANTIBUTTERATURA:
                if(value <= 0){

                    if(flag_acqua){
                        messaggio = this.translocoService.translate('InserireUnValoreNumericoPerIndicareLaBQuan');
                    }else if(flag_dose_hl){
                        messaggio = this.translocoService.translate('NonEPossibileInserireUnaDoseNullaONegativa');
                    }

                    listerroriGias.push(<ErroreGias>{
                        severity: ErroreGias_Severity.Bloccante,
                        tipo:  enum_ErroreGias_Tipo.Generico,
                        messaggio: messaggio
                    });

                    return;
                }
                break;
            case enum_LAVCOD.TRATTAMENTO_ANTIPARASSITARIO:
            case enum_LAVCOD.DISERBO:
            case enum_LAVCOD.DISSECCAMENTO:
            case enum_LAVCOD.TRATTAMENTO_FITOREGOLATORE:
                //Per ora il flag polverulento viene gestito solamente lato server

                /*if((RigaDaInserire.Prodotto as MultiColumnComboboxTrattamento).polverulento){
                    if(value !== 0){
                        listerroriGias.push(<ErroreGias>{
                            severity: ErroreGias_Severity.Bloccante,
                            tipo:  enum_ErroreGias_Tipo.Generico,
                            messaggio: this.translocoService.translate('ImpossibileAcquaPolverulenti')
                        });

                        return;
                    }
                }else{
                    if(value <= 0){

                        if(flag_acqua){
                            messaggio = this.translocoService.translate('InserireUnValoreNumericoPerIndicareLaBQuan');
                        }else if(flag_dose_hl){
                            messaggio = this.translocoService.translate('NonEPossibileInserireUnaDoseNullaONegativa');
                        }

                        listerroriGias.push(<ErroreGias>{
                            severity: ErroreGias_Severity.Bloccante,
                            tipo:  enum_ErroreGias_Tipo.Generico,
                            messaggio: messaggio
                        });

                        return;
                    }

                    return;
                }
                break;*/
            default:
                if(value < 0){

                    if(flag_acqua){
                        messaggio = this.translocoService.translate('InserireUnValoreNumericoPerIndicareLaBQuan');
                    }else if(flag_dose_hl){
                        messaggio = this.translocoService.translate('NonEPossibileInserireUnaDoseNullaONegativa');
                    }

                    listerroriGias.push(<ErroreGias>{
                        severity: ErroreGias_Severity.Bloccante,
                        tipo:  enum_ErroreGias_Tipo.Generico,
                        messaggio: messaggio
                    });

                    return;
                }
                break;
        }
    }

    private Controlli_Per_Categoria_Magazzino(RigaDaInserire: any, RigheGiaInserite: Array<any>,listerroriGias: ErroreGias[]){

        let elem_cod = RigaDaInserire.Categoria_Magazzino;

        let lav_cod = + RigaDaInserire.Operazione.primaryKey.codice;

        switch(elem_cod){
            case FORMULATI:
                this.Controlli_Formulati(RigaDaInserire,listerroriGias);
                break;
            case FERTILIZZANTI:
                this.Controlli_Fertilizzanti(RigaDaInserire,listerroriGias);
                break;
            case SEMENTI:
                this.Controlli_Sementi(RigaDaInserire,RigheGiaInserite,listerroriGias);
                break;
        }

    }

    private Controlli_Fertilizzanti(RigaDaInserire: any,listerroriGias: ErroreGias[]){

        if(RigaDaInserire.Efficienza < 0 || RigaDaInserire.Efficienza > 1){

            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('InserireUnValoreDiEfficienzaIncluso01')
            });

            return;

        }

        //---------------
        //N - P - K - Cu
        //---------------
        if(RigaDaInserire.N < 0 ){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaQuantitaDiNNegativ')
            });

            return;

        }

        if(RigaDaInserire.N_Utile < 0 ){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaQuantitaDiNUtileNe')
            });

            return;

        }

        if(RigaDaInserire.P < 0 ){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaQuantitaDiP2O5Nega')
            });

            return;

        }

        if(RigaDaInserire.K < 0 ){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaQuantitaDiK2ONegat')
            });

            return;

        }

        if(RigaDaInserire.Cu < 0 ){
            listerroriGias.push(<ErroreGias>{
                severity: ErroreGias_Severity.Bloccante,
                tipo:  enum_ErroreGias_Tipo.Generico,
                messaggio: this.translocoService.translate('NonEPossibileInserireUnaQuantitaDiCuNegat')
            });

            return;

        }

        this.Controlla_EpocaFertilizzazione(RigaDaInserire,listerroriGias);

        if(listerroriGias.length > 0)
            return;

    }


    private Controlli_Sementi(RigaDaInserire: any,RigheGiaInserite: Array<any>,listerroriGias: ErroreGias[]){

        let Opzione_Semina = this.qdcservice.getOpzione_Semina_Model(RigaDaInserire.Operazione);

        this.qdcservice.Controllo_Sup_Calcolata(Opzione_Semina,RigaDaInserire,RigheGiaInserite,listerroriGias);

    }

    private Controlla_EpocaFertilizzazione(RigaDaInserire: any,listerroriGias: ErroreGias[]){

        //Non Aggiungere riga vuota solo per DISTRIBUZIONE_AMMENDANTI e se il disciplinare scelto ha una direttiva nitrati
        //(RegolamentoConcimazione.tipo = 2) perchè l'Epoca è obbligatoria.
        //Negli altri casi la Epoca di Fertlizzazione non è obbligatoria e aggiungo la riga vuota nella ddl.

      if(this.qdcfertilizzantiservice.ObbligatoryEpocaFertilizzazione(RigaDaInserire.Operazione)){
        if(!RigaDaInserire.EpocaFertilizzazione ||
          RigaDaInserire.EpocaFertilizzazione.codice === 0){

          listerroriGias.push(<ErroreGias>{
            severity: ErroreGias_Severity.Bloccante,
            tipo:  enum_ErroreGias_Tipo.Generico,
            messaggio: this.translocoService.translate("qdc.SelezionareUnEpoca")
          });
        }

      }

    }



    private gestisciTipoErrore_Warning(row:any,rowsInserite: Array<any>,FormProdotto: FormGroup, Tipo_Operazione_DB: enum_TipoOperazioneDB,
                                             DosiProdottiGridrowId: number,listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[], rispostaStandard: RispostaStandard): Promise<ErroreGias[]>{

        return new Promise <ErroreGias[]>(async (resolve,reject)=>{
            let listErroriGias_filtrati: ErroreGias[] = [];

            let res = await this.qdcservice.gestisci_ErroriGias(rispostaStandard.ErroriGias,true,false);

            if(res){
                switch(res.severity){
                    case ErroreGias_Severity.WarningBloccante:
                        listErroriGias_filtrati = res.list_errori_filtrati;
                        break;
                    case ErroreGias_Severity.Bloccante:
                        listErroriGias_filtrati = res.list_errori_filtrati;
                        break;
                    case ErroreGias_Severity.Warning:
                        //Se l'utente ha cliccato su Sì allora eseguo il salvataggio saltando il warning a cui ha detto sì
                        if (res.result.returnObj === true) {

                            this.change_lista_MostraWarning(res.list_errori_filtrati,listParametri_Aggiuntivi_ControllaDosi);

                            listErroriGias_filtrati = await this.ControlliDoseProdotto_Lato_Server(row,rowsInserite,FormProdotto,Tipo_Operazione_DB,DosiProdottiGridrowId,listParametri_Aggiuntivi_ControllaDosi);

                        }else{
                            listErroriGias_filtrati = res.list_errori_filtrati;
                        }
                        break;
                }
            }

            resolve(listErroriGias_filtrati);
        });


    }

    private setParametriAggiuntivi_Controlla_Dosi(Operazione: Lavorazione): Parametri_Aggiuntivi_ControllaDosi[]{

        let listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[] = [];

        let codici_Attivita_x_CentriAziendali = this.qdcformtoattivitaservice.setParametriAggiuntivi_X_MultiCentro();

        if(codici_Attivita_x_CentriAziendali && codici_Attivita_x_CentriAziendali.length > 0){
            listParametri_Aggiuntivi_ControllaDosi.push({
                key: Key_Parametri_Aggiuntivi.lista_Codici_Attivita_x_CentriAziendali,
                value: JSON.stringify(codici_Attivita_x_CentriAziendali)
            });
        }

        let SezioneProdottoForm = this.qdcservice.Sezione_ProdottoFormGroup(Operazione);

        if(SezioneProdottoForm){

            let SezioneProdottoFormValue = SezioneProdottoForm.getRawValue();

            switch(SezioneProdottoFormValue.Categoria_Magazzino) {
                case FORMULATI:
                    break;
                case FERTILIZZANTI:
                    break;
                case SEMENTI:

                    SezioneProdottoFormValue = SezioneProdottoFormValue as Sezione_Prodotto_Sementi;

                    if (SezioneProdottoFormValue.Opzioni_Semina) {

                        listParametri_Aggiuntivi_ControllaDosi.push({
                            key: Key_Parametri_Aggiuntivi_Attivita.Opzione_Semina,
                            value: !SezioneProdottoFormValue.Opzioni_Semina ? "0" : SezioneProdottoFormValue.Opzioni_Semina.codice.toString()
                        });
                    }
            }
        }

        this.set_lista_MostraWarning(listParametri_Aggiuntivi_ControllaDosi);

        return listParametri_Aggiuntivi_ControllaDosi;
    }

    private set_lista_MostraWarning(listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[]){

        listParametri_Aggiuntivi_ControllaDosi.push({
            key: Key_Parametri_Aggiuntivi_ControlloDosi.mostraWarning_CheckGiacenza,
            value: "true"
        });

        listParametri_Aggiuntivi_ControllaDosi.push({
            key: Key_Parametri_Aggiuntivi_ControlloDosi.mostraWarning_CheckMassimali,
            value: "true"
        });

        listParametri_Aggiuntivi_ControllaDosi.push({
            key: Key_Parametri_Aggiuntivi_ControlloDosi.mostraWarning_CheckEtichetta,
            value: "true"
        });

        listParametri_Aggiuntivi_ControllaDosi.push({
          key: Key_Parametri_Aggiuntivi_ControlloDosi.mostraWarning_CheckProdottoInRibaltamento,
          value: "true"
        });


      return listParametri_Aggiuntivi_ControllaDosi;

    }

    private change_lista_MostraWarning(list_warning: ErroreGias[],listParametri_Aggiuntivi_ControllaDosi: Parametri_Aggiuntivi_ControllaDosi[]){

        let distinct_warning: ErroreGias[] = [...new Map(list_warning.map(item => [item["ex"], item])).values()];

        if(distinct_warning && listParametri_Aggiuntivi_ControllaDosi && listParametri_Aggiuntivi_ControllaDosi.length > 0){

            for(let v of listParametri_Aggiuntivi_ControllaDosi){
                for(let w of distinct_warning){
                    if(v.key === w.ex){
                        v.value = "false";
                    }
                }
            }
        }

    }

    private parseProdottiDaTrattare(prodotti: ProdottoDaTrattareCDC[]): RowGridProdottiDaTrattare[] {
        const result: RowGridProdottiDaTrattare[] = [];
        for (const prodotto of prodotti) {
            const mag = prodotto.giacenzaMagazzino.Magazzino;
            const prod = prodotto.giacenzaMagazzino.Prodotto;
            result.push({
                PIVA: mag.primaryKey.centroAziendalePK.partitaIva,
                SA_COD: mag.primaryKey.centroAziendalePK.codice,
                FABBRICATO_COD: mag.primaryKey.codice,
                Mat_Cod: prod.codice,
                Elem_Cod: prod.elemCod,
                Codice_Alfanumerico: prod.codice_alfanumerico,
                Prodotto: prod.descrizione,
                Progetto_Cod: prodotto.giacenzaMagazzino.codice_progetto,
                Lotto: prodotto.giacenzaMagazzino.Lotto,
                QtaProdotto: prodotto.giacenzaMagazzino.Qta
            });
        }

        return result;
    }

    private Controlli_Formulati(RigaDaInserire: any,listerroriGias: ErroreGias[]){
      this.qdcformulatiservice.Controlla_EpocaDPI(RigaDaInserire.EpocaDPI,RigaDaInserire.Operazione,listerroriGias);

      if(listerroriGias.length > 0)
        return;
    }
}
