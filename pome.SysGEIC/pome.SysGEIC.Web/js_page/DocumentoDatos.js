﻿var idEstudio;
var nombreEstudio;
var idDocumento;
var idVersion;
var idEstado;
var idComentario;
var idRecordatorioInformeAvence;
var idRecordatorioInactividadDocumento;

var ID_INFROME_AVANCE = 3;
var ID_INACTIVIDAD = 4;
var ID_VENCIMIENTO = 5;

$(document).ready(function () {

    idEstudio = getURLParam("IdEstudio");
    idDocumento = getURLParam("IdDocumento");
    idVersion = getURLParam("IdVersion");

    limpiarControles();
    configurarPaneles();
    incializarEventosControles();
    obtenerDatosDocumento();

});


function incializarEventosControles() {
    $('#ContentPlaceBody_cboTipoDocumento').combobox({
        onSelect: function (row) {
            if (idDocumento == -1) {
                $('#txtNombre').val(row.text);
                setearDefaultRecordatorios(row.value);
            }
        }
    });

    $('#ContentPlaceBody_cboEstado').combobox({
        onLoadSuccess: function () {
            var data = $('#ContentPlaceBody_cboEstado').combobox("getData");
            if (data && data.length == 1)
                $('#ContentPlaceBody_cboEstado').combobox("select", data[0].Id)
        }
    });

    $("#btnActaDocumentoComentarioControl_Guardar").click(function () {
        guardarActaDocumentoComentario();
    });

    $("#btnActaDocumentoComentarioControl_Cancelar").click(function () {
        cancelarActaDocumentoComentario();
    });

    $("#panelAlertaInactividad").css("display", "none");
    $("#chkRequiereAlertaInactividad").click(function () {
        mostrarPanelConfigAlerta('chkRequiereAlertaInactividad', 'panelAlertaInactividad');
    });

    $("#btnConfigAlertaInactividad").click(function () {
        configAlertaInactividadDocumento();
    });

    $("#panelAlertaInformeAvance").css("display", "none");
    $("#chkRequiereAlertaInformeAvance").click(function () {
        mostrarPanelConfigAlerta('chkRequiereAlertaInformeAvance', 'panelAlertaInformeAvance');
    });
    
    $("#btnConfigInformeAvance").click(function () {
        configAlertaInformeAvance();
    });

    $("#panelAlertaVencimiento").css("display", "none");
    $("#chkRequiereAlertaVencimiento").click(function () {
        mostrarPanelConfigAlerta('chkRequiereAlertaVencimiento', 'panelAlertaVencimiento');
    });

    $("#panelAlertaReaprobacion").css("display", "none");
    $("#chkRequiereAlertaVencimiento").click(function () {
        mostrarPanelConfigAlerta('chkRequiereAlertaReaprobacion', 'panelAlertaReaprobacion');
    });
    
    $("#btnConfigVencimiento").click(function () {
        configAlertaVencimiento();
    });
}

function configurarPaneles() {
    $('#panelActaDocumentoControlComentario').panel({
        width: 630,
        height: 350,
        title: 'Nota',
//        tools: [{
//            iconCls: 'icon-view',
//            handler: function () {
//                ctrolSeleccionPlantilla_abrir('txtActaDocumentoControlComentario');
//            }
//        }]
    });
}

