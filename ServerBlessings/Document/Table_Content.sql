USE [Bless]
GO

/****** Object:  Table [dbo].[Content]    Script Date: 2020/7/13 7:39:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Content](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Message] [nvarchar](500) NULL,
	[CreateTime] [datetime] NULL,
	[IsShowed] [bit] NULL,
	[ObjectIndex] [int] NULL,
	[ObjectType] [nvarchar](50) NULL,
 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_IsShowed]  DEFAULT ((0)) FOR [IsShowed]
GO

