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
            //FiltroGlobal();

            //IgnoreFiltroGlobal();

            ConsultaProjetada();
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
    }
}
