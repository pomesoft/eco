var idSeleccionado;
var filaSeleccionada;
var idTipoPlantilla;
var TIPO_PLANTILLA_LIBRE = 1;
var TIPO_PLANTILLA_CARTARESPUESTA = 2;

$(document).ready(function () {

    configurarControles();
    configurarGrilla();
    obtenerDatos();

    $("#btnBuscar").click(function () {
        obtenerDatos();
    });
});

function configurarControles() {
    //buscador
    $('#txtPlantillaBuscar').searchbox({
        searcher: buscarPlantillas,
        menu: '#menuPlantillaBuscar',
        width: '100%',
        prompt: prompBusqueda
    });

    //paneles
    $('#panelDatos').panel({
        width: '100%',
        height: 550,
        title: 'Datos plantilla seleccionada'
    });

}

function configurarGrilla() {
    $('#dg').datagrid({
        title: 'Plantillas de texto',
        width: '100%',
        height: 550,
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
		    { field: 'Id', title: 'Código', hidden: true },
                { field: 'TipoPlantilla', title: 'TipoPlantilla', hidden: true },
                { field: 'TipoPlantillaDescripcion', title: 'Tipo', sortable: true, width: '30%' },
		    { field: 'Descripcion', title: 'Nombre', sortable: true, width: '70%' }
	  ]],
        onSelect: function (rowIndex, rowData) {
            var row = $('#dg').datagrid('getSelected');
            mostrar(row);
            filaSeleccionada = rowIndex;
        }
    });

}

function obtenerDatos() {
    var parametros;
    parametros = {
        accion: 'LISTAR-TODOS',
        descripcion: ''
    };

    controladorAJAX_GET('handlers/PlantillasHandler.ashx', parametros, cargarGrilla);
}


function buscarPlantillas(value, name) {
    var idTipoPlantilla = $('#cboPlantillaTipoBuscar').combobox('getValue');

    var parametros;
    parametros = {
        accion: 'LISTAR-TODOS',
        descripcion: name == 'Descripcion' ? value : '',
        idTipo: idTipoPlantilla
    }
    controladorAJAX_GET('handlers/PlantillasHandler.ashx', parametros, cargarGrilla);
}

function cargarGrilla(data) {
    if (data) {
        $('#dg').datagrid('loadData', data);
        $('#dg').datagrid('selectRow', (filaSeleccionada == -1 ? 0 : filaSeleccionada));
        var row = $('#dg').datagrid('getSelected');
        mostrar(row);
    }
}

function mostrar(data) {
    limpiarControlesPlantilla();
    if (data) {
        idSeleccionado = data.Id;
        if (data.TipoPlantilla != null) {
            idTipoPlantilla = data.TipoPlantilla.Id;
        } else {
            idTipoPlantilla = -1;
        }
        $('#txtPlantillaNombre').textbox('setText', data.Descripcion);
        $('#cboPlantillaTipo').combobox('setValue', idTipoPlantilla);
        $('#txtPlantillaTexto').val(data.Texto);
    }

}

function limpiarControlesPlantilla() {
    idSeleccionado = -1;
    idTipoPlantilla = -1;
    $('#txtPlantillaNombre').textbox('clear');
    $('#cboPlantillaTipo').combobox('setValue', '');
    $('#txtPlantillaTexto').val('');
}

function nuevaPlantilla() {
    filaSeleccionada = -1;
    limpiarControlesPlantilla     
}

function guardarPlantilla() {
    var parametros;
    parametros = {
        accion: 'GRABAR',
        id: idSeleccionado,
        nombre: $('#txtPlantillaNombre').textbox('getText'),
        texto: $('#txtPlantillaTexto').val(),
        idTipo: $('#cboPlantillaTipo').combobox('getValue') 
    };
    controladorAJAX_POST('handlers/PlantillasHandler.ashx', parametros, guardarPlantillaSuccess);
}

function eliminarPlantilla() {
    var row = $('#dg').datagrid('getSelected');
    if (row) {
        $.messager.confirm('Confirmar', '¿Desea eliminar ' + row.Descripcion + '?', function (r) {
            if (r) {
                var parametros;
                parametros = {
                    accion: 'ELIMINAR',
                    id: idSeleccionado
                };
                controladorAJAX_GET('handlers/PlantillasHandler.ashx', parametros, guardarPlantillaSuccess);
            }
        });
    }
}

function guardarPlantillaSuccess(data) {
    if (data.result == 'OK') {
        $.messager.alert('Confirmación', 'Los datos se actualizaron correctamente', 'info');
        limpiarControlesPlantilla();
        obtenerDatos();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarPlantilla() {
    var row = $('#dg').datagrid('getSelected');
    if (row) mostrar(row.Id);
}
