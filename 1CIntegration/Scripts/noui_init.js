var html5Slider = document.getElementById('html5');

noUiSlider.create(html5Slider, {
    start: [1500, 5000],
    connect: true,
    range: {
        'min': 0,
        'max': 10000
    }
});

var inputNumber2 = document.getElementById('input-number2');
var inputNumber1 = document.getElementById('input-number1');

html5Slider.noUiSlider.on('update', function (values, handle) {

    var value = values[handle];

    if (handle) {
        inputNumber2.value = Math.round(value);
    } else {
        inputNumber1.value = Math.round(value);
    }
});

inputNumber2.addEventListener('change', function () {
    html5Slider.noUiSlider.set([null, this.value]);
});
inputNumber1.addEventListener('change', function () {
    html5Slider.noUiSlider.set([this.value, null]);
});

Djinaro.filterByGoods();

var timeNoUi = setTimeout(function () { }, 500);
var observer = new MutationObserver(function (mutations) {
    clearInterval(timeNoUi);
    timeNoUi = setTimeout(function () { Djinaro.filterByGoods(); }, 300);
});
var noUiOrigin = document.getElementsByClassName('noUi-origin');
observer.observe(noUiOrigin[0], {
    attributes: true
});
observer.observe(noUiOrigin[1], {
    attributes: true
});
jQuery("#search-terms")
    .keyup(function () {
        clearInterval(timeNoUi);
        timeNoUi = setTimeout(function () { Djinaro.filterByGoods(); }, 300);
    })
    .keyup();

window.onload = function () {
    if (jQuery().magnificPopup) {
        jQuery('[data-lightbox=image], .lightbox').each(function (index, element) { jQuery(this).magnificPopup({ type: 'image', mainClass: 'mfp-fade', removalDelay: 300, fixedContentPos: false, fixedBgPos: true, overflowY: 'auto', closeOnContentClick: true }); });
        jQuery('[data-lightbox=video], [data-lightbox=map], [data-lightbox=iframe], .lightbox-video, .lightbox-map, .lightbox-iframe').each(function (index, element) { jQuery(this).magnificPopup({ mainClass: 'mfp-fade', removalDelay: 300, fixedContentPos: false, fixedBgPos: true, overflowY: 'auto', type: 'iframe', fixedContentPos: false }); });
        jQuery('[data-lightbox=gallery], .lightbox-gallery').each(function (index, element) {
            jQuery(this).magnificPopup({
                mainClass: 'mfp-fade',
                removalDelay: 300,
                fixedContentPos: false,
                fixedBgPos: true,
                overflowY: 'auto',
                type: 'image',
                delegate: 'a',
                gallery: { enabled: true }
            });
        });
    };

    jQuery('.mcs-open').hover(function () {
        jQuery(this).addClass('open').find('.mcs-open').first().stop(true, true).slideDown(300);
        jQuery(this).parent().find('.mcs-container').css("display", "block");
    }, function () {
        jQuery(this).removeClass('open').find('.mcs-open').first().stop(true, true).hide(300);
        jQuery(this).parent().find('.mcs-container').css("display", "none");
    });

    jQuery('.mcs-container').hover(function () {
        jQuery(this).parent().find('.mcs-open').addClass('open').find('.mcs-open').first().stop(true, true).slideDown(300);
        jQuery(this).css("display", "block");
    }, function () {
        jQuery(this).parent().find('.mcs-open').removeClass('open').find('.mcs-open').first().stop(true, true).hide(300);
        jQuery(this).css("display", "none");
    });

    jQuery('.tocollapse').hover(function () {
        jQuery(this).addClass('open').find('.mcs-open collapsed').first().stop(true, true).slideDown(300);
        jQuery(this).parent().find('.panel-collapse').css("display", "block");
    }, function () {
        jQuery(this).removeClass('open').find('.mcs-open collapsed').first().stop(true, true).hide(300);
        jQuery(this).parent().find('.panel-collapse').css("display", "none");
    });
};