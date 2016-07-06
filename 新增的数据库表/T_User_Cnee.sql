USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_User_Cnee]    Script Date: 2016/7/5 20:31:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_User_Cnee](
	[Guid] [varchar](49) NOT NULL,
	[t_User_Guid] [varchar](49) NULL,
	[t_Cnee_Name] [varchar](20) NULL,
	[t_Cnee_Phone] [varchar](20) NULL,
	[t_Cnee_Addr] [varchar](200) NULL,
	[t_IsDefault] [int] NULL,
	[t_DelState] [int] NULL,
 CONSTRAINT [PK_t_user_cnee] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[T_User_Cnee] ADD  CONSTRAINT [DF_t_user_cnee_t_IsDefault]  DEFAULT ((0)) FOR [t_IsDefault]
GO

ALTER TABLE [dbo].[T_User_Cnee] ADD  CONSTRAINT [DF_t_user_cnee_t_DelState]  DEFAULT ((0)) FOR [t_DelState]
GO

