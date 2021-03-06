﻿var idActaSeleccionada;
var idEstudioSeleccionado;
var idDocumentoSeleccionado;
var documentoRequiereVersion;
var investigadoresEstudio;

var idNotaTratadaAntes;
var idNotaTratadaDespues;


$(document).ready(function () {

    idActaSeleccionada = -1;
    idEstudioSeleccionado = -1;
    idDocumentoSeleccionado = -1;
    documentoRequiereVersion = false;

    configurarPaneles();
    inicializarBuscadores();
    configurarGrillas();
    incializarEventosControles();

    cargarCboActas();    
        
});

function configurarPaneles() {
    
    $("#panelContainer").panel({
        width: 980,
        height: 750,
        title: 'Bandeja Entrada de Documentos'
    });
    $("#panelSeleccionActa").panel({
        width: '99%',
        height: 50,
        title: ''
    });

    $("#panelSeleccionEstudioDocumentos").panel({
        width: '99%',
        height: 660,
        collapsible: true,
        title: ''
    });       
}

function inicializarBuscadores() {
   
    $('#txtDocumentoBuscar').searchbox({
        searcher: buscarDocumentos,
        menu: '#menuDocumentoBuscar',
        width: 590,
        prompt: prompBusqueda
    });

}

function incializarEventosControles() {
    $('#txtActaFecha').datebox({
        onSelect: function (date) {
            var fechaLarga = formatterDateBoxLarge(date);
            $('#txtActaDescripcion').textbox('setText', 'ACTA DEL ' + fechaLarga);
        }
    });

    $("#btnNuevaActa").click(function () {
        nuevaActa();
    });

    $("#btnEstudioDetalle").linkbutton({
        onClick: function () {
            var estudioSeleccionado = $('#cboSeleccionEstudio').combobox('getValue');
            if (estudioSeleccionado) {
                var parametroIdActa = idActaSeleccionada != -1 ? '&IdActa=' + idActaSeleccionada : '';
                location.href = 'EstudioCargaDatos.aspx?PaginaReturn=ActaCargaDocumentosV2&IdEstudio=' + estudioSeleccionado + parametroIdActa;
            }
        }
    });
    $("#btnNuevoEstudio").linkbutton({
        onClick: function () {
            var parametroIdActa = idActaSeleccionada != -1 ? '&IdActa=' + idActaSeleccionada : '';
            location.href = 'EstudioCargaDatos.aspx?PaginaReturn=ActaCargaDocumentosV2&IdEstudio=-1' + parametroIdActa;
        }
    });

    $("#bntMostrarEstudioSeleccionado").linkbutton({
        onClick: function () {
            obtenerEstudioSeleccionado();
        }
    });

    $("#btnNuevoDocumento").linkbutton({
        onClick: function () {
            mostrarDatosDocumento(-1, null);
        }
    });


    $("#btnAgregarDocumentosActa").click(function () {
        actualizarActaDocumentos();
    });

    $("#btnExpandir").click(function () {
        $('#dgDocumentos').datagrid('expandGroup');
    });

    $("#btnContraer").click(function () {
        $('#dgDocumentos').datagrid('collapseGroup');
    });

    $("#btnGuardarNuevaActa").click(function () {
        guardarActa();
    });

    $("#btnCancelarNuevaActa").click(function () {
        cancelarNuevaActa();
    });

    $("#btnGuardarDocumento").click(function () {
        guardarDocumento();
    });

    $("#btnCancelarNuevoDocumento").click(function () {
        cancelarNuevoDocumento();
    });

    $("#btnActaDocumentoComentarioControl_Guardar").click(function () {
        guardarActaDocumentoComentario();
    });

    $("#btnActaDocumentoComentarioControl_Cancelar").click(function () {
        cancelarActaDocumentoComentario();
    });

    $("#btnMotrarNotasEstudioSeleccionado").click(function () {
        mostrarNotasEstudioSeleccionado();
    });

    $("#btnActaNotasEstudio_Guardar").click(function () {
        actaNotasEstudioGuardar();
    });

    $("#btnActaNotasEstudio_Cancelar").click(function () {
        $('#dlgActaNotasEstudio').dialog('close');
    });

    $('#chkSoloPendientes').change(function () {
        obtenerEstudioSeleccionado();
    });
}

