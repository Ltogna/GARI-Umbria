using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.UtilityDB.DAL.DataLayer.UtilityDB
{
    public interface IUtilityDB
    {
        Task<int> Read_SQL_Version_YearAsync(AgronicaCoreParametri objParametriServer);
        Task<int> Read_SQL_Compatibility_LevelAsync(AgronicaCoreParametri objParametriServer);
        string GetDBName(AgronicaCoreParametri objParametri);
    }
}
