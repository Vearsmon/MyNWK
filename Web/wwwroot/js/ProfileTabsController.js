function loadPurchases() {
    const getProductsByBuyer = new URL('http://127.0.0.1:80/products/get/byBuyer');
    const container = document.getElementsByClassName("profile-purchases-container")[0];
    container.innerHTML = '';

    fetch(getProductsByBuyer, {method: 'get'})
        .then((response) => response.json())
        .then((orders) => {
            for (order of orders) {
                const header = document.createElement('div');
                header.setAttribute('class', 'profile-purchases-header');
                header.innerHTML = `Номер заказа: ${order["orderId"]}`;
                
                const purchases = document.createElement('div');
                purchases.setAttribute('class', 'profile-purchases');
                for (product of order["products"]) {
                    const productImage = document.createElement('img');
                    productImage.setAttribute('src', product.imageRef);
                    productImage.setAttribute('class', 'profile-purchase-photo');
    
                    const productPrice = document.createElement('p');
                    productPrice.innerText = `${product['price']} р.`;

                    const number = document.createElement('p');
                    number.innerText = `${product['remained']} шт.`;
    
                    const productInfo = document.createElement('div');
                    productInfo.setAttribute('class', 'profile-purchase-info');
                    productInfo.textContent = product['title'];
                    productInfo.appendChild(productPrice);
                    productInfo.appendChild(number);
    
                    const productSlot = document.createElement('div');
                    productSlot.setAttribute('class', 'profile-purchase');
                    productSlot.appendChild(productImage);
                    productSlot.appendChild(productInfo);

                    purchases.appendChild(productSlot);
                }
                
                const innerContainer = document.createElement('div');
                innerContainer.setAttribute('class', 'profile-purchases-inner-container');
                innerContainer.appendChild(header);
                innerContainer.appendChild(purchases);

                console.log(order);
                if (order["workflowState"] === 4) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-purchases-status");
                    div.innerHTML = `Отменён
                    <img src="/assets/otmeneno.png" width="15px" height="15px">`;
                    innerContainer.appendChild(div);
                } else if (order["workflowState"] === 3) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-purchases-status");
                    div.innerHTML = `Получен
                    <img src="/assets/poluchen.png" width="15px" height="15px">`;
                    innerContainer.appendChild(div);
                } else if (order["workflowState"] === 2) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-purchases-status");
                    div.innerHTML = `Принят в работу
                    <img src="/assets/prinyato.png" width="15px" height="15px">`;
                    innerContainer.appendChild(div);
                    
                    const button = document.createElement('button');
                    button.setAttribute('class', "profile-purchases-accept");
                    button.setAttribute('id', `accept-${order["orderId"]}`);
                    button.innerHTML = 'Подтвердить получение';
                    button.addEventListener('click', async (event) => {
                        const confirm = new URL('http://127.0.0.1:80/orders/confirm');
                        const id = event.target.getAttribute('id');
                        confirm.search = new URLSearchParams({orderId: id.substring(id.indexOf('-') + 1)}).toString();
                        fetch(confirm, {method: 'get'})
                            .then(() => loadPurchases());
                    });
                    innerContainer.appendChild(button);
                } else if (order["workflowState"] === 1) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-purchases-status");
                    div.innerHTML = `Ожидание ответа продавца`;
                    innerContainer.appendChild(div);
                } else {
                    const button = document .createElement('button');
                    button.setAttribute('class', "profile-purchases-accept");
                    button.setAttribute('id', `accept-${order["orderId"]}`);
                    button.innerHTML = 'Подтвердить получение';
                    innerContainer.appendChild(button);
                }
                getUserInfo = new URL('http://127.0.0.1:80/api/get/user/info');
                getUserInfo.search = new URLSearchParams({userId: order["products"][0]["fullId"]["userId"]}).toString();
                console.log(order);
                fetch(getUserInfo, {method: 'get'})
                    .then((response) => response.json())
                    .then((userId) => {
                        console.log(userId);
                        const userDiv = document.createElement('div');
                        userDiv.setAttribute('class', "profile-purchases-status");
                        userDiv.innerHTML = `@${userId["username"]}`;
                        container.appendChild(innerContainer);
                    });
            }
        });
}

