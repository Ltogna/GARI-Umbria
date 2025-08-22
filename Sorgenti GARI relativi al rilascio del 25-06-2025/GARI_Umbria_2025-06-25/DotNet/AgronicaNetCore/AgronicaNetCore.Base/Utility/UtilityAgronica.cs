using AgronicaDataProvider6.Interfaces;
using AgronicaDataProvider6.Models;
using AgronicaDataProvider6.Settings;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AgronicaNetCore.Base.Utility
{
    public class UtilityAgronica
    {
        public static AgronicaCoreParametri convertStringtoOBJparametri(string s, IOptions<SecuritySettings> options)
        {
            var sE = Security.Stringa_Decodifica_LANCompatibile(s, CostantiPersonalizzate.AgroKey_EncoderDecoder, options);
            return (JsonConvert.DeserializeObject<AgronicaCoreParametri>(sE))!;
        }

        public static SqlProviderConnection getSqlConnectionFromObjParametri(AgronicaCoreParametri obj)
        {
            var ret = new SqlProviderConnection();
            var connPars = obj.StringaConnessione.Split(";");
            foreach (var par in connPars)
            {
                var p = par.Split("=");
                switch (p[0].Trim())
                {
                    case "Server":
                        ret.DataSource = p[1];
                        break;
                    case "Initial Catalog":
                    case "Database":
                        ret.InitialCatalog = p[1];
                        break;
                    case "User Id":
                        ret.UserId = p[1];
                        break;
                    case "Password":
                        ret.Password = p[1];
                        break;
                    default:
                        //opzione non mappata
                        break;
                }
            }
            return ret;
        }

        [Obsolete("Method is deprecated, please use DAL_Base.FormatClauseIn instead.", false)]
        public static string GenerateParameterizedStringForInClause(string customParamsName, List<string> stringList, Dictionary<string, object> parSql )
        {
            List<string>? paramStringList = new();
            string strParameterized = "";

            foreach (string _string in stringList)
            {
                string parId = "@" + customParamsName;
                parId += stringList.IndexOf(_string).ToString();

                paramStringList.Add(parId);
                parSql.Add(parId, _string);
            }

            strParameterized = string.Join(", \n", paramStringList);

            paramStringList.Clear();
            paramStringList = null;

            return strParameterized;
        }
    }
}
