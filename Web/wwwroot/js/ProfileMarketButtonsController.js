function openProductAddWindow() {
    productAddWindow[0].hidden = false;
    alert('OPENED');
}

function closeProductAddWindow() {
    productAddWindow[0].hidden = true;
    alert('CLOSED');
}


function openSettingsWindow() {
    settingsWindow[0].hidden = false;
    alert('OPENED');
}

function closeSettingsWindow() {
    settingsWindow[0].hidden = true;
    alert('CLOSED');
}


const productAddWindow = document.getElementsByClassName("profile-product-add-window");
const settingsWindow = document.getElementsByClassName("profile-market-settings-window");

const productAddOpenButton = document.getElementById("profile-product-add");
const settingsOpenButton = document.getElementById("profile-market-settings");

productAddOpenButton.addEventListener('click', () => openProductAddWindow());
settingsOpenButton.addEventListener('click', () => openSettingsWindow());

const productAddCloseButton = document.getElementById("profile-product-add-close");
const settingsCloseButton = document.getElementById("profile-market-settings-close");

productAddCloseButton.addEventListener('click', () => closeProductAddWindow());
settingsCloseButton.addEventListener('click', () => closeSettingsWindow());
