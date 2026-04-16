using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCalculator_DotNet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connString = @"Server = DESKTOP-FM7OT9G; Database = DynamicDB; Trusted_Connection = True;TrustServerCertificate=True;";

            DatabaseManager dbManager = new DatabaseManager(connString);

            Console.WriteLine("---Start filling in data---");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                dbManager.SeedDatabase();
                sw.Stop();
                Console.WriteLine($"A million records were entered in {sw.Elapsed.TotalSeconds:F2} seconds successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error : " + ex.Message);
            }
            Console.WriteLine("לחצי על מקש כלשהו לסיום...");
            Console.ReadKey();
        }
    }
}
