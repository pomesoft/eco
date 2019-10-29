var idEstudio;
var idParticipante;
var idCentroHabilitado;
var idPatrocinador;
var idMonodroga;
var idActa;
var idRecordatorioRenovacionAnual;

$(document).ready(function () {

    limpiarControles();
    inicializarBotones();
    obtenerDatosEstudio();

});

function inicializarBotones() {
    $("#btnVolver").click(function () {
        volver();
    });
    $("#btnGuardar").click(function () {
        guardar();
    });
    $("#btnConfigRecordatorio").click(function () {
        mostrarConfigRecordatorio();
    });

    $("#panelAlertaInactividad").css("display", "none");
    $("#chkEstudioRenovacionAnual").click(function () {
        mostrarConfigAlertaRenovacioAnual();
    });
}

function configurarGrillas() {
    $('#dgParticipantes').datagrid({
        title: 'Participantes Equipo de Investigación',
        width: '100%',
        height: 200,
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
		    { field: 'Rol', title: 'Rol', width: 150, formatter: function (value, row, index) {
		        if (row.Rol) return row.Rol.Descripcion;
		        else return value;
		    }
		    },
		    { field: 'Profesional', title: 'Profesional', width: 230, formatter: function (value, row, index) {
		        if (row.Profesional) return row.Profesional.NombreCompleto;
		        else return value;
		    }
		    },
		    { field: 'DesdeToString', title: 'Desde', width: 100 },
		    { field: 'HastaToString', title: 'Hasta', width: 100 }
	  ]],
        toolbar: [{
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: agregarParticipante
        }, '-', {
            text: 'Detalle',
            iconCls: 'icon-edit',
            handler: detalleParticipante
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarParticipante
        }],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                idParticipante = rowData.Id;
            }
        }
    });

    $('#dgCentrosHabilitados').datagrid({
        title: 'Centros Habilitados',
        width: '100%',
        height: 200,
        showHeader: false,
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
		    { field: 'Descripcion', title: 'Centro Médico', width: '70%', formatter: function (value, row, index) {
		            if (row.Centro) return row.Centro.Descripcion;
		            else return value;
		        }
		    },
            { field: 'DesdeToString', title: 'Desde', width: '15%' },
		    { field: 'HastaToString', title: 'Hasta', width: '15%' }
	  ]],
        toolbar: [{
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: agregarCentroHabilitado
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarCentroHabilitado
        }],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                idCentroHabilitado = rowData.Id;
            }
        }
    });

    $('#dgPatrocinadores').datagrid({
        title: 'Patrocinadores',
        width: '100%',
        height: 200,
        showHeader: false,
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
		    {
		        field: 'Patrocinador', title: 'Patrocinador', width: '100%', formatter: function (value, row, index) {
		        if (row.Patrocinador) return row.Patrocinador.Descripcion;
		        else return value;
		    }
		    }
	  ]],
        toolbar: [{
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: agregarPatrocinador
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarPatrocinador
        }],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                idPatrocinador = rowData.Id;
            }
        }
    });

    $('#dgMonodrogas').datagrid({
        title: 'Monodrogas',
        width: '100%',
        height: 200,
        showHeader: false,
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
		    { field: 'Monodroga', title: 'Monodroga', width: '100%', formatter: function (value, row, index) {
		        if (row.Monodroga) return row.Monodroga.Descripcion;
		        else return value;
		    }
		    }
	  ]],
        toolbar: [{
            text: 'Agregar',
            iconCls: 'icon-add',
            handler: agregarMonodroga
        }, '-', {
            text: 'Quitar',
            iconCls: 'icon-remove',
            handler: quitarMonodroga
        }],
        onClickRow: function (rowIndex, rowData) {
            if (rowData) {
                idMonodroga = rowData.Id;
            }
        }
    });
}

function obtenerDatosEstudio() {
    idEstudio = getURLParam("IdEstudio");
    idActa = getURLParam("IdActa");

    if (!idEstudio) idEstudio = -1;

    if (idEstudio != -1) {

        configurarGrillas();

        var parametros;
        parametros = {
            accion: 'OBTENER',
            idEstudio: idEstudio
        };
        invocarControlador(parametros, cargarControles);
    }
}

