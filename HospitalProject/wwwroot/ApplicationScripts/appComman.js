
function SetAlertDiv(AlertDivID, isError, status, message) {
    var alertControl = $("#" + AlertDivID);
    if (isError) {
        alertControl.addClass('alert-danger').removeClass('');
    }
    else {
        alertControl.addClass('alert-success').removeClass('');
    }
    alertControl.find('span[id="lblStatus"]')[0].innerHTML = status;
    alertControl.find('span[id="lblMessage"]')[0].innerHTML = message;
    alertControl.fadeIn(1000).delay(5000).fadeOut(1000);
}