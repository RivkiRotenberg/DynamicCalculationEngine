-- זיהוי אי תאימות בין הנוסחאות 
use DynamicDB
go
SELECT DISTINCT
	t.targil AS [Formula],
	r1.data_id,
	r1.method AS [Method_A],
	r2.method AS [Method_B],
	r1.result AS [Result_A],
	r2.result AS [Result_B],
	ABS(r1.result - r2.result) AS [Difference]
FROM t_results r1
JOIN t_results r2 ON r1.data_id = r2.data_id AND r1.targil_id = r2.targil_id
JOIN t_targil t ON r1.targil_id = t.targil_id
WHERE r1.method < r2.method
	AND ABS(r1.result - r2.result) > 0.001


