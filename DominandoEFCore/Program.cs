﻿using DominandoEFCore.Data;
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
            //TesteInterceptacao();

            TesteInterceptacaoSaveChanges();
        }

        static void TesteInterceptacao()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Funcao _consulta = db.Funcoes.TagWith("Use NOLOCK")
                                             .FirstOrDefault();

                Console.WriteLine($"Consulta: {_consulta?.Descricao1}");
            }
        }

        static void TesteInterceptacaoSaveChanges()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Funcoes.Add(new Funcao 
                {
                    Descricao1 = "Teste"
                });

                db.SaveChanges();
            }
        }
    }
}
