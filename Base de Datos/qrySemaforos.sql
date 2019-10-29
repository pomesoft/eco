SELECT * FROM Estudios WHERE Codigo = ' D3250C00021' ORDER BY Codigo

SELECT * FROM EstudioTiposDocumento WHERE IdEstudio = 79

SELECT * 
FROM Documentos AS D 
	INNER JOIN DocumentoVersiones AS DV ON D.IdDocumento = DV.IdDocumento 
	INNER JOIN DocumentoVersionEstados AS DVE ON DV.IdDocumentoVersion = DVE.IdDocumentoVersion
WHERE IdEstudio = 79
ORDER BY D.Descripcion

SELECT D.IdDocumento, D.Descripcion AS Documento, MAX(DV.Fecha) AS FechaVersion, MAX(DV.Descripcion) AS VersionDocumento, MAX(DVE.Fecha) AS FechaEstado, ED.Descripcion AS Estado, DVE.EstadoFinal
FROM Documentos AS D 
	LEFT JOIN DocumentoVersiones AS DV ON D.IdDocumento = DV.IdDocumento 
	LEFT JOIN DocumentoVersionEstados AS DVE ON DV.IdDocumentoVersion = DVE.IdDocumentoVersion
	INNER JOIN EstadosDocumento AS ED ON ED.IdEstadoDocumento = DVE.IdEstadoDocumento
WHERE IdEstudio = 79
GROUP BY D.IdDocumento, D.Descripcion
ORDER BY D.Descripcion



SELECT D.IdDocumento, D.Descripcion AS Documento, MAX(DV.Fecha) AS FechaVersion, MAX(DV.Descripcion) AS VersionDocumento, MAX(IdEstadoDocumento) AS IdEstado, DVE.EstadoFinal
FROM Documentos AS D 
	LEFT JOIN DocumentoVersiones AS DV ON D.IdDocumento = DV.IdDocumento 
	LEFT JOIN DocumentoVersionEstados AS DVE ON DV.IdDocumentoVersion = DVE.IdDocumentoVersion
WHERE IdEstudio = 79
GROUP BY D.IdDocumento, D.Descripcion
ORDER BY D.Descripcion



SELECT * FROM Documentos WHERE Vigente = 1

SELECT	E.IdEstudio, E.Codigo AS Estudio, 
		D.IdDocumento, D.Descripcion AS Documento, TD.Descripcion AS TipoDocumento, 
		UVD.IdDocumentoVersion, DV.IdDocumentoVersion, DV.Fecha, DV.Descripcion AS VersionDocumento, 
		UEVD.IdVersionEstado, DVE.IdVersionEstado, DVE.Fecha AS FechaEstado, DVE.IdEstadoDocumento, 
		ED.IdEstadoDocumento, ED.Descripcion AS EstadoDocumentoVersion, DVE.EstadoFinal
FROM Estudios AS E
	INNER JOIN Documentos AS D ON E.IdEstudio = D.IdEstudio
	INNER JOIN TiposDocumento AS TD ON TD.IdTipoDocumento = D.IdTipoDocumento
	LEFT JOIN (SELECT MAX(DV.IdDocumentoVersion) AS IdDocumentoVersion, DV.IdDocumento 
				FROM DocumentoVersiones AS DV 
				GROUP BY DV.IdDocumento) AS UVD ON D.IdDocumento = UVD.IdDocumento 
	LEFT JOIN DocumentoVersiones AS DV ON UVD.IdDocumentoVersion = DV.IdDocumentoVersion
	LEFT JOIN (SELECT MAX(DVE.IdVersionEstado) AS IdVersionEstado, DVE.IdDocumentoVersion  
			   FROM DocumentoVersionEstados AS DVE 
			   GROUP BY DVE.IdDocumentoVersion) AS UEVD ON UEVD.IdDocumentoVersion = DV.IdDocumentoVersion
	LEFT JOIN DocumentoVersionEstados AS DVE ON DVE.IdVersionEstado = UEVD.IdVersionEstado
	LEFT JOIN EstadosDocumento AS ED ON ED.IdEstadoDocumento = DVE.IdEstadoDocumento
WHERE D.Vigente = 1 AND DVE.EstadoFinal = 1 AND E.IdEstudio = 79
ORDER BY E.Codigo, D.Descripcion

