document.addEventListener('DOMContentLoaded', function() {
    const postData = async (url, data) => {
        return fetch(url, {
            method: 'post',
            body: data,
            credentials: 'include'
        });
    }

    var userId = "non";
    let formData = new FormData();
    formData.append('check', 'true');
    postData("http://127.0.0.1:80/Api/GetUser", formData)
        .then((response) => {
            const reader = response.body.getReader();
            reader.read().then(({ done, value }) => {
                alert(bin2String(value));
                userId = bin2String(value);
                if (userId !== "non") {
                    var li = document.createElement('li');
                    li.setAttribute('class', 'li');
                    li.setAttribute('id', 'profileli');
                    document.getElementsByClassName("tgli")[0].remove();
                    li.insertAdjacentHTML("afterbegin", `<button class="profileButton"><a class="link" href="http://127.0.0.1:80/Profile">Профиль</a></button>`);
                    document.getElementsByClassName("ul")[0].appendChild(li)
                }
            });
        });

    function bin2String(array) {
        var result = "";
        for (var i = 0; i < array.length; i++) {
            result += String.fromCharCode(parseInt(array[i]));
        }
        return result;
    }
});

function onTelegramAuth(user) {
    var form = document.createElement("form");
    var element1 = document.createElement("input");
    var element2 = document.createElement("input");
    var element3 = document.createElement("input");

    var li = document.createElement('li');
    li.setAttribute('class', 'li');
    li.setAttribute('id', 'profileli');
    li.insertAdjacentHTML("afterbegin", `<button class="profileButton"><a class="link" href="http://127.0.0.1:80/Profile">Профиль</a></button>`);

    form.method = "POST";

    element1.value=user.id;
    element1.name="id";
    form.appendChild(element1);

    element2.value=user.username;
    element2.name="username";
    form.appendChild(element2);

    element3.value=user.name;
    element3.name="name";
    form.appendChild(element3);

    document.body.appendChild(form);

    form.submit();

    document.getElementsByClassName("tgli")[0].remove();
}