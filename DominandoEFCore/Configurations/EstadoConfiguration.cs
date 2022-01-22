using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.HasOne(estado => estado.Governador)
                   .WithOne(governador => governador.Estado)
                   .HasForeignKey<Governador>(governador => governador.EstadoId);   // Neste caso não é obrígatório informar a chave estrangeira pois já foi definido explicitamente na classe Governador

            builder.Navigation(estado => estado.Governador)
                   .AutoInclude();

            builder.HasMany(estado => estado.Cidades)
                   .WithOne(cidade => cidade.Estado);
        }
    }
}
