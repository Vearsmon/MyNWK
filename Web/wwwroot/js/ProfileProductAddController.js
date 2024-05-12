function openProductAddWindow() {
    productAddWindow[0].hidden = false;
    //alert('OPENED');
}

function closeProductAddWindow() {
    productAddWindow[0].hidden = true;
    //alert('CLOSED');
}


const productAddWindow = document.getElementsByClassName("profile-product-add-window");

const openButton = document.getElementById("profile-product-add");
openButton.addEventListener('click', () => openProductAddWindow());

const closeButton = document.getElementsByClassName("product-card-background-shadow");
closeButton[0].addEventListener('click', () => closeProductAddWindow());

