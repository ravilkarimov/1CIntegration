
var Djinaro = {};

Djinaro.WriteResponse = function (data) {
    var categories = document.getElementById('categories');
    data.forEach(function (item, i, arr) {
        debugger;
        var row = document.createElement('div');
        row.className = 'row';

        var div = document.createElement('div');
        div.className = 'col-md-12';
        div.innerHTML = data[0].good;

        row.appendChild(div);
        categories.appendChild(row);
    });
}

Djinaro.getGoods = function () {
    $.ajax({
        url: '/api/values',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponse(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
