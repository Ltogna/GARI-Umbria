using AgronicaCoreDTOStd.InData;
using AgronicaCoreModelsSTD.attivita.dettagli;
using AgronicaNetCore.Anagrafe.DAL.DataLayer.Imprese_Impostazioni;
using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Farmaci;
using AgronicaNetCore.Operazione.DAL.DataLayer.Agenda;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioni;
using InData.Anagrafica;
using InData.Zoo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AgronicaCoreModelsSTD.attivita.Attivita;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;

namespace AgronicaNetCore.Operazione.BIZ.Services.Agenda
{
    public class MagazzinoService : BaseService, IMagazzinoService
    {
        private readonly IUtentiImpostazioni _utentiImpostazioni;
        private readonly IImprese_Impostazioni _ImpreseImpostazioni;
        private readonly IMagazzino _MagazzinoDAL;
        private readonly IFarmaci _FarmaciDAL;

        public MagazzinoService(IServiceProvider provider) : base(provider)
        {
            _utentiImpostazioni = provider.GetRequiredService<IUtentiImpostazioni>();
            _ImpreseImpostazioni = provider.GetRequiredService<IImprese_Impostazioni>();
            _MagazzinoDAL = provider.GetRequiredService<IMagazzino>();
            _FarmaciDAL = provider.GetRequiredService<IFarmaci>();
        }

