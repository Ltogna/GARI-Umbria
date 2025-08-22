using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaNetCore.Base.Models;
using System.Data;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.CodiciAnagrafe
{
    public interface ICodiciAnagrafe
    {
        Task<List<CodiceAnagrafeBase>> LeggiCodiciUsatixEntitaUsatixEntitaAsync(int entitaLetturaCodici, int idBudget, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiCodiciUsatixEntitaDestinazioniDUsoAsync(AgronicaCoreParametri objParametri);
    }
}
