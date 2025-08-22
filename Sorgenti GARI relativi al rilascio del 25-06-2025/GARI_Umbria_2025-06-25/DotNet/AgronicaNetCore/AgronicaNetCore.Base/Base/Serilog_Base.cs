using AgronicaNetCore.Anagrafe.DAL.Base;
using AgronicaNetCore.Base.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Base.Base
{
    public class Serilog_Base
    {
        private readonly ILogger<Serilog_Base> _logger;
        public Serilog_Base(IServiceProvider provider) {
            _logger = provider.GetRequiredService<ILogger<Serilog_Base>>();
        }

        private void WriteLog(LogLevel level,Exception ex, string message, params object[] pars)
        {
            if (ex == null)
                _logger.Log(level, message, pars);
            else
                _logger.Log(level,ex, message, pars);
        }
        private KeyValuePair<string, object> getKeyContext(AgronicaCoreParametri? objParametri)
        {
            var keyContext = new KeyValuePair<string, object>();
            if (objParametri == null)
                keyContext = new KeyValuePair<string, object>("General", true);
            else
                keyContext = new KeyValuePair<string, object>("LogPath", Path.Combine(objParametri.LogDirectory, Path.GetFileNameWithoutExtension(objParametri.LogFileName)));

            return keyContext;
        }

        protected void LogTrace(string msg, AgronicaCoreParametri? objParametri = null, Exception? ex=null, params object?[] pars)
        {
            var keyContext = getKeyContext(objParametri);          

            using (LogContext.PushProperty(keyContext.Key, keyContext.Value))
            {
                WriteLog(LogLevel.Trace, ex, msg, pars);
            }

        }
        protected void LogDebug(string msg, AgronicaCoreParametri? objParametri = null, Exception? ex = null, params object?[] pars)
        {
            var keyContext = getKeyContext(objParametri);

            using (LogContext.PushProperty(keyContext.Key, keyContext.Value))
            {
                WriteLog(LogLevel.Debug, ex, msg, pars);
            }
        }
        protected void LogInformation(string msg, AgronicaCoreParametri? objParametri = null, Exception? ex = null, params object?[] pars)
        {
            var keyContext = getKeyContext(objParametri);

            using (LogContext.PushProperty(keyContext.Key, keyContext.Value))
            {
                WriteLog(LogLevel.Information, ex, msg, pars);
            }
        }
        protected void LogWarning(string msg, AgronicaCoreParametri? objParametri = null, Exception? ex = null, params object?[] pars)
        {
            var keyContext = getKeyContext(objParametri);

            using (LogContext.PushProperty(keyContext.Key, keyContext.Value))
            {
                WriteLog(LogLevel.Warning, ex, msg, pars);
            }
        }
        protected void LogError(string msg, AgronicaCoreParametri? objParametri = null, Exception? ex = null, params object?[] pars)
        {
            var keyContext = getKeyContext(objParametri);

            using (LogContext.PushProperty(keyContext.Key, keyContext.Value))
            {
                WriteLog(LogLevel.Error, ex, msg, pars);
            }
        }
        protected void LogCritical(string msg, AgronicaCoreParametri? objParametri = null, Exception? ex = null, params object?[] pars)
        {
            var keyContext = getKeyContext(objParametri);

            using (LogContext.PushProperty(keyContext.Key, keyContext.Value))
            {
                WriteLog(LogLevel.Critical, ex, msg, pars);
            }
        }
    }
}
