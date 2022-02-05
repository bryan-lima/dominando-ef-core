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
            //ComportamentoPadrao();

            //GerenciandoTransacaoManualmente();

            //ReverterTransacao();

            //SalvarPontoTransacao();

            TransactionScope();
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

        static void ReverterTransacao()
        {
            CadastrarLivro();

            using (ApplicationContext db = new ApplicationContext())
            {
                IDbContextTransaction _transacao = db.Database.BeginTransaction();

                try
                {
                    Livro _livro = db.Livros.FirstOrDefault(livro => livro.Id == 1);
                    _livro.Autor = "Bryan Lima";
                    db.SaveChanges();

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "Dominando o Entity Framework Core",
                            Autor = "Bryan Lima".PadLeft(16, '*')
                        });

                    db.SaveChanges();
                    _transacao.Commit();
                }
                catch (Exception ex)
                {
                    _transacao.Rollback();
                }
            }
        }

        static void SalvarPontoTransacao()
        {
            CadastrarLivro();

            using (ApplicationContext db = new ApplicationContext())
            {
                IDbContextTransaction _transacao = db.Database.BeginTransaction();

                try
                {
                    Livro _livro = db.Livros.FirstOrDefault(livro => livro.Id == 1);
                    _livro.Autor = "Bryan Lima";
                    db.SaveChanges();

                    _transacao.CreateSavepoint("desfazer_apenas_insercao");

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "ASP.NET Core Enterprise Applications",
                            Autor = "Eduardo Pires"
                        });

                    db.SaveChanges();

                    db.Livros.Add(
                        new Livro
                        {
                            Titulo = "Dominando o Entity Framework Core",
                            Autor = "Bryan Lima".PadLeft(16, '*')
                        });

                    db.SaveChanges();
                    _transacao.Commit();
                }
                catch (DbUpdateException ex)
                {
                    _transacao.RollbackToSavepoint("desfazer_apenas_insercao");

                    if (ex.Entries.Count(entityEntry => entityEntry.State == EntityState.Added) == ex.Entries.Count)
                    {
                        _transacao.Commit();
                    }
                }
            }
        }

        static void TransactionScope()
        {
            CadastrarLivro();

            using (TransactionScope transactionScope = new TransactionScope())
            {
                ConsultarAtualizar();
                CadastrarLivroEnterprise();
                CadastrarLivroDominandoEFCore();

                transactionScope.Complete();
            }
        }

        static void CadastrarLivroDominandoEFCore()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "Introdução ao Entity Framework Core",
                        Autor = "Bryan Lima"
                    });

                db.SaveChanges();
            }
        }

        static void CadastrarLivroEnterprise()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Livros.Add(
                    new Livro
                    {
                        Titulo = "ASP.NET Core Enterprise Applications",
                        Autor = "Eduardo Pires"
                    });

                db.SaveChanges();
            }
        }

        static void ConsultarAtualizar()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Livro _livro = db.Livros.FirstOrDefault(livro => livro.Id == 1);
                _livro.Autor = "Bryan Lima";
                db.SaveChanges();
            }
        }
    }
}
