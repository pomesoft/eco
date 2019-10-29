var idSeleccionado;
var idFlujoSeleccionado;

var ID_INFROME_AVANCE = 3;
var ID_INACTIVIDAD = 4;
var ID_VENCIMIENTO = 5;


$(document).ready(function () {

    idSeleccionado = -1;
    inicializarBuscadores();
    configurarPaneles();
    configurarGrilla();
    cargarGrilla();

});


function configurarPaneles() {
    $('#panelDatos').panel({
        width: '100%',
        //height: 430,
        title: 'Datos Tipo de Documento seleccionado'
    });
}

function inicializarBuscadores() {
    $('#txtTipoDocumentoBuscar').searchbox({
        searcher: buscarTiposDocumento,
        menu: '#menuTipoDocumentoBuscar',
        width: '100%',
        prompt: prompBusqueda
    });
}



function configurarGrilla() {
    $('#dg').datagrid({
        title: 'Información Tipos de Documentos',
        width: '100%',
        height: 635,
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
		    { field: 'Descripcion', title: 'Descripción', sortable: true, width: '70%' },
            { field: 'TipoDocumentoGrupoDescripcion', title: 'Tratamiento en Acta', sortable: true, width: '30%' }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });

    $('#dgFlujos').datagrid({
        title: 'Circuito de Estados',
        width: '98%',
        height: 80,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        showHeader: false,
        columns: [[
		    { field: 'Id', title: 'Código', width: 60, hidden: true },
		    { field: 'Descripcion', title: 'Descripción', width: '100%' }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                mostrarFlujo(rowData);
            }
        }
    });

    $('#dgFlujoEstados').datagrid({
        title: '',
        width: '98%',
        height: 135,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    {
		        field: 'Id', title: 'Código', width: 60, hidden: true
		    },
		    {
		        field: 'EstadoPadre', title: 'Estado Actual', width: '40%',
		        formatter: function (value, row, index) {
		            if (row.EstadoPadre) return row.EstadoPadre.Descripcion;
		            else return value;
		        }
		    },
            {
                field: 'Estado', title: 'Estado Próximo', width: '40%',
                formatter: function (value, row, index) {
                        if (row.Estado) return row.Estado.Descripcion;
                        else return value;
                    }
                },
		    {
		        field: 'Final', title: 'Final', width: '20%',
		        formatter: function (value, row, index) {
		            if (row.Final) return 'SI';
		            else return 'NO';
		        } 
		    }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            
        }
    });
}

function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/TiposDocumentoHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/TiposDocumentoHandler.ashx', dataParametros, funcionOK);
}


function buscarTiposDocumento(value, name) {
    if (value != prompBusqueda) {
        cargarGrilla(name == 'Descripcion' ? value : '');
    }
}

function cargarGrilla(descripcionBuscar) {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        descripcion: descripcionBuscar
    };

    invocarControlador(parametros, function (data) {
        $('#dg').datagrid('loadData', data);

        if (idSeleccionado != -1) {
            var rows = $('#dg').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].Id == idSeleccionado) {
                    $('#dg').datagrid('selectRow', i);
                    break;
                }
            }
        }
        else {
            $('#dg').datagrid('selectRow', 0);
        }

        var row = $('#dg').datagrid('getSelected');
        if (row) mostrar(row.Id);
    });
}

function mostrar(id) {
    var parametros;
    parametros = {
        accion: 'OBTENER',
        id: id
    };

    invocarControlador(parametros, cargarControles);
}

