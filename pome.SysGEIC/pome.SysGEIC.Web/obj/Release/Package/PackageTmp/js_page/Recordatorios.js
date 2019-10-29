var idSeleccionado;
var infoMailEstados = [
        { id: '-1', descripcion: '' },
        { id: '1', descripcion: 'PENDIENTE' },
        { id: '2', descripcion: 'ENVIADO' },
        { id: '3', descripcion: 'REENVIAR' }
];

var TIPO_MANUAL_ID = 1;
var TIPO_RENOVACIONESTUDIO_ID = 2;
var TIPO_INFORMEAVANCE_ID = 3;
var TIPO_INACTIVIDADDOCUMENTO_ID = 4;

var ESTADO_PROGRAMADO_ID = 1;
var ESTADO_PROGRAMADO_DESCRIPCION = 'PROGRAMADO';
var idEstadoRecordatorio;

var dgIngresaMails;


$(document).ready(function () {
    /*NO agregar ninguna function para obtener datos porque se mezcla con loads de otros jquery */
    idSeleccionado = -1;
    gdIngresaMails = '';
    incializarControlColor('SelectorColor');
    incializarEventosDatosRecordatorio();
    configurarGrillaDatosRecordatorio();
    configurarPanelesRecordatorio();
});

function incializarRecordatorioControl() {
   
}

function incializarEventosDatosRecordatorio() {
    $("#btnSeleccionarDocumentos").click(function () {
        seleccionarDocumentosRecordatorio();
    });
    $("#btnSeleccionDocumentosAceptar").click(function () {
        aceptarDocumentosSeleccionados();
    });
    $("#btnSeleccionDocumentosCancelar").click(function () {
        cancelarDocumentosSeleccionados();
    });
    $("#btnReactivarRecordatorio").click(function () {
        reactivarEstadoRecordatorio();
    });
    $("#btnInfoMailReenviar").click(function () {
        enviarEmailRecordatorio();
    });
    $("#btnIngresoEmailsAceptar").click(function () {
        ingresoEmailsAceptar();
    });
    $("#btnIngresoEmailsCancelar").click(function () {
        ingresoEmailsCancelar();
    });

    $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox({
        onSelect: function (row) {
            EstablecerDefaultTipoRecordatorio(row);
        }
    });
}

function configurarGrillaDatosRecordatorio() {
    $('#dgRecordatorioDocumentos').datagrid({
        title: '',
        width: '100%',
        height: 120,
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
		    { field: 'Descripcion', title: 'Documentos', sortable: true, width: '100%' }
	  ]],
        onClickRow: function (rowIndex, rowData) {
          
        }
    });

    $('#dgSeleccionDocumentos').datagrid({
        title: '',
        width: 780,
        height: 500,
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
                { field: 'Check', width: 40, checkbox: true },
		    { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Descripcion', title: 'Documentos', sortable: true, width: 700 }
	  ]],
        onClickRow: function (rowIndex, rowData) {

        }
    });

    $('#dgInfoMailToEmails').datagrid({
        title: 'Destinatarios',
        width: '100%',
        height: 120,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        showHeader: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Descripcion', title: 'Correo electrónico', width: '900%' },
                { field: 'BotonQuitarEmail', title: '', width: '10%', align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/cancel.png"/></a>';
                    }
                }
	  ]],
        tools: [
            {
                iconCls: 'icon-add',
                handler: mostrarIngresoEmailsTO
            }
        ],
        onClickCell: function (rowIndex, field, value) {
            if (field == 'BotonQuitarEmail') {
                eliminarEmailRecordatorio('dgInfoMailToEmails', rowIndex);
            }
        }
    });


    $('#dgInfoMailCCEmails').datagrid({
        title: 'Con Copia',
        width: '100%',
        height: 120,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        showHeader: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Descripcion', title: 'Correo electrónico', width: '90%' },
                { field: 'BotonQuitarEmail', title: '', width: '10%', align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/cancel.png"/></a>';
                    }
                }
	  ]],
        tools: [
            {
                iconCls: 'icon-add',
                handler: mostrarIngresoEmailsCC
            }
        ],
        onClickCell: function (rowIndex, field, value) {
            if (field == 'BotonQuitarEmail') {
                eliminarEmailRecordatorio('dgInfoMailCCEmails', rowIndex);
            }
        }
    });
}