function configurarGrillas() {
    $('#dgVersiones').datagrid({
        title: 'Versiones',
        width: 580,
        height: 370,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Id',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Id', width: 60, hidden: true },
		    { field: 'FechaToString', title: 'Fecha', width: 100 },
		    { field: 'Descripcion', title: 'Versión', width: 300 },
		    { field: 'AprobadoANMAT', title: 'Aprobado ANMAT', width: 100, formatter: 
                function (value, rowData, index) {
		            if (rowData.AprobadoANMAT == null) return '';
		            else if (rowData.AprobadoANMAT) return 'SI';
		            else return 'NO';
                }
		    },
		    { field: 'FechaAprobadoANMATToString', title: 'Fecha Aprobado', width: 100 }
	  ]],
        toolbar: [{
            text: 'Buscar',
            iconCls: 'icon-search',
            handler: function () {
                $.messager.show({ title: 'Evento', msg: 'Buscar Versión de Documento' });
            }
        }, '-', {
            text: 'Nuevo',
            iconCls: 'icon-add',
            handler: nuevaVersion
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleVersion
        }, '-', {
            text: 'Adjuntar Archivo',
            iconCls: 'icon-attach',
            handler: adjuntarArchivo
        }, '-', {
            text: 'Ver Archivo',
            iconCls: 'icon-view',
            handler: descargarArchivo
        }],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                idVersion = rowData.Id;
                cargarControlesVersion();
            }
        }
    });

    $('#dgVersionEstados').datagrid({
        title: '',
        width: 580,
        height: 370,
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
		    { field: 'FechaToString', title: 'Fecha', width: 90 },
                { field: 'Estado', title: 'Estado', width: 150, formatter: function (value, row, index) {
                                                                                if (row.Estado) return row.Estado.Descripcion;
                                                                                else return value;
                                                                            } 
                },
		    { field: 'ProfesionalAutor', title: 'Autor', width: 230, formatter: function (value, row, index) {
		                                                                            if (row.ProfesionalAutor) return row.ProfesionalAutor.NombreCompleto;
		                                                                            else return value;
		                                                                        } 
		    },
		    { field: 'EstadoFinal', title: 'Final', width: 90, formatter: function (value, row, index) {
		                                                                        if (row.EstadoFinal) return 'SI';
		                                                                        else return 'NO';
		                                                                    } 
		    }
	  ]],
        toolbar: [{
            text: 'Nuevo',
            iconCls: 'icon-add',
            handler: nuevoEstado
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleEstado
        }, '-', {
            text: 'Eliminar último',
            iconCls: 'icon-remove',
            handler: eliminarUltimoEstado
        }],
        onClickRow: function (rowIndex, rowData) {
            
        }
    });

    $('#dgComentariosActas').datagrid({
        width: 580,
        height: 370,
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
                { field: 'Fecha', title: 'Fecha', width: 80 },
                { field: 'Descripcion', title: 'Acta', width: 230 },
		    { field: 'DocumentoVersion', title: 'Versión', width: 120 },
                { field: 'DocumentoVersionEstado', title: 'Estado Versión', width: 100 }
	  ]],
        toolbar: [{
            text: 'Agregar Versión del Documento a un Acta',
            iconCls: 'icon-add',
            handler: seleccionarActa
        }, {
            text: 'Ingresar comentario',
            iconCls: 'icon-edit',
            handler: ingresarComentarioActa
        }],
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {
            var datosAMostrar = '';
            datosAMostrar += '<p>Acta: <span><strong>' + rowData.Descripcion + '</strong></span><br />';
            datosAMostrar += 'Versión del Documento tratado en el Acta: <span><strong>' + rowData.DocumentoVersion + '</strong></span><br />';
            datosAMostrar += 'Estado de la Versión: <span><strong>' + rowData.DocumentoVersionEstado + '</strong></span></p>';
            datosAMostrar += '<p>Comentario: ' + formatearTextoHTML(rowData.Comentario) + '</p>';
            return datosAMostrar;
        }
    });

    $('#dgSeleccionActa').datagrid({
        width: 550,
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
                { field: 'Fecha', title: 'Fecha', width: 80 },
                { field: 'Descripcion', title: 'Acta', width: 380 },
		    { field: 'Cerrada', title: 'Cerrada', width: 80, formatter: function (value, row, index) {
		        if (row.Cerrada) return 'SI';
		        else return 'NO';
		    }
		    }
	  ]],
        onDblClickRow: function (rowIndex, rowData) {
            if (rowData)
                seleccionActaAgregar();
        }
    });

    $('#dgComentarios').datagrid({
        width: 580,
        height: 370,
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
		    { field: 'ProfesionalAutor', title: 'Autor', width: 460, formatter: function (value, row, index) {
		        if (row.ProfesionalAutor) return row.ProfesionalAutor.NombreCompleto;
		        else return value;
		    }
		    }
	  ]],
        toolbar: [{
            text: 'Nuevo',
            iconCls: 'icon-add',
            handler: nuevoComentario
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleComentario
        }],
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {
            var datosAMostrar =
                    '<table><tr>' +
                        '<td style="border:0">' +
                            '<p>Comentario: ' + formatearTextoHTML(rowData.Observaciones) + '</p>' +
                        '</td>' +
                    '</tr></table>'
            return datosAMostrar;
        }
    });

    $('#dgRecordatorios').datagrid({
        width: 580,
        height: 370,
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
		    { field: 'ProfesionalAutor', title: 'Autor', width: 390, formatter: function (value, row, index) {
		                                                                                    if (row.ProfesionalAutor) return row.ProfesionalAutor.NombreCompleto;
		                                                                                    else return value;
		                                                                                }
		    },
		    { field: 'Pendiente', title: 'Pendiente', width: 70, formatter: function (value, row, index) {
		                                                                                    if (row.Pendiente) return 'SI';
		                                                                                    else return 'NO';
		                                                                                } 
		    }
	  ]],
        toolbar: [{
            text: 'Nuevo',
            iconCls: 'icon-add',
            handler: nuevoRecordatorioDocumento
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleRecordatorio
        }],
        onClickRow: function (rowIndex, rowData) {

        },
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {
            var datosAMostrar =
                    '<table><tr>' +
                        '<td style="border:0">' +
                            '<p>' + formatearTextoHTML(rowData.Observaciones) + '</p>' +
                        '</td>' +
                    '</tr></table>'
            return datosAMostrar;
        }
    });


    $('#dgParticipantes').datagrid({
        title: '',
        width: 580,
        height: 370,
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
		    { field: 'Profesional', title: 'Investigador', width: 560, 
                    formatter: function (value, row, index) {
		            if (row.Profesional) return row.Profesional.NombreCompleto;
		            else return value;
		        }
		    }
	  ]],
        toolbar: [{
            text: 'Asignar investigadores',
            iconCls: 'icon-add',
            handler: seleccionarParticipantes
        }],
        onClickRow: function (rowIndex, rowData) {

        }
    });

    $('#dgSeleccionParticipantes').datagrid({
        title: '',
        width: 500,
        height: 200,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
                { field: 'Check', width: 40, checkbox: true },
                { field: 'NombreCompleto', title: 'Profesionales', width: 520 }
	  ]]
    });
}

function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/DocumentoDatosHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/DocumentoDatosHandler.ashx', dataParametros, funcionOK);
}

function obtenerDatosDocumento() {

    if (!idEstudio) idEstudio = -1;
    if (!idDocumento) idDocumento = -1;
    if (!idVersion) idVersion = -1;

    if (idDocumento != -1) configurarGrillas();

    if (idEstudio != -1) {

        var parametros;
        parametros = {
            accion: 'OBTENER',
            idEstudio: idEstudio,
            idDocumento: idDocumento
        };

        invocarControlador(parametros, cargarControles);
    }
}

