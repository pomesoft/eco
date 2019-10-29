/*
   sábado, 10 de agosto de 201910:30:29 a.m.
   User: sa
   Server: USUARIO-PC\SQLEXP2016
   Database: pmlSysGEIC_PROD
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Notas
	DROP CONSTRAINT FK_Notas_Actas
GO
ALTER TABLE dbo.Actas SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Notas SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



BEGIN TRANSACTION
GO
ALTER TABLE dbo.TiposDocumento ADD
	NecesarioAprobacionEstudio bit NULL
GO
ALTER TABLE dbo.TiposDocumento SET (LOCK_ESCALATION = TABLE)
GO
COMMIT





/****** Object:  Table [dbo].[ItemChart]    Script Date: 01/10/2019 05:16:55 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ItemChart](
	[Id] [int] NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
	[Descripcion2] [nvarchar](200) NULL,
	[Valor] [int] NULL,
	[Valor2] [int] NULL
) ON [PRIMARY]
GO


