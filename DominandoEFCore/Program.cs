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
            //Collations();

            //PropagarDados();

            //Esquema();

            //ConversorDeValor();

            //ConversorCustomizado();

            //PropriedadesDeSombra();

            //TrabalhandoComPropriedadesDeSombra();

            //TiposDePropriedades();

            //RelacionamentoUmParaUm();

            //RelacionamentoUmParaMuitos();

            //RelacionamentoMuitosParaMuitos();

            CampoDeApoio();
        }

        static void Collations()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        static void PropagarDados()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            string script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void Esquema()
        {
            using ApplicationContext db = new ApplicationContext();

            string script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void ConversorDeValor() => Esquema();

        static void ConversorCustomizado()
        {
            using ApplicationContext db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Conversor
                {
                    Status = Status.Devolvido,
                });

            db.SaveChanges();

            Conversor conversorEmAnalise = db.Conversores.AsNoTracking()
                                                         .FirstOrDefault(conversor => conversor.Status == Status.Analise);
            
            Conversor conversorDevolvido = db.Conversores.AsNoTracking()
                                                         .FirstOrDefault(conversor => conversor.Status == Status.Devolvido);
        }
        static void PropriedadesDeSombra()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        static void TrabalhandoComPropriedadesDeSombra()
        {
            using ApplicationContext db = new ApplicationContext();

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //Departamento departamento = new Departamento
            //{
            //    Descricao = "Departamento Propriedade de Sombra"
            //};

            //db.Departamentos.Add(departamento);

            //db.Entry(departamento)
            //  .Property("UltimaAtualizacao").CurrentValue = DateTime.Now;

            //db.SaveChanges();

            Departamento[] departamentos = db.Departamentos.Where(departamento => EF.Property<DateTime>(departamento, "UltimaAtualizacao") < DateTime.Now)
                                                           .ToArray();
        }

        static void TiposDePropriedades()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Cliente cliente = new Cliente
            {
                Nome = "Bryan Lima",
                Telefone = "(79) 98888-9999",
                Endereco = new Endereco { Bairro = "Centro", Cidade = "Sao Paulo" }
            };

            db.Clientes.Add(cliente);
            db.SaveChanges();

            List<Cliente> clientes = db.Clientes.AsNoTracking()
                                                .ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };

            clientes.ForEach(cli => 
            {
                string json = JsonSerializer.Serialize(cli, options);
                Console.WriteLine(json);
            });
        }

        static void RelacionamentoUmParaUm()
        {
            using ApplicationContext db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Estado estado = new Estado
            {
                Nome = "Sao Paulo",
                Governador = new Governador { Nome = "Bryan Lima" }
            };

            db.Estados.Add(estado);

            db.SaveChanges();

            List<Estado> estados = db.Estados.AsNoTracking()
                                             .ToList();

            estados.ForEach(est =>
            {
                Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
            });
        }

        static void RelacionamentoUmParaMuitos()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Estado estado = new Estado
                {
                    Nome = "Sao Paulo",
                    Governador = new Governador { Nome = "Bryan Lima" }
                };

                estado.Cidades.Add(new Cidade { Nome = "Sorocaba" });

                db.Estados.Add(estado);

                db.SaveChanges();
            }

            using (ApplicationContext db = new ApplicationContext())
            {
                List<Estado> estados = db.Estados.ToList();

                estados[0].Cidades.Add(new Cidade { Nome = "Atibaia" });

                db.SaveChanges();

                foreach (Estado estado in db.Estados.Include(estado => estado.Cidades)
                                                 .AsNoTracking())
                {
                    Console.WriteLine($"Estado: {estado.Nome}, Governador: {estado.Governador.Nome}");

                    foreach (Cidade cidade in estado.Cidades)
                    {
                        Console.WriteLine($"\t Cidade: {cidade.Nome}");
                    }
                }
            }
        }

        static void RelacionamentoMuitosParaMuitos()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Ator ator1 = new Ator { Nome = "Bryan" };
                Ator ator2 = new Ator { Nome = "Pires" };
                Ator ator3 = new Ator { Nome = "Bruno" };

                Filme filme1 = new Filme { Descricao = "A volta dos que não foram" };
                Filme filme2 = new Filme { Descricao = "De volta para o futuro" };
                Filme filme3 = new Filme { Descricao = "Poeira em alto mar filme" };

                ator1.Filmes.Add(filme1);
                ator1.Filmes.Add(filme2);

                ator2.Filmes.Add(filme1);

                filme3.Atores.Add(ator1);
                filme3.Atores.Add(ator2);
                filme3.Atores.Add(ator3);

                db.AddRange(ator1, ator2, filme3);

                db.SaveChanges();

                foreach (Ator ator in db.Atores.Include(ator => ator.Filmes))
                {
                    Console.WriteLine($"Ator: {ator.Nome}");

                    foreach (Filme filme in ator.Filmes)
                    {
                        Console.WriteLine($"\tFilme: {filme.Descricao}");
                    }
                }
            }
        }

        public static void CampoDeApoio()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Documento _documento = new Documento();
                _documento.SetCPF("12345678933");

                db.Documentos.Add(_documento);
                db.SaveChanges();

                foreach (Documento documento in db.Documentos.AsNoTracking())
                {
                    Console.WriteLine($"CPF -> {documento.GetCPF()}");
                }
            }
        }
    }
}
