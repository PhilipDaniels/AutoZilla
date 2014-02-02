/*
Object type:
AF = Aggregate function (CLR)
C = CHECK constraint
D = DEFAULT (constraint or stand-alone)
F = FOREIGN KEY constraint
FN = SQL scalar function
FS = Assembly (CLR) scalar-function
FT = Assembly (CLR) table-valued function
IF = SQL inline table-valued function
IT = Internal table
P = SQL Stored Procedure
PC = Assembly (CLR) stored-procedure
PG = Plan guide
PK = PRIMARY KEY constraint
R = Rule (old-style, stand-alone)
RF = Replication-filter-procedure
S = System base table
SN = Synonym
SO = Sequence object
SQ = Service queue
TA = Assembly (CLR) DML trigger
TF = SQL table-valued-function
TR = SQL DML trigger
TT = Table type
U = Table (user-defined)
UQ = UNIQUE constraint
V = View
X = Extended stored procedure
*/

-- =====================================================================================
-- TABLES - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[TableName]', 'U') IS NOT NULL BEGIN
	PRINT 'Dropping table [dbo].[TableName]'
	EXEC('DROP TABLE [dbo].[TableName]');
END
CREATE TABLE [TableName](ID INT)

-- ALTER SYNTAX FORM - NA for tables.

-- =====================================================================================
-- TEMP TABLES - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('tempdb..#TempTableName', 'U') IS NOT NULL BEGIN
	PRINT 'Dropping temp table #TempTableName'
	EXEC('DROP TABLE #TempTableName');
END
CREATE TABLE #TempTableName(ID INT)

-- =====================================================================================
-- VIEWS - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[ViewName]', 'V') IS NOT NULL BEGIN
	PRINT 'Dropping view [dbo].[ViewName]'
	EXEC('DROP VIEW [dbo].[ViewName]');
END
GO
CREATE VIEW [dbo].[ViewName] AS
SELECT 1 as Id
GO

-- ALTER SYNTAX FORM
IF OBJECT_ID('[dbo].[ViewName]', 'V') IS NULL BEGIN
	PRINT '[dbo].[ViewName] did not exist - creating dummy view so ALTER will succeed'
	EXEC('CREATE VIEW [dbo].[ViewName] AS SELECT 1 as Id')
END
GO
ALTER VIEW [dbo].[ViewName] AS
SELECT 1 as Id
GO

-- =====================================================================================
-- PROCS - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[ProcName]', 'P') IS NOT NULL BEGIN
	PRINT 'Dropping proc [dbo].[ProcName]'
	EXEC('DROP PROCEDURE [dbo].[ProcName]');
END
GO
CREATE PROCEDURE [dbo].[ProcName] AS
SELECT 1 as Id
GO

-- ALTER SYNTAX FORM
IF OBJECT_ID('[dbo].[ProcName]', 'P') IS NULL BEGIN
	PRINT '[dbo].[ProcName] did not exist - creating dummy proc so ALTER will succeed'
	EXEC('CREATE PROCEDURE [dbo].[ProcName] AS SELECT 1 as Id')
END
GO
ALTER PROCEDURE [dbo].[ProcName] AS
SELECT 1 as Id
GO

-- =====================================================================================
-- INLINE TABLE VALUED FUNCTIONS - DROP/CREATE SYNTAX FORM.
IF OBJECT_ID('[dbo].[FunctionName]', 'IF') IS NOT NULL BEGIN
	PRINT 'Dropping ITVF [dbo].[FunctionName]'
	EXEC('DROP FUNCTION [dbo].[FunctionName]')
END
GO
CREATE FUNCTION [dbo].[FunctionName]()
RETURNS TABLE AS RETURN 
(
SELECT 1 as Id
)
GO

-- ALTER SYNTAX FORM
IF OBJECT_ID('[dbo].[FunctionName]', 'IF') IS NULL BEGIN
	PRINT '[dbo].[FunctionName] did not exist - creating dummy ITVF so ALTER will succeed'
	EXEC('CREATE FUNCTION [dbo].[FunctionName] RETURNS TABLE AS RETURN (SELECT 1 as Id)')
END
GO
ALTER FUNCTION [dbo].[FunctionName]()
RETURNS TABLE AS RETURN 
(
SELECT 1 as Id
)
GO


-- =====================================================================================
-- COLUMNS - CREATE/ALTER SYNTAX FORM.
-- THESE ALL WORK ON TEMP TABLES TOO - JUST USE '#TempTableName#' instead of [dbo.[TableName]
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NULL BEGIN
	PRINT 'Adding column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ADD [ColumnName] NVARCHAR(MAX)
END ELSE BEGIN
	PRINT 'Altering column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ALTER COLUMN [ColumnName] NVARCHAR(MAX)
END

-- DROP FORM (WILL FAIL IF THERE ARE CONSTRAINTS OR INDEXES ON THE COLUMN).
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NOT NULL BEGIN
	PRINT 'Dropping column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] DROP COLUMN [ColumnName];
END

-- ADD NON-NULLABLE COLUMN WITH NAMED DEFAULT.
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NULL BEGIN
	PRINT 'Adding column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ADD [ColumnName] NVARCHAR(MAX) NOT NULL
		CONSTRAINT [DF_TableName_ColumnName] DEFAULT 'Hello World'
END

-- ADD COMPUTED COLUMN
IF COL_LENGTH('[dbo].[TableName]', 'ColumnName') IS NULL BEGIN
	PRINT 'Adding column [dbo].[TableName].[ColumnName]'
	ALTER TABLE [dbo].[TableName] ADD [ColumnName] AS 'Hello ' + cast(ID AS VARCHAR)
END


select * from dbo.TableName
insert into dbo.TableName values (4)

-- =====================================================================================
-- INDEXES - CREATE/ALTER SYNTAX FORM.
-- THESE ALL WORK ON TEMP TABLES TOO - JUST USE '#TempTableName#' instead of [dbo].[TableName]

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IDX_Name' AND object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
END ELSE BEGIN
END
DROP_EXISTING

IF EXISTS (SELECT * FROM sys.indexes WHERE name='IDX_Name' AND object_id = OBJECT_ID('[dbo].[TableName]')) BEGIN
	PRINT 'The index exists'
END

UNIQUE constraints
PRIMARY KEY constraints

ALTER TABLE categories DROP CONSTRAINT PK_Categories;
ALTER TABLE [dbo].[TestTable] ADD  CONSTRAINT [PK_TestTable] PRIMARY KEY CLUSTERED([ID] ASC)
IF OBJECT_ID('CK_ConstraintName', 'C') IS NOT NULL 
    ALTER TABLE dbo.[tablename] DROP CONSTRAINT CK_ConstraintName

