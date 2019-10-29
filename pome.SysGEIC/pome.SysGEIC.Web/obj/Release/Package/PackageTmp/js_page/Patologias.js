var idSeleccionado;

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
        title: 'Información de Patologías',
        width: 1198,
        height: 300,
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
		    { field: 'Descripcion', title: 'Descripción', width: 670 }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });

}

function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/PatologiasHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/PatologiasHandler.ashx', dataParametros, funcionOK);
}

function cargarGrilla() {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        descripcion: $('#txtDescripcionBuscar').val()
    };

    invocarControlador(parametros, function (data) {
        $('#dg').datagrid('loadData', data);
    });    
}

function mostrar(id) {
    var parametros;
    parametros = {
        accion: 'OBTENER',
        id: id
    };

    invocarControlador(parametros, function (data) {
        $('#txtDescripcion').val(data.Descripcion);
        idSeleccionado = id;
    });
}

function nuevo() {
    idSeleccionado = -1;
    $('#txtDescripcion').val('');
    $('#txtDescripcion').focus();
}

function guardar() {
    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idSeleccionado.toString(),
        descripcion: $('#txtDescripcion').val()
    };
    invocarControlador_POST(parametros, actualizarDatosSuccess);
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
                var parametros;
                parametros = {
                    accion: 'ELIMINAR',
                    id: row.Id
                };
                invocarControlador_POST(parametros, actualizarDatosSuccess);                
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