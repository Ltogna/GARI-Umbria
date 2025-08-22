using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.MetaSchema.DAL.DataLayer.DivisioniAmministrative
{
    public interface IDivisioniAmministrative
    {
        Task<DataTable> LeggiStatiAsync(string codice, AgronicaCoreParametri objParametri);
        
        Task<DataTable> LeggiRegioniAsync(LeggiRegioni_IN leggiRegioniIN, AgronicaCoreParametri objParametri);
        
        Task<DataTable> LeggiProvinceAsync(LeggiProvince_IN leggiProvinceIN, AgronicaCoreParametri objParametri);
        
        Task<DataTable> LeggiComuniAsync(LeggiComuni_IN leggiComuni_IN, AgronicaCoreParametri objParametri);
    }
}
