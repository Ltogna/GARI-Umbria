using AgronicaNetCore.Anagrafe.DAL.Base;
using AgronicaNetCore.Base.Models;
using InData.Log.AgronicaLogInvio;
using System.Dynamic;
using System.Text;
using AgronicaNetCore.Base.Constants;

namespace AgronicaNetCore.Logging.DAL.DataLayer.AgronicaLogInvio.AgronicaLogInvioChiamate
{
    public class AgronicaLogInvioChiamate: DAL_Base,IAgronicaLogInvioChiamate
    {

        public AgronicaLogInvioChiamate(IServiceProvider provider) : base(provider)
        {
        }

        public async Task<bool> WriteAsync(WriteAgronicaLogInvioChiamate writeAgronicaLogInvioChiamate, AgronicaCoreParametri objParams)
        {

            var stbQuery = new StringBuilder();
            stbQuery.AppendLine("INSERT INTO Agronica_Log_Invio_Chiamate (PivaSuperUser, ID, Tipo_Esportazione,")
                .AppendLine("    Dati_Inviati, Data_Invio, Esito, Dati_Ricevuti, Controllata, Tipo_Operazione, inviato, datainvio,")
                .AppendLine("    Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine,")
                .AppendLine("    Dettaglio1, Dettaglio2, Dettaglio3) ");
            stbQuery.AppendLine("VALUES (@PivaSuperUser, @ID, @Tipo_Esportazione, @Dati_Inviati, @Data_Invio,  ")
                .AppendLine("    @Esito, @Dati_Ricevuti, @Controllata, @Tipo_Operazione, @inviato, @datainvio, ")
                .AppendLine("    @Data_Creazione, @Data_Modifica, @Username_Creazione, @Username_Modifica, @Validita_Inizio, @Validita_Fine, ")
                .AppendLine("    @Dettaglio1, @Dettaglio2, @Dettaglio3) ");

            var expandoObj = new ExpandoObject();
            expandoObj.TryAdd("@PivaSuperUser", objParams.PivaSuperUser);
            expandoObj.TryAdd("@ID", writeAgronicaLogInvioChiamate.ID);
            expandoObj.TryAdd("@Tipo_Esportazione", writeAgronicaLogInvioChiamate.Tipo_Esportazione);
            expandoObj.TryAdd("@Dati_Inviati", writeAgronicaLogInvioChiamate.Dati_Inviati);
            expandoObj.TryAdd("@Data_Invio", writeAgronicaLogInvioChiamate.Data_Invio);
            expandoObj.TryAdd("@Esito", writeAgronicaLogInvioChiamate.Esito);
            expandoObj.TryAdd("@Dati_Ricevuti", writeAgronicaLogInvioChiamate.Dati_Ricevuti);
            expandoObj.TryAdd("@Controllata", writeAgronicaLogInvioChiamate.Controllata == null ? 0 : writeAgronicaLogInvioChiamate.Controllata);
            expandoObj.TryAdd("@Tipo_Operazione", writeAgronicaLogInvioChiamate.Tipo_Operazione);
            expandoObj.TryAdd("@inviato", 0);
            expandoObj.TryAdd("@datainvio", DBNull.Value);

            expandoObj.TryAdd("@Data_Creazione", DateTime.Now);
            expandoObj.TryAdd("@Data_Modifica", DateTime.Now);
            expandoObj.TryAdd("@Username_Creazione", objParams.UsernameOperazione);
            expandoObj.TryAdd("@Username_Modifica", objParams.UsernameOperazione);
            expandoObj.TryAdd("@Validita_Inizio", writeAgronicaLogInvioChiamate.Validita_Inizio == null ? CostantiPersonalizzate.AGRODATAINIZIO : writeAgronicaLogInvioChiamate.Validita_Inizio);
            expandoObj.TryAdd("@Validita_Fine", writeAgronicaLogInvioChiamate.Validita_Fine == null ? CostantiPersonalizzate.AGRODATAFINE : writeAgronicaLogInvioChiamate.Validita_Fine);

            expandoObj.TryAdd("@Dettaglio1", writeAgronicaLogInvioChiamate.Dettaglio1);
            expandoObj.TryAdd("@Dettaglio2", writeAgronicaLogInvioChiamate.Dettaglio2);
            expandoObj.TryAdd("@Dettaglio3", writeAgronicaLogInvioChiamate.Dettaglio3);

            try
            {
                return await GetDataProvider(objParams).Execute_WriteAsync(stbQuery.ToString(), expandoObj);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParams, ex);
                throw;
            }
        }
    }
}
