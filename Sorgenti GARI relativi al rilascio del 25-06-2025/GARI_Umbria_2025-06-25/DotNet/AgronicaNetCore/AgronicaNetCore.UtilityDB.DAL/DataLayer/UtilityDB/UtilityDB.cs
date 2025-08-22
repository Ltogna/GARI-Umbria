using System.Data;
using System.Text;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Base.Services.Security;
using AgronicaNetCore.UtilityDB.DAL.Resources;
using Microsoft.Extensions.Localization;

namespace AgronicaNetCore.UtilityDB.DAL.DataLayer.UtilityDB
{
    public class UtilityDB : BaseDALUtilityDB, IUtilityDB
    {
        protected readonly ISecurityService? _securityService;
        public UtilityDB(IServiceProvider provider, ISecurityService? securityService, IStringLocalizer<Messages> localizer) : base(provider, localizer)
        {
            _securityService = securityService;
        }

        public async Task<int> Read_SQL_Version_YearAsync(AgronicaCoreParametri objParametri)
        {

            int version = 0;

            var stbQuery = new StringBuilder();
            DataTable result = new DataTable();

            stbQuery.AppendLine(" SELECT @@Version AS Version ");

            try
            {
                result = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString());

                string versionStr = result.Rows[0]["Version"].ToString()!;

                string[] s1 = versionStr.Split(new char[0]);

                int l = s1.Length - 1;

                for (int i = 0; i <= l; i++)
                {
                    int _int;
                    if (s1[i].Length == 4 && int.TryParse(s1[i], out _int))
                    {
                        version = _int;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return version;

        }

        public async Task<int> Read_SQL_Compatibility_LevelAsync(AgronicaCoreParametri objParametri)
        {

            int compatibilityLevel = 0;

            Dictionary<string, object> parSql = new();
            StringBuilder stbQuery = new();
            DataTable result = new();

            stbQuery.AppendLine(" SELECT compatibility_level ");
            stbQuery.AppendLine(" FROM sys.databases ");
            stbQuery.AppendLine(" WHERE name = @name ");

            parSql.Add("@name", GetDBName(objParametri));

            try
            {
                result = await GetDataProvider(objParametri).ExecuteReadAsync(stbQuery.ToString(), parSql);

                compatibilityLevel = (int)(byte)result.Rows[0]["compatibility_level"]!;

            }
            catch (Exception ex)
            {
                LogError(ex.Message, objParametri, ex);
                throw;
            }
            return compatibilityLevel;

        }

        public string GetDBName(AgronicaCoreParametri objParametri)
        {
            _securityService?.SetConnectionString(objParametri);
            string UserDBName = "";

            string[] dummy = objParametri.StringaConnessione.Split(';');

            //[0] Provider = SQLOLEDB;
            //[1] Server = ;
            //[2] Initial Catalog = DB_Name;
            //[3] User Id = ;
            //[4] Password = ;

            if (dummy.Length > 0)
            {
                UserDBName = dummy[2].Split("=")[1];
            }

            return UserDBName;

        }
    }
}
