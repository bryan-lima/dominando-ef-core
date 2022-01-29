using DominandoEFCore.Configurations;
using DominandoEFCore.Conversores;
using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Ator> Atores { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Instrutor> Instrutores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");
        public DbSet<Atributo> Atributos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=DESKTOP-B76722G\\SQLEXPRESS; Initial Catalog=DominandoEFCore; User ID=developer; Password=dev*10; Integrated Security=True; Persist Security Info=False; Pooling=False; MultipleActiveResultSets=False; Encrypt=False; Trusted_Connection=False";

            optionsBuilder.UseSqlServer(strConnection)
                          .LogTo(Console.WriteLine, LogLevel.Information)
                          .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            //modelBuilder.Entity<Departamento>()
            //            .Property(departamento => departamento.Descricao)
            //            .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            //modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
            //            .StartsAt(1)
            //            .IncrementsBy(2)
            //            .HasMin(1)
            //            .HasMax(10)
            //            .IsCyclic();    //Reinicia o valor sequencial após atingir o valor limite definido

            //modelBuilder.Entity<Departamento>()
            //            .Property(departamento => departamento.Id)
            //            .HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia"); //Varia de acordo com o banco de dados, este exemplo é para SQL Server

            //modelBuilder.Entity<Departamento>()
            //            .HasIndex(departamento => new { departamento.Descricao, departamento.Ativo })
            //            .HasDatabaseName("idx_meu_indice_composto")
            //            .HasFilter("Descricao IS NOT NULL")
            //            .HasFillFactor(80)  //80% -> Logo, 20% da folha de preenchimento de dados fica reservado para que o SQL Server possa ser mais otimizado e utilizar o espaço em benefício próprios
            //            .IsUnique();

            //modelBuilder.Entity<Estado>().HasData(new[]
            //{
            //    new Estado { Id = 1, Nome = "Sao Paulo" },
            //    new Estado { Id = 2, Nome = "Sergipe" }
            //});

            //modelBuilder.HasDefaultSchema("cadastros");

            //modelBuilder.Entity<Estado>()
            //            .ToTable("Estados", "SegundoEsquema");

            //ValueConverter<Versao, string> conversao = new ValueConverter<Versao, string>(conversorAoSerSalvoNoBD => conversorAoSerSalvoNoBD.ToString(), 
            //                                                                              obterConversorSalvoNoBD => (Versao)Enum.Parse(typeof(Versao), obterConversorSalvoNoBD));

            //EnumToStringConverter<Versao> conversao1 = new EnumToStringConverter<Versao>();

            ////Para verificar outros conversores
            ////Microsoft.EntityFrameworkCore.Storage.ValueConversion.

            //modelBuilder.Entity<Conversor>()
            //            .Property(conversor => conversor.Versao)
            //            .HasConversion(conversao1);
            //            //.HasConversion(conversao);
            //            //.HasConversion(conversorAoSerSalvoNoBD => conversorAoSerSalvoNoBD.ToString(), obterConversorSalvoNoBD => (Versao)Enum.Parse(typeof(Versao), obterConversorSalvoNoBD)); //Primeiro argumento: como o sistema deverá converter a informação que será salva no banco de dados / Segundo argumento: como o sistema deverá converter a informação que está puxando da base de dados
            //            //.HasConversion<string>();

            //modelBuilder.Entity<Conversor>()
            //            .Property(conversor => conversor.Status)
            //            .HasConversion(new ConversorCustomizado());

            //modelBuilder.Entity<Departamento>()
            //            .Property<DateTime>("UltimaAtualizacao");

            //modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", entityTypeBuilder =>
            {
                entityTypeBuilder.Property<int>("Id");

                entityTypeBuilder.Property<string>("Chave")
                                 .HasColumnType("VARCHAR(40)")
                                 .IsRequired();

                entityTypeBuilder.Property<string>("Valor")
                                 .HasColumnType("VARCHAR(255)")
                                 .IsRequired();
            });
        }
    }
}
