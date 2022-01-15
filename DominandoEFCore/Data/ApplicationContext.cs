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
                          //.LogTo(Console.WriteLine, LogLevel.Information)
                          //.LogTo(Console.WriteLine, 
                          //       new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted }, 
                          //       LogLevel.Information,
                          //       DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine)
                          //.LogTo(_writer.WriteLine, LogLevel.Information)
                          .EnableDetailedErrors();
        }

        public override void Dispose()
        {
            base.Dispose();
            _writer.Dispose();
        }
    }
}
