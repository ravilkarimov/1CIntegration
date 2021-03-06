﻿
var Djinaro = {};


var activeSizes = [];
var activeBrands = [];
var priceLeft = 1500;
var priceRight = 5000;

Djinaro.ById = function(id) {
    return document.getElementById(id);
}

Djinaro.ByClassName = function(className) {
    return document.getElementsByClassName(className);
}

Djinaro.setDispEl = function (id, value) {
    Djinaro.ById(id).style.display = value;
    var activeItems;
    var h2Header;
    if (id === 'brands') {
        var itemsBrand = Djinaro.ById('brands').getElementsByTagName('input');
        for (var i = 0; itemsBrand[i]; ++i) {
            itemsBrand[i].checked = false;
            for (var k = 0; activeBrands[k]; k++) {
                if (itemsBrand[i] === activeBrands[k])
                    itemsBrand[i].checked = true;
            }
        }
        h2Header = Djinaro.ById('header-h3-brand');
        itemsBrand = Djinaro.ById('brands').getElementsByTagName('input');
        activeItems = [];
        for (var i = 0; itemsBrand[i]; ++i) {
            if (itemsBrand[i].checked) {
                activeItems.push(itemsBrand[i]);
            }
        }
        var brandString = '';
        for (var j = 0; j < activeItems.length; j++) {
            if (j > 0 && j <= 2) brandString += ', ';
            if (j <= 2) brandString += activeItems[j].getAttribute('brand');
        }
        if (activeItems.length > 0 && activeItems.length <= 2) {
            h2Header.innerHTML = brandString;
        } else if (activeItems.length > 2) {
            h2Header.innerHTML = brandString + '...';
        } else {
            h2Header.innerHTML = 'Бренд<i class="angle right icon" style="float:right;"></i>';
        }
    } else if (id === 'sizes') {
        var itemsSize = Djinaro.ById('sizes').getElementsByTagName('input');
        for (var i = 0; itemsSize[i]; ++i) {
            itemsSize[i].checked = false;
            for (var k = 0; activeSizes[k]; k++) {
                if (itemsSize[i] === activeSizes[k])
                    itemsSize[i].checked = true;
            }
        }
        h2Header = Djinaro.ById('header-h3-size');
        itemsSize = Djinaro.ById('sizes').getElementsByTagName('input');
        activeItems = [];
        for (var i = 0; itemsSize[i]; ++i) {
            if (itemsSize[i].checked) {
                activeItems.push(itemsSize[i]);
            }
        }
        var sizesString = '';
        for (var j = 0; j < activeItems.length; j++) {
            if (j > 0 && j <= 3) sizesString += ', ';
            if (j <= 3) sizesString += activeItems[j].getAttribute('size');
        }
        if (activeItems.length > 0 && activeItems.length <= 3) {
            h2Header.innerHTML = sizesString;
        } else if (activeItems.length > 3) {
            h2Header.innerHTML = sizesString + '...';
        } else {
            h2Header.innerHTML = 'Размер<i class="angle right icon" style="float:right;"></i>';
        }
    } else if (id === 'pricerange') {
        var inputPriceLeft = jQuery('#input-number1');
        var inputPriceRigth = jQuery('#input-number2');
        if (inputPriceLeft !== priceLeft) {
            inputPriceLeft.val(priceLeft);
        }
        if (inputPriceRigth !== priceRight) {
            inputPriceRigth.val(priceRight);
        }
    }
}

Djinaro.setHeadPrice = function() {
    var h3Header = Djinaro.ById('header-h3-price');
    var inputPriceLeft = jQuery('#input-number1');
    var inputPriceRigth = jQuery('#input-number2');
    h3Header.innerHTML = inputPriceLeft.val() + '-' + inputPriceRigth.val();
    priceLeft = inputPriceLeft.val();
    priceRight = inputPriceRigth.val();
}


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
Djinaro.filterMobileByGoods = function () {
    var brands = '';
    var inputElementsBrand = Djinaro.ById('brands').getElementsByTagName('input');
    for (var i = 0; inputElementsBrand[i]; ++i) {
        if (inputElementsBrand[i].checked) {
            if (brands.length === 0) {
                brands += "'" + inputElementsBrand[i].getAttribute('brand_id') + "'";
            } else {
                brands += ", '" + inputElementsBrand[i].getAttribute('brand_id') + "'";
            }
        }
    }
    var sizes = '';
    var inputElementsSizes = Djinaro.ById('sizes').getElementsByTagName('input');
    for (var i = 0; inputElementsSizes[i]; ++i) {
        if (inputElementsSizes[i].checked) {
            if (sizes.length === 0) {
                sizes += "'" + inputElementsSizes[i].getAttribute('size') + "'";
            } else {
                sizes += ", '" + inputElementsSizes[i].getAttribute('size') + "'";
            }
        }
    }
    var inputSearch = jQuery('#search-terms');
    var inputPriceLeft = jQuery('#input-number1');
    var inputPriceRigth = jQuery('#input-number2');
    var butonFetch = Djinaro.ById('fetch-button');
    Djinaro.getShoes(1, sizes, brands, inputSearch[0].value, inputPriceLeft.val(), inputPriceRigth.val(), parseInt(butonFetch.value));
}

