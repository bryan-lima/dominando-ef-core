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
            //FuncoesDeDatas();

            //FuncaoLike();

            //FuncaoDataLength();

            //FuncaoProperty();

            FuncaoCollate();
        }

        public static void FuncoesDeDatas()
        {
            ApagarCriarBancoDeDados();

            using (ApplicationContext db = new ApplicationContext())
            {
                var _script = db.Database.GenerateCreateScript();
                
                Console.WriteLine(_script);

                var _dados = db.Funcoes.AsNoTracking()
                                       .Select(funcao => 
                    new 
                    {
                        Dias = EF.Functions.DateDiffDay(DateTime.Now, funcao.Data1),
                        Meses = EF.Functions.DateDiffMonth(DateTime.Now, funcao.Data1),
                        Data = EF.Functions.DateFromParts(2021, 1, 2),
                        DataValida = EF.Functions.IsDate(funcao.Data2)
                    });

                foreach (var f in _dados)
                {
                    Console.WriteLine(f);
                }
            }
        }

        public static void ApagarCriarBancoDeDados()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Funcoes.AddRange(
            new Funcao
            {
                Data1 = DateTime.Now.AddDays(2),
                Data2 = "2021-01-01",
                Descricao1 = "Bala 1 ",
                Descricao2 = "Bala 1 "
            },
            new Funcao
            {
                Data1 = DateTime.Now.AddDays(1),
                Data2 = "XX21-01-01",
                Descricao1 = "Bola 2",
                Descricao2 = "Bola 2"
            },
            new Funcao
            {
                Data1 = DateTime.Now.AddDays(1),
                Data2 = "XX21-01-01",
                Descricao1 = "Tela",
                Descricao2 = "Tela"
            });

            db.SaveChanges();
        }

        public static void FuncaoLike()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                string _script = db.Database.GenerateCreateScript();

                Console.WriteLine(_script);

                string[] _dados = db.Funcoes.AsNoTracking()
                                            //.Where(funcao => EF.Functions.Like(funcao.Descricao1, "Bo%"))
                                            .Where(funcao => EF.Functions.Like(funcao.Descricao1, "B[ao]%"))
                                            .Select(funcao => funcao.Descricao1)
                                            .ToArray();

                Console.WriteLine("Resultado:");
                foreach (string descricao in _dados)
                {
                    Console.WriteLine(descricao);
                }
            }
        }

        public static void FuncaoDataLength()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var _resultado = db.Funcoes.AsNoTracking()
                                           .Select(funcao => new 
                { 
                    TotalBytesCampoData = EF.Functions.DataLength(funcao.Data1),
                    TotalBytes1 = EF.Functions.DataLength(funcao.Descricao1),
                    TotalBytes2 = EF.Functions.DataLength(funcao.Descricao2),
                    Total1 = funcao.Descricao1.Length,
                    Total2 = funcao.Descricao2.Length,
                }).FirstOrDefault();

                Console.WriteLine("Resultado:");
                Console.WriteLine(_resultado);
            }
        }

        public static void FuncaoProperty()
        {
            ApagarCriarBancoDeDados();

            using (ApplicationContext db = new ApplicationContext())
            {
                Funcao _resultado = db.Funcoes//.AsNoTracking()
                                              .FirstOrDefault(funcao => EF.Property<string>(funcao, "PropriedadeSombra") == "Teste");

                string _propriedadeSombra = db.Entry(_resultado)
                                              .Property<string>("PropriedadeSombra").CurrentValue;

                Console.WriteLine("Resultado:");
                Console.WriteLine(_propriedadeSombra);
            }
        }

        public static void FuncaoCollate()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Funcao _consulta1 = db.Funcoes.FirstOrDefault(funcao => EF.Functions.Collate(funcao.Descricao1, "SQL_Latin1_General_CP1_CS_AS") == "tela"); //CS = Case Sensitive

                Funcao _consulta2 = db.Funcoes.FirstOrDefault(funcao => EF.Functions.Collate(funcao.Descricao1, "SQL_Latin1_General_CP1_CI_AS") == "tela"); //CI = Case Insensitive

                Console.WriteLine($"Consulta1: {_consulta1?.Descricao1}");
                
                Console.WriteLine($"Consulta1: {_consulta2?.Descricao2}");
            }
        }
    }
}
