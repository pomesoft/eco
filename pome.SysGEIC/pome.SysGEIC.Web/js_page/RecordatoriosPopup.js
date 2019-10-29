
$(document).ready(function () {
    
    $('#panelRecordatoriosPopup').dialog({
        title: 'Recordatorios y Alertas',
        width: 800,
        height: 450,
        closed: true,
        modal: true
    });
});

function configurarGrillaPopupRecordatorios() {
    $('#dgRecordatoriosPopup').datagrid({
        title: '',
        width: 780,
        height: 400,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        remoteSort: false,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        nowrap: false,
        showHeader: false,
        columns: [[
		    { field: 'Id', title: 'Código', hidden: true, width: 60 },
                { field: 'Descripcion', title: '', align: 'left', width: 760,
                    formatter: function (value, row, index) {
                        if (value != '') {

                            var texto = '<table style="border:0;<tr>">';
                            texto += '<td style="width:30px;"><input type="text" id="color" style="background:' + row.Color + ';width:20px; height: 20px;"/></td>';
                            texto += '<td style="width:750px;"><div class="dialog-content" style="padding: 5px;">';
                            texto += '<span><strong>' + row.Descripcion + '</strong></span><br />';
                            texto += '<span>Tipo: ' + row.TipoRecordatorioDescripcion + '</span><br />';
                            texto += '<span>Fecha Activación: ' + row.FechaActivacionToString + '</span><br />';
                            texto += '<p>' + row.Texto + '</p>';
                            texto += '</div></td></tr></table>'
                            return texto;
                        }
                        else
                            return value;
                    }
                }
	  ]],
        toolbar: [{
            text: 'Cerrar recordatorio',
            iconCls: 'icon-ok',
            handler: cerrarEstadoRecordatorio
        }, '-', {
            text: 'Salir',
            iconCls: 'icon-cancel',
            handler: cerrarPopupRecordatorios
        }],
        onClickRow: function (rowIndex, rowData) {

        },
        onClickRow: function (rowIndex, rowData) {

        }
    });
}

function cargarGrillaPopupRecordatorios() {

    configurarGrillaPopupRecordatorios();

    var parametros;
    parametros = {
        accion: 'LISTAR-RECORDATORIOS-ACTIVOS-POPUP'
    };

    controladorAJAX_GET('handlers/RecordatoriosHandler.ashx', parametros, function (data) {
        if (data) {
            if (data.result == 'Error') {
                $.messager.alert('Error', 'Ocurrió un error al obtener datos, por favor reintente', 'error');
            } else {
                $('#dgRecordatoriosPopup').datagrid('loadData', data);

                var rows = $('#dgRecordatoriosPopup').datagrid('getRows');
                if (rows.length > 0)
                    $('#imgIconoAlertaProncipal').attr('src', '../img/alertRojo.png');
                else
                    $('#imgIconoAlertaProncipal').attr('src', '../img/alertVerde.png');
            }
        }
    });
}

function mostrarPopupRecordatorios() {

    cargarGrillaPopupRecordatorios();
    $('#panelRecordatoriosPopup').dialog("open");
    
}

function cerrarPopupRecordatorios() {
    $('#panelRecordatoriosPopup').dialog("close");
}


function cerrarEstadoRecordatorio() {
    var recordatorioSeleccionado = $('#dgRecordatoriosPopup').datagrid('getSelected');
    if (recordatorioSeleccionado) {
        var parametros;
        parametros = {
            accion: 'CAMBIAR-ESTADO-RECORDATORIO',
            id: recordatorioSeleccionado.Id.toString(),
            idEstado: 3
        };
        controladorAJAX_POST('handlers/RecordatoriosHandler.ashx', parametros, function () {
            cargarGrillaPopupRecordatorios();
        });
    }
}