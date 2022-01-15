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
            //FiltroGlobal();

            //IgnoreFiltroGlobal();

            //ConsultaProjetada();

            //ConsultaParametrizada();

            //ConsultaInterpolada();

            //ConsultaComTAG();

            //EntendendoConsulta1xNNx1();

            //DivisaoDeConsulta();

            //CriarStoredProcedure();

            //InserirDadosViaProcedure();

            //CriarStoredProcedureDeConsulta();

            ConsultaViaProcedure();
        }

        static void FiltroGlobal()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }

        static void Setup(ApplicationContext db)
        {
            if (db.Database.EnsureCreated())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bryan Lima",
                                Cpf = "12345678911",
                                Rg = "113332220"
                            }
                        },
                        Excluido = true
                    },
                    new Departamento
                    {
                        Ativo = true,
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bruno Brito",
                                Cpf = "88888888811",
                                Rg = "3100062"
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                Cpf = "77777777711",
                                Rg = "1100062"
                            }
                        }
                    });
                
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        static void IgnoreFiltroGlobal()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.IgnoreQueryFilters().Where(p => p.Id > 0).ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluído: {departamento.Excluido}");
            }
        }

        static void ConsultaProjetada()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.Where(p => p.Id > 0)
                                                .Select(p => new { p.Descricao, Funcionarios = p.Funcionarios.Select(f => f.Nome) })
                                                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\t Nome: {funcionario}");
                }
            }
        }

        static void ConsultaParametrizada()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var id = new SqlParameter()
            {
                Value = 1,
                SqlDbType = System.Data.SqlDbType.Int
            };
            var departamentos = db.Departamentos.FromSqlRaw("SELECT * FROM Departamentos WHERE Id > {0}", id)
                                                .Where(departamento => !departamento.Excluido)
                                                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void ConsultaInterpolada()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var id = 1;
            var departamentos = db.Departamentos.FromSqlInterpolated($"SELECT * FROM Departamentos WHERE Id > {id}")
                                                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void ConsultaComTAG()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.TagWith(@"Estou enviando um comentário para o servidor

                                                         Segundo comentário
                                                         Terceiro comentário")
                                                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void EntendendoConsulta1xNNx1()
        {
            using var db = new ApplicationContext();
            Setup(db);

            //var departamentos = db.Departamentos.Include(departamento => departamento.Funcionarios)
            //                                    .ToList();

            //foreach (var departamento in departamentos)
            //{
            //    Console.WriteLine($"Descrição: {departamento.Descricao}");

            //    foreach (var funcionario in departamento.Funcionarios)
            //    {
            //        Console.WriteLine($"\tNome: {funcionario.Nome}");
            //    }
            //}

            var funcionarios = db.Funcionarios.Include(funcionario => funcionario.Departamento)
                                              .ToList();

            foreach (var funcionario in funcionarios)
            {
                Console.WriteLine($"Nome: {funcionario.Nome} / Descrição Departamento: {funcionario.Departamento.Descricao}");
            }
        }

        static void DivisaoDeConsulta()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.Include(departamento => departamento.Funcionarios)
                                                .Where(departamento => departamento.Id < 3)
                                                //.AsSplitQuery()
                                                .AsSingleQuery()
                                                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"Descrição: {departamento.Descricao}");

                foreach (var funcionario in departamento.Funcionarios)
                {
                    Console.WriteLine($"\tNome: {funcionario.Nome}");
                }
            }
        }

        static void CriarStoredProcedure()
        {
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE CriarDepartamento
                @Descricao VARCHAR(50),
                @Ativo bit
            AS
            BEGIN
                INSERT INTO
                    Departamentos(Descricao, Ativo, Excluido)
                VALUES (@Descricao, @Ativo, 0)
            END
            ";

            using var db = new ApplicationContext();

            db.Database.ExecuteSqlRaw(criarDepartamento);
        }

        static void InserirDadosViaProcedure()
        {
            using var db = new ApplicationContext();

            //db.Database.ExecuteSqlRaw("EXECUTE CriarDepartamento @p0, @p1", new object[] {"Departamento via Procedure", true});
            db.Database.ExecuteSqlRaw("EXECUTE CriarDepartamento @p0, @p1", "Departamento via Procedure", true);
        }

        static void CriarStoredProcedureDeConsulta()
        {
            var getDepartamentos = @"
            CREATE OR ALTER PROCEDURE GetDepartamentos
                @Descricao VARCHAR(50)
            AS
            BEGIN
                SELECT * FROM Departamentos WHERE Descricao LIKE @Descricao + '%'
            END
            ";

            using var db = new ApplicationContext();

            db.Database.ExecuteSqlRaw(getDepartamentos);
        }

        static void ConsultaViaProcedure()
        {
            using var db = new ApplicationContext();

            var departamentos = db.Departamentos.FromSqlRaw("EXECUTE GetDepartamentos @p0", "Departamento")
                                                .ToList();

            foreach (var departamento in departamentos)
            {
                Console.WriteLine(departamento.Descricao);
            }
        }
    }
}
