var filaSeleccionada;


$(document).ready(function () {

    filaSeleccionada = -1;
    incializarEventosRecordatorios();
    inicializarBuscadores();

    configurarGrillaRecordatorios();
    cargarDatosRecordatorio();
});



function incializarEventosRecordatorios() {
    $("#btnBuscar").click(function () {
        cargarGrillaRecordatorios();
    });
    $("#btnRecordatorioNuevo").click(function () {
        filaSeleccionada = -1;
        nuevoRecordatorioManual();
    });
    $("#btnRecordatorioGuardar").click(function () {
        guardarRecordatorio();
    });
    $("#btnRecordatorioCancelar").click(function () {
        cancelarRecordatorio();
    });
    
}

function inicializarBuscadores() {
    $('#txtRecordatorioBuscar').searchbox({
        searcher: buscarRecordatorios,
        menu: '#menuRecordatorioBuscar',
        width: '100%',
        prompt: prompBusqueda
    });
}

function configurarGrillaRecordatorios() {
    $('#dg').datagrid({
        title: '',
        width: '100%',
        height: 570,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        nowrap: false,
        showHeader: false,
        columns: [[
		    { field: 'Id', title: 'Código', hidden: true, width: 60 },
                { field: 'Descripcion', title: '', align: 'left', width: 570,
                    formatter: function (value, row, index) {
                        if (value != '') {

                            var texto = '<table style="border:0;<tr>">';
                            texto += '<td style="width:30px;"><input type="text" id="color" style="background:' + row.Color + ';width:20px; height: 20px;"/></td>';
                            texto += '<td style="width:570px;"><div class="dialog-content" style="padding: 5px;">';
                            texto += '<span><strong>' + row.Descripcion + '</strong></span><br />';
                            texto += '<span>Tipo: ' + row.TipoRecordatorioDescripcion + '</span><br />';
                            texto += '<span>Estado: ' + row.EstadoRecordatorioDescripcion + '</span><br />';
                            texto += '<span>Fecha Activación: ' + row.FechaActivacionToString + '</span><br />';
                            texto += '</div></td></tr></table>'
                            return texto;
                        }
                        else
                            return value;
                    }
                }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) mostrarRecordatorio(rowData.Id);
        },
        onSelect: function (rowIndex, rowData) {
            if (rowData) {
                filaSeleccionada = rowIndex;
                mostrarRecordatorio(rowData.Id);
            }
        }
    });
    
}


function buscarRecordatorios(value, name) {
    if (value != prompBusqueda) {

        inicio_idEstudioSeleccionado = -1;

        var parametros;
        parametros = {
            accion: 'BUSCAR-RECORDATORIOS',
            tipoRecordatorio: name == 'TipoRecordatorio' ? value : '',
            descripcion: name == 'Descripcion' ? value : '',
            codigoEstudio: name == 'CodigoEstudio' ? value : '',
            estado: name == 'RecordatoriosActivos' ? '2' : ''
        };

        controladorAJAX_GET('handlers/RecordatoriosHandler.ashx', parametros, function (data) {
            if (data) {
                $('#dg').datagrid('loadData', data);
                $('#dg').datagrid('selectRow', 0);
            }
        });
    }
}

function cargarGrillaRecordatorios() {
    var parametros;
    parametros = {
        accion: 'LISTAR-RECORDATORIOS',
        descripcion: ''
    };

    invocarControladorRecordatorio(parametros, function (data) {
        if (data) {
            if (data.result == 'Error') {
                $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
            }
            else {
                $('#dg').datagrid('loadData', data);
                $('#dg').datagrid('selectRow', (filaSeleccionada == -1 ? 0 : filaSeleccionada));
            }
        }
    });
}


 

function cancelarRecordatorio() {
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrarRecordatorio(row.Id);
}

function eliminarRecordatorio() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR',
                    id: row.Id
                };
                invocarControladorRecordatorio_POST(parametros, actualizarDatosSuccess);
            }
        });
    }
}

function actualizarDatosRecordatorioSuccess(data) {
    if (data.result == 'OK') {
        cargarGrillaRecordatorios();
        $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');        
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}