function loadProducts() {
    const getProductsByUser = new URL('http://127.0.0.1:80/products/get/byUser');
    const container = document.getElementsByClassName("profile-products-inner-container")[0];
    container.innerHTML = '';
        
    fetch(getProductsByUser, {method: 'get'})
        .then((response) => response.json())
        .then((products) => {
            let i = 0;
            for (let product of products) {
                const productImage = document.createElement('img');
                productImage.setAttribute('src', product.imageRef);
                productImage.setAttribute('class', 'profile-product-photo');
                productImage.setAttribute('id', `profile-product-photo-id-${i}`)

                const productPrice = document.createElement('p');
                productPrice.innerText = `${product['price']} р.`;

                const productInfo = document.createElement('div');
                productInfo.setAttribute('class', 'profile-product-info');
                productInfo.textContent = product['title'];
                productInfo.appendChild(productPrice);

                const productSlot = document.createElement('div');
                productSlot.setAttribute('class', 'profile-product');
                productSlot.appendChild(productImage);
                productSlot.appendChild(productInfo);

                container.appendChild(productSlot);
                document.getElementById(`profile-product-photo-id-${i}`)
                    .addEventListener('click', async (event) => {
                        const id = event.target.getAttribute('id').split('-').pop();
                        await openProductInfoUpdateWindow(products[Number(id)]);
                    });
                i++;
            }
        });
}

function loadOrders() {
    const getProductsBySeller = new URL('http://127.0.0.1:80/products/get/bySeller');
    const container = document.getElementsByClassName("profile-orders-container")[0];
    container.innerHTML = '';

    fetch(getProductsBySeller, {method: 'get'})
        .then((response) => response.json())
        .then((orders) => {
            for (order of orders) {
                const header = document.createElement('div');
                header.setAttribute('class', 'profile-orders-header');
                header.innerHTML = `Номер заказа: ${order["orderId"]}`;
                
                const orders = document.createElement('div');
                orders.setAttribute('class', 'profile-orders');
                for (product of order["products"]) {
                    const productImage = document.createElement('img');
                    productImage.setAttribute('src', product.imageRef);
                    productImage.setAttribute('class', 'profile-order-photo');
    
                    const productPrice = document.createElement('p');
                    productPrice.innerText = `${product['price']} р.`;
    
                    const number = document.createElement('p');
                    number.innerText = `${product['remained']} шт.`;

                    const productInfo = document.createElement('div');
                    productInfo.setAttribute('class', 'profile-order-info');
                    productInfo.textContent = product['title'];
                    productInfo.appendChild(productPrice);
                    productInfo.appendChild(number);
    
                    const productSlot = document.createElement('div');
                    productSlot.setAttribute('class', 'profile-order');
                    productSlot.appendChild(productImage);
                    productSlot.appendChild(productInfo);

                    orders.appendChild(productSlot);
                }
                
                const innerContainer = document.createElement('div');
                innerContainer.setAttribute('class', 'profile-orders-inner-container');
                innerContainer.appendChild(header);
                innerContainer.appendChild(orders);

                if (order["workflowState"] === 4) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-orders-status");
                    div.innerHTML = `Отменён
                    <img src="/assets/otmeneno.png" width="15px" height="15px">`;
                    innerContainer.appendChild(div);
                } else if (order["workflowState"] === 3) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-orders-status");
                    div.innerHTML = `Получен
                    <img src="/assets/poluchen.png" width="15px" height="15px">`;
                    innerContainer.appendChild(div);
                } else if (order["workflowState"] === 2) {
                    const div = document.createElement('div');
                    div.setAttribute('class', "profile-orders-status");
                    div.innerHTML = `Принят в работу
                    <img src="/assets/prinyato.png" width="15px" height="15px">`;
                    innerContainer.appendChild(div);

                    const cancelButton = document.createElement('button');
                    cancelButton.setAttribute('class', "profile-orders-cancel");
                    cancelButton.setAttribute('id', `cancel-${order["orderId"]}`);
                    cancelButton.innerHTML = 'Отменить заказ';
                    cancelButton.addEventListener('click', async (event) => {
                        const id = event.target.getAttribute('id');
                        const cancel = new URL('http://127.0.0.1:80/orders/cancel');
                        cancel.search = new URLSearchParams({orderId: id.substring(id.indexOf('-') + 1)}).toString();
                        fetch(cancel, {method: 'get'})
                            .then(() => loadOrders());
                    });
                    innerContainer.appendChild(cancelButton);
                } else if (order['workflowState'] === 1) {
                    const cancelButton = document.createElement('button');
                    cancelButton.setAttribute('class', "profile-orders-cancel");
                    cancelButton.setAttribute('id', `cancel-${order["orderId"]}`);
                    cancelButton.innerHTML = 'Отменить заказ';
                    cancelButton.addEventListener('click', async (event) => {
                        const id = event.target.getAttribute('id');
                        const cancel = new URL('http://127.0.0.1:80/orders/cancel');
                        cancel.search = new URLSearchParams({orderId: id.substring(id.indexOf('-') + 1)}).toString();
                        fetch(cancel, {method: 'get'})
                            .then(() => loadOrders());
                    });
                    innerContainer.appendChild(cancelButton);
                    
                    const confirmButton = document.createElement('button');
                    confirmButton.setAttribute('class', "profile-orders-accept");
                    confirmButton.setAttribute('id', `cancel-${order["orderId"]}`);
                    confirmButton.innerHTML = 'Взять в работу';
                    confirmButton.addEventListener('click', async (event) => {
                        const id = event.target.getAttribute('id');
                        const cancel = new URL('http://127.0.0.1:80/orders/confirm');
                        cancel.search = new URLSearchParams({orderId: id.substring(id.indexOf('-') + 1)}).toString();
                        fetch(cancel, {method: 'get'})
                            .then(() => loadOrders());
                    });
                    innerContainer.appendChild(confirmButton);
                }

                container.appendChild(innerContainer);
            }
        });
}

