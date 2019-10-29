var inicio_idEstudioSeleccionado;
var documentoSeleccionado; 

$(function () {

    inicializarBotones();
    inicializarBuscadores();
    configurar_dgEstudios();
    configurar_dgDocumentos();
    configurar_dgActas();
    configurarGrillasNotas();
    configurarControles_TiposDocumentoSemaforo();

    obtenerDatosBandejaInicio();

    
});

function inicializarBotones() {
    $("#btnDocumentoVerDocumento").click(function () {
        descargarArchivoBandejaInicio();
    });

    $("#btnDocumentoVersiones").click(function () {
        obtenerDocumentoVersiones();
    });

    $("#chkRecordatorioPendiente").click(function () {
        obtenerRecordatoriosDelEstudioSeleccionado();
    });

    $("#btnDocumentoAnular").click(function () {
        anularDocumentoSeleccionado();
    });
    $("#btnDocumentoReactivar").click(function () {
        obtenerDocumentoAnulados();
    });

    $("#btnDocumentoExpandir").click(function () {
        $('#dgDocumentos').datagrid('expandGroup');
    });

    $("#btnDocumentoContraer").click(function () {
        $('#dgDocumentos').datagrid('collapseGroup');
    });

    $('#tabsDatosEstudio').tabs({
        onSelect: function (title, index) {
            switch (index) {
                case 1:
                    cargarEstudioTiposDocumentoSemaforo();
                    break;
//                case 1:
//                    obtenerRecordatoriosDelEstudioSeleccionado();
//                    break;
                case 2:
                    obtenerActasDelEstudioSeleccionado();
                    break;
            }
        }
    });
}

function inicializarBuscadores() {
    $('#txtEstudioBuscar').searchbox({
        searcher: buscarEstudios,
        menu: '#menuEstudioBuscar',
        width: '100%',
        prompt: prompBusqueda
    });

    $('#txtDocumentoBuscar').searchbox({
        searcher: buscarDocumentos,
        menu: '#menuDocumentoBuscar',
        width: '100%',
        prompt: prompBusqueda
    });

    $('#txtActaBuscar').searchbox({
        searcher: '',
        menu: '#menuActaBuscar',
        width: '100%',
        prompt: prompBusqueda
    });  
}

function configurar_dgEstudios() {
    $('#dgEstudios').datagrid({
        title: '',
        width: '100%',
        height: 520,
        autoRowHeight: true,
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
            { field: 'EstadoSemaforo', title: '', width: '35', align: 'center', halign: 'center',
                formatter: function (value, rowData, index) {
                    switch (value) {
                        case 1:
                            return '<img src="css/icons/rojo.png"/>';
                            break;
                        case 2:
                            return '<img src="css/icons/amarillo.png"/>';
                            break;
                        case 3:
                            return '<img src="css/icons/verde.png"/>';
                            break;
                    }
                }
            },
            { field: 'Codigo', title: 'Código', sortable: true, width: 100 },
		    { field: 'Descripcion', title: 'Abreviado', sortable: true, width: 120 },
		    { field: 'EstadoDescripcion', title: 'Estado', sortable: true, width: 120 }
	  ]],
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {
            var datosAMostrar =
                    '<table><tr>' +
                        '<td style="border:0">' +
                            '<p>' + formatearTextoHTML(rowData.NombreCompleto) + '</p>' +
                        '</td>' +
                    '</tr></table>'
            return datosAMostrar;
        },
        onClickRow: function (rowIndex, rowData) {
            if (rowData.Id != inicio_idEstudioSeleccionado)
                obtenerEstudioSeleccionado();
        },
        onDblClickRow: function (rowIndex, rowData) {
            location.href = 'EstudioCargaDatos.aspx?IdEstudio=' + rowData.Id.toString();
        }
    });
}

function obtenerDatosBandejaInicio() {
    var parametros;
    parametros = {
        accion: 'LISTAR'
    };

    controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, obtenerDatosBandejaInicioSuccess);
}

function buscarEstudios(value, name) {
    if (value != prompBusqueda) {

        inicio_idEstudioSeleccionado = -1;

        var parametros;
        parametros = {
            accion: 'BUSCAR-ESTUDIOS',
            codigo: name == 'Codigo' ? value : '',
            abreviado: name == 'Abreviado' ? value : '',
            nombreCompleto: name == 'NombreCompleto' ? value : ''
        };

        controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, obtenerDatosBandejaInicioSuccess);
    }
}

