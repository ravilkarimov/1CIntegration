
var Djinaro = {};

Djinaro.WriteResponse = function (data) {
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
