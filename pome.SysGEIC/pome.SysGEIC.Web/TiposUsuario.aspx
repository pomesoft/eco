<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="TiposUsuario.aspx.cs" Inherits="pome.SysGEIC.Web.TiposUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/TiposUsuario.js" type="text/javascript"></script>
    <script src="js_page/GrillaAcceso.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        <div id="dg_toolbar" style="padding: 5px; height: auto">
            <div>
                Descripción:
                <input type="text" id="txtDescripcionBuscar" name="DescripcionBuscar" class="campoABM"></input>
                <a href="#" id="btnBuscar" name="btnBuscar" class="easyui-linkbutton" iconcls="icon-search"
                    plain="true">Buscar</a>
            </div>
        </div>
        <table id="dg" title="Información de Tipos de Usuario" class="easyui-datagrid" style="width: 595px;
            height: 480px" toolbar="#dg_toolbar" pagination="false" rownumbers="false" fitcolumns="true"
            singleselect="true">
            <thead>
                <tr>
                    <th field="Id" width="50">
                        Id
                    </th>
                    <th field="Descripcion" width="500">
                        Descripción
                    </th>
                </tr>
            </thead>
        </table>
        <div id="toolbar" style="padding: 3px; width: 100%; border: 1px solid #ccc">
            <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
                Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                    onclick="eliminar()">Eliminar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                        plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                            iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
        </div>
        <fieldset>
            <div class="fitem">
                <label for="Descripcion">
                    Descripción:</label>
                <input class="easyui-validatebox campoABM" type="text" id="txtDescripcion" name="Descripcion"
                    data-options="required:true"></input>
            </div>
        </fieldset>
    </div>

     <div id="panelDerecho" class="contenedorControlesMitad">
        <table id="dgMenuAcceso">
        </table>
     </div>
</asp:Content>
