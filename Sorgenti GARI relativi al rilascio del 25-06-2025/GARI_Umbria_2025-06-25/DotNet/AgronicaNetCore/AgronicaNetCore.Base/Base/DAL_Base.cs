using AgronicaDataProvider6.Interfaces;
using AgronicaNetCore.Base.Base;
using AgronicaNetCore.Base.Models;
using AgronicaNetCore.Base.Services.Security;
using AgronicaNetCore.Base.Utility;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace AgronicaNetCore.Anagrafe.DAL.Base
{
    public class DAL_Base : Serilog_Base
    {
        protected readonly IDataProvider6Factory? _dataProviderFactory;
        protected readonly ISecurityService? _securityService;
        protected readonly IServiceProvider _serviceProvider;

        public DAL_Base(IServiceProvider provider, bool securityBypass = false) : base(provider)
        {
            _serviceProvider = provider;
            _dataProviderFactory = provider.GetRequiredService<IDataProvider6Factory>();
            if (!securityBypass) _securityService = provider.GetRequiredService<ISecurityService>();
        }


        //protected IDataProvider GetDataProvider(string objParametriString)
        //{
        //    if (_dataProviderFactory == null)
        //        throw new Exception("Riferimento a dataprovider factory null");
        //    var ObjParametri = UtilityAgronica.convertStringtoOBJparametri(objParametriString);
        //    return (IDataProvider)_dataProviderFactory.GetDataProvider(UtilityAgronica.getSqlConnectionFromObjParametri(ObjParametri), ObjParametri.Lingua_Cod);
        //}

        /// <summary>
        /// Recupero nuova istanza d DataProvider
        /// </summary>
        /// <param name="objParametri"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected IDataProvider GetDataProvider(AgronicaCoreParametri objParametri)
        {
            if (_dataProviderFactory == null)
                throw new Exception("Riferimento a dataprovider factory null");
            _securityService?.SetConnectionString(objParametri);
            return (IDataProvider)_dataProviderFactory.GetDataProvider(UtilityAgronica.getSqlConnectionFromObjParametri(objParametri), objParametri.Lingua_Cod, objParametri.objConnessione, objParametri.objTransazione);
        }

        protected Dictionary<Type, List<object>> FormatClauseIn(List<string> elenco)
        {
            var r = new Dictionary<Type, List<object>>();
            r.Add(typeof(string), elenco.OfType<object>().ToList());
            return r;
        }

        protected Dictionary<Type, List<object>> FormatClauseIn(List<int> elenco)
        {
            var r = new Dictionary<Type, List<object>>();
            r.Add(typeof(int), elenco.OfType<object>().ToList());
            return r;
        }
        protected string FormatFiltroAggiuntivo(FiltroAggiuntivo filter, ref Dictionary<string, object> sqlParams, string prefix = "")
        {
            if (filter == null)
            {
                return "";
            }
            if (!filter.CheckMaximumFiltersNumber())
                throw new Exception("Il limite di filtri applicabili è stato superato.");

            if (filter == null || filter.filters.Count == 0)
                return string.Empty;

            bool isFirst = true;
            StringBuilder sb = new();
            
            sb.AppendLine("    AND (");
            foreach (var f in filter.filters)
            {
                sb.AppendLine($"    {f.GetFormattedFilter(ref sqlParams, isFirst, prefix)}");
                isFirst = false;
            }
            sb.AppendLine(" )");

            return sb.ToString();
        }
    }
}
