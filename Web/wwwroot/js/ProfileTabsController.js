function loadPurchases() {
    const getProductsByBuyer = new URL('http://127.0.0.1:80/products/get/byBuyer');
    const params = {pageNumber:0, batchSize:20};
    getProductsByBuyer.search = new URLSearchParams(params).toString();
    const container = document.getElementsByClassName("profile-purchases-container")[0];
    //container.innerHTML = '';

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
    
                    const productInfo = document.createElement('div');
                    productInfo.setAttribute('class', 'profile-purchase-info');
                    productInfo.textContent = product['title'];
                    productInfo.appendChild(productPrice);
    
                    const productSlot = document.createElement('div');
                    productSlot.setAttribute('class', 'profile-purchase');
                    productSlot.appendChild(productImage);
                    productSlot.appendChild(productInfo);

                    purchases.appendChild(productSlot);
                }
                
                const button = document.createElement('button');
                button.setAttribute('class', 'profile-purchases-accept');
                button.innerHTML = 'Подтвердить получение';

                const innerContainer = document.createElement('div');
                innerContainer.setAttribute('class', 'profile-purchases-inner-container');
                innerContainer.appendChild(header);
                innerContainer.appendChild(purchases);
                innerContainer.appendChild(button);

                container.appendChild(innerContainer);
            }
        });
}

function loadProducts() {
    const getProductsByUser = new URL('http://127.0.0.1:80/products/get/byUser');
    const params = {pageNumber:0, batchSize:20};
    getProductsByUser.search = new URLSearchParams(params).toString();
    const container = document.getElementsByClassName("profile-products-inner-container")[0];
    //container.innerHTML = '';
        
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
    const getProductsBySeller = new URL('http://127.0.0.1:80/products/get/byBuyer');
    const params = {pageNumber:0, batchSize:20};
    getProductsBySeller.search = new URLSearchParams(params).toString();
    const container = document.getElementsByClassName("profile-orders-container")[0];
    //container.innerHTML = '';

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
    
                    const productInfo = document.createElement('div');
                    productInfo.setAttribute('class', 'profile-order-info');
                    productInfo.textContent = product['title'];
                    productInfo.appendChild(productPrice);
    
                    const productSlot = document.createElement('div');
                    productSlot.setAttribute('class', 'profile-order');
                    productSlot.appendChild(productImage);
                    productSlot.appendChild(productInfo);

                    orders.appendChild(productSlot);
                }
                
                const button = document.createElement('button');
                button.setAttribute('class', 'profile-orders-cancel');
                button.innerHTML = 'Отменить заказ';

                const innerContainer = document.createElement('div');
                innerContainer.setAttribute('class', 'profile-orders-inner-container');
                innerContainer.appendChild(header);
                innerContainer.appendChild(orders);
                innerContainer.appendChild(button);

                container.appendChild(innerContainer);
            }
        });
}

async function openProductInfoUpdateWindow(data)
{
    const title = document.createElement("div");
    const price = document.createElement("div");
    const remained = document.createElement("div");
    const description = document.createElement("div");

    title.setAttribute('class', 'product-info-content');
    title.setAttribute('id', 'product-info-title');
    price.setAttribute('class', 'product-info-content');
    price.setAttribute('id', 'product-info-price');
    remained.setAttribute('class', 'product-info-content');
    remained.setAttribute('id', 'product-info-remained');
    description.setAttribute('class', 'product-info-content');
    description.setAttribute('id', 'product-info-description');

    const info = document.getElementsByClassName('product-info-content');
    const len = info.length;
    for (let i = 0; i < len; i++) {
        info[0].remove();
    }

    title.insertAdjacentText('afterbegin', `Название:`);
    price.insertAdjacentText('afterbegin', `Цена:`);
    remained.insertAdjacentText('afterbegin', `Осталось:`);
    description.insertAdjacentText('afterbegin', `Описание:`);
    
    title.insertAdjacentHTML('beforeend', `<br><input class="product-info-fields" type="text" name="title" id="info-title"/>`)
    price.insertAdjacentHTML('beforeend', `<br><input class="product-info-fields" type="number" name="title" id="info-price"/>`)
    remained.insertAdjacentHTML('beforeend', `<br><input class="product-info-fields" type="number" name="title" id="info-remained"/>`)
    description.insertAdjacentHTML('beforeend', `<br><textarea class="product-info-fields" name="description" id="info-desc"></textarea>`)

    // title.insertAdjacentText('afterbegin', `Название: ${data['title']}`);
    // price.insertAdjacentText('afterbegin', `Цена: ${data['price']} р.`);
    // remained.insertAdjacentText('afterbegin', `Осталось: ${data['remained']} шт.`);
    // description.insertAdjacentText('afterbegin', `Описание: ${data['description']}`);

    const applyButton = document.getElementsByClassName('info-update-buttons')[0]
    
    document.getElementsByClassName("info-update-form-container")[0].insertBefore(title, applyButton);
    document.getElementsByClassName("info-update-form-container")[0].insertBefore(price, applyButton);
    document.getElementsByClassName("info-update-form-container")[0].insertBefore(remained, applyButton);
    document.getElementsByClassName("info-update-form-container")[0].insertBefore(description, applyButton);
    
    document.getElementById("info-title").value = data['title'];
    document.getElementById("info-price").value = data['price'];
    document.getElementById("info-remained").value = data['remained'];
    document.getElementById("info-desc").value = data['description'];
    
    document.getElementsByClassName('info-update-accept')[0].addEventListener('click', async function () {
        let formData = new FormData();
        formData.append('marketId', JSON.stringify(data['fullId']['marketId']));
        formData.append('userId', JSON.stringify(data['fullId']['userId']));
        formData.append('productId', JSON.stringify(data['fullId']['productId']));
        formData.append('title', data['title']);
        formData.append('price', data['price']);
        formData.append('remained', data['remained']);
        formData.append('description', data['description']);
        
        await fetch('products/save', {
            method: 'post',
            body: formData
        });
        productInfoUpdateWindow[0].hidden = true;
    })
    
    productInfoUpdateWindow[0].hidden = false;
}

function openPurchases () {
    //alert("BUTTON1");
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
    //alert("BUTTON2");
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
    //alert("BUTTON3");
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
loadPurchases();
openProducts();