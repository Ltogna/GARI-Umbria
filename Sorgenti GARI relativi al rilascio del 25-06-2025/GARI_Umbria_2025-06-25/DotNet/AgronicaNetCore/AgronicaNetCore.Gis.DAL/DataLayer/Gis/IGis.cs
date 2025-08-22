using AgronicaNetCore.Base.Models;
using System.Data;

namespace AgronicaNetCore.Gis.DAL.DataLayer.Gis
{
    public interface IGis
    {
        Task<DataTable> LeggiGIS_ProcessingAlgorithms_Cleaning_AlgorithmAsync(AgronicaCoreParametri objParametri);
    }
}
