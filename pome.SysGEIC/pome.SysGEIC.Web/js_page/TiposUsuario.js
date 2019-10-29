var idSeleccionado;

$(document).ready(function () {

    cargarGrilla();

    $('#dg').datagrid({
        onClickRow: function (rowIndex) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });

    $("#btnBuscar").click(function () {
        cargarGrilla();
    });
});


function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/TiposUsuarioHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/TiposUsuarioHandler.ashx', dataParametros, funcionOK);
}



function cargarGrilla() {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        descripcion: $('#txtDescripcionBuscar').val()
    };

    invocarControlador(parametros, function (data) {
        $('#dg').datagrid('loadData', data);

        $('#dg').datagrid('selectRow', 0)
        var row = $('#dg').datagrid('getSelected');
        if (row) mostrar(row.Id);
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
        //cargarGrillaMenuAcceso();
    });    
}

function cargarGrillaMenuAcceso() {
    var parametros;
    parametros = {
        accion: 'LISTAR_MENUACCESOS',
        id: idSeleccionado
    };

    invocarControlador(parametros, function (data) {
        $('#dgMenuAcceso').datagrid('loadData', data);
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
        descripcion: $('#txtDescripcion').val(),
        permisos: obtenerPermisoSeleccionados()
    };

    invocarControlador_POST(parametros, guardarSucess);
}

function obtenerPermisoSeleccionados() {
    var permisos = '';
    var rows = $('#dgMenuAcceso').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        permisos = permisos + ';' + rows[i].IdMenuPrincipal + ',' + rows[i].IdMenuSecundario + ',' + rows[i].IdNivelAcceso;
    }
    return permisos;
}

function guardarSucess(data) {
    if (data.result == 'OK') {
        $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
        cargarGrilla();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
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
                invocarControlador_POST(parametros, guardarSucess);
            }
        });
    }
}
