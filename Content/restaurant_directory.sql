USE [restaurant_directory]
GO
/****** Object:  Table [dbo].[cuisines]    Script Date: 12/8/2016 6:31:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cuisines](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[restaurants]    Script Date: 12/8/2016 6:31:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[restaurants](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[cuisine_id] [int] NULL,
	[address] [varchar](255) NULL,
	[website] [varchar](255) NULL,
	[phone] [varchar](20) NULL
) ON [PRIMARY]

GO
