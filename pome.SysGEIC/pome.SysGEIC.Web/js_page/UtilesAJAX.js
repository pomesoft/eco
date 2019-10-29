function controladorAJAX_GET(url, dataParametros, funcionSuccess) {
    $('#dlgCargandoSite').dialog('open').dialog('setTitle', '');
    $.ajax({
        type: 'GET',
        url: url,
        data: dataParametros,
        contentType: 'application/json; charset=utf8',
        dataType: 'json',
        success: funcionSuccess,
        error: function (jqXHR, textStatus, errorThrown) {
            if (errorThrown.toString().indexOf('token') != -1)
                location.href = 'BandejaInicio.aspx';
            else
                $.messager.alert('Error', textStatus + ' ' + errorThrown, 'error');
        },
        complete: function () {
            //console.log('complete');
            $('#dlgCargandoSite').dialog('close');
            $("#submit").attr("disabled", false);
        }
    });
}

function controladorAJAX_POST(url, dataParametros, funcionSuccess) {
    $('#dlgCargandoSite').dialog('open').dialog('setTitle', '');
    $.post(url, dataParametros, function (data) {
        $('#dlgCargandoSite').dialog('close');
        funcionSuccess(data);
    });
}



function controladorAJAXPromise_GET(url, dataParametros) {
    $('#dlgCargandoSite').dialog('open').dialog('setTitle', '');

    return new Promise(function (resolve, reject) {
        $.ajax({
            type: 'GET',
            url: url,
            data: dataParametros,
            contentType: 'application/json; charset=utf8',
            dataType: 'json',
            success: function (data) {
                resolve(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                /*if (errorThrown.toString().indexOf('token') != -1)
                    location.href = 'BandejaInicio.aspx';*/
                reject(textStatus + ' ' + errorThrown);
            },
            complete: function () {
                $('#dlgCargandoSite').dialog('close');
                $("#submit").attr("disabled", false);
            }
        });
    });
}


function controladorAJAXPromise_POST(url, dataParametros) {
    $('#dlgCargandoSite').dialog('open').dialog('setTitle', '');
    
    return new Promise(function (resolve, reject) {

        $.post({
            type: 'POST',
            url: url,
            data: dataParametros,
            contentType: 'application/json; charset=utf8',
            dataType: 'json',
            success: function (data) {
                resolve(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                /*if (errorThrown.toString().indexOf('token') != -1)
                    location.href = 'BandejaInicio.aspx';*/
                reject(textStatus + ' ' + errorThrown);
            },
            complete: function () {
                $('#dlgCargandoSite').dialog('close');
                $("#submit").attr("disabled", false);
            }
        });
    });
}
