
var Djinaro = {};

Djinaro.sortingProduct = function() {
    var groupBtnSort = jQuery('#group_btn_sort');
    if (groupBtnSort) {
        groupBtnSort.on('click', function(b) {
            var btnClick = b.target;
            jQuery('#group_btn_sort .active')[0].className = 'btn btn-primary';
            btnClick.className = 'btn btn-primary active';
            Djinaro.filterByGoods();
        });
    }

    var groupBtnPaging = jQuery('#group_btn_paging');
    if (groupBtnPaging) {
        groupBtnPaging.on('click', function (b) {
            var btnClick = b.target;
            jQuery('#group_btn_paging .active')[0].className = 'btn btn-primary btn-xs btn-alt btn-circle';
            btnClick.className = 'btn btn-primary btn-xs btn-alt btn-circle active';
            Djinaro.filterByGoods();
        });
    }
}

Djinaro.filterByGoods = function () {
    var groupActive = jQuery('#menu_nav .active');
    var sortActive = jQuery('#group_btn_sort .active');
    var brandsActive = jQuery('#brands_chosen .search-choice');
    var sizesActive = jQuery('#sizes_chosen .search-choice');
    var inputSearch = jQuery('#searchinput')
    
    if (groupActive[0].nodeName == 'A') {
        groupActive = groupActive.parentElement;
    }
    if (groupActive[0].nodeName == 'LI') {
        var brands = '';
        for (b = 0; b < brandsActive.length; b++) {
            if (brands.length == 0) {
                brands += "'" + brandsActive[b].value + "'";
            } else {
                brands += ", '" + brandsActive[b].value + "'";
            }
        }
        var sizes = '';
        for (b = 0; b < sizesActive.length; b++) {
            if (sizes.length == 0) {
                sizes += "'" + sizesActive[b].value + "'";
            } else {
                sizes += ", '" + sizesActive[b].value + "'";
            }
        }

        jQuery.ajax({
            url: '/Store/setfilter',
            type: 'GET',
            async: true,
            dataType: 'json',
            data: {
                'groups': parseInt(groupActive[0].id),
                'sizes': sizes,
                'brands': brands,
                'search': inputSearch[0].value
            },
            success: function (data) {
                Djinaro.getShoes();
            },
            error: function (x, y, z) {
                console.log(x + '\n' + y + '\n' + z);
            }
        });
    }
}

Djinaro.WriteResponseGroups = function (data) {
    var groups = document.getElementById('groups');

    var title = document.createElement('h3');
    title.className = 'title-widget fancy-title';
    var title_div = document.createElement('span');
    title_div.innerHTML = 'Категории';
    title.appendChild(title_div);

    var list = document.createElement('ul');
    list.className = 'shop-product-categories';
    jQuery(list).on('click', function (a) {
        var liClick = a.target;
        if (liClick.nodeName == 'A') {
            liClick = liClick.parentElement;
        }
        if (liClick.nodeName == 'LI') {
            var ulClick = a.currentTarget;
            jQuery('.active', ulClick)[0].className = '';
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

Djinaro.WriteResponseBrands = function (data) {
    var brands = document.getElementById('brands');

    for (var i = 0; i < data.length; i++) {
        var option = document.createElement('option');
        option.innerHTML = data[i].brand;
        option.value = data[i].brand_id;
        brands.appendChild(option);
    }
}

Djinaro.WriteResponseSizes = function (data) {
    var sizes = document.getElementById('sizes');
    
    for (var i = 0; i < data.length; i++) {
        var option = document.createElement('option');
        option.innerHTML = data[i].size;
        option.value = data[i].size;
        sizes.appendChild(option);
    }
}

Djinaro.WriteResponseGoods = function (data) {
    var categories = document.getElementById('goods');
    jQuery('#goods').children().remove();
    if (data.length > 0) {
        var countRow = data.length / 4;
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
                            '<div class="col-md-3">' +
                                '   <!-- Shop Product -->' +
                                '   <div class="shop-product" id="shop-product-' + goodKey + '">' +
                                '       <!-- Overlay Img -->' +
                                '       <div class="overlay-wrapper">' +
                                '           <img src="../img/theme/hole-2038430_640.png" realsrc="../store/GetImgProductMin?good_id=' + data[itemIndex].good_id + '" class="img-zoom owl-item" width="1200" height="900" alt="' + data[itemIndex].feature + '">' +
                                '           <div class="overlay-wrapper-content"> ' +
                                '				<div class="overlay-details"> ' +
                                '        			<a href="../store/GetImgProduct?good_id=' + data[itemIndex].good_id + '" class="color-white" data-lightbox="image""> ' +
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
                        
                        if (addItem < 4) addItem++;
                    }

                    itemIndex += 1;
                }
            }

            var shopProducts = jQuery('.shop-product');
            var listener = function (a) {
                var current = jQuery(a.currentTarget);
                if (current.length == 1) {
                    Djinaro.getSizesByGood(current[0].id.replace('shop-product-', ''));
                }
            }
            for (var s = 0; s < shopProducts.length; s++) {
                jQuery('#'+shopProducts[s].id).on('mouseover', listener);
            }
        }
    }
}

