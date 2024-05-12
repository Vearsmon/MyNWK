function openProductAddWindow() {
    productAddWindow.hidden = false;
    alert('OPENED');
}

function closeProductAddWindow() {
    productAddWindow.hidden = true;
    alert('CLOSED');
}


function openSettingsWindow() {
    settingsWindow.hidden = false;
    fetch('api/get/market/info', {method: 'get'})
        .then((response) => response.json())
        .then((marketInfo) => {
            const settings = settingsWindow.getElementsByClassName('settings-form-container')[0];
            console.log(marketInfo);
            settings.innerHTML = `
            <div class="settings-header">Настройте свой магазин</div>
            
            <input hidden='true' type="number" name="id" value="${marketInfo.id}"/>
            
            <div class="settings-content">
                Название магазина
            </div>

            <input class="settings-fields" type="text" name="name" value="${marketInfo.name}"/>

            <div class="settings-content">
                Описание магазина
            </div>

            <input class="settings-fields" type="text" name="description" value="${marketInfo.description}"/>

            <div class="settings-content">
                Время работы от:
            </div>

            <input id="time-from" class="settings-fields" type="time" name="worksFrom" value="${marketInfo.worksFrom}"/>

            <div class="settings-content">
                Время работы до:
            </div>

            <input id="time-to" class="settings-fields" type="time" name="worksTo" value="${marketInfo.worksTo}"/>
            
            <div class="settings-buttons">
                <input class="settings-accept" type="submit" value="Изменить"/>
            </div>`;
            
            
        })
}

function closeSettingsWindow() {
    settingsWindow.hidden = true;
    alert('CLOSED');
}

function closeProductInfoUpdateWindow() {
    productInfoUpdateWindow[0].hidden = true;
    alert('CLOSED');
}

const productAddWindow = document.getElementsByClassName("profile-product-add-window")[0];
const settingsWindow = document.getElementsByClassName("profile-market-settings-window")[0];
const productInfoUpdateWindow = document.getElementsByClassName("profile-market-info-update-window")[0];

const productAddOpenButton = document.getElementById("profile-product-add");
const settingsOpenButton = document.getElementById("profile-market-settings");

productAddOpenButton.addEventListener('click', () => openProductAddWindow());
settingsOpenButton.addEventListener('click', () => openSettingsWindow());

const productAddCloseButton = document.getElementById("profile-product-add-close");
const settingsCloseButton = document.getElementById("profile-market-settings-close");
const productInfoUpdateCloseButton = document.getElementById("profile-market-info-update-close");

productAddCloseButton.addEventListener('click', () => closeProductAddWindow());
settingsCloseButton.addEventListener('click', () => closeSettingsWindow());
productInfoUpdateCloseButton.addEventListener('click', () => closeProductInfoUpdateWindow());

