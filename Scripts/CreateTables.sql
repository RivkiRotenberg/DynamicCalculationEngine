-- Create the main data table for raw variables (a, b, c, d)
CREATE TABLE t_data (
	data_id INT PRIMARY KEY IDENTITY(1,1),
	a FLOAT NOT NULL,
	b FLOAT NOT NULL,
	c FLOAT NOT NULL,
	d FLOAT NOT NULL
);

-- Create the formulas table to store dynamic logic and conditions
CREATE TABLE t_targil (
	targil_id INT PRIMARY KEY IDENTITY(1,1),
	targil VARCHAR(MAX) NOT NULL,
	tnai VARCHAR(MAX) NULL,
	targil_false VARCHAR(MAX) NULL
);

-- Create the results table to store the output of each calculation method
CREATE TABLE t_results (
	results_id INT PRIMARY KEY IDENTITY(1,1),
	data_id INT NOT NULL,
	targil_id INT NOT NULL,
	method VARCHAR(50) NOT NULL,
	result FLOAT NULL,

	CONSTRAINT FK_data FOREIGN KEY (data_id) REFERENCES t_data(data_id),
	CONSTRAINT FK_targil FOREIGN KEY (targil_id) REFERENCES t_targil(targil_id)
);

-- Create the logging table to measure and compare execution times
CREATE TABLE t_log (
	log_id INT PRIMARY KEY IDENTITY(1,1),
	targil_id INT NOT NULL,
	method VARCHAR(50) NOT NULL,
	run_time FLOAT NULL

	CONSTRAINT FK_log_targil FOREIGN KEY (targil_id) REFERENCES t_targil(targil_id)

);
