<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="Profesionales.aspx.cs" Inherits="pome.SysGEIC.Web.Profesionales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/Profesionales.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="dg_toolbar" style="padding: 5px; height: auto">
       <div>
            Tipo: 
            <input class="easyui-combobox" id="cboTipoProfesionalBuscar" name="TipoProfesionalBuscar" style="width: 300px" 
                    data-options="
				    url:'handlers/ProfesionalesHandler.ashx?accion=LISTAR-TIPOSPROFESIONALES',
				    valueField:'Id',
				    textField:'Descripcion',
				    panelHeight:'auto'" />
            Apellido: 
            <input type="text" id="txtApellidoBuscar" name="ApellidoBuscar" class="campoABM"></input>
            Nombre: 
            <input type="text" id="txtNombreBuscar" name="NombreBuscar" class="campoABM"></input>
            <a href="#" id="btnBuscar" name="btnBuscar" class="easyui-linkbutton" iconcls="icon-search"
                plain="true">Buscar</a>
        </div>
    </div>
    <table id="dg" toolbar="#dg_toolbar">
    </table>
    <div id="toolbar" style="padding: 3px; width: 950px; border: 1px solid #ccc">
        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
            Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                onclick="eliminar()">Eliminar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                    plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                        iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
    </div>
    <fieldset>
        <div class="fitem">
            <label for="Titulo">
                Título:</label>
            <input class="easyui-validatebox campoABM" type="text" id="txtTitulo" name="Titulo"
                data-options="required:true"></input>
        </div>
        <div class="fitem">
            <label for="Apellido">
                Apellido:</label>
            <input class="easyui-validatebox campoABM" type="text" id="txtApellido" name="Apellido"
                data-options="required:true"></input>
        </div>
        <div class="fitem">
            <label for="Nombre">
                Nombre:</label>
            <input class="easyui-validatebox campoABM" type="text" id="txtNombre" name="Nombre"
                data-options="required:true"></input>
        </div>
        <div class="fitem">
		<label for="TipoProfesional">
		    Tipo:</label>
		<input class="easyui-combobox" id="cboTipoProfesional" name="TipoProfesional" style="width: 300px" 
                    data-options="
				    url:'handlers/ProfesionalesHandler.ashx?accion=LISTAR-TIPOSPROFESIONALES',
				    valueField:'Id',
				    textField:'Descripcion',
				    panelHeight:'auto'" />
	  </div>
         <div class="fitem">
		<label for="RolComite">
		    Rol:</label>
		<input class="easyui-combobox" id="cboRolComite" name="RolComite" style="width: 300px" 
                    data-options="
				    url:'handlers/ProfesionalesHandler.ashx?accion=LISTAR-ROLESCOMITE',
				    valueField:'Id',
				    textField:'Descripcion',
				    panelHeight:'auto'" />
	  </div>
        <div class="fitem">
            <label for="OrdenActa">
                Orden en Actas:</label>
            <input class="easyui-validatebox campoABM" type="text" id="txtOrdenActa" name="OrdenActa" />
        </div>
    </fieldset>
</asp:Content>
