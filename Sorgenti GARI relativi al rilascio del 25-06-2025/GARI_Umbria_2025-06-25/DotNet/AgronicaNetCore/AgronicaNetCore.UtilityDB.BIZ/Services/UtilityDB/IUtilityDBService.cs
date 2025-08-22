using AgronicaNetCore.Base.Models;

namespace AgronicaNetCore.UtilityDB.BIZ.Services.UtilityDB
{
    public interface IUtilityDBService
    {
        Task<int> Read_SQL_Version_YearAsync(AgronicaCoreParametri objParametri);
        Task<int> Read_SQL_Compatibility_LevelAsync(AgronicaCoreParametri objParametri);
        Task<string> GetDBName(AgronicaCoreParametri objParametri);
    }
}
