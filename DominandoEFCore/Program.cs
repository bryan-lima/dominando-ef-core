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

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConsultarDepartamentos();

            DadosSensiveis();
        }

        static void ConsultarDepartamentos()
        {
            using ApplicationContext db = new ApplicationContext();

            Departamento[] departamentos = db.Departamentos.Where(departamento => departamento.Id > 0)
                                                           .ToArray();
        }

        static void DadosSensiveis()
        {
            using ApplicationContext db = new ApplicationContext();

            string descricao = "Departamento";
            Departamento[] departamentos = db.Departamentos.Where(departamento => departamento.Descricao.Equals(descricao))
                                                           .ToArray();
        }
    }
}
