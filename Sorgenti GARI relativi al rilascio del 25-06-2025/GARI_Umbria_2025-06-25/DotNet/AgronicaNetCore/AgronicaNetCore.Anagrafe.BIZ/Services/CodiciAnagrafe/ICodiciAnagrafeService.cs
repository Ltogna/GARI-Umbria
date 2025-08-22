using AgronicaCoreDTOStd.InData.Anagrafica;
using AgronicaCoreModelsSTD.anagrafiche;
using AgronicaNetCore.Base.Models;
using System.Data;
using static AgronicaNetCore.Base.Constants.TipiEnumerativi;

namespace AgronicaNetCore.Anagrafe.BIZ.Services.CodiciAnagrafe
{
    public interface ICodiciAnagrafeService
    {
        Task<List<CodiceAnagrafeBase>> LeggiCodiciUsatixEntitaAsync(LeggiCodiciUsatixEntitaAnagrafe_IN LeggiCodiciUsatixEntitaAnagrafe, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiCodiciUsatixEntitaDestinazioniDUsoAsync(AgronicaCoreParametri objParametri);
    }
}
