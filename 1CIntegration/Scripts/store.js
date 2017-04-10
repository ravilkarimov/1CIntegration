
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
    var brandsActive = jQuery('#selectcontrolbrand .active');
    var sizesActive = jQuery('#selectcontrolsize .active');
    var inputSearch = jQuery('#searchinput');
    var inputPriceLeft = jQuery('#input-number1');
    var inputPriceRigth = jQuery('#input-number2');
    
    if (groupActive[0].nodeName == 'A') {
        groupActive = groupActive.parentElement;
    }
    if (groupActive[0].nodeName == 'LI') {
        var brands = '';
        for (b = 0; b < brandsActive.length; b++) {
            if (brands.length == 0) {
                brands += "'" + jQuery(brandsActive[b]).attr("data") + "'";
            } else {
                brands += ", '" + jQuery(brandsActive[b]).attr("data") + "'";
            }
        }
        var sizes = '';
        for (b = 0; b < sizesActive.length; b++) {
            if (sizes.length == 0) {
                sizes += "'" + jQuery(sizesActive[b]).attr("data") + "'";
            } else {
                sizes += ", '" + jQuery(sizesActive[b]).attr("data") + "'";
            }
        }

        Djinaro.getShoes(parseInt(groupActive[0].id), sizes, brands, inputSearch[0].value, inputPriceLeft.val(), inputPriceRigth.val());
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
    var brands = document.getElementById('selectcontrolbrand');
    var select = null;
    if (jQuery('select', brands).length !== 0) {
        select = jQuery('select', brands)[0];
    } else {
        select = document.createElement('select');
        select.setAttribute("name", "brand");
        brands.appendChild(select);
    }
    for (var i = 0; i < data.length; i++) {
        var option = document.createElement('option');
        option.innerHTML = data[i].brand;
        option.setAttribute("value", data[i].brand_id);
        select.appendChild(option);
    }
    jQuery('#selectcontrolbrand').MultiColumnSelect({
        multiple: true,              // Single or Multiple Select- Default Single
        useOptionText: true,               // Use text from option. Use false if you plan to use images
        hideselect: true,               // Hide Original Select Control
        openmenuClass: 'mcs-open',         // Toggle Open Button Class
        openmenuText: 'БРЕНД', // Text for button
        openclass: 'open',             // Class added to Toggle button on open
        containerClass: 'mcs-container',    // Class of parent container
        itemClass: 'mcs-item',         // Class of menu items
        idprefix: null,                        // Assign as ID to items eg 'item-' = #item-1, #item-2, #item-3...
        duration: 200,                         //Toggle Height duration
        onOpen: function () { },
        onClose: function () { },
        onItemSelect: function(e) {
            var activeItems = jQuery('#selectcontrolbrand .active');
            var items = jQuery('#selectcontrolbrand .mcs-item');

            if (activeItems.length > 0 && activeItems.length <= 3) {
                var brandString = '';
                for (var j = 0; j < activeItems.length; j++) {
                    if (j > 0) brandString += ', ';
                    brandString += jQuery(activeItems[j])[0].innerHTML;
                }
                jQuery('#selectcontrolbrand .mcs-open')[0].innerHTML = 'БРЕНД: ' + brandString;
            }
            else if (activeItems.length > 3) {
                jQuery('#selectcontrolbrand .mcs-open')[0].innerHTML = 'БРЕНД: ' + activeItems.length + ' из ' + items.length + ' выбрано';
            } else {
                jQuery('#selectcontrolbrand .mcs-open')[0].innerHTML = 'БРЕНД';
            }
            Djinaro.filterByGoods();
        }
    });
}

