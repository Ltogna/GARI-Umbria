using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaCoreDTOStd.InData.Zoo;
using AgronicaCoreModelsSTD.attivita;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.MetaSchema.DAL.DataLayer.Operazioni;
using AgronicaNetCore.OperazioniZoo.DAL.DataLayer.OperazioniZoo;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioni;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiProfili;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiVisibilitaAppoggio;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.Agro_Sequenze;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using System.Text;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;
using Microsoft.Extensions.Localization;
using static AgronicaNetCore.Base.Constants.CostantiPersonalizzate;
using AgronicaNetCore.UtilityDB.DAL.DataLayer.UtilityDB;
using AgronicaNetCore.OperazioniZoo.BIZ.Resources;

namespace AgronicaNetCore.OperazioniZoo.BIZ.Services.OperazioniZoo
{


    public class OperazioniZooService : BaseServiceOperazioniZooBIZ, IOperazioniZooService
    {
        private readonly IOperazioniZoo _operazioniZoo;
        private readonly IOperazioni _operazioni;
        private readonly IUtentiImpostazioni _utentiImpostazioni;
        private readonly IUtentiProfili _utentiProfili;
        private readonly IUtentiVisibilitaAppoggio _utentiVisibilitaAppoggio;
        private readonly IUtilityDB _utilityDB;
        private readonly IAgro_Sequence _agroSequence;


        readonly List<int> ListaOperazioniZoo = new() { 3000, 3001, 3002, 3003, 3004, 3020, 3023, 3028, 3030, 3033, 3034, 3035, 3036, 3037 };
        public OperazioniZooService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _operazioniZoo = provider.GetRequiredService<IOperazioniZoo>();
            _operazioni = provider.GetRequiredService<IOperazioni>();
            _utentiImpostazioni = provider.GetRequiredService<IUtentiImpostazioni>();
            _utentiProfili = provider.GetRequiredService<IUtentiProfili>();
            _utentiVisibilitaAppoggio = provider.GetRequiredService<IUtentiVisibilitaAppoggio>();
            _utilityDB = provider.GetRequiredService<IUtilityDB>();
            _agroSequence = provider.GetRequiredService<IAgro_Sequence>();
        }

        public async Task<DataTable> LeggiCentriAziendaliZooAsync(string piva, AgronicaCoreParametri objParametriServer)
        {
            DataTable dt;
            try
            {
                dt = await _operazioniZoo.LeggiCentriZooAsync(piva, objParametriServer);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }

            return dt;
        }