function configurarGrillas() {

    $('#dgDocumentos').datagrid({
        title: '',
        width: '99%',
        height: 540,
        autoRowHeight: true,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: false,
        remoteSort: true,
        nowrap: false,
        view: groupview,
        groupField: 'TipoDocumentoIdDescripcion',
        groupFormatter: function (value, rows) {
            var id = value.substring(0, value.indexOf('-'));
            var descripcion = value.substring(value.indexOf('-') + 1);
            var valorFormateado = '<span>' + descripcion + '</span> - ' + rows.length + (rows.length == 1 ? ' documento' : ' documentos');
            var htmlReturn = '<div style=" width: 600px;">'
            htmlReturn += '<div style="width: 550px; float: left;">' + valorFormateado + '</div>';
            htmlReturn += '<div style="text-align: center"><img src="css/icons/edit_add.png" style="cursor: pointer" onClick="mostrarDatosDocumento(-1, ' + id + ')" /></div>';
            htmlReturn += '</div>';

            return htmlReturn;
        },
        rowStyler: function(index,row){
            if (row.VersionActualActa > 0) {
                return 'font-style: italic; font-weight:bold; color: #3629F8;';
            }
        },
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'Check', checkbox: true },
                { field: 'TipoDocumentoIdDescripcion', title: 'IdTipo', hidden: true },
                { field: 'TipoDocumentoDescripcion', title: 'Tipo', hidden: true },
                { field: 'Descripcion', title: 'Nombre Documento', sortable: true, width: '50%' },
		        { field: 'VersionActualDescripcion', title: 'Versión', sortable: true, width: '20%' },
                { field: 'EstadoActual', title: 'Estado', sortable: true, width: '20%' },
                { field: 'Boton', title: '', width: '5%', align: 'center',
                    formatter: function (value, rowData, index) {
                        var linkNuevaVersion = '';
                        if (rowData.TipoDocumentoRequiereVersion) linkNuevaVersion = '<img src="css/icons/edit_add.png" style="cursor: pointer" />';
                        return linkNuevaVersion;
                    }
                }
        ]],
        onClickCell: function (rowIndex, field, value) {
            if (field == 'Boton')
                mostrarDatosDocumento(rowIndex, null);
        },
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                var idVersionActual = -1;
                if (rowData.VersionActual != null)
                    idVersionActual = rowData.VersionActual.Id;

                var parametroIdActa = idActaSeleccionada != -1 ? '&IdActa=' + idActaSeleccionada : '';
                $('#btnDocumentoDetalle').attr("href", "DocumentoDatos.aspx?PaginaReturn=ActaCargaDocumentosV2"
                                                                        + "&IdEstudio=" + rowData.IdEstudio.toString()
                                                                        + "&IdDocumento=" + rowData.Id.toString()
                                                                        + "&IdVersion=" + idVersionActual.toString()
                                                                        + parametroIdActa);

                
                               
            }
        },
        onDblClickRow: function (rowIndex, rowData) {
            var idVersionActual = -1;
            if (rowData.VersionActual != null)
                idVersionActual = rowData.VersionActual.Id;

            location.href = "DocumentoDatos.aspx?PaginaReturn=ActaCargaDocumentos"
                                            + "&IdEstudio=" + rowData.IdEstudio.toString()
                                            + "&IdDocumento=" + rowData.Id.toString()
                                            + "&IdVersion=" + idVersionActual.toString();
        },
        onLoadSuccess: function (data) {
            //$('#dgDocumentos').datagrid('collapseGroup');
        }
    });
    //$('#dgDocumentos').datagrid('enableFilter');


    $('#dgParticipantes').datagrid({
        title: '',
        width: '100%',
        height: 150,
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
                { field: 'NombreCompleto', title: 'Profesionales', width: '90%' }
        ]]
    });

}