function obtenerDatosBandejaInicioSuccess(data) {
    if (data) {
        if (data.result == 'Error') {
            $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
        } else {
            $('#dgEstudios').datagrid('loadData', data);

            var URLParamIdEstudio = getURLParam("IdEstudio");
            if (URLParamIdEstudio && URLParamIdEstudio != -1)
                inicio_idEstudioSeleccionado = URLParamIdEstudio;

            if (inicio_idEstudioSeleccionado && inicio_idEstudioSeleccionado != -1) {
                var rows = $('#dgEstudios').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].Id == inicio_idEstudioSeleccionado)
                        $('#dgEstudios').datagrid('selectRow', i);
                }
            }
            else {
                $('#dgEstudios').datagrid('selectRow', 0);
            }
            obtenerEstudioSeleccionado();
        }
    }
}


function obtenerEstudioSeleccionado() {
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

    if (estudioSeleccionado) {

        inicio_idEstudioSeleccionado = estudioSeleccionado.Id;

        var parametros;
        parametros = {
            accion: 'OBTENER-ESTUDIO',
            idEstudio: inicio_idEstudioSeleccionado
        };

        controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, obtenerEstudioSeleccionadoSuccess);
    }
}

function eliminarEstudioSeleccionado() {
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

    if (estudioSeleccionado) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + estudioSeleccionado.Codigo + ' ' + estudioSeleccionado.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR-ESTUDIO',
                    idEstudio: estudioSeleccionado.Id
                };
                controladorAJAX_GET('handlers/EstudioCargaDatosHandler.ashx', parametros, obtenerDatosBandejaInicio);
            }
        });
    }
}

function obtenerEstudioSeleccionadoSuccess(data) {
    if (data) {
        if (data.result == 'Error') {
            $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente nuevamente', 'error');
        }
        else {
            $('#tabsDatosEstudio').tabs('select', 0);
            if (data.Documentos) $('#dgDocumentos').datagrid('loadData', data.Documentos);
            seleccionarDocumento();
            if (data.Notas) $('#dgNotas').datagrid('loadData', data.Notas);

            $('#btnEstudioDetalle').attr('href', 'EstudioCargaDatos.aspx?IdEstudio=' + data.Id.toString());
            $('#btnDocumentoNuevo').attr('href', 'DocumentoDatos.aspx?idEstudio=' + data.Id.toString() + '&IdDocumento=-1');
            $('#btnActaNuevo').attr('href', 'ActaDatos.aspx?IdEstudio=' + inicio_idEstudioSeleccionado + '&IdActa=-1');
        }
    }
}

function seleccionarDocumento() {
    var inicio_idDocumentoSeleccionado = getURLParam("IdDocumento");

    if (inicio_idDocumentoSeleccionado && inicio_idDocumentoSeleccionado != -1) {
        var rows = $('#dgDocumentos').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].Id == inicio_idDocumentoSeleccionado)
                $('#dgDocumentos').datagrid('selectRow', i);
        }
    }
    else {
        $('#dgDocumentos').datagrid('selectRow', 0);
    }
}

function obtenerActasDelEstudioSeleccionado() {
    if (inicio_idEstudioSeleccionado) {
        var parametros;
        parametros = {
            accion: 'LISTAR-ACTASESTUDIO',
            idEstudio: inicio_idEstudioSeleccionado
        };
        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros, obtenerActasEstudioSeleccionadoSuccess);
    }
}

function obtenerActasEstudioSeleccionadoSuccess(data) {
    if (data) {
        $('#dgActas').datagrid('loadData', data);
    }
}

function obtenerRecordatoriosDelEstudioSeleccionado() {
    if (inicio_idEstudioSeleccionado) {
        var parametros;
        parametros = {
            accion: 'BUSCAR-RECORDATORIOS-DEL-ESTUDIO',
            idEstudio: inicio_idEstudioSeleccionado,
            pendiente: $('#chkRecordatorioPendiente').is(':checked')
        };
        controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, obtenerRecordatoriosDelEstudioSeleccionadoSuccess);
    }
}

function obtenerRecordatoriosDelEstudioSeleccionadoSuccess(data) {
    if (data) {
        $('#dgEstudioRecordatorios').datagrid('loadData', data);
    }
}

