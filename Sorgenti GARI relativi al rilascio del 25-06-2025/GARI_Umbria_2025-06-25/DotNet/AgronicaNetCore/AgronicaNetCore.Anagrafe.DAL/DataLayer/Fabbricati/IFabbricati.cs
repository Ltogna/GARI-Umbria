using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Anagrafe.DAL.DataLayer.Fabbricati
{
    public interface IFabbricati
    {
        public Task<int> ReadFabbCodAsync(string Piva, int Sa_Cod, int Tipo_Fabb, AgronicaCoreParametri objP);
        public Task<DataTable> ReadAsync(string Piva, int Sa_Cod, int Fabbricato_Cod, AgronicaCoreParametri objP);
    }
}
