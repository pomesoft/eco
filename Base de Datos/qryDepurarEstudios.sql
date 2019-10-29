

/*
pmlSysGEIC_PROD.dbo.ActaEstudios: FK_ActaEstudios_Estudios
pmlSysGEIC_PROD.dbo.EstudioCentros: FK_EstudioCentros_Estudio
pmlSysGEIC_PROD.dbo.EstudioMonodrogas: FK_EstudioMonodrogas_Estudio
pmlSysGEIC_PROD.dbo.EstudioParticipantes: FK_EstudioParticipantes_Estudios
pmlSysGEIC_PROD.dbo.EstudioPatrocinadores: FK_EstudioPatrocinador_Estudio
pmlSysGEIC_PROD.dbo.EstudioTiposDocumento: FK_EstudioTiposDocumento_Estudios
pmlSysGEIC_PROD.dbo.Notas: FK_Notas_Estudios
pmlSysGEIC_PROD.dbo.Recordatorios: FK_Recordatorios_Estudios
*/


declare @idEstudio int
declare @idEstudio_Max int
set  @idEstudio = 51
set @idEstudio_Max = 57

select * from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max

delete  from Documentos where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from EstudioCentros where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from EstudioMonodrogas where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from EstudioParticipantes where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from EstudioPatrocinadores where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from EstudioTiposDocumento where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from Notas where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from Recordatorios where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from ActaEstudios where IdEstudio in (select IdEstudio from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max)
delete from Estudios WHERE IdEstudio BETWEEN @idEstudio AND @idEstudio_Max

select IdDocumentoVersion from DocumentoVersiones where IdDocumento not in (select IdDocumento from Documentos)

delete from ActaDocumentos where IdDocumentoVersion in (select IdDocumentoVersion from DocumentoVersiones where IdDocumento not in (select IdDocumento from Documentos))
delete from DocumentoVersiones
