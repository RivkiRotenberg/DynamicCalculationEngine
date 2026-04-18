using NCalc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DynamicCalculator_DotNet
{
    internal class Calculator
    {


        private DataTable _helperTable = new DataTable();



        //פונצקיה שמחליפה את הפרמטרים שהתקבלו לערכים
        private string PrepareFormula(string formula, float a, float b, float c, float d)
        {
            return formula.Replace("a", a.ToString("G"))
                          .Replace("b", b.ToString("G"))
                          .Replace("c", c.ToString("G"))
                          .Replace("d", d.ToString("G"));
        }

        //Calculation with DataTable.compute library
        public float Evaluate( string formulaTrue,string condition,string formulaFalse,float a,float b,float c,float d)
        {
            try
            {
                string chosenFormula = formulaTrue;

                if(!string.IsNullOrEmpty(condition) && condition.ToUpper() != "NULL")
                {
                    string processedCode = PrepareFormula(condition, a, b, c, d).Replace("==", "=");
                    bool isTrue = Convert.ToBoolean(_helperTable.Compute(processedCode, ""));
                    chosenFormula = isTrue ? formulaTrue : formulaFalse;
                }
                
                if(chosenFormula.Contains("sqrt") ) return (float)Math.Sqrt((c*c) +(d*d));
                if (chosenFormula.Contains("pow")) return (float)Math.Pow(a, b);

                string processedFinal = PrepareFormula(chosenFormula, a, b, c, d);
                return Convert.ToSingle(_helperTable.Compute(chosenFormula, ""));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        //Calculation with NCalc library 
        public float EvaluateNCalc(string formulaTrue,string condition,string formulaFalse,float a,float b ,float c ,float d)
        {
            try
            {
                string chosenFormula = formulaTrue;

                if (!string.IsNullOrEmpty(condition) && condition.ToUpper() != "NULL")
                {
                    NCalc.Expression eCode = new NCalc.Expression(condition);
                    eCode.Parameters["a"] = a;
                    eCode.Parameters["b"] = b;
                    eCode.Parameters["c"] = c;
                    eCode.Parameters["d"] = d;

                    bool isTrue = Convert.ToBoolean(eCode.Evaluate());
                    chosenFormula = isTrue ? formulaTrue : formulaFalse;
                }
                    if (string.IsNullOrEmpty(chosenFormula) || chosenFormula.ToUpper() == "NULL")
                    return 0;

                NCalc.Expression eCalc = new NCalc.Expression(chosenFormula);
                eCalc.Parameters["a"] = a;
                eCalc.Parameters["b"] = b;
                eCalc.Parameters["c"] = c;
                eCalc.Parameters["d"] = d;
                return Convert.ToSingle(eCalc.Evaluate());

            }
            catch (Exception ex)
            {
                Console.WriteLine("calculation err"+ex.Message);
                return 0;
            }
        }
       

    }
}
