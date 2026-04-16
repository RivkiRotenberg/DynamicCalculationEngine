using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NCalc;

namespace DynamicCalculator_DotNet
{
    internal class Calculator
    {

        private DataTable _helperTable = new DataTable();

        //Calculation with DataTable.compute library
        public float Evaluate(string formula,float a,float b,float c,float d)
        {
            string processed = formula.Replace("a", a.ToString())
                                      .Replace("b", b.ToString())
                                      .Replace("c", c.ToString())
                                      .Replace("d", d.ToString());

            if (processed.Contains("sqrt"))
            {
                return (float)Math.Sqrt((c*c)+(d*d));
            }
            if (processed.Contains("pow"))
            {
                return (float)Math.Pow(a, b);
            }

            DataTable dt = new DataTable();
            return Convert.ToSingle(dt.Compute(processed, ""));
        }

        //Calculation with NCalc library
        public float EvaluateCalculator(string formula,float a,float b ,float c ,float d)
        {
            try
            {
                NCalc.Expression e = new NCalc.Expression(formula);
                e.Parameters["a"] = a;
                e.Parameters["b"] = b;
                e.Parameters["c"] = c;
                e.Parameters["d"] = d;

                var result = e.Evaluate();
                return Convert.ToSingle(result);

              

            }
            catch (Exception ex)
            {
                Console.WriteLine("calculation err"+ex.Message);
                return 0;
            }
        }

    }
}