        public async Task<DataTable> LeggiStalleZooAsync(string piva, int centro, AgronicaCoreParametri objParams)
        {
            DataTable dt;
            try
            {
                dt = await _operazioniZoo.LeggiStalleZooAsync(piva, centro, objParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

            return dt;
        }

        public async Task<DataTable> LeggiStalleRaggruppamentiZooAsync(string piva, int centro, int stanum, AgronicaCoreParametri objParams)
        {
            DataTable dt;
            try
            {
                dt = await _operazioniZoo.LeggiStalleRaggruppamentiZooAsync(piva, centro, stanum, objParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

            return dt;
        }

        public async Task<List<Zootecnia>> LeggiOperazioniZooAsync(AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtenti)
        {
            try
            {
                var utentiImpostazioniDt = await _utentiImpostazioni.Read_JoinWithFiltroMonoAsync(TipiEnumerativi.Enum_Impostazioni_Utenti.UTENTE_COD_FILTRO_GRUPPI_OPERAZIONI, 1, objParametriUtenti);
                var dtOperazioni = (await _operazioni.Operazioni_GestioneFiltroUtente_LeggiAsync(new LeggiOperazioni_IN { Operazioni = ListaOperazioniZoo }, utentiImpostazioniDt, objParametriServer, objParametriUtenti)).DataTable;

                var sortedRows = dtOperazioni.Select("", "Lav_Des");

                var listaOperazioni = new List<Zootecnia>();

                foreach (var item in sortedRows)
                {
                    listaOperazioni.Add(new Zootecnia((int)item["LAV_COD"], item["LAV_DES"].ToString()));
                }

                return listaOperazioni;
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }
        }

        public async Task<List<Zootecnia>> LeggiOperazioniZooPreferiteAsync(AgronicaCoreParametri objParametriServer, AgronicaCoreParametri objParametriUtenti)
        {
            try
            {
                //Prelevo i preferiti
                var dtPreferiti = await _utentiImpostazioni.Read_User_Then_SuperUserAsync((int)Enum_Impostazioni_Utenti.UTENTE_OPERAZIONI_ZOO_PREFERITE, objParametriUtenti);

                string val = "";

                var listaPreferiti = new List<Zootecnia>();

                if (dtPreferiti != null && dtPreferiti.Rows.Count > 0)
                {
                    val = dtPreferiti.Rows[0]["Impostazione_Valore_1"].ToString()!;

                    foreach (var item in val.Split("|"))
                    {
                        listaPreferiti.Add(new Zootecnia(int.Parse(item), ""));
                    }
                }

                return listaPreferiti;

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }

        }

        public async Task<DataTable> LeggiOperazioniAgendaZooAsync(string piva, int sa_Cod, int sta_Num, DateTime validitaInizio, DateTime validitaFine, AgronicaCoreParametri objParametriServer)
        {
            try
            {

                List<string> filtroCentri = new();

                if (sa_Cod == 0)
                {
                    //leggo se ci sono filtri sui centri
                    var DtCentriVisibili = await _utentiVisibilitaAppoggio.ReadVisibilitaCentriAsync(piva, objParametriServer);
                    if (!(DtCentriVisibili == null) && DtCentriVisibili.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DtCentriVisibili.Rows)
                        {
                            filtroCentri.Add(dr["sa_cod"].ToString());
                        }
                    }
                }

                var validitaFineGiacenze = validitaFine;
                if (validitaFineGiacenze < DateTime.Parse(CostantiPersonalizzate.AGRODATAFINE))
                    validitaFineGiacenze = validitaFine.AddDays(1);

                var livelloCompatibilita = await _utilityDB.Read_SQL_Compatibility_LevelAsync(objParametriServer);


                //Leggo le operazioni
                var dtOperazioni = await _operazioniZoo.CaricaAgendaZooAsync(piva, sa_Cod, sta_Num, validitaInizio, validitaFine, validitaFineGiacenze, filtroCentri, livelloCompatibilita, objParametriServer);

                //int i = 0;

                //var listaOperazioni = new List<Zootecnia>();

                return dtOperazioni;

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriServer, ex);
                throw;
            }

        }
        public async Task<DataTable> LeggiGiacenzeZooAsync(
            LeggiGiacenzeZooDto paramsLeggiGiacenze,
            AgronicaCoreParametri objParametriServer,
            AgronicaCoreParametri objParametriUtente,
            string? superUserUsername = null)
        {
            DataTable result;
            try
            {

                var Sql_Permessi = string.Empty;
                var filtroVisibilitaUtente = false;
                var filtraGiacenze = true;


                // Parameter checks
                var filtraFornitori = true;
                var piva = paramsLeggiGiacenze.Piva;
                var saCod = paramsLeggiGiacenze.CodCentro;
                var filtroAll = paramsLeggiGiacenze.Istantanea;
                var codAnimale = paramsLeggiGiacenze.CodAnimale;
                var dataGiacenza = paramsLeggiGiacenze.DataGiacenza;
                var staNum = (saCod != 0 && paramsLeggiGiacenze.CodStalla != 0) ? paramsLeggiGiacenze.CodStalla : 0;
                var raggrCod = (staNum != 0 && paramsLeggiGiacenze.CodRaggruppamento != 0) ? paramsLeggiGiacenze.CodRaggruppamento : 0;
                var listaCodAnimali = (paramsLeggiGiacenze.CodAnimale == 0 && paramsLeggiGiacenze.ListaCodAnimali != null && paramsLeggiGiacenze.ListaCodAnimali.Any()) ? paramsLeggiGiacenze.ListaCodAnimali : null;

                var DTProfilo = await _utentiProfili.ReadAsync(objParametriUtente, ID_SERVIZIO.GIASONLINE_SERVICE_ID);

                if (DTProfilo?.Rows.Count > 0)
                {
                    Sql_Permessi = DTProfilo.Rows[0]["Descrizione_2"]?.ToString() ?? "";
                }

                filtroVisibilitaUtente = !string.IsNullOrEmpty(Sql_Permessi);

                result = await _operazioniZoo.Leggi_GiacenzeAsync(
                    piva,
                    saCod,
                    staNum,
                    raggrCod,
                    codAnimale,
                    dataGiacenza,
                    objParametriServer,
                    filtroAll,
                    filtroVisibilitaUtente,
                    listaCodAnimali,
                    filtraGiacenze,
                    filtraFornitori,
                    paramsLeggiGiacenze.MostraPesate,
                    true,
                    paramsLeggiGiacenze.Matricola ?? string.Empty,
                    paramsLeggiGiacenze.MostraGGPrimoCaricamento,
                    paramsLeggiGiacenze.CFproprietario ?? string.Empty);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex: ex);
                throw;
            }

            return result;
        }

        public async Task<DataTable> LeggiTrattamentiZooAsync(
            GetTrattamentiZooDto filter,
            AgronicaCoreParametri objParametriUtente,
            AgronicaCoreParametri objParametriServer)
        {

            string? nomeRoutine = MethodBase.GetCurrentMethod()?.Name;

            string messaggioErrore = "";
            var stb = new StringBuilder();
            DataTable result;

            try
            {
                var DTProfilo = await _utentiProfili.ReadAsync(objParametriUtente, ID_SERVIZIO.GIASONLINE_SERVICE_ID);
                var Sql_Permessi = string.Empty;
                if (DTProfilo?.Rows.Count > 0)
                {
                    Sql_Permessi = DTProfilo.Rows[0]["Descrizione_2"]?.ToString() ?? "";
                }

                result = await _operazioniZoo.LeggiTrattamentiAsync(
                     piva: filter.Piva,
                     saCod: filter.CodCentro,
                     staNum: filter.CodStalla,
                     raggruppamentoCod: filter.CodRaggruppamento,
                     codAnimale: filter.CodAnimale,
                     matricola: filter.Matricola,
                     dataInizio: filter.DataInizio,
                     dataFine: filter.DataFine,
                     filtroVisibilitaUtente: !string.IsNullOrWhiteSpace(Sql_Permessi),
                     listCodAnimali: filter.ListaCodAnimali,
                     farmCatList: filter.FarmCatList?.ToArray(),
                     farmCatSemplList: filter.FarmCatSemplList?.ToArray(),
                    objParametriServer);
            }

            catch (Exception ex)
            {
                messaggioErrore = ex.Message;
                LogError(ex.Message, ex: ex);
                result = default;
                throw new Exception("[" + nomeRoutine + "] : " + messaggioErrore);
            }

            return result;

        }

        public async Task<DataTable> LeggiCapiSenzaTrattamentiAsync(
            GetSenzaTrattamentiZooDto filtro,
            AgronicaCoreParametri objParametriUtente,
            AgronicaCoreParametri objParametriServer)
        {
            string? nomeRoutine = MethodBase.GetCurrentMethod()?.Name;

            string messaggioErrore = "";
            DataTable result;

            try
            {
                var DTProfilo = await _utentiProfili.ReadAsync(objParametriUtente, ID_SERVIZIO.GIASONLINE_SERVICE_ID);
                var Sql_Permessi = string.Empty;
                if (DTProfilo?.Rows.Count > 0)
                {
                    Sql_Permessi = DTProfilo.Rows[0]["Descrizione_2"]?.ToString() ?? "";
                }

                var compatibilityLevel = await _utilityDB.Read_SQL_Compatibility_LevelAsync(objParametriServer);
                result = await _operazioniZoo.LeggiCapiSenzaTrattamentiAsync
                    (
                        filtro.Piva,
                        filtro.CodCentro,
                        filtro.CodStalla,
                        filtro.CodRaggruppamento,
                        filtro.CodAnimale,
                        filtro.Data,
                        filtro.GiorniSenzaTrattamento,
                        filtroVisibilitaUtente: !string.IsNullOrWhiteSpace(Sql_Permessi),
                        null,
                        (filtro.FarmCatList ?? new List<int>()).ToArray(),
                        (filtro.FarmCatSemplList ?? new List<int>()).ToArray(),
                        objParametriServer,
                        compatibilityLevel,
                        filtro.MostraGGInizioTotali
                    );
            }

            catch (Exception ex)
            {
                messaggioErrore = ex.Message;
                LogError(ex.Message, ex: ex);
                result = default;
                throw new Exception("[" + nomeRoutine + "] : " + messaggioErrore);
            }

            return result;

        }

        public async Task<DataTable> LeggiStazionamentoZooAsync(
            GetStazionamentoZooDto filter,
            AgronicaCoreParametri objParametriUtente,
            AgronicaCoreParametri objParametriServer)
        {
            string? nomeRoutine = MethodBase.GetCurrentMethod()?.Name;

            string messaggioErrore = "";
            DataTable result;
            try
            {
                var DTProfilo = await _utentiProfili.ReadAsync(objParametriUtente, ID_SERVIZIO.GIASONLINE_SERVICE_ID);
                var Sql_Permessi = string.Empty;
                if (DTProfilo?.Rows.Count > 0)
                {
                    Sql_Permessi = DTProfilo.Rows[0]["Descrizione_2"]?.ToString() ?? "";
                }

                var compatibilityLevel = await _utilityDB.Read_SQL_Compatibility_LevelAsync(objParametriServer);

                result = await _operazioniZoo.LeggiStazionamentoZooAsync(
                    filter.Piva,
                    filter.CodCentro,
                    filter.CodStalla,
                    filter.CodRaggruppamento,
                    filter.CodAnimale,
                    filter.Data,
                    filter.GiorniStazionamento,
                    filtroVisibilitaUtente: !string.IsNullOrWhiteSpace(Sql_Permessi),
                    filter.ListaCodAnimali ?? new List<int>(),
                    objParametriServer,
                    compatibilityLevel
                    );

            }
            catch (Exception ex)
            {
                messaggioErrore = ex.Message;
                LogError(ex.Message, ex: ex);
                result = default;
                throw new Exception("[" + nomeRoutine + "] : " + messaggioErrore);
            }

            return result;
        }
    }
}
