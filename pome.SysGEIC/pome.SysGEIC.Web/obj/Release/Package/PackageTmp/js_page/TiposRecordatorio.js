var idSeleccionado;

$(document).ready(function () {

    incializarControlColor('SelectorColor');
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
        title: 'Información Tipos de Recordatorios',
        width: 1198,
        height: 350,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Código', sortable: true, width: 60 },
		    { field: 'Descripcion', title: 'Descripción', sortable: true, width: 670 }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            var row = $('#dg').datagrid('getSelected');
            if (row) mostrar(row.Id);
        }
    });
}

function invocarControlador(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/TiposRecordatorioHandler.ashx', dataParametros, funcionOK);
}

function invocarControlador_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/TiposRecordatorioHandler.ashx', dataParametros, funcionOK);
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
        limpiarControles();
        $('#txtDescripcion').val(data.Descripcion);
        $("#txtDescripcion").attr("disabled", "disabled");
        $("#chkAvisoMail").prop('checked', data.AvisoMail);
        $("#chkAvisoPopup").prop('checked', data.AvisoPopup);
        $("#SelectorColor").spectrum('set', data.Color);
        idSeleccionado = id;
    });
}

function nuevo() {
    idSeleccionado = -1;
    limpiarControles();
}

function limpiarControles() {
    $('#txtDescripcion').val('');
    $("#chkAvisoMail").prop('checked', false);
    $("#chkAvisoPopup").prop('checked', false);
    $("#SelectorColor").spectrum('set', 'white');

    $('#txtDescripcion').focus();
}

function guardar() {

    var datos = new Object();
    datos.Id = idSeleccionado;
    datos.Descripcion = $('#txtDescripcion').val();
    datos.AvisoMail = $("#chkAvisoMail").is(':checked');
    datos.AvisoPopup = $("#chkAvisoPopup").is(':checked');
    var colorSeleccionado = $("#SelectorColor").spectrum('get');
    datos.Color = colorSeleccionado.toHexString();

    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idSeleccionado.toString(),
        datos: JSON.stringify(datos)
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