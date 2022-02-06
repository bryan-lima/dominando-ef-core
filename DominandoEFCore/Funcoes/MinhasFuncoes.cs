using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominandoEFCore.Funcoes
{
    public static class MinhasFuncoes
    {
        [DbFunction(name: "Left", schema: "", IsBuiltIn = true)]
        public static string Left(string dados, int quantidade)
        {
            throw new NotImplementedException();
        }

        public static void RegistrarFuncoes(ModelBuilder modelBuilder)
        {
            IEnumerable<System.Reflection.MethodInfo> _funcoes = typeof(MinhasFuncoes).GetMethods()
                                                                                      .Where(methodInfo => Attribute.IsDefined(methodInfo, typeof(DbFunctionAttribute)));

            foreach (System.Reflection.MethodInfo funcao in _funcoes)
            {
                modelBuilder.HasDbFunction(funcao);
            }
        }

        public static string LetrasMaiusculas(string dados)
        {
            throw new NotImplementedException();
        }
    }
}
