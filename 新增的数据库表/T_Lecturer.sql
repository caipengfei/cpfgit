USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_Lecturer]    Script Date: 2016/4/16 14:59:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_Lecturer](
	[Guid] [varchar](49) NULL,
	[T_Lecturer_Name] [varchar](20) NULL,
	[T_Lecturer_Pic] [varchar](150) NULL,
	[T_Lecturer_Position] [varchar](20) NULL,
	[T_Lecturer_Intor] [varchar](201) NULL,
	[T_Lecturer_GoodArea] [varchar](150) NULL,
	[T_Lecturer_Subject] [nvarchar](100) NULL,
	[T_Lecturer_CreateDate] [datetime] NULL,
	[T_Lecturer_Count] [int] NULL,
	[T_Lecturer_Remark] [nvarchar](100) NULL,
	[T_Lecturer_Sort] [int] NULL,
	[T_Lecturer_Recommend] [int] NULL,
	[T_Lecturer_Good] [int] NULL,
	[T_Lecturer_Bad] [int] NULL,
	[T_DelState] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

