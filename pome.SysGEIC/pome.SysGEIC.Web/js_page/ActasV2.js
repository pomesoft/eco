var idActa;
var idEstudio;
var idActaDocumento;

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
    configurarPaneles();
    incializarEventosControles();
    cargarDatos();
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

    $('#chkActaCerrada').switchbutton({
        checked: true,
        onText: 'SI',
        offText: 'NO',
        wigth: 50,
        onChange: function (checked) {
        }
    });

    $('#cboActaEstadosEstudio').combobox({
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 200,
        onSelect: function (row) {
                
        },
        onLoadSuccess: function () {
                
        }
    });

    
    $('#cboActaCartaRespuestaModelo').combobox({
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 200,
        onSelect: function (row) {

        },
        onLoadSuccess: function () {
                
        }
    });

    
    $("#btnEstudioGuardar").linkbutton({
        onClick: guardarEstudio
    });

    $("#btnEstudioExportarCartaPDF").linkbutton({
        onClick: function (){
            location.href = 'Impresion.aspx?Plantilla=PLANTILLA_CARTARESPUESTA&IdEstudio=' + idEstudio + '&IdActa=' + idActa;
        }
    });
    $("#btnEstudioExportarCartaWORD").linkbutton({
        onClick: function (){
            location.href = 'Impresion.aspx?Plantilla=PLANTILLA_CARTARESPUESTA&IdEstudio=' + idEstudio + '&IdActa=' + idActa + '&ExportTo=2';
        }
    });
}

function volver() {
    if($('#chkActaCerrada').is(':checked')){
        location.href = getPaginaReturn();
    } else {
        var parametroIdEstudio = idEstudio && idEstudio != -1 ? '&IdEstudio=' + idEstudio : '';
        location.href = getPaginaReturn() + '?IdActa=' + idActa + parametroIdEstudio;
    }
}


function configurarPaneles() {
    $("#panelInfoActa").panel({
        width: '100%',
        title: 'Acta'
    });
    
    $("#panelComentariosParticipantes").panel({
        width: '100%',
        //height: 1300,
        collapsible: true,
        title: 'Introducción, participantes, comentarios iniciales y finales'
    });
    $('#panelComentariosParticipantes').panel('collapse', true);

    $('#panelComentarioInicialFijo').panel({
        width: '99%',
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

    $('#panelComentarioInicial').panel({
        width: '99%',
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

    $('#panelComentarioFinal').panel({
        width: '99%',
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
    
    $('#panelParticipantes').panel({
        width: '99%',
        height: 300,
        title: 'Participantes',
        collapsible: true
    });


    $('#panelDatosEstudio').panel({
        width: '100%',
        title: '',
        //collapsible: true
    });
        
    $('#panelNotaCartaRespuesta').panel({
        width: '100%',
        height: 250,
        title: 'Notas que se imprimen en la carta de respuesta del estudio',
        collapsible: true,
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtNotaCartaRespuesta');
            }
        }],
        onExpand: function () {

        }
    });
    $('#panelNotaCartaRespuesta').panel('collapse', true);

    $('#panelNotaAntesDocsTratados').panel({
        width: '100%',
        height: 250,
        title: 'Notas que se imprimen antes de documentos tratados del estudio',
        collapsible: true,
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtComentarioAntesDocumentos');
            }
        }],
        onExpand: function () {

        }
    });
    $('#panelNotaAntesDocsTratados').panel('collapse', true);

    $('#panelNotaDespuesDocsTratados').panel({
        width: '100%',
        height: 250,
        title: 'Notas que se imprimen después de documentos tratados del estudio',
        collapsible: true,
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla('txtComentarioDespuesDocumentos');
            }
        }],
        onExpand: function () {

        }
    });
    $('#panelNotaDespuesDocsTratados').panel('collapse', true);

}