function limpiarControles() {
    $('#txtEstudio').val('');
    $('#txtNombre').val('');
    $('#ContentPlaceBody_cboTipoDocumento').combobox('setValue', '');
    $("#chkLimitante").prop('checked', false);
    
    limpiarRecordatoriosAlertas();
}

function limpiarRecordatoriosAlertas() {
    $('#chkRequiereAlertaInactividad').prop('checked', false);
    $('#txtAlertaInactividadMeses').numberbox('setValue', -1); 
    $("#panelAlertaInactividad").css("display", "none");

    $('#chkRequiereAlertaInformeAvance').prop('checked', false);
    $('#txtAlertaInformeAvanceMeses').numberbox('setValue', -1);
    $("#panelAlertaInformeAcance").css("display", "none");

    $('#chkRequiereAlertaVencimiento').prop('checked', false);
    $('#txtAlertaInformeVencimientoMeses').numberbox('setValue', -1);
    $("#panelAlertaVencimiento").css("display", "none");
}

function cargarControles(data) {
    limpiarControles();
    if(data.result && data.result == 'Error') {
        $.messager.alert('Error', data.message, 'error');
    }
    else {
        $('#txtEstudio').val(data.NombreEstudio + '\n' + data.NombreEstudioCompleto);
        nombreEstudio = data.NombreEstudio;
        $("#txtEstudio").attr("disabled", "disabled");
        $('#txtNombre').val(data.Descripcion);
        if(data.TipoDocumento != null){
            $('#ContentPlaceBody_cboTipoDocumento').combobox('setValue', data.TipoDocumento.Id);
        }
        $("#chkLimitante").prop('checked', data.Limitante);

        if(data.RecordatorioInactividadId > 0) {
            $('#chkRequiereAlertaInactividad').prop('checked', true);
            $('#txtAlertaInactividadMeses').numberbox('setValue', data.RecordatorioInactividadMeses);
            mostrarPanelConfigAlerta('chkRequiereAlertaInactividad', 'panelAlertaInactividad');
        }

        if(data.RecordatorioInformeAvanceId > 0){
            $('#chkRequiereAlertaInformeAvance').prop('checked', true);
            $('#txtAlertaInformeAvanceMeses').numberbox('setValue', data.RecordatorioInformeAvanceMeses);
            mostrarPanelConfigAlerta('chkRequiereAlertaInformeAvance', 'panelAlertaInformeAvance');
        }

        if(data.RecordatorioVencimientoId > 0){
            $('#chkRequiereAlertaVencimiento').prop('checked', true);
            $('#txtAlertaInformeVencimientoMeses').numberbox('setValue', data.RecordatorioVencimientoMeses);
            mostrarPanelConfigAlerta('chkRequiereAlertaVencimiento', 'panelAlertaVencimiento');
        }

        if (data.Versiones && data.Versiones.length > 0) {
            $('#dgVersiones').datagrid('loadData', data.Versiones);
            cargarControlesVersion();
        }
    }
}

function obtenerActasDocumentoVersion() {
    var parametros;
    parametros = {
        accion: 'OBTENER-ACTASDOCUMENTOVERSION',
        idDocumento: idDocumento
    };

    invocarControlador(parametros, function (data) {
        $('#dgComentariosActas').datagrid('loadData', data);
    });
}


function guardar() {
    var parametros = {
        accion: 'GRABAR',
        idEstudio: idEstudio,
        idDocumento: idDocumento,
        descripcion: $('#txtNombre').val(),
        idTipoDocumento: $('#ContentPlaceBody_cboTipoDocumento').combobox('getValue'),
        limitante: $('#chkLimitante').is(':checked'),
        requiereAlertaInactividad: $('#chkRequiereAlertaInactividad').is(':checked'),
        mesesAlertaInactividad: $('#txtAlertaInactividadMeses').numberbox('getValue'), 
        requiereAlertaInformeAvance: $('#chkRequiereAlertaInformeAvance').is(':checked'),
        mesesAlertaInformeAvance: $('#txtAlertaInformeAvanceMeses').numberbox('getValue')
    };
    invocarControlador_POST(parametros, guardarSucess);    
}

function guardarSucess(data) {
    if (data.result == 'OK') {
        location.href = 'DocumentoDatos.aspx?IdEstudio=' + idEstudio + '&IdDocumento=' + data.id;
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelar() {
    var idActa = getURLParam("IdActa");
    var parametroIdActa = idActa && idActa != -1 ? '&IdActa=' + idActa : '';
    location.href = getPaginaReturn() + '?IdEstudio=' + idEstudio + '&IdDocumento=' + idDocumento + parametroIdActa;
}

// ****** Versiones ***********
function nuevaVersion() {
    if (idDocumento != -1) {
        idVersion = -1;
        limpiarControlesVersion();

        $('#txtVersionDocumento').val($('#txtNombre').val());
        $("#txtVersionDocumento").attr("disabled", "disabled");

        $("#panelVersionEstado").show();

        $('#dlgVersion').dialog({
            title: 'Nueva Versión del Documento',
            width: 650,
            height: 400,
            closed: true,
            modal: true
        });
        $('#dlgVersion').dialog("open");
    }
}

function detalleVersion(rowData) {
    if (idDocumento != -1) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtVersionDocumento').val($('#txtNombre').val());
            $("#txtVersionDocumento").attr("disabled", true);
            $('#txtVersionFecha').datebox('setValue', versionSeleccionada.FechaToString);
            $('#txtVersionDescripcion').val(versionSeleccionada.Descripcion);
            $('#txtVersionArchivo').val(versionSeleccionada.Archivo);
            $("#chkVersionAprobadoANMAT").prop('checked', versionSeleccionada.AprobadoANMAT);
            $('#txtVersionFechaAprobadoANMAT').datebox('setValue', versionSeleccionada.FechaAprobadoANMATToString);

            $('#txtVersionFecha').focus();

            $("#panelVersionEstado").hide();

            $('#dlgVersion').dialog({
                title: 'Detalle Versión del Documento',
                width: 650,
                height: 310,
                closed: true,
                modal: true
            });
            $('#dlgVersion').dialog("open");
        }
    }
}