function cargarCboEstudios() {
    $('#cboSeleccionEstudio').combobox({
        url: 'handlers/BandejaInicioHandler.ashx?accion=LISTAR-NOFINALIZADOS',
        valueField: 'Id',
        textField: 'NombreEstudioListados',
        panelWidth: 700,
        panelHeight: 400,
        formatter: formatItemEstudio,
        onSelect: function (row) {            
            $('#txtEsudioNombreCompleto').textbox('setText', row.NombreCompleto);
        },
        onLoadSuccess: function () {
            var URLParamIdEstudio = getURLParam("IdEstudio");
            if (URLParamIdEstudio && URLParamIdEstudio != -1) {
                idEstudioSeleccionado = URLParamIdEstudio;
                $('#cboSeleccionEstudio').combobox('select', idEstudioSeleccionado);
                obtenerEstudioSeleccionado();
            }
        }
    });
}

function formatItemEstudio(row) {
    var s = '<span style="font-weight:bold">' + row.NombreEstudioListados + '</span><br/>' +
            '<span style="color:#888">' + row.NombreCompleto + '</span>';
    return s;
}

function cargarCboTipoDocumento() {
    //Si esta vacio se carga el combo
    var items = $('#cboTipoDocumento').combobox('getData');
    if (items.length == 0) {
        $('#cboTipoDocumento').combobox({
            url: 'handlers/TiposDocumentoHandler.ashx?accion=LISTAR',
            valueField: 'Id',
            textField: 'Descripcion',
            panelHeight: 350,
            onSelect: function (row) {
                cboTipoDocumento_onSelect(row);
            },
            onLoadSuccess: function () {
                cargarCboEstudios();
            }
        });
    } else {
        cargarCboEstudios();
    }

}

function cboTipoDocumento_onSelect(row) {
    if (row) {
        $('#txtNombreDocumento').val(row.Descripcion);
        obtenerPrimerEstadoVersion(row.Id);

        documentoRequiereVersion = (row.RequiereVersion && row.RequiereVersion == true) ? true : false;
        if (documentoRequiereVersion)
            $('#panelVersionControl').show();
        else
            $('#panelVersionControl').hide();
    }
}

function cargarCboActas() {
    //Si esta vacio se carga el combo
    var items = $('#cboSeleccionActa').combobox('getData');
    if (items.length == 0) {
        $('#cboSeleccionActa').combobox({
            url: 'handlers/ActasHandler.ashx?accion=LISTAR-ACTAS&cerrada=false&orden=2',
            valueField: 'Id',
            textField: 'Descripcion',
            panelHeight: 350,
            onSelect: function (rec) {
                idActaSeleccionada = rec.Id;
                
                var parametroIdEstudio = idEstudioSeleccionado!=-1?'&IdEstudio=' + idEstudioSeleccionado:'';
                $('#btnActaDetalle').attr('href', 'ActaDatosV2.aspx?PaginaReturn=ActaCargaDocumentosv2'
                                                               + '&IdActa=' + idActaSeleccionada
                                                               + parametroIdEstudio);
            },
            onLoadSuccess: function () {
                var URLParamIdActa = getURLParam("IdActa");
                if (URLParamIdActa && URLParamIdActa != -1) {
                    $('#cboSeleccionActa').combobox('select', URLParamIdActa);                    
                } else {
                    var dataCbo = $('#cboSeleccionActa').combobox('getData');
                    if (dataCbo.length > 0) {
                        $('#cboSeleccionActa').combobox('select', dataCbo[0].Id);                        
                    }
                }
                cargarCboTipoDocumento();
            },
            onChange: function (newValue, oldValue) {
                idEstudioSeleccionado = -1;
                $('#cboSeleccionEstudio').combobox('clear');
                $('#dgDocumentos').datagrid('loadData', []);
            }
        });
    }
}


function buscarDocumentos(value, name) {
    if (value == '') {
        obtenerEstudioSeleccionado();
    }
    else if (value != prompBusqueda) {
    
        if (idEstudioSeleccionado != -1) {
    
            var parametros;
            parametros = {
                accion: 'BUSCAR-DOCUMENTOS-DEL-ESTUDIO',
                idEstudio: idEstudioSeleccionado,
                tipoDocumento: name == 'TipoDocumento' ? value : '',
                nombreDocumento: name == 'NombreDocumento' ? value : ''
            };

            controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros,
                    function (data) {
                        if (data && data.Documentos)
                            $('#dgDocumentos').datagrid('loadData', data.Documentos);
                    });
        }
    }
}

