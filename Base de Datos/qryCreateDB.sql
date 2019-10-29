SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Roles](
	[IdRol] [smallint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Profesionales](
	[IdProfesional] [int] IDENTITY(1,1) NOT NULL,
	[Apellido] [nvarchar](50) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[IdTipoDocumento] [tinyint] NULL,
	[NroDocumento] [nvarchar](10) NULL,
	[MatNacional] [nvarchar](50) NULL,
	[MatProvincial] [nvarchar](50) NULL,
	[TelefParticular] [nvarchar](20) NULL,
	[TelefLaboral] [nvarchar](20) NULL,
	[Celular] [nvarchar](20) NULL,
	[TelefOtro] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
 CONSTRAINT [PK_Profesionales] PRIMARY KEY CLUSTERED 
(
	[IdProfesional] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Plantillas](
	[IdPlantilla] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[Texto] [ntext] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Patrocinadores](
	[IdPatrocinador] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [nvarchar](100) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Patrocinadors] PRIMARY KEY CLUSTERED 
(
	[IdPatrocinador] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Patologias](
	[IdPatologia] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Patologias] PRIMARY KEY CLUSTERED 
(
	[IdPatologia] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TiposUsuario](
	[IdTipoUsuario] [smallint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](20) NOT NULL,
	[Vigente] [bit] NULL,
 CONSTRAINT [PK_TipoUsuario] PRIMARY KEY CLUSTERED 
(
	[IdTipoUsuario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TiposDocumento](
	[IdTipoDocumento] [smallint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](150) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_TiposDocumentos] PRIMARY KEY CLUSTERED 
(
	[IdTipoDocumento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [NivelAcceso](
	[IdNivelAcceso] [smallint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_NivelAcceso] PRIMARY KEY CLUSTERED 
(
	[IdNivelAcceso] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Monodrogas](
	[IdMonodroga] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Monodrogas] PRIMARY KEY CLUSTERED 
(
	[IdMonodroga] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MenuPrincipal](
	[IdMenuPrincipal] [smallint] IDENTITY(1,1) NOT NULL,
	[Orden] [smallint] NOT NULL,
	[Texto] [nchar](30) NOT NULL,
	[Descripcion] [nchar](150) NOT NULL,
	[NavigateURL] [nchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_MenuPrincipal] PRIMARY KEY CLUSTERED 
(
	[IdMenuPrincipal] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EstadosEstudio](
	[IdEstadoEstudio] [smallint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[Final] [bit] NOT NULL,
 CONSTRAINT [PK_EstadosEstudio] PRIMARY KEY CLUSTERED 
(
	[IdEstadoEstudio] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EstadosDocumento](
	[IdEstadoDocumento] [smallint] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_EstadosDocumentos] PRIMARY KEY CLUSTERED 
(
	[IdEstadoDocumento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Equipos](
	[IdEquipo] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Equipo] PRIMARY KEY CLUSTERED 
(
	[IdEquipo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Centros](
	[IdCentro] [int] IDENTITY(1,1) NOT NULL,
	[RazonSocial] [nvarchar](100) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Centros] PRIMARY KEY CLUSTERED 
(
	[IdCentro] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Actas](
	[IdActa] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](150) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[ComentarioInicial] [ntext] NULL,
	[ComentarioFinal] [ntext] NULL,
	[Cerrada] [bit] NOT NULL,
 CONSTRAINT [PK_Actas] PRIMARY KEY CLUSTERED 
(
	[IdActa] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ActaProfesionales](
	[IdActaProfesional] [int] IDENTITY(1,1) NOT NULL,
	[IdActa] [int] NOT NULL,
	[IdProfesional] [int] NOT NULL,
 CONSTRAINT [PK_ActaProfesionales] PRIMARY KEY CLUSTERED 
(
	[IdActaProfesional] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MenuItem](
	[IdMenuItem] [smallint] IDENTITY(1,1) NOT NULL,
	[IdMenuPrincipal] [smallint] NULL,
	[Orden] [smallint] NOT NULL,
	[Texto] [nchar](30) NOT NULL,
	[Descripcion] [nchar](150) NOT NULL,
	[NavigateURL] [nchar](100) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_MenuItem] PRIMARY KEY CLUSTERED 
(
	[IdMenuItem] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [IntegranteEquipo](
	[IdIntegranteEquipo] [int] IDENTITY(1,1) NOT NULL,
	[IdEquipo] [int] NOT NULL,
	[IdProfesional] [int] NOT NULL,
	[IdRol] [smallint] NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_IntegranteEquipo] PRIMARY KEY CLUSTERED 
(
	[IdIntegranteEquipo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Estudios](
	[IdEstudio] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](150) NOT NULL,
	[NombreCompleto] [nvarchar](500) NULL,
	[Vigente] [bit] NOT NULL,
	[Codigo] [nvarchar](50) NOT NULL,
	[Poblacion] [nvarchar](150) NULL,
	[IdPatologia] [int] NULL,
	[IdEquipo] [int] NULL,
 CONSTRAINT [PK_Estudio] PRIMARY KEY CLUSTERED 
(
	[IdEstudio] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TipoDocumentoFlujos](
	[IdFlujo] [smallint] IDENTITY(1,1) NOT NULL,
	[IdTipoDocumento] [smallint] NOT NULL,
	[Descripcion] [nvarchar](150) NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_TipoDocumentoFlujos] PRIMARY KEY CLUSTERED 
(
	[IdFlujo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Apellido] [nvarchar](50) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[LoginUsuario] [nvarchar](20) NULL,
	[LoginClave] [nvarchar](20) NULL,
	[IdTipoUsuario] [smallint] NOT NULL,
	[Vigente] [bit] NOT NULL,
	[TimesStamp] [timestamp] NULL,
	[Mail] [nvarchar](100) NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Documentos](
	[IdDocumento] [int] IDENTITY(1,1) NOT NULL,
	[IdEstudio] [int] NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[IdTipoDocumento] [smallint] NOT NULL,
	[Limitante] [bit] NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_Documentos] PRIMARY KEY CLUSTERED 
(
	[IdDocumento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [UsuarioAcceso](
	[IdUsuarioAcceso] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdMenu] [smallint] NOT NULL,
	[IdNivelAcceso] [smallint] NOT NULL,
 CONSTRAINT [PK_UsuarioAcceso] PRIMARY KEY CLUSTERED 
(
	[IdUsuarioAcceso] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TipoDocumentoFlujoEstados](
	[IdFlujoEstado] [int] IDENTITY(1,1) NOT NULL,
	[IdFlujo] [smallint] NOT NULL,
	[IdEstado] [smallint] NOT NULL,
	[IdEstadoPadre] [smallint] NULL,
	[Final] [bit] NOT NULL,
 CONSTRAINT [PK_TipoDocumentoFlujoEstados_1] PRIMARY KEY CLUSTERED 
(
	[IdFlujoEstado] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Notas](
	[IdNota] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](250) NOT NULL,
	[Vigente] [bit] NOT NULL,
	[IdEstudio] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Numero] [int] NOT NULL,
	[IdAutor] [int] NULL,
	[NombreArchivo] [nvarchar](150) NULL,
	[PathArchivo] [nvarchar](250) NULL,
	[RequiereRespuesta] [bit] NULL,
 CONSTRAINT [PK_Notas] PRIMARY KEY CLUSTERED 
(
	[IdNota] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EstudioPatrocinadores](
	[IdEstudioPatrocinador] [int] IDENTITY(1,1) NOT NULL,
	[IdEstudio] [int] NOT NULL,
	[IdPatrocinador] [int] NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_EstudioPatrocinador] PRIMARY KEY CLUSTERED 
(
	[IdEstudioPatrocinador] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EstudioParticipantes](
	[IdEstudioParticipante] [int] IDENTITY(1,1) NOT NULL,
	[IdEstudio] [int] NOT NULL,
	[IdProfesional] [int] NOT NULL,
	[IdRol] [smallint] NOT NULL,
	[Desde] [datetime] NULL,
	[Hasta] [datetime] NULL,
 CONSTRAINT [PK_EstudioParticipantes] PRIMARY KEY CLUSTERED 
(
	[IdEstudioParticipante] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EstudioMonodrogas](
	[IdEstudioMonodroga] [int] IDENTITY(1,1) NOT NULL,
	[IdEstudio] [int] NOT NULL,
	[IdMonodroga] [int] NOT NULL,
	[Vigente] [bit] NOT NULL,
 CONSTRAINT [PK_EstudioMonodrogas] PRIMARY KEY CLUSTERED 
(
	[IdEstudioMonodroga] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [EstudioCentros](
	[IdEstudioCentro] [int] IDENTITY(1,1) NOT NULL,
	[IdEstudio] [int] NOT NULL,
	[IdCentro] [int] NOT NULL,
	[Vigente] [bit] NOT NULL,
	[Desde] [datetime] NULL,
	[Hasta] [datetime] NULL,
 CONSTRAINT [PK_EstudioCentros] PRIMARY KEY CLUSTERED 
(
	[IdEstudioCentro] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DocumentoVersiones](
	[IdDocumentoVersion] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumento] [int] NOT NULL,
	[Fecha] [smalldatetime] NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[AprobadoANMAT] [bit] NULL,
	[FechaAprobadoANMAT] [smalldatetime] NULL,
	[Archivo] [nvarchar](100) NULL,
 CONSTRAINT [PK_DocumentoVersiones] PRIMARY KEY CLUSTERED 
(
	[IdDocumentoVersion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [NotaDocumentos](
	[IdNotaDocumento] [int] IDENTITY(1,1) NOT NULL,
	[IdNota] [int] NOT NULL,
	[IdDocumentoVersion] [int] NOT NULL,
 CONSTRAINT [PK_NotaDocumento] PRIMARY KEY CLUSTERED 
(
	[IdNotaDocumento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DocumentoVersionRecordatorios](
	[IdRecordatorio] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentoVersion] [int] NOT NULL,
	[Fecha] [smalldatetime] NOT NULL,
	[IdProfesionalAutor] [int] NOT NULL,
	[Observaciones] [nvarchar](200) NULL,
	[Pendiente] [bit] NULL,
 CONSTRAINT [PK_DocumentoVersionRecordatorios] PRIMARY KEY CLUSTERED 
(
	[IdRecordatorio] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DocumentoVersionEstados](
	[IdVersionEstado] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [smalldatetime] NOT NULL,
	[IdDocumentoVersion] [int] NOT NULL,
	[IdEstadoDocumento] [smallint] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdProfesionalAutor] [int] NULL,
	[IdProfesionalPresenta] [int] NULL,
	[IdProfesionalResponsable] [int] NULL,
	[Observaciones] [nvarchar](250) NULL,
	[EstadoFinal] [bit] NULL,
 CONSTRAINT [PK_DocumentoVersionEstados] PRIMARY KEY CLUSTERED 
(
	[IdVersionEstado] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [DocumentoVersionComentarios](
	[IdComentario] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentoVersion] [int] NOT NULL,
	[Fecha] [smalldatetime] NOT NULL,
	[IdProfesionalAutor] [int] NOT NULL,
	[Observaciones] [nvarchar](2000) NULL,
 CONSTRAINT [PK_DocumentoVersionComentarios] PRIMARY KEY CLUSTERED 
(
	[IdComentario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ActaDocumentos](
	[IdActaDocumento] [int] IDENTITY(1,1) NOT NULL,
	[IdActa] [int] NOT NULL,
	[IdDocumentoVersion] [int] NOT NULL,
	[Comentario] [ntext] NULL,
 CONSTRAINT [PK_ActaDocumento] PRIMARY KEY CLUSTERED 
(
	[IdActaDocumento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [ActaDocumentos]  WITH CHECK ADD  CONSTRAINT [FK_ActaDocumento_Actas] FOREIGN KEY([IdActa])
REFERENCES [Actas] ([IdActa])
GO
ALTER TABLE [ActaDocumentos] CHECK CONSTRAINT [FK_ActaDocumento_Actas]
GO
ALTER TABLE [ActaDocumentos]  WITH CHECK ADD  CONSTRAINT [FK_ActaDocumentos_DocumentoVersiones] FOREIGN KEY([IdDocumentoVersion])
REFERENCES [DocumentoVersiones] ([IdDocumentoVersion])
GO
ALTER TABLE [ActaDocumentos] CHECK CONSTRAINT [FK_ActaDocumentos_DocumentoVersiones]
GO
ALTER TABLE [ActaProfesionales]  WITH CHECK ADD  CONSTRAINT [FK_ActaProfesionales_Actas] FOREIGN KEY([IdActa])
REFERENCES [Actas] ([IdActa])
GO
ALTER TABLE [ActaProfesionales] CHECK CONSTRAINT [FK_ActaProfesionales_Actas]
GO
ALTER TABLE [ActaProfesionales]  WITH CHECK ADD  CONSTRAINT [FK_ActaProfesionales_Profesionales] FOREIGN KEY([IdProfesional])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [ActaProfesionales] CHECK CONSTRAINT [FK_ActaProfesionales_Profesionales]
GO
ALTER TABLE [Documentos]  WITH CHECK ADD  CONSTRAINT [FK_Documentos_Estudio] FOREIGN KEY([IdEstudio])
REFERENCES [Estudios] ([IdEstudio])
GO
ALTER TABLE [Documentos] CHECK CONSTRAINT [FK_Documentos_Estudio]
GO
ALTER TABLE [Documentos]  WITH CHECK ADD  CONSTRAINT [FK_Documentos_TiposDocumentos] FOREIGN KEY([IdTipoDocumento])
REFERENCES [TiposDocumento] ([IdTipoDocumento])
GO
ALTER TABLE [Documentos] CHECK CONSTRAINT [FK_Documentos_TiposDocumentos]
GO
ALTER TABLE [DocumentoVersionComentarios]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionComentarios_DocumentoVersiones] FOREIGN KEY([IdDocumentoVersion])
REFERENCES [DocumentoVersiones] ([IdDocumentoVersion])
GO
ALTER TABLE [DocumentoVersionComentarios] CHECK CONSTRAINT [FK_DocumentoVersionComentarios_DocumentoVersiones]
GO
ALTER TABLE [DocumentoVersionComentarios]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionComentarios_Profesionales] FOREIGN KEY([IdProfesionalAutor])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [DocumentoVersionComentarios] CHECK CONSTRAINT [FK_DocumentoVersionComentarios_Profesionales]
GO
ALTER TABLE [DocumentoVersiones]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersiones_Documentos] FOREIGN KEY([IdDocumento])
REFERENCES [Documentos] ([IdDocumento])
GO
ALTER TABLE [DocumentoVersiones] CHECK CONSTRAINT [FK_DocumentoVersiones_Documentos]
GO
ALTER TABLE [DocumentoVersionEstados]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionEstados_DocumentoVersiones] FOREIGN KEY([IdDocumentoVersion])
REFERENCES [DocumentoVersiones] ([IdDocumentoVersion])
GO
ALTER TABLE [DocumentoVersionEstados] CHECK CONSTRAINT [FK_DocumentoVersionEstados_DocumentoVersiones]
GO
ALTER TABLE [DocumentoVersionEstados]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionEstados_EstadosDocumentos] FOREIGN KEY([IdEstadoDocumento])
REFERENCES [EstadosDocumento] ([IdEstadoDocumento])
GO
ALTER TABLE [DocumentoVersionEstados] CHECK CONSTRAINT [FK_DocumentoVersionEstados_EstadosDocumentos]
GO
ALTER TABLE [DocumentoVersionEstados]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionEstados_Profesionales] FOREIGN KEY([IdProfesionalAutor])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [DocumentoVersionEstados] CHECK CONSTRAINT [FK_DocumentoVersionEstados_Profesionales]
GO
ALTER TABLE [DocumentoVersionEstados]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionEstados_Profesionales1] FOREIGN KEY([IdProfesionalPresenta])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [DocumentoVersionEstados] CHECK CONSTRAINT [FK_DocumentoVersionEstados_Profesionales1]
GO
ALTER TABLE [DocumentoVersionEstados]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionEstados_Profesionales2] FOREIGN KEY([IdProfesionalResponsable])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [DocumentoVersionEstados] CHECK CONSTRAINT [FK_DocumentoVersionEstados_Profesionales2]
GO
ALTER TABLE [DocumentoVersionEstados]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionEstados_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [Usuarios] ([IdUsuario])
GO
ALTER TABLE [DocumentoVersionEstados] CHECK CONSTRAINT [FK_DocumentoVersionEstados_Usuarios]
GO
ALTER TABLE [DocumentoVersionRecordatorios]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionRecordatorios_DocumentoVersiones] FOREIGN KEY([IdDocumentoVersion])
REFERENCES [DocumentoVersiones] ([IdDocumentoVersion])
GO
ALTER TABLE [DocumentoVersionRecordatorios] CHECK CONSTRAINT [FK_DocumentoVersionRecordatorios_DocumentoVersiones]
GO
ALTER TABLE [DocumentoVersionRecordatorios]  WITH CHECK ADD  CONSTRAINT [FK_DocumentoVersionRecordatorios_Profesionales] FOREIGN KEY([IdProfesionalAutor])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [DocumentoVersionRecordatorios] CHECK CONSTRAINT [FK_DocumentoVersionRecordatorios_Profesionales]
GO
ALTER TABLE [EstudioCentros]  WITH CHECK ADD  CONSTRAINT [FK_EstudioCentros_Centros] FOREIGN KEY([IdCentro])
REFERENCES [Centros] ([IdCentro])
GO
ALTER TABLE [EstudioCentros] CHECK CONSTRAINT [FK_EstudioCentros_Centros]
GO
ALTER TABLE [EstudioCentros]  WITH CHECK ADD  CONSTRAINT [FK_EstudioCentros_Estudio] FOREIGN KEY([IdEstudio])
REFERENCES [Estudios] ([IdEstudio])
GO
ALTER TABLE [EstudioCentros] CHECK CONSTRAINT [FK_EstudioCentros_Estudio]
GO
ALTER TABLE [EstudioMonodrogas]  WITH CHECK ADD  CONSTRAINT [FK_EstudioMonodrogas_Estudio] FOREIGN KEY([IdEstudio])
REFERENCES [Estudios] ([IdEstudio])
GO
ALTER TABLE [EstudioMonodrogas] CHECK CONSTRAINT [FK_EstudioMonodrogas_Estudio]
GO
ALTER TABLE [EstudioMonodrogas]  WITH CHECK ADD  CONSTRAINT [FK_EstudioMonodrogas_Monodrogas] FOREIGN KEY([IdMonodroga])
REFERENCES [Monodrogas] ([IdMonodroga])
GO
ALTER TABLE [EstudioMonodrogas] CHECK CONSTRAINT [FK_EstudioMonodrogas_Monodrogas]
GO
ALTER TABLE [EstudioParticipantes]  WITH CHECK ADD  CONSTRAINT [FK_EstudioParticipantes_Estudios] FOREIGN KEY([IdEstudio])
REFERENCES [Estudios] ([IdEstudio])
GO
ALTER TABLE [EstudioParticipantes] CHECK CONSTRAINT [FK_EstudioParticipantes_Estudios]
GO
ALTER TABLE [EstudioParticipantes]  WITH CHECK ADD  CONSTRAINT [FK_EstudioParticipantes_Profesionales] FOREIGN KEY([IdProfesional])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [EstudioParticipantes] CHECK CONSTRAINT [FK_EstudioParticipantes_Profesionales]
GO
ALTER TABLE [EstudioParticipantes]  WITH CHECK ADD  CONSTRAINT [FK_EstudioParticipantes_Roles] FOREIGN KEY([IdRol])
REFERENCES [Roles] ([IdRol])
GO
ALTER TABLE [EstudioParticipantes] CHECK CONSTRAINT [FK_EstudioParticipantes_Roles]
GO
ALTER TABLE [EstudioPatrocinadores]  WITH CHECK ADD  CONSTRAINT [FK_EstudioPatrocinador_Estudio] FOREIGN KEY([IdEstudio])
REFERENCES [Estudios] ([IdEstudio])
GO
ALTER TABLE [EstudioPatrocinadores] CHECK CONSTRAINT [FK_EstudioPatrocinador_Estudio]
GO
ALTER TABLE [EstudioPatrocinadores]  WITH CHECK ADD  CONSTRAINT [FK_EstudioPatrocinador_Patrocinadores] FOREIGN KEY([IdPatrocinador])
REFERENCES [Patrocinadores] ([IdPatrocinador])
GO
ALTER TABLE [EstudioPatrocinadores] CHECK CONSTRAINT [FK_EstudioPatrocinador_Patrocinadores]
GO
ALTER TABLE [Estudios]  WITH CHECK ADD  CONSTRAINT [FK_Estudio_Patologias] FOREIGN KEY([IdPatologia])
REFERENCES [Patologias] ([IdPatologia])
GO
ALTER TABLE [Estudios] CHECK CONSTRAINT [FK_Estudio_Patologias]
GO
ALTER TABLE [IntegranteEquipo]  WITH CHECK ADD  CONSTRAINT [FK_IntegranteEquipo_Equipo] FOREIGN KEY([IdEquipo])
REFERENCES [Equipos] ([IdEquipo])
GO
ALTER TABLE [IntegranteEquipo] CHECK CONSTRAINT [FK_IntegranteEquipo_Equipo]
GO
ALTER TABLE [IntegranteEquipo]  WITH CHECK ADD  CONSTRAINT [FK_IntegranteEquipo_Profesionales] FOREIGN KEY([IdProfesional])
REFERENCES [Profesionales] ([IdProfesional])
GO
ALTER TABLE [IntegranteEquipo] CHECK CONSTRAINT [FK_IntegranteEquipo_Profesionales]
GO
ALTER TABLE [IntegranteEquipo]  WITH CHECK ADD  CONSTRAINT [FK_IntegranteEquipo_Roles] FOREIGN KEY([IdRol])
REFERENCES [Roles] ([IdRol])
GO
ALTER TABLE [IntegranteEquipo] CHECK CONSTRAINT [FK_IntegranteEquipo_Roles]
GO
ALTER TABLE [MenuItem]  WITH CHECK ADD  CONSTRAINT [FK_MenuItem_MenuPrincipal] FOREIGN KEY([IdMenuPrincipal])
REFERENCES [MenuPrincipal] ([IdMenuPrincipal])
GO
ALTER TABLE [MenuItem] CHECK CONSTRAINT [FK_MenuItem_MenuPrincipal]
GO
ALTER TABLE [NotaDocumentos]  WITH CHECK ADD  CONSTRAINT [FK_NotaDocumento_Notas] FOREIGN KEY([IdNota])
REFERENCES [Notas] ([IdNota])
GO
ALTER TABLE [NotaDocumentos] CHECK CONSTRAINT [FK_NotaDocumento_Notas]
GO
ALTER TABLE [NotaDocumentos]  WITH CHECK ADD  CONSTRAINT [FK_NotaDocumentos_DocumentoVersiones] FOREIGN KEY([IdDocumentoVersion])
REFERENCES [DocumentoVersiones] ([IdDocumentoVersion])
GO
ALTER TABLE [NotaDocumentos] CHECK CONSTRAINT [FK_NotaDocumentos_DocumentoVersiones]
GO
ALTER TABLE [Notas]  WITH CHECK ADD  CONSTRAINT [FK_Notas_Estudios] FOREIGN KEY([IdEstudio])
REFERENCES [Estudios] ([IdEstudio])
GO
ALTER TABLE [Notas] CHECK CONSTRAINT [FK_Notas_Estudios]
GO
ALTER TABLE [TipoDocumentoFlujoEstados]  WITH CHECK ADD  CONSTRAINT [FK_TipoDocumentoFlujoEstados_EstadosDocumento] FOREIGN KEY([IdEstado])
REFERENCES [EstadosDocumento] ([IdEstadoDocumento])
GO
ALTER TABLE [TipoDocumentoFlujoEstados] CHECK CONSTRAINT [FK_TipoDocumentoFlujoEstados_EstadosDocumento]
GO
ALTER TABLE [TipoDocumentoFlujoEstados]  WITH CHECK ADD  CONSTRAINT [FK_TipoDocumentoFlujoEstados_EstadosDocumento1] FOREIGN KEY([IdEstadoPadre])
REFERENCES [EstadosDocumento] ([IdEstadoDocumento])
GO
ALTER TABLE [TipoDocumentoFlujoEstados] CHECK CONSTRAINT [FK_TipoDocumentoFlujoEstados_EstadosDocumento1]
GO
ALTER TABLE [TipoDocumentoFlujoEstados]  WITH CHECK ADD  CONSTRAINT [FK_TipoDocumentoFlujoEstados_TipoDocumentoFlujos] FOREIGN KEY([IdFlujo])
REFERENCES [TipoDocumentoFlujos] ([IdFlujo])
GO
ALTER TABLE [TipoDocumentoFlujoEstados] CHECK CONSTRAINT [FK_TipoDocumentoFlujoEstados_TipoDocumentoFlujos]
GO
ALTER TABLE [TipoDocumentoFlujos]  WITH CHECK ADD  CONSTRAINT [FK_TipoDocumentoFlujos_TiposDocumento1] FOREIGN KEY([IdTipoDocumento])
REFERENCES [TiposDocumento] ([IdTipoDocumento])
GO
ALTER TABLE [TipoDocumentoFlujos] CHECK CONSTRAINT [FK_TipoDocumentoFlujos_TiposDocumento1]
GO
ALTER TABLE [UsuarioAcceso]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioAcceso_NivelAcceso] FOREIGN KEY([IdNivelAcceso])
REFERENCES [NivelAcceso] ([IdNivelAcceso])
GO
ALTER TABLE [UsuarioAcceso] CHECK CONSTRAINT [FK_UsuarioAcceso_NivelAcceso]
GO
ALTER TABLE [UsuarioAcceso]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioAcceso_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [Usuarios] ([IdUsuario])
GO
ALTER TABLE [UsuarioAcceso] CHECK CONSTRAINT [FK_UsuarioAcceso_Usuarios]
GO
ALTER TABLE [Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_RolesUsuario] FOREIGN KEY([IdTipoUsuario])
REFERENCES [TiposUsuario] ([IdTipoUsuario])
GO
ALTER TABLE [Usuarios] CHECK CONSTRAINT [FK_Usuarios_RolesUsuario]
GO
