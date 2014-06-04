create function dbo.GetContextInfo()
returns table
as
/*
 * This function will return the context information for the *current* connection.
 * It requires a dbo.UserContext table to be created.
 * 
 * Example Usage
 * select * from dbo.GetContextInfo()
 *
 * Date       Author              Description
 * 2014-05-21 Philip Daniels      Initial version.
 */
return select
	--SYSP.context_info,
	--IdLength.nLen,
	UserId.UserId,
	UC.Locale,
	SYSP.SPID
from
	master.dbo.sysprocesses SYSP  with (nolock)
	cross apply (select cast(substring(SYSP.context_info, 1, 1) as tinyint) as nLen) IdLength
	cross apply (select convert(varchar(127), substring(SYSP.context_info, 2, IdLength.nLen)) as UserId) UserIdFromTable
	cross apply (select case when IdLength.nLen = 0 then suser_sname() else UserIdFromTable.UserId end as UserId) as UserId
	left join dbo.UserContext UC with (nolock) on UserId.UserId = UC.UserId
where
	SYSP.SPID = @@SPID;
