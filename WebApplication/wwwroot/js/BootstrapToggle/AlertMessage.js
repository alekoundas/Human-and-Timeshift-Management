$(document).ready(() => $("#Alert").hide());


const Alert = (type, body) => {
    document.getElementById('AlertBody').innerHTML = body;
    $("#Alert").fadeTo(2000, 500).slideUp(500, () => $("Alert").slideUp(500));

}
