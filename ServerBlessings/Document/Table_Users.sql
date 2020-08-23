USE [Bless]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 2020/8/23 16:23:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nickname] [nvarchar](50) NULL,
	[OpenID] [nvarchar](500) NULL,
	[Sex] [real] NULL,
	[Province] [nvarchar](500) NULL,
	[City] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[Headimgurl] [nvarchar](500) NULL,
	[Privilege] [nvarchar](500) NULL,
	[UnionID] [nvarchar](500) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Sex]  DEFAULT ((0)) FOR [Sex]
GO

