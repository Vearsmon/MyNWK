const productAddWindow = document.getElementsByClassName("profile-product-info-window");
const increaseCountForCart = document.getElementById('product-count-inc');
increaseCountForCart.addEventListener("click", function () {
    const count = Number(document.getElementById('product-curr-count').innerText);
    document.getElementById('product-curr-count').innerText = `${count + 1}`;
});
const decreaseCountForCart = document.getElementById('product-count-dec');
decreaseCountForCart.addEventListener("click", function () {
    const count = Number(document.getElementById('product-curr-count').innerText);
    document.getElementById('product-curr-count').innerText = `${count - 1}`;
});
const addToCartButton = document.getElementById('product-add-to-cart');
addToCartButton.addEventListener("click", async function () {
    const count = document.getElementById('product-curr-count').innerText;
    const productInfo = JSON.parse(document.getElementsByClassName('product-id')[0].textContent);
    let formData = new FormData();
    formData.append('count', count);
    formData.append('marketId', productInfo['marketId']);
    formData.append('productId', productInfo['productId']);
    formData.append('sellerId', productInfo['userId']);
    
    await fetch('cart/add', {
        method: 'post',
        body: formData
    });

    closeProductInfoWindow();
})

async function openProductInfoWindow(data) {
    const getProductUrl = new URL('http://127.0.0.1:80/products/get');
    getProductUrl.search = new URLSearchParams(data).toString();
    await fetch(getProductUrl, { method: 'get' })
    .then((response) => response.json())
        .then((infoParams) => {
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
            
            title.insertAdjacentText('afterbegin', `Название: ${infoParams['title']}`);
            price.insertAdjacentText('afterbegin', `Цена: ${infoParams['price']} р.`);
            remained.insertAdjacentText('afterbegin', `Осталось: ${infoParams['remained']} шт.`);
            description.insertAdjacentText('afterbegin', `Описание: ${infoParams['description']}`);
            
            document.getElementsByClassName("product-info-form-container")[0].appendChild(title);
            document.getElementsByClassName("product-info-form-container")[0].appendChild(price);
            document.getElementsByClassName("product-info-form-container")[0].appendChild(remained);
            document.getElementsByClassName("product-info-form-container")[0].appendChild(description);

            document.getElementsByClassName('product-id')[0].textContent = JSON.stringify(data["fullId"]);
            document.getElementById('product-curr-count').innerText = '1';
        });
    productAddWindow[0].hidden = false;
}

function closeProductInfoWindow() {
    productAddWindow[0].hidden = true;
}

const closeButton = document.getElementsByClassName("product-info-background-shadow");
closeButton[0].addEventListener('click', () => closeProductInfoWindow());


document.addEventListener('DOMContentLoaded', async function() {
    fetch('api/isAuthenticated', {method: 'get'})
        .then((response) => response.text())
        .then((userId) => {
            if (userId !== "non") {
                const li = document.createElement('li');
                li.setAttribute('class', 'li');
                li.setAttribute('id', 'profileli');
                document.getElementsByClassName("tgli")[0].remove();
                li.insertAdjacentHTML("afterbegin", `<button class="profileButton"><a class="link" href="Profile">Профиль</a></button>`);
                document.getElementsByClassName("ul")[0].appendChild(li)
            }
            else {
                const tgli = document.getElementsByClassName('tgli')[0];
                tgli.hidden = false;
            }
        });
        
    fetchProducts(null, null);    
    
    const categoriesList = document.getElementsByClassName("baraholka-filters category")[0];
    fetch('api/get/all/categories', {method: 'get'})
        .then((response) => response.json())
        .then((categories) => {
            for (category of categories) {
                const option = document.createElement("option");
                option.setAttribute('value', category.id);
                option.textContent = category.title;
                categoriesList.appendChild(option);
            }
        })
        
    const marketsList = document.getElementsByClassName("baraholka-filters seller")[0];
    fetch('api/get/all/markets', {method: 'get'})
        .then((response) => response.json())
        .then((markets) => {
            for (market of markets) {
                const option = document.createElement("option");
                option.setAttribute('value', market.id);
                option.textContent = market.name;
                marketsList.appendChild(option);
            }
        })
        
        
    const confirmButton = document.getElementsByClassName("baraholka-filters-confirm-button")[0];
    confirmButton.addEventListener('click', () => {
        const categorySelector = document.getElementsByClassName("baraholka-filters category")[0];
        let categoryId = categorySelector.options[categorySelector.selectedIndex].value;
        if (categoryId === 'defaultCategory') {
            categoryId = null;
        }
        
        const marketSelector = document.getElementsByClassName("baraholka-filters seller")[0];
        let marketId = marketSelector.options[marketSelector.selectedIndex].value;
        if (marketId === 'defaultSeller') {
            marketId = null;
        }
        fetchProducts(categoryId, marketId);
    });
    
    const resetButton = document.getElementsByClassName("baraholka-filters-reset-button")[0];
    resetButton.addEventListener('click', () => alert('2'));
});

async function onTelegramAuth(user) {
    const postData = async (url, data) => {
        return fetch(url, {
            method: 'POST',
            redirect: 'manual',
            body: data,
            credentials: 'include'
        });
    }

    let formData = new FormData();
    formData.append('id', user.id);
    formData.append('first_name', user.first_name);
    formData.append('last_name', user.last_name);
    formData.append('username', user.username);
    formData.append('photo_url', user.photo_url);
    formData.append('auth_date', user.auth_date);
    formData.append('hash', user.hash);
    postData("/Account/Login", formData)
        .then(response => {location.reload();});
}

async function fetchProducts(categoryId, marketId) {
    const getAllProductsUrl = new URL('http://127.0.0.1:80/products/get/all');
    const params = {pageNumber:0, batchSize:20};
    if (categoryId) {
        params.categoryId = categoryId;
    }
    if (marketId) {
        params.marketId = marketId;
    }
    getAllProductsUrl.search = new URLSearchParams(params).toString();
    const slots = document.getElementsByClassName("baraholka-slots-container")[0];
    slots.innerHTML = '';
    fetch(getAllProductsUrl, {method: 'get'})
        .then((response) => response.json())
        .then((products) => {
            let i = 0;
            for (let product of products) {
                const productImage = document.createElement('img');
                productImage.setAttribute('src', product.imageRef);
                productImage.setAttribute('class', 'baraholka-slot-photo');
                productImage.setAttribute('id', `baraholka-slot-photo-id-${i}`);

                const productPrice = document.createElement('p');
                productPrice.innerText = `${product['price']} р.`;

                const productInfo = document.createElement('div');
                productInfo.setAttribute('class', 'baraholka-slot-info');
                productInfo.textContent = product['title'];
                productInfo.appendChild(productPrice);

                const productSlot = document.createElement('div');
                productSlot.setAttribute('class', 'baraholka-slot');
                productSlot.appendChild(productImage);
                productSlot.appendChild(productInfo);

                slots.appendChild(productSlot);
                document.getElementById(`baraholka-slot-photo-id-${i}`)
                    .addEventListener('click', () => {
                        openProductInfoWindow(product["fullId"])
                    });
               i += 1;
            }
        });
}




