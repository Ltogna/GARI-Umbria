using AgronicaNetCore.MetaSchema.DAL.DataLayer.SpecieVegetali;
using AgronicaNetCore.Base.Models;
using System.Data;
using AgronicaCoreDTOStd.InData.Metaschema;
using Microsoft.Extensions.DependencyInjection;
using AgronicaCoreDTOStd.OutData.FiltroRicerca;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioniFiltroMono;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;
using AgronicaNetCore.Utenti.DAL.DataLayer.UtentiVisibilitaAppoggio;
using Microsoft.Extensions.Localization;
using AgronicaNetCore.MetaSchema.BIZ.Resources;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.SpecieVegetali
{
    public class SpecieVegetaliService : BaseServiceMetaschemaBIZ, ISpecieVegetaliService
    {
        private readonly ISpecieVegetali _specieVegetali;
        private readonly IUtentiImpostazioniFiltroMono _utentiImpostazioniMono;
        private readonly IUtentiVisibilitaAppoggio _utentiVisibilita;

        public SpecieVegetaliService(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _specieVegetali = _serviceProvider.GetRequiredService<ISpecieVegetali>();
            _utentiImpostazioniMono = _serviceProvider.GetRequiredService<IUtentiImpostazioniFiltroMono>();
            _utentiVisibilita = _serviceProvider.GetRequiredService<IUtentiVisibilitaAppoggio>();
        }

        public async Task<DtConVisibilita_OUT> SpecieVegetali_GestioneFiltroUtente_LeggiAsync(LeggiSpecieVegetali_IN leggiSpecie_IN, AgronicaCoreParametri objParametri)
        {
            DtConVisibilita_OUT dtConVisibilita;
            try
            {
                var utentiImpostazioniMonoDt = await _utentiImpostazioniMono.LeggiAsync(TipiEnumerativi.Enum_Impostazioni_Utenti.UTENTE_COD_FILTRO_GRUPPI_VEGETALI, 0, objParametri);
                dtConVisibilita = await _specieVegetali.SpecieVegetali_GestioneFiltroUtente_LeggiAsync(leggiSpecie_IN, utentiImpostazioniMonoDt, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return dtConVisibilita;
        }

        public async Task<DataTable> Cultivar_GestioneFiltroUtente_LeggiAsync(Cultivar_GestioneFiltroUtente_Leggi_IN leggiCultivar_IN, AgronicaCoreParametri objParametri)
        {
            DataTable dt;
            try
            {
                var utentiImpostazioniMonoDt = await _utentiImpostazioniMono.LeggiAsync(TipiEnumerativi.Enum_Impostazioni_Utenti.UTENTE_COD_FILTRO_GRUPPI_VEGETALI, 0, objParametri);
                dt = await _specieVegetali.Cultivar_GestioneFiltroUtente_LeggiAsync(leggiCultivar_IN, utentiImpostazioniMonoDt, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return dt;
        }

        public async Task<DataTable> GruppoVegetale_GestioneFiltroUtente_LeggiAsync(int gru_cod, AgronicaCoreParametri objParametri)
        {
            DataTable dt;
            try
            {
                dt = await _specieVegetali.GruppoVegetale_GestioneFiltroUtente_LeggiAsync(gru_cod, objParametri);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return dt;
        }

        public async Task<DataTable> LeggiGruppiVarietaliAsync(LeggiGruppiVarietali_IN leggiGruppiVarietali_IN, AgronicaCoreParametri objParametri)
        {
            DataTable temporaryResult;
            DataTable result;

            try
            {
                temporaryResult = await _specieVegetali.LeggiGruppiVarietaliAsync(leggiGruppiVarietali_IN, objParametri);
                result = temporaryResult.Clone();

                foreach (DataRow row in temporaryResult.Rows)
                {
                    result.ImportRow(row);

                    var grva_cod = (int)row["Grva_Cod"] * (-1);
                    var grva_des = (string)row["Grva_Des"] + " - Ibrido";

                    result.Rows.Add(grva_cod, grva_des, row["Veg_Cod"], row["Veg_Des"]);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return result;
        }

        public async Task<DataTable> LeggiVarietaAsync(int culCod, int vegCod, AgronicaCoreParametri objParams)
        {
            DataTable result;

            try
            {
                result = await _specieVegetali.LeggiVarietaAsync(culCod, vegCod, objParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

            return result;
        }

        public async Task<DataTable> LeggiSpecieAziendaliAsync(string piva, AgronicaCoreParametri objParams)
        {
            DataTable result;

            try
            {
                result = await _specieVegetali.LeggiSpecieAziendaliAsync(piva, objParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

            return result;
        }

        public async Task<DataTable> LeggiVarietaFilteredAsync(string piva, int culCod, int vegCod, AgronicaCoreParametri objParams)
        {
            DataTable dt;
            try
            {
                var dtCentriVisibili = await _utentiVisibilita.ReadAsync((int)Enum_TipoEntita.Centro, objParams, piva: piva);
                dt = await _specieVegetali.LeggiVarietaFilteredAsync(piva, culCod, vegCod, dtCentriVisibili, objParams);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }

            return dt;
        }
    }
}