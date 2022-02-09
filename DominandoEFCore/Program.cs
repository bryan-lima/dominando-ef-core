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
            //Setup();

            //ConsultaRastreada();

            //ConsultaNaoRastreada();

            //ConsultaComResolucaoDeIdentidade();

            //ConsultaProjetadaERastreada();

            Inserir200DepartamentosCom1MB();
        }

        static void Setup()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.Add(new Departamento 
            { 
                Descricao = "Departamento Teste",
                Ativo = true,
                Funcionarios = Enumerable.Range(1, 100)
                                         .Select(numero => new Funcionario
                {
                    Cpf = numero.ToString().PadLeft(11, '0'),
                    Nome = $"Funcionário {numero}",
                    Rg = numero.ToString()
                }).ToList()
            });

            db.SaveChanges();
        }

        static void ConsultaRastreada()
        {
            using ApplicationContext db = new ApplicationContext();

            List<Funcionario> _funcionarios = db.Funcionarios.Include(funcionario => funcionario.Departamento)
                                                             .ToList();
        }

        static void ConsultaNaoRastreada()
        {
            using ApplicationContext db = new ApplicationContext();

            List<Funcionario> _funcionarios = db.Funcionarios.AsNoTracking()
                                                             .Include(funcionario => funcionario.Departamento)
                                                             .ToList();
        }

        static void ConsultaComResolucaoDeIdentidade()
        {
            using ApplicationContext db = new ApplicationContext();

            List<Funcionario> _funcionarios = db.Funcionarios.AsNoTrackingWithIdentityResolution()
                                                             .Include(funcionario => funcionario.Departamento)
                                                             .ToList();
        }

        static void ConsultaCustomizada()
        {
            using ApplicationContext db = new ApplicationContext();

            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            List<Funcionario> _funcionarios = db.Funcionarios.Include(funcionario => funcionario.Departamento)
                                                             .ToList();
        }

        static void ConsultaProjetadaERastreada()
        {
            using ApplicationContext db = new ApplicationContext();

            var _departamentos = db.Departamentos.Include(departamento => departamento.Funcionarios)
                                                 .Select(departamento => new
                                                 {
                                                     Departamento = departamento,
                                                     TotalFuncionarios = departamento.Funcionarios.Count()
                                                 })
                                                 .ToList();

            _departamentos[0].Departamento.Descricao = "Departamento Teste Atualizado";

            db.SaveChanges();
        }

        static void Inserir200DepartamentosCom1MB()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Random _random = new Random();

            db.Departamentos.AddRange(Enumerable.Range(1, 200)
                                                .Select(numero => 
                                                new Departamento 
                                                { 
                                                    Descricao = "Departamento Teste",
                                                    Image = getBytes()
                                                }));

            db.SaveChanges();

            byte[] getBytes()
            {
                byte[] _buffer = new byte[1024 + 1024];
                _random.NextBytes(_buffer);

                return _buffer;
            }
        }
    }
}