/**************************************************/
/***************** DOCUMENTOS *********************/
function configurar_dgDocumentos() {
    $('#dgDocumentos').datagrid({
        title: '',
        width: '100%',
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        nowrap: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        view: groupview,
        groupField: 'TipoDocumentoIdDescripcion',
        groupFormatter: function (value, rows) {
            var id = value.substring(0, value.indexOf('-'));
            var descripcion = value.substring(value.indexOf('-') + 1);
            var valorFormateado = '<span>' + descripcion + '</span> - ' + rows.length + (rows.length == 1 ? ' documento' : ' documentos');
            var htmlReturn = '<div style=" width: 600px;">'
            htmlReturn += '<div style="width: 550px; float: left;">' + valorFormateado + '</div>';
            htmlReturn += '</div>';

            return htmlReturn;
        },
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Descripcion', title: 'Nombre Documento', sortable: true, width: 300 },
		        //{ field: 'TipoDocumentoDescripcion', title: 'Tipo', sortable: true, width: 200 },
		        { field: 'VersionActualDescripcion', title: 'Versión', sortable: true, width: 80 },
                { field: 'EstadoFinal', title: 'Final', sortable: true, width: 40 },
                { field: 'VersionActualFecha', title: 'Fecha Versión', sortable: true, width: 100 },
                { field: 'EstadoActual', title: 'Estado', sortable: true, width: 120 },
                { field: 'EstadoActualFecha', title: 'Fecha Estado', sortable: true, width: 100 }
	  ]],
        onSelect: function (rowIndex, rowData) {
            documentoSeleccionado = null;
            if (rowData) {
                documentoSeleccionado = $('#dgDocumentos').datagrid('getSelected');
                $('#btnDocumentoDetalle').attr("href", "DocumentoDatos.aspx?IdEstudio=" + rowData.IdEstudio.toString() + "&IdDocumento=" + rowData.Id.toString() + "&IdVersion=" + rowData.IdVersionActual.toString());
            }
        },
        onDblClickRow: function (rowIndex, rowData) {

            location.href = "DocumentoDatos.aspx?IdEstudio=" + rowData.IdEstudio.toString() + "&IdDocumento=" + rowData.Id.toString() + "&IdVersion=" + rowData.IdVersionActual.toString();
        }
    });

    $('#dgDocumentoVersiones').datagrid({
        title: '',
        width: '100%',
        height: 250,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Id', width: 60, hidden: true },
		    { field: 'FechaToString', title: 'Fecha', width: 100 },
		    { field: 'Descripcion', title: 'Versión', width: 480 }
	  ]]
    });

//    $('#dgEstudioRecordatorios').datagrid({
//        width: 600,
//        height: 520,
//        autoRowHeight: false,
//        collapsible: false,
//        pagination: false,
//        rownumbers: false,
//        fitcolumns: true,
//        singleSelect: true,
//        sortName: 'Descripcion',
//        sortOrder: 'asc',
//        columns: [[
//		    { field: 'Id', title: 'Id', width: 60, hidden: true },
//                { field: 'NombreDocumento', title: 'Nombre Documento', width: 280 },
//                { field: 'VersionDocumento', title: 'Versión', width: 140 },
//		    { field: 'Fecha', title: 'Fecha', width: 80 },
//		    { field: 'Pendiente', title: 'Pendiente', width: 60 }
//	  ]],
//        view: detailview,
//        detailFormatter: function (rowIndex, rowData) {
//            var datosAMostrar =
//                    '<table><tr>' +
//                        '<td style="border:0">' +
//                            '<p>Autor: ' + rowData.Autor + '</p>' +
//                            '<p>' + formatearTextoHTML(rowData.Observaciones) + '</p>' +
//                        '</td>' +
//                    '</tr></table>'
//            return datosAMostrar;
//        }
//    });

    $('#dgDocumentosAnulados').datagrid({
        title: 'Seleccione el documento que desea reactivar',
        width: 595,
        height: 280,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Descripcion', title: 'Nombre Documento', sortable: true, width: 300 },
		    { field: 'TipoDocumentoDescripcion', title: 'Tipo', sortable: true, width: 180 },
		    { field: 'VersionActualDescripcion', title: 'Versión', sortable: true, width: 200 },
                { field: 'VersionActualFecha', title: 'Fecha Versión', sortable: true, width: 100 },
                { field: 'EstadoActual', title: 'Estado', sortable: true, width: 120 },
                { field: 'EstadoActualFecha', title: 'Fecha Estado', sortable: true, width: 100 },
                { field: 'EstadoFinal', title: 'Final', sortable: true, width: 40 }
	  ]]
    });
}


