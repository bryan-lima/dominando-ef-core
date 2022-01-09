using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Data
{
    public class ApplicationContextCidade : DbContext
    {
        public DbSet<Cidade> Cidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=DESKTOP-B76722G\\SQLEXPRESS; Initial Catalog=DominandoEFCore; User ID=developer; Password=dev*10; Integrated Security=True; Persist Security Info=False; Pooling=False; MultipleActiveResultSets=False; Encrypt=False; Trusted_Connection=False";

            optionsBuilder.UseSqlServer(strConnection)
                          .EnableSensitiveDataLogging()
                          .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
