using DominandoEFCore.Data;
using DominandoEFCore.Domain;
using DominandoEFCore.Funcoes;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Transactions;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //FuncaoLEFT();

            //FuncadoDefinidaPeloUsuario();

            DateDIFF();
        }

        static void FuncaoLEFT()
        {
            CadastrarLivro();

            using ApplicationContext _db = new ApplicationContext();

            IQueryable<string> _resultado = _db.Livros.Select(livro => MinhasFuncoes.Left(livro.Titulo, 10));

            foreach (var parteTitulo in _resultado)
            {
                Console.WriteLine(parteTitulo);
            }
        }

        static void CadastrarLivro()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Introdução ao Entity Framework Core",
                        Autor = "Bryan",
                        CadastradoEm = DateTime.Now.AddDays(-1)
                    });

                db.SaveChanges();
            }
        }

        static void FuncadoDefinidaPeloUsuario()
        {
            CadastrarLivro();

            using ApplicationContext db = new ApplicationContext();

            db.Database.ExecuteSqlRaw(@"
                CREATE FUNCTION ConverterParaLetrasMaiusculas(@dados VARCHAR(100))
                RETURNS VARCHAR(100)
                BEGIN
                    RETURN UPPER(@dados)
                END");

            IQueryable<string> _resultado = db.Livros.Select(livro => MinhasFuncoes.LetrasMaiusculas(livro.Titulo));

            foreach (string parteTitulo in _resultado)
            {
                Console.WriteLine(parteTitulo);
            }
        }

        static void DateDIFF()
        {
            CadastrarLivro();

            using var db = new ApplicationContext();

            IQueryable<int> _resultado = db.Livros.Select(livro => EF.Functions.DateDiffDay(livro.CadastradoEm, DateTime.Now));

            foreach (int diff in _resultado)
            {
                Console.WriteLine(diff);
            }
        }
    }
}
