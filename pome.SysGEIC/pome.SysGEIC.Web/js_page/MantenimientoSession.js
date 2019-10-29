

$(document).ready(function () {    
    //Temporizador para matener la sesión activa
    setInterval("MantenSesion()", 300000);
});

//Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
function MantenSesion() {
    var head = document.getElementsByTagName('head').item(0);
    script = document.createElement('script');
    script.src = "handlers/SessionHandler.ashx";
    script.setAttribute('type', 'text/javascript');
    script.defer = true;
    head.appendChild(script);
}