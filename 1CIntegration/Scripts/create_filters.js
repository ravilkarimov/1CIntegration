jQuery(function ($) {
        $('#selectcontrolprice').MultiColumnSelect({
            multiple: true, // Single or Multiple Select- Default Single
            useOptionText: true, // Use text from option. Use false if you plan to use images
            hideselect: true, // Hide Original Select Control
            openmenuClass: 'mcs-open', // Toggle Open Button Class
            openmenuText: '<i class="angle left icon" style="float:left;"></i>ЦЕНА', // Text for button
            openclass: 'open', // Class added to Toggle button on open
            containerClass: 'mcs-container', // Class of parent container
            itemClass: 'mcs-item', // Class of menu items
            idprefix: null, // Assign as ID to items eg 'item-' = #item-1, #item-2, #item-3...
            duration: 200, //Toggle Height duration
            onOpen: function () { },
            onClose: function () { },
            onItemSelect: function () { }
        });
});

jQuery(function ($) {
        $('#selectcontrolseason').MultiColumnSelect({
            multiple: true,              // Single or Multiple Select- Default Single
            useOptionText: true,               // Use text from option. Use false if you plan to use images
            hideselect: true,               // Hide Original Select Control
            openmenuClass: 'mcs-open',         // Toggle Open Button Class
            openmenuText: '<i class="angle left icon" style="float:left;"></i>СЕЗОН', // Text for button
            openclass: 'open',             // Class added to Toggle button on open
            containerClass: 'mcs-container',    // Class of parent container
            itemClass: 'mcs-item',         // Class of menu items
            idprefix: null,                        // Assign as ID to items eg 'item-' = #item-1, #item-2, #item-3...
            duration: 200,                         //Toggle Height duration
            onOpen: function () { },
            onClose: function () { },
            onItemSelect: function () { Djinaro.filterByGoods(); }
        });
});