function invocarControladorRecordatorio(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/RecordatoriosHandler.ashx', dataParametros, funcionOK);
}

function invocarControladorRecordatorio_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/RecordatoriosHandler.ashx', dataParametros, funcionOK);
}


function configurarPanelesRecordatorio() {
    $('#panelTextoAviso').panel({
        width: '100%',
        height: 180,
        title: 'Texto recordatorio/aviso',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                ctrolSeleccionPlantilla_abrir('txtTextoAviso');
            }
        }]
    });

    $('#panelInfoMailTexto').panel({
        width: '100%',
        height: 200,
        title: 'Texto Correo Electrónico',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                ctrolSeleccionPlantilla_abrir('txtInfoMailTexto');
            }
        }]
    });
}

function cargarDatosRecordatorio() {
    cargarGrillaRecordatorios();
}

function EstablecerDefaultTipoRecordatorio(tipo) {
    $("#chkAvisoMail").prop('checked', tipo.AvisoMail);
    $("#chkAvisoPopup").prop('checked', tipo.AvisoPopup);
    $("#SelectorColor").spectrum('set', tipo.Color);
}



function seleccionarDocumentosRecordatorio() {

    var estudioSeleccionado = $('#ContentPlaceBody_RecordatorioControl1_cboEstudio').combobox('getValue');
    if (estudioSeleccionado) {

        var parametros;
        parametros = {
            accion: 'OBTENER-ESTUDIO',
            idEstudio: estudioSeleccionado
        };

        controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros,
        function (data) {
            if (data && data.Documentos) {
                $('#dgSeleccionDocumentos').datagrid('loadData', data.Documentos);
                $('#dlgSeleccionDocumentos').dialog('center')
                                            .dialog('open')
                                            .dialog('setTitle', 'Seleccionar Documentos');
            }
        });
    }
}

function obtenerDocumenosSeleccionadosRecordatorio() {
    var docsSeleccionados = new Array();
    var doc;

    var rows = $('#dgSeleccionDocumentos').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        doc = new Object();
        doc.Id = rows[i].Id;
        doc.Descripcion = rows[i].Descripcion;

        docsSeleccionados.push(doc);
    }

    return docsSeleccionados;
}

function aceptarDocumentosSeleccionados() {
    $('#dgRecordatorioDocumentos').datagrid('loadData', []);
    var documentosSeleccionados = obtenerDocumenosSeleccionadosRecordatorio();

    if (documentosSeleccionados) $('#dgRecordatorioDocumentos').datagrid('loadData', documentosSeleccionados);

    $('#dlgSeleccionDocumentos').dialog('close');
}

function cancelarDocumentosSeleccionados() {
    $('#dlgSeleccionDocumentos').dialog('close');
}


