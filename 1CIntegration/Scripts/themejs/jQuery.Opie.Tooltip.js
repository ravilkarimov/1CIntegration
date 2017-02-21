(function($) {
	"use strict";
	var _OpieToolTipCounter = 0;
	$.fn.OpieTooltip = function(options, args) {
		var defaults = 
		{
			position: "bc:tc",
			event: "mouseover",
			eventout: "mouseout",
			fade: 50,
			fadeout: 200
		};
		if (options === "defaults") 
		{
			return defaults;
		}
		var Opt = new Opts($.extend(defaults, options), $);
		
		var PG;
		this.each(function() {
			PG = new $.OpieTooltip(Opt, $(this), arguments);
			if (options === "initCSS3") 
			{
				IS.initCSS3.apply(PG, args);
			}
			else 
			{
				PG.init();
			}
		});
		
		return this;
	}
	
	$.OpieTooltip = function(Opt, $th, allArgs) {
		var me = this;
		
		/**
		 * Initialize tooltip
		 */
		this.init = function() {
			var eventOpt = me.getOpt($th, "event");
			var eventIn = (eventOpt) ? eventOpt : "mouseover";
			
			var $tooltip = me.draw($th);
			$th.on(eventIn, function(e) {
				var $target = $(this);
				var $tooltip = me.draw($target);
				if (e.type == "click") 
				{
					$tooltip.find(".o-tooltip-close").show();
				}
				else
				{
					$tooltip.find(".o-tooltip-close").hide();
				}
				if (!$target.getData("title") && !$target.attr("title") && !me.getOpt($target, "template") && !$target.attr("alt") && $target.attr('href') == '#') 
				{
					return;
				}
				var fadeOpt = me.getOpt($target, "fade");
				$tooltip.stop().fadeTo(fadeOpt, 1);
				if (!$target.getData("title")) 
				{
					$target.setData('title', $target.attr("title")).attr("title", "");
					$target.setData('alt', $target.attr("alt")).attr("alt", "");
				}
				
				me.setContent($target, $tooltip);
				
				
				var aSP = me.getOpt($target, "position").split(":");
				var tooltipPos = aSP[0];
				var arrowPos = aSP[1];
				me.updatePosition($tooltip, $target, tooltipPos, arrowPos);
				
			});
			var $close = $tooltip.find(".o-tooltip-close");
			if (eventIn == "click") 
			{
				var eventOut = "void";
				$close.show();
			}
			else 
			{
				var eventOpt = me.getOpt($th, "eventout");
				var eventOut = (eventOpt) ? eventOpt : "mouseout";
				$close.hide();
			}
			
			if (eventOut != "void") 
			{
				$th.on(eventOut, function(e) {
					if (e.type != "click") 
					{
						$tooltip.find(".o-tooltip-close").hide();
					}
					me.close($(this));
				});
			}
		}
		
		this.close = function($target) {
			var $tooltip = me.draw($target);
			var fadeOutOpt = me.getOpt($target, "fadeout");
			var fadeOut = (fadeOutOpt) ? parseFloat(fadeOutOpt) : null;
			$tooltip.stop().fadeTo(fadeOut, 0, function() {
				$tooltip.remove()
			});
		}
		
		/**
		 * Draw tooltip html
		 * @param {Object} $target - target jquery element
		 */
		this.draw = function($target) {
			var ID = $target.getData("opie-tool-tip-id");
			if (ID) 
			{
				var $tooltip = $("#" + ID);
				if ($tooltip.size() > 0) 
				{
					return $tooltip;
				}
			}
			if ($target.getData("tooltip-id")) 
			{
				var tooltipID = $target.getData("tooltip-id");
			}
			else 
			{
				var tooltipID = "opieTooltip-" + _OpieToolTipCounter;
				_OpieToolTipCounter++;
			}
			var $tooltip = $('<div class="o-tooltip" id="' + tooltipID + '"><div class="o-tooltip-arrow"></div><div class="o-tooltip-close"><span>x</span></div><div class="o-tooltip-inner"></div></div>');
			$tooltip.find(".o-tooltip-close").on("click", function() {
				me.close($(this).parents(".o-tooltip").getData("target"));
			});
			$tooltip.setData("target", $target);
			$target.setData("opie-tool-tip-id", tooltipID);
			$tooltip.setEvent("updatePosition", function(e, $target, tooltipPos, arrowPos, voidSecondRecCall) {
				me.updatePosition($(this), $target, tooltipPos, arrowPos, voidSecondRecCall);
			})
			$('body').append($tooltip);
			
			$tooltip.hide()
			return $tooltip;
		}
		
		this.getOpt = function($target, optName, getAsString) {
			if (Is.defined($target.getAttr(optName))) 
			{
				return $target.getAttr(optName);
			}
			return (getAsString) ? Opt.string(optName) : Opt.get(optName);
		}
		
		this.setContent = function($target, $tooltip) {
			var $inner = $tooltip.find(".o-tooltip-inner");
			//$inner.width("auto");
			//$inner.height("auto");
			var template = me.getOpt($target, "template", true);
			if (template) 
			{
				if (!$tooltip.getData("template")) 
				{
					if (!Is.cssSelector(template)) 
					{
						template = "<div>" + template + "</div>";
					}
					var $template = $(template);
					$tooltip.setData("template", $template.html());
				}
				var appendHTML = $tooltip.getData("template");
				if (!Is.defined($tooltip.getData("template"))) 
				{
					console.log("Template not found");
					return false;
				}
				$.each($tooltip.getData("template").match(/\{.*?\}/img), function(i, match) {
					var attr = match.replace("{", "").replace("}", "");
					var c = $target.attr(attr);
					if (attr == "title") 
					{
						c = $target.getData("title");
					}
					appendHTML = appendHTML.replace(match, c);
				})
				$tooltip.find(".o-tooltip-inner").html(appendHTML);
			}
			else 
			{
				$tooltip.find(".o-tooltip-inner").html($target.getData("title"));
			}
			//$inner.width($inner.width());
			//$inner.height($inner.height());
		}
		
		this.updatePosition = function($tooltip, $target, tooltipPos, arrowPos, voidSecondRecCall) {
			if (!$target) 
			{
				$target = $tooltip.getData("target");
				var aSP = me.getOpt($target, "position").split(":");
				var tooltipPos = aSP[0];
				var arrowPos = aSP[1];
			}
			var targetTop = $target.offset().top;
			var targetLeft = $target.offset().left;
			var targetHeight = $target.outerHeight();
			var targetWidth = $target.outerWidth();
			
			var tooltipTop = $tooltip.offset().top;
			var tooltipLeft = $tooltip.offset().left;
			var tooltipHeight = $tooltip.outerHeight();
			var tooltipWidth = $tooltip.outerWidth();
			
			var Css = 
			{
				top: 0,
				left: 0
			}
			
			$tooltip.attr("class", "o-tooltip " + arrowPos);
			if (tooltipPos.charAt(0) == "t") 
			{
				Css.top = targetTop;
			}
			else if (tooltipPos.charAt(0) == "m") 
			{
				Css.top = targetTop + (targetHeight / 2);
			}
			else if (tooltipPos.charAt(0) == "b") 
			{
				Css.top = targetTop + targetHeight;
			}
			
			if (tooltipPos.charAt(1) == "l") 
			{
				Css.left = targetLeft;
			}
			else if (tooltipPos.charAt(1) == "c") 
			{
				Css.left = targetLeft + (targetWidth / 2);
			}
			else if (tooltipPos.charAt(1) == "r") 
			{
				Css.left = targetLeft + targetWidth;
			}
			
			if (arrowPos.charAt(0) == "m") 
			{
				Css.top -= (tooltipHeight / 2);
			}
			else if (arrowPos.charAt(0) == "b") 
			{
				Css.top -= tooltipHeight;
			}
			
			if (arrowPos.charAt(1) == "r") 
			{
				Css.left -= tooltipWidth;
			}
			else if (arrowPos.charAt(1) == "c") 
			{
				Css.left -= (tooltipWidth / 2);
			}
			$tooltip.css(Css);
			if (voidSecondRecCall !== true) 
			{
				var tooltipTop = $tooltip.offset().top;
				var tooltipLeft = $tooltip.offset().left;
				
				var scrollTop = $(window).scrollTop();
				var scrollLeft = $(window).scrollLeft();
				var tooltipBottom = tooltipTop + $tooltip.outerHeight() - scrollTop;
				var tooltipRight = tooltipLeft + $tooltip.outerWidth() - scrollLeft;
				var screenBottom = $(window).height();
				var screenRight = $(window).width();
				var oppositePositions = 
				{
					"b": "t",
					"t": "b",
					"l": "r",
					"r": "l"
				}
				if (tooltipTop < scrollTop) 
				{
					var cantbe = "b"
				}
				else if (tooltipBottom > screenBottom) 
				{
					var cantbe = "t"
				}
				if (tooltipTop < scrollTop || tooltipBottom > screenBottom) 
				{
					var newToolTipPos = tooltipPos;
					if (tooltipPos.charAt(0) != cantbe) 
					{
						oppositePositions.m = "b";
						if (tooltipBottom > screenBottom) 
						{
							oppositePositions.m = "t";
						}
						var newToolTipPos = oppositePositions[tooltipPos.charAt(0)] + tooltipPos.charAt(1);
					}
					
					var newArrowPos = arrowPos;
					if (arrowPos != "ml" && arrowPos != "mr") 
					{
						var newArrowPos = oppositePositions[arrowPos.charAt(0)] + arrowPos.charAt(1);
					}
					tooltipPos = newToolTipPos;
					arrowPos = newArrowPos;
				}
				
				if (tooltipLeft < 0) 
				{
					var cantbe = "r"
				}
				else if (tooltipRight > screenRight) 
				{
					var cantbe = "l"
				}
				if (tooltipLeft < 0 || tooltipRight > screenRight) 
				{
					var newToolTipPos = tooltipPos;
					if (tooltipPos.charAt(0) != cantbe) 
					{
						oppositePositions.m = "r";
						if (tooltipRight > screenRight) 
						{
							oppositePositions.m = "l";
						}
						var newToolTipPos = tooltipPos.charAt(0) + oppositePositions[tooltipPos.charAt(1)];
					}
					
					var newArrowPos = arrowPos;
					if (arrowPos != "tc" && arrowPos != "bc") 
					{
						var newArrowPos = arrowPos.charAt(0) + oppositePositions[arrowPos.charAt(1)];
					}
				}
				if (tooltipTop < scrollTop || tooltipBottom > screenBottom || tooltipLeft < 0 || tooltipRight > screenRight) 
				{
					me.updatePosition($tooltip, $target, newToolTipPos, newArrowPos, true);
				}
			}
		}
		
	}
})(jQuery);
