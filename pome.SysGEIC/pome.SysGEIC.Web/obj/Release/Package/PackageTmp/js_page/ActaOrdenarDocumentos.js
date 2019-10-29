var idActa;
var filaEstudioSeleccionada;

$(document).ready(function () {
    idActa = -1;
    filaEstudioSeleccionada = -1;

    configurarGrillas();
    configurarPaneles();
    incializarEventosControles();
    obtenerEstudiosActa();
});

function incializarEventosControles() {
    $("#btnActaGuardar").click(function () {
        guardarOrdenDocumentos();
    });

    $("#btnActaVolver").click(function () {
        volver();
    });
}

function configurarPaneles() {
    $("#panelInfoActa").panel({
        width: 1200,
        height: 70,
        title: 'Acta'
    });

    $("#panelDocumentos").panel({
        width: 1200,
        height: 460,
        title: 'Estudios y Documentos tratados'
    });
}

function configurarGrillas() {
    $('#dgEstudios').datagrid({
        title: '',
        width: 470,
        height: 430,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'OrdenEstudio',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Id', hidden: true },
                { field: 'BotonSubir', title: '', width: 30, align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/up.png" /></a>';
                    }
                },
                { field: 'BotonBajar', title: '', width: 30, align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/down.png" /></a>';
                    }
                },
                { field: 'OrdenEstudio', title: '', sortable: true, width: 30 },
                { field: 'Codigo', title: 'Estudio', width: 200 },
		    { field: 'Descripcion', title: 'Abreviado', width: 200 }
	    ]],
        onSelect: function (rowIndex, rowData) {
            obtenerDocumentosEstudioSeleccionado();
        },
        onClickRow: function (rowIndex, rowData) {
            filaEstudioSeleccionada = rowIndex;
            $('#dgDocumentos').datagrid('selectRow', rowIndex);
        },
        onClickCell: function (rowIndex, field, value) {
            if (field == 'BotonSubir')
                subirEstudio(rowIndex);
            if (field == 'BotonBajar')
                bajarEstudio(rowIndex);
        }
    });

    $('#dgDocumentos').datagrid({
        title: '',
        width: 715,
        height: 430,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'OrdenDocumento',
        sortOrder: 'asc',
        columns: [[
                { field: 'Id', title: 'Código', width: 60, hidden: true },
                { field: 'BotonSubir', title: '', width: 30, align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/up.png" /></a>';
                    }
                },
                { field: 'BotonBajar', title: '', width: 30, align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/down.png" /></a>';
                    }
                },
                { field: 'OrdenEstudio', title: '', width: 30, hidden: false },
                { field: 'OrdenDocumento', title: '', sortable: true, width: 30 },
                { field: 'NombreDocumento', title: 'Documento', width: 480 },
                { field: 'VersionDocumento', title: 'Versión', width: 200 }
	  ]],
        onClickRow: function () {

        },
        onClickCell: function (rowIndex, field, value) {
            if (field == 'BotonSubir')
                subirDocumento(rowIndex);
            if (field == 'BotonBajar')
                bajarDocumento(rowIndex);
        }
    });
}


function obtenerEstudiosActa() {
    var URLParamIdActa= getURLParam("IdActa");
    if (URLParamIdActa && URLParamIdActa != -1)
        idActa = URLParamIdActa;

    if (idActa != -1) {

        var parametros;
        parametros = {
            accion: 'OBTENER-ACTADTO',
            idActa: idActa
        };

        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros, obtenerEstudiosActaSeleccionadaSuccess);
    }
}

function obtenerEstudiosActaSeleccionadaSuccess(data) {
    if (data) {

        $('#txtActaFecha').datebox('setValue', data.Fecha);
        $('#txtActaFecha').datebox('disable');
        $('#txtActaHora').timespinner('setValue', data.Hora);
        $('#txtActaHora').timespinner('disable');
        $('#txtActaDescripcion').val(data.Descripcion);
        $("#txtActaDescripcion").attr('disabled', true);
        $("#chkCerrada").prop('checked', data.Cerrada);
        $("#chkCerrada").attr('disabled', true);

        $('#dgEstudios').datagrid('loadData', data.EstudiosTratados);
        $('#dgDocumentos').datagrid('loadData', []);

        $('#dgEstudios').datagrid('selectRow', 0);
    }
}

function obtenerDocumentosEstudioSeleccionado() {
    var estudioSeleccionado = $('#dgEstudios').datagrid('getSelected');

    if (idActa != -1 && estudioSeleccionado) {
        var parametros = {
            accion: 'OBTENER-ACTAESTUDIODOCUMENTOS',
            idActa: idActa,
            idEstudio: estudioSeleccionado.Id
        };

        controladorAJAX_GET('handlers/ActasHandler.ashx', parametros, function (data) {
            if (data.result != 'Error') {
                $('#dgDocumentos').datagrid('loadData', data);
            }
        });
    }
}



function subirEstudio(rowIndex) {
    if (rowIndex > 0) {

        var filaSeleccionFinal = rowIndex - 1;
        var dataEstudios = $('#dgEstudios').datagrid('getData');
        
        var filaBaja = $('#dgEstudios').datagrid('getRows')[rowIndex - 1];
        filaBaja.OrdenEstudio = filaBaja.OrdenEstudio + 1;

        var filaSube = $('#dgEstudios').datagrid('getRows')[rowIndex];
        filaSube.OrdenEstudio = filaSube.OrdenEstudio - 1;
        
        $('#dgEstudios').datagrid('deleteRow', rowIndex - 1);
        $('#dgEstudios').datagrid('deleteRow', rowIndex - 1);

        $('#dgEstudios').datagrid('insertRow', {
            index: rowIndex - 1,
            row: filaBaja
        });
        $('#dgEstudios').datagrid('insertRow', {
            index: rowIndex - 1,
            row: filaSube
        });
        $('#dgEstudios').datagrid('loadData', dataEstudios);
        $('#dgEstudios').datagrid('selectRow', filaSeleccionFinal);
    }
}