async function openProductInfoUpdateWindow(data) {
    const title = document.createElement("div");
    const price = document.createElement("div");
    const remained = document.createElement("div");
    const description = document.createElement("div");
    const form = document.createElement('form');
    const windowTitle = document.createElement('div');
    const applyButton = document.createElement("div");

    title.setAttribute('class', 'product-info-content');
    title.setAttribute('id', 'product-info-title');
    price.setAttribute('class', 'product-info-content');
    price.setAttribute('id', 'product-info-price');
    remained.setAttribute('class', 'product-info-content');
    remained.setAttribute('id', 'product-info-remained');
    description.setAttribute('class', 'product-info-content');
    description.setAttribute('id', 'product-info-description');
    form.setAttribute('class', 'info-update-form-container');
    form.setAttribute('encType', 'multipart/form-data');
    windowTitle.setAttribute('class', 'info-update-header');
    windowTitle.innerText = 'Информация о товаре';
    applyButton.setAttribute('class', 'info-update-buttons');
    applyButton.insertAdjacentHTML('afterbegin', `<input class="info-update-accept" type="button" value="Применить изменения"/>`)

    title.insertAdjacentText('afterbegin', `Название:`);
    price.insertAdjacentText('afterbegin', `Цена:`);
    remained.insertAdjacentText('afterbegin', `Осталось:`);
    description.insertAdjacentText('afterbegin', `Описание:`);

    title.insertAdjacentHTML('beforeend', `<br><input class="product-info-fields" type="text" name="title" id="info-title"/>`)
    price.insertAdjacentHTML('beforeend', `<br><input class="product-info-fields" type="number" name="title" id="info-price"/>`)
    remained.insertAdjacentHTML('beforeend', `<br><input class="product-info-fields" type="number" name="title" id="info-remained"/>`)
    description.insertAdjacentHTML('beforeend', `<br><textarea class="product-info-fields" name="description" id="info-desc"></textarea>`)

    document.getElementById("info-title").value = data['title'];
    document.getElementById("info-price").value = data['price'];
    document.getElementById("info-remained").value = data['remained'];
    document.getElementById("info-desc").value = data['description'];
    
    const info = document.getElementsByClassName('product-info-content');
    const len = info.length;
    for (let i = 0; i < len; i++) {
        info[0].remove();
    }
    
    form.appendChild(windowTitle);
    form.appendChild(title);
    form.appendChild(price);
    form.appendChild(remained);
    form.appendChild(description);
    form.appendChild(applyButton);
    
    document.getElementsByClassName('profile-market-info-update-window')[0].appendChild(form);

    applyButton.addEventListener('click', async function () {
        let formData = new FormData();
        formData.append('marketId', JSON.stringify(data['fullId']['marketId']));
        formData.append('userId', JSON.stringify(data['fullId']['userId']));
        formData.append('productId', JSON.stringify(data['fullId']['productId']));
        formData.append('title', document.getElementById("info-title").value);
        formData.append('price', document.getElementById("info-price").value);
        formData.append('remained', document.getElementById("info-remained").value);
        formData.append('description', document.getElementById("info-desc").value);

        await fetch('products/save', {
            method: 'post',
            body: formData
        });
        productInfoUpdateWindow.hidden = true;
    })

    productInfoUpdateWindow.hidden = false;
}