function descargarArchivoBandejaInicio() {
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');
    
    if (documentoSeleccionado && documentoSeleccionado.IdVersionActual != -1) {
        if (documentoSeleccionado.VersionActualArchivo != null && documentoSeleccionado.VersionActualArchivo != '') {

            window.open('handlers/ArchivosHandler.ashx?idEstudio=' + estudioSeleccionado.Id.toString()
                                            + '&idDocumento=' + documentoSeleccionado.Id.toString()
                                            + '&idVersion=' + documentoSeleccionado.IdVersionActual.toString());
        }
    }
}

function buscarDocumentos(value, name) {
    if (value == '') {
        obtenerEstudioSeleccionado();
    }
    else if (value != prompBusqueda) {

        var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

        if (estudioSeleccionado) {

            inicio_idEstudioSeleccionado = estudioSeleccionado.Id;

            var parametros;
            parametros = {
                accion: 'BUSCAR-DOCUMENTOS-DEL-ESTUDIO',
                idEstudio: inicio_idEstudioSeleccionado,
                tipoDocumento: name == 'TipoDocumento' ? value : '',
                nombreDocumento: name == 'NombreDocumento' ? value : ''
            };

            controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros, function (data) {
                if (data && data.Documentos)
                    $('#dgDocumentos').datagrid('loadData', data.Documentos);
            });
        }
    }
}

function obtenerDocumentoVersiones() {
    if (documentoSeleccionado) {
        $('#txtDocumentoVersiones_Documento').val(documentoSeleccionado.Descripcion);
        $("#txtDocumentoVersiones_Documento").attr("disabled", "disabled");
        $('#txtDocumentoVersiones_Estudio').val(documentoSeleccionado.NombreEstudio);
        $("#txtDocumentoVersiones_Estudio").attr("disabled", "disabled");

        var parametros;
        parametros = {
            accion: 'LISTAR-DOCUMENTOVERSIONES',
            idDocumento: documentoSeleccionado.Id
        };
        controladorAJAX_GET('handlers/DocumentoDatosHandler.ashx', parametros, function (data) {
            if (data) {
                $('#dgDocumentoVersiones').datagrid('loadData', data);
                $('#dlgDocumentoVersiones').dialog('open').dialog('setTitle', 'Versiones');
            }
        });
    }
}

function cerrarDocumentoVersiones() {
    $('#dlgDocumentoVersiones').dialog('close');
}

function eliminarDocumentoSeleccionado() {
    if (documentoSeleccionado) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + documentoSeleccionado.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR-DOCUMENTO',
                    idEstudio: documentoSeleccionado.IdEstudio,
                    idDocumento: documentoSeleccionado.Id
                };
                controladorAJAX_GET('handlers/DocumentoDatosHandler.ashx', parametros, obtenerEstudioSeleccionado);
            }
        });
    }
}

function anularDocumentoSeleccionado() {
    if (documentoSeleccionado) {
        $.messager.confirm('Confirmar', '¿Desea anular ' + documentoSeleccionado.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ANULAR-DOCUMENTO',
                    idEstudio: documentoSeleccionado.IdEstudio,
                    idDocumento: documentoSeleccionado.Id
                };
                controladorAJAX_GET('handlers/DocumentoDatosHandler.ashx', parametros, obtenerEstudioSeleccionado);
            }
        });
    }
}

function obtenerDocumentoAnulados() {
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');
    if (estudioSeleccionado) {
        var parametros;
        parametros = {
            accion: 'LISTAR-DOCUMENTOSANULADOS',
            idEstudio: estudioSeleccionado.Id
        };
        controladorAJAX_GET('handlers/DocumentoDatosHandler.ashx', parametros, function (data) {
            if (data) {
                $('#txtReactivarDocumento_Estudio').val(estudioSeleccionado.NombreCompleto);
                $("#txtReactivarDocumento_Estudio").attr("disabled", "disabled");
                $('#dgDocumentosAnulados').datagrid('loadData', data);
                $('#dlgReactivarDocumento').dialog('open').dialog('setTitle', 'Documentos Anulados');
            }
        });
    }
}

