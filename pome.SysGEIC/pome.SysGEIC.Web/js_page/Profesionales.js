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
        title: 'Información de Profesionales',
        width: 1198,
        height: 280,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Apellido',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Código', sortable: true, width: 60 },
                { field: 'Titulo', title: 'Título', sortable: true, width: 70 },
		    { field: 'Apellido', title: 'Apellido', sortable: true, width: 300 },
		    { field: 'Nombre', title: 'Nombre', sortable: true, width: 300 },
                { field: 'TipoProfesional', title: 'Tipo', sortable: true, width: 300,
                    formatter: function (value, row, index) {
                        if (row.TipoProfesional) return row.TipoProfesional.Descripcion;
                        else return value;
                    }
                },
                { field: 'OrdenActa', title: 'Orden en Actas', sortable: true, width: 100 }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });

}

function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/ProfesionalesHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/ProfesionalesHandler.ashx', dataParametros, funcionOK);
}

function cargarGrilla() {
    var parametros;
    parametros = {
        accion: 'LISTAR',
        apellido: $('#txtApellidoBuscar').val(),
        nombre: $('#txtNombreBuscar').val(),
        tipoProfesional: $('#cboTipoProfesionalBuscar').combobox('getValue')
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
    limpiarCuadros();

    invocarControlador(parametros, function (data) {
        $('#txtTitulo').val(data.Titulo);
        $('#txtApellido').val(data.Apellido);
        $('#txtNombre').val(data.Nombre);
        $('#cboTipoProfesional').combobox('setValue', data.TipoProfesional.Id);
        if (data.RolComite != null)
            $('#cboRolComite').combobox('setValue', data.RolComite.Id);
        $('#txtOrdenActa').val(data.OrdenActa);
        idSeleccionado = id;
    });
}

function nuevo() {
    idSeleccionado = -1;
    limpiarCuadros();
}

function limpiarCuadros() {
    $('#txtTitulo').val('');
    $('#txtApellido').val('');
    $('#txtNombre').val('');
    $('#cboTipoProfesional').combobox('setValue', '');
    $('#cboRolComite').combobox('setValue', '');
    $('#txtOrdenActa').val('');
    $('#txtApellido').focus();
}

function guardar() {
    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idSeleccionado.toString(),
        apellido: $('#txtApellido').val(),
        nombre: $('#txtNombre').val(),
        idTipoProfesional: $('#cboTipoProfesional').combobox('getValue'),
        idRolComite: $('#cboRolComite').combobox('getValue'),
        titulo: $('#txtTitulo').val(),
        ordenActa: $('#txtOrdenActa').val()
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
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.NombreCompleto + '?', function (r) {
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