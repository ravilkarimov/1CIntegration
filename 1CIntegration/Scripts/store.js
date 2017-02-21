
var Djinaro = {};

Djinaro.WriteResponse = function (data) {
    var categories = document.getElementById('categories');
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
                        '           <img src="img/demo/shop/product1.jpg" alt="' + data[itemIndex].feature + '">' +
                        '           <img class="img-hover" src="img/demo/shop/product1_hover.jpg" alt="Product 1">' +
                        '           <div class="rating">' +
                        '               <span class="star"></span><span class="star star-half"></span>' +
                        '               <span class="star star-full"></span><span class="star star-full"></span>' +
                        '               <span class="star star-full"></span>' +
                        '           </div>' +
                        '       </div>' +
                        '       <!-- Overlay Img -->' +
                        '       <div class="shop-product-info">' +
                        '           <a href=""><h5 class="product-name">' + data[itemIndex].good + '</h5></a>' +
                        '           <p class="product-category"><a href="">Мужская</a>, <a href="">' + data[itemIndex].group_name + '</a></p>' +
                        '           <p class="product-price">' + data[itemIndex].price + '</p>' +
                        '       </div>' +
                        '       <div class="product-links">' +
                        '           <ul>' +
                        '               <li><a href="#" class="ToolTip" title="Добавить в корзину" data-opie-position="tc:bc"><span class="fa fa-shopping-cart"></span></a></li>' +
                        '               <li><a href="#" class="ToolTip" title="Просмотреть детали" data-opie-position="tc:bc"><span class="fa fa-list-ul"></span></a></li>' +
                        '               <li><a href="#" class="ToolTip" title="Добавить в WishList" data-opie-position="tc:bc"><span class="fa fa-heart-o"></span></a></li>' +
                        '               <li><a href="#" class="ToolTip" title="Сравнить" data-opie-position="tc:bc"><span class="fa fa-columns"></span></a></li>' +
                        '            </ul>' +
                        '        </div>' +
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
