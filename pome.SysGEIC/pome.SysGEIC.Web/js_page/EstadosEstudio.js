var idSeleccionado;
var ESTADO_APROBADO = 2;
var DOCUMENTOS_APROBADOS = 7;

$(document).ready(function () {

    configurarGrilla();
    cargarGrilla();

    $('#dg').datagrid('selectRow', 1)
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrar(row.Id);

    $("#btnBuscar").click(function () {
        cargarGrilla();
    });
});

function configurarGrilla() {
    $('#dg').datagrid({
        title: 'Información Estados de Estudios',
        width: '100%',
        height: 400,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Código', width: '10%' },
		    { field: 'Descripcion', title: 'Descripción', width: '70%' },
		    { field: 'Final', title: 'Estado Final', width: '20%',
		        formatter: function (value, row, index) {
		            if (row.Final) return 'SI';
		            else return 'NO';
		        } 
		    }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });

}

function cargarGrilla() {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        descripcion: $('#txtDescripcionBuscar').val()
    };

    $.ajax({
        type: 'GET',
        url: 'handlers/EstadosEstudioHandler.ashx',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            $('#dg').datagrid('loadData', data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.messager.alert('Error', textStatus + ' ' + errorThrown, 'error');
        }
    });
}

function mostrar(id) {
    $.ajax({
        type: 'GET',
        url: 'handlers/EstadosEstudioHandler.ashx',
        data: 'accion=OBTENER&id=' + id,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            $('#txtDescripcion').val(data.Descripcion);
            $('#txtDescripcion').attr('disabled', (id == ESTADO_APROBADO || id == DOCUMENTOS_APROBADOS));
            $("#chkFinal").prop('checked', data.Final);
            idSeleccionado = id;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.messager.alert('Error', textStatus + ' ' + errorThrown, 'error');
        }
    });
}

function nuevo() {
    idSeleccionado = -1;
    $('#txtDescripcion').val('');
    $("#chkFinal").prop('checked', false);
    $('#txtDescripcion').focus();
}

function guardar() {
    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idSeleccionado.toString(),
        descripcion: $('#txtDescripcion').val(),
        final: $("#chkFinal").is(':checked')
    };
    actualizarDatos(parametros);
}

function cancelar() {
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrar(row.Id);
}

function eliminar() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        if (row.Id == ESTADO_APROBADO || row.Id == DOCUMENTOS_APROBADOS) {
            $.messager.alert('Estados', 'El estado seleccionado no se puede eliminar', 'info');
        }
        else {
            $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
                if (r) {
                    actualizarDatos({ accion: 'ELIMINAR', id: row.Id });
                }
            });
        }
    }
}

function actualizarDatos(dataParametros) {

    $.ajax({
        type: 'GET',
        url: 'handlers/EstadosEstudioHandler.ashx',
        data: dataParametros,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.result == 'OK') {
                $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                cargarGrilla();
            } else {
                $.messager.alert('Error', data.message, 'error');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.messager.alert('Error', textStatus + ' ' + errorThrown, 'error');
        }
    });
}