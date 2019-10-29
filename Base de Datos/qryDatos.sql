/*
update parametros set valor = 'CEFC Fundacion CIDEA' where idparametro = 1
update parametros set valor = '04-2013' where idparametro = 2
select * from parametros

DBCC CHECKIDENT (tiposrecordatorio, RESEED,0)
*/

/*Querys por Estudio*/
declare @idestudio int
set @idestudio = 146
select * from estudios where idestudio = @idestudio
--select * from estudiocentros where idestudio = @idestudio
--select * from estudiomonodrogas where idestudio = @idestudio
--select * from estudioparticipantes where idestudio = @idestudio
--select * from estudiopatrocinadores where idestudio = @idestudio

--select * from notas where idestudio = @idestudio
--select * from notadocumentos where idnota in (select idnota from notas where idestudio = @idestudio)

select * from documentos where idestudio = @idestudio
select * from documentoversiones where iddocumento in (select iddocumento from documentos where idestudio = @idestudio)
select * from documentoversionparticipantes where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento in (select iddocumento from documentos where idestudio = @idestudio))
select * from documentoversionestados where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento in (select iddocumento from documentos where idestudio = @idestudio))
select * from documentoversioncomentarios where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento in (select iddocumento from documentos where idestudio = @idestudio))
select * from documentoversionrecordatorios where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento in (select iddocumento from documentos where idestudio = @idestudio))
GO


/*Querys por Acta*/
declare @idacta int
declare @idestudio int
set @idacta = 106
set @idestudio = 105
select * from Actas where IdActa = @idacta 
--select Comentario from ActaDocumentos where IdActa = @idacta 
select * from Notas where IdActa = @idacta
--select * from ActaProfesionales where IdActa = @idacta

select * from ActaEstudios where IdActa = @idacta and IdEstudio = @idestudio 

select	d.idestudio, e.codigo, e.descripcion, 
		ltrim(str(ad.ordenestudio)) + ltrim(str(td.idtipodocumentogrupo)) + ltrim(str(ad.ordendocumento)) as orden,
		td.idtipodocumentogrupo, tdg.descripcion, 
		ad.ordenestudio, ad.ordendocumento, d.descripcion as documento, dv.descripcion as [version]
from actadocumentos as ad
	inner join documentoversiones as dv on dv.iddocumentoversion = ad.iddocumentoversion
	inner join documentos as d on d.iddocumento = dv.iddocumento 
	inner join tiposdocumento as td on d.idtipodocumento = td.idtipodocumento
	inner join tipodocumentogrupos as tdg on tdg.IdTipoDocumentoGrupo = td.IdTipoDocumentoGrupo
	inner join estudios as e on e.idestudio = d.idestudio
where ad.idacta = @idacta 
order by ad.ordenestudio, td.idtipodocumentogrupo, ad.ordendocumento

select comentario from actadocumentos where idacta = @idacta 


declare @idestudio int
set @idestudio = 19

select	a.fecha as fechaActa,
		CONVERT(nvarchar(30), a.fecha, 112) + ltrim(str(td.idtipodocumentogrupo)) + ltrim(str(ad.ordendocumento)) as ordenCarta,
		td.idtipodocumentogrupo, tdg.descripcion, 
		ad.ordendocumento, d.descripcion as documento, dv.descripcion as [version], CartaIncluirTercerPunto
from actadocumentos as ad
	inner join actas as a on a.idacta = ad.idacta
	inner join documentoversiones as dv on dv.iddocumentoversion = ad.iddocumentoversion
	inner join documentos as d on d.iddocumento = dv.iddocumento 
	inner join tiposdocumento as td on d.idtipodocumento = td.idtipodocumento
	inner join tipodocumentogrupos as tdg on tdg.IdTipoDocumentoGrupo = td.IdTipoDocumentoGrupo
	inner join estudios as e on e.idestudio = d.idestudio
where d.idestudio = @idestudio
order by ordenCarta



/*
declare @idacta int
set @idacta = 28
delete from actadocumentos where idacta = @idacta 
delete from actaprofesionales where idacta = @idacta
delete from notas where idacta = @idacta
delete from actas where idacta = @idacta 
*/

/*Querys por Documento*/
declare @iddocumento int
set @iddocumento = 39
select * from documentos where iddocumento = @iddocumento
select * from documentoversiones where iddocumento = @iddocumento
select * from documentoversionestados where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento = @iddocumento)
select * from documentoversioncomentarios where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento = @iddocumento)
select * from documentoversionrecordatorios where iddocumentoversion in (select iddocumentoversion from documentoversiones where iddocumento = @iddocumento)

select * from tiposdocumento 
select * from tipodocumentoflujos 
select * from tipodocumentoflujoestados 



/* Query para importar los diagramas

insert sysdiagrams
Select [name], [principal_id], [version], [definition] 
From pmlSysGEIC.dbo.sysdiagrams

*/




-- errror en orden de estudios en anta

select distinct d.IdEstudio into #acta_estudios --Count(distinct d.IdEstudio) as total_estudios 
from ActaDocumentos a
	inner join DocumentoVersiones	as b on a.IdDocumentoVersion = b.IdDocumentoVersion
	inner join Documentos			as c on b.IdDocumento = c.IdDocumento
	inner join Estudios				as d on c.IdEstudio = d.IdEstudio
where a.IdActa = 95


select b.IdEstudio, b.Descripcion as estudio, c.IdEstudio 
from Notas a
	inner join Estudios b on a.IdEstudio = b.IdEstudio
	left join #acta_estudios c on a.IdEstudio = c.IdEstudio
where a.IdActa = 95

select * from Estudios where idestudio = 103

