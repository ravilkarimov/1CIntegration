
var Djinaro = {};

Djinaro.WriteResponse = function (data) {
    var categories = document.getElementById('categories');
    data.forEach(function (item, i, arr) {
        debugger;
        var product = document.createElement('div');
        product.className = 'shop-product';

        var wrapper = document.createElement('div');
        wrapper.className = 'overlay-wrapper';
        var prod_img = document.createElement('img');
        prod_img.src = data[i].img_path;

        var prod_info = document.createElement('div');
        prod_info.className = 'shop-product-info';
        var prod_title = document.createElement('h5');
        prod_title.className = 'product-name';
        prod_title.innerHTML = data[i].good;

        prod_info.appendChild(prod_title);
        wrapper.appendChild(prod_img);
        product.appendChild(wrapper);
        categories.appendChild(product);
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