function obtenerEstudioSeleccionado() {
    var estudioSeleccionado = $('#cboSeleccionEstudio').combobox('getValue');
    investigadoresEstudio = [];
    if (estudioSeleccionado) {

        idEstudioSeleccionado = estudioSeleccionado;
        
        var parametros;
        parametros = {
            accion: 'OBTENER-ESTUDIO',
            idEstudio: idEstudioSeleccionado,
            idActa: idActaSeleccionada,
            soloPendientes: $("#chkSoloPendientes").is(':checked')
        };

        controladorAJAX_GET('handlers/BandejaInicioHandler.ashx', parametros,
            function (data) {
                if (data) {
                    if (data.Documentos) {
                        $('#dgDocumentos').datagrid('loadData', data.Documentos);
                        if (data.InvestigadoresPrincipalesProfesional) {
                            investigadoresEstudio = data.InvestigadoresPrincipalesProfesional;
                        }
                    }                    
                }
            });
    }
}

function nuevaActa() {
    $('#txtActaFecha').datebox('setValue', '');
    $('#txtActaHora').timespinner('setValue', '08:00');
    $('#txtActaDescripcion').val('');
    $('#dgActaEstudios').datagrid('loadData', []);
    $('#dlgNuevaActa').dialog("open");
}

function guardarActa() {

    var parametros = {
        accion: 'GRABAR-ACTA',
        idActa: -1,
        descripion: $('#txtActaDescripcion').textbox('getText'),
        fecha: $('#txtActaFecha').datebox('getValue'),
        hora: $('#txtActaHora').timespinner('getValue')
    };
    controladorAJAX_POST('handlers/ActasHandler.ashx', parametros,
        function (data) {
            if (data.result == 'OK') {
                location.href = 'ActaCargaDocumentosV2.aspx?IdActa=' + data.id;
            } else {
                $.messager.alert('Error', data.message, 'error');
            }
        });
}

function cancelarNuevaActa() {
    $('#dlgNuevaActa').dialog("close");
}


function guardarDocumento() {

    if (idEstudioSeleccionado != -1) {

        var parametros = {
            accion: 'GRABAR-DOCUMENTO-Y-VERSION',
            idEstudio: idEstudioSeleccionado,
            idDocumento: idDocumentoSeleccionado,
            descripcion: $('#txtNombreDocumento').val(),
            idTipoDocumento: $('#cboTipoDocumento').combobox('getValue'),
            idVersion: -1,
            versionDescripcion: formatearCaracteres($('#txtVersionDescripcion').val()),
            versionFecha: $('#txtVersionFecha').datebox('getValue'),
            versionAprobadoANMAT: $('#chkVersionAprobadoANMAT').is(':checked'),
            versionFechaAprobadoANMAT: $('#txtVersionFechaAprobadoANMAT').datebox('getValue'),
            versionFechaEstado: $('#txtVersionFechaEstado').datebox('getValue'),
            versionEstado: $('#cboVersionEstado').combobox('getValue'),
            participantes: obtenerParticipantesSeleccionados(),
            requiereVersion: documentoRequiereVersion
        };

        controladorAJAX_POST('handlers/DocumentoDatosHandler.ashx', parametros,
            function (data) {
                if (data.result == 'OK') {
                    idDocumentoSeleccionado = data.id;
                    obtenerEstudioSeleccionado();
                    
                    $('#dlgNuevoDocumento').dialog("close");
                } else {
                    $.messager.alert('Error', data.message, 'error');
                }
            });
    }
}

function cargarTiposDocumentoItems() {
    if (!tiposDocumentosItems) {
        var parametros = { accion: 'LISTAR' };

        controladorAJAX_GET('handlers/TiposDocumentoHandler.ashx', parametros,
                function (data) {
                    if (data.result == 'Error') {
                        $.messager.alert('Error', data.message, 'error');
                    } else if (data) {
                        tiposDocumentosItems = data;
                    }
                });
    }    
}

function obtenerTipoDocumento(tipoDocumentoDescripcion) {
    var idTipoDocumentoReturn = -1;
    if (tiposDocumentosItems) {
        $.each(tiposDocumentosItems, function (index, value) {
            if (value.Descripcion == tipoDocumentoDescripcion) {
                idTipoDocumentoReturn = value.IdTipoDocumento;
                return false;
            }
        });
    }
    return idTipoDocumentoReturn;
}

