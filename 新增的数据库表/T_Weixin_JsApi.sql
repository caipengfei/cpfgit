USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_Weixin_JsApi]    Script Date: 2016/4/9 9:40:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_Weixin_JsApi](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [varchar](15) NULL,
	[Noncestr] [varchar](50) NULL,
	[Signature] [varchar](200) NULL,
	[Jsapi_ticket] [varchar](200) NULL,
	[AppId] [varchar](50) NULL,
	[access_token] [varchar](200) NULL,
	[PageName] [nvarchar](150) NULL,
 CONSTRAINT [PK_t_weixin_jsapi] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

