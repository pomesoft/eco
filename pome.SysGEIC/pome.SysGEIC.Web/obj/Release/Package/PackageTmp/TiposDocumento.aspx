<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="TiposDocumento.aspx.cs" Inherits="pome.SysGEIC.Web.TiposDocumento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/TiposDocumento.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div id="panelIzquierdo" class="contenedorControlesMitad">
        <div id="dg_toolbar" style="padding: 5px; height: auto">
            <div>
                <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="nuevo()">
                    Nuevo</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                        onclick="eliminar()">Eliminar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-ok"
                            plain="true" onclick="guardar()">Guardar</a> <a href="#" class="easyui-linkbutton"
                                iconcls="icon-cancel" plain="true" onclick="cancelar()">Cancelar</a>
                <div>
                    <input type="text" id="txtTipoDocumentoBuscar" class="campoTextBuscador" />
                    <div id="menuTipoDocumentoBuscar" style="width: 250px">
                        <div data-options="name:'Descripcion'">
                            Descripción</div>
                    </div>
                </div>
            </div>
        </div>
        <table id="dg" toolbar="#dg_toolbar">
        </table>
    </div>
    <div id="panelDerecho" class="contenedorControlesMitad">
        <div id="panelDatos" style="padding-left: 15px">
            <div class="fitem">
                <label class="label" for="Descripcion">
                    Descripción:</label>
                <input class="easyui-textbox campoABMLargo" type="text" id="txtDescripcion" name="Descripcion"
                    data-options="required:true"></input>
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereVersion" name="RequiereVersion" />
                <label class="labelAncho300Izq" for="RequiereVersion">
                    Requiere Versión:</label>                
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkCartaRespuesta" name="CartaRespuesta" />
                <label class="labelAncho300Izq" for="CartaRespuesta">
                    Inluir Cartas de Respuesta:</label>                                
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkListarDocsCartaRespuesta" name="ListarDocsCartaRespuesta" />
                <label class="labelAncho300Izq" for="CartaRespuesta">
                    Listar en la Aprobación del Estudio:</label>
            </div>                
            <div class="fitem">
                <input type="checkbox" id="chkNecesarioAprobacionEstudio" name="NecesarioAprobacionEstudio" />
                <label class=" labelAncho300Izq" for="NecesarioAprobacionEstudio">Obligatorio para la aprobación del estudio:</label>                
            </div>
            <div class="fitem">
                <label class="labelAncho" for="TipoDocumentoGrupo">
                    Tratamiento en Acta:</label>
                <input class="easyui-combobox" id="cboTipoDocumentoGrupo" name="TipoDocumentoGrupo"
                    style="width: 300px" data-options="
				url:'handlers/TiposDocumentoHandler.ashx?accion=LISTAR_TIPODOCUMENTOGRUPOS',
				valueField:'Id',
				textField:'Descripcion',
				panelHeight:'auto',
                            required:true" />
            </div>
            
            <div class="fsubtitle">Recoratorios y Alertas</div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereAlertaInactividad" />
                <label class="labelAncho300Izq">
                    Requiere alerta por Inactividad de Documento:</label>
                <label class="label">
                    Meses:</label>
                <input id="txtAlertaInactividadMeses" class="easyui-numberspinner" style="width: 60px;" data-options="min:1,max:12,editable:true" />
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereAlertaInformeAvance" />
                <label class="labelAncho300Izq">
                    Requiere alerta por Informe de Avance:</label>                    
                <label class="label">
                    Meses:</label>
                <input id="txtAlertaInformeAvenceMeses" class="easyui-numberspinner" style="width: 60px;" data-options="min:1,max:12,editable:true" />
            </div>
            <div class="fitem">
                <input type="checkbox" id="chkRequiereAlertaVencimiento" />
                <label class="labelAncho300Izq">
                    Requiere alerta por Vencimiento:</label>                    
                <label class="label">
                    Meses:</label>
                <input id="txtAlertaVencimientoMeses" class="easyui-numberspinner" style="width: 60px;" data-options="min:1,max:12,editable:true" />
            </div>
            <table id="dgFlujos">
            </table>

            <div id="toolbar_dgFlujoEstados" style="display: none; padding-left: 15px; width: 587px;
            border: 1px solid #808080">
            <div style="float: left;">
                <div class="fitem">
                    <label for="Estado">
                        Estado:</label>
                    <asp:DropDownList ID="cboEstado" name="Estado" runat="server" CssClass="easyui-combobox"
                        Width="300px">
                    </asp:DropDownList>
                </div>
                <div class="fitem">
                    <label for="Estado">
                        Estado Padre:</label>
                    <asp:DropDownList ID="cboEstadoPadre" name="Estado" runat="server" CssClass="easyui-combobox"
                        Width="300px">
                    </asp:DropDownList>
                </div>
            </div>
            <div style="float: left; padding-left: 10px;">
                <div class="fitem">
                    <label for="Final">
                        Final:</label>
                    <input type="checkbox" id="chkFinal" name="Final" />
                </div>
                <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="agregarEstado()">
                    Agregar</a> <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true"
                        onclick="eliminarEstado()">Quitar</a>
            </div>
            <%--<table id="dgFlujoEstados" toolbar="toolbar_dgFlujoEstados"></table>--%>
        </div>
        <table id="dgFlujoEstados" toolbar="toolbar_dgFlujoEstados">
        </table>
        </div>
        
        
    </div>
</asp:Content>
