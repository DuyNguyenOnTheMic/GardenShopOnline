var url = '/ShoppingCart/GetMiniCart'
    miniCart = $('#miniCart');

$(function () {
    getMiniCart();
});

function getMiniCart() {
    // Get Partial View cart data
    $.get(url, function (data) {
        miniCart.html(data);
    });
}