        public async Task<List<DettaglioRegistroSomministrazioni>> Leggi_Giacenze_Farmaci(
            LeggiGiacenzaFarmaci paramsFarmaci,
            AgronicaCoreModelsSTD.attivita.Attivita.Tipo_Attivita tipoAttivita,
            AgronicaCoreModelsSTD.attivita.Attivita.Stati statoAttivita,
            bool escludiGiacenzeZero,
            List<string> codiciAIC,
            AgronicaCoreParametri objParametriSuperServer,
            AgronicaCoreParametri objParametriServer,
            AgronicaCoreParametri objParametriUtenti)
        {
            List<AgronicaCoreModelsSTD.attivita.dettagli.DettaglioRegistroSomministrazioni> farmaciList = new List<AgronicaCoreModelsSTD.attivita.dettagli.DettaglioRegistroSomministrazioni>();


            // DT:     bisognerebbe prendere i centri dei fabbricati, non quelli degli impianti.
            // ma si è valutato di fermarsi a livello di super user o azienda, quindi per il momento è sufficiente passare la PIVA 
            string str_gestioneMagazzino = await _ImpreseImpostazioni.LeggiScalareMulticentroAziendaSuperUser_ElemCod(paramsFarmaci.Piva, null, Enum_Impostazioni_Utenti.SUPERUSER_GESTIONE_MAGAZZINO_ABILITATA, ELEM_COD.FARMACI, "1", objParametriUtenti, objParametriServer);
            string str_gestioneGiacenze = await _ImpreseImpostazioni.LeggiScalareMulticentroAziendaSuperUser_ElemCod(paramsFarmaci.Piva, null, Enum_Impostazioni_Utenti.UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_GIACENZE, ELEM_COD.FARMACI, ((int)enum_Gestione_Giacenze.TuttiProdotti).ToString(), objParametriUtenti, objParametriServer);
            string str_gestioneLotto = await _ImpreseImpostazioni.LeggiScalareMulticentroAziendaSuperUser_ElemCod(paramsFarmaci.Piva, null, Enum_Impostazioni_Utenti.UTENTE_COD_FILTRO_CATEGORIAMAGAZZINO_LOTTI, ELEM_COD.FARMACI, ((int)enum_Gestione_Lotti.Nessuna).ToString(), objParametriUtenti, objParametriServer);
            var bloccaGiacenze_Utente = await _utentiImpostazioni.LeggiConDefault(Enum_Impostazioni_Utenti.UTENTE_COD_BLOCCA_SE_SUPERA_GIACENZE, Username_1Utente_o_2SuperUser: 1, "0", objParametriUtenti);

            int gestioneMagazzino = Convert.ToInt32(str_gestioneMagazzino);
            int gestioneGiacenze = Convert.ToInt32(str_gestioneGiacenze);
            int gestioneLotto = Convert.ToInt32(str_gestioneLotto);

            bool usaLotto = false, usaMagazzino = false, usaAnagrafica = false, flagQtaMaggioreZero = false;
            GetAssettoMagazzino(tipoAttivita, statoAttivita, (enum_Gestione_Lotti)gestioneLotto, gestioneMagazzino, (enum_Gestione_Giacenze)gestioneGiacenze, bloccaGiacenze_Utente, escludiGiacenzeZero, ref usaLotto, ref usaMagazzino, ref usaAnagrafica, ref flagQtaMaggioreZero);

            // Se utilizzo il magazzino, leggo prima i prodotti in giacenza
            DataTable Dt_Giacenze = new DataTable();
            DataTable Dt_Giacenze_Tot = new DataTable();

            string strFiltroFarmCod = "";

            if (usaMagazzino)
            {

                //string xFiltroAggiuntivo_MagazzinoAttivoAllaData = " AND (Fabbricati.Validita_Inizio <= " + Agro_SQL_SaveDate(validitaFine) + " AND Fabbricati.Validita_Fine >= " + Agro_SQL_SaveDate(validitaFine) + ") " + " AND Fabbricati.CodiceBDN = '" + Agro_SQL_SaveText(codiceBDN) + "' AND Fabbricati.ChkMagazzinoFarmaci = 1 AND Fabbricati.ProprietarioCapi = '" + Agro_SQL_SaveText(codFiscaleProprietario) + "' ";
                FiltroAggiuntivo xFiltroAggiuntivo_MagazzinoAttivoAllaData = new FiltroAggiuntivo
                {
                    defaultBooleanOp = Boolean_Operators.And,
                    filters = {
                        new Filter("Fabbricati.CodiceBDN", "@CodiceBDN", Comparison_Operators.Equal, Boolean_Operators.And, paramsFarmaci.codiceBDN, typeof(string)),
                        new Filter("Fabbricati.ChkMagazzinoFarmaci", "@chkMagazzinoFarmaci", Comparison_Operators.Equal, Boolean_Operators.And, 1, typeof(int)),
                    }
                };

                if (paramsFarmaci.codFiscaleProprietario != null && paramsFarmaci.codFiscaleProprietario != "")
                {
                    xFiltroAggiuntivo_MagazzinoAttivoAllaData.filters.Add(new Filter("Fabbricati.ProprietarioCapi", "@codFiscaleProprietario", Comparison_Operators.Equal, Boolean_Operators.And, paramsFarmaci.codFiscaleProprietario, typeof(string)));
                }

                Dt_Giacenze = await _MagazzinoDAL.SchedaGiacenzeMagazzino(paramsFarmaci.ValiditaFine, paramsFarmaci.Piva, paramsFarmaci.SaCod, 0, ELEM_COD.FARMACI, paramsFarmaci.ProCod, 0, 0, 0, 0, 0, COSTANTI_GENERALI.LOTTO_NONDEFINITO, Flag_QtaNoZero: false, null, null, null, null, null, null, null, null, null, null, null, null, "", objParametriServer, objParametriUtenti, xFiltroAggiuntivo_16: xFiltroAggiuntivo_MagazzinoAttivoAllaData, Flag_QtaMaggioreZero: flagQtaMaggioreZero);

                Dt_Giacenze_Tot = await _MagazzinoDAL.SchedaGiacenzeMagazzino(DateTime.Parse(CostantiPersonalizzate.AGRODATAFINE), paramsFarmaci.Piva, paramsFarmaci.SaCod, 0, ELEM_COD.FARMACI, 0, 0, 0, 0, 0, 0, COSTANTI_GENERALI.LOTTO_NONDEFINITO, Flag_QtaNoZero: false, null, null, null, null, null, null, null, null, null, null, null, null, "", objParametriServer, objParametriUtenti, xFiltroAggiuntivo_16: xFiltroAggiuntivo_MagazzinoAttivoAllaData, Flag_QtaMaggioreZero: flagQtaMaggioreZero);

                for (var i = 0; i <= Dt_Giacenze.Rows.Count - 1; i++)
                    strFiltroFarmCod += " Farmaci.Farm_Cod = " + Dt_Giacenze.Rows[i]["Pro_Cod"].ToString() + " OR ";

                if (strFiltroFarmCod != "")
                {
                    strFiltroFarmCod = Strings.Left(strFiltroFarmCod, strFiltroFarmCod.Length - 3);
                    strFiltroFarmCod = " AND (" + strFiltroFarmCod + ")";
                }
                else
                    // se non ho alcun farmaco in magazzino e non si vogliono prodotti presenti solo in anagrafica, si esce
                    if (!usaAnagrafica)
                    return farmaciList;
            }

            DataTable DtRisultati = new DataTable();

            DtRisultati = await _FarmaciDAL.LeggiFarmaciListAICAsync(0, null, codiciAIC, objParametriServer);

            var DtFarmaciOrdinati = new DataTable();
            DtFarmaciOrdinati.Columns.Add(new DataColumn("FarmDes", typeof(string)));
            DtFarmaciOrdinati.Columns.Add(new DataColumn("DettaglioSomministrazione", typeof(AgronicaCoreModelsSTD.attivita.dettagli.DettaglioRegistroSomministrazioni)));
            DtFarmaciOrdinati.Columns.Add(new DataColumn("ConGiacenza", typeof(int)));


            DataRow Dr;
            DataRow[] DrGiacenze = { };
            DataRow[] DrGiacenze_Tot = { };


            foreach (DataRow farmaco in DtRisultati.Rows)
            {
                int Farm_Cod = (int)farmaco["Farm_Cod"];
                string Farm_Des = (string)farmaco["Denominazione"] + " - " + farmaco["Confezione"];
                string codice_AIC = (string)farmaco["AIC"];

                var dettaglioSomministrazione = new AgronicaCoreModelsSTD.attivita.dettagli.DettaglioRegistroSomministrazioni();
                dettaglioSomministrazione.codiceAIC = codice_AIC;
                dettaglioSomministrazione.MagazziniMovimentazioni = new List<AgronicaCoreModelsSTD.attivita.RilevamentoDiMagazzino>();
                dettaglioSomministrazione.prodotto = new AgronicaCoreModelsSTD.attivita.risorse.Prodotto(Farm_Cod, ELEM_COD.FARMACI);
                dettaglioSomministrazione.prodotto.descrizione = Farm_Des;

                dettaglioSomministrazione.dataPrescrizione = DateTime.Parse(CostantiPersonalizzate.AGRODATAINIZIO);

                bool trovataGiacenza = false;
                if (usaMagazzino)
                {
                    if (Dt_Giacenze != null && Dt_Giacenze.Rows.Count > 0)
                        DrGiacenze = Dt_Giacenze.Select("Pro_Cod = " + Farm_Cod);

                    if (DrGiacenze != null && DrGiacenze.Length > 0)
                    {
                        for (var g = 0; g <= DrGiacenze.Length - 1; g++)
                        {
                            AgronicaCoreModelsSTD.attivita.RilevamentoDiMagazzino rilevamentoMagazzino = new AgronicaCoreModelsSTD.attivita.RilevamentoDiMagazzino();
                            rilevamentoMagazzino.Qta = Math.Round((decimal)DrGiacenze[g]["Giacenza"], 4);
                            rilevamentoMagazzino.udm = new AgronicaCoreModelsSTD.metaschema.UnitaDiMisura();
                            rilevamentoMagazzino.udm.codice = (int)DrGiacenze[g]["Udm_Cod"];
                            rilevamentoMagazzino.udm.simbolo = (string)DrGiacenze[g]["Udm_Sim"];
                            rilevamentoMagazzino.Lotto = (string)DrGiacenze[g]["lotto"];
                            rilevamentoMagazzino.Magazzino = new AgronicaCoreModelsSTD.anagrafiche.Fabbricato();
                            rilevamentoMagazzino.Magazzino.primaryKey = new AgronicaCoreModelsSTD.anagrafiche.Fabbricato.PK();
                            rilevamentoMagazzino.Magazzino.primaryKey.centroAziendalePK = new AgronicaCoreModelsSTD.anagrafiche.CentroAziendale.PK();
                            rilevamentoMagazzino.Magazzino.primaryKey.centroAziendalePK.partitaIva = (string)DrGiacenze[g]["Piva"];
                            rilevamentoMagazzino.Magazzino.primaryKey.centroAziendalePK.codice = (int)DrGiacenze[g]["Sa_Cod"];
                            rilevamentoMagazzino.Magazzino.primaryKey.codice = (int)DrGiacenze[g]["Id_Destinazione"];
                            rilevamentoMagazzino.Magazzino.descrizione = (string)DrGiacenze[g]["Fabbricato_Des"] + " (" + (string)DrGiacenze[g]["Sa_Nome"] + ")";
                            rilevamentoMagazzino.Magazzino.tipo = (int)DrGiacenze[g]["Tipo_Destinazione"];

                            dettaglioSomministrazione.unitaDiMisura = rilevamentoMagazzino.udm;

                            if (Dt_Giacenze_Tot != null && Dt_Giacenze_Tot.Rows.Count > 0)
                            {
                                DrGiacenze_Tot = Dt_Giacenze_Tot.Select(" Piva = '" + DrGiacenze[g]["Piva"] + "' and Sa_Cod =" + (int)DrGiacenze[g]["Sa_Cod"] + " and Id_Destinazione=" + (int)DrGiacenze[g]["Id_Destinazione"] + " and lotto='" + (string)DrGiacenze[g]["lotto"] + "'");
                                if (DrGiacenze_Tot != null && DrGiacenze_Tot.Length > 0)
                                {
                                    rilevamentoMagazzino.QtaTot = Math.Round((decimal)DrGiacenze_Tot[0]["Giacenza"], 4);
                                }
                            }

                            var dettaglioTrattamentoGiacenza = dettaglioSomministrazione.Clona();
                            dettaglioTrattamentoGiacenza.MagazziniMovimentazioni.Add(rilevamentoMagazzino);

                            Dr = DtFarmaciOrdinati.NewRow();
                            Dr["FarmDes"] = Farm_Des;
                            Dr["DettaglioSomministrazione"] = dettaglioTrattamentoGiacenza;
                            Dr["ConGiacenza"] = 1;
                            DtFarmaciOrdinati.Rows.Add(Dr);

                            trovataGiacenza = true;
                        }
                    }
                }

                // DT: il prodotto va restituito anche se presente solo in anagrafica se "usaAnagrafica=true"
                // DT: se si sta usando anche il magazzino, si restituisce il prodotto (senza indicazioni di giacenza) solo se non è stato trovato in magazzino (trovataGiacenza=False)
                if (usaAnagrafica && !trovataGiacenza)
                {
                    Dr = DtFarmaciOrdinati.NewRow();
                    Dr["FarmDes"] = Farm_Des;
                    Dr["DettaglioSomministrazione"] = dettaglioSomministrazione;
                    Dr["ConGiacenza"] = 0;
                    DtFarmaciOrdinati.Rows.Add(Dr);
                }
            }

            if (DtFarmaciOrdinati != null)
            {
                DataView Dv = new DataView();
                DtFarmaciOrdinati.TableName = "Farmaci";
                Dv.Table = DtFarmaciOrdinati;
                Dv.Sort = "ConGiacenza DESC, FarmDes ASC";

                for (var i = 0; i <= Dv.Count - 1; i++)
                {
                    farmaciList.Add((AgronicaCoreModelsSTD.attivita.dettagli.DettaglioRegistroSomministrazioni)Dv[i]["DettaglioSomministrazione"]);
                }
            }

            return farmaciList;
        }

