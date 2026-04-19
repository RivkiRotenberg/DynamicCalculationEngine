--חישוב סך זמן ריצה עבור כל אחת מהנוסחאות
SELECT * 
FROM (
	SELECT
		ISNULL(t.tnai,t.targil) AS [Formula],
		l.method,
		l.run_time
	FROM t_log l
	JOIN t_targil t ON l.targil_id = t.targil_id
) AS SourceTable
PIVOT (	
	SUM(run_time)
	For method IN ([python] , [DB], [DotNet])
) AS PivotTable
		