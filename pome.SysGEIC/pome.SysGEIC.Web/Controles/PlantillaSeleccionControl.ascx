<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlantillaSeleccionControl.ascx.cs"
    Inherits="pome.SysGEIC.Web.Controles.PlantillaSeleccionControl" %>
<div id="dlgSeleccionPlantillaTexto" class="easyui-dialog" style="width: 750px; height: 530px;
    padding: 10px 20px" closed="true" modal="true" buttons="#dlgSeleccionPlantillaTexto-buttons">
    <div class="ftitle">
        Selección Plantilla de Texto</div>
    <fieldset>
        <div class="fitem">
            <label for="Plantilla">
                Plantilla:</label>
            <input class="easyui-combobox" id="ctrolSeleccionPlantilla_cboPlantilla" name="Plantilla"
                style="width: 500px" />
        </div>
        <div id="ctrolSeleccionPlantilla_Texto" style="width: '90%'; height: 360px; padding: 20px"></div>
    </fieldset>

</div>
<div id="dlgSeleccionPlantillaTexto-buttons">
    <a href="#" class="easyui-linkbutton" iconcls="icon-ok" onclick="ctrolSeleccionPlantilla_aceptar()">
	  Aceptar</a> 
    <a href="#" class="easyui-linkbutton" iconcls="icon-cancel" onclick="ctrolSeleccionPlantilla_cancelar()">
	  Cancelar</a>
</div>