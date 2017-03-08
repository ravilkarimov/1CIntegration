
var Djinaro = {};

Djinaro.filterByGoods = function () {
    var groupActive = $('.shop-product-categories .active');
    var pagingActive = $('#paging .active');
    var sizesActive = $('#sizes .active');
    var numberPagingActive = 1;
    if (pagingActive.length == 1) numberPagingActive = parseInt(pagingActive[0].innerText);
    if (groupActive[0].nodeName == 'A') {
        groupActive = groupActive.parentElement;
    }
    if (groupActive[0].nodeName == 'LI') {
        var sizes = '';
        for (z = 0; z < sizesActive.length; z++) {
            if (sizes.length == 0) {
                sizes += "'" + sizesActive[z].innerText + "'";
            } else {
                sizes += ", '" + sizesActive[z].innerText + "'";
            }
        }
        Djinaro.getShoes(parseInt(groupActive[0].id), sizes, numberPagingActive);
        Djinaro.getShoesCount(parseInt(groupActive[0].id), sizes);
    }
}

Djinaro.WriteResponseGroups = function (data) {
    var groups = document.getElementById('groups');

    var title = document.createElement('h4');
    title.className = 'title-widget fancy-title';
    var title_div = document.createElement('span');
    title_div.innerHTML = 'Категории';
    title.appendChild(title_div);

    var list = document.createElement('ul');
    list.className = 'shop-product-categories';
    $(list).on('click', function (a) {
        var liClick = a.target;
        if (liClick.nodeName == 'A') {
            liClick = liClick.parentElement;
        }
        if (liClick.nodeName == 'LI') {
            var ulClick = a.currentTarget;
            $('.active', ulClick)[0].className = '';
            liClick.className = 'active';
            Djinaro.filterByGoods();
        }
    });

    for (var i = 0; i < data.length; i++) {
        var row = document.createElement('li');
        if (data[i].group_name == 'ОБУВЬ') {
            row.className = 'active';
        }
        row.id = data[i].group_id;
        var link = document.createElement('a');
        //link.href = '';
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
        link.id = 'size-' + data[i].size;
        link.className = 'btn btn-sm btn-default btn-alt margin-bottom10';
        row.appendChild(link);
        list.appendChild(row);
    }
    groups.appendChild(title);
    groups.appendChild(list);

    var sizeArray = $('#sizes');
    for (var s = 0; s < sizeArray.length; s++) {
        $('#' + sizeArray[s].id).click(function (a) {
            if (a && a.target) {
                var clickSize = $('#' + a.target.id);
                if (clickSize && clickSize[0].className.indexOf('active') < 0) {
                    clickSize[0].className = "btn btn-sm btn-default btn-alt margin-bottom10 active";
                    Djinaro.filterByGoods();
                } else if (clickSize && clickSize[0].className.indexOf('active') >= 0) {
                    clickSize[0].className = "btn btn-sm btn-default btn-alt margin-bottom10";
                    Djinaro.filterByGoods();
                }
            }
        });
    }
}

Djinaro.WriteResponseGoods = function (data) {
    var categories = document.getElementById('goods');
    $('#goods').children().remove();
    if (data.length > 0) {
        var countRow = data.length / 3;
        var itemIndex = 0;

        for (var i = 0; i < countRow; i++) {

            var row = document.createElement('div');
            row.className = 'row';
            var addItem = 1;

            for (var j = 0; j < addItem; j++) {
                if (data[itemIndex]) {
                    var goodKey = data[itemIndex].good_key;

                    if (data[itemIndex]) {
                        var stringElement =
                            '<div class="col-md-4">' +
                                '   <!-- Shop Product -->' +
                                '   <div class="shop-product" id="shop-product-' + goodKey + '">' +
                                '       <!-- Overlay Img -->' +
                                '       <div class="overlay-wrapper">' +
                                '           <img src="../img/demo/shop/product1.jpg" class="img-zoom" width="1200" height="900" alt="' + data[itemIndex].feature + '">' +
                                '           <div class="overlay-wrapper-content"> ' +
								'				<div class="overlay-details"> ' +
								'        			<a onclick="Djinaro.openModalProduct()"> ' +
                                '                       <span class="icon gfx-zoom-in-1" ></span>' +
                                '                   </a> ' +
                                '    			</div> ' +
                                '        		<!--div class="overlay-bg bg-color-dark"></div--> ' +
                                '    		</div> ' +
                                '           <div class="rating" id="rating_' + goodKey + '"></div>' +
                                '       </div>' +
                                '       <!-- Overlay Img -->' +
                                '       <div class="shop-product-info">' +
                                '           <a href=""><h5 class="product-name">' + data[itemIndex].good + ' </h5></a>' +
                                '           <p class="product-category"><a href=""> ' + data[itemIndex].group_name + '</a></p> ' +
                                '           <p class="product-price">' + data[itemIndex].price + ' ' + data[itemIndex].currency + ' </p>' +
                                '       </div>' +
                                '    </div>' +
                                '    <!-- /Shop Product -->' +
                                '    <div class="white-space space-small"></div>' +
                                '</div> ';

                        row.innerHTML += stringElement;
                        categories.appendChild(row);

                        if (addItem < 3) addItem++;
                    }

                    itemIndex += 1;
                }
            }

            var shopProducts = $('.shop-product');
            var listener = function (a) {
                var current = $(a.currentTarget);
                if (current.length == 1) {
                    Djinaro.getSizesByGood(current[0].id.replace('shop-product-', ''));
                }
            }
            for (var s = 0; s < shopProducts.length; s++) {
                $('#'+shopProducts[s].id).on('mouseover', listener);
            }
        }
    }
}

