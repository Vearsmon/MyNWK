function loadPurchases() {
    const getProductsByBuyer = new URL('http://127.0.0.1:80/products/get/byBuyer');
    const container = document.getElementsByClassName("profile-purchases-container")[0];

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
                
                const innerContainer = document.createElement('div');
                innerContainer.setAttribute('class', 'profile-purchases-inner-container');
                innerContainer.appendChild(header);
                innerContainer.appendChild(purchases);

                if (order["CancelledBySeller"]) {
                    const label = document.createElement('label');
                    label.innerHTML = "Отменён";
                    innerContainer.appendChild(label);
                } else if (order["receivedByBuyer"]) {
                    const label = document.createElement('label');
                    label.innerHTML = "Получен";
                    innerContainer.appendChild(label);
                } else {
                    const button = document.createElement('button');
                    button.setAttribute('class', 'profile-purchases-accept');
                    button.innerHTML = 'Подтвердить получение';
                    button.addEventListener('click', async function() {
                        const confirm = new URL('http://127.0.0.1:80/orders/confirm');
                        confirm.search = new URLSearchParams({orderId: order["orderId"]}).toString();
                        fetch(confirm, {method: 'get'});
                    });
                    innerContainer.appendChild(button);
                }
                
                container.appendChild(innerContainer);
            }
        });
}

function loadProducts() {
    const getProductsByUser = new URL('http://127.0.0.1:80/products/get/byUser');
    const container = document.getElementsByClassName("profile-products-inner-container")[0];
        
    fetch(getProductsByUser, {method: 'get'})
        .then((response) => response.json())
        .then((products) => {
            for (product of products) {
                const productImage = document.createElement('img');
                productImage.setAttribute('src', product.imageRef);
                productImage.setAttribute('class', 'profile-product-photo');

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
            }
        });
}

function loadOrders() {
    const getProductsBySeller = new URL('http://127.0.0.1:80/products/get/bySeller');
    const container = document.getElementsByClassName("profile-orders-container")[0];

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
                
                const innerContainer = document.createElement('div');
                innerContainer.setAttribute('class', 'profile-orders-inner-container');
                innerContainer.appendChild(header);
                innerContainer.appendChild(orders);

                if (order["CancelledBySeller"]) {
                    const label = document.createElement('label');
                    label.innerHTML = "Отменён";
                    innerContainer.appendChild(label);
                } else if (order["receivedByBuyer"]) {
                    const label = document.createElement('label');
                    label.innerHTML = "Получен";
                    innerContainer.appendChild(label);
                } else {
                    const button = document.createElement('button');
                    button.setAttribute('class', 'profile-orders-cancel');
                    button.innerHTML = 'Отменить заказ';
                    button.addEventListener('click', async function() {
                        const confirm = new URL('http://127.0.0.1:80/orders/cancel');
                        confirm.search = new URLSearchParams({orderId: order["orderId"]}).toString();
                        fetch(confirm, {method: 'get'});
                    });
                    innerContainer.appendChild(button);
                }

                container.appendChild(innerContainer);
            }
        });
}

function openPurchases () {
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