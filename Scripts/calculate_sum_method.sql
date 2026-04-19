--חישוב זמן הריצה הכולל עבור כל אחת מהשיטות
select l.method,SUM(l.run_time) AS [sum_runTime]
FROM t_log l
GROUP BY l.method
  