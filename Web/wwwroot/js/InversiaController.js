function addStyleSheets(href) {
    const head = document.head;
    const link = document.createElement('link');

    link.rel = 'stylesheet';
    link.id = "inversia-true";
    link.href = href;

    head.appendChild(link);
    
    mode = true;
}


function disableStyleSheets(href) {
    const head = document.head;
    const link = document.getElementById("inversia-true");

    head.removeChild(link);

    mode=false;
}


function switchMode(href) {
    if (mode == true) {
        disableStyleSheets(href);
    }
    else {
        addStyleSheets(href);
    }
}



const modeButton = document.getElementsByClassName("modeButton");
modeButton[0].addEventListener('click', () => switchMode("/css/ColorsInversia.css"));

let mode = false;