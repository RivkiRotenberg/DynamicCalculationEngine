import pandas as pd
import numpy as np

#פונצקיה לחישוב הנוסחה
def calculate_formula(df,formula,condition=None,formula_false=None):


    f_true = formula.lower().replace("^","**").replace("power","pow")
    #חישוב נוסחה רגילה
    if pd.isna(condition) or str(condition).strip().upper()=='NULL' or str(condition).strip() == '':
        return df.eval(f_true,engine='python')
    #חישוב נוסחת תנאי
    else:
        cond_clean = str(condition).lower().strip()

        if pd.isna(formula_false) or str(formula_false).strip().upper == 'NULL':
            f_false = "0"
        else:
            f_false = str(formula_false).lower().replace("^","**").replace("power","pow")

        mask = df.eval(cond_clean,engine='python')
        res_true = df.eval(f_true,engine='python')
        res_false = df.eval(f_false,engine='python')
        #בדיקה האם התנאי התקיים אם כן החזרת res_true אם לא החזרת res_false
        return np.where(mask,res_true,res_false)