Djinaro.filterByGoods = function () {
    var groupActive = jQuery('#menu_nav .active');
    var brandsActive = jQuery('#selectcontrolbrand .active');
    var sizesActive = jQuery('#selectcontrolsize .active');
    var inputSearch = jQuery('#search-terms');
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

        Djinaro.getShoes(parseInt(groupActive[0].id), sizes, brands, inputSearch[0].value, inputPriceLeft.val(), inputPriceRigth.val(), 0);
    }
}

Djinaro.WriteResponseGroups = function (data) {
    var groups = Djinaro.ById('groups');

    var title = document.createElement('h3');
    title.className = 'title-widget fancy-title';
    var title_div = document.createElement('span');
    title_div.innerHTML = 'Категории<i class="angle right icon" style="float:right;"></i>';
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

Djinaro.WriteMobileResponseBrands = function (data) {
    var brands = Djinaro.ById('brands');
    var itemHead = document.createElement('div');
    itemHead.className = 'item';
    itemHead.addEventListener("click", function () {
        Djinaro.setDispEl('brands', 'none');
        Djinaro.setDispEl('right-menu-main', 'block');
    });
    var h2 = document.createElement('h2');
    h2.innerHTML = '<i class="angle left icon" style="float:left;"></i>БРЕНД';
    itemHead.appendChild(h2);
    brands.appendChild(itemHead);

    for (var i = 0; i < data.length; i++) {
        var divItemBrand = document.createElement('div');
        divItemBrand.className = 'item left';

        var div = document.createElement('div');
        div.className = 'ui slider checkbox full-width';

        var inputBrand = document.createElement('input');
        inputBrand.type = 'checkbox';
        inputBrand.setAttribute('name', 'newsletter');
        inputBrand.setAttribute('brand_id', data[i].brand_id);
        inputBrand.setAttribute('brand', data[i].brand);
        inputBrand.className = 'full-width';

        var label = document.createElement('label');
        label.innerHTML = data[i].brand;

        div.appendChild(inputBrand);
        div.appendChild(label);
        divItemBrand.appendChild(div);
        brands.appendChild(divItemBrand);
    }

    var itemFoot = document.createElement('div');
    itemFoot.className = 'item';
    itemFoot.style = 'text-align: center;'
    var button = document.createElement('button');
    button.className = 'ui left labeled icon button';
    button.addEventListener("click", function () {
        var itemsBrand = Djinaro.ById('brands').getElementsByTagName('input');
        activeBrands = [];
        for (var i = 0; itemsBrand[i]; ++i) {
            if (itemsBrand[i].checked) {
                activeBrands.push(itemsBrand[i]);
            }
        }
        Djinaro.setDispEl('brands', 'none'); filterApply();
    });
    button.innerHTML = 'Применить';
    var iElement = document.createElement('i');
    iElement.className = 'left arrow icon';
    button.appendChild(iElement);
    itemFoot.appendChild(button);
    brands.appendChild(itemFoot); 
}

Djinaro.WriteResponseBrands = function (data) {
    var brands = Djinaro.ById('selectcontrolbrand');
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
        openmenuText: '<i class="angle left icon" style="float:left;"></i>БРЕНД', // Text for button
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

            if (activeItems.length > 0 && activeItems.length <= 4) {
                var brandString = '';
                for (var j = 0; j < activeItems.length; j++) {
                    if (j > 0) brandString += ', ';
                    brandString += jQuery(activeItems[j])[0].innerHTML;
                }
                jQuery('#selectcontrolbrand .mcs-open')[0].innerHTML = brandString;
            }
            else if (activeItems.length > 4) {
                jQuery('#selectcontrolbrand .mcs-open')[0].innerHTML = activeItems.length + ' из ' + items.length + ' выбрано';
            } else {
                jQuery('#selectcontrolbrand .mcs-open')[0].innerHTML = '<i class="angle left icon" style="float:left;"></i>БРЕНД';
            }
            Djinaro.filterByGoods();
        }
    });
} 