function cancelarNuevoDocumento() {
    $('#dlgNuevoDocumento').dialog("close");
}

function mostrarDatosDocumento(rowIndex, tipoDocumentoSeleccionar) {
    if (idEstudioSeleccionado != -1) {
        
        $("#panelVersionControl").show();
        $('#panelVersionDocumento').hide();
        $("#panelVersionEstado").show();
        $("#txtEstudio").attr('disabled', 'disabled');

        limpiarControlesVersion();
		
        
        $('#dgParticipantes').datagrid('loadData', investigadoresEstudio);

        $('#txtEstudio').textbox('setText', $('#cboSeleccionEstudio').combobox('getText'));
        
        var documentoSeleccionado;
        var tituloDialogo = '';
        if (rowIndex != -1) {
            tituloDialogo = 'Nueva Versión del Documento';
            
            documentoSeleccionado = $('#dgDocumentos').datagrid('getRows')[rowIndex];
            idDocumentoSeleccionado = documentoSeleccionado.Id;

            $('#txtNombreDocumento').val(documentoSeleccionado.Descripcion);
            $("#txtNombreDocumento").attr('disabled', true);
            $('#cboTipoDocumento').combobox('select', documentoSeleccionado.IdTipoDocumento);
            $('#cboTipoDocumento').combobox('disable');

            documentoRequiereVersion = documentoSeleccionado.TipoDocumentoRequiereVersion;
            obtenerPrimerEstadoVersion(documentoSeleccionado.IdTipoDocumento);

            var rows = $('#dgParticipantes').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                
                var versionParticipantes = documentoSeleccionado.Participantes;
                for (var j = 0; j < versionParticipantes.length; j++) {

                    if (rows[i].Id == versionParticipantes[j].Id) {
                        $('#dgParticipantes').datagrid('checkRow', i);
                        break;
                    }
                }
            }

            $('#txtVersionFecha').focus();

        } else {
            tituloDialogo = 'Nuevo Documento';

            idDocumentoSeleccionado = -1;

            $('#txtNombreDocumento').val('');
            $("#txtNombreDocumento").removeAttr('disabled', 'disabled');
            if (tipoDocumentoSeleccionar) {
                $('#cboTipoDocumento').combobox('select', tipoDocumentoSeleccionar);
            } else {
                $('#cboTipoDocumento').combobox('select', '');
            }
           
            $('#cboTipoDocumento').combobox('enable');

            var rows = $('#dgParticipantes').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                $('#dgParticipantes').datagrid('checkRow', i);
            }

            $('#cboTipoDocumento').focus();
        }
        
        $('#dlgNuevoDocumento').dialog({
            title: tituloDialogo,
            width: 900,
            height: 500,
            closed: true,
            modal: true
        });
        $('#dlgNuevoDocumento').dialog('center');
        $('#dlgNuevoDocumento').dialog('open');

    }
}

