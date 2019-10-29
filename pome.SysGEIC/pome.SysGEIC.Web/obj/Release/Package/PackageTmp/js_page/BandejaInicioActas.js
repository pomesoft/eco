var inicio_idActaSeleccionada;
var inicio_idEstudioSeleccionado;

$(function () {

    configurar_dgEstudios();
    configurar_dgDocumentos();
    configurar_dgActas();
    
    obtenerDatosBandejaInicio();

    inicializarBotones();
    inicializarBuscadores();
});

function inicializarBotones() {
    $('#btnActaNuevo').attr('href', 'ActaDatosV2.aspx?IdActa=-1');
}

function inicializarBuscadores() {
    $('#txtActaBuscar').searchbox({
        searcher: buscarActas,
        menu: '#menuActaBuscar',
        width: 290,
        prompt: prompBusqueda
    });

//    $('#txtEstudioBuscar').searchbox({
//        searcher: buscarEstudios,
//        menu: '#menuEstudioBuscar',
//        width: 290,
//        prompt: prompBusqueda
//    });

//    $('#txtDocumentoBuscar').searchbox({
//        searcher: buscarDocumentos,
//        menu: '#menuDocumentoBuscar',
//        width: 590,
//        prompt: prompBusqueda
//    });
}

function obtenerDatosBandejaInicio() {
    var parametros;
    parametros = {
        accion: 'BUSCAR-ACTAS',
        cerrada: false,
        descripcion: ''
    };

    controladorAJAX_GET('handlers/BandejaInicioActasHandler.ashx', parametros, obtenerDatosBandejaInicioSuccess);
}

function obtenerDatosBandejaInicioSuccess(data) {
    if (data) {
        $('#dgActas').datagrid('loadData', data);

        var URLParamIdActa = getURLParam("IdActa");
        if (URLParamIdActa && URLParamIdActa != -1)
            inicio_idActaSeleccionada = URLParamIdActa;

        if (inicio_idActaSeleccionada && inicio_idActaSeleccionada != -1) {
            var rows = $('#dgActas').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].IdActa == inicio_idActaSeleccionada)
                    $('#dgActas').datagrid('selectRow', i);
            }
        }
        else {
            $('#dgActas').datagrid('selectRow', 0);
        }
        obtenerEstudiosActaSeleccionada();
    }
}

/**************************************************/
/*********************  ACTAS *********************/
function configurar_dgActas() {
    $('#dgActas').datagrid({
        title: '',
        width: 300,
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: true,
        columns: [[
                { field: 'IdActa', title: 'Id', width: 60, hidden: true },
		    { field: 'Fecha', title: 'Fecha', sortable: false, width: 80 },
		    { field: 'Descripcion', title: 'Descripción', sortable: true, width: 200 },
		    { field: 'Cerrada', title: 'Cerrada', width: 80, formatter: function (value, row, index) {
		        if (row.Cerrada) return 'SI';
		        else return 'NO';
		    }
		    }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            obtenerEstudiosActaSeleccionada();            
        }
    });
}

function obtenerEstudiosActaSeleccionada() {
    var actaSeleccionada = $('#dgActas').datagrid('getSelected');

    if (actaSeleccionada) {
    
        inicio_idActaSeleccionada = actaSeleccionada.IdActa;
        $('#btnActaDetalle').attr('href', 'ActaDatosV2.aspx?PaginaReturn=BandejaInicioActas&IdActa=' + inicio_idActaSeleccionada);

        var parametros;
        parametros = {
            accion: 'LISTAR-ESTUDIOSDELACTA',
            idActa: inicio_idActaSeleccionada
        };

        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros, obtenerEstudiosActaSeleccionadaSuccess);
    }
}

function obtenerEstudiosActaSeleccionadaSuccess(data) {
    if (data) {
        $('#dgEstudios').datagrid('loadData', data);
        $('#dgDocumentos').datagrid('loadData', []);

        var URLParamIdEstudio = getURLParam("IdEstudio");
        if (URLParamIdEstudio && URLParamIdEstudio != -1)
            inicio_idEstudioSeleccionado = URLParamIdEstudio;

        if (inicio_idEstudioSeleccionado && inicio_idEstudioSeleccionado != -1) {
            var rows = $('#dgEstudios').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].IdActa == inicio_idEstudioSeleccionado)
                    $('#dgEstudios').datagrid('selectRow', i);
            }
        }
        else {
            $('#dgEstudios').datagrid('selectRow', 0);
        }

        obtenerDocumentosEstudioSeleccionado();
    }
}