Djinaro.WriteResponseSizes = function (data) {
    var sizes = document.getElementById('selectcontrolsize');
    var select = null;
    if (jQuery('select', sizes).length !== 0) {
        select = jQuery('select', sizes)[0];
    } else {
        select = document.createElement('select');
        select.setAttribute("name", "size");
        sizes.appendChild(select);
    }
    for (var i = 0; i < data.length; i++) {
        var option = document.createElement('option');
        option.innerHTML = data[i].size;
        option.setAttribute("value", data[i].size);
        select.appendChild(option);
    }
    jQuery('#selectcontrolsize').MultiColumnSelect({
        multiple: true,              // Single or Multiple Select- Default Single
        useOptionText: true,               // Use text from option. Use false if you plan to use images
        hideselect: true,               // Hide Original Select Control
        openmenuClass: 'mcs-open',         // Toggle Open Button Class
        openmenuText: 'РАЗМЕР', // Text for button
        openclass: 'open',             // Class added to Toggle button on open
        containerClass: 'mcs-container',    // Class of parent container
        itemClass: 'mcs-item',         // Class of menu items
        idprefix: null,                        // Assign as ID to items eg 'item-' = #item-1, #item-2, #item-3...
        duration: 200,                         //Toggle Height duration
        onOpen: function () { },
        onClose: function () { },
        onItemSelect: function () {
            var activeItems = jQuery('#selectcontrolsize .active');
            var items = jQuery('#selectcontrolsize .mcs-item');

            if (activeItems.length > 0 && activeItems.length <= 3) {
                var sizesString = '';
                for (var j = 0; j < activeItems.length; j++) {
                    if (j > 0) sizesString += ', ';
                    sizesString += jQuery(activeItems[j]).attr("data");
                }
                jQuery('#selectcontrolsize .mcs-open')[0].innerHTML = 'РАЗМЕР: ' + sizesString;
            }
            else if (activeItems.length > 3) {
                jQuery('#selectcontrolsize .mcs-open')[0].innerHTML = 'РАЗМЕР: ' + activeItems.length + ' из ' + items.length + ' выбрано';
            } else {
                jQuery('#selectcontrolsize .mcs-open')[0].innerHTML = 'РАЗМЕР';
            }
            Djinaro.filterByGoods();
        }
    });
}