function obtenerPrimerEstadoVersion(idTipoDocumento) {

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

function limpiarControlesVersion() {
    idDocumentoSeleccionado = -1;
    $('#txtVersionDocumento').val('');
    $('#txtVersionFecha').datebox('setValue', '');
    $('#txtVersionDescripcion').val('');
    $('#txtVersionArchivo').val('');
    $("#chkVersionAprobadoANMAT").attr('checked', false);
    $('#txtVersionFechaAprobadoANMAT').datebox('setValue', '');

    $('#cboVersionEstado').val(''); //.combobox('setValue', '');
    $('#txtVersionFechaEstado').datebox('setValue', getFechaActual());
}

function agregarDocumentosActa() {
    $('#dlgVersion').dialog("close");
}

function actualizarActaDocumentos() {

    if (idActaSeleccionada != -1) {
        var documentos = new Array();

        var documentosSeleccionados = $('#dgDocumentos').datagrid('getSelections');
        var index = 0;
        if (documentosSeleccionados) {
            for (var i = 0; i < documentosSeleccionados.length; i++) {
                if (    documentosSeleccionados[i].VersionActualActa == 0
                    &&  documentosSeleccionados[i].IdVersionActual != -1) {

                    documentos[index] = new Object();
                    documentos[index].idActaDocumento = -1;
                    documentos[index].idEstudio = idEstudioSeleccionado;
                    documentos[index].idDocumento = documentosSeleccionados[i].Id;
                    documentos[index].idDocumentoVersion = documentosSeleccionados[i].IdVersionActual;
                    documentos[index].comentario = '';
                    index++;

                }
            }
        }

        var parametros = {
            accion: 'GRABAR-ACTADOCUMENTOS',
            idActa: idActaSeleccionada,
            documentos: JSON.stringify(documentos),
            idEstudio: idEstudioSeleccionado
        };

        controladorAJAX_POST('handlers/ActasHandler.ashx', parametros,
            function (data) {
                if (data.result == 'Error') {
                    $.messager.alert('Error', data.message, 'error');
                } else {
                    $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                    $('#dgDocumentos').datagrid('uncheckAll');
                    obtenerEstudioSeleccionado();
                }
            });
    }
}

function obtenerDocumentosActa() {
    if (idActaSeleccionada != -1) {

        var parametros = {
            accion: 'OBTENER',
            idActa: idActaSeleccionada
        };

        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros,
            function (data) {
                if (data.result == 'Error') {
                    $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
                } else {
                    if (data) $('#dgActaEstudios').datagrid('loadData', data.Documentos);
                }
            });
    }
}

function quitarDocumento(rowIndex) {
    var actaEstudioSeleccionado = $('#dgActaEstudios').datagrid('getRows')[rowIndex];
    if (actaEstudioSeleccionado) {

        $.messager.confirm('Confirmar', '¿Desea quitar el documento ' + actaEstudioSeleccionado.NombreEstudio + ' - ' + actaEstudioSeleccionado.NombreDocumento + '?',
            function (r) {
                if (r) {
                    var parametros = {
                        accion: 'ELIMINAR-ACTADOCUMENTO',
                        idActa: idActaSeleccionada,
                        idActaDocumento: actaEstudioSeleccionado.Id
                    };

                    controladorAJAX_GET('handlers/ActasHandler.ashx', parametros,
                        function (data) {
                            if (data.result == 'OK') {
                                obtenerDocumentosActa();
                            } else {
                                $.messager.alert('Error', data.message, 'error');
                            }
                        });
                }
            });
    }
}

function ingresarComentarioActa(rowIndex) {
    var actaEstudioSeleccionado = $('#dgActaEstudios').datagrid('getRows')[rowIndex];
    if (actaEstudioSeleccionado) {
        $('#txtActaDocumentoControlComentario').val(actaEstudioSeleccionado.ComentarioDocumento);

        $('#dlgActaDocumentoComentarioControl').dialog('setTitle', 'Comentario del Acta');
        $('#dlgActaDocumentoComentarioControl').dialog('center');
        $('#dlgActaDocumentoComentarioControl').dialog('open');
    }
}

