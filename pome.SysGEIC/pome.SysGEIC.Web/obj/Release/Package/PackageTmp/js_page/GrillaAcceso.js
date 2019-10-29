var permisos = [
                { id: '0', descripcion: '' },
		    { id: '1', descripcion: 'SIN ACCESO' },
		    { id: '2', descripcion: 'SOLO LECTURA' },
		    { id: '3', descripcion: 'AGREGAR/EDITAR' },
		    { id: '4', descripcion: 'ELIMINAR' }
		];

var lastIndex;

$(document).ready(function () {

    configurarGrillaAcceso();

});


function configurarGrillaAcceso() {
    $('#dgMenuAcceso').datagrid({
        title: 'Configuración de accesos por item de menú',
        width: 600,
        height: 550,
        autoRowHeight: false,
        collapsible: false,
        pagination: false,
        rownumbers: false,
        fitcolumns: true,
        singleSelect: true,
        sortName: 'Descripcion',
        sortOrder: 'asc',
        columns: [[
		    { field: 'IdMenuPrincipal', title: 'IdMenuPrincipal', width: 60, hidden: true },
		    { field: 'MenuPrincipal', title: 'Menu Principal', width: 230 },
		    { field: 'IdMenuSecundario', title: 'IdMenuSecundario', width: 60, hidden: true },
		    { field: 'MenuSecundario', title: 'Submenu', width: 230 },
                { field: 'IdNivelAcceso', title: 'Permiso', width: 110,
                    formatter: function (value) {
                        for (var i = 0; i < permisos.length; i++) {
                            if (permisos[i].id == value) return permisos[i].descripcion;
                        }
                        return value;
                    },
                    editor: {
                        type: 'combobox',
                        options: {
                            valueField: 'id',
                            textField: 'descripcion',
                            data: permisos
                        }
                    }
                }
	  ]],
        onClickRow: function (rowIndex, rowData) {
            if (rowData.IdNivelAcceso != 0) {
                if (lastIndex == rowIndex) {
                    $('#dgMenuAcceso').datagrid('endEdit', lastIndex);
                    lastIndex = -1;
                }
                else
                    if (lastIndex != rowIndex) {
                        $('#dgMenuAcceso').datagrid('endEdit', lastIndex);
                        $('#dgMenuAcceso').datagrid('beginEdit', rowIndex);
                    }
                lastIndex = rowIndex;
            }
        }
    });

}