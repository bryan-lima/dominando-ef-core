using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //Collations();

            //PropagarDados();

            //Esquema();

            //ConversorDeValor();

            //ConversorCustomizado();

            //PropriedadesDeSombra();

            TrabalhandoComPropriedadesDeSombra();
        }

        static void Collations()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        static void PropagarDados()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            string script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void Esquema()
        {
            using ApplicationContext db = new ApplicationContext();

            string script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void ConversorDeValor() => Esquema();

        static void ConversorCustomizado()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Conversor
                {
                    Status = Status.Devolvido,
                });

            db.SaveChanges();

            Conversor conversorEmAnalise = db.Conversores.AsNoTracking()
                                                         .FirstOrDefault(conversor => conversor.Status == Status.Analise);
            
            Conversor conversorDevolvido = db.Conversores.AsNoTracking()
                                                         .FirstOrDefault(conversor => conversor.Status == Status.Devolvido);
        }
        static void PropriedadesDeSombra()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        static void TrabalhandoComPropriedadesDeSombra()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Departamento departamento = new Departamento
            {
                Descricao = "Departamento Propriedade de Sombra"
            };

            db.Departamentos.Add(departamento);

            db.Entry(departamento)
              .Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

            db.SaveChanges();
        }
    }
}
