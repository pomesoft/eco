var ctrolSeleccionPlantilla_id

$(document).ready(function () {

    idPlantillaSeleccionada = -1;
    //$('#ctrolSeleccionPlantilla_txtTexto').texteditor();

});

function ctrolSeleccionPlantilla_cargarCboPlantilla() {
    $('#ctrolSeleccionPlantilla_cboPlantilla').combobox({
        url: 'handlers/PlantillasHandler.ashx?accion=LISTAR',
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: '300',
        onSelect: function (rec) {
            if (rec) ctrolSeleccionPlantilla_obtenerPlantilla(rec);
        }
    });
}

function ctrolSeleccionPlantilla_limpiarControles() {
    idPlantillaSeleccionada = -1;
    $('#ctrolSeleccionPlantilla_Texto').empty();
}

function ctrolSeleccionPlantilla_obtenerPlantilla(data) {
    idPlantillaSeleccionada = data.Id;
    $('#ctrolSeleccionPlantilla_Texto').html(data.Texto);
    //$('#ctrolSeleccionPlantilla_txtTexto').texteditor('setValue', data.Texto);
}

function ctrolSeleccionPlantilla_abrir() {
    ctrolSeleccionPlantilla_limpiarControles();
    ctrolSeleccionPlantilla_cargarCboPlantilla();
    $('#dlgSeleccionPlantillaTexto').dialog('setTitle', 'Documento a tratar');
    $('#dlgSeleccionPlantillaTexto').dialog('center');
    $('#dlgSeleccionPlantillaTexto').dialog('open');
}

function ctrolSeleccionPlantilla_aceptar() {
    if (idPlantillaSeleccionada != -1) {
        $('#dlgSeleccionPlantillaTexto').dialog('close');
        actualizar_ctrolSeleccionPlantilla($('#ctrolSeleccionPlantilla_Texto').text())
    }
}

function ctrolSeleccionPlantilla_cancelar() {
    $('#dlgSeleccionPlantillaTexto').dialog('close');
}