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

            ConsultaComResolucaoDeIdentidade();
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
    }
}
