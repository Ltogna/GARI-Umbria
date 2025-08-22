using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgronicaDataProvider6.Interfaces
{
    public interface IWriteDataProvider
    {
        string Get_DataBase_Name_From_ConnectionString(string connectionString);

        string Get_Instance_Name_From_ConnectionString(string connectionString);

        Task<bool> Execute_WriteAsync(string sqlString, ExpandoObject parameters);
        Task<bool> Execute_WriteAsync(string sqlString);

    }
}
