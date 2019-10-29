<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="TiposRecordatorio.aspx.cs" Inherits="pome.SysGEIC.Web.TiposRecordatorio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/TiposRecordatorio.js" type="text/javascript"></script>
    <script src="js/spectrum.js" type="text/javascript"></script>
    <link href="css/spectrum.css" rel="stylesheet" type="text/css" />
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
    <div id="toolbar" style="padding: 3px; width: 950px; border: 1px solid #ccc">
        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
            Nuevo</a> 
        <%--<a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
            onclick="eliminar()">Eliminar</a> --%>
        <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                    plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                        iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
    </div>
    <fieldset>
        <div class="fitem">
            <label class="labelAncho" for="Descripcion">
                Descripción:</label>
            <input class="easyui-validatebox campoABMLargo" type="text" id="txtDescripcion" name="Descripcion"
                data-options="required:true"></input>
        </div>
        <div class="fitem">
            <label class="labelAncho" for="AvisoMail">
                Alerta vía E-Mail:</label>
            <input type="checkbox" id="chkAvisoMail" name="AvisoMail" />
            <label class="labelAncho" for="Seperador">
            </label>
            <label class="labelAncho" for="AvisoPopup">
                Alerta con Popup:</label>
            <input type="checkbox" id="chkAvisoPopup" name="AvisoPopup" />
            <label class="labelAncho" for="Seperador">
            </label>
            <label for="Color">
                Color:</label>
            <input id='SelectorColor' />
        </div>
    </fieldset>
</asp:Content>
