USE DynamicDB
GO

IF OBJECT_ID('sp_CalcultateDynamicalsFormulas','P') IS NOT NULL
	DROP PROCEDURE sp_CalculateDynamicFormulas;
GO

ALTER PROCEDURE sp_CalculateDynamicalsFormulas
AS
BEGIN 
	SET NOCOUNT ON;

	DECLARE @FormulaId INT;
    DECLARE @FormulaStr NVARCHAR(MAX);
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @StartTime DATETIME2;
    DECLARE @EndTime DATETIME2;

	IF CURSOR_STATUS('global','formula_cursor')>=1
	BEGIN
		DEALLOCATE formula_cursor;
	END

	DECLARE formula_cursor CURSOR LOCAL FOR
	SELECT CAST(targil_id AS INT), CAST(targil AS NVARCHAR(MAX))
	FROM t_targil

	OPEN formula_cursor;

	FETCH NEXT FROM formula_cursor INTO @FormulaId,@FormulaStr;

	WHILE @@FETCH_STATUS = 0
	BEGIN 
		SET @StartTime = SYSDATETIME();

		DECLARE @IdStr NVARCHAR(10)= CAST(@FormulaId AS NVARCHAR(10));

		SET @SQL = N'INSERT INTO t_results (data_id,targil_id,method,result) ' +
		           N'SELECT data_id, ' + @IdStr + N', ''DB'', (' +@FormulaStr+ N') '+
				   N'FROM t_data';

		BEGIN TRY
			EXEC sp_executesql @SQL;
		END TRY
		BEGIN CATCH
			PRINT 'Error in formula if '+@IdStr+':' + ERROR_MESSAGE();
		END CATCH

		SET @EndTime = SYSDATETIME();

		INSERT INTO t_log (targil_id,method,run_time)
		VALUES (@FormulaId,'DB',CAST(DATEDIFF(MILLISECOND,@StartTime,@EndTime) AS FLOAT) /1000.0);

		FETCH NEXT FROM formula_cursor INTO @FormulaId,@FormulaStr;
	END;

	CLOSE formula_cursor;
	DEALLOCATE formula_cursor;

	PRINT' THE PROCESS FINISHED SUCCESSFULLY';
END;
GO

