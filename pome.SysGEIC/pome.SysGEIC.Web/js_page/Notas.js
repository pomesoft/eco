var idNota;
var idNotaEstudio;
var idNotaActa;

$(document).ready(function () {

    configurarGrillasNotas();
    configurarPanelNota();

});

function configurarGrillasNotas() {
    $('#dgNotas').datagrid({
        title: '',
        width: 775,
        height: 520,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
                { field: 'Id', title: 'Número', width: 60, },
		    { field: 'FechaToString', title: 'Fecha', width: 100 },
		    { field: 'Descripcion', title: 'Descripción', width: 550 }
	  ]],
        toolbar: [{
            text: 'Buscar',
            iconCls: 'icon-search',
            handler: function () {
                $.messager.show({ title: 'Evento', msg: 'Buscar Notas' });
            }
        }, '-', {
            text: 'Nuevo',
            iconCls: 'icon-add',
            handler: nuevaNota
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleNota
        }],
        onClickRow: function () {
        }
    });
}

function configurarPanelNota() {
    $('#panelNotaTexto').panel({
        width: 572,
        height: 220,
        title: 'Nota',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                ctrolSeleccionPlantilla_abrir('txtNotaTexto');
            }
        }]
    });
}
function cargarCboDocumento(id) {
    $('#cboNotaDocumento').combobox({
        url: 'handlers/DocumentoDatosHandler.ashx?accion=LISTAR-DOCUMENTOS&idEstudio=' + id,
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 'auto',
        onSelect: function (rec) {
            if (rec) cargarCboDocumentoVersion(rec.Id);
        }
    });
}

function cargarCboDocumentoVersion(id) {
    $('#cboNotaDocumentoVersion').combobox({
        url: 'handlers/DocumentoDatosHandler.ashx?accion=LISTAR-DOCUMENTOVERSIONES&idDocumento=' + id,
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 'auto'
    });
}

function nuevaNota() {
    limpiarControlesNota();
    
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');
    if (estudioSeleccionado) {
        idNotaEstudio = estudioSeleccionado.Id;
        $('#cboNotaEstudio').combobox('setValue', estudioSeleccionado.NombreCompleto);
        cargarCboDocumento(estudioSeleccionado.Id);
    }
    $('#txtNotaFecha').focus();
    $('#dlgNota').dialog('open').dialog('setTitle', 'Nueva Nota');
}

function limpiarControlesNota() {
    idNota = -1;
    idNotaEstudio = -1;
    idNotaActa = -1
    $('#cboNotaEstudio').combobox('setValue', '');
    $("#cboNotaEstudio").attr('disabled', 'disabled');
    $('#txtNotaFecha').datebox('setValue', '');
    $('#txtNotaNumero').val('');
    $("#txtNotaNumero").attr('disabled', 'disabled');
    $('#txtNotaDescripcion').val('');
    $('#cboNotaAutor').combobox('setValue', '');
    $('#cboNotaDocumento').combobox('setValue', '');
    $('#cboNotaDocumentoVersion').combobox('setValue', '');
    $("#chkNotaRequiereRespuesta").prop('checked', false);
    $('#txtNotaTexto').val('');    
}

function detalleNota() {
    limpiarControlesNota();
    var notaSeleccionada = $('#dgNotas').datagrid('getSelected');
    if (notaSeleccionada) {
        idNota = notaSeleccionada.Id;
        idNotaEstudio = notaSeleccionada.IdEstudio;
        $('#cboNotaEstudio').combobox('setValue', notaSeleccionada.NombreEstudio);
        $('#txtNotaFecha').datebox('setValue', notaSeleccionada.FechaToString);
        $('#txtNotaNumero').val(notaSeleccionada.Id);
        $('#txtNotaDescripcion').val(notaSeleccionada.Descripcion);
        if (notaSeleccionada.Autor)
            $('#cboNotaAutor').combobox('setValue', notaSeleccionada.Autor.Id);

        cargarCboDocumento(notaSeleccionada.IdEstudio);
        $('#cboNotaDocumento').combobox('setValue', notaSeleccionada.NombreDocumento);
        $("#cboNotaDocumento").attr("disabled", "disabled");

        $('#cboNotaDocumentoVersion').combobox('setValue', notaSeleccionada.VersionDocumento);
        $("#cboNotaDocumentoVersion").attr("disabled", "disabled");

        if (notaSeleccionada.RequiereRespuesta)
            $("#chkNotaRequiereRespuesta").prop('checked', notaSeleccionada.RequiereRespuesta);
        $('#txtNotaTexto').val(notaSeleccionada.Texto);

        $('#txtNotaFecha').focus();
        $('#dlgNota').dialog('open').dialog('setTitle', 'Detalle Nota');
    }
}

function guardarNota() {
    
    if(idNotaEstudio == -1) {
        idNotaEstudio = $('#cboNotaEstudio').combobox('getValue');
    }
            
    if (idNotaEstudio != -1) {        
        var parametros = {
            accion: 'GRABAR-NOTA',
            idEstudio: idNotaEstudio,
            idNota: idNota,
            descripion: $('#txtNotaDescripcion').val(),
            fecha: $('#txtNotaFecha').datebox('getValue'),
            idAutor: $('#cboNotaAutor').combobox('getValue'),
            requiereRespuesta: $('#chkNotaRequiereRespuesta').is(':checked'),
            idDocumento: $('#cboNotaDocumento').combobox('getValue'),
            idDocumentoVersion: $('#cboNotaDocumentoVersion').combobox('getValue'),
            texto: $('#txtNotaTexto').val() 
        };
        var url = 'handlers/NotasHandler.ashx';
        controladorAJAX_POST(url, parametros, guardarNotaSucess);
    }
}

function guardarNotaSucess(data) {
    if (data.result == 'OK') {
        $('#dlgNota').dialog('close');
        //$('#dgNotas').datagrid('loadData', data.estudio.Notas);
        obtenerDatosBandejaInicio();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarNota() {
    $('#dlgNota').dialog('close');
}

/*********** FUNCIONES PARA CARGAR NOTAS DESDE ACTAS ***************/

function cargarNotaCboEstudio() {
    $('#cboNotaEstudio').combobox({
        url: 'handlers/EstudioCargaDatosHandler.ashx?accion=LISTAR-ESTUDIOS',
        valueField: 'Id',
        textField: 'NombreCompleto',
        panelHeight: 'auto'
    });
}

function nuevaNotaActa() {
    limpiarControlesNota();
    idNota= -1;
    idNotaEstudio = -1;

    cargarNotaCboEstudio();
   
    $('#txtNotaFecha').focus();
    $('#dlgNota').dialog('open').dialog('setTitle', 'Nueva Nota');
}
