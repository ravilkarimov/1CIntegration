; (function ($) {
    "use strict"; var
    isIE = ($.support.optSelected === false ? true : false), viewports = {}, fn = {}; function processElements(ev) { var elements = viewports[$(this).prop("stickOnScroll")], i, j; for (i = 0, j = elements.length; i < j; i++) { (function (o) { var scrollTop, maxTop, cssPosition, footerTop, eleHeight, yAxis; o = elements[i]; if (o !== null) { if (o.ele[0].parentNode === null) { elements[i] = o = null; } } if (o !== null) { scrollTop = o.viewport.scrollTop(); maxTop = o.getEleMaxTop(); if (o.isWindow === false && isIE) { o.ele.stop(); } if (scrollTop > maxTop) { cssPosition = { position: "fixed", top: (o.topOffset - o.eleTopMargin) }; if (o.isWindow === false) { cssPosition = { position: "absolute", top: ((scrollTop + o.topOffset) - o.eleTopMargin) }; } o.isStick = true; if (o.footerElement.length) { footerTop = o.footerElement.position().top; eleHeight = o.ele.outerHeight(); yAxis = (cssPosition.top + eleHeight + o.bottomOffset + o.topOffset); if (o.isWindow === false) { yAxis = (eleHeight + o.bottomOffset + o.topOffset); } else { yAxis = (cssPosition.top + scrollTop + eleHeight + o.bottomOffset); footerTop = o.footerElement.offset().top; } if (yAxis > footerTop) { if (o.isWindow === true) { cssPosition.top = (footerTop - (scrollTop + eleHeight + o.bottomOffset)); } else { cssPosition.top = (scrollTop - (yAxis - footerTop)); } } } if (o.setParentOnStick === true) { o.eleParent.css("height", o.eleParent.height()); } if (o.setWidthOnStick === true) { o.ele.css("width", o.ele.css("width")); } if (isIE && o.isWindow === false) { o.ele.addClass(o.stickClass).css("position", cssPosition.position).animate({ top: cssPosition.top }, 150); } else { o.ele.css(cssPosition).addClass(o.stickClass); } if (o.wasStickCalled === false) { o.wasStickCalled = true; setTimeout(function () { if (o.isOnStickSet === true) { o.onStick.call(o.ele, o.ele); } o.ele.trigger("stickOnScroll:onStick", [o.ele]); }, 20); } } else if (scrollTop <= maxTop) { if (o.isStick) { o.ele.css({ position: "", top: "" }).removeClass(o.stickClass); o.isStick = false; if (o.setParentOnStick === true) { o.eleParent.css("height", ""); } if (o.setWidthOnStick === true) { o.ele.css("width", ""); } o.wasStickCalled = false; setTimeout(function () { if (o.isOnUnStickSet) { o.onUnStick.call(o.ele, o.ele); } o.ele.trigger("stickOnScroll:onUnStick", [o.ele]); }, 20); } } if (scrollTop === 0) { o.setEleTop(); } } })(elements[i]); } return this; } $.fn.stickOnScroll = function (options) { return this.each(function () { if ($(this).hasClass("hasStickOnScroll")) { return this; } var o = $.extend({}, { topOffset: 0, bottomOffset: 5, footerElement: null, viewport: window, stickClass: 'stickOnScroll-on', setParentOnStick: false, setWidthOnStick: false, onStick: null, onUnStick: null }, options), viewportKey, setIntID, setIntTries = 1800; o.isStick = false; o.ele = $(this).addClass("hasStickOnScroll"); o.eleParent = o.ele.parent(); o.viewport = $(o.viewport); o.eleTop = 0; o.eleTopMargin = parseFloat(o.ele.css("margin-top")); o.footerElement = $(o.footerElement); o.isWindow = true; o.isOnStickSet = $.isFunction(o.onStick); o.isOnUnStickSet = $.isFunction(o.onUnStick); o.wasStickCalled = false; o.setEleTop = function () { if (o.isStick === false) { if (o.isWindow) { o.eleTop = o.ele.offset().top; } else { o.eleTop = o.ele.position().top; } } }; o.getEleMaxTop = function () { var max = ((o.eleTop - o.topOffset)); if (!o.isWindow) { max = (max + o.eleTopMargin); } return max; }; if (o.setParentOnStick === true && o.eleParent.is("body")) { o.setParentOnStick = false; } if (!$.isWindow(o.viewport[0])) { o.isWindow = false; } function addThisEleToViewportList() { o.setEleTop(); viewportKey = o.viewport.prop("stickOnScroll"); if (!viewportKey) { viewportKey = "stickOnScroll" + String(Math.random()).replace(/\D/g, ""); o.viewport.prop("stickOnScroll", viewportKey); viewports[viewportKey] = []; o.viewport.on("scroll", processElements); } viewports[viewportKey].push(o); o.viewport.scroll(); } if (o.ele.is(":visible")) { addThisEleToViewportList(); } else { setIntID = setInterval(function () { if (o.ele.is(":visible") || !setIntTries) { clearInterval(setIntID); addThisEleToViewportList(); } --setIntTries; }, 100); } return this; }); };
})(jQuery);