Djinaro.WriteMobileResponseSizes = function (data) {
    var sizes = Djinaro.ById('sizes');
    var itemHead = document.createElement('div');
    itemHead.className = 'item';
    itemHead.addEventListener("click", function () {
        Djinaro.setDispEl('sizes', 'none');
        Djinaro.setDispEl('right-menu-main', 'block');
    });
    var h2 = document.createElement('h2');
    h2.innerHTML = '<i class="angle left icon" style="float:left;"></i>РАЗМЕР';
    itemHead.appendChild(h2);
    sizes.appendChild(itemHead);

    for (var i = 0; i < data.length; i++) {
        var divItemBrand = document.createElement('div');
        divItemBrand.className = 'item left';

        var div = document.createElement('div');
        div.className = 'ui slider checkbox full-width';

        var inputSize = document.createElement('input');
        inputSize.type = 'checkbox';
        inputSize.setAttribute('name', 'newsletter');
        inputSize.setAttribute('size', data[i].size);
        inputSize.className = 'full-width';

        var label = document.createElement('label');
        label.innerHTML = data[i].size;

        div.appendChild(inputSize);
        div.appendChild(label);
        divItemBrand.appendChild(div);
        sizes.appendChild(divItemBrand);
    }

    var itemFoot = document.createElement('div');
    itemFoot.className = 'item';
    itemFoot.style = "text-align: center;"
    var button = document.createElement('button');
    button.className = 'ui left labeled icon button';
    button.addEventListener("click", function () {
        var itemsSize = Djinaro.ById('sizes').getElementsByTagName('input');
        activeSizes = [];
        for (var i = 0; itemsSize[i]; ++i) {
            if (itemsSize[i].checked) {
                activeSizes.push(itemsSize[i]);
            }
        }
        Djinaro.setDispEl('sizes', 'none'); filterApply();
    });
    button.innerHTML = 'Применить';
    var iElement = document.createElement('i');
    iElement.className = 'left arrow icon';
    button.appendChild(iElement);
    itemFoot.appendChild(button);
    sizes.appendChild(itemFoot);
}

Djinaro.WriteResponseSizes = function (data) {
    var sizes = Djinaro.ById('selectcontrolsize');
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
        openmenuText: 'РАЗМЕР<i class="angle right icon" style="float:right;"></i>', // Text for button
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

            if (activeItems.length > 0 && activeItems.length <= 5) {
                var sizesString = '';
                for (var j = 0; j < activeItems.length; j++) {
                    if (j > 0) sizesString += ', ';
                    sizesString += jQuery(activeItems[j]).attr("data");
                }
                jQuery('#selectcontrolsize .mcs-open')[0].innerHTML = sizesString;
            }
            else if (activeItems.length > 5) {
                jQuery('#selectcontrolsize .mcs-open')[0].innerHTML = activeItems.length + ' из ' + items.length + ' выбрано';
            } else {
                jQuery('#selectcontrolsize .mcs-open')[0].innerHTML = '<i class="angle left icon" style="float:left;"></i>РАЗМЕР';
            }
            Djinaro.filterByGoods();
        }
    });
}

Djinaro.WriteResponseGoods = function (data) {
    var categories = Djinaro.ById('goods');
    jQuery('#goods').children().remove();
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var row = document.createElement('div');
            row.className = 'row';
            row.innerHTML = data[i];
            categories.appendChild(row);
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

        if (jQuery().magnificPopup) {
            jQuery('[data-lightbox=image], .lightbox').each(function(index, element) {
                jQuery(this).magnificPopup({ type: 'image', mainClass: 'mfp-fade', removalDelay: 300, fixedContentPos: false, fixedBgPos: true, overflowY: 'auto', closeOnContentClick: true });
            });
        };
    }
}

