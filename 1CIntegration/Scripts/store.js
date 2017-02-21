
var Djinaro = {};

Djinaro.WriteResponse = function (data) {
    var categories = document.getElementById('categories');
    data.forEach(function (item, i, arr) {
        debugger;

        var stringElement =
            '<div class="col-md-4">'+
            '   <!-- Shop Product -->'+
            '   <div class="shop-product">'+
            '       <!-- Overlay Img -->'+
            '       <div class="overlay-wrapper">'+
            '           <img src="' + data[i].img_path +'" alt="Product 1">' +
            '           <img class="img-hover" src="img/demo/shop/product1_hover.jpg" alt="Product 1">'+
            '           <div class="rating">'+
            '               <span class="star"></span><span class="star star-half"></span>'+
            '               <span class="star star-full"></span><span class="star star-full"></span>'+
            '               <span class="star star-full"></span>'+
            '           </div>'+
            '       </div>'+
            '       <!-- Overlay Img -->'+
            '       <div class="shop-product-info">'+
            '           <a href=""><h5 class="product-name">' + data[i].good + '</h5></a>' +
            '           <p class="product-category"><a href="">Мужская</a>, <a href="">Обувь</a></p>'+
            '           <p class="product-price">2299.90</p>'+
            '       </div>'+
            '       <div class="product-links">'+
            '           <ul>'+
            '               <li><a href="#" class="ToolTip" title="Добавить в корзину" data-opie-position="tc:bc"><span class="fa fa-shopping-cart"></span></a></li>'+
            '               <li><a href="#" class="ToolTip" title="Просмотреть детали" data-opie-position="tc:bc"><span class="fa fa-list-ul"></span></a></li>'+
            '               <li><a href="#" class="ToolTip" title="Добавить в WishList" data-opie-position="tc:bc"><span class="fa fa-heart-o"></span></a></li>'+
            '               <li><a href="#" class="ToolTip" title="Сравнить" data-opie-position="tc:bc"><span class="fa fa-columns"></span></a></li>'+
            '            </ul>'+
            '        </div>'+
            '    </div>'+
            '    <!-- /Shop Product -->'+
            '    <div class="white-space space-small"></div>'+
            '</div>';

        var row = document.createElement('div');
        row.className = 'row';

        row.innerHTML = stringElement;

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
