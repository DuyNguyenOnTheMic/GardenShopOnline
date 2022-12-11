var url = '/ShoppingCart/GetMiniCart'
    miniCart = $('#miniCart');

$(function () {
    // Get Partial View cart data
    $.get(url, function (data) {
        miniCart.html(data);
    });
});