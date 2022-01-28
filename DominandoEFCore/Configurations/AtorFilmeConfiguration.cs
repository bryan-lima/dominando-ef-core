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
    public class AtorFilmeConfiguration : IEntityTypeConfiguration<Ator>
    {
        public void Configure(EntityTypeBuilder<Ator> builder)
        {
            //builder.HasMany(ator => ator.Filmes)
            //       .WithMany(filme => filme.Atores)
            //       .UsingEntity(entityTypeBuilder => entityTypeBuilder.ToTable("AtoresFilmes"));

            builder.HasMany(ator => ator.Filmes)
                   .WithMany(filme => filme.Atores)
                   .UsingEntity<Dictionary<string, object>>("FilmesAtores", 
                                                            entityTypeBuilder => entityTypeBuilder.HasOne<Filme>()
                                                                                                  .WithMany()
                                                                                                  .HasForeignKey("FilmeId"),
                                                            entityTypeBuilder => entityTypeBuilder.HasOne<Ator>()
                                                                                                  .WithMany()
                                                                                                  .HasForeignKey("AtorId"),
                                                            entityTypeBuilder => 
                                                            {
                                                                entityTypeBuilder.Property<DateTime>("CadastradoEm")
                                                                                 .HasDefaultValueSql("GETDATE()");
                                                            });
        }
    }
}
