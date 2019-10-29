
select * from estudios order by idestudio

SELECT  'EST-' + REPLICATE('0',4-LEN(CONVERT(VARCHAR, IdEstudio))) + CONVERT(VARCHAR, IdEstudio), 
		REPLICATE('0',4-LEN(CONVERT(VARCHAR, IdEstudio))), LEN(CONVERT(VARCHAR, IdEstudio))  
from Estudios
ORDER BY IdEstudio


update Estudios 
set Descripcion = 'ESTUDIO ' + REPLICATE('0',4-LEN(CONVERT(VARCHAR, IdEstudio))) + CONVERT(VARCHAR, IdEstudio),
	Codigo = REPLICATE('0',4-LEN(CONVERT(VARCHAR, IdEstudio))) + CONVERT(VARCHAR, IdEstudio)



select * from centros

update Centros set RazonSocial = 'CENTRO HABILITADO ' + REPLICATE('0',4-LEN(CONVERT(VARCHAR, IdCentro))) + CONVERT(VARCHAR, IdCentro)


update p
set p.Apellido = tp.Descripcion, p.Nombre = 'PROFESIONAL ' + REPLICATE('0',3-LEN(CONVERT(VARCHAR, IdProfesional))) + CONVERT(VARCHAR, IdProfesional)
from profesionales p inner join TiposProfesional as tp on p.IdTipoProfesional = tp.IdTipoProfesional

select tp.Descripcion, * 
from profesionales p inner join TiposProfesional as tp on p.IdTipoProfesional = tp.IdTipoProfesional


update Patrocinadores
set RazonSocial = 'PATROCINADOR ' + + REPLICATE('0',3-LEN(CONVERT(VARCHAR, IdPatrocinador))) + CONVERT(VARCHAR, IdPatrocinador)

select * from Patrocinadores



update usuarios set LoginClave = 'eco1234+'
select * from usuarios




 