function bajarEstudio(rowIndex) {    
    var cantidadFilas = $('#dgEstudios').datagrid('getRows').length;
    if (rowIndex < cantidadFilas) {
        var filaSeleccionFinal = rowIndex + 1;
        var dataEstudios = $('#dgEstudios').datagrid('getData');

        var filaBaja = $('#dgEstudios').datagrid('getRows')[rowIndex];
        filaBaja.OrdenEstudio = filaBaja.OrdenEstudio + 1;

        var filaSube = $('#dgEstudios').datagrid('getRows')[rowIndex + 1];
        filaSube.OrdenEstudio = filaSube.OrdenEstudio - 1;
        
        $('#dgEstudios').datagrid('deleteRow', rowIndex);
        $('#dgEstudios').datagrid('deleteRow', rowIndex);

        $('#dgEstudios').datagrid('insertRow', {
            index: rowIndex,
            row: filaBaja
        });
        $('#dgEstudios').datagrid('insertRow', {
            index: rowIndex,
            row: filaSube
        });
        $('#dgEstudios').datagrid('loadData', dataEstudios);
        $('#dgEstudios').datagrid('selectRow', filaSeleccionFinal);
    }
}

function subirDocumento(rowIndex) {
    if (rowIndex > 0) {
        var filaSeleccionFinal = rowIndex - 1;
        var filaBaja = $('#dgDocumentos').datagrid('getRows')[rowIndex - 1];
        filaBaja.OrdenDocumento = filaBaja.OrdenDocumento + 1;

        var filaSube = $('#dgDocumentos').datagrid('getRows')[rowIndex];
        filaSube.OrdenDocumento = filaBaja.OrdenDocumento - 1;
        
        $('#dgDocumentos').datagrid('deleteRow', rowIndex - 1);
        $('#dgDocumentos').datagrid('deleteRow', rowIndex - 1);
        
        $('#dgDocumentos').datagrid('insertRow', {
            index: rowIndex - 1,
            row: filaBaja
        });
        $('#dgDocumentos').datagrid('insertRow', {
            index: rowIndex - 1,
            row: filaSube
        });
        $('#dgDocumentos').datagrid('selectRow', filaSeleccionFinal);
    }
}

function bajarDocumento(rowIndex) {
    var cantidadFilas = $('#dgDocumentos').datagrid('getRows').length;
    if (rowIndex < cantidadFilas) {
        var filaSeleccionFinal = rowIndex + 1;
        var filaBaja = $('#dgDocumentos').datagrid('getRows')[rowIndex];
        filaBaja.OrdenDocumento = filaBaja.OrdenDocumento + 1;

        var filaSube = $('#dgDocumentos').datagrid('getRows')[rowIndex + 1];
        filaSube.OrdenDocumento = filaSube.OrdenDocumento - 1;

        $('#dgDocumentos').datagrid('deleteRow', rowIndex);
        $('#dgDocumentos').datagrid('deleteRow', rowIndex);
        
        $('#dgDocumentos').datagrid('insertRow', {
            index: rowIndex,
            row: filaBaja
        });        
        $('#dgDocumentos').datagrid('insertRow', {
            index: rowIndex,
            row: filaSube
        });
        $('#dgDocumentos').datagrid('selectRow', filaSeleccionFinal);
    }
}


function guardarOrdenDocumentos() {    
    
    var index = 0;
    var documentos = new Array();
    var documentosSeleccionados = $('#dgDocumentos').datagrid('getRows');   
    if (documentosSeleccionados) {
        for (var i = 0; i < documentosSeleccionados.length; i++) {
            documentos[index] = new Object();
            documentos[index].IdActaDocumento = documentosSeleccionados[i].Id;
            documentos[index].OrdenDocumento = documentosSeleccionados[i].OrdenDocumento;
            index++;
        }
    }

    index = 0;
    var estudios = new Array();
    var estudiosSeleccionados = $('#dgEstudios').datagrid('getRows');
    if (estudiosSeleccionados) {
        for (var i = 0; i < estudiosSeleccionados.length; i++) {
            estudios[index] = new Object();
            estudios[index].IdEstudio = estudiosSeleccionados[i].Id;
            estudios[index].OrdenEstudio = estudiosSeleccionados[i].OrdenEstudio;
            index++;
        }
    }

    var parametros = {
        accion: 'GRABAR-ORDENDOCUMENTOS',
        idActa: idActa,
        documentos: JSON.stringify(documentos),
        estudios: JSON.stringify(estudios)
    };

    controladorAJAX_POST('handlers/ActasHandler.ashx', parametros,
            function (data) {
                if (data.result == 'OK') {
                    $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                    obtenerEstudiosActa();
                }
                else {
                    $.messager.alert('Error', data.message, 'error');
                }
            });
}

function volver() {
    var idEstudio = getURLParam("IdEstudio");
    var parametroIdEstudio = idEstudio && idEstudio != -1 ? '&IdEstudio=' + idEstudio : '';
    location.href = getPaginaReturn() + '?IdActa=' + idActa + parametroIdEstudio;
}
