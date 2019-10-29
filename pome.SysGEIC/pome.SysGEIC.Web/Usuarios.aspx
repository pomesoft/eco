<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="Usuarios.aspx.cs" Inherits="pome.SysGEIC.Web.Usuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/Usuarios.js" type="text/javascript"></script>
    <script src="js_page/GrillaAcceso.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        <div id="dg_toolbar" style="padding: 5px; height: auto">
            <div>
                Apellido:
                <input type="text" id="txtApellidoBuscar" name="ApellidoBuscar" class="campoABM"></input>
            </div>
            <div>
                Nombre:
                <input type="text" id="txtNombreBuscar" name="NombreBuscar" class="campoABM"></input>
                <a href="#" id="btnBuscar" name="btnBuscar" class="easyui-linkbutton" iconcls="icon-search"
                    plain="true">Buscar</a>
            </div>
        </div>
        <table id="dg">
        </table>
        <%--<table id="dg" title="Información de Usuarios" class="easyui-datagrid" style="width: 1198px;
            height: 300px" toolbar="#dg_toolbar" pagination="false" rownumbers="false" fitcolumns="true"
            singleselect="true">
            <thead>
                <tr>
                    <th field="Id" width="10">
                        Id
                    </th>
                    <th field="Apellido" width="30">
                        Apellido
                    </th>
                    <th field="Nombre" width="30">
                        Nombre
                    </th>
                    <th field="LoginUsuario" width="50">
                        Login
                    </th>
                </tr>
            </thead>
        </table>--%>
        <div id="toolbar" style="padding: 3px; width: 950px; border: 1px solid #ccc">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
                Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                    onclick="eliminar()">Eliminar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                        plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                            iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
        </div>
        <fieldset>
            <div class="bloqueCamposABM">
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
                    <label for="LoginUsuario">
                        Login:</label>
                    <input class="easyui-validatebox campoABMsinTransform" type="text" id="txtLoginUsuario"
                        name="LoginUsuario" data-options="required:true"></input>
                </div>
                <div class="fitem">
                    <label for="Mail">
                        Email:</label>
                    <input class="easyui-validatebox campoABMsinTransform" type="text" id="txtEMail" name="Mail"
                        data-options="validType:'email'"></input>
                </div>
                <div class="fitem">
                    <label for="TipoUsuario">
                        Tipo Usuario:</label>
                    <input class="easyui-combobox" id="cboTipoUsuario" name="TipoUsuario" style="width: 300px"
                        data-options="
					url:'handlers/UsuariosHandler.ashx?accion=LISTAR_TIPOSUSUARIO',
					valueField:'Id',
					textField:'Descripcion',
					panelHeight:'auto',
                              required:true" />
                </div>
            </div>
        </fieldset>
        <div class="faclaracion">
            <label>
                Los usuarios nuevos se asignan contraseña 1, podrá modificarla desde Seguridad
                / Cambiar Contraseña</label>
        </div>
    </div>
    <div id="panelDerecho" class="contenedorControlesMitad">
        <table id="dgMenuAcceso">
        </table>
    </div>
</asp:Content>