function cargarControles(data) {
    limpiarControles();

    if (data) {
        idSeleccionado = data.Id;
        $('#txtDescripcion').textbox('setValue', data.Descripcion);
        $("#chkRequiereVersion").prop('checked', data.RequiereVersion);
        $("#chkCartaRespuesta").prop('checked', data.ListarCartaRespuesta);
        $("#chkListarDocsCartaRespuesta").prop('checked', data.ListarDocsCartaRespuesta);
        $("#chkNecesarioAprobacionEstudio").prop('checked', data.NecesarioAprobacionEstudio);

        if (data.TipoDocumentoGrupo)
            $('#cboTipoDocumentoGrupo').combobox('setValue', data.TipoDocumentoGrupo.Id);

        $('#dgFlujos').datagrid('loadData', data.Flujos);
        $('#dgFlujos').datagrid('selectRow', 0)

        if (data.TiposRecordatorio) {
            for (var i = 0; i < data.TiposRecordatorio.length; i++) {
                switch (data.TiposRecordatorio[i].IdTipoRecordatorio) {
                    case ID_INFROME_AVANCE:
                        $("#chkRequiereAlertaInformeAvance").prop('checked', true);
                        $('#txtAlertaInformeAvenceMeses').numberbox('setValue', data.TiposRecordatorio[i].Meses);
                        break;

                    case ID_INACTIVIDAD:
                        $("#chkRequiereAlertaInactividad").prop('checked', true);
                        $('#txtAlertaInactividadMeses').numberbox('setValue', data.TiposRecordatorio[i].Meses);
                        break;

                    case ID_VENCIMIENTO:
                        $("#chkRequiereAlertaVencimiento").prop('checked', true);
                        $('#txtAlertaVencimientoMeses').numberbox('setValue', data.TiposRecordatorio[i].Meses);
                        break;
                }
            }
        }
        var row = $('#dgFlujos').datagrid('getSelected');
        if (row) mostrarFlujo(row);
    }
}

function limpiarControles() {
    idSeleccionado = -1;
    idFlujoSeleccionado = -1;
    $('#txtDescripcion').textbox('setValue','');
    $("#chkRequiereVersion").prop('checked', false);
    $("#chkCartaRespuesta").prop('checked', true);
    $("#chkListarDocsCartaRespuesta").prop('checked', true);
    $("#chkNecesarioAprobacionEstudio").prop('checked', false);
    $('#cboTipoDocumentoGrupo').combobox('setValue', '');
    $("#chkRequiereAlertaInformeAvance").prop('checked', false);
    $('#txtAlertaInformeAvenceMeses').numberbox('setValue', '');
    $("#chkRequiereAlertaInactividad").prop('checked', false);
    $('#txtAlertaInactividadMeses').numberbox('setValue', '');
    $("#chkRequiereAlertaVencimiento").prop('checked', false);
    $('#txtAlertaVencimientoMeses').numberbox('setValue', '');
    nuevoFlujo();
}

function nuevo() {
    limpiarControles();

    $('#txtDescripcion').focus();
    
}

function guardar() {

    var tiposRecordatorioSeleccionados = new Array();
    var tipoRecordatorio;

    if ($("#chkRequiereAlertaInformeAvance").is(':checked')) {
        tipoRecordatorio = new Object();
        tipoRecordatorio.IdTipoRecordatorio = ID_INFROME_AVANCE;
        tipoRecordatorio.Meses = $('#txtAlertaInformeAvenceMeses').numberbox('getValue');
        tiposRecordatorioSeleccionados.push(tipoRecordatorio);
    }    
    if ($("#chkRequiereAlertaInactividad").is(':checked')) {
        tipoRecordatorio = new Object();
        tipoRecordatorio.IdTipoRecordatorio = ID_INACTIVIDAD;
        tipoRecordatorio.Meses = $('#txtAlertaInactividadMeses').numberbox('getValue');
        tiposRecordatorioSeleccionados.push(tipoRecordatorio);
    }
    if ($("#chkRequiereAlertaVencimiento").is(':checked')) {
        tipoRecordatorio = new Object();
        tipoRecordatorio.IdTipoRecordatorio = ID_VENCIMIENTO;
        tipoRecordatorio.Meses = $('#txtAlertaVencimientoMeses').numberbox('getValue');
        tiposRecordatorioSeleccionados.push(tipoRecordatorio);
    }
    
    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idSeleccionado,
        descripcion: $('#txtDescripcion').textbox('getValue'),
        requiereVersion: $("#chkRequiereVersion").is(':checked'),
        idTipoDocumentoGrupo: $('#cboTipoDocumentoGrupo').combobox('getValue'),
        listarCartaRespuesta: $("#chkCartaRespuesta").is(':checked'),
        listarDocsCartaRespuesta: $("#chkListarDocsCartaRespuesta").is(':checked'),
        necesarioAprobacionEstudio: $("#chkNecesarioAprobacionEstudio").is('checked'),
        tiposRecordatorio: JSON.stringify(tiposRecordatorioSeleccionados)
    };

    invocarControlador_POST(parametros, actualizarDatosSuccess);
    if (idSeleccionado == -1)
        nuevo();
}

