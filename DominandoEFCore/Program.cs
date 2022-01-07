using DominandoEFCore.Data;
using System;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            EnsureCreatedAndDeleted();
        }

        static void EnsureCreatedAndDeleted()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureCreated();
        }
    }
}
