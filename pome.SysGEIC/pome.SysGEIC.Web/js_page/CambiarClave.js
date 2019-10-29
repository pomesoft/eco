
$(document).ready(function () {
    obtenerDatos();
});

function obtenerDatos() {
    var parametros;
    parametros = {
        accion: 'OBTENER_USUARIOLOGIN'
    };

    controladorAJAX_GET('handlers/UsuariosHandler.ashx',
                        parametros,
                        function (data) {
                            if (data) {
                                $('#txtApellido').val(data.Apellido);
                                $('#txtNombre').val(data.Nombre);
                                $('#txtLogin').val(data.LoginUsuario);
                            }
                        });
}

function cambiarClave() {
    var parametros;
    parametros = {
        accion: 'GRABAR_CAMBIOCLAVE',
        claveActual: $('#ContentPlaceBody_txtClaveActual').val(),
        nuevaClave1: $('#ContentPlaceBody_txtNuevaClave1').val(),
        nuevaClave2: $('#ContentPlaceBody_txtNuevaClave2').val()
    };
    controladorAJAX_POST('handlers/UsuariosHandler.ashx', parametros, cambiarClaveSuccess);
}

function cambiarClaveSuccess(data) {
    if (data.result == 'OK') {
        $.messager.alert('Confirmación', 'La contraseña se cambió correctamente correctamente', 'info');        
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}
