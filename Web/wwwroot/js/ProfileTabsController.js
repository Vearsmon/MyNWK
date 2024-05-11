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
    // productsInnerContainer[0].style.opacity = 0;
    purchasesInnerContainer[0].hidden = false;


    ordersInnerContainer[0].style.opacity = 0;
    // productsInnerContainer[0].style.opacity = 0;
    purchasesInnerContainer[0].style.opacity = 1;

    // for (let product of products) {
    //     product.style.opacity = 0;
    // }

    for (let order of orders) {
        order.hidden = true;
    }

    for (let purchase of purchases) {
        purchase.hidden = false;
    }

    // const name = 'Чипсы';
    // const seller = 'Литовкин';
    // const price = '256';
    // const number = '+79966872294';
    // const bank = 'Тинькофф';

    // const newDiv = document.createElement("div");
    // newDiv.innerHTML = `<div class="profile-purchases-container">
    //                         <div class="profile-purchases">
    //                             <div class="profile-purchases-name">${name}</div>

    //                             <div class="profile-purchases-content-header">Продавец</div>
    //                             <div class="profile-purchases-content">${seller}</div>

    //                             <div class="profile-purchases-content-header">Цена</div>
    //                             <div class="profile-purchases-content">${price} руб</div>

    //                             <div class="profile-purchases-content-header">Реквизиты оплаты</div>
    //                             <div class="profile-purchases-content">${number}, ${bank}</div>
    //                         </div>

    //                         <button class="profile-purchases-accept">Подтвердить получение</button>
    //                     </div>`;

    // innerContainer[0].appendChild(newDiv);
}

function openProducts () {
    alert("BUTTON2");

    purchasesInnerContainer[0].hidden = true;
    ordersInnerContainer[0].hidden = true;
    // productsInnerContainer[0].style.opacity = 1;

    ordersInnerContainer[0].style.opacity = 0;
    // productsInnerContainer[0].style.opacity = 1;
    purchasesInnerContainer[0].style.opacity = 0;

    for (let order of orders) {
        order.hidden = true;
    }

    for (let purchase of purchases) {
        purchase.hidden = true;
    }

    // // for (let product of products) {
    // //     product.style.opacity = 1;
    // // }
}

function openOrders () {
    alert("BUTTON3");

    purchasesInnerContainer[0].hidden = true;
    // // productsInnerContainer[0].style.opacity = 0;
    ordersInnerContainer[0].hidden = false;

    ordersInnerContainer[0].style.opacity = 1;
    // productsInnerContainer[0].style.opacity = 0;
    purchasesInnerContainer[0].style.opacity = 0;

    for (let purchase of purchases) {
        purchase.hidden = true;
    }

    // // for (let product of products) {
    // //     product.style.opacity = 0;
    // // }

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


const purchasesInnerContainer = document.getElementsByClassName("profile-purchases-inner-container");
const productsInnerContainer = document.getElementsByClassName("profile-products-inner-container");
const ordersInnerContainer = document.getElementsByClassName("profile-orders-inner-container");

const purchases = document.getElementsByClassName("profile-purchases-container")
const products = document.getElementsByClassName("profile-products")
const orders = document.getElementsByClassName("profile-orders-container")