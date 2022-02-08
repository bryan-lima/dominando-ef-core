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
            Setup();
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
    }
}