function limpiarControlesVersion() {
    $('#txtVersionDocumento').val('');
    $('#txtVersionFecha').datebox('setValue', '');
    $('#txtVersionDescripcion').val('');
    $('#txtVersionArchivo').val('');
    $("#chkVersionAprobadoANMAT").prop('checked', false);
    $('#txtVersionFechaAprobadoANMAT').datebox('setValue', '');

    $('#cboVersionEstado').combobox('setValue', '');
    $('#txtVersionFechaEstado').datebox('setValue', getFechaActual());
    obtenerPrimerEstadoVersion();

    $('#txtVersionFecha').focus();
}

function obtenerPrimerEstadoVersion() {

    var idTipoDocumento = $('#ContentPlaceBody_cboTipoDocumento').combobox('getValue');

    if (idTipoDocumento && idTipoDocumento != -1) {
        var url = 'handlers/DocumentoDatosHandler.ashx?accion=OBTENER-PRIMERESTADO&idTipoDocumento=' + idTipoDocumento;
        $('#cboVersionEstado').combobox({
            url: url,
            valueField: 'Id',
            textField: 'Descripcion',
            panelHeight: 100,
            onLoadSuccess: function () {
                var data = $('#cboVersionEstado').combobox("getData");
                if (data && data.length == 1)
                    $('#cboVersionEstado').combobox("select", data[0].Id)
            }
        });
    }
}

function cargarControlesVersion() {
    if (idVersion == -1) {
        var filas = $('#dgVersiones').datagrid('getRows') ;
        $('#dgVersiones').datagrid('selectRow', (filas.length - 1));
    }
    else {
        var rows = $('#dgVersiones').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].Id == idVersion)
                $('#dgVersiones').datagrid('selectRow', i);
        }
    }

    var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
    if (versionSeleccionada) {
        idVersion = versionSeleccionada.Id;
        $('#dgVersionEstados').datagrid('loadData', versionSeleccionada.Estados);
        $('#dgComentarios').datagrid('loadData', versionSeleccionada.Comentarios);
        $('#dgRecordatorios').datagrid('loadData', versionSeleccionada.Recordatorios);
        $('#dgParticipantes').datagrid('loadData', versionSeleccionada.Participantes);

        obtenerActasDocumentoVersion();
    }
}

function obtenerVersionParticipantes() {
    var profSeleccionados = '';

    var rows = $('#dgParticipantes').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Profesional)
            profSeleccionados = profSeleccionados + ';' + rows[i].Profesional.Id;
        else
            profSeleccionados = profSeleccionados + ';' + rows[i].Id;
    }

    return profSeleccionados;
}

function guardarVersion() {
    var parametros = {
        accion: 'GRABAR-VERSION',
        idEstudio: idEstudio,
        idDocumento: idDocumento,
        idVersion: idVersion,
        descripcion: formatearCaracteres($('#txtVersionDescripcion').val()),
        fecha: $('#txtVersionFecha').datebox('getValue'),
        aprobadoANMAT: $('#chkVersionAprobadoANMAT').is(':checked'),
        fechaAprobadoANMAT: $('#txtVersionFechaAprobadoANMAT').datebox('getValue'),
        nombreArchivo: '',
        fechaEstado: $('#txtVersionFechaEstado').datebox('getValue'),
        estado: $('#cboVersionEstado').combobox('getValue'),
        participantes: obtenerVersionParticipantes()
    };
    invocarControlador_POST(parametros, guardarVersionSucess);
}    

function guardarVersionSucess(data) {
    if (data.result == 'OK') {
        $('#dlgVersion').dialog('close');
        obtenerDatosDocumento()
        //location.href = 'DocumentoDatos.aspx?IdEstudio=' + idEstudio + '&IdDocumento=' + idDocumento + '&IdVersion=' + idVersion;
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarVersion() {
    limpiarControlesVersion();
    $('#dlgVersion').dialog('close')
}

function descargarArchivo() {
    if (idDocumento != null) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            if (versionSeleccionada.Archivo != null && versionSeleccionada.Archivo != '') {

                window.open('handlers/ArchivosHandler.ashx?idEstudio=' + idEstudio
                                            + '&idDocumento=' + idDocumento
                                            + '&idVersion=' + idVersion);
            }
        }
    }
}

function adjuntarArchivo() {
    if (idDocumento != -1) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtAdjuntarArchivoDocumento').val($('#txtNombre').val());
            $("#txtAdjuntarArchivoDocumento").attr("disabled", true);
            $('#txtAdjuntarArchivoVersion').val(versionSeleccionada.Descripcion);
            $("#txtAdjuntarArchivoVersion").attr("disabled", true);
            
            $('#dlgAdjuntarArchivo').dialog("open");
        }
    }
}

