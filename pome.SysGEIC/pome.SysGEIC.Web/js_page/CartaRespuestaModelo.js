/* Notas
* 
* al lado de cada combo de plantillas agregar boton para vista preliminar de la plantilla
*/

var filaSeleccionada;
var idSeleccionado;

$(document).ready(function () {
    filaSeleccionada = -1;
    idSeleccionado = -1;

    incializarEventos();
    configurarGrilla();
    configurarControles();
    cargarGrilla();
});

function incializarEventos() {
    $("#btnCartaRespuestaModeloNuevo").click(function () {
        nuevaCartaRespuestaModelo();
    });
    $("#btnCartaRespuestaModeloEliminar").click(function () {
        eliminarCartaRespuestaModelo();
    });
    $("#btnCartaRespuestaModeloGuardar").click(function () {
        grabarCartaRespuestaModelo();
    });
    $("#btnCartaRespuestaModeloCancelar").click(function () {
        cancelarCartaRespuestaModelo();
    });
}

function configurarControles() {
    //buscador
    $('#txtCartaRespuestaModeloBuscar').searchbox({
        searcher: buscarCartaRespuestaModelos,
        menu: '#menuCartaRespuestaModeloBuscar',
        width: '100%',
        prompt: prompBusqueda
    });

    //paneles
    $('#panelDatos').panel({
        width: '100%',
        height: 550,
        title: 'Datos modelo seleccionado'
    });
}

function configurarGrilla() {
    $('#dgModelos').datagrid({
        title: 'Modelos carta de respuesta',
        width: '100%',
        height: 550,
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
		    { field: 'Id', title: 'Id', hidden: true },
                { field: 'Descripcion', title: 'Descripción', sortable: true, width: '100%' }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            
        },
        onDblClickRow: function (rowIndex, rowData) {

        },
        onSelect: function (rowIndex, rowData) {
            if (rowData) {
                filaSeleccionada = rowIndex;
                mostrarCartaRespuestaModelo(rowData.Id);
            }
        }
    });
}

function invocarControladorCartaRespuestaModelo(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/CartaRespuestaHandler.ashx', dataParametros, funcionOK);
}

function invocarControladorCartaRespuestaModelo_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/CartaRespuestaHandler.ashx', dataParametros, funcionOK);
}

function cargarGrillaCartaRespuestaModelos(parametros) {

    invocarControladorCartaRespuestaModelo(parametros, function (data) {
        if (data) {
            if (data.result == 'Error') {
                $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
            }
            else {
                $('#dgModelos').datagrid('loadData', data);
                $('#dgModelos').datagrid('selectRow', (filaSeleccionada == -1 ? 0 : filaSeleccionada));
            }
        }
    });

}

function cargarGrilla() {
    var parametros;
    parametros = {
        accion: 'LISTAR-CARTARESPUESTAMODELO',
        descripcion: ''
    };
    cargarGrillaCartaRespuestaModelos(parametros);
}

function buscarCartaRespuestaModelos(value, name) {
    if (value != prompBusqueda) {

        var parametros;
        parametros = {
            accion: 'LISTAR-CARTARESPUESTAMODELO',
            descripcion: name == 'Descripcion' ? value : ''            
        };
        cargarGrillaCartaRespuestaModelos(parametros);
    }
}

function limpiarControles() {
    $('#txtDescripcion').textbox('clear');
    $('#chkIncluirDocumentosEvaluados').prop('checked', false);
    $('#chkIncluirDocumentosTomaConocimiento').prop('checked', false);
    $('#chkIncluirDocumentosPedidoCambio').prop('checked', false);
    $('#chkIncluirTodosDocumentosEstudio').prop('checked', false);
    $('#chkIncluirFirmaPresidente').prop('checked', false);
    $('#chkIncluirFirmaMiembros').prop('checked', false);
    $('#ContentPlaceBody_cboPlantillaIntroduccion').combobox('setValue', '');
    $('#ContentPlaceBody_cboPlantillaIntroduccion2').combobox('setValue', '');
    $('#ContentPlaceBody_cboPlantillaPiePagina').combobox('setValue', '');
    $('#ContentPlaceBody_cboPlantillaBuenasPracticas').combobox('setValue', '');
    $('#ContentPlaceBody_cboPlantillaTextoAprobacion').combobox('setValue', '');
    $('#ContentPlaceBody_cboPlantillaTextoFirmaPresidente').combobox('setValue', '');
    $('#txtTextoLibre').textbox('clear');
    
}

function nuevaCartaRespuestaModelo() {
    filaSeleccionada = -1;
    idSeleccionado = -1;
    limpiarControles();
    $('#txtDescripcion').focus();
}

