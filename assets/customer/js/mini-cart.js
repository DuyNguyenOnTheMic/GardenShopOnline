﻿var url = '/ShoppingCart/GetMiniCart',
    miniCartList = $('#miniCartList');

$(function () {
    getMiniCart();
});

function getMiniCart() {
    // Get Partial View cart data
    $.get(url, function (data) {
        miniCartList.html(data);
    });

    $(document).off('click', '.product-item_remove').on('click', '.product-item_remove', function (e) {
        e.preventDefault();
        // Get the id from the link
        var recordToDelete = $(this).attr('data-id');
        if (recordToDelete != '') {
            // Perform the ajax post
            $.post('/ShoppingCart/RemoveProduct', { 'id': recordToDelete }, function (data) {
                $('#miniRow-' + data.DeleteId).fadeOut('slow');
                $('.minicart-item_total .ammount').text(data.CartTotal);
            });
        }
    });
}