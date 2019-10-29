<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="Impresion.aspx.cs" Inherits="pome.SysGEIC.Web.Impresion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js/plugins/jquery.PrintArea.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnImpresionImprimir").click(function () {
                $("div#panelAreaImpresion").printArea();
            })


            $('#btnImpresionVolver').attr('href', getPaginaReturn() + '?IdEstudio=' + getURLParam("IdEstudio") + '&IdActa=' + getURLParam("IdActa"));
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="toolbar" style="padding: 3px; width: 1190px; border: 1px solid #ccc">
    <a href="#" id="btnImpresionImprimir" class="easyui-linkbutton" iconcls="icon-print" plain="true">
        Imprimir</a> 
    <a href="#" id="btnImpresionVolver" class="easyui-linkbutton" iconcls="icon-back" plain="true" ">
        Volver</a>
    </div>
    <div id="panelVistaPrevia" class="easyui-panel" 
        style="background-color: White; width:900px;height: 515px;padding:1px; border: thin solid #666666;"
        data-options="title:'Vista preva'">
        <div id="panelAreaImpresion" 
            style="background-color: White;">
            <asp:Literal ID="litHTML" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