        public static void GetAssettoMagazzino(AgronicaCoreModelsSTD.attivita.Attivita.Tipo_Attivita tipoAttivita,
            AgronicaCoreModelsSTD.attivita.Attivita.Stati statoAttivita,
            enum_Gestione_Lotti gestioneLotto,
            int gestioneMagazzino,
            enum_Gestione_Giacenze gestioneGiacenze,
            string bloccaGiacenze_Utente,
            bool escludiGiacenzeZero,
            ref bool usaLotto,
            ref bool usaMagazzino,
            ref bool usaAnagrafica,
            ref bool flagQtaMaggioreZero)
        {
            usaLotto = false;
            usaMagazzino = false;
            usaAnagrafica = false;
            flagQtaMaggioreZero = false;

            switch (gestioneLotto)
            {
                case enum_Gestione_Lotti.Nessuna:
                    {
                        usaLotto = false;
                        break;
                    }

                default:
                    {
                        usaLotto = true;
                        break;
                    }
            }

            enum_Tipo_Operazione_Agenda tipoOperazione = getTipoOperazione(tipoAttivita, statoAttivita);
            switch (tipoOperazione)
            {
                case enum_Tipo_Operazione_Agenda.QuadernoDiCampagna:
                case enum_Tipo_Operazione_Agenda.RicettaBrogliaccio:
                    {
                        switch (gestioneMagazzino)
                        {
                            case 0:
                                {
                                    usaMagazzino = false;
                                    usaAnagrafica = true;
                                    flagQtaMaggioreZero = false;
                                    break;
                                }

                            case 1:
                                {
                                    usaMagazzino = true;

                                    switch (gestioneGiacenze)
                                    {
                                        case enum_Gestione_Giacenze.SoloMovimentati:
                                            {
                                                usaAnagrafica = false;
                                                flagQtaMaggioreZero = false;

                                                if (escludiGiacenzeZero)
                                                    flagQtaMaggioreZero = true;
                                                break;
                                            }

                                        case enum_Gestione_Giacenze.SoloPresenti:
                                            {
                                                usaAnagrafica = false;
                                                flagQtaMaggioreZero = true;
                                                break;
                                            }

                                        case enum_Gestione_Giacenze.TuttiProdotti:
                                            {
                                                usaAnagrafica = true;
                                                flagQtaMaggioreZero = false;

                                                if (escludiGiacenzeZero)
                                                {
                                                    usaAnagrafica = false;
                                                    flagQtaMaggioreZero = true;
                                                }

                                                break;
                                            }
                                    }

                                    if (bloccaGiacenze_Utente == "1")
                                        flagQtaMaggioreZero = true;
                                    break;
                                }
                        }

                        break;
                    }

                case enum_Tipo_Operazione_Agenda.Ricetta:
                    {
                        switch (gestioneMagazzino)
                        {
                            case 0:
                                {
                                    usaMagazzino = false;
                                    usaAnagrafica = true;
                                    flagQtaMaggioreZero = false;
                                    break;
                                }

                            case 1:
                                {
                                    usaMagazzino = true;
                                    usaAnagrafica = true;
                                    flagQtaMaggioreZero = false;

                                    if (escludiGiacenzeZero)
                                    {
                                        usaAnagrafica = false;
                                        flagQtaMaggioreZero = true;
                                    }

                                    break;
                                }
                        }

                        break;
                    }
            }
        }

