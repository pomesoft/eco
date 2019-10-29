var idEstudio;
var idActa;
var idActaDocumento;
var idNotaTratada;

var fila_dgActaDocumentos = -1;
var fila_dgActaDocumentos_GrabaDocumentos = -1;

var seGraboAlgunDocumento = false;
var seGraboInfoActa = false;
var lastIndex;
var estadosEstudio;
var rolesComite;
var lastIndexCartaRespuesta;

var lastIndexParticipantes;

var recargarDocumentos;

var controlActualizaTextoPlantilla;

$(document).ready(function () {
    idEstudio = -1;
    recargarDocumentos = false;

    configurarPaneles();
    incializarEventosControles();
    configurarGrillas();

    cargarRolesComite();
});

function incializarEventosControles() {
    $('#txtActaFecha').datebox({
        onSelect: function () {
            var fechaLarga = formatterDateBoxLarge($('#txtActaFecha').datebox('getValue'));
            if ($('#txtActaDescripcion').textbox('getValue') == '') {
                $('#txtActaDescripcion').textbox('setValue','ACTA DEL ' + fechaLarga);
            }
        }
    });
        
}

function volver() {
    var parametroIdEstudio = idEstudio != -1 ? '&IdEstudio=' + idEstudio : '';
    location.href = getPaginaReturn() + '?IdActa=' + idActa + parametroIdEstudio;
}

function cargarRolesComite() {
    var parametros;
    parametros = {
        accion: 'LISTAR-ROLESCOMITE'
    };

    controladorAJAX_GET('handlers/ProfesionalesHandler.ashx', parametros,
    function (data) {
        if (data) rolesComite = data;

        obtenerDatosActa();
    });
}