Djinaro.WriteResponseGoods = function (data) {
    var categories = document.getElementById('goods');
    jQuery('#goods').children().remove();
    if (data.length > 0) {
        var countRow = data.length / 6;
        var itemIndex = 0;

        for (var i = 0; i < countRow; i++) {

            var row = document.createElement('div');
            row.className = 'row';
            var addItem = 1;

            for (var j = 0; j < addItem; j++) {
                if (data[itemIndex]) {
                    var goodKey = data[itemIndex].good_key;
                    var src = "../img/theme/hole-2038430_640.png";
                    var realsrc = "../store/GetImgProductMin?good_id=" + data[itemIndex].good_id;
                    if (i <= 3) {
                        src = realsrc;
                        realsrc = "";
                    }
                    if (data[itemIndex]) {
                        var stringElement =
                            '<div class="col-xs-2">' +
                                '   <div class="shop-product animation fadeInUp delay4 animation-active" id="shop-product-' + goodKey + '">' +
                                '       <div class="overlay-wrapper">' +
                                '           <img src="' + src + '" realsrc="' + realsrc + '" class="img-zoom owl-item" width="1200" height="900" alt="' + data[itemIndex].good + '">' +
                                '           <div class="overlay-wrapper-content"> ' +
                                '				<div class="overlay-details"> ' +
                                '        			<a href="../store/GetImgProduct?good_id=' + data[itemIndex].good_id + '" class="color-white" data-lightbox="image""> ' +
                                '                       <span class="icon gfx-zoom-in-1" ></span>' +
                                '                   </a> ' +
                                '    			</div> ' +
                                '    		</div> ' +
                                '       </div>' +
                                '       <div class="shop-product-info">' +
                                '           <a><h5 class="product-name">' + data[itemIndex].good + ' </h5></a>' +
                                '           <p class="product-price margin-top-10">' + data[itemIndex].price + ' ₽ </p>' +
                                '           <div class="rating" id="rating_' + goodKey + '"></div>' +
                                '       </div>' +
                                '    </div>' +
                                '    <div class="white-space space-small"></div>' +
                                '</div> ';
                        row.innerHTML += stringElement;
                        categories.appendChild(row);

                        if (addItem < 6) addItem++;

                        // Get the <span> element that closes the modal
                        var span = document.getElementsByClassName("close")[0];

                        // When the user clicks on <span> (x), close the modal
                        span.onclick = function () {
                            modal.style.display = "none";
                        }
                    }

                    itemIndex += 1;
                }
            }
        }

        var shopProducts = jQuery('.shop-product');
        var listener = function(a) {
            var current = jQuery(a.currentTarget);
            if (current.length == 1) {
                Djinaro.getSizesByGood(current[0].id.replace('shop-product-', ''));
            }
        }
        for (var s = 0; s < shopProducts.length; s++) {
            jQuery('#' + shopProducts[s].id).on('mouseover', listener);
        }

        var imgs = document.getElementsByTagName('img');
        for (var i = 0; i < imgs.length; i++) {

            var img = imgs[i];

            var realsrc = img.getAttribute('realsrc');
            if (!realsrc) continue;

            img.src = realsrc;
            img.async = true;
            img.setAttribute('realsrc', '');
            img.onclick = function () {
                document.getElementById('myModal').style.display = "block";
                document.getElementById("img01").src = this.src;
                document.getElementById("caption").innerHTML = this.alt;
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

Djinaro.getShoes = function (groups, sizes, brands, search, price_1, price_2) {
    jQuery.ajax({
        url: '/Store/getshoes',
        type: 'GET',
        async: true,
        data: {
            'groups': groups,
            'sizes': sizes,
            'brands': brands,
            'search': search,
            'price_1': price_1,
            'price_2': price_2
        },
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

var isQverySize = 0;

Djinaro.getSizesByGood = function (good_key) {
    var divRating = document.getElementById('rating_' + good_key);
    if (divRating && divRating.innerHTML == '' && isQverySize == 0) {
        isQverySize = 1;
        divRating.innerHTML = '';
        divRating.style = 'border-top: 3px solid #ececec;';
        var title = document.createElement('span');
        title.innerHTML = 'Размеры в наличии:  ';
        divRating.appendChild(title);
        jQuery.ajax({
            url: '/Store/getsizesgood',
            type: 'GET',
            async: true,
            dataType: 'json',
            data: { id: good_key },
            success: function (data) {
                var divRating = document.getElementById('rating_' + good_key);
                if (divRating) {
                    if (data && data.length > 0) {
                        data.sort().reverse();
                        var countTDinTr = Math.ceil(data.length / 10);
                        if (countTDinTr < 1) countTDinTr = 1;
                        var currentTd = 0;
                        for (var r = 0; r < countTDinTr; r++) {
                            var sizesString = '';
                            var row = document.createElement('p');
                            row.className = 'margin-bottom0';
                            for (var i = 0; i <= Math.ceil(data.length / countTDinTr) ; i++) {
                                var tmp = data.length - 1;
                                if (tmp > currentTd) {
                                    sizesString += '' + data[currentTd].size + ' | ';
                                    currentTd++;
                                }
                                if (tmp == currentTd) {
                                    sizesString += '' + data[currentTd].size + '';
                                    currentTd++;
                                }
                            }
                            row.innerHTML = sizesString;
                            divRating.appendChild(row);
                            divRating.style.background = 'none';
                        }
                    }
                }
                isQverySize = 0;
            },
            error: function (x, y, z) {
                console.log(x + '\n' + y + '\n' + z);
            }
        });
    }
}

Djinaro.getAllSizes = function() {
    jQuery.ajax({
        url: '/Store/getsizes',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function(data) {
            Djinaro.WriteResponseSizes(data);
        },
        error: function(x, y, z) {
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

var elementTarget;

Djinaro.ClassOpen = function(e) {
    var selector = jQuery(e.currentTarget);
    var aMscOpen = jQuery('.mcs-open', selector);
    var aAccordionOpen = jQuery('.accordion-toggle', selector);
    if (!elementTarget || elementTarget[0] !== selector[0] || selector.find(e.relatedTarget).length == 0) {
        if (aMscOpen && e.type == "mouseover") {
            if (aMscOpen[0] && aMscOpen[0].className == 'mcs-open mcs') {
                aMscOpen.click();
            } else if (aAccordionOpen[0] && aAccordionOpen[0].className == 'accordion-toggle collapsed') {
                aAccordionOpen.click();
            }
        } else if (aMscOpen && e.type == "mouseout") {
            if (aMscOpen[0] && aMscOpen[0].className == 'mcs-open mcs open') {
                aMscOpen.click();
            } else if (aAccordionOpen[0] && aAccordionOpen[0].className == 'accordion-toggle') {
                aAccordionOpen.click();
            }
        }
        elementTarget = selector;
    }
}

