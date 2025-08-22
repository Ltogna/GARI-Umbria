using AgronicaCoreDTOStd.InData.Metaschema;
using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.MetaSchema.BIZ.Services.DivisioniAmministrative
{
    public interface IDivisioniAmministrativeService
    {
        Task<DataTable> LeggiStatiAsync(string codice, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiRegioniAsync(LeggiRegioni_IN leggiRegioni_IN, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiProvinceAsync(LeggiProvince_IN leggiProvince_IN, AgronicaCoreParametri objParametri);

        Task<DataTable> LeggiComuniAsync(LeggiComuni_IN leggiComuni_IN, AgronicaCoreParametri objParametri);

    }
}