function guardarAdjuntarArchivo() {
    var parametros = {
        accion: 'GRABAR-VERSIONARCHIVOADJUNTO',
        idEstudio: idEstudio,
        idDocumento: idDocumento,
        idVersion: idVersion
    };

    invocarControlador_POST(parametros,
    function (data) {
        if (data.result == 'OK') {
            $('#dlgAdjuntarArchivo').dialog("close");
            location.href = 'DocumentoDatos.aspx?IdEstudio=' + idEstudio + '&IdDocumento=' + idDocumento + '&IdVersion=' + idVersion;
        } else {
            $.messager.alert('Error', data.message, 'error');
        }
    });
}

function cancelarAdjuntarArchivo() {
    $('#dlgAdjuntarArchivo').dialog("close");
}

// ****** Estados ***********
function nuevoEstado() {
    if (idDocumento != -1) {
        idEstado = -1;
        limpiarControlesEstado();
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtEstdoVersion').val(versionSeleccionada.Descripcion);
            $("#txtEstdoVersion").attr("disabled", "disabled");

            obtenerEstados();

            $('#dlgEstado').dialog('open').dialog('setTitle', 'Agregar Estado');
        }
    }
}

function detalleEstado() {
    if (idDocumento != -1 && idVersion != -1) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtEstdoVersion').val(versionSeleccionada.Descripcion);
            $("#txtEstdoVersion").attr("disabled", "disabled");
        }
        var estadoSeleccionado = $('#dgVersionEstados').datagrid('getSelected');
        if (estadoSeleccionado) {
            idEstado = estadoSeleccionado.Id;
            $('#txtEstadoFecha').datebox('setValue', estadoSeleccionado.FechaToString);
            $('#ContentPlaceBody_cboEstado').combobox('setValue', estadoSeleccionado.Estado.Id);
            if (estadoSeleccionado.ProfesionalAutor)
                $('#ContentPlaceBody_cboEstadoProfesionalAutor').combobox('setValue', estadoSeleccionado.ProfesionalAutor.Id);
            if (estadoSeleccionado.ProfesionalPresenta)
                $('#ContentPlaceBody_cboEstadoProfesionalPresenta').combobox('setValue', estadoSeleccionado.ProfesionalPresenta.Id);
            if (estadoSeleccionado.ProfesionalResponsable)
                $('#ContentPlaceBody_cboEstadoProfesionalResponsable').combobox('setValue', estadoSeleccionado.ProfesionalResponsable.Id);
            
            $('#dlgEstado').dialog('open').dialog('setTitle', 'Detalle Estado');
        }
    }
}

function obtenerUltimoEstado() {
    var rows = $('#dgVersionEstados').datagrid('getRows');
    $('#dgVersionEstados').datagrid('selectRow', rows.length - 1);
    var estado = $('#dgVersionEstados').datagrid('getSelected');

    return estado;
}

function eliminarUltimoEstado() {
    if (idDocumento != -1 && idVersion != -1) {

        var estadoSeleccionado = obtenerUltimoEstado();
        
        if (estadoSeleccionado) {
            idEstado = estadoSeleccionado.Id;
            $.messager.confirm('Confirmar', '¿Desea eliminar el estado' + estadoSeleccionado.Estado.Descripcion + '?', 
                function (r) {
                    if (r) {
                        var parametros = {
                            accion: 'ELIMINAR-VERSIONESTADO',
                            idEstudio: idEstudio,
                            idDocumento: idDocumento,
                            idVersion: idVersion,
                            idVersionEstado: idEstado
                        };
                        invocarControlador(parametros, guardarEstadoSucess);
                    }
                });
        }
    }
}

function obtenerEstados() {

    var url = 'handlers/DocumentoDatosHandler.ashx?accion=OBTENER-ESTADOS'
                                                + '&idDocumento=' + idDocumento
                                                + '&idVersion=' + idVersion;
    $('#ContentPlaceBody_cboEstado').combobox('reload', url);
}

function obtenerFlujos() {
    var url = 'handlers/DocumentoDatosHandler.ashx?accion=OBTENER-Flujos&idTipoDocumento=' + $('#ContentPlaceBody_cboTipoDocumento').combobox('getValue');
    $('#ContentPlaceBody_cboFlujos').combobox('reload', url);
}

function guardarEstado() {
    var parametros = {
        accion: 'GRABAR-VERSIONESTADO',
        idEstudio: idEstudio,
        idDocumento: idDocumento,
        idVersion: idVersion,
        idVersionEstado: idEstado,
        idEstado: $('#ContentPlaceBody_cboEstado').combobox('getValue'),
        fecha: $('#txtEstadoFecha').datebox('getValue'),
        idProfesionalAutor: $('#ContentPlaceBody_cboEstadoProfesionalAutor').combobox('getValue'),
        idProfesionalPresenta: $('#ContentPlaceBody_cboEstadoProfesionalPresenta').combobox('getValue'),
        idProfesionalResponsable: $('#ContentPlaceBody_cboEstadoProfesionalResponsable').combobox('getValue')        
    };
    invocarControlador_POST(parametros, guardarEstadoSucess);
}

