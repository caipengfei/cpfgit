USE [DB_QCH]
GO

/****** Object:  Table [dbo].[t_Goods_Convert]    Script Date: 2016/7/5 20:31:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[t_Goods_Convert](
	[Guid] [varchar](49) NOT NULL,
	[t_Goods_Code] [varchar](50) NULL,
	[t_Goods_Name] [nvarchar](150) NULL,
	[t_Need_Integral] [int] NULL,
	[t_Goods_Pic] [nvarchar](150) NULL,
	[t_Goods_Unit] [nvarchar](50) NULL,
	[t_Goods_Size] [nvarchar](150) NULL,
	[t_Goods_Stock] [int] NULL,
	[t_Goods_Desc] [nvarchar](350) NULL,
	[t_Goods_Info] [nvarchar](max) NULL,
	[t_Goods_Freight] [decimal](18, 2) NULL,
	[t_CreateDate] [datetime] NULL,
	[t_Goods_Audit] [int] NULL,
	[t_Goods_IsSale] [int] NULL,
	[t_DelState] [int] NULL,
 CONSTRAINT [PK_t_Goods_Convert] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