function configurarGrillas() {

    $('#dgActaDocumentos').datagrid({
        title: 'Documentos tratados en la reunión',
        width: 980,
        height: 680,
        autoRowHeight: false,
        collapsible: true,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'NombreEstudio',
        sortOrder: 'asc',
        remoteSort: true,
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Orden', title: 'Orden', width: 30, hidden: true },
                { field: 'OrdenEstudio', title: '', width: 30, hidden: true },
                { field: 'OrdenDocumento', title: '', width: 30, hidden: true },
                { field: 'NombreEstudio', title: 'Estudio', width: 250 },
                { field: 'NombreDocumento', title: 'Documento', width: 420 },
                { field: 'VersionDocumento', title: 'Versión', width: 150 },
                { field: 'IdEstadoActual', title: 'Estado', width: 110,
                    formatter: function (value, row, index) {
                        for (var i = 0; i < row.EstadosDocumento.length; i++) {
                            if (row.EstadosDocumento[i].Id == value) return row.EstadosDocumento[i].Descripcion;
                        }
                        return value;
                    }
                }
	  ]],        
        toolbar: [{
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: agregarActaDocumento
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarActaDocumento
        }, '-', {
            text: 'Setear Estado Final',
            iconCls: 'icon-edit',
            handler: cambiarEstadosActaDocumento
        }, '-', {
            text: 'Guardar',
            iconCls: 'icon-save',
            handler: grabarActaDocumentos
        }, '-', {
            text: 'Reordenar',
            iconCls: 'icon-sort',
            handler: ordenarDocumentos
        }, '-', {
            text: 'Expandir',
            iconCls: 'icon-expand',
            handler: expandirFilasDocumentos
        }, '-', {
            text: 'Contraer',
            iconCls: 'icon-collapse',
            handler: contraerFilasDocumentos
        }, '-', {
            text: 'Datos del Estudio y Carta de Respuesta',
            iconCls: 'icon-edit',
            handler: actualizarEstudio
        }],
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {

            var responsable = '';
            if (rowData.ResponsableComite)
                responsable = rowData.ResponsableComite.NombreCompleto;

            var datosAMostrar = '<p>'
            datosAMostrar += 'Documento: <span><strong>' + rowData.NombreDocumento + '</strong></span><br />'; /// <reference path="../css/default/" />
            if (rowData.VersionDocumento != '') {
                datosAMostrar += 'Versión del documento: <span><strong>' + rowData.VersionDocumento + '</strong></span>   Fecha: <span><strong>' + rowData.VersionFecha + '</strong></span><br />';
            }
            datosAMostrar += 'Estado del documento: ' + crearComboEstados(rowIndex.toString(), rowData);
            datosAMostrar += '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;';
            datosAMostrar += 'Imprimir en carta de respuesta ' + crearCheckImprimirCarta(rowIndex.toString(), rowData.ImprimirCarta);
            datosAMostrar += '</p>';
       
            datosAMostrar += '<div id="panelDocumentoComentario' + rowIndex.toString() + '">';
            datosAMostrar += '<textarea id="txtComentario' + rowIndex.toString() + '" class="campoTextAreaPanel" cols="20" rows="7">'
                           + formatearTextoASCII(rowData.ComentarioDocumento) + '</textarea>';
            datosAMostrar += '</div>';

            return datosAMostrar;
        },
        onClickRow: function (rowIndex, rowData) {
            $('#dgActaDocumentos').datagrid('selectRow', rowIndex);
            var rows = $('#dgNotasTratadas').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].IdEstudio == rowData.IdEstudio)
                    $('#dgNotasTratadas').datagrid('selectRow', i);
            }
        },
        onSelect: function (rowIndex, rowData) {
            idActaDocumento = rowData.Id;
            fila_dgActaDocumentos = rowIndex;
            $('#dgActaDocumentos').datagrid('expandRow', rowIndex);
        },
        onExpandRow: function (rowIndex, rowData) {

            configurarPanelesGrillaComentarioDocumentos('panelDocumentoComentario' + rowIndex.toString(), 'txtComentario' + rowIndex.toString());
            $('#dgActaDocumentos').datagrid('fixDetailRowHeight', rowIndex);
        }
    });

    $('#dgActaDocumentos').datagrid('enableFilter');

    $('#dgNotasTratadas').datagrid({
        title: 'Notas tratadas en la reunión',
        width: 980,
        height: 680,
        autoRowHeight: false,
        collapsible: true,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: true,
        columns: [[
                { field: 'ActaOrden', title: 'Orden', width: 60, hidden: false },
                { field: 'NombreEstudio', title: 'Estudio', width: 250 },
                { field: 'Id', title: 'Número', width: 60 },
                { field: 'Descripcion', title: 'Nota', width: 200, hidden: true },
                { field: 'ActaImprimeAlFinal', title: 'Imprimir', width: 610,
                    formatter: function (value, row, index) {
                        return formatearImprimeNotas(row.ActaImprimeAlFinal);
                    }
                }
	  ]],        
        toolbar: [{
            text: 'Agregar nota existente',
            iconCls: 'icon-add',
            handler: agregarNotaTratada
        }, '-', {
            text: 'Nueva nota',
            iconCls: 'icon-add',
            handler: agregarNuevaNotaTratada
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleNuevaNotaTratada
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarNotaTratada
        }, '-', {
            text: 'Guardar Notas',
            iconCls: 'icon-save',
            handler: grabarActaNotasTratadas
        }, '-', {
            text: 'Expandir',
            iconCls: 'icon-expand',
            handler: expandirFilasNotas
        }, '-', {
            text: 'Contraer',
            iconCls: 'icon-collapse',
            handler: contraerFilasNotas
        }],
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {
            //'<p>Estudio: <span><strong>' + rowData.NombreEstudioCompleto + '</strong></span></p>' +                                
            var datosAMostrar = '<p>Imprimir: <span>' + formatearImprimeNotas(rowData.ActaImprimeAlFinal); +'</span></p>';

            datosAMostrar += '<div id="panelNotaComentario' + rowIndex.toString() + '">';
            datosAMostrar += '<textarea id="txtNotaComentario' + rowIndex.toString() + '" class="campoTextAreaPanel" cols="30" rows="10">'
                           + formatearTextoASCII(rowData.Texto) + '</textarea>';
            datosAMostrar += '</div>';

            return datosAMostrar;
        },
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                idNotaTratada = rowData.Id;
                $('#dgNotasTratadas').datagrid('expandRow', rowIndex);
            }
        },
        onExpandRow: function (rowIndex, rowData) {
            
            configurarPanelesGrillaComentarioDocumentos('panelNotaComentario' + rowIndex.toString(), 'txtNotaComentario' + rowIndex.toString());
            $('#dgNotasTratadas').datagrid('fixDetailRowHeight', rowIndex);
        }
    });

    $('#dgNotasTratadas').datagrid('enableFilter');

    $('#dgActaParticipantes').datagrid({
        title: '',
        width: 980,
        height: 300,
        showHeader: false,
        autoRowHeight: false,
        collapsible: true,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,

        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
            { field: 'Id', title: 'Código', width: 60, hidden: true },
            { field: 'Profesional', title: 'Participantes', width: 270,
                formatter: function (value, row, index) {
                    if (row.Profesional) return row.Profesional.NombreCompleto;
                    else return value;
                }
            },
            { field: 'IdRolComite', title: 'Rol en Comité', width: 200,
                formatter: function (value) {
                    for (var i = 0; i < rolesComite.length; i++) {
                        if (rolesComite[i].Id == value) return rolesComite[i].Descripcion;
                    }
                    return value;
                },
                editor: {
                    type: 'combobox',
                    options: {
                        valueField: 'Id',
                        textField: 'Descripcion',
                        data: rolesComite
                    }
                }
            }
	  ]],
        toolbar: [{
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: actaSeleccionarParticipantes
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarParticipante
        }],
        onClickRow: function (rowIndex, rowData) {
            if (rowData.IdRolComite != -1) {
                if (lastIndex == rowIndex) {
                    $('#dgActaParticipantes').datagrid('endEdit', lastIndexParticipantes);
                    lastIndexParticipantes = -1;
                }
                else {
                    if (lastIndexParticipantes != rowIndex) {
                        $('#dgActaParticipantes').datagrid('endEdit', lastIndexParticipantes);
                        $('#dgActaParticipantes').datagrid('beginEdit', rowIndex);
                    }
                }
                lastIndexParticipantes = rowIndex;
            }
        }
    });

    $('#dgActaSeleccionParticipantes').datagrid({
        title: '',
        width: 500,
        height: 200,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: false,
        columns: [[
                { field: 'Check', width: 40, checkbox: true },
                { field: 'NombreCompleto', title: 'Profesionales', width: 270 },
                { field: 'RolComite', title: 'Rol en Comité', width: 270,
                    formatter: function (value, row, index) {
                        if (row.RolComite) return row.RolComite.Descripcion;
                        else return value;
                    }
                }
	  ]]
    });

    $('#dgActaSeleccionCartaRespuesta').datagrid({
        title: '',
        width: 850,
        height: 300,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: true,
        columns: [[
		    { field: 'IdEstudio', title: 'IdEstudio', hidden: true },
		    { field: 'NombreCompleto', title: 'Estudio', width: 200 },
		    { field: 'EstadoDescripcion', title: 'Estado', width: 170 },
                { field: 'CartaRespuestaModeloDescripcion', title: 'Modelo Carta', width: 450 }
        ]],
        onSelect: function (rowIndex, rowData) {
            $("#btnExportarCartaRespuestaPDF").attr({
                href: 'Impresion.aspx?Plantilla=PLANTILLA_CARTARESPUESTA&IdEstudio=' + rowData.IdEstudio + '&IdActa=' + idActa,
                target: '_blank'
            });
            $("#btnExportarCartaRespuestaWord").attr({
                href: 'Impresion.aspx?Plantilla=PLANTILLA_CARTARESPUESTA&IdEstudio=' + rowData.IdEstudio + '&IdActa=' + idActa + '&ExportTo=2'
            });
        },
        onLoadSuccess: function (data) {
            if (data && data.rows != null && data.rows.length > 0) {
                $('#dgActaSeleccionCartaRespuesta').datagrid('selectRow', 0);
            }
        },
        onClickRow: function (rowIndex, rowData) {

        }
    });
}