function configurarGrillas() {

    $('#dgActaEstudios').datagrid({
        title: 'Estudios tratados',
        width: '100%',
        //height: 800,
        showHeader: true,
        autoRowHeight: false,
        collapsible: true,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'OrdenEstudio',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Id', hidden: true },
            { field: 'OrdenEstudio', title: 'OrdenEstudio', width: '30%', hidden: true },
            { field: 'NombreCompleto', title: '', width: '99%' }
        ]],
        onSelect: function (rowIndex, rowData) {
            idEstudio = rowData.Id;
            obtenerDatosEstudioSeleccionado();
        },
        onClickRow: function (rowIndex, rowData) {
            
        },
        onClickCell: function (rowIndex, field, value) {
            
        }
    });
    $('#dgActaEstudios').datagrid('enableFilter');


    $('#dgActaDocumentos').datagrid({
        title: '',
        width: '100%',
        height: 550,
        autoRowHeight: true,
        collapsible: true,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        nowrap: false,
        singleSelect: true,
        sortName: 'Orden',
        sortOrder: 'asc',
        remoteSort: false,
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Orden', title: 'Orden', width: 60, hidden: true },
                { field: 'OrdenEstudio', title: '', width: 30, hidden: true },
                { field: 'OrdenDocumento', title: 'Orden', width: '5%', hidden: true },
                { field: 'NombreDocumento', title: 'Documento', width: '55%' },
                { field: 'VersionDocumento', title: 'Versión', width: '25%' },
                { field: 'IdEstadoActual', title: 'Estado', width: '15%',
                    formatter: function (value, row, index) {
                        for (var i = 0; i < row.EstadosDocumento.length; i++) {
                            if (row.EstadosDocumento[i].Id == value) return row.EstadosDocumento[i].Descripcion;
                        }
                        return value;
                    }
                }
        ]],     
        toolbar: [{
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarActaDocumento
        }, {
            text: 'Guardar',
            iconCls: 'icon-save',
            handler: guardarEstudio
        }, '-', {
            text: 'Aprobar y Tomar Conocimiento',
            iconCls: 'icon-ok',
            handler: cambiarEstadosActaDocumento
        }, '-', {
            text: 'Expandir',
            iconCls: 'icon-expand',
            handler: expandirFilasDocumentos
        }, {
            text: 'Contraer',
            iconCls: 'icon-collapse',
            handler: contraerFilasDocumentos
        }, '-', {
            text: 'Reordenar',
            iconCls: 'icon-sort',
            handler: ordenarDocumentos
        }],
        view: detailview,
        detailFormatter: function (rowIndex, rowData) {

            var responsable = '';
            if (rowData.ResponsableComite)
                responsable = rowData.ResponsableComite.NombreCompleto;

            var datosAMostrar = ''
            datosAMostrar += '<p>Documento: <span><strong>' + rowData.NombreDocumento + '</strong></span></p>'; /// <reference path="../css/default/" />
            if (rowData.VersionDocumento != '') {
                datosAMostrar += '<p>Versión del documento: <span><strong>' + rowData.VersionDocumento + '</strong></span>   Fecha: <span><strong>' + rowData.VersionFecha + '</strong></span></p>';
            }
            datosAMostrar += '<p>Estado del documento: ' + crearComboEstados(rowIndex.toString(), rowData);
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
            {
                field: 'IdRolComite', title: 'Rol en Comité', width: 200,
                formatter: function (value, row, index) {
                    if (rolesComite) {
                        for (var i = 0; i < rolesComite.length; i++) {
                            if (rolesComite[i].Id == value) return rolesComite[i].Descripcion;
                        }
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



function configurarPanelesGrillaComentarioDocumentos(controlPanel, controlText) {
    $('#' + controlPanel).panel({
        width: 870,
        height: 150,
        title: 'Comentario',
        tools: [{
            iconCls: 'icon-view',
            handler: function () {
                abrir_ctrolSeleccionPlantilla(controlText);
            }
        }]
    });
}


function cargarDatos() {    
obtenerRolesComite()
.then(roles => {
    rolesComite = roles;
    configurarGrillas();
    return obtenerEstadosEstudio();
    
})
.then(estados => {
    $('#cboActaEstadosEstudio').combobox('loadData', estados);
    return obtenerModelosCarta();        

})
.then(modelos => {
    $('#cboActaCartaRespuestaModelo').combobox('loadData', modelos);
    return obtenerEstudiosActa();    

})
.then(datosActa => {
    mostrarDatosActa(datosActa);
});
}

function obtenerEstadosEstudio(){
    
    return new Promise(function (resolve, reject) {
         
        var parametros;
        parametros = {
            accion: 'OBTENER-ESTADOSESTUDIO'
        };

        controladorAJAX_GET('handlers/EstudioCargaDatosHandler.ashx', parametros, 
            function (data){
                if (data.result == 'Error') {
                    reject(data.result + ' - ' + data.message);        
                } else {
                    resolve(data);    
                }
            });

    });
}


function obtenerModelosCarta(){
    
    return new Promise(function (resolve, reject) {
         
        var parametros;
        parametros = {
            accion: 'LISTAR-CARTARESPUESTAMODELO'
        };

        controladorAJAX_GET('handlers/CartaRespuestaHandler.ashx', parametros, 
            function (data){
                if (data.result == 'Error') {
                    reject(data.result + ' - ' + data.message);        
                } else {
                    resolve(data);    
                }
            });

    });
}

function obtenerRolesComite() {
    
    return new Promise(function (resolve, reject) {
         
        var parametros;
        parametros = {
            accion: 'LISTAR-ROLESCOMITE'
        };

        controladorAJAX_GET('handlers/ProfesionalesHandler.ashx', parametros,
            function (data){
                if (data.result == 'Error') {
                    reject(data.result + ' - ' + data.message);        
                } else {
                    resolve(data);    
                }
            });

    });
}



function obtenerEstudiosActa() {
    var URLParamIdActa = getURLParam("IdActa");
    if (URLParamIdActa && URLParamIdActa != -1)
        idActa = URLParamIdActa;

    if (idActa != -1) {
        var parametros;
        parametros = {
            accion: 'OBTENER-ACTADTO',
            idActa: idActa
        };

        
        $('#dlgCargandoSite').dialog('open').dialog('setTitle', '');

        return new Promise(function (resolve, reject) {
            
            controladorAJAX_GET('handlers/ActasHandler.ashx', parametros, 
                function (data){
                    if (data.result == 'Error') {
                        reject(data.result + ' - ' + data.message);        
                    } else {
                        resolve(data);    
                    }
                });

        });
    }
}


function mostrarDatosActa(data) {
    if (data) {
        limpiarControlesActa();
        $('#txtActaFecha').datebox('setValue', data.Fecha);
        $('#txtActaFecha').datebox('disable');
        $('#txtActaHora').timespinner('setValue', data.Hora);
        $('#txtActaHora').timespinner('disable');
        $('#txtActaDescripcion').textbox('setText', data.Descripcion);
        if(data.Cerrada){
            $('#chkActaCerrada').switchbutton('check'); 
        } else {
            $('#chkActaCerrada').switchbutton('uncheck');
        }

        $('#txtActaComentarioInicialFijo').val(formatearTextoASCII(data.ComentarioInicialFijo));
        $('#txtActaComentarioInicial').val(formatearTextoASCII(data.ComentarioInicial));
        $('#txtActaComentarioFinal').val(formatearTextoASCII(data.ComentarioFinal));

        $('#dgActaParticipantes').datagrid('loadData', data.Participantes);

        $('#dgActaEstudios').datagrid('loadData', data.EstudiosTratados);
        $('#dgActaDocumentos').datagrid('loadData', []);

        $('#dgActaEstudios').datagrid('selectRow', 0);

        $("#btnActaImprimirActa").attr({
            href: 'Impresion.aspx?PaginaReturn=ActaDatosV2&Plantilla=PLANTILLA_ACTA&ExportTo=3&IdEstudio=' + idEstudio + '&IdActa=' + idActa,
            target: '_blank'
        });

        $("#btnActaExportarWord").attr({
            href: 'Impresion.aspx?PaginaReturn=ActaDatosV2&Plantilla=PLANTILLA_ACTA&IdEstudio=' + idEstudio + '&IdActa=' + idActa + '&ExportTo=2'
        });
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

    $('#chkActaCerrada').switchbutton('uncheck');

}




function guardarActa() {

    if (lastIndexParticipantes != -1)
        $('#dgActaParticipantes').datagrid('endEdit', lastIndexParticipantes);

    var parametros = {
        accion: 'GRABAR-ACTA',
        idActa: idActa,
        descripion: $('#txtActaDescripcion').textbox('getText'),
        fecha: $('#txtActaFecha').datebox('getValue'),
        hora: $('#txtActaHora').timespinner('getValue'),
        comentarioInicialFijo: $('#txtActaComentarioInicialFijo').val(),
        comentarioInicial: $('#txtActaComentarioInicial').val(),
        comentarioFinal: $('#txtActaComentarioFinal').val(),
        cerrada: $('#chkActaCerrada').is(':checked'),
        
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
        obtenerDatosEstudioSeleccionado();
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



/*******************************************************/
/**************** ESTUDIO *******************/
function limpiarControlesEstudio() {
    $('#tituloEstudio').html('');
    $('#cboActaEstadosEstudio').combobox('clear');
    $('#cboActaCartaRespuestaModelo').combobox('clear');
    $('#txtNotaCartaRespuesta').val( '');
}

function obtenerDatosEstudioSeleccionado() {
    if (idActa != -1 && idEstudio != -1) {

        limpiarControlesEstudio();

        var parametros = {
            accion: 'OBTENER-ACTADATOSESTUDIO',
            idActa: idActa,
            idEstudio: idEstudio
        };

        invocarControlador(parametros, function (data) {
            if (data.result != 'Error') {

                /*
                $('#panelDatosEstudio').panel({
                    width: '100%',
                    title: 'Estudio seleccionado: ' + data.NombreEstudioListados,
                    collapsible: true
                });
                */
                $('#tituloEstudio').html(data.NombreCompleto + ' <br /> ' + 'Código: ' + data.NombreEstudioListados );
                                
                if (data.EstadoEstudio != null)
                    $('#cboActaEstadosEstudio').combobox('setValue', data.EstadoEstudio.Id);
                
                if (data.CartaRespuestaModelo) {
                    $('#cboActaCartaRespuestaModelo').combobox('setValue', data.CartaRespuestaModelo.Id);
                    $("#btnEstudioExportarCartaPDF").linkbutton({ disabled: false });
                    $("#btnEstudioExportarCartaWORD").linkbutton({ disabled: false });
                }
                else {
                    $("#btnEstudioExportarCartaPDF").linkbutton({ disabled: true });
                    $("#btnEstudioExportarCartaWORD").linkbutton({ disabled: true });
                }

                $('#txtNotaCartaRespuesta').val(data.TextoLibreCartaRespuesta);

                $('#txtComentarioAntesDocumentos').val(data.ComentarioAntesDocumentos);
                $('#txtComentarioDespuesDocumentos').val(data.ComentarioDespuesDocumentos);
                

                obtenerDocumentosEstudioSeleccionado();

                //$('#txtNotaCartaRespuesta').val(formatearTextoASCII(data.TextoLibreCartaRespuesta));
            }
        });
    }
}

function obtenerDocumentosEstudioSeleccionado() {
    if (idEstudio != -1) {
        var parametros = {
            accion: 'OBTENER-ACTAESTUDIODOCUMENTOS',
            idActa: idActa,
            idEstudio: idEstudio
        };

        invocarControlador(parametros, function (data) {
            if (data.result != 'Error') {
                $('#dgActaDocumentos').datagrid('loadData', data);
            }
        });
    }
}


function guardarEstudio() {

    if(idEstudio!=-1){

        var datos = new Object();
        datos.IdEstudio = idEstudio;
        datos.IdEstadoEstudio = $('#cboActaEstadosEstudio').combobox('getValue');
        datos.IdCartaRespuestaModelo = $('#cboActaCartaRespuestaModelo').combobox('getValue');        
        datos.TextoLibreCartaRespuesta = $('#txtNotaCartaRespuesta').val();
        datos.ComentarioAntesDocumentos = $('#txtComentarioAntesDocumentos').val();
        datos.ComentarioDespuesDocumentos = $('#txtComentarioDespuesDocumentos').val();

        var parametros = {
            accion: 'GRABAR-ACTADATOSESTUDIO',
            idActa: idActa,
            datosActaEstudio: JSON.stringify(datos)
        };

        invocarControladorPOST(parametros,
            function (data) {
                if (data.result == 'OK') {
                    grabarActaDocumentos();
                } else {
                    $.messager.alert('Error', data.message, 'error');
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
                    $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                    obtenerDatosEstudioSeleccionado();

                } else {
                    $.messager.alert('Error', data.message, 'error');
                }
            });

        } else {
            $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
            obtenerDatosEstudioSeleccionado();
        }

    }
}

function quitarActaDocumento() {
    if (idActa != -1 && idActaDocumento != -1) {

        var actaDocumentoSeleccionado = $('#dgActaDocumentos').datagrid('getSelected');
        if (actaDocumentoSeleccionado) {

            $.messager.confirm({
                title: 'Confirmar',
                msg: '¿Desea quitar el documento ' + actaDocumentoSeleccionado.NombreEstudio + ' - ' + actaDocumentoSeleccionado.NombreDocumento + '?',
                width: 500,
                height: 180,
                icon: 'info',
                fn: function (r) {
                    if (r) {
                        var parametros = {
                            accion: 'ELIMINAR-ACTADOCUMENTO',
                            idActa: idActa,
                            idActaDocumento: idActaDocumento
                        };
                        invocarControlador(parametros, guardarDatosActaSucess);
                    }
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
    location.href = 'ActaOrdenarDocumentos.aspx?PaginaReturn=ActaDatosV2&IdActa=' + idActa + parametroIdEstudio;
}

/***********************************************/
/**************** NOTAS *******************/


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
/**************** PLANTILLAS DE TEXTO *******************/
//esta funcion se invoca desde PlantillaCtrolCKEditorSeleccion para actualizar el txt en esta pantalla
function actualizar_ctrolSeleccionPlantilla(textoSeleccionado) {
    
    var textCtrl = $('#' + controlActualizaTextoPlantilla).val();
    $('#' + controlActualizaTextoPlantilla).val(textCtrl + textoSeleccionado);

}


function abrir_ctrolSeleccionPlantilla(controlTxt) {
    controlActualizaTextoPlantilla = controlTxt;
    ctrolSeleccionPlantilla_abrir();
}
