var ctrolSeleccionPlantilla_id;

$(document).ready(function () {
    idPlantillaSeleccionada = -1;
    
    CKEDITOR.disableAutoInline = true;
    CKEDITOR.inline('ctrolSeleccionPlantilla_Texto');
});

function ctrolSeleccionPlantilla_cargarCboPlantilla() {
    $('#ctrolSeleccionPlantilla_cboPlantilla').combobox({
        url: 'handlers/PlantillasHandler.ashx?accion=LISTAR',
        valueField: 'Id',
        textField: 'Descripcion',
        panelHeight: 'auto',
        onSelect: function (rec) {
            ctrolSeleccionPlantilla_limpiarControles();
            if (rec) ctrolSeleccionPlantilla_obtenerPlantilla(rec);
        }
    });
}

function ctrolSeleccionPlantilla_limpiarControles() {
    idPlantillaSeleccionada = -1;
    CKEDITOR.instances.ctrolSeleccionPlantilla_Texto.setData('');
    CKEDITOR.instances.ctrolSeleccionPlantilla_Texto.updateElement();
}

function ctrolSeleccionPlantilla_obtenerPlantilla(data) {
    idPlantillaSeleccionada = data.Id;

    CKEDITOR.instances.ctrolSeleccionPlantilla_Texto.setData(data.Texto);
    CKEDITOR.instances.ctrolSeleccionPlantilla_Texto.updateElement();
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
        actualizar_ctrolSeleccionPlantilla(formatearCaracteres(CKEDITOR.instances.ctrolSeleccionPlantilla_Texto.getData()));
        
    }
}

function ctrolSeleccionPlantilla_cancelar() {
    $('#dlgSeleccionPlantillaTexto').dialog('close');
}