function mostrarRecordatorio(id) {
    
    var parametros;
    parametros = {
        accion: 'OBTENER-RECORDATORIO',
        idRecordatorio: id
    };

    invocarControladorRecordatorio(parametros, function (data) {
        limpiarControlesRecordatorio();
        idSeleccionado = id;
        $('#txtDescripcion').val(data.Descripcion);
        $("#chkAvisoMail").prop('checked', data.AvisoMail);
        $("#chkAvisoPopup").prop('checked', data.AvisoPopup);
        $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('setValue', data.TipoRecordatorio.Id);
        $("#SelectorColor").spectrum('set', data.Color);
        idEstadoRecordatorio = data.EstadoRecordatorio.Id;
        $('#txtEstadoRecordatorio').val(data.EstadoRecordatorio.Descripcion);
        if (idEstadoRecordatorio == 3)
            $('#btnReactivarRecordatorio').css("display", "inline-block");
        else
            $('#btnReactivarRecordatorio').css("display", "none");


        $('#txtFechaActivacion').datebox('setValue', data.FechaActivacionToString);
        $('#txtTextoAviso').val(data.Texto);

        cargarGrillaEmails('#dgInfoMailToEmails', data.Destinatarios);
        cargarGrillaEmails('#dgInfoMailCCEmails', data.DestinatariosCC);

        $('#txtInfoMailAsunto').val(data.Asunto);
        $('#txtInfoMailTexto').val(data.TextoMail);
        $('#cboInfoMailEstado').combobox('setValue', data.EstadoMail);
        if (data.EstudioId > 0)
            $('#ContentPlaceBody_RecordatorioControl1_cboEstudio').combobox('setValue', data.EstudioId);

        if (data.ListadoDocumentos)
            $('#dgRecordatorioDocumentos').datagrid('loadData', data.ListadoDocumentos);
    });
}



function nuevoRecordatorioGenerico() {
    idSeleccionado = -1;
    limpiarControlesRecordatorio();
    idEstadoRecordatorio = ESTADO_PROGRAMADO_ID;
    $('#txtEstadoRecordatorio').val(ESTADO_PROGRAMADO_DESCRIPCION);    
    $('#cboInfoMailEstado').combobox('setValue', 1);
    $('#txtDescripcion').focus();
}

function nuevoRecordatorioManual() {
    nuevoRecordatorioGenerico();
    $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('setValue', TIPO_MANUAL_ID);
    //TipoRecordatorioDefault($('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('getValue'));
    $('#txtFechaActivacion').datebox('setValue', getFechaActual());
    $("#chkAvisoPopup").prop('checked', true);
}

function nuevoRecordatorioEstudio(tipoRecordatorio, estudio, fechaActivacion) {
    nuevoRecordatorioGenerico();
    $('#txtFechaActivacion').datebox('setValue', fechaActivacion);
    cargarDefaultsRecordatoriosAutomaticos(tipoRecordatorio, estudio);
    EstablecerDefaultTipoRecordatorio(tipoRecordatorio);
    $('#txtDescripcion').val('RECORDATORIO ANUAL ESTUDIO: ' + estudio.NombreEstudioListados);
    $('#txtInfoMailAsunto').val($('#txtDescripcion').val());
}
function mostrarRecordatorioEstudio(tipoRecordatorio, estudio, fechaActivacion, idRecordatorio) {
    cargarDefaultsRecordatoriosAutomaticos(tipoRecordatorio, estudio);
    mostrarRecordatorio(idRecordatorio);
    $('#txtFechaActivacion').datebox('setValue', fechaActivacion);
}

function nuevoRecordatorioAlertaDocumento(tipoRecordatorio, estudio, documentos, fechaActivacion) {
    nuevoRecordatorioGenerico();
    $('#txtFechaActivacion').datebox('setValue', fechaActivacion);
    cargarDefaultsRecordatoriosAutomaticos(tipoRecordatorio, estudio);
    EstablecerDefaultTipoRecordatorio(tipoRecordatorio);
    //$('#dgRecordatorioDocumentos').datagrid('loadData', documentos);

    $('#txtDescripcion').val(tipoRecordatorio.Descripcion + ' - ESTUDIO: ' + estudio.NombreEstudioListados);
    $('#txtInfoMailAsunto').val($('#txtDescripcion').val());
}

