
var Djinaro = {};

Djinaro.WriteResponseGroups = function (data) {
    var groups = document.getElementById('groups');

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

Djinaro.WriteResponseSizes = function (data) {
    var groups = document.getElementById('sizes');

    var title = document.createElement('h4');
    title.className = 'title-widget fancy-title';
    var title_div = document.createElement('span');
    title_div.innerHTML = 'Отфильтровать по размеру';
    title.appendChild(title_div);

    var list = document.createElement('ul');
    list.className = 'list-inline';
    for (var i = 0; i < data.length; i++) {
        var row = document.createElement('li');
        var link = document.createElement('a');
        link.innerHTML = data[i].size;
        link.className = 'btn btn-sm btn-default btn-alt margin-bottom10';
        row.appendChild(link);
        list.appendChild(row);
    }
    groups.appendChild(title);
    groups.appendChild(list);
}

Djinaro.WriteResponseGoods = function (data) {
    var categories = document.getElementById('goods');
    categories.innerHtml = '';
    var countRow = data.length / 3;
    var itemIndex = 0;

    for (var i = 0; i < countRow; i++) {

        var row = document.createElement('div');
        row.className = 'row';
        var addItem = 1;

        for (var j = 0; j < addItem; j++) {
            var goodKey = data[itemIndex].good_key;

            if (data[itemIndex]) {
                var stringElement =
                        '<div class="col-md-4">' +
                        '   <!-- Shop Product -->' +
                        '   <div class="shop-product" id="shop-product-' + goodKey + '">' +
                        '       <!-- Overlay Img -->' +
                        '       <div class="overlay-wrapper">' +
                        '           <img src="../img/demo/shop/product1.jpg" alt="' + data[itemIndex].feature + '">' +
                        '           <img class="img-hover" src="../img/demo/shop/product1_hover.jpg" alt="Product 1">' +
                        '           <div class="rating" id="rating_' + goodKey + '"></div>' +
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
                        '</div> ';

                row.innerHTML += stringElement;
                
                categories.appendChild(row);

                if(addItem < 3) addItem++;
            }

            itemIndex += 1;
        }

        var shopProducts = $('.shop-product');
        for (var s = 0; s < shopProducts.length; s++) {
            $('#'+shopProducts[s].id).hover(function (a) {
                  Djinaro.getSizesByGood(a.currentTarget.id.replace('shop-product-',''));
            });
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

Djinaro.getAllGroups = function () {
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

Djinaro.getSizesByGood = function (good_key) {
    $.ajax({
        url: '/Store/getsizesgood',
        type: 'GET',
        dataType: 'json',
        data: { id: good_key },
        success: function (data) {
            var divRating = document.getElementById('rating_' + good_key);
            divRating.innerHTML = '';
            var sizesString = '';
            for (var i = 0; i < data.length; i++) {
                sizesString += ' ' + data[i].size;
            }
            var div = document.createElement('div');
            div.innerText = sizesString;
            divRating.appendChild(div);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.getAllSizes = function () {
    $.ajax({
        url: '/Store/getsizes',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseSizes(data);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}