Djinaro.MobileWriteResponseGoods = function (data) {
    var categories = Djinaro.ById('goods');
    var butonFetch = Djinaro.ById('fetch-button');
    if (parseInt(butonFetch.value) < 2) {
        jQuery('#goods').children().remove();
    }
    var row;

    if (data.length > 0 && data[0] !== "") {
        for (var i = 0; i < data.length; i++) {
            row = document.createElement('div');
            row.className = 'row';
            row.innerHTML = data[i];
            categories.appendChild(row);
        }
    } else {
        jQuery('#goods').children().remove();
        row = document.createElement('div');
        row.className = 'row';
        var col1 = document.createElement('div');
        row.className = 'col-xs-12';
        var p = document.createElement('p');
        p.innerHTML = "По заданному фильтру товаров не найдено";
        p.style = "text-align:center;";
        col1.appendChild(p);
        row.appendChild(col1);
        categories.appendChild(row);
    }

    var countProduct = document.getElementsByClassName('shop-product shop-mobile').length;
    jQuery('#fetch-button').hide();
    if (countProduct % 50 === 0) {
        jQuery('#fetch-button').show();
    }

    if (jQuery().magnificPopup) {
        jQuery('[data-lightbox=image], .lightbox').each(function(index, element) {
            jQuery(this).magnificPopup({
                type: 'image',
                mainClass: 'mfp-fade',
                removalDelay: 300,
                fixedContentPos: false,
                fixedBgPos: true,
                overflowY: 'auto',
                closeOnContentClick: true,
                callbacks: {
                    open: function() {
                        var b1 = document.getElementsByClassName("mfp-close")[0];
                        var b2 = document.getElementsByTagName("figure")[0];
                        var b3 = document.getElementsByClassName("mfp-content")[0];
                        if (b1 && b2) {
                            b1.parentNode.insertBefore(b2, b1);
                            b1.style.marginTop = "0px";
                            if (b3) {
                                b3.style.padding = "0px";
                            }
                        }
                    }
                }
            });
        });
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
    var paging = Djinaro.ById('paging');
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

Djinaro.getShoes = function (groups, sizes, brands, search, price_1, price_2, fetch) {
    if (device) {
        if (device.windows()) {
            //Полная версия
            jQuery.ajax({
                url: './Store/getshoes',
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
        } else if (device.mobile() || device.iphone()) {
            //Мобильная
            jQuery(function() {
                jQuery.ajax({
                    url: '/mobile/getshoes',
                    type: 'GET',
                    async: true,
                    data: {
                        'groups': groups,
                        'sizes': sizes,
                        'brands': brands,
                        'search': search,
                        'price_1': price_1,
                        'price_2': price_2,
                        'fetch': fetch
                    },
                    dataType: 'json',
                    success: function(data) {
                        Djinaro.MobileWriteResponseGoods(data);
                    },
                    error: function(x, y, z) {
                        console.log(x + '\n' + y + '\n' + z);
                    }
                });
            });
        }
    }
}

Djinaro.getShoesCount = function (groups, sizes, count, brands) {
    jQuery.ajax({
		url: './Store/getshoescount',
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
		url: './Store/getgroups',
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
		url: './Store/getbrands',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            if (device) {
                if (device.windows()) {
                    //Полная версия
                    Djinaro.WriteResponseBrands(data);
                } else if (device.mobile() || device.iphone()) {
                    //Мобильная
                    Djinaro.WriteMobileResponseBrands(data);
                }
            }
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

var isQverySize = 0;

Djinaro.getSizesByGood = function (good_key) {
    var divRating = Djinaro.ById('rating_' + good_key);
    if (divRating && divRating.innerHTML == '' && isQverySize == 0) {
        isQverySize = 1;
        divRating.innerHTML = '';
        divRating.style = 'border-top: 3px solid #ececec;';
        var title = document.createElement('span');
        title.innerHTML = 'Размеры в наличии:  ';
        divRating.appendChild(title);
        jQuery.ajax({
			url: './Store/getsizesgood',
            type: 'GET',
            async: true,
            dataType: 'json',
            data: { id: good_key },
            success: function (data) {
                var divRating = Djinaro.ById('rating_' + good_key);
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
		url: './Store/getsizes',
        type: 'GET',
        async: true,
        dataType: 'json',
        success: function (data) {
            if (device.windows()) {
                //Полная версия
                Djinaro.WriteResponseSizes(data);
            } else if (device.mobile() || device.iphone()) {
                //Мобильная
                Djinaro.WriteMobileResponseSizes(data);
            }
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
var lock;
 
Djinaro.ClassOpen = function(e) {
    var selector = jQuery(e.currentTarget);
    var aMscOpen = jQuery('.mcs-open', selector);
    var aAccordionOpen = jQuery('.panel-collapse.collapsed.collapse', selector);
    if (!elementTarget || elementTarget[0] !== selector[0] || selector.find(e.relatedTarget).length == 0) {
        if (aMscOpen && e.type == "mouseover") {
            if (aMscOpen[0] && aMscOpen[0].className == 'mcs-open mcs') {
                aMscOpen.click();
            } else if (aAccordionOpen[0] && aAccordionOpen[0].className == 'panel-collapse collapsed in') {
                aAccordionOpen.click();
            }
        } else if (aMscOpen && e.type == "mouseout") {
            if (aMscOpen[0] && aMscOpen[0].className == 'mcs-open mcs open') {
                aMscOpen.click();
            } else if (aAccordionOpen[0] && aAccordionOpen[0].className == 'panel-collapse collapsed collapse') {
                aAccordionOpen.click();
            }
        }
        elementTarget = selector;
    }
}

