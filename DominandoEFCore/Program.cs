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

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //ComportamentoPadrao();

            GerenciandoTransacaoManualmente();
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

        static void ComportamentoPadrao()
        {
            CadastrarLivro();

            using (ApplicationContext db = new ApplicationContext())
            {
                var _livro = db.Livros.FirstOrDefault(livro => livro.Id == 1);
                _livro.Autor = "Bryan Lima";

                db.Livros.Add(
                    new Livro 
                    {
                        Titulo = "Dominando o Entity Framework Core",
                        Autor = "Bryan Lima"
                    });

                db.SaveChanges();
            }
        }

        static void GerenciandoTransacaoManualmente()
        {
            CadastrarLivro();

            using (ApplicationContext db = new ApplicationContext())
            {
                IDbContextTransaction _transacao = db.Database.BeginTransaction();

                Livro _livro = db.Livros.FirstOrDefault(livro => livro.Id == 1);
                _livro.Autor = "Bryan Lima";
                db.SaveChanges();

                Console.ReadKey();

                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Dominando o Entity Framework Core",
                        Autor = "Bryan Lima"
                    });

                db.SaveChanges();
                _transacao.Commit();
            }
        }
    }
}
