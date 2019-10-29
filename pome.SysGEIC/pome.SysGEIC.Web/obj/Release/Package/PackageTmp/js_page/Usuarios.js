var idUsuario;

$(function () {

    configurarGrilla();
    cargarGrilla();

    $('#dg').datagrid('selectRow', 0);
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrar(row.Id);

    $("#btnBuscar").click(function () {
        cargarGrilla();
    });
});

function configurarGrilla() {
    $('#dg').datagrid({
        title: 'Información de Usuarios',
        width: 590,
        height: 325,
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
		    { field: 'Apellido', title: 'Apellido', width: 210 },
		    { field: 'Nombre', title: 'Nombre', width: 210 },
		    { field: 'LoginUsuario', title: 'Login', width: 150 }
	  ]],
        toolbar: '#dg_toolbar',
        onClickRow: function (rowIndex) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });
}

function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/UsuariosHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/UsuariosHandler.ashx', dataParametros, funcionOK);
}

function cargarGrilla() {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        apellidoBuscar: $('#txtApellidoBuscar').val(),
        nombreBuscar: $('#txtNombreBuscar').val()
    };

    invocarControlador(parametros, function (data) {
        $('#dg').datagrid('loadData', data);

//        if (data.result == 'OK') {
//            $('#dg').datagrid('loadData', data.listado);
//        } else {
//            $.messager.alert('Error', data.message, 'error');
//        }        
    });
}

function mostrar(id) {
    var parametros;
    parametros = {
        accion: 'OBTENER',
        id: id
    };

    invocarControlador(parametros, function (data) {
        $('#txtApellido').val(data.Apellido);
        $('#txtNombre').val(data.Nombre);
        $('#txtLoginUsuario').val(data.LoginUsuario);
        $('#txtEMail').val(data.Mail);
        $('#cboTipoUsuario').combobox('setValue', data.TipoUsuario.Id);

        idUsuario = id;
    });
}

function nuevo() {

    idUsuario = -1;

    $('#txtApellido').val('');
    $('#txtNombre').val('');
    $('#txtLoginUsuario').val('');
    $('#txtEMail').val('');
    $('#cboTipoUsuario').combobox('setValue', '');

    $('#txtApellido').focus();

}

function guardar() {
    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idUsuario.toString(),
        apellido: $('#txtApellido').val(),
        nombre: $('#txtNombre').val(),
        loginUsuario: $('#txtLoginUsuario').val(),
        email: $('#txtEMail').val(),
        tipoUsuario: $('#cboTipoUsuario').combobox('getValue')
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
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Apellido + ' ' + row.Nombre + '?', function (r) {
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