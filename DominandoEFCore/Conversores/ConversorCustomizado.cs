using DominandoEFCore.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Conversores
{
    public class ConversorCustomizado : ValueConverter<Status, string>
    {
        public ConversorCustomizado() : base(statusParaSalvarNoBD => ConverterParaBancoDeDados(statusParaSalvarNoBD),
                                             statusConsultadoDoBD => ConverterParaAplicacao(statusConsultadoDoBD),
                                             new ConverterMappingHints(1))
        {

        }

        static string ConverterParaBancoDeDados(Status status)
        {
            return status.ToString()[0..1];
        }

        static Status ConverterParaAplicacao(string value)
        {
            Status status = Enum.GetValues<Status>()
                                .FirstOrDefault(status => status.ToString()[0..1] == value);
            
            return status;
        }
    }
}