function cargarDefaultsRecordatoriosAutomaticos(tipoRecordatorio, estudio) {    
    $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox({
        valueField: 'Id',
        textField: 'Descripcion',
        disabled: true,
        data: [{
            Id: tipoRecordatorio.Id,
            Descripcion: tipoRecordatorio.Descripcion
        }]
    });
    $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('setValue', tipoRecordatorio.Id);
    $('#ContentPlaceBody_RecordatorioControl1_cboEstudio').combobox({
        valueField: 'Id',
        textField: 'NombreEstudioListados',
        data: [{
            Id: estudio.Id,
            NombreEstudioListados: estudio.NombreEstudioListados
        }]
    });
    $('#ContentPlaceBody_RecordatorioControl1_cboEstudio').combobox('setValue', estudio.Id);
}


function limpiarControlesRecordatorio() {
    $('#txtDescripcion').val('');
    $("#chkAvisoMail").prop('checked', false);
    $("#chkAvisoPopup").prop('checked', false);
    $("#SelectorColor").spectrum('set', 'white');

    $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('setValue', '');
    idEstadoRecordatorio = -1;
    $('#txtEstadoRecordatorio').val('');

    $('#txtFechaActivacion').datebox('setValue', '');
    $('#txtTextoAviso').val('');

    $('#ContentPlaceBody_RecordatorioControl1_cboEstudio').combobox('setValue', '');
    //$('#dgRecordatorioDocumentos').datagrid('loadData', []);

    //$('#dgInfoMailToEmails').datagrid('loadData', []);
    //$('#dgInfoMailCCEmails').datagrid('loadData', []);
    $('#txtInfoMailAsunto').val('');
    $('#txtInfoMailTexto').val('');
    $('#cboInfoMailEstado').combobox('setValue', '');

}

function guardarRecordatorio() {

    var tipoRecordatorio = new Object();
    tipoRecordatorio.Id = $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('getValue');
    tipoRecordatorio.Descripcion = $('#ContentPlaceBody_RecordatorioControl1_cboTipoRecordatorio').combobox('getText');

    var estadoRecordatorio = new Object();
    estadoRecordatorio.Id = idEstadoRecordatorio;
    estadoRecordatorio.Descripcion = $('#lblEstadoRecordatorio').text();

    var datos = new Object();
    datos.Id = idSeleccionado;
    datos.Descripcion = $('#txtDescripcion').val();
    datos.AvisoMail = $("#chkAvisoMail").is(':checked');
    datos.AvisoPopup = $("#chkAvisoPopup").is(':checked');
    datos.TipoRecordatorio = tipoRecordatorio;
    datos.EstadoRecordatorio = estadoRecordatorio;    
    datos.Texto = $('#txtTextoAviso').val();
    datos.Destinatarios = obtenerEmailsIngresados('#dgInfoMailToEmails');
    datos.DestinatariosCC = obtenerEmailsIngresados('#dgInfoMailCCEmails');
    datos.Asunto = $('#txtInfoMailAsunto').val();
    datos.TextoMail = $('#txtInfoMailTexto').val();
    datos.EstadoMail = $('#cboInfoMailEstado').combobox('getValue');

    var colorSeleccionado = $("#SelectorColor").spectrum('get');
    datos.Color = colorSeleccionado.toHexString();

    var parametros;
    parametros = {
        accion: 'GRABAR-RECORDATORIO',
        id: idSeleccionado.toString(),
        datos: JSON.stringify(datos),
        fechaActivacion: $('#txtFechaActivacion').datebox('getValue'),
        idEstudio: $('#ContentPlaceBody_RecordatorioControl1_cboEstudio').combobox('getValue'),
        documentos: obtenerDocumentosRecordatorio()
    };
    invocarControladorRecordatorio_POST(parametros, actualizarDatosRecordatorioSuccess);
}

function reactivarEstadoRecordatorio() {
    var parametros;
    parametros = {
        accion: 'CAMBIAR-ESTADO-RECORDATORIO',
        id: idSeleccionado.toString(),
        idEstado: 2
    };
    invocarControladorRecordatorio_POST(parametros, function () {
        mostrarRecordatorio(idSeleccionado);
    });
}
function obtenerDocumentosRecordatorio() {
    var documentos = '';

    var rows = $('#dgRecordatorioDocumentos').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        documentos = documentos + ';' + rows[i].Id;
    }

    return documentos;
}