function aceptarReactivarDocumento() {
    var documentoSeleccionado = $('#dgDocumentosAnulados').datagrid('getSelected');
    if (documentoSeleccionado) {
        $.messager.confirm('Confirmar', '¿Desea reactivar ' + documentoSeleccionado.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'REACTIVAR-DOCUMENTO',
                    idDocumento: documentoSeleccionado.Id
                };
                controladorAJAX_GET('handlers/DocumentoDatosHandler.ashx', parametros, obtenerEstudioSeleccionado);
            }
        });
        $('#dlgReactivarDocumento').dialog('close');
    }
}

function cancelarReactivarDocumento() {
    $('#dlgReactivarDocumento').dialog('close');
}



/**************************************************/
/*********************  ACTAS *********************/
function configurar_dgActas() {
    $('#dgActas').datagrid({
        title: '',
        width: '100%',
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        nowrap: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
                { field: 'Id', title: 'Id', width: 60, hidden: true },
		    { field: 'Fecha', title: 'Fecha', width: 100 },
		    { field: 'Descripcion', title: 'Descripción', width: 530 },
		    { field: 'Cerrada', title: 'Cerrada', width: 120, 
                formatter: function (value, row, index) {
		            if (row.Cerrada) return 'SI';
		            else return 'NO';
		        }
		    }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                $('#btnActaDetalle').attr('href', 'ActaDatos.aspx?IdEstudio=' + inicio_idEstudioSeleccionado + '&IdActa=' + rowData.Id);
            }
        },
        onDblClickRow: function (rowIndex, rowData) {
            location.href = 'ActaDatos.aspx?IdEstudio=' + inicio_idEstudioSeleccionado + '&IdActa=' + rowData.Id;
        }
    });
}

function nuevaActa() {
    limpiarControlesActa();
    idActa = -1;
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');
    if (estudioSeleccionado)
        $('#txtActaEstudio').val(estudioSeleccionado.Descripcion);
    $("#txtActaEstudio").attr("disabled", "disabled");
    $('#txtActaFecha').focus();
    $('#dlgActa').dialog('open').dialog('setTitle', 'Nueva Acta');
}


/*********************************************************************/
/***************** TISPO DE DOCUMENTO - SEMAFORO *********************/
function configurarControles_TiposDocumentoSemaforo() {
    $('#dgTiposDocumentoSemaforo').datagrid({
        title: '',
        width: '100%',
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        nowrap: false,
        sortName: 'TipoDocumentoDescripcion',
        sortOrder: 'asc',
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'EstadoSemaforo', title: '', width: '35', align: 'center', halign: 'center',
                formatter: function (value, rowData, index) {
                        switch (value) {
                            case 1:
                                return '<img src="css/icons/rojo.png"/>';
                                break;
                            case 2:
                                return '<img src="css/icons/amarillo.png"/>';
                                break;
                            case 3:
                                return '<img src="css/icons/verde.png"/>';
                                break;
                        }                        
                    }
                },
		        { field: 'TipoDocumentoDescripcion', title: 'Tipo de Documento', sortable: true, width: 650 }
	    ]],
        toolbar: [{
            text: 'Seleccionar Tipos de Documentos',
            iconCls: 'icon-add',
            handler: agregarTipoDocumentoSemaforo
        }, {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarTipoDocumentoSemaforo
        }],
        onLoadSuccess: function (rowData) {
            
        }

    });

    $('#dgSeleccionTispoDocumentoSemaforo').datagrid({
        title: '',
        width: 800,
        height: 480,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: false,
        remoteSort: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
                { field: 'Check', width: '5%', checkbox: true },
		        { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Descripcion', title: 'Tipo de Documento', sortable: true, width: '95%' }
	  ]],
        onClickRow: function (rowIndex, rowData) {

        }
    });

    $('#dlgSeleccionTiposDocumentoSemaforo').dialog({
        title: 'Selección tipos de documento para el semaforo',
        width: 820,
        height: 600,
        closed: true,
        modal: true,
        padding: 10,
        toolbar: [{
            text: 'Aceptar',
            iconCls: 'icon-ok',
            handler: aceptarDlgTipoDocumentoSemaforo
        }, {
            text: 'Cancelar',
            iconCls: 'icon-cancel',
            handler: cancelarDlgTipoDocumentoSemaforo
        }],
        onOpen: cargarTiposDocumento
    });
}

