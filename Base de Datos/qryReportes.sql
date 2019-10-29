
select 
		ROW_NUMBER() OVER(ORDER BY e.EstadoSemaforo ASC) AS Id,
		case e.EstadoSemaforo
			when 1 then 'Estudios Sin Registros'  
			when 2 then 'Estudios Con Pendientes'  
			when 3 then 'Estudios Sin Pendientes'  
		end as Descripcion, count(*) as Valor
from Estudios e
where not e.IdEstado in (3, 4) and e.EstadoSemaforo > 0
group by e.EstadoSemaforo
order by e.EstadoSemaforo


select  ROW_NUMBER() OVER(ORDER BY ee.Descripcion ASC) AS Id,
        ee.Descripcion as Descripcion, Count(*) as Valor
from Estudios e 
    inner join EstadosEstudio ee on e.IdEstado = ee.IdEstadoEstudio
where not e.IdEstado in (3, 4)
group by ee.Descripcion
order by Count(*) desc


/*documentos tratados por mes*/

drop table #tempo	
select	ROW_NUMBER() OVER(ORDER BY MONTH(de.Mes) ASC) AS Id,
		e.Descripcion as Descripcion,
		'' as Descripcion2,
		de.Mes as Valor,		
		count(*) as Valor2
into #tempo		
from (select	MONTH(a.Fecha)  as Mes, 
				ad.IdDocumentoVersion,
				MAX(dve.IdVersionEstado) as ultimaVersionEstado
		from Actas a
			inner join ActaDocumentos ad on a.IdActa = ad.IdActa
			inner join DocumentoVersionEstados as dve on dve.IdDocumentoVersion = ad.IdDocumentoVersion 
		where YEAR(a.Fecha) = 2019
		group by MONTH(a.Fecha), ad.IdDocumentoVersion) as de 

		inner join DocumentoVersionEstados dve on dve.IdVersionEstado = de.ultimaVersionEstado 
		inner join EstadosDocumento as e on dve.IdEstadoDocumento = e.IdEstadoDocumento
group by de.Mes, e.Descripcion
order by de.Mes, e.Descripcion


select * from #tempo where Valor = 5

select * from EstadosDocumento


select	ROW_NUMBER() OVER(ORDER BY MONTH(de.Mes) ASC) AS Id,
		                            e.Descripcion as Descripcion,
		                            '' as Descripcion2,
		                            de.Mes as Valor,		
		                            count(*) as Valor2
		
                            from (select	MONTH(a.Fecha)  as Mes, 
				                            ad.IdDocumentoVersion,
				                            MAX(dve.IdVersionEstado) as ultimaVersionEstado
		                            from Actas a
			                            inner join ActaDocumentos ad on a.IdActa = ad.IdActa
			                            inner join DocumentoVersionEstados as dve on dve.IdDocumentoVersion = ad.IdDocumentoVersion 
		                            where YEAR(a.Fecha) = 2019
		                            group by MONTH(a.Fecha), ad.IdDocumentoVersion) as de 

		                            inner join DocumentoVersionEstados dve on dve.IdVersionEstado = de.ultimaVersionEstado 
		                            inner join EstadosDocumento as e on dve.IdEstadoDocumento = e.IdEstadoDocumento
                            group by de.Mes, e.Descripcion
                            order by de.Mes, e.Descripcion


