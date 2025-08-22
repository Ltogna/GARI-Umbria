using System.Dynamic;
using System.Text;
using AgronicaNetCore.Anagrafe.DAL.Base;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Logging.DAL.DataLayer.AgronicaLogInvio.AgronicaLogInvioAgenda;
using InData.Log.AgronicaLogInvio;
using AgronicaNetCore.Base.Constants;

public class AgronicaLogInvioAgenda: DAL_Base, IAgronicaLogInvioAgenda
{
    public AgronicaLogInvioAgenda(IServiceProvider provider) : base(provider)
    {
    }

    public async Task<bool> WriteAsync(WriteAgronicaLogInvioAgenda writeAgronicaLogInvioAgenda, AgronicaCoreParametri objParams)
    { 

        var stbQuery = new StringBuilder();
        stbQuery.AppendLine("INSERT INTO Agronica_Log_Invio_Agenda (Tipo_Esportazione, ID_Log_Invio, ID_Agenda,")
            .AppendLine("    ID_Operazione_Esterna, inviato, DataInvio, Data_Creazione, Data_Modifica, Username_Creazione, Username_Modifica, Validita_Inizio, Validita_Fine,")
            .AppendLine("    Id_Mov, Id_Mov_Det, Id_Mov_Dest, Causale_Cod, Piva, Chiave_Esterna, Chiave) ");
        stbQuery.AppendLine("VALUES (@Tipo_Esportazione, @ID_Log_Invio, @ID_Agenda, @ID_Operazione_Esterna, @inviato, @DataInvio,  ")
            .AppendLine("    @Data_Creazione, @Data_Modifica, @Username_Creazione, @Username_Modifica, @Validita_Inizio, @Validita_Fine, ")
            .AppendLine("    @Id_Mov, @Id_Mov_Det, @Id_Mov_Dest, @Causale_Cod, @Piva, @Chiave_Esterna, @Chiave) ");

        var expandoObj = new ExpandoObject();
        expandoObj.TryAdd("@Tipo_Esportazione", writeAgronicaLogInvioAgenda.Tipo_Esportazione);
        expandoObj.TryAdd("@ID_Log_Invio", writeAgronicaLogInvioAgenda.ID_Log_Invio);
        expandoObj.TryAdd("@ID_Agenda", writeAgronicaLogInvioAgenda.ID_Agenda);
        expandoObj.TryAdd("@ID_Operazione_Esterna", writeAgronicaLogInvioAgenda.ID_Operazione_Esterna == null ? DBNull.Value : writeAgronicaLogInvioAgenda.ID_Operazione_Esterna);
        expandoObj.TryAdd("@inviato", 0);
        expandoObj.TryAdd("@DataInvio", DBNull.Value);

        expandoObj.TryAdd("@Data_Creazione",DateTime.Now);
        expandoObj.TryAdd("@Data_Modifica", DateTime.Now);
        expandoObj.TryAdd("@Username_Creazione", objParams.UsernameOperazione);
        expandoObj.TryAdd("@Username_Modifica", objParams.UsernameOperazione);
        expandoObj.TryAdd("@Validita_Inizio", writeAgronicaLogInvioAgenda.Validita_Inizio == null ? CostantiPersonalizzate.AGRODATAINIZIO: writeAgronicaLogInvioAgenda.Validita_Inizio);
        expandoObj.TryAdd("@Validita_Fine", writeAgronicaLogInvioAgenda.Validita_Fine == null ? CostantiPersonalizzate.AGRODATAFINE : writeAgronicaLogInvioAgenda.Validita_Fine);
        expandoObj.TryAdd("@Id_Mov", writeAgronicaLogInvioAgenda.Id_Mov);
        expandoObj.TryAdd("@Id_Mov_Det", writeAgronicaLogInvioAgenda.Id_Mov_Det);
        expandoObj.TryAdd("@Id_Mov_Dest", writeAgronicaLogInvioAgenda.Id_Mov_Dest);
        expandoObj.TryAdd("@Causale_Cod", writeAgronicaLogInvioAgenda.Causale_Cod == null ? 0 : writeAgronicaLogInvioAgenda.Causale_Cod);
        expandoObj.TryAdd("@Piva", writeAgronicaLogInvioAgenda.Piva);
        expandoObj.TryAdd("@Chiave_Esterna", writeAgronicaLogInvioAgenda.Chiave_Esterna);
        expandoObj.TryAdd("@Chiave", writeAgronicaLogInvioAgenda.Chiave);

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
