var idSeleccionado;
var filaListaSeleccionada;
var filaEmailSeleccionada;

$(document).ready(function () {

    filaEmailSeleccionada = -1;
    filaListaSeleccionada = -1;
    idSeleccionado = -1;

    configurarGrillas();
    inicializarEventosControles();
    cargarGrillaListasEmails();

});

function inicializarEventosControles() {
    $("#btnActualizarEmail").click(function () {
        actualizarEmail();
    });
    $("#btnListasNuevo").click(function () {
        nuevaListaEmails();
    });
    $("#btnListasEliminar").click(function () {
        var row = $('#dgListas').datagrid('getRows')[filaListaSeleccionada];
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
            if (r) eliminarListaEmails();
        });

    });
    $("#btnListasGrabar").click(function () {
        grabarListaEmails();
    });
    $("#btnListasCancelar").click(function () {
        cancelarListaEmails();
    });
}

function configurarGrillas() {
    $('#dgListas').datagrid({
        title: 'Listas de correo electrónico',
        width: '100%',
        height: 550,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        showHeader: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Id', title: 'Id', width: 60, hidden: true },
		    { field: 'Descripcion', title: 'Lista', width: 560 }
	  ]],
        onSelect: function (rowIndex, rowData) {
            if (rowData) {
                filaListaSeleccionada = rowIndex;
                idSeleccionado = rowData.Id;
                $('#txtListaDescripcion').val(rowData.Descripcion);
                $('#dgEmails').datagrid('loadData', rowData.Emails);
            }
        }
    });

    $('#dgEmails').datagrid({
        title: 'Correos electrónico de la lista seleccionada',
        width: 590,
        height: 550,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        showHeader: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'Descripcion', title: 'Correo electrónico', width: 560 },
                { field: 'BotonQuitarEmail', title: '', width: 30, align: 'center',
                    formatter: function (value, rowData, index) {
                        return '<a href="#"><img src="css/icons/cancel.png"/></a>';
                    }
                }
	  ]],
        onClickCell: function (rowIndex, field, value) {
            if (field == 'BotonQuitarEmail') {
                eliminarEmail(rowIndex);
            } else {
                $('#txtEmail').val(value);
                filaEmailSeleccionada = rowIndex;
            }
        }
    });
}

function invocarControladorListasEmails(dataParametros, funcionOK) {
    controladorAJAX_GET('handlers/ListasEmailsHandler.ashx', dataParametros, funcionOK);
}

function invocarControladorListasEmails_POST(dataParametros, funcionOK) {
    controladorAJAX_POST('handlers/ListasEmailsHandler.ashx', dataParametros, funcionOK);
}

function cargarGrillaListasEmails() {
    var parametros;
    parametros = {
        accion: 'LISTAR-LISTASEMAILS'
    };

    invocarControladorListasEmails(parametros, function (data) {
        if (data) {
            if (data.result == 'Error') {
                $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
            }
            else {
                $('#dgListas').datagrid('loadData', data);
                $('#dgListas').datagrid('selectRow', (filaListaSeleccionada == -1 ? 0 : filaListaSeleccionada));
            }
        }
    });
}

function nuevaListaEmails() {
    idSeleccionado = -1;
    filaListaSeleccionada = -1;
    $('#txtListaDescripcion').val('');
    $('#txtListaDescripcion').focus();
    $('#dgEmails').datagrid('loadData', []);
}

function grabarListaEmails() {

    var emails = obtenerListadoEmailsCargados();

    var parametros;
    parametros = {
        accion: 'GRABAR-LISTASEMAILS',
        id: idSeleccionado.toString(),
        descripcion: $('#txtListaDescripcion').val(),
        emails: JSON.stringify(emails)
    };
    invocarControladorListasEmails_POST(parametros, ActualizacionSuccess);
}

function eliminarListaEmails() {
    var parametros;
    parametros = {
        accion: 'ELIMINAR-LISTASEMAILS',
        id: idSeleccionado.toString()
    };
    invocarControladorListasEmails_POST(parametros, ActualizacionSuccess);
}

function ActualizacionSuccess(data) {
    if (data.result == 'OK') {
        $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
        cargarGrillaListasEmails();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function obtenerListadoEmailsCargados() {
    var listado = new Array();
    var item;

    var rows = $('#dgEmails').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        item = new Object();
        item.Id = rows[i].Id;
        item.Descripcion = rows[i].Descripcion;
        listado.push(item);
    }
    return listado;
}


function cancelarListaEmails() {

}

function actualizarEmail() {
    if ($('#txtEmail').validatebox('isValid')) {
        if (filaEmailSeleccionada != -1) {
            $('#dgEmails').datagrid('updateRow', {
                index: filaEmailSeleccionada,
                row: {
                    Descripcion: $('#txtEmail').val()
                }
            });
        } else {

            var emailExiste = false;
            var rows = $('#dgEmails').datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                if (rows[i].Descripcion == $('#txtEmail').val())
                    emailExiste = true;
            }

            if (!emailExiste) {
                $('#dgEmails').datagrid('appendRow', {
                    Descripcion: $('#txtEmail').val()
                });
                $('#txtEmail').val('');
            } else {
                $.messager.alert('Error', 'El correo electrónico ingresado ya existe en la lista', 'error');
            }
        }
        filaEmailSeleccionada = -1;
        $('#txtEmail').focus()
    }
}


function eliminarEmail(rowIndex) {
    var row = $('#dgEmails').datagrid('getRows')[rowIndex];
    $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
        if (r) {
            $('#dgEmails').datagrid('deleteRow', rowIndex);
            filaEmailSeleccionada = -1;
        }
    });
}
