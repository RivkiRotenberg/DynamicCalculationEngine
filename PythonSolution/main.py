from config import DB_CONFIG
import pandas as pd
import time
from calculate import calculate_formula
import pyodbc

def get_sql_connection():
    conn_str = (
        f"DRIVER={DB_CONFIG['driver']};"
        f"SERVER={DB_CONFIG['server']};"
        f"DATABASE={DB_CONFIG['database']};"
        f"Trusted_Connection={DB_CONFIG['trusted_connection']}"
    )
    return pyodbc.connect(conn_str)

def run_calculation_process():
    try:
        print("connect to db...")
        conn= get_sql_connection()
        cursor = conn.cursor()

        print("give data")
        df_data = pd.read_sql_query("SELECT data_id, a,b,c,d FROM t_data",conn)

        df_formulas = pd.read_sql_query("SELECT targil_id,targil FROM t_targil",conn)
        print(f"find {len(df_formulas)} formulas")

        for _, row in df_formulas.iterrows():
            t_id = row['targil_id']
            formula = row['targil']

            print(f"calculate {formula}")
            start_time = time.time()
            try:
                results = calculate_formula(df_data, formula)
                end_time = time.time()
                duration = end_time - start_time

                values_to_insert = []
                for idx, res_val in enumerate(results):
                    values_to_insert.append(
                        (
                            int(df_data.iloc[idx]['data_id']),
                            int(t_id),
                            'Python',
                            float(res_val),
                        )
                    )
                cursor.fast_executemany = True
                cursor.executemany(
                    'INSERT INTO t_results (data_id,targil_id,method,result) VALUES (?,?,?,?)', values_to_insert
                )

                cursor.execute(
                    "INSERT INTO t_log (targil_id,method,run_time) VALUES (?,?,?)",
                    (t_id,'python',duration)

                )
                conn.commit()
                print(f" formula {formula} is done in {duration} seconds")

            except Exception as e:
                print(f" error in {formula}: {e}")

        print("all success")
        conn.close()
    except Exception as e:
        print(f" error : {e}")
if __name__ == '__main__':
    run_calculation_process()
