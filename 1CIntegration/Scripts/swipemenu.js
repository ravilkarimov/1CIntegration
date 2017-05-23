jQuery(document).ready(function () {
    jQuery("[data-toggle]").click(function () {
        var toggle_el = jQuery(this).data("toggle");
        jQuery(toggle_el).toggleClass("open-sidebar");
    });

});

jQuery(".swipe-area").swipe({
    swipeStatus: function (event, phase, direction, distance, duration, fingers) {
        if (phase == "move" && direction == "right") {
            jQuery(".container").addClass("open-sidebar");
            return false;
        }
        if (phase == "move" && direction == "left") {
            jQuery(".container").removeClass("open-sidebar");
            return false;
        }
    }
});