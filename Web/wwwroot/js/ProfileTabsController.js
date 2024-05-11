// class and id names:
// profile-container class
// profile-tabs class
// profile-inner-container class
// profile-tabs-purchases id
// profile-tabs-products id
// profile-tabs-orders id



function openPurchases () {
    alert("BUTTON1");
    ordersInnerContainer[0].hidden = true;
    productsInnerContainer[0].style.opacity = true;
    purchasesInnerContainer[0].hidden = false;


    ordersInnerContainer[0].style.opacity = 0;
    productsInnerContainer[0].style.opacity = 0;
    purchasesInnerContainer[0].style.opacity = 1;

    const getProductsByUser = new URL('http://127.0.0.1:80/products/get/byBuyer');
    const params = {pageNumber:0, batchSize:20};
    getProductsByUser.search = new URLSearchParams(params).toString();
    const slots = document.getElementsByClassName("profile-purchases-inner-container")[0];
    slots.innerHTML = '';

    // <div class="profile-purchases">
    //     <div class="profile-purchases-name">${name}</div>

    //     <div class="profile-purchases-content-header">Продавец</div>
    //     <div class="profile-purchases-content">${seller}</div>

    //     <div class="profile-purchases-content-header">Цена</div>
    //     <div class="profile-purchases-content">${price} руб</div>

    //     <div class="profile-purchases-content-header">Реквизиты оплаты</div>
    //     <div class="profile-purchases-content">${number}, ${bank}</div>
    // </div>
        
    fetch(getProductsByUser, {method: 'get'})
        .then((response) => response.json())
        .then((orders) => {
            for (order of orders) {
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

               slots.appendChild(productSlot);
            }
        });

    for (let order of orders) {
        order.hidden = true;
    }

    for (let purchase of purchases) {
        purchase.hidden = false;
    }
}

function openProducts () {
    alert("BUTTON2");
    purchasesInnerContainer[0].hidden = true;
    ordersInnerContainer[0].hidden = true;
    productsInnerContainer[0].hidden = false;

    ordersInnerContainer[0].style.opacity = 0;
    productsInnerContainer[0].style.opacity = 1;
    purchasesInnerContainer[0].style.opacity = 0;

    const getProductsByUser = new URL('http://127.0.0.1:80/products/get/byUser');
    const params = {pageNumber:0, batchSize:20};
    getProductsByUser.search = new URLSearchParams(params).toString();
    const slots = document.getElementsByClassName("profile-products-inner-container")[0];
    slots.innerHTML = '';
        
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

               slots.appendChild(productSlot);
            }
        });

        for (let order of orders) {
            order.hidden = true;
        }
    
        for (let purchase of purchases) {
            purchase.hidden = true;
        }
}

function openOrders () {
    alert("BUTTON3");
    purchasesInnerContainer[0].hidden = true;
    productsInnerContainer[0].style.opacity = 0;
    ordersInnerContainer[0].hidden = false;

    ordersInnerContainer[0].style.opacity = 1;
    productsInnerContainer[0].style.opacity = 0;
    purchasesInnerContainer[0].style.opacity = 0;

    for (let purchase of purchases) {
        purchase.hidden = true;
    }

    for (let order of orders) {
        order.hidden = false;
    }
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

//openPurchases();