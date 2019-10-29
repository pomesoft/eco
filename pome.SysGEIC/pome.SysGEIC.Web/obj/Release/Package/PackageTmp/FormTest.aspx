<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormTest.aspx.cs" Inherits="pome.SysGEIC.Web.FormTest" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <%--<script src="js/jquery-ui.custom.min.js" type="text/javascript"></script>
    <script src="js/jquery.ui.ufd.min.js" type="text/javascript"></script>--%>
    <script src="js/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="js/easyui-lang-es.js" type="text/javascript"></script>
    <link href="css/Estilo.css" rel="stylesheet" type="text/css" />
    <%--<link href="css/jquery-ui.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/plain.css" rel="stylesheet" type="text/css" />
    <link href="css/ufd-base.css" rel="stylesheet" type="text/css" />--%>
    <link href="css/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="css/icon.css" rel="stylesheet" type="text/css" />
    <script src="js_page/Patologias.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="dd" class="easyui-datebox" required="required"></input>
    <div id="dg_toolbar" style="padding: 5px; height: auto">
        <div>
            Descripción:
            <input type="text" id="txtDescripcionBuscar" name="DescripcionBuscar" class="campoABM"></input>
            <a href="#" id="A1" name="btnBuscar" class="easyui-linkbutton" iconcls="icon-search"
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
            <label for="txtDescripcion">
                Descripción:</label>
            <input class="easyui-validatebox campoABM" type="text" id="txtDescripcion" name="txtDescripcion"
                data-options="required:true"></input>
        </div>
    </fieldset>

    <div class="panel  datagrid ">
        <div class="panel-header " style="width: 1186px;">
            <div class="panel-title">
                Información de Patologías</div>
        </div>
        <div class="datagrid-wrap panel-body " title="" id="" style="width: 1198px;">
            <asp:GridView ID="GridView1" runat="server" Width="1198px" 
                AutoGenerateColumns="False" GridLines="None"
                DataKeyNames="IdPatologia" DataSourceID="SqlDataSource1" 
                onrowdatabound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="IdPatologia" HeaderText="IdPatologia" 
                        InsertVisible="False" ReadOnly="True" SortExpression="IdPatologia">
                    </asp:BoundField>
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                        SortExpression="Descripcion">
                    </asp:BoundField>
                </Columns>                
                <HeaderStyle CssClass="datagrid-header datagrid-header-row" Height="25px" />
                <RowStyle CssClass="datagrid-body datagrid-row " Height="25px" />
                <SelectedRowStyle CssClass="datagrid-body datagrid-row-selected" Height="25px" />
            </asp:GridView>
        </div>
    </div>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:pmlSysGEICConnectionString %>"
        
        SelectCommand="SELECT [IdPatologia], [Descripcion] FROM [Patologias]">
        
    </asp:SqlDataSource>

    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </form>
</body>
</html>