function openPurchases() {
    purchasesInnerContainer[0].hidden = false;
    productsInnerContainer[0].hidden = true;
    ordersInnerContainer[0].hidden = true;

    document.getElementById("profile-tabs-purchases").innerHTML = "<b>Мои покупки</b>";
    document.getElementById("profile-tabs-products").innerHTML = "Мои товары";
    document.getElementById("profile-tabs-orders").innerHTML = "Мои заказы";

    purchasesInnerContainer[0].style.opacity = 1;
    productsInnerContainer[0].style.opacity = 0;
    ordersInnerContainer[0].style.opacity = 0;
}

function openProducts () {
    purchasesInnerContainer[0].hidden = true;
    productsInnerContainer[0].hidden = false;
    ordersInnerContainer[0].hidden = true;

    document.getElementById("profile-tabs-purchases").innerHTML = "Мои покупки";
    document.getElementById("profile-tabs-products").innerHTML = "<b>Мои товары</b>";
    document.getElementById("profile-tabs-orders").innerHTML = "Мои заказы";

    purchasesInnerContainer[0].style.opacity = 0;
    productsInnerContainer[0].style.opacity = 1;
    ordersInnerContainer[0].style.opacity = 0;
}

function openOrders () {
    purchasesInnerContainer[0].hidden = true;
    productsInnerContainer[0].hidden = true;
    ordersInnerContainer[0].hidden = false;

    document.getElementById("profile-tabs-purchases").innerHTML = "Мои покупки";
    document.getElementById("profile-tabs-products").innerHTML = "Мои товары";
    document.getElementById("profile-tabs-orders").innerHTML = "<b>Мои заказы</b>";

    purchasesInnerContainer[0].style.opacity = 0;
    productsInnerContainer[0].style.opacity = 0;
    ordersInnerContainer[0].style.opacity = 1;
}

const container = document.querySelector('.profile-container');

const purchasesButton = document.getElementById("profile-tabs-purchases");
purchasesButton.addEventListener('click', () => openPurchases());

const productsButton = document.getElementById("profile-tabs-products");
productsButton.addEventListener('click', () => openProducts());

const ordersButton = document.getElementById("profile-tabs-orders");
ordersButton.addEventListener('click', () => openOrders());

const purchasesInnerContainer = document.getElementsByClassName("profile-purchases-container");
const productsInnerContainer = document.getElementsByClassName("profile-products-container");
const ordersInnerContainer = document.getElementsByClassName("profile-orders-container");

const purchases = document.getElementsByClassName("profile-purchases-inner-container")
const products = document.getElementsByClassName("profile-products-inner-container")
const orders = document.getElementsByClassName("profile-orders-inner-container")

loadPurchases();
loadProducts();
loadOrders();
openProducts();