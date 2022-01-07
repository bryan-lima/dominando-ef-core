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
            //GapDoEnsureCreated();
            HealthCheckBancoDeDados();
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
            var canConnect = db.Database.CanConnect();

            if (canConnect)
            {
                Console.WriteLine("Posso me conectar");
            }
            else
            {
                Console.WriteLine("Não posso me conectar");
            }
        }
    }
}
