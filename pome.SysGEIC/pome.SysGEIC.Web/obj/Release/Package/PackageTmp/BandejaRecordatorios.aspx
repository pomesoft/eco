<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="BandejaRecordatorios.aspx.cs" Inherits="pome.SysGEIC.Web.BandejaRecordatorios" %>

<%@ Register src="Controles/RecordatorioControl.ascx" tagname="RecordatorioControl" tagprefix="uc1" %>

<%@ Register src="Controles/PlantillaSeleccionControl.ascx" tagname="PlantillaSeleccionControl" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">    
    <script src="js_page/BandejaRecordatorios.js" type="text/javascript"></script>
    <script src="js_page/Recordatorios.js" type="text/javascript"></script>
    <script src="js_page/PlantillasCtrolSeleccion.js" type="text/javascript"></script>
    <script src="js/spectrum.js" type="text/javascript"></script>
    <link href="css/spectrum.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelRecordatorios" style="width: 50%; float: left;">
        <div class="easyui-tabs">
            <div title="Recordatorios Activos" style="padding: 1px;">
                <div id="tbRecordatorios" style="padding: 2px; height: auto">
                    <a id="btnRecordatorioNuevo" href="#" class="easyui-linkbutton" iconcls="icon-add"
                        plain="true">Nuevo</a> <a id="btnRecordatorioEliminar" href="#" class="easyui-linkbutton"
                            iconcls="icon-remove" plain="true">Eliminar</a> <a id="btnRecordatorioGuardar" href="#"
                                class="easyui-linkbutton" iconcls="icon-ok" plain="true">Guardar</a>
                    <a id="btnRecordatorioCancelar" href="#" class="easyui-linkbutton" iconcls="icon-cancel"
                        plain="true">Cancelar</a>
                </div>
                <div>
                    <input type="text" id="txtRecordatorioBuscar" class="campoTextBuscador" />
                    <div id="menuRecordatorioBuscar" style="width: 250px">
                        <div data-options="name:'Todos'">
                            Listar todos</div>
                        <div data-options="name:'TipoRecordatorio'">
                            Tipo de Recordatorio</div>
                        <div data-options="name:'Descripcion'">
                            Descripción Recordatorio</div>
                        <div data-options="name:'CodigoEstudio'">
                            Código Estudio</div>
                        <div data-options="name:'RecordatoriosActivos'">
                            Listar Recordatorios Activos</div>
                    </div>
                </div>
                <table id="dg" toolbar="#tbRecordatorios">
                </table>
            </div>
        </div>
    </div>
    <div id="panelDatos" style="width: 50%; float: left;">
       <uc1:RecordatorioControl ID="RecordatorioControl1" runat="server" />
    </div>

    <uc2:PlantillaSeleccionControl ID="PlantillaSeleccionControl1" runat="server" />

</asp:Content>