function formatearImprimeNotas(imprimeAlFinal) {
    if (imprimeAlFinal)
        return 'A CONTINUACIÓN DE DOCUMENTOS TRATADOS';
    else
        return 'ANTES DE DOCUMENTOS TRATADOS';
}

function crearComboEstados(idItem, rowData) {
    var sestados = '<select id="cboEstadoDocumento' + idItem + '">';
    $(rowData.EstadosDocumento).each(function () {
        var itemSeleccionado = (rowData.IdEstadoActual == this.Id) ? ' selected="selected" ' : '';
        sestados += '<option value="' + this.Id + '" ' + itemSeleccionado + '>' + this.Descripcion + '</option>';
    });
    sestados += '</select>';
    return sestados;
}

function crearCheckImprimirCarta(idItem, valor) {
    return '<input type="checkbox" id="chkImprimirCarta' + idItem + '" name="ImprimirCarta" ' + (valor ? ' checked="checked" ' : '') + ' />';
}

function configurarPaneles() {
    $("#panelInfoActa").panel({
        width: 980,
        height: 70,
        title: 'Acta'
    });
    $('#panelTextos').panel('collapse', true);

    $('#panelComentarioInicialFijo').panel({
        width: 980,
        height: 250,
        title: 'Introducción',
        collapsible: true,
        tools: [{
            iconCls: 'icon-reload',
            handler: function () {
                obrenerComentarioInicioFijo()
            }
        }],
        onExpand: function () {
                       
        }
    });
    $('#panelComentarioInicialFijo').panel('collapse', true);
    
    $('#panelComentarioInicial').panel({
        width: 980,
        height: 350,
        title: 'Comentario Inicial',
        collapsible: true,
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtActaComentarioInicial');
            }
        }],
        onExpand: function () {

        }
    });
    $('#panelComentarioInicial').panel('collapse', true);
    
    $('#panelComentarioFinal').panel({
        width: 980,
        height: 350,
        title: 'Comentario Final',
        collapsible: true,
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtActaComentarioFinal');
            }
        }],
        onExpand: function () {
            
        }
    });    
    $('#panelComentarioFinal').panel('collapse', true);
    
    $('#panelParticipantes').panel({
        width: 980,
        height: 300,
        title: 'Participantes',
        collapsible: true
    });
    $('#panelParticipantes').panel('collapse', true);

    $('#panelActaDocumentoComentario').panel({
        width: 980,
        height: 200,
        title: 'Comentario documento',
        collapsible: true,
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtActaDocumentoComentario');
            }
        }]
    });
    $('#panelNuevaNotaTexto').panel({
        width: 588,
        height: 220,
        title: 'Nota',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtNuevaNotaTexto');
            }
        }]
    });
}

function configurarPanelesGrillaComentarioDocumentos(controlPanel, controlText) {
    $('#' + controlPanel).panel({
        width: 920,
        height: 190,
        title: 'Comentario',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla(controlText);
            }
        }]
    });
}


function configurarTextEditor(controlTextEditor) {
    // style="height: 150px; padding: 10px;"
    $('#' + controlTextEditor).texteditor({
        width: 910,
        height: 250,
        title: 'Comentario',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla(controlTextEditor);
            }
        }]
    });
}

function obtenerDatosActa() {
    idEstudio = getURLParam("IdEstudio");
    idActa = getURLParam("IdActa");

    if (!idEstudio) idEstudio = -1;
    if (!idActa) idActa = -1;

    if (idActa != -1) {
    
        var parametros;
        parametros = {
            accion: 'OBTENER',
            idActa: idActa
        };
        invocarControlador(parametros, cargarControlesActa);
    }
}

function invocarControlador(params, funcionOK) {
    controladorAJAX_GET('handlers/ActasHandler.ashx', params, funcionOK);
}


function invocarControladorPOST(params, funcionOK) {
    controladorAJAX_POST('handlers/ActasHandler.ashx', params, funcionOK);
}

function limpiarControlesActa() {
    $('#txtActaFecha').datebox('setValue', '');
    $('#txtActaHora').timespinner('setValue', '');
    $('#txtActaDescripcion').textbox('setValue', '');

    $('#txtActaComentarioInicialFijo').val('');
    $('#txtActaComentarioInicial').val('');
    $('#txtActaComentarioFinal').val('');

    $("#chkCerrada").prop('checked', false);

}