function buscarActas(value, name) {
    if (value != prompBusqueda) {

        inicio_idActaSeleccionada = -1;

        var _cerrada = undefined;
        if (name == 'Cerradas') _cerrada = true;
        if (name == 'Abiertas') _cerrada = false;

        var parametros;
        parametros = {
            accion: 'BUSCAR-ACTAS',
            cerrada: _cerrada != undefined ? _cerrada : '',
            descripcion: name == 'Descripcion' ? value : ''
        };

        controladorAJAX_GET('handlers/BandejaInicioActasHandler.ashx', parametros, obtenerDatosBandejaInicioSuccess);
    }
}

/**************************************************/
/***************** Estudios *********************/
function configurar_dgEstudios() {
    $('#dgEstudios').datagrid({
        title: '',
        width: 300,
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Codigo',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Id', hidden: true },
                { field: 'Codigo', title: 'Código', sortable: true, width: 100 },
		    { field: 'Descripcion', title: 'Abreviado', sortable: true, width: 190 },
		    { field: 'Estado', title: 'Estado', sortable: true, width: 120 }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            obtenerDocumentosEstudioSeleccionado();            
        }
    });
}

function buscarEstudios(value, name) {
//    if (value != prompBusqueda) {

//        inicio_idEstudioSeleccionado = -1;

//        var parametros;
//        parametros = {
//            accion: 'BUSCAR-ESTUDIOS',
//            codigo: name == 'Codigo' ? value : '',
//            abreviado: name == 'Abreviado' ? value : '',
//            nombreCompleto: name == 'NombreCompleto' ? value : ''
//        };

//        controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, obtenerDatosBandejaInicioSuccess);
//    }
}

function obtenerDocumentosEstudioSeleccionado() {
    var actaSeleccionada = $('#dgActas').datagrid('getSelected');
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

    if (actaSeleccionada && estudioSeleccionado) {
        var parametros = {
            accion: 'OBTENER-ACTAESTUDIODOCUMENTOS',
            idActa: actaSeleccionada.IdActa,
            idEstudio: estudioSeleccionado.Id
        };

        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros, function (data) {
            if (data.result != 'Error') {
                $('#dgDocumentos').datagrid('loadData', data);
            }
        });
    }
}

/**************************************************/
/***************** DOCUMENTOS *********************/
function configurar_dgDocumentos() {
    $('#dgDocumentos').datagrid({
        title: '',
        width: 600,
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Orden',
        sortOrder: 'asc',
        nowrap: false,
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Orden', title: 'Orden', width: 60, hidden: true },
                { field: 'NombreDocumento', title: 'Nombre Documento', sortable: true, width: 350 },
		        { field: 'TipoDocumentoDescripcion', title: 'Tipo', sortable: true, width: 280 },
                { field: 'VersionDocumento', title: 'Versión', sortable: true, width: 200 },
                { field: 'VersionFecha', title: 'Fecha Versión', sortable: true, width: 100 }
	  ]],
        onClickRow: function () {

            var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

            var documentoSeleccionado = $('#dgDocumentos').datagrid('getSelected');
            if (estudioSeleccionado && documentoSeleccionado) {

                var idVersionActual = -1;
                if (documentoSeleccionado.VersionActual != null)
                    idVersionActual = documentoSeleccionado.VersionActual.Id;

                $('#btnDocumentoDetalle').attr("href", "DocumentoDatos.aspx?PaginaReturn=BandejaInicioActas"
                                                                        + "&IdActa=" + inicio_idActaSeleccionada
                                                                        + "&IdEstudio=" + estudioSeleccionado.Id
                                                                        + "&IdDocumento=" + documentoSeleccionado.IdDocumento
                                                                        + "&IdVersion=" + idVersionActual);
            }
        }
    });
}

function buscarDocumentos(value, name) {
//    if (value == '') {
//        obtenerEstudioSeleccionado();
//    }
//    else if (value != prompBusqueda) {

//        var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

//        if (estudioSeleccionado) {

//            inicio_idEstudioSeleccionado = estudioSeleccionado.Id;

//            var parametros;
//            parametros = {
//                accion: 'BUSCAR-DOCUMENTOS-DEL-ESTUDIO',
//                idEstudio: inicio_idEstudioSeleccionado,
//                tipoDocumento: name == 'TipoDocumento' ? value : '',
//                nombreDocumento: name == 'NombreDocumento' ? value : ''
//            };

//            controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, function (data) {
//                if (data && data.Documentos)
//                    $('#dgDocumentos').datagrid('loadData', data.Documentos);
//            });
//        }
//    }
}