function guardarActaDocumentoComentario() {

    var documentosJSON = new Array();
    var actaDocumento = $('#dgActaEstudios').datagrid('getSelected');
    var index = 0;
    if (actaDocumento) {

        actaDocumento.ComentarioDocumento = formatearTextoASCII($('#txtActaDocumentoControlComentario').val()); 

        documentosJSON[index] = new Object();
        documentosJSON[index].idActaDocumento = actaDocumento.Id;
        documentosJSON[index].idDocumento = actaDocumento.IdDocumento;
        documentosJSON[index].idDocumentoVersion = actaDocumento.IdVersion;
        documentosJSON[index].comentario = formatearTextoASCII($('#txtActaDocumentoControlComentario').val());
        documentosJSON[index].idEstadoDocumento = actaDocumento.IdEstadoActual;
        documentosJSON[index].actualizarEstadoFinal = false;

        var parametros = {
            accion: 'GRABAR-ACTADOCUMENTOCOMENTARIOESTADO',
            idActa: idActaSeleccionada,
            documentos: JSON.stringify(documentosJSON)
        };

        controladorAJAX_POST('handlers/ActasHandler.ashx', parametros,
            function (data) {
                if (data.result == 'OK') {
                    //obtenerDocumentosActa();
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

function obtenerParticipantesSeleccionados() {
    var profSeleccionados = '';

    var rows = $('#dgParticipantes').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].Profesional)
            profSeleccionados = profSeleccionados + ';' + rows[i].Profesional.Id;
        else
            profSeleccionados = profSeleccionados + ';' + rows[i].Id;
    }

    return profSeleccionados;
}

function mostrarNotasEstudioSeleccionado() {

    idNotaTratadaAntes = -1;
    $('#txtNotaEstudioAntesDocumentos').textbox('clear');

    idNotaTratadaDespues = -1;
    $('#txtNotaEstudioDespuesDocumentos').textbox('clear');

    var estudioSeleccionado = $('#cboSeleccionEstudio').combobox('getValue');

    if (idActaSeleccionada != -1 && estudioSeleccionado) {

        $('#txtNotaEstudioNombre').textbox('setText', $('#cboSeleccionEstudio').combobox('getText'));

        var parametros;
        parametros = {
            accion: 'OBTENER-ACTA_NOTASESTUDIO',
            idActa: idActaSeleccionada,
            idEstudio: estudioSeleccionado
        };

        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros,
            function (data) {
                if (data) {

                    $.each(data, function (i, valor) {
                        if (!valor.ActaImprimeAlFinal) {
                            idNotaTratadaAntes = valor.Id;
                            $('#txtNotaEstudioAntesDocumentos').textbox('setText', formatearTextoASCII(valor.Texto));
                        } else {
                            idNotaTratadaDespues = valor.Id;
                            $('#txtNotaEstudioDespuesDocumentos').textbox('setText', formatearTextoASCII(valor.Texto));
                        }
                    });

                    $('#dlgActaNotasEstudio').dialog({
                        title: 'Notas del estudio seleccionado',
                        width: 800,
                        height: 600,
                        closed: true,
                        modal: true
                    });
                    $('#dlgActaNotasEstudio').dialog('center');
                    $('#dlgActaNotasEstudio').dialog('open');
                }
            });
    }
}

function actaNotasEstudioGuardar() {

        
    var descripcionNota = $('#cboSeleccionActa').combobox('getText') + ' - Estudio ' + $('#cboSeleccionEstudio').combobox('getText');
    var notasJSON = new Array();
    var index = 0;

    //NOTA ANTES DE DOCUMENTOS TRATADOS
    notasJSON[index] = new Object();
    notasJSON[index].imprimeAlFinal = 0;
    notasJSON[index].idNota = idNotaTratadaAntes;
    notasJSON[index].idEstudio = $('#cboSeleccionEstudio').combobox('getValue');
    notasJSON[index].descripcion = descripcionNota + ' - INICIO';
    notasJSON[index].fecha = getFechaActual();
    notasJSON[index].idAutor = -1;
    notasJSON[index].requiereRespuesta = false;
    notasJSON[index].idDocumento = -1;
    notasJSON[index].idDocumentoVersion = -1;
    notasJSON[index].texto = $('#txtNotaEstudioAntesDocumentos').textbox('getText');

    //NOTA A CONTINUACIÓN DE DOCUMENTOS TRATADOS
    index++;
    notasJSON[index] = new Object();
    notasJSON[index].imprimeAlFinal = 1;
    notasJSON[index].idNota = idNotaTratadaDespues;
    notasJSON[index].idEstudio = $('#cboSeleccionEstudio').combobox('getValue');
    notasJSON[index].descripcion = descripcionNota + ' - FINAL';
    notasJSON[index].fecha = getFechaActual();
    notasJSON[index].idAutor = -1;
    notasJSON[index].requiereRespuesta = false;
    notasJSON[index].idDocumento = -1;
    notasJSON[index].idDocumentoVersion = -1;
    notasJSON[index].texto = $('#txtNotaEstudioDespuesDocumentos').textbox('getText');

    var parametros = {
        accion: 'GRABAR-ACTANUEVANOTATRATADA',
        idActa: $('#cboSeleccionActa').combobox('getValue'),
        notas: JSON.stringify(notasJSON)
    };

    controladorAJAX_POST('handlers/ActasHandler.ashx', parametros, function (data) {
        if (data.result == 'OK') {
            $('#dlgActaNotasEstudio').dialog('close');
        } else {
            $.messager.alert('Error', data.message, 'error');
        }
    });

}