Djinaro.openModalProduct = function() {
    debugger;
    $('#myModal').modal('show');
}

Djinaro.WriteResponseGoodsPaging = function (data) {
    var pagingActive = $('#paging .active');
    var numberPagingActive = 1;
    if (pagingActive.length == 1) numberPagingActive = parseInt(pagingActive[0].innerText);
    var paging = document.getElementById('paging');
    $('#paging').children().remove();

    var ul = document.createElement('ul');
    ul.className = 'pagination';

    var leftA = document.createElement('li');
    var linkLeft = document.createElement('a');
    linkLeft.link = "#";
    //var iLeft = document.createElement('i');
    //iLeft.className = 'fa fa-angle-left';
    //linkLeft.appendChild(iLeft);
    leftA.appendChild(linkLeft);
    ul.appendChild(linkLeft);

    for (z = 1; z <= data[0].count; z++) {
        var li = document.createElement('li');
        var link = document.createElement('a');
        link.link = "#";
        link.innerText = z;
        li.appendChild(link);
        ul.appendChild(li);
        if (z == numberPagingActive) {
            li.className = 'active';
        }
    }

    var rightA = document.createElement('li');
    var linkRight = document.createElement('a');
    linkRight.link = "#";
    //var iRight = document.createElement('i');
    //iLeft.className = 'fa fa-angle-right';
    //linkRight.appendChild(iRight);
    rightA.appendChild(linkRight);
    ul.appendChild(linkRight);

    $(ul).on('click', function (a) {
        var liClick = a.target;
        if (liClick.nodeName == 'A') {
            liClick = liClick.parentElement;
        }
        if (liClick.nodeName == 'LI') {
            var ulClick = a.currentTarget;
            $('.active', ulClick)[0].className = '';
            liClick.className = 'active';
            Djinaro.filterByGoods();
        }
    });

    paging.appendChild(ul);
}

Djinaro.getShoes = function (groups, sizes, page) {
    $.ajax({
        url: '/Store/getshoes',
        type: 'GET',
        dataType: 'json',
        data: {
            'groups': groups,
            'sizes': sizes,
            'page': page
        },
        success: function (data) {
            Djinaro.WriteResponseGoods(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.getShoesCount = function (groups, sizes) {
    $.ajax({
        url: '/Store/getshoescount',
        type: 'GET',
        dataType: 'json',
        data: {
            'groups': groups,
            'sizes': sizes
        },
        success: function (data) {
            Djinaro.WriteResponseGoodsPaging(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
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
            console.log(x + '\n' + y + '\n' + z);
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
            if (divRating) {
                divRating.innerHTML = '';
                var sizesString = '';
                for (var i = 0; i < data.length; i++) {
                    sizesString += '<td class="cart-table btn-xs" bgcolor="f4f4f4" style="margin: 10px;">' + data[i].size + 'EU</td>';
                }
                var table = document.createElement('table');
                var row = document.createElement('tr');
                row.innerHTML = sizesString;
                table.appendChild(row);
                divRating.appendChild(table);
            }
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
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
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}
