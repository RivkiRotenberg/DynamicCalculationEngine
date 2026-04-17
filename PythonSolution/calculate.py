import pandas as pd
import numpy as np
def calculate_formula(df,formula):



    f_clean = formula.lower()
    f_clean = f_clean.replace('^', '**')
    f_clean = f_clean.replace('power', 'pow')

    return df.eval(f_clean,engine='python')