function guardarEstadoSucess(data) {
    if (data.result == 'OK') {
        $('#dlgEstado').dialog('close');
        obtenerDatosDocumento();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarEstado() {
    limpiarControlesEstado();
    $('#dlgEstado').dialog('close');
}

function limpiarControlesEstado() {
    $('#txtEstdoVersion').val('');
    $('#txtEstadoFecha').datebox('setValue', getFechaActual());
    $('#ContentPlaceBody_cboEstado').combobox('setValue', '');
    $('#ContentPlaceBody_cboEstadoProfesionalAutor').combobox('setValue', '');
    $('#ContentPlaceBody_cboEstadoProfesionalPresenta').combobox('setValue', '');
    $('#ContentPlaceBody_cboEstadoProfesionalResponsable').combobox('setValue', '');
    
    $('#txtEstadoFecha').focus();
}


// ****** Comentarios ***********
function nuevoComentario() {
    if (idDocumento != -1) {
        idComentario = -1;
        limpiarControlesComentario();
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtComentarioVersion').val(versionSeleccionada.Descripcion);
            $("#txtComentarioVersion").attr("disabled", "disabled");

            $('#dlgComentario').dialog('open').dialog('setTitle', 'Comentario');
        }
    }
}

function detalleComentario() {
    if (idDocumento != -1 && idVersion != -1) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtComentarioVersion').val(versionSeleccionada.Descripcion);
            $("#txtComentarioVersion").attr("disabled", "disabled");
        }
        var comentarioSeleccionado = $('#dgComentarios').datagrid('getSelected');
        if (comentarioSeleccionado) {
            idComentario = comentarioSeleccionado.Id;
            $('#txtComentarioFecha').datebox('setValue', comentarioSeleccionado.FechaToString);
            if (comentarioSeleccionado.ProfesionalAutor)
                $('#ContentPlaceBody_cboComentarioProfesionalAutor').combobox('setValue', comentarioSeleccionado.ProfesionalAutor.Id);
            $('#txtComentarioObservaciones').val(comentarioSeleccionado.Observaciones);

            $('#dlgComentario').dialog('open').dialog('setTitle', 'Comentario');
        }
    }
}

function limpiarControlesComentario() {
    $('#txtComentarioVersion').val('');
    $('#txtComentarioFecha').datebox('setValue', getFechaActual());
    $('#ContentPlaceBody_cboComentarioProfesionalAutor').combobox('setValue', '');
    $('#txtComentarioObservaciones').val('');
    
    $('#txtComentarioFecha').focus();
}

function guardarComentario() {
    var parametros = {
        accion: 'GRABAR-VERSIONCOMENTARIO',
        idEstudio: idEstudio,
        idDocumento: idDocumento,
        idVersion: idVersion,
        idVersionComentario: idComentario,
        fecha: $('#txtComentarioFecha').datebox('getValue'),
        idProfesionalAutor: $('#ContentPlaceBody_cboComentarioProfesionalAutor').combobox('getValue'),
        observaciones: formatearCaracteres($('#txtComentarioObservaciones').val())
    };
    invocarControlador_POST(parametros, guardarComentarioSucess);
}

function guardarComentarioSucess(data) {
    if (data.result == 'OK') {
        $('#dlgComentario').dialog('close');
        obtenerDatosDocumento();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarComentario() {
    limpiarControlesComentario();
    $('#dlgComentario').dialog('close');
}


// ****** Recordatorios ***********
function nuevoRecordatorioDocumento() {
    if (idDocumento != -1) {
        idRecordatorio = -1;
        limpiarControlesRecordatorio();
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtRecordatorioVersion').val(versionSeleccionada.Descripcion);
            $("#txtRecordatorioVersion").attr("disabled", "disabled");

            $('#dlgRecordatorio').dialog('open').dialog('setTitle', 'Recordatorio');
        }
    }
}

function detalleRecordatorio() {
    if (idDocumento != -1 && idVersion != -1) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtRecordatorioVersion').val(versionSeleccionada.Descripcion);
            $("#txtRecordatorioVersion").attr("disabled", "disabled");
        }
        var recordatorioSeleccionado = $('#dgRecordatorios').datagrid('getSelected');
        if (recordatorioSeleccionado) {
            idRecordatorio = recordatorioSeleccionado.Id;
            $('#txtRecordatorioFecha').datebox('setValue', recordatorioSeleccionado.FechaToString);
            if (recordatorioSeleccionado.ProfesionalAutor)
                $('#ContentPlaceBody_cboRecordatorioProfesionalAutor').combobox('setValue', recordatorioSeleccionado.ProfesionalAutor.Id);
            $('#txtRecordatorioObservaciones').val(recordatorioSeleccionado.Observaciones);
            if (recordatorioSeleccionado.Pendiente)
                $("#chkRecordatorioPendiente").prop('checked', recordatorioSeleccionado.Pendiente);

            $('#dlgRecordatorio').dialog('open').dialog('setTitle', 'Recordatorio');
        }
    }
}

function limpiarControlesRecordatorio() {
    $('#txtRecordatorioVersion').val('');
    $('#txtRecordatorioFecha').datebox('setValue', getFechaActual());
    $('#ContentPlaceBody_cboRecordatorioProfesionalAutor').combobox('setValue', '');
    $('#txtRecordatorioObservaciones').val('');
    $("#chkRecordatorioPendiente").prop('checked', true);    

    $('#txtRecordatorioFecha').focus();
}

function guardarVersionRecordatorio() {
    var parametros = {
        accion: 'GRABAR-VERSIONRECORDATORIO',
        idEstudio: idEstudio,
        idDocumento: idDocumento,
        idVersion: idVersion,
        idVersionRecordatorio: idRecordatorio,
        fecha: $('#txtRecordatorioFecha').datebox('getValue'),
        idProfesionalAutor: $('#ContentPlaceBody_cboRecordatorioProfesionalAutor').combobox('getValue'),
        observaciones: formatearCaracteres($('#txtRecordatorioObservaciones').val()),
        pendiente: $('#chkRecordatorioPendiente').is(':checked')
    };
    invocarControlador_POST(parametros, guardarVersionRecordatorioSucess);
}

