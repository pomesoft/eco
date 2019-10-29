<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="Plantillas.aspx.cs" Inherits="pome.SysGEIC.Web.Plantillas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/Plantillas.js" type="text/javascript"></script>
    <link href="css/texteditor.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div class="contenedorControlesMitad">
        <div id="dg_toolbar" style="padding: 5px; height: auto">
            <div>
                <input type="text" id="txtPlantillaBuscar" class="campoTextBuscador" />
                <div id="menuPlantillaBuscar" style="width: 250px">                    
                    <div data-options="name:'Descripcion'">
                        Descripción</div>
                    <div data-options="name:'Todos'">
                        Listar todos</div>
                </div>
            </div>
            Tipo: <input class="easyui-combobox" id="cboPlantillaTipoBuscar" name="TipoPlantillaBuscar" style="width: 150px" 
                data-options="
		        valueField: 'Id',
		        textField: 'Descripcion',
		        data: [{ Id: -1, Descripcion: 'TODOS'},
                           { Id: 1, Descripcion: 'LIBRE'},
                           { Id: 2, Descripcion: 'CARTA DE RESPUESTA'},
                           { Id: 3, Descripcion: 'ACTA'},
                           { Id: 4, Descripcion: 'APROBACIÓN DE ESTUDIO'}
                           ]" />
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevaPlantilla()">
                Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                    onclick="eliminarPlantilla()">Eliminar</a> <a href="#" class="easyui-linkbutton"
                        iconcls="icon-ok" plain="true" onclick="guardarPlantilla()">Guardar</a>
            <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" plain="true" onclick="cancelarPlantilla()">
                Cancelar</a>
            
        </div>
        <table id="dg" toolbar="#dg_toolbar">
        </table>
    </div>
    <div class="contenedorControlesMitad">
        <div id="panelDatos" style="padding: 5px;">
            <fieldset>    
                <div class="fitem">
                    <label for="TipoPlantilla">
                        Tipo:</label>
                    <input class="easyui-combobox" id="cboPlantillaTipo" name="TipoPlantilla" style="width: 450px" 
                            data-options="
		                    valueField: 'Id',
		                    textField: 'Descripcion',
		                    data: [{ Id: 1, Descripcion: 'LIBRE'},
                                       { Id: 2, Descripcion: 'CARTA DE RESPUESTA'},
                                       { Id: 3, Descripcion: 'ACTA'},
                                       { Id: 4, Descripcion: 'APROBACIÓN DE ESTUDIO'}
                                       ]" />
                </div>
                <div class="fitem">
                    <label for="Nombre">
                        Nombre:</label>
                    <input class="easyui-textbox campoABMLargo" type="text" id="txtPlantillaNombre"
                        name="Nombre" data-options="required:true" />
                </div>
                <div class="fitem">
                    <textarea class="campoTextAreaPanel" id="txtPlantillaTexto" name="Texto" cols="20"
                        rows="33" style="width: 530px;"></textarea>
                    
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
