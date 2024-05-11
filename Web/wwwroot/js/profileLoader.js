document.addEventListener('DOMContentLoaded', async function() {
    fetch('api/isAuthenticated', {method: 'get'})
        .then((response) => response.json())
        .then((userId) => {
            if (userId !== "non") {
                const li = document.createElement('li');
                li.setAttribute('class', 'li');
                li.setAttribute('id', 'profileli');
                document.getElementsByClassName("tgli")[0].remove();
                li.insertAdjacentHTML("afterbegin", `<button class="profileButton"><a class="link" href="Profile">Профиль</a></button>`);
                document.getElementsByClassName("ul")[0].appendChild(li)
            }
        });
});