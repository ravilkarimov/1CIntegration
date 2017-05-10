jQuery(document).ready(function () {
    jQuery("[data-toggle]").click(function () {
        var sidebar = jQuery("#sidebar");
        if (sidebar.hasClass('open-sidebar')) {
            sidebar.removeClass("open-sidebar");
        } else sidebar.addClass("open-sidebar");
    });

});
jQuery(".swipe-area").swipe({
    swipeStatus: function (event, phase, direction, distance, duration, fingers) {
        if (phase == "move" && direction == "right") {
            jQuery(".sidebar").addClass("open-sidebar");
            return false;
        }
        if (phase == "move" && direction == "left") {
            jQuery(".sidebar").removeClass("open-sidebar");
            return false;
        }
    }
});