document.addEventListener('DOMContentLoaded', async function () {
    fetch('api/get/user/myInfo', {method: 'get'})
        .then((response) => response.json())
        .then((data) => {
            if (data["address"] !== 'undefined') {
                fillRoomField(data);
            }
            if (data["username"] !== 'undefined') {
                const username = document.createElement('div');
                username.setAttribute('class', 'profile-telegram-info');
                document.getElementsByClassName("profile-telegram-info")[0].remove();
                username.insertAdjacentText("afterbegin", `@${data["username"]}`)
                document.getElementsByClassName("profile-user-telegram-container")[0].appendChild(username)
            }
            if (data["name"] !== 'undefined') {
                const name = document.createElement('div');
                name.setAttribute('class', 'profile-name');
                document.getElementsByClassName("profile-name")[0].remove();
                name.insertAdjacentHTML("afterbegin",
                    `<button class="editorButton" id="user-name-editor">` +
                    `<img src="assets/editButton.png" width="28px" height="26px"/>` +
                    `</button>`);
                if (data["name"] !== null) {
                    name.insertAdjacentText("afterbegin", `${data["name"]}`)
                } else {
                    name.insertAdjacentText("afterbegin", `Имя пользователя`)
                }
                document.getElementsByClassName("profile-name-container")[0].appendChild(name)
            }
            
            waitForElm('user-room-editor').then((button) => {
                button.addEventListener('click', function(){
                    const roomInput = document.createElement('input');
                    roomInput.setAttribute('class', 'roomInput');
                    roomInput.setAttribute('id', 'roomInputId');
                    
                    const roomButtonApply = document.createElement('button');
                    roomButtonApply.textContent = 'Применить';
                    roomButtonApply.addEventListener('click', function(){
                        let formData = new FormData();
                        formData.append('address', document.getElementById('roomInputId').value);
                        
                        fetch('api/set/user/info/address',
                            {
                                method: 'post', 
                                body: formData
                            }).then(x => location.reload())
                    });

                   
                    document.getElementsByClassName("profile-room-info")[0].remove();

                    document.getElementsByClassName("profile-user-room-container")[0].appendChild(roomInput)
                    roomInput.after(roomButtonApply);
                });
            });
            
            waitForElm('user-name-editor').then((button) => {
                button.addEventListener('click', function(){
                    const nameInput = document.createElement('input');
                    nameInput.setAttribute('class', 'nameInput');
                    nameInput.setAttribute('id', 'nameInputId');

                    const nameButtonApply = document.createElement('button');
                    nameButtonApply.textContent = 'Применить';
                    nameButtonApply.addEventListener('click', function(){
                        let formData = new FormData();
                        formData.append('name', document.getElementById('nameInputId').value);

                        fetch('api/set/user/info/name',
                            {
                                method: 'post',
                                body: formData
                            }).then(x => location.reload())
                    });


                    document.getElementsByClassName("profile-name")[0].remove();

                    document.getElementsByClassName("profile-name-container")[0].appendChild(nameInput)
                    nameInput.after(nameButtonApply);
                });
            });

            const categoriesList = document.getElementsByClassName("product-add-select")[0];
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
        });

    function waitForElm(selector) {
        return new Promise(resolve => {
            if (document.getElementById(selector)) {
                return resolve(document.getElementById(selector));
            }

            const observer = new MutationObserver(mutations => {
                if (document.getElementById(selector)) {
                    observer.disconnect();
                    resolve(document.getElementById(selector));
                }
            });
            
            observer.observe(document.body, {
                childList: true,
                subtree: true
            });
        });
    }

    function fillRoomField(data) {
        const room = document.createElement('div');
        room.setAttribute('class', 'profile-room-info');
        document.getElementsByClassName("profile-room-info")[0].remove();
        room.insertAdjacentHTML("afterbegin",
            `<button class="editorButton" id="user-room-editor">` +
            `<img src="assets/editButton.png" width="28px" height="26px"/>` +
            `</button>`);
        if (data["address"] !== null) {
            room.insertAdjacentText("afterbegin", `${data["address"]}`)
        } else {
            room.insertAdjacentText("afterbegin", `не указана`)
        }
        document.getElementsByClassName("profile-user-room-container")[0].appendChild(room)
    }
});