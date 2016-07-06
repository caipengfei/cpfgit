USE [DB_QCH]
GO

/****** Object:  Table [dbo].[t_Goods_Convert_List]    Script Date: 2016/7/5 20:31:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[t_Goods_Convert_List](
	[Guid] [varchar](49) NOT NULL,
	[t_User_Guid] [varchar](49) NULL,
	[t_Goods_Guid] [varchar](49) NULL,
	[t_Cnee_Guid] [varchar](49) NULL,
	[t_Convert_OrderNo] [varchar](50) NULL,
	[t_Convert_CreateDate] [datetime] NULL,
	[t_Logistics_Company] [varchar](50) NULL,
	[t_Logistics_WaybillNo] [varchar](50) NULL,
	[t_Logistics_Status] [int] NULL,
	[t_DelState] [int] NULL,
 CONSTRAINT [PK_t_Goods_Convert_List] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

