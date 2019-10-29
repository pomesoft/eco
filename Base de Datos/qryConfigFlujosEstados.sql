

--select * from [dbo].[TiposDocumento] where idtipodocumento = 41

delete [dbo].[TipoDocumentoFlujoEstados]


SET IDENTITY_INSERT [dbo].[TipoDocumentoFlujoEstados] ON
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (1, 15, 1, NULL, 0)
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (2, 15, 2, 1, 0)
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (3, 15, 3, 2, 1)
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (4, 15, 4, 2, 1)
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (5, 1, 1, NULL, 0)
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (6, 1, 2, 1, 0)
INSERT [dbo].[TipoDocumentoFlujoEstados] ([IdFlujoEstado], [IdFlujo], [IdEstado], [IdEstadoPadre], [Final]) VALUES (7, 1, 5, 2, 1)
SET IDENTITY_INSERT [dbo].[TipoDocumentoFlujoEstados] OFF

insert into TipoDocumentoFlujoEstados (IdFlujo, IdEstado, IdEstadoPadre, Final)
select F.IdFlujo, FE.IdEstado, FE.IdEstadoPadre, FE.Final 
from dbo.TipoDocumentoFlujoEstados as FE, dbo.TiposDocumento as TD 
inner join dbo.TipoDocumentoFlujos AS F on TD.IdTipoDocumento = F.IdTipoDocumento
where Td.Vigente = 1 
	and IdTipoDocumentoGrupo = 1
	and FE.IdFlujo = 15

insert into TipoDocumentoFlujoEstados (IdFlujo, IdEstado, IdEstadoPadre, Final)
select F.IdFlujo, FE.IdEstado, FE.IdEstadoPadre, FE.Final  
from dbo.TipoDocumentoFlujoEstados as FE, dbo.TiposDocumento as TD 
inner join dbo.TipoDocumentoFlujos AS F on TD.IdTipoDocumento = F.IdTipoDocumento
where Td.Vigente = 1 
	and IdTipoDocumentoGrupo =2
	and FE.IdFlujo = 1

/*

select * 
from dbo.TiposDocumento as TD 
inner join dbo.TipoDocumentoFlujos AS F on TD.IdTipoDocumento = F.IdTipoDocumento
inner join dbo.TipoDocumentoGrupos as G on G.IdTipoDocumentoGrupo = TD.IdTipoDocumentoGrupo
where Td.Vigente = 1 and IdFlujo in (38)

select * 
from dbo.TipoDocumentoFlujoEstados
where IdFlujo in (15, 10)

select * 
from dbo.TipoDocumentoFlujoEstados as FE, dbo.TiposDocumento as TD 
inner join dbo.TipoDocumentoFlujos AS F on TD.IdTipoDocumento = F.IdTipoDocumento
where Td.Vigente = 1 
	--and RequiereVersion = 1
	and F.IdFlujo in (15, 1)
	and TD.IdTipoDocumento in (15, 1)

*/