function invocarControlador(params, funcionOK) {
    controladorAJAX_GET('handlers/EstudioCargaDatosHandler.ashx', params, funcionOK);
}

function invocarControlador_POST(params, funcionOK) {
    controladorAJAX_POST('handlers/EstudioCargaDatosHandler.ashx', params, funcionOK);
}

function limpiarControles() {
    $('#txtCodigo').textbox('setValue', '');
    $('#txtNombre').textbox('setValue', '');
    $('#txtNombreCompleto').textbox('setValue', '');
    $('#ContentPlaceBody_cboPatologia').combobox('setValue', '');
    $('#ContentPlaceBody_cboEstado').combobox('setValue', '');
    $('#ContentPlaceBody_cboTipoEstudio').combobox('setValue', '');
    $('#txtPoblacion').textbox('setValue', '');
    $("#chkEstudioRenovacionAnual").prop('checked', false);
    $("#panelAlertaRenovacioAnual").css("display", "none");
}

function cargarControles(data) {
    limpiarControles();
    $('#txtCodigo').textbox('setValue', data.Codigo);
    $('#txtNombre').textbox('setValue', data.Descripcion);
    $('#txtNombreCompleto').textbox('setValue', data.NombreCompleto);
    if (data.Patologia)
        $('#ContentPlaceBody_cboPatologia').combobox('setValue', data.Patologia.Id);
    if (data.Estado)
        $('#ContentPlaceBody_cboEstado').combobox('setValue', data.Estado.Id);
    if (data.IdTipoEstudio)
        $('#ContentPlaceBody_cboTipoEstudio').combobox('setValue', data.IdTipoEstudio);
    $('#txtPoblacion').textbox('setValue', data.Poblacion);
    
    $('#txtFechaPresentacion').datebox('setValue', data.FechaPresentacionToString);
    $("#chkEstudioRenovacionAnual").prop('checked', data.RequiereAlerta);
    mostrarConfigAlertaRenovacioAnual();
    idRecordatorioRenovacionAnual = data.IdRecordatorioRenovacionAnual;
    $('#txtMesesAlertaRenovacionAnual').numberbox('setValue', data.MesesAlerta);

    if (data.Participantes) $('#dgParticipantes').datagrid('loadData', data.Participantes);
    if (data.CentrosHabilitados) $('#dgCentrosHabilitados').datagrid('loadData', data.CentrosHabilitados);
    if (data.Patrocinadores) $('#dgPatrocinadores').datagrid('loadData', data.Patrocinadores);
    if (data.Monodrogas) $('#dgMonodrogas').datagrid('loadData', data.Monodrogas);

}

function guardar() {
    var parametros = {
        accion: 'GRABAR',
        idEstudio: idEstudio,
        codigo: $('#txtCodigo').textbox('getValue'),
        nombre: formatearCaracteres($('#txtNombre').textbox('getValue')),
        nombreCompleto: formatearCaracteres($('#txtNombreCompleto').textbox('getValue')),
        patologia: $('#ContentPlaceBody_cboPatologia').combobox('getValue'),
        poblacion: $('#txtPoblacion').textbox('getValue'),
        estado: $('#ContentPlaceBody_cboEstado').combobox('getValue'), 
        fechaPresentacion: $('#txtFechaPresentacion').datebox('getValue'), 
        requiereAlerta: $('#chkEstudioRenovacionAnual').is(':checked'), 
        mesesAlerta: $('#txtMesesAlertaRenovacionAnual').numberbox('getValue'),
        tipoEstudio: $('#ContentPlaceBody_cboTipoEstudio').combobox('getValue')
    };
    invocarControlador_POST(parametros, guardarSucess);
}

