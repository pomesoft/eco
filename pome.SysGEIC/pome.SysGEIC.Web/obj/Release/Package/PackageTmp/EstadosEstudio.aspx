<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="EstadosEstudio.aspx.cs" Inherits="pome.SysGEIC.Web.EstadosEstudio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/EstadosEstudio.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="dg_toolbar" style="padding: 5px; height: auto">
        <div>
            Descripción:
            <input type="text" id="txtDescripcionBuscar" name="DescripcionBuscar" class="campoABM"></input>
            <a href="#" id="btnBuscar" name="btnBuscar" class="easyui-linkbutton" iconcls="icon-search"
                plain="true">Buscar</a>
        </div>
    </div>
    <table id="dg" toolbar="#dg_toolbar">
    </table>
    <div id="toolbar" style="padding: 3px; width: 100%; border: 1px solid #ccc; margin-bottom: 10px;">
        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
            Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                onclick="eliminar()">Eliminar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                    plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                        iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
    </div>
    <div class="fitem">
        <label class="labelAncho" for="txtDescripcion">
            Descripción:</label>
        <input class="easyui-validatebox campoABM" type="text" id="txtDescripcion" name="txtDescripcion"
            data-options="required:true"></input>
    </div>
    <div class="fitem">
        <label class="labelAncho" for="chkFinal">
            Estado final:</label>
        <input type="checkbox" id="chkFinal" name="chkFinal"></input>
    </div>

</asp:Content>
