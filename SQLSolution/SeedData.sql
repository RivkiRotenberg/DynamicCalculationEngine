DECLARE @i INT = 1;
WHILE @i <= 5
BEGIN
	INSERT INTO t_data (a,b,c,d)
	VALUES (RAND()*10,RAND()*10,RAND()*10,RAND()*10);
	SET @i = @i +1;
END;

