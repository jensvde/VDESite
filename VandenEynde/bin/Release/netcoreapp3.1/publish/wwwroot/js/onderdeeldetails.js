var Dt = document.getElementById("Dt0");
Dt.style.display = 'none';
var Dd = document.getElementById("Dd0");
Dd.style.display = 'none';

for (i = 0; i < 10; i++) {
    var idForDt = 'input' + i;
    var inpute = document.getElementById(idForDt);
    inpute.style.display = 'none';
    var idForDt = 'inputAuto' + i;
    var inpute = document.getElementById(idForDt);
    inpute.style.display = 'none';
    var idForDt = 'inputBestel' + i;
    var inpute = document.getElementById(idForDt);
    inpute.style.display = 'none';
    var idTo = 'autoDt' + i;
    var inpute = document.getElementById(idTo);
    inpute.style.display = 'none';
    var idTo2 = 'autoDd' + i;
    var inputee = document.getElementById(idTo2);
    inputee.style.display = 'none';
    
    if (i > 0) {
        var idForAlink = 'aLink' + i;
        var inpute = document.getElementById(idForAlink);
        inpute.style.display = 'none';
    }
    }


function toggle_visibility(id) {
    var idTo = 'Dt0';
    var Dt = document.getElementById(idTo);
    Dt.style.display = 'block';

    var idTo = 'input' + id;
    var inpute = document.getElementById(idTo);
    inpute.style.display = 'block';

    if (id > 0) {
        var idold = +id - +1;
        var idTo = 'input' + idold;
        var inpute = document.getElementById(idTo);
        inpute.style.display = 'none';

        var idTo = 'selectBestel' + idold;
        var e = document.getElementById(idTo);
        var strUser = e.value;

        var idTo = 'inputBestel' + idold;
        var inpute = document.getElementById(idTo);
        inpute.value = strUser;
        inpute.style.display = 'block';
    }
    var idTo = 'Dd0';
    var Dd = document.getElementById(idTo);
    Dd.style.display = 'block';
    
    var idForA = 'aLink' + id;
    var aLink = document.getElementById(idForA);
    aLink.style.display = 'none';
    var x = +id + +1;
    var idForA = 'aLink' + x;
    var aLink = document.getElementById(idForA);
    aLink.style.display = 'block';
}

function toggle_visibility_auto(id) {
    
    var idTo = 'autoDt' + id;
    var inpute = document.getElementById(idTo);
    inpute.style.display = 'block';
    var idTo = 'autoDd' + id; 
    var inpute = document.getElementById(idTo);
    inpute.style.display = 'block';

    
    if (+id > 0) {
        var idold = +id - +1;
    var idTo = 'autoDt' + idold;
    var inpute = document.getElementById(idTo);
    inpute.style.display = 'none';
    var idTo = 'autoDd' + idold;
    var inpute = document.getElementById(idTo);
        inpute.style.display = 'none';

        var idTo = 'select' + idold;
        var e = document.getElementById(idTo);
        var strUser = e.options[e.selectedIndex].text;

        var idTo = 'inputAuto' + idold;
        var inpute = document.getElementById(idTo);
        inpute.value = strUser;
        inpute.style.display = 'block';
    }

    var idForA = 'bLink' + id;
    var aLink = document.getElementById(idForA);
    aLink.style.display = 'none';
    var x = +id + +1;
    var idForA = 'bLink' + x;
    var aLink = document.getElementById(idForA);
    aLink.style.display = 'block';
    
}
    

function newFunction() {

}