        public static enum_Tipo_Operazione_Agenda getTipoOperazione(AgronicaCoreModelsSTD.attivita.Attivita.Tipo_Attivita tipoAttivita, AgronicaCoreModelsSTD.attivita.Attivita.Stati statoAttivita)
        {
            enum_Tipo_Operazione_Agenda Tipo_Operazione_Agenda = enum_Tipo_Operazione_Agenda.QuadernoDiCampagna;

            switch (tipoAttivita)
            {
                case AgronicaCoreModelsSTD.attivita.Attivita.Tipo_Attivita.QuadernoDiCampagna:
                    {
                        Tipo_Operazione_Agenda = enum_Tipo_Operazione_Agenda.QuadernoDiCampagna;
                        break;
                    }

                case AgronicaCoreModelsSTD.attivita.Attivita.Tipo_Attivita.Ricetta:
                    {
                        if (statoAttivita == AgronicaCoreModelsSTD.attivita.Attivita.Stati.Eseguita)
                            Tipo_Operazione_Agenda = enum_Tipo_Operazione_Agenda.RicettaBrogliaccio;
                        else
                            Tipo_Operazione_Agenda = enum_Tipo_Operazione_Agenda.Ricetta;
                        break;
                    }
            }

            return Tipo_Operazione_Agenda;
        }



    }



}
