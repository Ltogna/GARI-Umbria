using AgronicaCoreDTOStd.OutData;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Utenti.DAL.Resources;
using Microsoft.Extensions.Localization;
using System.Data;
using System.Text;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;

namespace AgronicaNetCore.Utenti.DAL.DataLayer.UtentiImpostazioni
{
    public class UtentiImpostazioni : BaseDALUtenti, IUtentiImpostazioni
    {
        public UtentiImpostazioni(IServiceProvider provider, IStringLocalizer<Messages> localizer) : base(provider, localizer) { }

        public async Task<DataTable?> Read_User_Then_SuperUserAsync(int cod, AgronicaCoreParametri objParametriUser)
        {
            DataTable? res = null;

            try
            {
                //Read User
                res = await ReadAsync(cod, 1, objParametriUser);

                if (res == null || res.Rows.Count == 0)
                {
                    //Read SuperUser
                    res = await ReadAsync(cod, 2, objParametriUser);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
            }

            return res;
        }

        public async Task<DataTable?> ReadAsync(int cod, int User1_SuperUser2, AgronicaCoreParametri objParametriUser)
        {
            var stbQuery = new StringBuilder();
            var parSql = new Dictionary<string, object>();
            DataTable? result;

            try
            {
                stbQuery.AppendLine(" SELECT * ");
                stbQuery.AppendLine(" FROM Utenti_Impostazioni ");
                stbQuery.AppendLine(" WHERE 1=1 ");
                stbQuery.AppendLine(" AND (Piva_SuperUser = @Piva_SuperUser) ");
                stbQuery.AppendLine(" AND Username = @Username ");

                if (cod > 0)
                {
                    stbQuery.AppendLine(" AND Impostazione_Cod = @cod ");
                }

                parSql.Add("@cod", cod);
                parSql.Add("@Piva_SuperUser", objParametriUser.PivaSuperUser);
                parSql.Add("@Username", User1_SuperUser2 == 2 ? objParametriUser.SuperUserUsername : objParametriUser.UtenteUsername);

                result = await GetDataProvider(objParametriUser).ExecuteReadAsync(stbQuery.ToString(), parSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametriUser, ex);
                result = null;
            }
            return result;
        }

        public async Task<DataTable> Read_JoinWithFiltroMonoAsync(Enum_Impostazioni_Utenti Impostazione_Cod, int User1_SuperUser2, AgronicaCoreParametri objParametri)
        {

            var stbQuery = new StringBuilder();
            var parametriSql = new Dictionary<string, object>();
            DataTable result;

            stbQuery.AppendLine(" SELECT        Utenti_Impostazioni.Impostazione_Cod,Utenti_Impostazioni_FiltroMono.ID_0 ");
            stbQuery.AppendLine(" FROM          Utenti_Impostazioni");
            stbQuery.AppendLine(" INNER JOIN    Utenti_Impostazioni_FiltroMono ");
            stbQuery.AppendLine(" ON            Utenti_Impostazioni.Piva_SuperUser = Utenti_Impostazioni_FiltroMono.Piva_SuperUser  ");
            stbQuery.AppendLine(" AND           Utenti_Impostazioni.UserName = Utenti_Impostazioni_FiltroMono.UserName  ");
            stbQuery.AppendLine(" AND           Utenti_Impostazioni.Impostazione_Cod = Utenti_Impostazioni_FiltroMono.Impostazione_Cod ");
            stbQuery.AppendLine(" WHERE         1=1  ");

            if (!string.IsNullOrEmpty(objParametri.PivaSuperUser))
            {
                stbQuery.AppendLine(" AND           Utenti_Impostazioni.Piva_SuperUser = @pivaSuperUser");
                parametriSql.Add("@pivaSuperUser", objParametri.PivaSuperUser);
            }

            if (User1_SuperUser2 == 2)
            {
                if (!string.IsNullOrEmpty(objParametri.SuperUserUsername))
                {
                    stbQuery.AppendLine(" AND           Utenti_Impostazioni.Username = @username");
                    parametriSql.Add("@username", objParametri.SuperUserUsername);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(objParametri.UtenteUsername))
                {
                    stbQuery.AppendLine(" AND           Utenti_Impostazioni.Username = @username");
                    parametriSql.Add("@username", objParametri.UtenteUsername);
                }
            }

            if (Impostazione_Cod != 0)
            {
                stbQuery.AppendLine(" AND           Utenti_Impostazioni.Impostazione_Cod = @impostazioneCod");
                parametriSql.Add("@impostazioneCod", Impostazione_Cod);
            }

            if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_SoloNonCancellati)
                stbQuery.AppendLine(" AND           Utenti_Impostazioni.Inviato >= 0 ");
            else if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_SoloCancellati)
                stbQuery.AppendLine(" AND           Utenti_Impostazioni.Inviato = -1 ");
            else if (objParametri.FlagVisibilita == AgronicaCoreParametri.enumVisibilita.Visibilita_Tutti)
            {
                //nessun filtro da aggiungere
            }
            else
                throw new Exception("Parametro non corretto nella query (FlagVisibilita)");

