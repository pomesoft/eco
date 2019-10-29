var prompBusqueda = 'Por favor, ingrese texto a buscar';


function getURLParam(strParamName) {
    var strReturn = "";
    var strHref = window.location.href;
    if (strHref.indexOf("?") > -1) {
        var strQueryString = strHref.substr(strHref.indexOf("?")).toLowerCase();
        var aQueryString = strQueryString.split("&");
        for (var iParam = 0; iParam < aQueryString.length; iParam++) {
            if (aQueryString[iParam].indexOf(strParamName.toLowerCase() + "=") > -1) {
                var aParam = aQueryString[iParam].split("=");
                strReturn = aParam[1];
                break;
            }
        }
    }
    var valorReturn = '';

    for (i = 0; i < strReturn.length; i++) {
        if (strReturn.charAt(i) != "#")
            valorReturn = valorReturn + strReturn.charAt(i);
    }

    return unescape(valorReturn);
}

function getPaginaReturn() {
    //por default el volver es a BandejaInicio.aspx
    var urlReturn = 'BandejaInicio.aspx';
    var param = getURLParam('PaginaReturn');

    if (param) urlReturn = param + '.aspx';
    
    return urlReturn;
}
/*
switch (param) {
            case '1':
                urlReturn = 'BandejaInicio.aspx';
                break;
            case '2':
                urlReturn = 'BandejaInicioActas.aspx';
                break;
            default:
                urlReturn = 'BandejaInicio.aspx';
                break;
        }
*/

// retorna verdadero si es un número
function isDigit(c) {
    return ((c >= "0") && (c <= "9"))
}

function formatterDateBox(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return d + '/' + m + '/' + y;
}


var meses = new Array ("Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre");

function formatterDateBoxLarge(s) {
    //var fecha = parserDateBox(s);
    var fecha = s;
    var fechaReturn = fecha.getDate() + ' de ' + meses[fecha.getMonth()] + ' de ' + fecha.getFullYear();
    return fechaReturn;
}

function parserDateBox(s) {
    if (!s) return new Date();
    var ss = s.split('/');
    var y = parseInt(ss[2], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[0], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}

function getFechaActual() {
    var currentDate = new Date();
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1;
    var year = currentDate.getFullYear();
    return day + "/" + month + "/" + year;
}
/**
 * Funcion que devuelve la fecha modificada n dias
 * Tiene que recibir el numero de dias en positivo o negativo para sumar o 
 * restar a la fecha actual.
 * Ejemplo:
 *  mostrarFecha(-10) => restara 10 dias a la fecha actual
 *  mostrarFecha(30) => añadira 30 dias a la fecha actual
 */
function sumarDiasFecha(fecha, days){
    milisegundos=parseInt(35*24*60*60*1000);
    
    day=fecha.getDate();
    // el mes es devuelto entre 0 y 11
    month=fecha.getMonth()+1;
    year=fecha.getFullYear();   
    
    //Obtenemos los milisegundos desde media noche del 1/1/1970
    tiempo=fecha.getTime();
    //Calculamos los milisegundos sobre la fecha que hay que sumar o restar...
    milisegundos=parseInt(days*24*60*60*1000);
    //Modificamos la fecha actual
    total=fecha.setTime(tiempo+milisegundos);
    day=fecha.getDate();
    month=fecha.getMonth()+1;
    year=fecha.getFullYear();

    return day + '/' + month + '/' + year;
}

function maximaLongitud(texto, maxlong) {
    var tecla, in_value, out_value;

    if (texto.value.length > maxlong) {
        in_value = texto.value;
        out_value = in_value.substring(0, maxlong);
        texto.value = out_value;
        return false;
    }
    return true;
}

function SeleccionarComboDefault (combo) {
    var data = $(combo).combobox("getData");
    if (data && data.length == 1)
        $(combo).combobox("select", data[0].Id)
}

function inicizalizarMouseHoverTabs(panelTabs) {
    var tabs = $('#' + panelTabs).tabs().tabs('tabs');
    for (var i = 0; i < tabs.length; i++) {
        tabs[i].panel('options').tab.unbind().bind('mouseenter', { index: i }, function (e) {
            $('#' + panelTabs).tabs('select', e.data.index);
        });
    }
}

/**
* Función que permite reemplazar TODAS las subcadenas encontradas
* en un string por otra nueva subcadena.
*/
function replaceAll(text, search, newstring) {
    if (text == '') return;
    var out = text.replace(new RegExp(search, 'g'), newstring);
    return out;
}

function formatearTextEditor(texto) {
    if (texto == '<br />') texto = '';
    if (texto == '<br>') texto = '';
    if (texto == '<p><br></p>') texto = '';
    return $.trim(texto);
}

function formatearTextoHTML(texto) {
    if (texto && texto.length > 0) {
        return replaceAll(texto, '\n', '<br />');
    } else {
        return '';
    }
}

function formatearTextoASCII(texto) {
    if (texto && texto.length > 0) {
        texto = replaceAll(texto, '<br />', '\n');
        text = replaceAll(texto, '</br>', '\n');
    } else {
        return '';
    }
    return texto;
}

function formatearCaracteres(texto) {
    if (texto && texto.length > 0) {
        texto = replaceAll(texto, '“', '"');
        texto = replaceAll(texto, '”', '"');
    } else {
        return '';
    }
    return texto;
}


function incializarControlColor(selectorColor) {
    $('#' + selectorColor).spectrum({
        flat: false,
        showInput: false,
        chooseText: "Aceptar",
        cancelText: "Cancelar",
        color: 'white',
        showPaletteOnly: true,
        showPalette: true,
        palette: [
		    ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
		    ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
		    ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
		    ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
		    ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
		    ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
		    ["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
		    ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
	  ]
    });
}