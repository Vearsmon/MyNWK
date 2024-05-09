document.addEventListener('DOMContentLoaded', async function() {
    fetch('Api/IsAuthenticated', {method: 'get'})
        .then((response) => response.text())
        .then((userId) => {
            if (userId !== "non") {
                var li = document.createElement('li');
                li.setAttribute('class', 'li');
                li.setAttribute('id', 'profileli');
                document.getElementsByClassName("tgli")[0].remove();
                li.insertAdjacentHTML("afterbegin", `<button class="profileButton"><a class="link" href="Profile">Профиль</a></button>`);
                document.getElementsByClassName("ul")[0].appendChild(li)
            }
        });
});

async function onTelegramAuth(user) {
    const postData = async (url, data) => {
        return fetch(url, {
            method: 'post',
            redirect: 'follow',
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
    await postData("/Account/Login", formData);
}