            try
            {
                result = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parametriSql);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }

            return result;
        }

        public async Task<DataInizioEFine> CropYearAsync(DateTime dataRiferimento, AgronicaCoreParametri objParametriUser)
        {
            string sXdate = "01013112";
            DataTable? res = await Read_User_Then_SuperUserAsync((int)Enum_Impostazioni_Utenti.UTENTE_InizioFineAnnataAgraria, objParametriUser);

            if (res != null && res.Rows.Count > 0)
            {
                sXdate = res.Rows[0]["Impostazione_Valore_1"].ToString()!;
            }

            var dataInizioEFine = CalculateCropYear(dataRiferimento, sXdate);
            return dataInizioEFine;
        }

        public async Task<string> ImpostazioneValore1_from_ImpostazioneCod(Enum_Impostazioni_Utenti Impostazione_Cod, int User1_SuperUser2, AgronicaCoreParametri objParametri)
        {
            var stbQuery = new StringBuilder();
            var parSql = new Dictionary<string, object>();
            DataTable? result;

            try
            {
                stbQuery.AppendLine(" SELECT * ");
                stbQuery.AppendLine(" FROM Utenti_Impostazioni ");
                stbQuery.AppendLine(" WHERE 1=1 ");
                stbQuery.AppendLine(" AND (Piva_SuperUser = @Piva_SuperUser) ");
                stbQuery.AppendLine(" AND Username = @Username ");

                if (Impostazione_Cod > 0)
                {
                    stbQuery.AppendLine(" AND Impostazione_Cod = @cod ");
                }

                parSql.Add("@cod", Impostazione_Cod);
                parSql.Add("@Piva_SuperUser", objParametri.PivaSuperUser);
                parSql.Add("@Username", User1_SuperUser2 == 2 ? objParametri.SuperUserUsername : objParametri.UtenteUsername);

                result = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parSql);

                if (result != null && result.Rows.Count > 0)
                {
                    return result.Rows[0]["Impostazione_Valore_1"].ToString()!;
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                result = null;
            }
            return string.Empty;
        }


        public static DataInizioEFine CalculateCropYear(DateTime dataRiferimento, string sXdate)
        {
            int xRif = 0;
            string dataRiferimentoXDateDay = dataRiferimento.Day.ToString().PadLeft(2, '0');
            string dataRiferimentoXDateMonth = dataRiferimento.Month.ToString().PadLeft(2, '0');

            string datainizioXDateDay = sXdate[..2].PadLeft(2, '0');
            string datainizioXDateMonth = sXdate.Substring(2, 2).PadLeft(2, '0');

            if (Convert.ToInt32(dataRiferimentoXDateMonth + dataRiferimentoXDateDay) < Convert.ToInt32(datainizioXDateMonth + datainizioXDateDay))
                xRif = -1;

            var dataInizioEFine = new DataInizioEFine();
            dataInizioEFine.DataInizio = new DateTime(dataRiferimento.Year + xRif, int.Parse(sXdate.Substring(2, 2)), int.Parse(sXdate[..2]));
            dataInizioEFine.DataFine = dataInizioEFine.DataInizio.AddDays(364);

            dataInizioEFine.DataFine = new DateTime(dataInizioEFine.DataFine.Year, int.Parse(sXdate.Substring(6, 2)), int.Parse(sXdate.Substring(4, 2)));
            return dataInizioEFine;
        }


        public async Task<string> LeggiConDefault(Enum_Impostazioni_Utenti impostazioneCod, int Username_1Utente_o_2SuperUser, string valoreDefault, AgronicaCoreParametri objParametri_Utenti)
        {
            var Impostazione_Valore_1 = await ImpostazioneValore1_from_ImpostazioneCod(impostazioneCod, Username_1Utente_o_2SuperUser, objParametri_Utenti);

            if (string.IsNullOrEmpty(Impostazione_Valore_1))
                return valoreDefault;
            else
                return Impostazione_Valore_1;
        }


    }
}
