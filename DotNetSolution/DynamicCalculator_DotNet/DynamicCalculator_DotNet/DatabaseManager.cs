using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace DynamicCalculator_DotNet
{
    internal class DatabaseManager
    {
        private string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }
        //fill a million records
        public void SeedDatabase()
        {
            DataTable table = new DataTable();
            table.Columns.Add("a", typeof(double));
            table.Columns.Add("b", typeof(double));
            table.Columns.Add("c", typeof(double));
            table.Columns.Add("d", typeof(double));

            Random rand = new Random();
            for (int i = 0; i<1000000; i++)
            {
                float valA = (float)(rand.NextDouble() * 100);
                float valB = (float)(rand.NextDouble() * 100);
                float valC = (float)(rand.NextDouble() * 100);
                float valD = (float)(rand.NextDouble() * 100);
                table.Rows.Add(valA, valB, valC, valD);


            }
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(_connectionString))
            {
                bulkCopy.DestinationTableName = "t_data";

                bulkCopy.ColumnMappings.Add("a", "a");
                bulkCopy.ColumnMappings.Add("b", "b");
                bulkCopy.ColumnMappings.Add("c", "c");
                bulkCopy.ColumnMappings.Add("d", "d");

                bulkCopy.WriteToServer(table);
            }


        }


    }
}
