USE [master]
GO

/****** Object:  Database [meeting_rooms]    Script Date: 06/11/2022 5.21.39 ******/
DROP DATABASE [meeting_rooms]
GO

/****** Object:  Database [meeting_rooms]    Script Date: 06/11/2022 5.21.39 ******/
CREATE DATABASE [meeting_rooms]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'meeting_room', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\meeting_room.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'meeting_room_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\meeting_room_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [meeting_rooms].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [meeting_rooms] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [meeting_rooms] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [meeting_rooms] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [meeting_rooms] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [meeting_rooms] SET ARITHABORT OFF 
GO

ALTER DATABASE [meeting_rooms] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [meeting_rooms] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [meeting_rooms] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [meeting_rooms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [meeting_rooms] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [meeting_rooms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [meeting_rooms] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [meeting_rooms] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [meeting_rooms] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [meeting_rooms] SET  DISABLE_BROKER 
GO

ALTER DATABASE [meeting_rooms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [meeting_rooms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [meeting_rooms] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [meeting_rooms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [meeting_rooms] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [meeting_rooms] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [meeting_rooms] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [meeting_rooms] SET RECOVERY FULL 
GO

ALTER DATABASE [meeting_rooms] SET  MULTI_USER 
GO

ALTER DATABASE [meeting_rooms] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [meeting_rooms] SET DB_CHAINING OFF 
GO

ALTER DATABASE [meeting_rooms] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [meeting_rooms] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [meeting_rooms] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [meeting_rooms] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO

ALTER DATABASE [meeting_rooms] SET QUERY_STORE = OFF
GO

ALTER DATABASE [meeting_rooms] SET  READ_WRITE 
GO


USE [meeting_rooms]
GO

/****** Object:  Table [dbo].[meeting_room]    Script Date: 06/11/2022 5.22.27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[meeting_room]') AND type in (N'U'))
DROP TABLE [dbo].[meeting_room]
GO

/****** Object:  Table [dbo].[meeting_room]    Script Date: 06/11/2022 5.22.27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[meeting_room](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
	[location] [varchar](255) NULL,
	[seats] [int] NULL,
 CONSTRAINT [PK_meeting_room] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [meeting_rooms]
GO

ALTER TABLE [dbo].[reservation] DROP CONSTRAINT [FK_reservation]
GO

/****** Object:  Table [dbo].[reservation]    Script Date: 06/11/2022 5.23.52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[reservation]') AND type in (N'U'))
DROP TABLE [dbo].[reservation]
GO

/****** Object:  Table [dbo].[reservation]    Script Date: 06/11/2022 5.23.52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[reservation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[meeting_room_id] [int] NULL,
	[host] [varchar](255) NULL,
	[time_from] [datetime] NULL,
	[time_to] [datetime] NULL,
 CONSTRAINT [PK_reservation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[reservation]  WITH CHECK ADD  CONSTRAINT [FK_reservation] FOREIGN KEY([meeting_room_id])
REFERENCES [dbo].[meeting_room] ([id])
GO

ALTER TABLE [dbo].[reservation] CHECK CONSTRAINT [FK_reservation]
GO


