using DominandoEFCore.Data;
using DominandoEFCore.Domain;
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
            //EnsureCreatedAndDeleted();
            //GapDoEnsureCreated();
            //HealthCheckBancoDeDados();

            //warmup
            //new ApplicationContext().Departamentos.AsNoTracking().Any();

            //_count = 0;
            //GerenciarEstadoDaConexao(false);

            //_count = 0;
            //GerenciarEstadoDaConexao(true);

            //SqlInjection();

            //MigracoesPendentes();

            //AplicarMigracaoEmTempoDeExecucao();

            //TodasMigracoes();

            //MigracoesJaAplicadas();

            //ScriptGeralDoBancoDeDados();

            //CarregamentoAdiantado();

            CarregamentoExplicito();
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

        static int _count;

        static void GerenciarEstadoDaConexao(bool gerenciarEstadoConexao)
        {
            using var db = new ApplicationContext();
            var time = Stopwatch.StartNew();

            var conexao = db.Database.GetDbConnection();
            conexao.StateChange += (_, __) => ++_count;

            if (gerenciarEstadoConexao)
            {
                conexao.Open();
            }

            for (var i = 0; i < 200; i++)
            {
                db.Departamentos.AsNoTracking().Any();
            }

            time.Stop();
            var mensagem = $"Tempo: {time.Elapsed.ToString()}, {gerenciarEstadoConexao}, Contador: {_count}";

            Console.WriteLine(mensagem);
        }

        static void ExecuteSQL()
        {
            using var db = new ApplicationContext();

            // Primeira opção
            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            // Segunda opção
            var descricao = "TESTE";
            db.Database.ExecuteSqlRaw("UPDATE Departamentos SET Descricao = {0} WHERE Id = 1", descricao);

            // Terceira opção
            db.Database.ExecuteSqlInterpolated($"UPDATE Departamentos SET Descricao = {descricao} WHERE Id = 1");
        }

        static void SqlInjection()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Departamento
                {
                    Descricao = "Departamento 01"
                },
                new Departamento
                {
                    Descricao = "Departamento 02"
                });

            db.SaveChanges();

            var descricao = "Teste ' or 1='1";
            db.Database.ExecuteSqlRaw($"UPDATE Departamentos SET Descricao = 'AtaqueSQLInjection' WHERE Descricao = '{descricao}'");

            foreach (var departamento in db.Departamentos.AsNoTracking())
            {
                Console.WriteLine($"Id: {departamento.Id}, Descrição: {departamento.Descricao}");
            }
        }

        static void MigracoesPendentes()
        {
            using var db = new ApplicationContext();

            var migracoesPendentes = db.Database.GetPendingMigrations();

            Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach (var migracao in migracoesPendentes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void AplicarMigracaoEmTempoDeExecucao()
        {
            using var db = new ApplicationContext();

            db.Database.Migrate();
        }

        static void TodasMigracoes()
        {
            using var db = new ApplicationContext();

            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void MigracoesJaAplicadas()
        {
            using var db = new ApplicationContext();

            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach (var migracao in migracoes)
            {
                Console.WriteLine($"Migração: {migracao}");
            }
        }

        static void ScriptGeralDoBancoDeDados()
        {
            using var db = new ApplicationContext();

            var script = db.Database.GenerateCreateScript();

            Console.WriteLine(script);
        }

        static void CarregamentoAdiantado()
        {
            using var db = new ApplicationContext();

            SetupTiposCarregamentos(db);

            var departamentos = db.Departamentos.Include(i => i.Funcionarios);

            foreach (var departamento in departamentos)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionário: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionário encontrado!");
                }
            }
        }

        static void SetupTiposCarregamentos(ApplicationContext db)
        {
            if (!db.Departamentos.Any())
            {
                db.Departamentos.AddRange(
                    new Departamento
                    {
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Bryan Lima",
                                Cpf = "12345678911",
                                Rg = "1122233"
                            }
                        }
                    },
                    new Departamento
                    {
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario>
                        {
                            new Funcionario
                            {
                                Nome = "Alex Brito",
                                Cpf = "98765432100",
                                Rg = "310062"
                            },
                            new Funcionario
                            {
                                Nome = "Eduardo Pires",
                                Cpf = "98712364599",
                                Rg = "110062"
                            }
                        }
                    });

                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }

        static void CarregamentoExplicito()
        {
            using var db = new ApplicationContext();

            SetupTiposCarregamentos(db);

            var departamentos = db.Departamentos.ToList();

            foreach (var departamento in departamentos)
            {
                if (departamento.Id == 2)
                {
                    // O método Collection aceita tanto string como lambda, conforme exemplos abaixo
                    //db.Entry(departamento).Collection("Funcionarios").Load();
                    db.Entry(departamento).Collection(p => p.Funcionarios).Load();
                }

                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"Departamento: {departamento.Descricao}");

                if (departamento.Funcionarios?.Any() ?? false)
                {
                    foreach (var funcionario in departamento.Funcionarios)
                    {
                        Console.WriteLine($"\tFuncionário: {funcionario.Nome}");
                    }
                }
                else
                {
                    Console.WriteLine($"\tNenhum funcionário encontrado!");
                }
            }
        }
    }
}
