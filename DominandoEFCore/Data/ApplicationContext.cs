using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly StreamWriter _writer = new StreamWriter("Logs_EFCore.txt", append: true);

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

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

            modelBuilder.Entity<Departamento>()
                        .HasIndex(departamento => departamento.Descricao);
        }
    }
}
