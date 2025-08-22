using AgronicaDataProvider6.Interfaces;
using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Constants;
using AgronicaNetCore.Base.DataLayer.Security;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Base.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Data;
using System.Text;

namespace AgronicaNetCore.Base.Services.Security
{
    public class SecurityService : ISecurityService
    {

        public const string ID_DB_Super_Server = "-1";

        private readonly string? _encryptKey;

        private readonly ISecurityLayer _securityLayer;

        private readonly AgronicaCoreParametri _objParams;

        private readonly ConcurrentDictionary<string, string> _connections = new();
        
        public SecurityService(IServiceProvider provider, IConfiguration config)
        {
            _securityLayer = provider.GetRequiredService<ISecurityLayer>();
            _encryptKey = config.GetValue<string>("cr2");

            var connectionString = config.GetValue<string>("ConnectionString");
            // string connectionStringEncoded = Utility.Security.EncryptString(connectionString, _encryptKey);
            
            // decodifica la stringa di connessione se criptata
            if (config.GetValue<bool>("ConnectionStringEncoded")) {
                connectionString = Utility.Security.DecryptString(connectionString, _encryptKey);
            }
            
            _objParams = new AgronicaCoreParametri()
            {
                StringaConnessione = connectionString,
                Lingua_Cod = 1,
                LogDirectory = "C:\\GIASLAN",
                LogFileName = "GiasOnline_log.txt",
            };

            InitConnessioni();
        }

        private void InitConnessioni()
        {
            string cryptConnection = LeggiConfigurazione("UserPwdConnectionString_toCrypt", _objParams);
            if (!string.IsNullOrEmpty(cryptConnection) && cryptConnection.ToLower() != "false")
            {
                string cr = LeggiConfigurazione("cr", _objParams);
                var dt = LeggiConnessioni(_objParams);
                
                foreach (DataRow row in dt.Rows)
                {
                    string ID_DB = row["ID_DB"].ToString()!;
                    string userId = row["UserId"].ToString()!;
                    string password = row["Password"].ToString()!;
                    bool flagIsEncrypted = row["Flag_Encrypted"].ToString() == "1";

                    if (flagIsEncrypted && !string.IsNullOrEmpty(_encryptKey))
                    {
                        userId = Utility.Security.DecryptString(userId, cr, _encryptKey);
                        password = Utility.Security.DecryptString(password, cr, _encryptKey);
                    }

                    string connectionString =
                        "Provider=" + row["Provider"] +
                        ";Server=" + row["Server"] +
                        ";Initial Catalog=" + row["DB"] +
                        ";User Id=" + userId +
                        ";Password=" + password + ";";

                    _connections[ID_DB] = connectionString;
                }

                _connections[ID_DB_Super_Server] = _objParams.StringaConnessione;

            }

        }

        private DataTable LeggiConnessioni(AgronicaCoreParametri objParametriSuperServer)
        {
            return _securityLayer.LeggiConnessioni(objParametriSuperServer);
        }

        private string LeggiConfigurazione(string chiave, AgronicaCoreParametri objParams)
        {
            var dt = _securityLayer.LeggiConfigurazioneSiti(chiave, objParams);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Valore"].ToString()!;
            }
            return "";
        }

        public string GetConnectionString(string connStr)
        {
            try
            {
                return _connections[connStr];
            }
            catch (Exception)
            {
                throw new Exception("Impossibile ricavare la stringa di connessione: " + connStr);
            }
        }

        public void SetConnectionString(AgronicaCoreParametri obj)
        {
            if (!_connections.IsEmpty && int.TryParse(obj.StringaConnessione, out _))
            {
                obj.StringaConnessione = GetConnectionString(obj.StringaConnessione);
            }
        }
    }

}
