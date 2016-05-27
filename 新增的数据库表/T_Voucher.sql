USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_Voucher]    Script Date: 2016/5/20 16:06:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_Voucher](
	[Guid] [varchar](49) NOT NULL,
	[T_Voucher_Price] [decimal](18, 2) NULL,
	[T_Voucher_FromIn] [varchar](49) NULL,
	[T_CreateDate] [datetime] NULL,
	[T_sDate] [datetime] NULL,
	[T_eDate] [datetime] NULL,
	[T_Voucher_Type] [int] NULL,
	[T_Voucher_Scope] [varchar](50) NULL,
	[T_Remark] [varchar](50) NULL,
	[T_DelState] [int] NULL,
 CONSTRAINT [PK_T_Voucher] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[T_Voucher] ADD  CONSTRAINT [DF_T_Voucher_T_CreateDate]  DEFAULT (getdate()) FOR [T_CreateDate]
GO

ALTER TABLE [dbo].[T_Voucher] ADD  CONSTRAINT [DF_T_Voucher_T_sDate]  DEFAULT (getdate()) FOR [T_sDate]
GO

ALTER TABLE [dbo].[T_Voucher] ADD  CONSTRAINT [DF_T_Voucher_T_Voucher_Type]  DEFAULT ((0)) FOR [T_Voucher_Type]
GO

ALTER TABLE [dbo].[T_Voucher] ADD  CONSTRAINT [DF_T_Voucher_T_DelState]  DEFAULT ((0)) FOR [T_DelState]
GO

