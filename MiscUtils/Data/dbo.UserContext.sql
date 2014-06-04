-- Use this script to create the dbo.UserContext table.
CREATE TABLE [dbo].[UserContext]
    (
	UserId varchar (127) NOT NULL,
	Locale varchar (5) NULL,
    CONSTRAINT [PK_dbo_UserContext] PRIMARY KEY CLUSTERED
    );
