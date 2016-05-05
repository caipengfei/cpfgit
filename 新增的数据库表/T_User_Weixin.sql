USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_User_Weixin]    Script Date: 2016/4/9 9:40:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_User_Weixin](
	[Guid] [varchar](50) NOT NULL,
	[UserGuid] [varchar](50) NULL,
	[OpenId] [varchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[QrCode] [nvarchar](500) NULL,
	[WxTgUserGuid] [varchar](50) NULL,
	[Nonce] [nvarchar](100) NULL,
	[MediaId] [nvarchar](500) NULL,
	[MediaDate] [datetime] NULL,
 CONSTRAINT [PK_t_user_weixin] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

