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
       
        
        //שליפת הנתונים מdata 
        public DataTable GetAllData()
        {
            return ExecuteQuery("SELECT data_id, a,b,c,d FROM t_data");
        }

        public DataTable GetFormulas()
        {
            return ExecuteQuery("SELECT * FROM t_targil");
        }
        
        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        //שמירה לטבלת הלוג
        public void SaveLog(int targilId,string method, float runTime)
        {
            using( SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO t_log (targil_id,method,run_time) VALUES (@id,@method,@time) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", targilId);
                cmd.Parameters.AddWithValue("@method", method);
                cmd.Parameters.AddWithValue("@time", runTime);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        //שמירה לטבלת התוצאות
        public void SaveResult (int dataId, int targilId, string method, float result)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO t_results (data_id,targil_id,method,result) VALUES (@dId,@tId,@meth,@res)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dId", dataId);
                cmd.Parameters.AddWithValue("@tId", targilId);
                cmd.Parameters.AddWithValue("@meth", method);
                cmd.Parameters.AddWithValue("@res", result);

                conn.Open();
                cmd.ExecuteNonQuery();

            }
        }


    }
}
