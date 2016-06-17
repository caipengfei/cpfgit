USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_User_SignIn]    Script Date: 2016/6/6 9:54:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_User_SignIn](
	[Guid] [varchar](49) NOT NULL,
	[T_User_Guid] [varchar](49) NULL,
	[T_SignIn_Date] [datetime] NULL,
	[T_Integral] [int] NULL,
	[T_SignIn_Days] [int] NULL,
	[T_Extra_Integral] [int] NULL,
	[T_Remark] [varchar](50) NULL,
	[T_DelState] [int] NULL,
 CONSTRAINT [PK_T_User_SignIn] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

