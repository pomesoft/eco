var objSeleccionado;

$(document).ready(function () {

    configurarGrilla();
    configurarGrillaIntegrantes();
    cargarGrilla();

    $('#dg').datagrid('selectRow', 0)
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrar(row.Id);

    $("#btnBuscar").click(function () {
        cargarGrilla();
    });

});

function configurarGrilla() {
    $('#dg').datagrid({
        title: 'Información de Equipos de Investigación',
        width: 599,
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
		    { field: 'Id', title: 'Código', width: 60 },
		    { field: 'Descripcion', title: 'Equipo', width: 535 }
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
        url: 'handlers/EquiposHandler.ashx',
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
        url: 'handlers/EquiposHandler.ashx',
        data: 'accion=OBTENER&id=' + id,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            $('#txtDescripcion').val(data.Descripcion);
            $('#dgIntegrantes').datagrid('loadData', data.Integrantes);
            objSeleccionado = data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.messager.alert('Error', textStatus + ' ' + errorThrown, 'error');
        }
    });
}

function nuevo() {
    objSeleccionado = null;
    $('#txtDescripcion').val('');
    $('#txtDescripcion').focus();
}

function guardar() {
    var datos = {
        id: objSeleccionado.Id.toString(),
        descripcion: $('#txtDescripcion').val()
    }
    var parametros = {
        accion: 'GRABAR',
        equipo: JSON.stringify(datos)
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
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
            if (r) {
                actualizarDatos({ accion: 'ELIMINAR', id: row.Id });
            }
        });
    }
}

function actualizarDatos(dataParametros) {

    $.ajax({
        type: 'GET',
        url: 'handlers/EquiposHandler.ashx',
        data: dataParametros,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            if (data.result == 'OK') {
                $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
                cargarGrilla();

                if (objSeleccionado != null)
                    $('#dg').datagrid('selectRecord', objSeleccionado.Id.toString());

            } else {
                $.messager.alert('Error', data.message, 'error');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $.messager.alert('Error', textStatus + ' ' + errorThrown, 'error');
        }
    });
}

//***************INTEGRANTES*********************************************************

function configurarGrillaIntegrantes() {
    $('#dgIntegrantes').datagrid({
        title: 'Integrantes del Equipo',
        width: 595,
        height: 400,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Codigo',
        sortOrder: 'asc',
        remoteSort: false,
        idField: 'Id',
        columns: [[
		    { field: 'Rol', title: 'Rol', width: 190, formatter: rolFormatter },
                { field: 'Profesional', title: 'Profesional', width: 400, formatter: profesionalFormatter }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            var row = $('#dgIntegrantes').datagrid('getSelected');
            if (row) mostrarIntegrante(row);
        }
    });
}

function rolFormatter(value) {
    return value.Descripcion;
}

function profesionalFormatter(value) {
    return value.NombreCompleto;
}

function mostrarIntegrante(row) {
    $('#cboProfesional').combobox('setValue', data.Profesional.Id);
    $('#cboRol').combobox('setValue', data.Rol.Id);
}


function agregarIntegrante() {
    
    var parametros = {
        accion: 'AGREGAR_INTEGRANTE',
        id: objSeleccionado.Id.toString(),
        idRol: $('#ContentPlaceBody_cboRol').combobox('getValue'),
        idProfesional: $('#ContentPlaceBody_cboProfesional').combobox('getValue')
    };
    actualizarDatos(parametros);
    $('#ContentPlaceBody_cboRol').combobox('setValue', '');
    $('#ContentPlaceBody_cboProfesional').combobox('setValue', '');

}