function guardarSucess(data) {
    if (data.result == 'OK') {
        var parametroIdActa = idActa ? '&IdActa=' + idActa : '';
        var paginaReturn = getURLParam('PaginaReturn') != '' ? '&PaginaReturn=' + getURLParam('PaginaReturn') : '';
        location.href = 'EstudioCargaDatos.aspx?IdEstudio=' + data.id + parametroIdActa + paginaReturn;
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function volver() {
    var parametroIdActa = idActa ? '&IdActa=' + idActa : '';
    var paginaReturn = getURLParam('PaginaReturn') != '' ? '&PaginaReturn=' + getURLParam('PaginaReturn') : '';
    location.href = getPaginaReturn() + '?IdEstudio=' + idEstudio + parametroIdActa + paginaReturn;
}

/******** PARTICIPANTES EQUIPO DE INVESTIGACION *********/
function agregarParticipante() {
    if (idEstudio != -1) {
        idParticipante = -1;
        limpiarControlesIntegrante();        
        $('#dlgParticipante').dialog('open').dialog('setTitle', 'Agregar Participante');
    }
}

function detalleParticipante() {
    if (idEstudio != -1) {
        var participanteSeleccionado = $('#dgParticipantes').datagrid('getSelected');
        if (participanteSeleccionado) {
            idParticipante = participanteSeleccionado.Id;
            $('#ContentPlaceBody_cboParticipanteProfesional').combobox('setValue', participanteSeleccionado.Profesional.Id);
            $('#ContentPlaceBody_cboParticipanteRol').combobox('setValue', participanteSeleccionado.Rol.Id);
            $('#txtParticipanteDesde').datebox('setValue', participanteSeleccionado.DesdeToString);
            $('#txtParticipanteHasta').datebox('setValue', participanteSeleccionado.HastaToString);

            $('#dlgParticipante').dialog('open').dialog('setTitle', 'Detalle Participante');
        }
    }
}

function quitarParticipante() {
    if (idEstudio != -1) {
        var participanteSeleccionado = $('#dgParticipantes').datagrid('getSelected');
        if (participanteSeleccionado) {

            $.messager.confirm('Confirmar', '¿Desea quitar el participante ' + participanteSeleccionado.Profesional.NombreCompleto + '?',
            function (r) {
                if (r) {
                    idParticipante = participanteSeleccionado.Id;
                    var parametros = {
                        accion: 'ELIMINAR-PARTICIPANTE',
                        idEstudio: idEstudio,
                        idParticipante: idParticipante
                    };
                    invocarControlador_POST(parametros, guardarParticipanteSucess);
                }
            });
        }
    }
}

function limpiarControlesIntegrante() {
    $('#ContentPlaceBody_cboParticipanteProfesional').combobox('setValue', '');        
    $('#ContentPlaceBody_cboParticipanteRol').combobox('setValue', '');
    $('#txtParticipanteDesde').datebox('setValue', '');
    $('#txtParticipanteHasta').datebox('setValue', '');
}

function cancelarParticipante() {
    limpiarControlesIntegrante();
    $('#dlgParticipante').dialog('close');
}


function guardarParticipante() {
    var parametros = {
        accion: 'GRABAR-PARTICIPANTE',
        idEstudio: idEstudio,
        idParticipante: idParticipante,
        idProfesional: $('#ContentPlaceBody_cboParticipanteProfesional').combobox('getValue'),
        idRol: $('#ContentPlaceBody_cboParticipanteRol').combobox('getValue'),
        desde: $('#txtParticipanteDesde').datebox('getValue'),
        hasta: $('#txtParticipanteHasta').datebox('getValue')
    };
    invocarControlador_POST(parametros, guardarParticipanteSucess);
}

function guardarParticipanteSucess(data) {
    if (data.result == 'OK') {
        $('#dlgParticipante').dialog('close');
        obtenerDatosEstudio();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

/*********CENTROS HABILITADOS *********/
function agregarCentroHabilitado() {
    if (idEstudio != -1) {
        idCentroHabilitado = -1;
        limpiarControlesCentroHabilitado();
        $('#dlgCentroHabilitado').dialog('open').dialog('setTitle', 'Agregar Centro Habilitado');
    }
}

function limpiarControlesCentroHabilitado() {
    $('#ContentPlaceBody_cboCentroHabilitado').combobox('setValue', '');
    $('#txtCentroHabilitadoDesde').datebox('setValue', '');
    $('#txtCentroHabilitadoHasta').datebox('setValue', '');
}


function quitarCentroHabilitado() {
    if (idEstudio != -1) {
        var centroHabilitadoSeleccionado = $('#dgCentrosHabilitados').datagrid('getSelected');
        if (centroHabilitadoSeleccionado) {

            $.messager.confirm('Confirmar', '¿Desea quitar el centro habilitado' + centroHabilitadoSeleccionado.Centro.Descripcion + '?',
            function (r) {
                if (r) {
                    idCentroHabilitado = centroHabilitadoSeleccionado.Id;
                    var parametros = {
                        accion: 'ELIMINAR-CENTROHABILITADO',
                        idEstudio: idEstudio,
                        idCentroHabilitado: idCentroHabilitado
                    };
                    invocarControlador_POST(parametros, guardarCentroHabilitadoSucess);
                }
            });
        }
    }
}

function guardarCentroHabilitado() {
    var parametros = {
        accion: 'GRABAR-CENTROHABILITADO',
        idEstudio: idEstudio,
        idCentroHabilitado: idCentroHabilitado,
        idCentro: $('#ContentPlaceBody_cboCentroHabilitado').combobox('getValue'),
        desde: $('#txtCentroHabilitadoDesde').datebox('getValue'),
        hasta: $('#txtCentroHabilitadoHasta').datebox('getValue')
    };
    invocarControlador_POST(parametros, guardarCentroHabilitadoSucess);
}

function guardarCentroHabilitadoSucess(data) {
    if (data.result == 'OK') {
        $('#dlgCentroHabilitado').dialog('close');
        obtenerDatosEstudio();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarCentroHabilitado() {
    limpiarControlesCentroHabilitado();
    $('#dlgCentroHabilitado').dialog('close');
}

/*********PATROCINADORES *********/
function agregarPatrocinador() {
    if (idEstudio != -1) {
        idPatrocinador = -1;
        limpiarControlesPatrocinador();
        $('#dlgPatrocinador').dialog('open').dialog('setTitle', 'Agregar Patrocinador');
    }
}

function limpiarControlesPatrocinador() {
    $('#ContentPlaceBody_cboPatrocinador').combobox('setValue', '');
}


function quitarPatrocinador() {
    if (idEstudio != -1) {
        var patrocinadorSeleccionado = $('#dgPatrocinadores').datagrid('getSelected');
        if (patrocinadorSeleccionado) {

            $.messager.confirm('Confirmar', '¿Desea quitar el Patrocinador' + patrocinadorSeleccionado.Patrocinador.Descripcion + '?',
            function (r) {
                if (r) {
                    idPatrocinador = patrocinadorSeleccionado.Id;
                    var parametros = {
                        accion: 'ELIMINAR-PATROCINADOR',
                        idEstudio: idEstudio,
                        idEstudioPatrocinador: idPatrocinador
                    };
                    invocarControlador_POST(parametros, guardarPatrocinadorSucess);
                }
            });
        }
    }
}

function guardarPatrocinador() {
    var parametros = {
        accion: 'GRABAR-PATROCINADOR',
        idEstudio: idEstudio,
        idPatrocinador: idPatrocinador,
        idPatrocinador: $('#ContentPlaceBody_cboPatrocinador').combobox('getValue')
    };
    invocarControlador_POST(parametros, guardarPatrocinadorSucess);
}

function guardarPatrocinadorSucess(data) {
    if (data.result == 'OK') {
        $('#dlgPatrocinador').dialog('close');
        obtenerDatosEstudio();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarPatrocinador() {
    limpiarControlesPatrocinador();
    $('#dlgPatrocinador').dialog('close');
}


/********* MONODROGAS *********/
function agregarMonodroga() {
    if (idEstudio != -1) {
        idMonodroga = -1;
        limpiarControlesMonodroga();
        $('#dlgMonodroga').dialog('open').dialog('setTitle', 'Agregar Monodroga');
    }
}

function limpiarControlesMonodroga() {
    $('#ContentPlaceBody_cboMonodroga').combobox('setValue', '');
}


function quitarMonodroga() {
    if (idEstudio != -1) {
        var monodrogaSeleccionado = $('#dgMonodrogas').datagrid('getSelected');
        if (monodrogaSeleccionado) {

            $.messager.confirm('Confirmar', '¿Desea quitar el Monodroga' + monodrogaSeleccionado.Monodroga.Descripcion + '?',
            function (r) {
                if (r) {
                    idMonodroga = monodrogaSeleccionado.Id;
                    var parametros = {
                        accion: 'ELIMINAR-MONODROGA',
                        idEstudio: idEstudio,
                        idEstudioMonodroga: idMonodroga
                    };
                    invocarControlador_POST(parametros, guardarMonodrogaSucess);
                }
            });
        }
    }
}

function guardarMonodroga() {
    var parametros = {
        accion: 'GRABAR-MONODROGA',
        idEstudio: idEstudio,
        idMonodroga: idMonodroga,
        idMonodroga: $('#ContentPlaceBody_cboMonodroga').combobox('getValue')
    };
    invocarControlador_POST(parametros, guardarMonodrogaSucess);
}

function guardarMonodrogaSucess(data) {
    if (data.result == 'OK') {
        $('#dlgMonodroga').dialog('close');
        obtenerDatosEstudio();
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function cancelarMonodroga() {
    limpiarControlesMonodroga();
    $('#dlgMonodroga').dialog('close');
}

/********* RECORDATORIOS *********/
function mostrarConfigAlertaRenovacioAnual() {
    if ($("#chkEstudioRenovacionAnual").is(':checked')) {

        if (idEstudio == -1) {
            $.messager.alert('Grabar estudio', 'Debe grabar los datos del estudio', 'warning');
        } else {
            $("#panelAlertaRenovacioAnual").css("display", "inline-block");

            var parametros;
            parametros = {
                accion: 'OBTENER-DEFUALT-ALERTA'
            };
            invocarControlador(parametros,
                function (data) {
                    if (data) {
                        $('#txtMesesAlertaRenovacionAnual').numberbox('setValue', data);
                    }
                });
        }

    } else {
        $("#panelAlertaRenovacioAnual").css("display", "none");
    }

}

function mostrarConfigRecordatorio() {

    var parametros;
    parametros = {
        id: 2,
        accion: 'OBTENER'
    };

    controladorAJAX_GET('handlers/TiposRecordatorioHandler.ashx', parametros,
        function (tipoRecordatorio) {
            if (tipoRecordatorio) {
                
                var estudio = new Object();
                estudio.Id = idEstudio;
                estudio.NombreEstudioListados = $('#txtCodigo').val() + ($('#txtNombre').val() != '' ? ' - ' + formatearCaracteres($('#txtNombre').val()) : '');

                var dias = parseInt($('#txtMesesAlertaRenovacionAnual').val()) * 30;
                var fechaActivacion = sumarDiasFecha(parserDateBox($('#txtFechaPresentacion').datebox('getValue')), dias);

                if (idRecordatorioRenovacionAnual == -1)
                    nuevoRecordatorioEstudio(tipoRecordatorio, estudio, fechaActivacion);
                else
                    mostrarRecordatorioEstudio(tipoRecordatorio, estudio, fechaActivacion, idRecordatorioRenovacionAnual);

                $('#dlgRecordatorioRenovacionAnual').dialog({
                    title: 'Recordatorio Renovación Anual del Estudio',
                    width: 650,
                    height: 660,
                    closed: true,
                    modal: true,
                    onOpen: function () {
                    }
                });
                $('#dlgRecordatorioRenovacionAnual').dialog('center').dialog('open');
            }
        });
}

function actualizarDatosRecordatorioSuccess(data) {
    if (data.result == 'OK') {
        guardar();    
    } else {
        $.messager.alert('Error', data.message, 'error');
    }
}

function guardarRecordatorioRenovacionAnual() {
    guardarRecordatorio();
    $('#dlgRecordatorioRenovacionAnual').dialog("close");
}

function cancelarRecordatorioRenovacionAnual() {
    $('#dlgRecordatorioRenovacionAnual').dialog("close");    
}