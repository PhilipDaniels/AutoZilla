create proc dbo.SetContextInfo
	(
	@UserId varchar(127),
	@Locale varchar(5)
	)
as
/*
 * This procedure associates a user id and a locale with the current connection.
 * It requires a dbo.UserContext table to be created.
 * 
 * Date       Author              Description
 * 2014-05-21 Philip Daniels      Initial version.
 */

-- Place the UserId into the master.dbo.sysprocesses.context_info column.
-- It's a binary, so we need to seem to store the string length as
-- the first byte so we can get it back out again.
declare
	@ctx varbinary(128),
	@length tinyint

select @length = len(@UserId)
set @ctx = convert(binary(1), @length) + convert(varbinary(127), @UserId)
set context_info @ctx

-- Place associated data into our UserContext table.
merge into dbo.UserContext as T
using
	(	
	select @UserId, @Locale
	) as S (UserId, Locale)
on
	(T.UserId = S.UserId)
when not matched then
	insert (UserId, Locale)
	values (@UserId, @Locale)
when matched then
	update set
		Locale = @Locale;

go

