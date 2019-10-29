DELETE FROM MenuItem
DELETE FROM MenuPrincipal

/******************************************************************************************/
/******************************************************************************************/

DECLARE @MaxID SMALLINT
DECLARE @Orden SMALLINT

SET @Orden = 1

INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Bandeja Inicio', 'Bandeja Inicio', 'BandejaInicio.aspx', 1)

/******************************************************************************************/
/*
SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Bandeja Documentos', 'Bandeja Documentos', 'ActaCargaDocumentosV3.aspx', 1)
SELECT @MaxID = @@IDENTITY
*/
/******************************************************************************************/

SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Actas', 'Actas', '#', 1)
SELECT @MaxID = @@IDENTITY

INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Bandeja de Actas', 'Bandeja de Actas', 'BandejaInicioActas.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Carga de Actas y Documentos', 'Carga de Actas y Documentos', 'ActaCargaDocumentos.aspx', 1)

/******************************************************************************************/

SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Estudios', 'Configuración de Estudios', '#', 1)
SELECT @MaxID = @@IDENTITY

INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Bandeja de Estudios', 'Bandeja de Estudios', 'BandejaInicio.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Estados de Estudios', 'Estados de Estudios', 'EstadosEstudio.aspx', 1)

/******************************************************************************************/

SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Documentos', 'Configuración de Documentos', '#', 1)
SELECT @MaxID = @@IDENTITY

INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Tipos de Documentos', 'Tipos de Documentos', 'TiposDocumento.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 2, 'Estados de Documentos', 'Estados de Documentos', 'EstadosDocumentos.aspx', 1)
--INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
--VALUES (@MaxID, 3, 'Workflow de Estados', 'Workflow de Estados', 'FlujoEstadosDocumento.aspx', 1)
--INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
--VALUES (@MaxID, 4, 'Historial de Versiones', 'Historial de Versiones', '#', 1)

/******************************************************************************************/

SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Recordatorios Y Alarmas', 'Recordatorios y Alarmas', '#', 1)
SELECT @MaxID = @@IDENTITY

INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Bandeja Recordatorios', 'Bandeja de Recordatorios', 'BandejaRecordatorios.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 2, 'Tipos de Recordatorios', 'Tipos de Recordatorios', 'TiposRecordatorio.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 3, 'Listas de correo', 'Listas de correo electrónico', 'LisasEmails.aspx', 1)


/******************************************************************************************/

SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Administración de Archivos', 'Administración de Archivos', '#', 1)
SELECT @MaxID = @@IDENTITY

INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo) 
VALUES (@MaxID, 1, 'Profesionales', 'Profesionales', 'Profesionales.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 2, 'Equipos de Investigación', 'Equipos de Investigación', 'Equipos.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 3, 'Centros', 'Centros', 'Centros.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 4, 'Monodrogas', 'Monodrogas', 'Monodrogas.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 5, 'Patrocinadores', 'Patrocinadores', 'Patrocinadores.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 6, 'Patologías', 'Patologías', 'Patologias.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 7, 'Plantillas de texto', 'Plantillas de texto', 'Plantillas.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 8, 'Modelos carta de respuesta', 'Modelos carta de respuesta', 'CartaRespuestaModelo.aspx', 1)


--INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
--VALUES (@MaxID, 1, 'Especialidades', 'Especialidades', '#', 1)
--INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
--VALUES (@MaxID, 3, 'Familias Química', 'Familias Química', '#', 1)

/******************************************************************************************/
--SET @Orden = @Orden + 1
--INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
--VALUES (@Orden, 'Listados', 'Listados', '#', 1)
--SELECT @MaxID = @@IDENTITY
/******************************************************************************************/
SET @Orden = @Orden + 1
INSERT INTO MenuPrincipal (Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@Orden, 'Seguridad', 'Seguridad del Sistema', '#', 1)
SELECT @MaxID = @@IDENTITY

INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 1, 'Tipos de Usuarios', 'Tipos de Usuarios', 'TiposUsuario.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 2, 'Usuarios', 'Usuarios', 'Usuarios.aspx', 1)
INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
VALUES (@MaxID, 3, 'Cambiar Contraseña', 'Cambiar Contraseña', 'CambiarClave.aspx', 1)
--INSERT INTO MenuItem (IdMenuPrincipal, Orden, Texto, Descripcion, NavigateURL, Activo)
--VALUES (@MaxID, 4, 'Cerrar Sesión', 'Cerrar Sesión', '#', 1)

/******************************************************************************************/