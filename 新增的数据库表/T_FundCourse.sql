USE [DB_QCH]
GO

/****** Object:  Table [dbo].[T_FundCourse]    Script Date: 2016/4/16 16:32:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[T_FundCourse](
	[Guid] [varchar](49) NOT NULL,
	[T_LecturerGuid] [varchar](49) NULL,
	[T_FundCourse_Pic] [varchar](150) NULL,
	[T_FundCourse_Title] [varchar](100) NULL,
	[T_FundCourse_Money] [decimal](18, 2) NULL,
	[T_PayMoney_Online] [decimal](18, 2) NULL,
	[T_PayMoney_Offline] [decimal](18, 2) NULL,
	[T_FundCourse_Info] [nvarchar](max) NULL,
	[T_FundCourse_OneWord] [nvarchar](100) NULL,
	[T_FundCourse_Tip] [varchar](500) NULL,
	[T_FundCourse_Count] [int] NULL,
	[T_FundCourse_Sort] [int] NULL,
	[T_FundCourse_Recommend] [int] NULL,
	[T_FundCourse_Good] [int] NULL,
	[T_FundCourse_Bad] [int] NULL,
	[T_FundCourse_Remark] [nvarchar](150) NULL,
	[T_DelState] [int] NULL,
 CONSTRAINT [PK_T_Fund_Course] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

