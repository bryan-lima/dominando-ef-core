using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Interceptadores
{
    public class InterceptadorDeConexao : DbConnectionInterceptor
    {
        public override InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
        {
            Console.WriteLine("Entrei no método ConnectionOpening");

            string connectionString = ((SqlConnection)connection).ConnectionString;

            Console.WriteLine(connectionString);

            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connectionString) 
            {
                //DataSource = "IP Segundo Servidor",
                ApplicationName = "CursoEFCore"
            };

            connection.ConnectionString = connectionStringBuilder.ToString();

            Console.WriteLine(connectionStringBuilder.ToString());

            return result;
        }
    }
}
