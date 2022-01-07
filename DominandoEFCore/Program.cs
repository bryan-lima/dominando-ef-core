using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //EnsureCreatedAndDeleted();
            GapDoEnsureCreated();
        }

        static void EnsureCreatedAndDeleted()
        {
            using var db = new ApplicationContext();
            //db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }

        static void GapDoEnsureCreated()
        {
            using var db1 = new ApplicationContext();
            using var db2 = new ApplicationContextCidade();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }

        static void HealthCheckBancoDeDados()
        {
            using var db = new ApplicationContext();

            try
            {
                // 1 - Método mais utilizado para checar conexão com banco de dados
                var connection = db.Database.GetDbConnection();
                connection.Open();

                // 2 - Outro método que possibilita validar a conexão com banco de dados
                db.Departamentos.Any();

                Console.WriteLine("Posso me conectar");
            }
            catch (Exception)
            {
                Console.WriteLine("Não posso me conectar");
            }
        }
    }
}