function cargarEstudioTiposDocumentoSemaforo() {

    $('#dgTiposDocumentoSemaforo').datagrid('loadData', []);
    
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

    if (estudioSeleccionado) {
        var parametros;
        parametros = {
            accion: 'LISTAR-TIPOSDOCUMENTO_SEMAFORO',
            idEstudio: estudioSeleccionado.Id
        };

        controladorAJAX_GET('handlers/EstudioCargaDatosHandler.ashx', parametros,
        function (data) {
            if (data) {
                $('#dgTiposDocumentoSemaforo').datagrid('loadData', data);                
            }
        });
    }
}

function agregarTipoDocumentoSemaforo() {
    $('#dlgSeleccionTiposDocumentoSemaforo').dialog('center').dialog('open');
}

function quitarTipoDocumentoSemaforo() {
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');
    var etdSeleccionado = $('#dgTiposDocumentoSemaforo').datagrid('getSelected');

    if (estudioSeleccionado && etdSeleccionado) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + etdSeleccionado.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR-TIPOSDOCUMENTO_SEMAFORO',
                    idEstudio: estudioSeleccionado.Id,
                    idTipoDocumento: etdSeleccionado.Id
                };
                controladorAJAX_GET('handlers/EstudioCargaDatosHandler.ashx', parametros,
                    function (data) {
                        cargarEstudioTiposDocumentoSemaforo();
                    });
            }
        });
    }    
}

function aceptarDlgTipoDocumentoSemaforo() {

    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

    if (estudioSeleccionado) {

        var parametros = {
            accion: 'GRABAR-TIPOSDOCUMENTO_SEMAFORO',
            idEstudio: estudioSeleccionado.Id,
            tiposDocumento: JSON.stringify(obtenerDocumenosSeleccionadosSemaforo())
        };

        controladorAJAX_POST('handlers/EstudioCargaDatosHandler.ashx', parametros,
            function (data) {
                if (data.result == 'Error') {
                    $.messager.alert('Error', data.message, 'error');
                }
                $('#dgSeleccionTispoDocumentoSemaforo').datagrid('uncheckAll');
                $('#dlgSeleccionTiposDocumentoSemaforo').dialog('close');
                
                obtenerDatosBandejaInicio();
            
                var rows = $('#dgEstudios').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    if (rows[i].Id == estudioSeleccionado.Id)
                        $('#dgEstudios').datagrid('selectRow', i);
                }                

            });
    }    
}

function cancelarDlgTipoDocumentoSemaforo() {

    $('#dlgSeleccionTiposDocumentoSemaforo').dialog('close');
}


function cargarTiposDocumento() {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        descripcion: ''
    };

    controladorAJAX_GET('handlers/TiposDocumentoHandler.ashx', parametros,
        function (data) {
            if (data) {
                $('#dgSeleccionTispoDocumentoSemaforo').datagrid('loadData', data);

                var rows = $('#dgSeleccionTispoDocumentoSemaforo').datagrid('getRows');
                var rowsSemaforoEstudio = $('#dgTiposDocumentoSemaforo').datagrid('getRows');
                
                if (rowsSemaforoEstudio.length > 0) {
                    for (var i = 0; i < rows.length; i++) {
                        for (var j = 0; j < rowsSemaforoEstudio.length; j++) {
                            if (rows[i].Descripcion == rowsSemaforoEstudio[j].TipoDocumentoDescripcion)
                                $('#dgSeleccionTispoDocumentoSemaforo').datagrid('checkRow', i);
                        }
                    }
                } else {
                    
                    for (var i = 0; i < rows.length; i++) {
                        if (rows[i].NecesarioAprobacionEstudio)
                            $('#dgSeleccionTispoDocumentoSemaforo').datagrid('checkRow', i);
                    }
                }
            }
        });
}


function obtenerDocumenosSeleccionadosSemaforo() {
    var docsSeleccionados = new Array();
    var doc;

    var rows = $('#dgSeleccionTispoDocumentoSemaforo').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        doc = new Object();
        doc.Id = rows[i].Id;
        doc.Descripcion = rows[i].Descripcion;

        docsSeleccionados.push(doc);
    }

    return docsSeleccionados;
}