Djinaro.openModalProduct = function() {
    if (jQuery.magnificPopup) {
        var popupInfo = jQuery('.pswp');
        if (popupInfo) {
            jQuery.magnificPopup.open({
                items: {
                    src: popupInfo,
                    type: 'inline'
                }
            });
        }
    };
}

Djinaro.WriteResponseGoodsPaging = function (data) {
    var pagingActive = jQuery('#paging .active');
    var numberPagingActive = 1;
    if (pagingActive.length == 1) numberPagingActive = parseInt(pagingActive[0].innerText);
    var paging = document.getElementById('paging');
    jQuery('#paging').children().remove();

    var ul = document.createElement('ul');
    ul.className = 'pagination';

    var leftA = document.createElement('li');
    var linkLeft = document.createElement('a');
    linkLeft.link = "#";
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
    rightA.appendChild(linkRight);
    ul.appendChild(linkRight);

    jQuery(ul).on('click', function (a) {
        var liClick = a.target;
        if (liClick.nodeName == 'A') {
            liClick = liClick.parentElement;
        }
        if (liClick.nodeName == 'LI') {
            var ulClick = a.currentTarget;
            jQuery('.active', ulClick)[0].className = '';
            liClick.className = 'active';
            Djinaro.filterByGoods();
        }
    });

    paging.appendChild(ul);
}

Djinaro.getShoes = function () {
    jQuery.ajax({
        url: '/Store/getshoes',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseGoods(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.getShoesCount = function (groups, sizes, count, brands) {
    jQuery.ajax({
        url: '/Store/getshoescount',
        type: 'GET',
        async: true,
        dataType: 'json',
        data: {
            'groups': groups,
            'sizes': sizes,
            'brands': brands
        },
        success: function (data) {
            //Djinaro.WriteResponseGoodsPaging(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.getAllGroups = function () {
    jQuery.ajax({
        url: '/Store/getgroups',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseGroups(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.getAllBrands = function () {
    jQuery.ajax({
        url: '/Store/getbrands',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseBrands(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.getSizesByGood = function (good_key) {
    var divRating = document.getElementById('rating_' + good_key);
    if (divRating && divRating.innerHTML == '') {
        divRating.innerHTML == ' '
        jQuery.ajax({
            url: '/Store/getsizesgood',
            type: 'GET',
            async: true,
            dataType: 'json',
            data: { id: good_key },
            success: function(data) {
                var divRating = document.getElementById('rating_' + good_key);
                if (divRating) {
                    divRating.innerHTML = '';
                    if (data && data.length > 0) {
                        data.sort().reverse();
                        var countTDinTr = Math.ceil(data.length / 5);
                        if (countTDinTr < 1) countTDinTr = 1;
                        var currentTd = 0;
                        for (var r = 0; r < countTDinTr; r++) {
                            var sizesString = '';
                            var table = document.createElement('table');
                            var row = document.createElement('tr');
                            for (var i = 0; i <= Math.ceil(data.length / countTDinTr); i++) {
                                if (data.length > currentTd) {
                                    sizesString += '<td class="cart-table btn-xs" bgcolor="f4f4f4" style="margin: 0px;">' + data[currentTd].size + ' EU</td>';
                                    currentTd++;
                                }
                            }
                            row.innerHTML = sizesString;
                            table.appendChild(row);
                            divRating.appendChild(table);
                        }
                    }
                }
            },
            error: function(x, y, z) {
                console.log(x + '\n' + y + '\n' + z);
            }
        });
    }
}

Djinaro.getAllSizes = function () {
    jQuery.ajax({
        url: '/Store/getsizes',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            Djinaro.WriteResponseSizes(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

Djinaro.isVisible = function (elem) {

    var coords = elem.getBoundingClientRect();
    var windowHeight = document.documentElement.clientHeight;
    
    var topVisible = coords.top > 0 && coords.top < windowHeight;
    var bottomVisible = coords.bottom < windowHeight && coords.bottom > 0;

    return topVisible || bottomVisible;
}

Djinaro.showVisible = function () {
    var imgs = document.getElementsByTagName('img');
    for (var i = 0; i < imgs.length; i++) {

        var img = imgs[i];

        var realsrc = img.getAttribute('realsrc');
        if (!realsrc) continue;

        if (Djinaro.isVisible(img)) {
            img.src = realsrc;
            img.async = true;
            img.setAttribute('realsrc', '');
        }
    }

}