function enviarEmailRecordatorio() {
    var parametros;
    parametros = {
        accion: 'ENVIAR-EMAILRECORDATORIO',
        idRecordatorio: idSeleccionado.toString()
    };
    invocarControladorRecordatorio_POST(parametros, function (data) {
        if (data.result == 'Error') {
            $.messager.alert('Error', 'Ocurrió un error al enviar el correo electrónico.', 'error');
        } else {
            $.messager.alert('Confirmación', 'Se envió el correo electrónico', 'info');
        }
    });
}

function eliminarEmailRecordatorio(dg, rowIndex) {
    var row = $('#' + dg).datagrid('getRows')[rowIndex];
    $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
        if (r) {
            $('#' + dg).datagrid('deleteRow', rowIndex);
        }
    });
}


function mostrarIngresoEmailsTO() {
    dgIngresaMails = '#dgInfoMailToEmails';    
    mostrarIngresoEmails(' - Destinatarios');
}

function mostrarIngresoEmailsCC() {
    dgIngresaMails = '#dgInfoMailCCEmails';
    mostrarIngresoEmails(' - Con Copia');
}

function mostrarIngresoEmails(titulo) {
    $('#ContentPlaceBody_RecordatorioControl1_cboInfoMailListas').combobox('setValue', '');
    $('#dlgIngresoEmails').dialog('open').dialog('setTitle', 'Ingresar correo electrónico' + titulo);
}

function ingresoEmailsAceptar() {

    if (!$('#txtInfoMailEmail').validatebox('isValid')) {
        $.messager.alert('Error', 'El correo electrónico es incorrecto', 'error');
    }

    else if ($('#txtInfoMailEmail').val() != '') {

        var emailExiste = false;
        var rows = $(dgIngresaMails).datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].Descripcion == $('#txtInfoMailEmail').val())
                emailExiste = true;
        }

        if (!emailExiste) {
            $(dgIngresaMails).datagrid('appendRow', {
                Descripcion: $('#txtInfoMailEmail').val()
            });
            $('#txtInfoMailEmail').val('');
            $('#txtInfoMailEmail').focus();
        } else {
            $.messager.alert('Error', 'El correo electrónico ingresado ya existe', 'error');
        }
    }

    else if ($('#ContentPlaceBody_RecordatorioControl1_cboInfoMailListas').combobox('getText') != '') {
        var parametros;
        parametros = {
            accion: 'OBTENER-LISTASEMAILS',
            id: $('#ContentPlaceBody_RecordatorioControl1_cboInfoMailListas').combobox('getValue')
        };

        controladorAJAX_GET('handlers/ListasEmailsHandler.ashx', parametros, function (data) {
            if (data) {
                if (data.result == 'Error') {
                    $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
                } else {
                    $.each(data.Emails, function (i, valor) {
                        $(dgIngresaMails).datagrid('appendRow', { Descripcion: valor.Descripcion });

                        $('#ContentPlaceBody_RecordatorioControl1_cboInfoMailListas').combobox('setText', '');
                        $('#ContentPlaceBody_RecordatorioControl1_cboInfoMailListas').focus();
                    });
                }
            }
        });
    }
}

function ingresoEmailsCancelar() {
    dgIngresaMails = '';
    $('#dlgIngresoEmails').dialog('close');
}

function obtenerEmailsIngresados(dgEmails) {
    var emailsIngresados = '';
    var rows = $(dgEmails).datagrid('getRows');

    $.each(rows, function (i, valor) {
        emailsIngresados += valor.Descripcion + ';';
    });

    return emailsIngresados;
}

function cargarGrillaEmails(dgEmails, valoresEmails) {
    var emails = valoresEmails.split(';');
    $.each(emails, function (i, valor) {
        if (valor != '') {
            $(dgEmails).datagrid('appendRow', { Descripcion: valor });
        }
    });
}