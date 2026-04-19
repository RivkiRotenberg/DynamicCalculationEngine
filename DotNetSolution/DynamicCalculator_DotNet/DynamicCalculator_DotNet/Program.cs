using System;
using System.Collections.Generic;
using System.Data;
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
            Calculator calc = new Calculator();

            Console.WriteLine("---Start filling in data---");
            

            try
            {
                
                Console.WriteLine("\n--- starting calculation phase ---");

                DataTable formulas = dbManager.GetFormulas();

                Console.WriteLine("loading 1000000 records into memory...");

                DataTable data = dbManager.GetAllData();

                foreach (DataRow formulaRow in formulas.Rows)
                {
                    int id = Convert.ToInt32(formulaRow["targil_id"]);

                    //שליפת נתוני הנוסחאות מהטבלה
                    string formulaStr = formulaRow["targil"].ToString();
                    string condition = formulaRow["tnai"]?.ToString() ?? "NULL";
                    string formulaFalse = formulaRow["targil_false"]?.ToString() ?? "NULL";

                    Stopwatch swCalc = Stopwatch.StartNew();

                    foreach (DataRow row in data.Rows)
                    {
                        //שליפת נתוני המשתנים מהטבלה
                        int dataId = Convert.ToInt32(row["data_id"]);
                        float a = Convert.ToSingle(row["a"]);
                        float b = Convert.ToSingle(row["b"]);
                        float c = Convert.ToSingle(row["c"]);
                        float d = Convert.ToSingle(row["d"]);

                        //שליחה לחישוב תוצאה ע"י אחת מהשיטות
                        //float result = calc.Evaluate(formulaStr,condition,formulaFalse, a, b, c, d);
                        float result = calc.EvaluateNCalc(formulaStr,condition,formulaFalse, a, b, c, d);

                      //  Console.WriteLine($"A:{a:F2} | B:{b:F2} | C:{c:F2} | D:{d:F2} => Result: {result:F2}");
                        //t_result שמירת התוצאה לטבלת  
                        dbManager.SaveResult(dataId,id,"DotNet",result);



                    }

                    swCalc.Stop();
                    //חישוב זמן ריצה של הנוסחאות
                    float totalSeconds = (float)swCalc.Elapsed.TotalSeconds;
                    //t_log שמירת התוצאה לטבלת  
                    dbManager.SaveLog(id,"DotNet",totalSeconds);
                    Console.WriteLine($"finished! time : {totalSeconds:F4}");
                }
                Console.WriteLine("all finished!");
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