function cargarControlesActa(data) {
    limpiarControlesActa();
    if (data && data.Id) {

        recargarDocumentos = false;
        seGraboInfoActa = false;

        idActa = data.Id;
        $('#txtActaFecha').datebox('setValue', data.FechaToString);
        $('#txtActaHora').timespinner('setValue', data.Hora);
        $('#txtActaDescripcion').textbox('setValue', data.Descripcion);
        $("#chkCerrada").prop('checked', data.Cerrada);

        $('#txtActaComentarioInicialFijo').val(formatearTextoASCII(data.ComentarioInicialFijo));
        $('#txtActaComentarioInicial').val(formatearTextoASCII(data.ComentarioInicial));
        $('#txtActaComentarioFinal').val(formatearTextoASCII(data.ComentarioFinal));

        $('#dgActaParticipantes').datagrid('loadData', data.Participantes);
        $('#dgNotasTratadas').datagrid('loadData', data.Notas);

        $('#dgActaDocumentos').datagrid('loadData', data.Documentos);
        if (fila_dgActaDocumentos_GrabaDocumentos && fila_dgActaDocumentos_GrabaDocumentos != -1)
            $('#dgActaDocumentos').datagrid('selectRow', fila_dgActaDocumentos_GrabaDocumentos);

        $("#btnActaImprimirActa").attr({
            href: 'Impresion.aspx?Plantilla=PLANTILLA_ACTA&ExportTo=3&IdEstudio=' + idEstudio + '&IdActa=' + idActa,
            target: '_blank'
        });

        $("#btnActaExportarWord").attr({
            href: 'Impresion.aspx?Plantilla=PLANTILLA_ACTA&IdEstudio=' + idEstudio + '&IdActa=' + idActa + '&ExportTo=2'
        });

        $('#txtActaFecha').focus();

        actaSeleccionarEstudioCartaRespuesta();
    }
    else
        if (data.result == 'Error') {
            $.messager.alert('Error', data.message, 'error');
        }
}

function guardarActa() {

    if (lastIndexParticipantes != -1)
        $('#dgActaParticipantes').datagrid('endEdit', lastIndexParticipantes);

    var parametros = {
        accion: 'GRABAR-ACTA',
        idActa: idActa,
        descripion: $('#txtActaDescripcion').textbox('getValue'),
        fecha: $('#txtActaFecha').datebox('getValue'),
        hora: $('#txtActaHora').timespinner('getValue'),
        comentarioInicialFijo: $('#txtActaComentarioInicialFijo').val(),
        comentarioInicial: $('#txtActaComentarioInicial').val(),
        comentarioFinal: $('#txtActaComentarioFinal').val(),
        cerrada: $('#chkCerrada').is(':checked'),
        participantes: obtenerParticipantes()
    };
    invocarControladorPOST(parametros, guardarActaSucess);

}

