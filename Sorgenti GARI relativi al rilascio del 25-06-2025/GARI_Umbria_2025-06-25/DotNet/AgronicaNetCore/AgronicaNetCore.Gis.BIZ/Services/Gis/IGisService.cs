using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Gis.BIZ.Services.Gis
{
    public interface IGisService
    {
        Task<DataTable> LeggiGIS_ProcessingAlgorithms_Cleaning_AlgorithmAsync(AgronicaCoreParametri objParametri);
    }
}
