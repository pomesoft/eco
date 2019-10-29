<%@ Page Title="" Language="C#" MasterPageFile="~/Sitio.Master" AutoEventWireup="true"
    CodeBehind="CartaRespuestaModelo.aspx.cs" Inherits="pome.SysGEIC.Web.CartaRespuestaModelo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHead" runat="server">
    <script src="js_page/CartaRespuestaModelo.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceBody" runat="server">
    <div class="contenedorControlesMitad">
        <div id="tbCartaRespuestaModelo" style="padding: 2px; height: auto">
            <div>
                <input type="text" id="txtCartaRespuestaModeloBuscar" class="campoTextBuscador" />
                <div id="menuCartaRespuestaModeloBuscar" style="width: 250px">
                    <div data-options="name:'Todos'">
                        Listar todos</div>
                    <div data-options="name:'Descripcion'">
                        Descripción</div>
                </div>
            </div>
            <a id="btnCartaRespuestaModeloNuevo" href="#" class="easyui-linkbutton" iconcls="icon-add"
                plain="true">Nuevo</a> <a id="btnCartaRespuestaModeloEliminar" href="#" class="easyui-linkbutton"
                    iconcls="icon-remove" plain="true">Eliminar</a> <a id="btnCartaRespuestaModeloGuardar"
                        href="#" class="easyui-linkbutton" iconcls="icon-ok" plain="true">Guardar</a>
            <a id="btnCartaRespuestaModeloCancelar" href="#" class="easyui-linkbutton" iconcls="icon-cancel"
                plain="true">Cancelar</a>
        </div>
        <table id="dgModelos" toolbar="#tbCartaRespuestaModelo">
        </table>
    </div>
    <div class="contenedorControlesMitad">
        <div id="panelDatos" style=" padding:5px;">
            <fieldset>
                <div class="fitem">
                    <label for="Descripcion">
                        Descripción:</label>
                    <input class="easyui-textbox campoABMLargo" type="text" id="txtDescripcion" name="Descripcion" />
                </div>
                <div class="fsubtitle">
                    Incluir:
                </div>
                <div class="fitem">
                    <label class="labelAncho200Der">
                        Documentos Evaluados</label>
                    <input type="checkbox" id="chkIncluirDocumentosEvaluados" />
                    <label>
                    </label>
                    <label class="labelAncho200Der">
                        Firma presidente del comité</label>
                    <input type="checkbox" id="chkIncluirFirmaPresidente" />
                </div>
                <div class="fitem">
                    <label class="labelAncho200Der">
                        Documentos Toma Conocimiento</label>
                    <input type="checkbox" id="chkIncluirDocumentosTomaConocimiento" />
                    <label>
                    </label>
                    <label class="labelAncho200Der">
                        Firma todos los integrantes del comité</label>
                    <input type="checkbox" id="chkIncluirFirmaMiembros" />
                </div>
                <div class="fitem">
                    <label class="labelAncho200Der">
                        Documentos con Pedido de Cambio</label>
                    <input type="checkbox" id="chkIncluirDocumentosPedidoCambio" />                   
                </div>
                <div class="fitem">
                    <label class="labelAncho200Der">
                        Todos los Documentos del Estudio</label>
                    <input type="checkbox" id="chkIncluirTodosDocumentosEstudio" />                   
                </div>
                <div class="fsubtitle">
                    Plantillas:
                </div>
                <div class="fitem">
                    <label class="labelAncho">
                        Intorducción:</label>
                    <asp:DropDownList ID="cboPlantillaIntroduccion" runat="server" CssClass="easyui-combobox"
                        Width="400px">
                    </asp:DropDownList>
                </div>
                <div class="fitem">
                    <label class="labelAncho">
                        Intorducción opcional:</label>
                    <asp:DropDownList ID="cboPlantillaIntroduccion2" runat="server" CssClass="easyui-combobox"
                        Width="400px">
                    </asp:DropDownList>
                </div>
                <div class="fitem">
                    <label class="labelAncho">
                        Pie de página:</label>
                    <asp:DropDownList ID="cboPlantillaPiePagina" runat="server" CssClass="easyui-combobox"
                        Width="400px">
                    </asp:DropDownList>
                </div>
                 <div class="fitem">
                    <label class="labelAncho">
                        Texto aprobación:</label>
                    <asp:DropDownList ID="cboPlantillaTextoAprobacion" runat="server" CssClass="easyui-combobox"
                        Width="400px">
                    </asp:DropDownList>
                </div>
                <div class="fitem">
                    <label class="labelAncho">
                        Buenas prácticas:</label>
                    <asp:DropDownList ID="cboPlantillaBuenasPracticas" runat="server" CssClass="easyui-combobox"
                        Width="400px">
                    </asp:DropDownList>
                </div>
                <div class="fitem">
                    <label class="labelAncho">
                        Texto firma presidente:</label>
                    <asp:DropDownList ID="cboPlantillaTextoFirmaPresidente" runat="server" CssClass="easyui-combobox"
                        Width="400px">
                    </asp:DropDownList>
                </div>
                <div class="fsubtitle">
                    Texto libre:
                </div>
                <div class="fitem">
                   <input class="easyui-textbox" type="text" id="txtTextoLibre" data-options="multiline:true" style=" width: 550px; height: 140px;"/>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
