using AgronicaNetCore.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaNetCore.Base.Services.Security
{
    public interface ISecurityService
    {

        string GetConnectionString(string connStr);

        void SetConnectionString(AgronicaCoreParametri obj);

    }
}