function guardarVersionRecordatorioSucess(data) {
    if (data.result == 'OK') {
        $('#dlgRecordatorio').dialog('close');
        obtenerDatosDocumento();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarVersionRecordatorio() {
    limpiarControlesRecordatorio();
    $('#dlgRecordatorio').dialog('close');
}

// ****** Actas ***********
function seleccionarActa() {

    if (idDocumento != -1 && idVersion != -1) {
        var versionSeleccionada = $('#dgVersiones').datagrid('getSelected');
        if (versionSeleccionada) {
            $('#txtActaDocumentoVersion').val(versionSeleccionada.Descripcion);
            $("#txtActaDocumentoVersion").attr("disabled", "disabled");
        }

        var parametros;
        parametros = {
            accion: 'LISTAR-ACTAS',
            cerrada: false,
            orden: 2
        };
        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros,
                            function (data) {
                                $('#dgSeleccionActa').datagrid('loadData', data);
                                $('#dlgSeleccionActa').dialog('open').dialog('setTitle', 'Agregar Documento en Acta');
                            });
    }
}

function seleccionActaAgregar() {
    var actaSeleccionada = $('#dgSeleccionActa').datagrid('getSelected');
    if (actaSeleccionada) {

        var parametros = {
            accion: 'GRABAR-ACTADOCUMENTO',
            idActa: actaSeleccionada.Id,
            idActaDocumento: -1,
            idDocumento: idDocumento,
            idDocumentoVersion: idVersion,
            comentario: '',
            idResponsableComite: -1,
            imprimirCarta: true
        };
        controladorAJAX_POST('handlers/ActasHandler.ashx', parametros, seleccionActaAgregarSuccess);        
    }
}

function seleccionActaAgregarSuccess(data) {
    if (data.result == 'OK') {
        $('#dlgSeleccionActa').dialog('close');
        //obtenerActasDocumentoVersion();
        obtenerDatosDocumento();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function seleccionActaCancelar() {
    $('#dlgSeleccionActa').dialog('close');
}

function ingresarComentarioActa() {

    if (idDocumento != -1 && idVersion != -1) {
        var actaSeleccionada = $('#dgComentariosActas').datagrid('getSelected');
        if (actaSeleccionada) {

            $('#txtActaDocumentoControlComentario').val(actaSeleccionada.Comentario);

            $('#dlgActaDocumentoComentarioControl').dialog('open').dialog('setTitle', 'Comentario del Acta');
        }
    }
}

function guardarActaDocumentoComentario() {

    var documentosJSON = new Array();
    var actaDocumento = $('#dgComentariosActas').datagrid('getSelected');
    var index = 0;
    if (actaDocumento) {
        
        var estadoSeleccionado = obtenerUltimoEstado();
        
        documentosJSON[index] = new Object();
        documentosJSON[index].idActaDocumento = actaDocumento.IdActaDocumento;
        documentosJSON[index].idDocumento = idDocumento;
        documentosJSON[index].idDocumentoVersion = idVersion;
        documentosJSON[index].comentario = formatearTextoASCII($('#txtActaDocumentoControlComentario').val());
        documentosJSON[index].idEstadoDocumento = estadoSeleccionado.Id;
        documentosJSON[index].actualizarEstadoFinal = false;
        
        var parametros = {
            accion: 'GRABAR-ACTADOCUMENTOCOMENTARIOESTADO',
            idActa: actaDocumento.Id,
            documentos: JSON.stringify(documentosJSON)
        };

        controladorAJAX_POST('handlers/ActasHandler.ashx', parametros, 
            function (data) {
                if (data.result == 'OK') {
                    obtenerActasDocumentoVersion();
                    $('#dlgActaDocumentoComentarioControl').dialog('close');               
                } else {
                    $.messager.alert('Error', data.message, 'error');
                }
            });
    }
}

function cancelarActaDocumentoComentario() {
    $('#dlgActaDocumentoComentarioControl').dialog('close');
}

// ****** Participantes ***********
function seleccionarParticipantes() {
    if (idVersion != -1) {
        var parametros;
        parametros = {
            accion: 'LISTAR-INVESTIGADORES-ESTUDIO',
            idEstudio: idEstudio
        };
        controladorAJAX_GET('handlers/EstudioCargaDatosHandler.ashx', parametros,
        function (data) {
            if (data) {
                $('#dgSeleccionParticipantes').datagrid('loadData', data);
                $('#dlgSeleccionParticipantes').dialog('open').dialog('setTitle', 'Seleccionar Investigadores');
            }
            else {
                $.messager.alert('Error', data.message, 'info');
            }
        });
    }
    else {
        $.messager.alert('Error', 'Debe ingresar una versión', 'error');
    }
}

function aceptarSeleccionParticipantes() {
    var participantesSeleccionados = $('#dgSeleccionParticipantes').datagrid('getSelections');
    if (participantesSeleccionados) {

        var parametros = {
            accion: 'GRABAR-VERSIONPARTICIPANTES',
            idDocumento: idDocumento,
            idVersion: idVersion,
            participantes: obtenerParticipantesSeleccionados()
        };
        invocarControlador_POST(parametros,
            function (data) {
                if (data.result == 'OK') {
                    $('#dlgSeleccionParticipantes').dialog('close')
                    obtenerDatosDocumento();
                } else {
                    $.messager.alert('Error', data.message, 'error');
            }
        });

    }    
}


function obtenerParticipantesSeleccionados() {
    var profSeleccionados = '';

    var rows = $('#dgSeleccionParticipantes').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Profesional)
            profSeleccionados = profSeleccionados + ';' + rows[i].Profesional.Id;
        else
            profSeleccionados = profSeleccionados + ';' + rows[i].Id;
    }

    return profSeleccionados;
}

function cancelarSeleccionParticipantes() {
    $('#dlgSeleccionParticipantes').dialog('close');
}

function quitarParticipante() {

}

function setearDefaultRecordatorios(idTipoDocumento) {

    limpiarRecordatoriosAlertas();
    if (idTipoDocumento > 0) {
        var parametros;
        parametros = {
            accion: 'OBTENER',
            id: idTipoDocumento
        };

        controladorAJAX_GET('handlers/TiposDocumentoHandler.ashx', parametros,
        function (data) {
            if (data) {
                if (data.TiposRecordatorio) {
                    for (var i = 0; i < data.TiposRecordatorio.length; i++) {
                        switch (data.TiposRecordatorio[i].IdTipoRecordatorio) {
                            case ID_INFROME_AVANCE:
                                $("#chkRequiereAlertaInformeAvance").prop('checked', true);
                                $('#txtAlertaInformeAvenceMeses').numberbox('setValue', data.TiposRecordatorio[i].Meses);
                                mostrarPanelConfigAlerta('chkRequiereAlertaInformeAvance', 'panelAlertaInformeAvance');
                                break;

                            case ID_INACTIVIDAD:
                                $("#chkRequiereAlertaInactividad").prop('checked', true);
                                $('#txtAlertaInactividadMeses').numberbox('setValue', data.TiposRecordatorio[i].Meses);
                                mostrarPanelConfigAlerta('chkRequiereAlertaInactividad', 'panelAlertaInactividad');
                                break;

                            case ID_VENCIMIENTO:
                                $("#chkRequiereAlertaVencimiento").prop('checked', true);
                                $('#txtAlertaVencimientoMeses').numberbox('setValue', data.TiposRecordatorio[i].Meses);
                                mostrarPanelConfigAlerta('chkRequiereAlertaVencimiento', 'panelAlertaVencimiento');
                                break;
                        }
                    }
                }
            }
        });
    }
}

function mostrarPanelConfigAlerta(checkNombre, panelNombre) {
    if ($('#' + checkNombre).is(':checked')) {
        $('#' + panelNombre).css("display", "inline-block");
    } else {
        $('#' + panelNombre).css("display", "none");
    }
}

function configAlertaInformeAvance() {

    var parametros;
    parametros = {
        id: 3,
        accion: 'OBTENER'
    };

    controladorAJAX_GET('handlers/TiposRecordatorioHandler.ashx', parametros,
    function (tipoRecordatorio) {
        if (tipoRecordatorio) {
            var dias = parseInt($('#txtAlertaInformeAvanceMeses').numberbox('getValue')) * 30;
            var fechaActivacion = sumarDiasFecha(parserDateBox(getFechaActual()), dias);
            mostrarConfigRecordatorio('Recordatorio y alerta por Informe de Avance', tipoRecordatorio, fechaActivacion, -1);
        }
    });

}


function configAlertaInactividadDocumento() {
    var parametros;
    parametros = {
        id: 4,
        accion: 'OBTENER'
    };

    controladorAJAX_GET('handlers/TiposRecordatorioHandler.ashx', parametros,
    function (tipoRecordatorio) {
        if (tipoRecordatorio) {
            var dias = parseInt($('#txtAlertaInactividadMeses').numberbox('getValue')) * 30;
            var fechaActivacion = sumarDiasFecha(parserDateBox(getFechaActual()), dias);
            mostrarConfigRecordatorio('Recordatorio y alerta por Inactividad de Documento', tipoRecordatorio, fechaActivacion, -1);
        }
    });
}

function configAlertaVencimiento() {
    var parametros;
    parametros = {
        id: 5,
        accion: 'OBTENER'
    };

    controladorAJAX_GET('handlers/TiposRecordatorioHandler.ashx', parametros,
    function (tipoRecordatorio) {
        if (tipoRecordatorio) {
            var dias = parseInt($('#txtAlertaVencimientoMeses').numberbox('getValue')) * 30;
            var fechaActivacion = sumarDiasFecha(parserDateBox(getFechaActual()), dias);
            mostrarConfigRecordatorio('Recordatorio y alerta por Vencimiento', tipoRecordatorio, fechaActivacion, -1);
        }
    });
}

function mostrarConfigRecordatorio(tituloRecordatorio, tipoRecordatorio, fechaActivacion, idRecordatorio) {

    var estudio = new Object();
    estudio.Id = idEstudio;
    estudio.NombreEstudioListados = nombreEstudio;

    var documentos = new Array();
    var doc = new Object();
    doc.Id = idDocumento;
    doc.Descripcion = $('#txtNombre').val();
    documentos.push(doc);

    if (idRecordatorio == -1)
        nuevoRecordatorioAlertaDocumento(tipoRecordatorio, estudio, documentos, fechaActivacion);
    //    else
    //        mostrarRecordatorioEstudio(tipoRecordatorio, estudio, fechaActivacion, idRecordatorio);

    $('#dlgRecordatorioAlerta').dialog({
        title: tituloRecordatorio,
        width: 650,
        height: 550,
        closed: true,
        modal: true
    });
    $('#dlgRecordatorioAlerta').dialog('center').dialog("open");
}

function actualizarDatosRecordatorioSuccess(data) {
    if (data.result == 'OK') {
        guardar();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function guardarRecordatorioAlerta() {
    guardarRecordatorio();
    $('#dlgRecordatorioAlerta').dialog("close");
}

function cancelarRecordatorioAlerta() {
    $('#dlgRecordatorioAlerta').dialog("close");
}

