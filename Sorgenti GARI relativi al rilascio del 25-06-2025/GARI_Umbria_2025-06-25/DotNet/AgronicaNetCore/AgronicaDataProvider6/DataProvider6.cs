using AgronicaDataProvider6.Interfaces;
using AgronicaDataProvider6.Models;
using AgronicaDataProvider6.Providers;
using System.Data;
using System.Data.Common;
using System.Dynamic;

namespace AgronicaDataProvider6
{

    public class TestObject
    { 
        public string? Guid { get; set; }
        public string? Name { get; set; }    
    }

    public class DataProvider6<TP> : BaseProvider, IDataProvider
    {
        private IDataProvider _provider;

        public DataProvider6()
        {
            CreateProvider<TP>(null,null);
        }

        public DataProvider6( IServiceProvider provider, DataProvider6Settings settings): base(settings)
        {
            CreateProvider<TP>(provider,settings);
        }


        private void CreateProvider<T>(IServiceProvider provider, DataProvider6Settings settings)
        {
            if (settings == null )
                throw new NotImplementedException("Settings non passati");
            if (String.IsNullOrEmpty(settings.ConnectionString))
                throw new NotImplementedException("Stringa di connessione non valorizzata");

            switch (typeof(T))
            {
                case Type sqlDP when sqlDP == typeof(SqlDataProvider):
                    _provider = new SqlDataProvider(provider,settings);
                    break;
                default:
                    throw new NotImplementedException("Provider Non Supportato");

            }

        }

        public async Task<DataTable> ExecuteReadAsync(string SqlString)
        {
            return await _provider.ExecuteReadAsync(SqlString);
        }

        /// <summary>
        /// DA NON USARE ASSOLUTAMENTE PERCHé SPECIFICO PER LE CORE API
        /// </summary>
        /// <param name="SqlString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Obsolete("Usare solo per core api, negli altri casi usare il metodo asincrono")]
        public DataTable ExecuteRead(string SqlString, Dictionary<string, object> parameters, Dictionary<string, Dictionary<Type,List< object>>> parameters_in)
        {
            return _provider.ExecuteRead(SqlString, parameters, parameters_in);
        }

        public async Task<DataTable> ExecuteReadAsync(string SqlString, Dictionary<string, object> parameters, Dictionary<string, Dictionary<Type, List<object>>>? parameters_in)
        {
            return await _provider.ExecuteReadAsync(SqlString, parameters, parameters_in);
        }

        public async Task<T> ExecuteReadAsync<T>(string SqlString) where T : class
        {
            return await _provider.ExecuteReadAsync<T>(SqlString);
        }

        public async Task<T> ExecuteReadAsync<T>(string SqlString, Dictionary<string, object> parameters, Dictionary<string, Dictionary<Type, List<object>>>? parameters_in) where T : class
        {
            return await _provider.ExecuteReadAsync<T>(SqlString, parameters, parameters_in);
        }

        public async Task<DataSet> ExecuteMultipleReadAsync(string sqlString, string dataSetName)
        {
            return await _provider.ExecuteMultipleReadAsync(sqlString, dataSetName);
        }

        public async Task<DataSet> ExecuteMultipleReadAsync(string sqlString, ExpandoObject parameters, string dataSetName)
        {
            return await _provider.ExecuteMultipleReadAsync(sqlString, parameters, dataSetName);
        }


        public string Get_DataBase_Name_From_ConnectionString(string connectionString)
        {
            return _provider.Get_DataBase_Name_From_ConnectionString(connectionString);
        }

        public string Get_Instance_Name_From_ConnectionString(string connectionString)
        {
            return _provider.Get_Instance_Name_From_ConnectionString(connectionString);
        }

        public async Task<bool> Execute_WriteAsync(string sqlString, ExpandoObject parameters)
        {
            return await _provider.Execute_WriteAsync(sqlString, parameters);
        }

        public async Task<bool> Execute_WriteAsync(string sqlString)
        {
            return await _provider.Execute_WriteAsync(sqlString);
        }

        public async Task<(DbConnection?,DbTransaction?)> OpenConnectionAsync(bool OpenTransaction = true, IsolationLevel? level=null)
        {
            return await _provider.OpenConnectionAsync(OpenTransaction,level);
        }

        public void CloseConnection(ref DbConnection? connection,bool RollbackTransaction = false)
        {
            _provider.CloseConnection(ref connection,RollbackTransaction);
        }

        public async Task<DbTransaction?> OpenTransactionAsync(DbConnection connection,IsolationLevel? level=null)
        {
            return await _provider.OpenTransactionAsync(connection,level);
        }

        public void CloseTransaction(ref DbTransaction transaction, bool Rollback = false)
        {
            _provider.CloseTransaction(ref transaction,Rollback);
        }
    }
}