function mostrarCartaRespuestaModelo(id) {
    limpiarControles();

    var parametros;
    parametros = {
        accion: 'OBTENER-CARTARESPUESTAMODELO',
        idCartaRespuestaModelo: id
    };

    invocarControladorCartaRespuestaModelo(parametros, function (data) {
        idSeleccionado = id;
        $('#txtDescripcion').textbox('setText', data.Descripcion);
        $('#chkIncluirDocumentosEvaluados').prop('checked', data.IncluirDocumentosEvaluados);
        $('#chkIncluirDocumentosTomaConocimiento').prop('checked', data.IncluirDocumentosTomaConocimiento);
        $('#chkIncluirDocumentosPedidoCambio').prop('checked', data.IncluirDocumentosPedidoCambio);
        $('#chkIncluirTodosDocumentosEstudio').prop('checked', data.IncluirTodosDocumentosEstudio);
        $('#chkIncluirFirmaPresidente').prop('checked', data.IncluirFirmaPresidente);
        $('#chkIncluirFirmaMiembros').prop('checked', data.IncluirFirmaMiembros);
        if (data.PlantillaIntroduccion != null) {
            $('#ContentPlaceBody_cboPlantillaIntroduccion').combobox('setValue', data.PlantillaIntroduccion.Id);
        }
        if (data.PlantillaIntroduccionOpcional != null) {
            $('#ContentPlaceBody_cboPlantillaIntroduccion2').combobox('setValue', data.PlantillaIntroduccionOpcional.Id);
        }
        if (data.PlantillaPiePagina != null) {
            $('#ContentPlaceBody_cboPlantillaPiePagina').combobox('setValue', data.PlantillaPiePagina.Id);
        }
        if (data.PlantillaBuenasPracticas != null) {
            $('#ContentPlaceBody_cboPlantillaBuenasPracticas').combobox('setValue', data.PlantillaBuenasPracticas.Id);
        }
        if (data.PlantillaTextoAprobacion != null) {
            $('#ContentPlaceBody_cboPlantillaTextoAprobacion').combobox('setValue', data.PlantillaTextoAprobacion.Id);
        }
        if (data.PlantillaTextoFirmaPresidente != null) {
            $('#ContentPlaceBody_cboPlantillaTextoFirmaPresidente').combobox('setValue', data.PlantillaTextoFirmaPresidente.Id);
        }
        $('#txtTextoLibre').textbox('setText', data.TextoLibre);
        
        $('#txtDescripcion').focus();
    });
}

function cancelarCartaRespuestaModelo() {
    mostrarCartaRespuestaModelo(idSeleccionado);
}

function grabarCartaRespuestaModelo() {

    var datos = new Object();
    datos.Descripcion = $('#txtDescripcion').textbox('getText');
    datos.IncluirDocumentosEvaluados = $("#chkIncluirDocumentosEvaluados").is(':checked');
    datos.IncluirDocumentosTomaConocimiento = $("#chkIncluirDocumentosTomaConocimiento").is(':checked');
    datos.IncluirDocumentosPedidoCambio = $("#chkIncluirDocumentosPedidoCambio").is(':checked');
    datos.IncluirTodosDocumentosEstudio = $("#chkIncluirTodosDocumentosEstudio").is(':checked');
    datos.IdPlantillaIntroduccion = $('#ContentPlaceBody_cboPlantillaIntroduccion').combobox('getText');
    datos.IdPlantillaIntroduccion2 = $('#ContentPlaceBody_cboPlantillaIntroduccion2').combobox('getText');
    datos.IdPlantillaPiePagina = $('#ContentPlaceBody_cboPlantillaPiePagina').combobox('getText');
    datos.IdPlantillaBuenasPracticas = $('#ContentPlaceBody_cboPlantillaBuenasPracticas').combobox('getText');
    datos.IdPlantillaTextoAprobacion = $('#ContentPlaceBody_cboPlantillaTextoAprobacion').combobox('getText');
    datos.IdPlantillaTextoFirmaPresidente = $('#ContentPlaceBody_cboPlantillaTextoFirmaPresidente').combobox('getText');
    datos.IncluirFirmaPresidente = $("#chkIncluirFirmaPresidente").is(':checked');
    datos.IncluirFirmaMiembros = $("#chkIncluirFirmaMiembros").is(':checked');
    datos.TextoLibre = $('#txtTextoLibre').textbox('getText');

    var parametros;
    parametros = {
        accion: 'GRABAR-CARTARESPUESTAMODELO',
        id: idSeleccionado.toString(),
        datos: JSON.stringify(datos)
    };

    invocarControladorCartaRespuestaModelo_POST(parametros, actualizarDatosSuccess);
}


function eliminarCartaRespuestaModelo() {
    var row = $('#dgModelos').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR-CARTARESPUESTAMODELO',
                    id: row.Id
                };
                invocarControladorCartaRespuestaModelo_POST(parametros, actualizarDatosSuccess);
            }
        });
    }
}

function actualizarDatosSuccess(data) {
    if (data.result == 'OK') {
        $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
        cargarGrilla();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}