function guardarActaSucess(data) {
    if (data.result == 'OK') {
        seGraboInfoActa = true;
        if (idActa == -1) {
            $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
            location.href = 'ActaDatos.aspx?IdEstudio=' + idEstudio + '&IdActa=' + data.id;
        }
        else {
            grabarActaDocumentos();
        }
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function guardarDatosActaSucess(data) {
    if (data.result == 'OK') {
        obtenerDatosActa();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarActa() {
    $('#dlgActa').dialog('close');
}


function obrenerComentarioInicioFijo() {
    if (idActa != -1) {
        var parametros;
        parametros = {
            accion: 'OBTENER-COMENTARIOINICIOFIJO',
            idActa: idActa
        };
        invocarControlador(parametros,
        function (data) {
            if (data) {
                $('#txtActaComentarioInicialFijo').val(formatearTextoASCII(data));
            }
        });
    }
}

/***********************************************/
/**************** DOCUMENTOS *******************/

function cargarCboEstudio() {
    $('#cboActaEstudio').combobox({
        url: 'handlers/EstudioCargaDatosHandler.ashx?accion=LISTAR-ESTUDIOS',
        valueField: 'Id',
        textField: 'NombreCompleto',
        panelHeight: 350,
        onSelect: function (rec) {
            if (rec) cargarCboDocumento(rec.Id);
        },
        onLoadSuccess: function () {
            cargarCboResponsable();
        }
    });
}

function cargarCboResponsable() {
    $('#cboActaEstudio').combobox({
        url: 'handlers/ProfesionalesHandler.ashx?accion=LISTAR-MIEMBOSCOMITE',
        valueField: 'Id',
        textField: 'NombreCompleto',
        panelHeight: 'auto',
        onSelect: function (rec) {
            
        },
        onLoadSuccess: function () {

        }
    });
}

function cargarCboDocumento(id) {
    $('#cboActaDocumento').combobox({
        url: 'handlers/DocumentoDatosHandler.ashx?accion=LISTAR-DOCUMENTOS&idEstudio=' + id,
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 350,
        onSelect: function (rec) {
            if (rec) cargarCboDocumentoVersion(rec.Id);
        }
    });
}

function cargarCboDocumentoVersion(id) {
    $('#cboActaDocumentoVersion').combobox({
        url: 'handlers/DocumentoDatosHandler.ashx?accion=LISTAR-DOCUMENTOVERSIONES&idDocumento=' + id,
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 350,
        onLoadSuccess: function () {
            var data = $('#cboActaDocumentoVersion').combobox("getData");
            if (data && data.length == 1)
                $('#cboActaDocumentoVersion').combobox("select", data[0].Id)
        }
    });
}

function agregarActaDocumento() {
    if (idActa != -1) {
        cargarCboEstudio();
        idActaDocumento = -1;
        fila_dgActaDocumentos = -1;
        limpiarControlesActaDocumento();
        $('#dlgActaDocumento').dialog('open').dialog('setTitle', 'Documento a tratar');
    }
}

function limpiarControlesActaDocumento() {
    $('#cboActaEstudio').combobox('setValue', '');
    $('#cboActaDocumento').combobox('setValue', '');
    $('#cboActaDocumentoVersion').combobox('setValue', '');
    $('#cboActaDocumentoResponsableComite').combobox('setValue', '');
    $('#txtActaDocumentoComentario').val('')
}

function grabarActaDocumentos() {
    if (idActa != -1) {
        procesarActaDocumentos(false);
    }
}

function procesarActaDocumentos(actualizarEstadoFinal) {
    var documentosJSON = new Array();
    var documentos = $('#dgActaDocumentos').datagrid('getRows');

    var index = 0;
    if (documentos.length > 0) {
        for (var i = 0; i < documentos.length; i++) {
            var idEstadoDocumento = -1;
            if (!actualizarEstadoFinal) {
                idEstadoDocumento = $('#cboEstadoDocumento' + i).val();
            }

            var grabarFila = false;
            var setearSaltoLinea = false;
            var comentarioDocumento = formatearTextoASCII($('#txtComentario' + i).val());

            if (documentos[i].ComentarioDocumento != comentarioDocumento
                || documentos[i].IdEstadoActual != idEstadoDocumento
                || documentos[i].ImprimirCarta != $('#chkImprimirCarta' + i).is(':checked')) {

                grabarFila = true;
            }

            if (grabarFila || actualizarEstadoFinal) {
                documentosJSON[index] = new Object();
                documentosJSON[index].idActaDocumento = documentos[i].Id;
                documentosJSON[index].idDocumento = documentos[i].IdDocumento;
                documentosJSON[index].idDocumentoVersion = documentos[i].IdVersion;
                documentosJSON[index].comentario = comentarioDocumento;
                documentosJSON[index].idEstadoDocumento = idEstadoDocumento;
                documentosJSON[index].actualizarEstadoFinal = actualizarEstadoFinal;
                documentosJSON[index].imprimirCarta = $('#chkImprimirCarta' + i).is(':checked');
                documentosJSON[index].setearSaltoLinea = setearSaltoLinea;
                index++;
            }
        }

        if (documentosJSON.length > 0) {

            var parametros = {
                accion: 'GRABAR-ACTADOCUMENTOCOMENTARIOESTADO',
                idActa: idActa,
                documentos: JSON.stringify(documentosJSON)
            };

            invocarControladorPOST(parametros, function (data) {
                if (data.result == 'OK') {
                    seGraboAlgunDocumento = true;
                    recargarDocumentos = actualizarEstadoFinal;
                    grabarActaNotasTratadas();

                } else {
                    $.messager.alert('Error', data.message, 'error');
                }
            });

        }
    }
}

function quitarActaDocumento() {
    if (idActa != -1 && idActaDocumento != -1) {

        var actaDocumentoSeleccionado = $('#dgActaDocumentos').datagrid('getSelected');
        if (actaDocumentoSeleccionado) {

            $.messager.confirm('Confirmar', '¿Desea quitar el documento ' + actaDocumentoSeleccionado.NombreEstudio + ' - ' + actaDocumentoSeleccionado.NombreDocumento + '?',
            function (r) {
                if (r) {
                    var parametros = {
                        accion: 'ELIMINAR-ACTADOCUMENTO',
                        idActa: idActa,
                        idActaDocumento: idActaDocumento
                    };
                    invocarControlador(parametros, guardarDatosActaSucess);
                }
            });
        }
    }
}

function guardarActaDocumento() {

    
    var parametros = {
        accion: 'GRABAR-ACTADOCUMENTO',
        idActa: idActa,
        idActaDocumento: idActaDocumento,
        idDocumento: $('#cboActaDocumento').combobox('getValue'),
        idDocumentoVersion: $('#cboActaDocumentoVersion').combobox('getValue'),
        comentario: formatearCaracteres($('#txtActaDocumentoComentario').val()),
        idResponsableComite: $('#cboActaDocumentoResponsableComite').combobox('getValue'),
        imprimirCarta: true
    };
    invocarControladorPOST(parametros, guardarActaDocumentoSucess);
}

function guardarActaDocumentoSucess(data) {
    if (data.result == 'OK') {
        obtenerDatosActa();
        $('#dlgActaDocumento').dialog('close');
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarActaDocumento() {
    $('#dlgActaDocumento').dialog('close');
}

function cambiarEstadosActaDocumento() {
    if (idActa != -1) {
        procesarActaDocumentos(true);
    }
}

function expandirFilasDocumentos() {
    var filas = $('#dgActaDocumentos').datagrid('getRows');
    if (filas.length > 0) {
        for (var i = filas.length - 1; i >= 0; i--) {
            $('#dgActaDocumentos').datagrid('expandRow', i);
        }
        //        $('#dgActaDocumentos').datagrid('selectedRow', 0);
    }
}

function contraerFilasDocumentos() {
    var filas = $('#dgActaDocumentos').datagrid('getRows');
    if (filas.length > 0) {
        for (var i = filas.length - 1; i >= 0; i--) {
            $('#dgActaDocumentos').datagrid('collapseRow', i);
        }
        $('#dgActaDocumentos').datagrid('selectedRow', 0);
    }
}

function ordenarDocumentos() {
    var parametroIdEstudio = idEstudio != -1 ? '&IdEstudio=' + idEstudio : '';
    location.href = 'ActaOrdenarDocumentos.aspx?PaginaReturn=ActaDatos&IdActa=' + idActa + parametroIdEstudio;
}

/***********************************************/
/**************** NOTAS *******************/

function cargarNotaCboEstudio() {
    $('#cboActaEstudioNota').combobox({
        url: 'handlers/EstudioCargaDatosHandler.ashx?accion=LISTAR-ESTUDIOS',
        valueField: 'Id',
        textField: 'NombreCompleto',
        panelHeight: 350,
        onSelect: function (rec) {
            if (rec) cargarCboActaNota(rec.Id);
        }
    });
}

function cargarCboActaNota(id) {
    $('#cboActaNotaTratada').combobox({
        url: 'handlers/NotasHandler.ashx?accion=LISTAR-NOTAS-SIN-ACTA&idEstudio=' + id,
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 350,
        onSelect: function (rec) {
            if (rec) $('#txtNotaTratadaTexto').val(rec.Texto);
        }
    });
}


function agregarNotaTratada() {
    if (idActa != -1) {
        cargarNotaCboEstudio();
        idNotaTratada = -1;
        limpiarControlesNotaTratada();
        $('#dlgNotaTratada').dialog('open').dialog('setTitle', 'Nota a tratar');
    }
}

function limpiarControlesNotaTratada() {
    $('#cboActaEstudioNota').combobox('setValue', '');
    $('#cboActaNotaTratada').combobox('setValue', '');
    $('#cboActaNotaPosicionImprime').combobox('setValue', '0');
    $('#txtNotaTratadaTexto').val('')
}

function quitarNotaTratada() {
    if (idActa != -1 && idNotaTratada != -1) {

        var notaTratadaSeleccionada = $('#dgNotasTratadas').datagrid('getSelected');
        if (notaTratadaSeleccionada) {

            $.messager.confirm('Confirmar', '¿Desea quitar la nota ' + notaTratadaSeleccionada.NombreEstudio + ' - ' + notaTratadaSeleccionada.Descripcion + '?',
            function (r) {
                if (r) {
                    var parametros = {
                        accion: 'ELIMINAR-ACTANOTATRATADA',
                        idActa: idActa,
                        idNotaTratada: idNotaTratada
                    };
                    invocarControlador(parametros, guardarDatosActaSucess);
                }
            });
        }
    }
}

function guardarNotaTratada() {

    var parametros = {
        accion: 'GRABAR-ACTANOTATRATADA',
        idActa: idActa,
        idNota: $('#cboActaNotaTratada').combobox('getValue'),
        imprimeAlFinal: $('#cboActaNotaPosicionImprime').combobox('getValue')
    };
    invocarControladorPOST(parametros, guardarNotaTratadaSucess);
}

function guardarNotaTratadaSucess(data) {
    if (data.result == 'OK') {
        obtenerDatosActa();
        $('#dlgNotaTratada').dialog('close');
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarNotaTratada() {
    $('#dlgNotaTratada').dialog('close');
}

/***********************************************/
/**************** NUEVA NOTA *******************/

function cargarNuevaNotaCboEstudio() {
    $('#cboActaEstudioNuevaNota').combobox({
        url: 'handlers/EstudioCargaDatosHandler.ashx?accion=LISTAR-ESTUDIOS',
        valueField: 'Id',
        textField: 'NombreCompleto',
        panelHeight: 'auto'
    });
}

function agregarNuevaNotaTratada() {
    if (idActa != -1) {
        cargarNuevaNotaCboEstudio();
        idNotaTratada = -1;
        limpiarControlesNuevaNotaTratada();
        $('#dlgNuevaNotaTratada').dialog('open').dialog('setTitle', 'Nueva Nota');
    }
}

function limpiarControlesNuevaNotaTratada() {
    $('#cboActaEstudioNuevaNota').combobox('setValue', '');
    $('#cboActaNotaPosicionImprime').combobox('setValue', '0');
    //$('#txtNuevaNotaDescripcion').val(''));
    $('#cboNuevaNotaAutor').combobox('setValue', '');
    $('#txtNuevaNotaTexto').val('');
}

function detalleNuevaNotaTratada() {
    if (idActa != -1 && idNotaTratada != -1) {
        limpiarControlesNuevaNotaTratada();
        var notaTratadaSeleccionada = $('#dgNotasTratadas').datagrid('getSelected');
        if (notaTratadaSeleccionada) {
            cargarNuevaNotaCboEstudio();
            $('#cboActaEstudioNuevaNota').combobox('setValue', notaTratadaSeleccionada.IdEstudio);

            $('#cboActaNuevaNotaPosicionImprime').combobox('setValue', (notaTratadaSeleccionada.ActaImprimeAlFinal ? 1 : 0));
            //$('#txtNuevaNotaDescripcion').val(notaTratadaSeleccionada.Descripcion);
            $('#txtNuevaNotaTexto').val(notaTratadaSeleccionada.Texto);

            $('#dlgNuevaNotaTratada').dialog('open').dialog('setTitle', 'Nota');
        }
    }
}

function guardarNuevaNotaTratada() {

    var descripcionNota = $('#txtActaDescripcion').textbox('getValue') + ' - Estudio ' + $('#cboActaEstudioNuevaNota').combobox('getText');
    var notasJSON = new Array();
    var index = 0;

    notasJSON[index] = new Object();
    notasJSON[index].imprimeAlFinal = $('#cboActaNuevaNotaPosicionImprime').combobox('getValue');
    notasJSON[index].idNota = idNotaTratada;
    notasJSON[index].idEstudio = $('#cboActaEstudioNuevaNota').combobox('getValue');
    notasJSON[index].descripcion = descripcionNota;
    notasJSON[index].fecha = getFechaActual();
    notasJSON[index].idAutor = $('#cboNuevaNotaAutor').combobox('getValue');
    notasJSON[index].requiereRespuesta = false;
    notasJSON[index].idDocumento = -1;
    notasJSON[index].idDocumentoVersion = -1;
    notasJSON[index].texto = formatearCaracteres($('#txtNuevaNotaTexto').val());

    var parametros = {
        accion: 'GRABAR-ACTANUEVANOTATRATADA',
        idActa: idActa,
        notas: JSON.stringify(notasJSON)
    };

    invocarControladorPOST(parametros, function (data) {
        if (data.result == 'OK') {
            obtenerDatosActa();
            $('#dlgNuevaNotaTratada').dialog('close');
        } else {
            $.messager.alert('Error', data.message, 'error');
        }
    });
}

function cancelarNuevaNotaTratada() {
    $('#dlgNuevaNotaTratada').dialog('close');
}


function grabarActaNotasTratadas() {
    if (idActa != -1) {
        procesarActaNotas()

//        var filas = $('#dgNotasTratadas').datagrid('getRows');
//        if (filas.length > 0)
//            grabarActaNotaRecursivo(0);
    }
}

function procesarActaNotas() {
    var notasJSON = new Array();
    var notas = $('#dgNotasTratadas').datagrid('getRows');
    var index = 0;
    if (notas.length > 0) {
        for (var i = 0; i < notas.length; i++) {

            var textComentario = formatearCaracteres($('#txtNotaComentario' + i).val());

            if (notas[i].Texto != textComentario) {
                notasJSON[index] = new Object();
                notasJSON[index].imprimeAlFinal = (notas[i].ActaImprimeAlFinal) ? 1 : 0;
                notasJSON[index].idNota = notas[i].Id;
                notasJSON[index].idEstudio = notas[i].IdEstudio;
                notasJSON[index].descripcion = notas[i].Descripcion;
                notasJSON[index].fecha = notas[i].FechaToString;
                notasJSON[index].idAutor = (notas[i].Autor != null) ? notas[i].Autor.Id : null;
                notasJSON[index].requiereRespuesta = false;
                notasJSON[index].idDocumento = -1;
                notasJSON[index].idDocumentoVersion = -1;
                notasJSON[index].texto = textComentario;
                index++;
            }
        }

        var parametros = {
            accion: 'GRABAR-ACTANUEVANOTATRATADA',
            idActa: idActa,
            notas: JSON.stringify(notasJSON)
        };

        invocarControladorPOST(parametros, function (data) {
            if (data.result == 'OK') {
                $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                if (recargarDocumentos)
                    obtenerDatosActa();
            } else {
                $.messager.alert('Error', data.message, 'error');
            }
        });        
    }
}

function expandirFilasNotas() {
    var filas = $('#dgNotasTratadas').datagrid('getRows');
    if (filas.length > 0) {
        for (var i = filas.length - 1; i >= 0; i--) {
            $('#dgNotasTratadas').datagrid('expandRow', i);
        }
        $('#dgNotasTratadas').datagrid('selectedRow', 0);
    }
}

function contraerFilasNotas() {
    var filas = $('#dgNotasTratadas').datagrid('getRows');
    if (filas.length > 0) {
        for (var i = filas.length - 1; i >= 0; i--) {
            $('#dgNotasTratadas').datagrid('collapseRow', i);
        }
        $('#dgNotasTratadas').datagrid('selectedRow', 0);
    }
}


/**************************************************/
/**************** PARTICIPANTES *******************/
function actaSeleccionarParticipantes() {
    if (idActa != -1) {
        var parametros;
        parametros = {
            accion: 'LISTAR-MIEMBOSCOMITE'
        };
        controladorAJAX_GET('handlers/ProfesionalesHandler.ashx', parametros,
        function (data) {
            if (data)
                $('#dgActaSeleccionParticipantes').datagrid('loadData', data);
        });

        $('#dlgActaSeleccionParticipantes').dialog('open').dialog('setTitle', 'Seleccionar Participantes');
    }
}

function aceptarActaSeleccionParticipantes() {
    var documentosSeleccionados = $('#dgActaSeleccionParticipantes').datagrid('getSelections');
    if (documentosSeleccionados) {
        //$('#datagrid('loadData', documentosSeleccionados);
        var parametros;
        parametros = {
            accion: 'GRABAR-ACTAPARTICIPANTES',
            idActa: idActa,
            participantes: obtenerParticipantesSeleccionados()
        };
        invocarControlador(parametros, aceptarActaSeleccionParticipantesSucess);
    }
}

function aceptarActaSeleccionParticipantesSucess(data) {
    if (data.result == 'OK') {
        obtenerDatosActa();
        $('#dlgActaSeleccionParticipantes').dialog('close');
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarActaSeleccionParticipantes() {
    $('#dlgActaSeleccionParticipantes').dialog('close');
}

function obtenerParticipantesSeleccionados() {
    var profSeleccionados = '';

    var rows = $('#dgActaSeleccionParticipantes').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Profesional)
            profSeleccionados = profSeleccionados + ';' + rows[i].Profesional.Id;
        else
            profSeleccionados = profSeleccionados + ';' + rows[i].Id;

        if (rows[i].RolComite)
            profSeleccionados = profSeleccionados + ',' + rows[i].RolComite.Id;
        else
            profSeleccionados = profSeleccionados + ',-1';
    }

    return profSeleccionados;
}

function obtenerParticipantes() {
    var participantes = '';

    var rows = $('#dgActaParticipantes').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        participantes = participantes + ';' + rows[i].Id + ','
                                            + rows[i].Profesional.Id + ','
                                            + rows[i].IdRolComite;
    }

    return participantes;
}


function quitarParticipante() {
    if (idActa != -1) {
        var participanteSeleccionado = $('#dgActaParticipantes').datagrid('getSelected');
        if (participanteSeleccionado) {

            $.messager.confirm('Confirmar', '¿Desea quitar el participante ' + participanteSeleccionado.Profesional.NombreCompleto + '?',
            function (r) {
                if (r) {
                    var parametros = {
                        accion: 'ELIMINAR-ACTAPARTICIPANTES',
                        idActa: idActa,
                        idActaParticipante: participanteSeleccionado.Id
                    };
                    invocarControlador(parametros, guardarDatosActaSucess);
                }
            });
        }
    }
}

/*******************************************************/
/**************** CARTA DE RESPUESTA *******************/
function actaSeleccionarEstudioCartaRespuesta() {

    
    if (idActa != -1) {
        var parametros;
        parametros = {
            accion: 'LISTAR-ACTAESTUDIOS',
            idActa: idActa
        };

        invocarControlador(parametros, function (data) {
            $('#dgActaSeleccionCartaRespuesta').datagrid('loadData', data);
        });
    }
}

function mostrarSeleccionCartaRespuesta() {
    $('#dlgActaSeleccionCartaRespuesta').dialog('setTitle', 'Generar Carta de Respuesta');
    $('#dlgActaSeleccionCartaRespuesta').dialog('open');
}


function cerrarActaSeleccionCartaRespuesta() {
    if (idActa != -1) {
        $('#dlgActaSeleccionCartaRespuesta').dialog('close');
    }
}


/*******************************************************/
/**************** ACTUALIZAR ESTUDIO *******************/
function limpiarControlesEstudio() {
    $('#txtActaCodigoEstudio').val('');
    $('#txtActaNombreCompletoEstudio').val('');
    $('#cboActaEstadosEstudio').combobox('clear');
    $('#cboActaCartaRespuestaModelo').combobox('clear');
    $('#txtTextoLibre').val( '');
}

function actualizarEstudio() {
    if (idActa != -1 && idActaDocumento != -1) {
        
        limpiarControlesEstudio();

        var actaDocumentoSeleccionado = $('#dgActaDocumentos').datagrid('getSelected');
        if (actaDocumentoSeleccionado) {

            $('#txtActaCodigoEstudio').val(actaDocumentoSeleccionado.NombreEstudio);
            $('#txtActaNombreCompletoEstudio').val(actaDocumentoSeleccionado.NombreEstudioCompleto);
            
            $("#btnExportarCartaPDFActaActualizarEstudio").attr({
                href: 'Impresion.aspx?Plantilla=PLANTILLA_CARTARESPUESTA&IdEstudio=' + actaDocumentoSeleccionado.IdEstudio + '&IdActa=' + idActa,
                target: '_blank'
            });
            $("#btnExportarCartaWORDActaActualizarEstudio").attr({
                href: 'Impresion.aspx?Plantilla=PLANTILLA_CARTARESPUESTA&IdEstudio=' + actaDocumentoSeleccionado.IdEstudio + '&IdActa=' + idActa + '&ExportTo=2'
            });

            var parametros = {
                accion: 'OBTENER-ACTADATOSESTUDIO',
                idActa: idActa,
                idEstudio: actaDocumentoSeleccionado.IdEstudio
            };

            invocarControlador(parametros, function (data) {
                if (data.result != 'Error') {
                    if (data.EstadoEstudio != null)
                        $('#cboActaEstadosEstudio').combobox('setValue', data.EstadoEstudio.Id);
                    if (data.CartaRespuestaModelo) {
                        $('#cboActaCartaRespuestaModelo').combobox('setValue', data.CartaRespuestaModelo.Id);
                        $("#btnExportarCartaPDFActaActualizarEstudio").linkbutton({ disabled: false });
                        $("#btnExportarCartaWORDActaActualizarEstudio").linkbutton({ disabled: false }); 
                    }
                    else {
                        $("#btnExportarCartaPDFActaActualizarEstudio").linkbutton({ disabled: true });
                        $("#btnExportarCartaWORDActaActualizarEstudio").linkbutton({ disabled: true }); 
                    }
                    
                    $('#txtTextoLibre').val(formatearTextoASCII(data.TextoLibreCartaRespuesta));
                }
            });

            $('#dlgActaActualizarEstudio').dialog('setTitle', 'Actualizar Estudio')
                                          .dialog('center')
                                          .dialog('open');
        }
    }
}

function aceptarActaActualizarEstudio() {

    var actaDocumentoSeleccionado = $('#dgActaDocumentos').datagrid('getSelected');

    var datos = new Object();
    datos.IdEstudio = actaDocumentoSeleccionado.IdEstudio;
    datos.IdEstadoEstudio = $('#cboActaEstadosEstudio').combobox('getValue');
    datos.IdCartaRespuestaModelo = $('#cboActaCartaRespuestaModelo').combobox('getValue');
    datos.TextoLibreCartaRespuesta = $('#txtTextoLibre').val();

    var parametros = {
        accion: 'GRABAR-ACTADATOSESTUDIO',
        idActa: idActa,
        datosActaEstudio: JSON.stringify(datos)
    };

    invocarControladorPOST(parametros,
        function (data) {
            if (data.result == 'OK') {
                $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                if (datos.IdCartaRespuestaModelo) {
                    $("#ctrolSeleccionPlantilla_abrir").linkbutton({ disabled: false });
                    $("#btnExportarCartaWORDActaActualizarEstudio").linkbutton({ disabled: false });
                }
                else {
                    $("#btnExportarCartaPDFActaActualizarEstudio").linkbutton({ disabled: true });
                    $("#btnExportarCartaWORDActaActualizarEstudio").linkbutton({ disabled: true });
                }
            } else {
                $.messager.alert('Error', data.message, 'error');
            }
        });
}

function cerrarActaActualizarEstudio() {
    $('#dlgActaActualizarEstudio').dialog('close');
}


function abrir_ctrolSeleccionPlantilla(controlTxt) {
    controlActualizaTextoPlantilla = controlTxt;
    ctrolSeleccionPlantilla_abrir();
}

//esta funcion se invoca desde PlantillaCtrolCKEditorSeleccion para actualizar el txt en esta pantalla
function actualizar_ctrolSeleccionPlantilla(textoSeleccionado) {
    
    var textCtrl = $('#' + controlActualizaTextoPlantilla).val();
    $('#' + controlActualizaTextoPlantilla).val(textCtrl + textoSeleccionado);

}