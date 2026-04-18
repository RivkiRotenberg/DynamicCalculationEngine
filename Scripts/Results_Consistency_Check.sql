
--Comparison between methods for each formula was performed 
--using Dynamic Pivot SQL to ensure scalability and adaptability for any future updates.

DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)
DECLARE @ColumnName AS NVARCHAR(MAX)

SELECT @ColumnName = ISNULL(@ColumnName + ',','')+QUOTENAME(method)
FROM (SELECT DISTINCT method FROM t_results) AS Methods

SET @DynamicPivotQuery = 
	N'SELECT Formula_Text,data_id, ' +@ColumnName + '
	  FROM (
		  SELECT
			t.targil AS Formula_TEXT,
			r.data_id,
			r.method,
			r.result
		  FROM t_results r
		  JOIN t_targil t ON r.targil_id = t.targil_id
	  )AS SourceTable
	  PIVOT (
		AVG(result)
		FOR method IN (' + @ColumnName + ')
	  ) AS PivotTable
	  ORDER BY Formula_Text,data_id;'


	  EXEC sp_executesql @DynamicPivotQuery;
