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
using System.Text.Json;
using System.Transactions;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            FuncaoLEFT();
        }

        static void FuncaoLEFT()
        {
            CadastrarLivro();

            using ApplicationContext _db = new ApplicationContext();

            IQueryable<string> _resultado = _db.Livros.Select(livro => ApplicationContext.Left(livro.Titulo, 10));

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
                        Autor = "Bryan"
                    });

                db.SaveChanges();
            }
        }
    }
}
