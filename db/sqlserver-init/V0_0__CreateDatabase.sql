IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'tester-db')
BEGIN
  CREATE DATABASE [tester-db]
END;
GO

USE [tester-db];
GO

CREATE TABLE samples (
	sample_id nvarchar(450) NOT NULL PRIMARY KEY,
	value float NOT NULL,
	as_of_date_utc datetime NOT NULL
);
GO

USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'sqluser')
BEGIN
    CREATE LOGIN [sqluser] WITH PASSWORD = 'sqlpass', CHECK_POLICY = OFF;
    ALTER SERVER ROLE [sysadmin] ADD MEMBER [sqluser];
END
GO
