/*
 * jQuery Multi-Column-Select v0.5
 * Copyright (c) 2014 DanSmith
 * Licensed under MIT
 *
 */
(function ($) {

//private functions
    var itemclick = function (selector, itemClass, args) {
        var $itemdata = $(selector).attr('data');
        var $menucontainer = $(selector).parent();
        if ($menucontainer.hasClass('Multi')) {
            if ($(selector).hasClass('active')) {
                //already selected, unselect it
                $(selector).removeClass('active');
                var removeItem = $itemdata; //ID to be removed
                args.splice($.inArray(removeItem, args), 1); //Look up at the ID and remove it
            } else {
                $(selector).addClass('active');
                args.push($itemdata);
            }
            $menucontainer.siblings('select').val(args);
        }

        if (!$menucontainer.hasClass('Multi')) {
            $menucontainer.siblings('select').val($itemdata); //bind form value
            $(selector).siblings('a.' + itemClass).removeClass('active'); //remove all active states
            $(selector).addClass('active'); //add new active state to clicked item
        }

    };

    var init_msc = function (openmenu, opentext, container, multi, append) {
        var selector = append.selector;
        var toggle = document.createElement('a');
        var mcscontainer = document.createElement('div');
        $(toggle).addClass(openmenu).addClass('mcs').html(opentext).appendTo(append);
        if (multi === true) {
            $(mcscontainer).addClass('Multi');
        }
        if (selector == '#selectcontrolprice') {
            //
            var i = 0;
            $(mcscontainer).append('<div id="html5" class="noUi-target noUi-ltr noUi-horizontal noUi-background" onchange=" Djinaro.filterByGoods()"></div>');
            $(mcscontainer).append('<div class="row" style="padding:20px;"><input class="form-control" type="number" min="0" max="10000" step="500" id="input-number1"><span style="line-height: 35px; font-weight: bold; color: #000; width: 10%">—</span><input class="form-control" type="number" min="0" max="10000" step="500" id="input-number2"></div>');
        }
        $(mcscontainer).addClass(container).appendTo(append);
        if (selector == '#selectcontrolprice') {
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
        };
    };

    var generateitems = function (selector, useOptionText, idprefix, itemClass, containerClass) {
        var itemtemplate;
        var $optioncount;
        var idtemplate = "";
        var $menucontainer = $(selector).parent();
        $optioncount += 1;
        var settext = '';
        if (useOptionText === true) {
            settext = $(selector).text();
        }

        if (idprefix !== null) {
            idtemplate = "' id='" + idprefix + $optioncount;
        }
        itemtemplate = "<a class='" + itemClass + "' data='" + $(selector).attr('value') + idtemplate + "'>"+ settext + "</a>";
        $menucontainer.siblings('.' + containerClass).append(itemtemplate);
    };

    var destroymsc = function (selector) {
        var $mcs = selector.find('select');
        $mcs.show();            // Shows the Select control if it was hidden;
        if ($mcs.next().hasClass('mcs'))
        {
            $mcs.next().remove();   // Remove the generated open/close toggle
            $mcs.next().remove();   // Remove the generated items
        }
    };

    $.fn.MultiColumnSelect = function (options) {
        var args = [];
        var selected = [];
        var $optioncount;

        $optioncount = 0;

        var settings = $.extend({
            multiple: false, // Single or Multiple Select- Default Single
            useOptionText: true, // Use text from option. Default true, use false if you plan to use images
            hideselect: true, // Hide Original Select Control
            openmenuClass: 'mcs-open', // Toggle Open Button Class
            openmenuText: 'Choose An Option', // Text for button
            openclass: 'open', // Class added to Toggle button on open
            containerClass: 'mcs-container', // Class of parent container
            itemClass: 'mcs-item', // Class of menu items
            idprefix: null, // Assign as ID to items eg 'item-' = #item-1, #item-2, #item-3...
            duration: 200, //Toggle Height duration
            onOpen: null,
            onClose: null,
            onItemSelect: null
        }, options);

        this.find('select').val(0);

        if (settings.hideselect === true)
        {
            this.find('select').hide();
        } else {
            this.find('select').show();
        }

        init_msc(settings.openmenuClass, settings.openmenuText, settings.containerClass, settings.multiple, this); //create the wrapper

        this.find('select option').each(function () //get elements in dropdown
        {

            if ($(this).attr('selected')){

                generateitems(this, settings.useOptionText, settings.idprefix, [settings.itemClass + ' selected'], settings.containerClass);


                var $select = $('.selected').parent().prev().prev();
                if ($select.val() !== null) {
                    args.push($select.val());
                }
                itemclick($('.selected'), settings.itemClass, args);
                $('.selected').toggleClass('selected');



            }else{
                generateitems(this, settings.useOptionText, settings.idprefix, settings.itemClass, settings.containerClass);

            }
        });

        this.on('click', '.' + settings.itemClass, function (e)    //on menu item click
        {
            var $select = $(this).parent().prev().prev();
            if ($select.val() !== null) {
                args.push($select.val());
            }
            itemclick(this, settings.itemClass, args);
            if ($.isFunction(settings.onItemSelect)) {
                settings.onItemSelect.call(this);// Select item :: callback
            }
            e.preventDefault();
        });

        this.find('.' + settings.openmenuClass).on('click', function (e)
        {
            var $menucontainer = $(this).next();
            if ($(this).hasClass(settings.openclass)) {
                $(this).removeClass(settings.openclass);
                $menucontainer.slideToggle("slow", function () {
                    // Close Animation complete :: callback
                    if ($.isFunction(settings.onClose)) {
                        settings.onClose.call(this);
                    }
                });
            } else {
                $(this).addClass(settings.openclass);
                //Set the height of the container
                $menucontainer.slideToggle("slow", function () {
                    // Open Animation complete :: callback
                    if ($.isFunction(settings.onOpen)) {
                        settings.onOpen.call(this);
                    }
                });
            }
            e.preventDefault();
        });
        return this;
    };

    //public functions
    $.fn.MultiColumnSelectDestroy = function () {
        destroymsc(this);
    };

    $.fn.MultiColumnSelectAddItem = function (itemvalue, itemtext, idprefix) {

        var $mcs = this.find('select');
        var $count = this.find('select options').size();
        $mcs.append($('<option/>', {value: itemvalue, text: itemtext}));
        var $toggle = $mcs.next();
        if ($toggle.hasClass('mcs')) {
            //Found init plugin
            var $container = $toggle.next();
            var $menuitem = $container.children();
            var $menuitemClass = $menuitem.attr('class');
            $menuitemClass = $menuitemClass.substring(0, $menuitemClass.indexOf(' '));

            var idtemplate = "";
            if (typeof (idprefix) !== 'undefined') {
                idtemplate = "' id='" + idprefix + $count;
            }
            var $newitem = "<a class='" + $menuitemClass + " additem' data='" + itemvalue + idtemplate + "'>" + itemtext + "</a>";
            $container.append($newitem);
        }
    };
}(jQuery));