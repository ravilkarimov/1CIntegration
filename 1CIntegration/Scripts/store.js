
var Djinaro = {};

Djinaro.WriteResponseGroups = function (data) {
    var groups = document.getElementById('groups');
    debugger;

    var title = document.createElement('h4');
    title.className = 'title-widget fancy-title';
    var title_div = document.createElement('span');
    title_div.innerHTML = 'Категории';
    title.appendChild(title_div);

    var list = document.createElement('ul');
    list.className = 'shop-product-categories';

    for (var i = 0; i < data.length; i++) {
        var row = document.createElement('li');
        if (data[i].group_name == 'ОБУВЬ') {
            row.className = 'active';
        }
        var link = document.createElement('a');
        link.innerHTML = data[i].group_name;
        var count = document.createElement('span');
        count.className = 'count';
        count.innerHTML = '(' + data[i].count + ')';
        row.appendChild(link);
        row.appendChild(count);
        list.appendChild(row);
    }
    groups.appendChild(title);
    groups.appendChild(list);
}

Djinaro.WriteResponseGoods = function (data) {
    var categories = document.getElementById('goods');
    debugger;
    var countRow = data.length / 3;
    var itemIndex = 0;

    for (var i = 0; i < countRow; i++) {

        var row = document.createElement('div');
        row.className = 'row';
        var addItem = 1;

        for (var j = 0; j < addItem; j++) {
            if (data[itemIndex]) {
                var stringElement =
                        '<div class="col-md-4">' +
                        '   <!-- Shop Product -->' +
                        '   <div class="shop-product">' +
                        '       <!-- Overlay Img -->' +
                        '       <div class="overlay-wrapper">' +
                        '           <img src="../img/demo/shop/product1.jpg" alt="' + data[itemIndex].feature + '">' +
                        '           <img class="img-hover" src="../img/demo/shop/product1_hover.jpg" alt="Product 1">' +
                        '       </div>' +
                        '       <!-- Overlay Img -->' +
                        '       <div class="shop-product-info">' +
                        '           <a href=""><h5 class="product-name">' + data[itemIndex].good + ' </h5></a>' +
                        '           <p class="product-category"><a href=""> ' + data[itemIndex].group_name + '</a></p> ' +
                        '           <p class="product-price">' + data[itemIndex].price + ' РУБ </p>' +
                        '       </div>' +
                        '    </div>' +
                        '    <!-- /Shop Product -->' +
                        '    <div class="white-space space-small"></div>' +
                        '</div>';

                row.innerHTML += stringElement;

                categories.appendChild(row);

                if(addItem < 3) addItem++;
            }

            itemIndex += 1;
        }
    }
}

Djinaro.getShoes = function () {
    $.ajax({
        url: '/api/values',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseGoods(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
Djinaro.getALLGroups = function () {
    debugger;
    $.ajax({
        url: '/Store/getgroups',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseGroups(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
