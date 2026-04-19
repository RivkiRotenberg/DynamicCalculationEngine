USE DynamicDB
GO

IF OBJECT_ID('sp_CalcultateDynamicalsFormulas','P') IS NOT NULL
	DROP PROCEDURE sp_CalculateDynamicFormulas;
GO

ALTER PROCEDURE sp_CalculateDynamicalsFormulas
AS
BEGIN 
	SET NOCOUNT ON;
	--הגדרת משתנים
	DECLARE @FormulaId INT;
    DECLARE @FormulaStr NVARCHAR(MAX);
	DECLARE @Condition NVARCHAR(MAX);
	DECLARE @FormulaFalse NVARCHAR(MAX);
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @StartTime DATETIME2;
    DECLARE @EndTime DATETIME2;

	
	--הגדרת מצביע לטבלת התרגילים כדי לחשב כל נוסחה
	DECLARE formula_cursor CURSOR LOCAL FOR
	SELECT targil_id , targil ,tnai,targil_false
	FROM t_targil

	OPEN formula_cursor;
	FETCH NEXT FROM formula_cursor INTO @FormulaId,@FormulaStr,@Condition,@FormulaFalse;

	--כל עוד השליפה מצליחה
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		SET @StartTime = SYSDATETIME();

		
			

		SET @Condition = REPLACE(@Condition, '==', '=');

		DECLARE @IdStr NVARCHAR(10)= CAST(@FormulaId AS NVARCHAR(10));
		--בדיקה אם קיימת נוסחת תנאי
		IF (LEN(@Condition)>0 AND UPPER(@Condition) <> 'NULL')
		BEGIN
			--t_results חישבות שאילתה זמינית לחישוב התוצאות והכנסה שלהן לטבלה 
			SET @SQL = N'INSERT INTO t_results (data_id,targil_id,method,result) '+
					   N'SELECT data_id, ' +@IdStr + N', ''DB'', '+
					           N'CASE WHEN ('+@Condition+ N') THEN CAST((' + @FormulaStr + N') AS FLOAT) ' +
							   N'ELSE CAST((' + ISNULL(@FormulaFalse, '0' ) + N') AS FLOAT) END ' +
							   N'FROM t_data';
		END
		ELSE
		BEGIN
			-- t_results  במקרה ואין נוסחת תנאי חישוב הנוסחה הרגילה והכנסת התוצאה לטבלה   
			SET @SQL = N'INSERT INTO t_results (data_id,targil_id,method,result) ' +
					   N'SELECT data_id, ' + @IdStr + N', ''DB'', CAST((' +@FormulaStr+ N') AS FLOAT) '+
				       N'FROM t_data';
		END
		PRINT 'PROCESSING ID' +@IdStr+'whit' +@SQL;
		--נסיון הרצת השאילתה הזמנית
		BEGIN TRY
			EXEC sp_executesql @SQL;
		END TRY
		BEGIN CATCH
			PRINT 'Error in formula if '+@IdStr+':' + ERROR_MESSAGE();
		END CATCH

		SET @EndTime = SYSDATETIME();
		--t_log הכנסת זמן הריצה של הנוסחה לטבלת 
		INSERT INTO t_log (targil_id,method,run_time)
		VALUES (@FormulaId,'DB',CAST(DATEDIFF(MILLISECOND,@StartTime,@EndTime) AS FLOAT) /1000.0);

		FETCH NEXT FROM formula_cursor INTO @FormulaId,@FormulaStr,@Condition,@FormulaFalse;
	END;

	CLOSE formula_cursor;
	DEALLOCATE formula_cursor;

	PRINT' THE PROCESS FINISHED SUCCESSFULLY';
END;
GO


