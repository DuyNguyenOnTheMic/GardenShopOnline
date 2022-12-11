var url = '/ShoppingCart/GetMiniCart',
    miniCartList = $('#miniCartList');

$(function () {
    getMiniCart();
});

function getMiniCart() {
    // Get Partial View cart data
    $.get(url, function (data) {
        miniCartList.html(data);
    });
}