function cancelar() {
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrar(row.Id);
}

function eliminar() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR',
                    id: row.Id
                };
                invocarControlador_POST(parametros, actualizarDatosSuccess);
            }
        });
    }
}

function actualizarDatosSuccess(data) {
    if (data.result == 'OK') {
        $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
        cargarGrilla();
        var rows = $('#dg').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].Id == idSeleccionado)
                $('#dg').datagrid('selectRow', i);
        }
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function nuevoFlujo() {
    idFlujoSeleccionado = -1;
    $('#txtDescripcionFlujo').val('');
    //$('#dgFlujoEstados').datagrid('loadData', null);
    $('#txtDescripcionFlujo').focus();
}

function mostrarFlujo(rowData) {
    idFlujoSeleccionado = rowData.Id;
    $('#txtDescripcionFlujo').val(rowData.Descripcion);
    //if (rowData.Estados)
    $('#dgFlujoEstados').datagrid('loadData', rowData.Estados);
}

function actualizarFlujo() {
    var parametros;
    parametros = {
        accion: 'GRABAR-FLUJO',
        id: idSeleccionado,
        idFlujo: idFlujoSeleccionado,
        descripcionFlujo: $('#txtDescripcionFlujo').val()
    };
    
    actualizarDatosFlujoEstado(parametros, refrescarFlujos);
}

function eliminarFlujo() {

    $.messager.confirm('Tipos de Documentos', '¿Desea eliminar el circuito seleccionado?', function (respuesta) {
        if (respuesta) {
            var parametros;
            parametros = {
                accion: 'ELIMINAR-FLUJO',
                id: idSeleccionado,
                idFlujo: idFlujoSeleccionado
            };

            actualizarDatosFlujoEstado(parametros, refrescarFlujos);            
        }
    });
    
}

function refrescarFlujos(data) {
    $('#dgFlujos').datagrid('loadData', data.Flujos);
    nuevoFlujo();
}

function agregarEstado() {
    var parametros;
    parametros = {
        accion: 'GRABAR-ESTADO',
        id: idSeleccionado,
        idFlujo: idFlujoSeleccionado,
        idEstado: $('#ContentPlaceBody_cboEstado').combobox('getValue'),
        idEstadoPadre: $('#ContentPlaceBody_cboEstadoPadre').combobox('getValue'),
        final: $("#chkFinal").is(':checked')
    };

    actualizarDatosFlujoEstado(parametros, refrescarEstados);
}

function eliminarEstado() {

    $.messager.confirm('Tipos de Documentos', '¿Desea eliminar el estado seleccionado?', function (respuesta) {
        if (respuesta) {

            var parametros;

            var row = $('#dgFlujoEstados').datagrid('getSelected');
            if (row) {

                parametros = {
                    accion: 'ELIMINAR-ESTADO',
                    id: idSeleccionado,
                    idFlujo: idFlujoSeleccionado,
                    idFlujoEstado: row.Id
                };

                actualizarDatosFlujoEstado(parametros, refrescarEstados);
            }
        }
    });
}

function refrescarEstados(data) {
    if (data.Estados)
        $('#dgFlujoEstados').datagrid('loadData', data.Estados);
    limpiarEstados();
}

function actualizarDatosFlujoEstado(dataParametros, funcionSuccess) {

    invocarControlador_POST(dataParametros, funcionSuccess);
       
}

function limpiarEstados() {
    $('#ContentPlaceBody_cboEstado').combobox('setValue', '');
    $('#ContentPlaceBody_cboEstadoPadre').combobox('setValue', '');
    $("#chkFinal").prop('checked', false);
}