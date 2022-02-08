using DominandoEFCore.Configurations;
using DominandoEFCore.Conversores;
using DominandoEFCore.Domain;
using DominandoEFCore.Funcoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
        public DbSet<Livro> Livros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=DESKTOP-B76722G\\SQLEXPRESS; Initial Catalog=DominandoEFCore; User ID=developer; Password=dev*10; Integrated Security=True; Persist Security Info=False; Pooling=False; MultipleActiveResultSets=False; Encrypt=False; Trusted_Connection=False";

            optionsBuilder.UseSqlServer(strConnection)
                          .LogTo(Console.WriteLine, LogLevel.Information)
                          .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //MinhasFuncoes.RegistrarFuncoes(modelBuilder);

            modelBuilder.HasDbFunction(_minhaFuncao)
                        .HasName("LEFT")
                        .IsBuiltIn();

            modelBuilder.HasDbFunction(_letrasMaiusculas)
                        .HasName("ConverterParaLetrasMaiusculas")   // Nome da função que será criada na base de dados
                        .HasSchema("dbo");

            modelBuilder.HasDbFunction(_dateDiff)
                        .HasName("DATEDIFF")
                        .HasTranslation(p => 
                        {
                            List<SqlExpression> argumentos = p.ToList();

                            SqlConstantExpression constante = (SqlConstantExpression)argumentos[0];
                            argumentos[0] = new SqlFragmentExpression(constante.Value.ToString());

                            return new SqlFunctionExpression("DATEDIFF", argumentos, false, new[] { false, false, false }, typeof(int), null);
                        })
                        .IsBuiltIn();
        }

        private static MethodInfo _minhaFuncao = typeof(MinhasFuncoes).GetRuntimeMethod("Left", new[] { typeof(string), typeof(int) });

        private static MethodInfo _letrasMaiusculas = typeof(MinhasFuncoes).GetRuntimeMethod(nameof(MinhasFuncoes.LetrasMaiusculas), new[] { typeof(string) });

        private static MethodInfo _dateDiff = typeof(MinhasFuncoes).GetRuntimeMethod(nameof(MinhasFuncoes.DateDiff), new[] { typeof(string), typeof(DateTime), typeof(DateTime) });

        //[DbFunction(name: "Left", schema: "", IsBuiltIn = true)]
        //public static string Left(string dados, int quantidade)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
