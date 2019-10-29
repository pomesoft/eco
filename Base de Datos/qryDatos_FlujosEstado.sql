select * from dbo.EstadosDocumento

select * 
from dbo.TiposDocumento as td
left join dbo.TipoDocumentoFlujos as f on td.idtipodocumento = f.idtipodocumento
left join dbo.TipoDocumentoFlujoEstados as e on f.idflujo = e.idflujo


delete from dbo.TipoDocumentoFlujoEstados
delete from dbo.EstadosDocumento

/* NOTAS:
* verificar en produccion si los estados grabados en DocumentoVersionEstados son reales, ó si se pueden eliminar
* Circuito Estandar: seria el que tiene los estados básicos propuestos INGRESADO - EN REVISION - PEDIDO DE CAMBIO - APROBADO
* Confirmar si con estos estados alcanzan para realizar el seguimiento del documento, y si el APROBADO sería el estado final
* Al dar de alta un nuevo tipo de documento, proponer el circuito estandar
*/