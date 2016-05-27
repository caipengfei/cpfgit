USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_User_Voucher]    Script Date: 2016/5/20 16:05:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_User_Voucher](
	[Guid] [varchar](49) NOT NULL,
	[T_User_Guid] [varchar](49) NULL,
	[T_Voucher_Pwd] [varchar](50) NULL,
	[T_Voucher_State] [int] NULL,
	[T_GetDate] [datetime] NULL,
	[T_DelState] [int] NULL,
 CONSTRAINT [PK_T_User_Voucher] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

