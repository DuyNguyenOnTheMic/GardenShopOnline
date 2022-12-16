// Example starter JavaScript for disabling form submissions if there are invalid fields
(function () {
    'use strict';
    window.addEventListener('load', function () {
        // Fetch all the forms we want to apply custom Bootstrap validation styles to
        var forms = document.getElementsByClassName('needs-validation');
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                }
                form.classList.add('was-validated');
            }, false);
        });

    }, false);
})();
$(document).ready(function () {
    var forms = document.getElementsByClassName('needs-validation');
    var validation = Array.prototype.filter.call(forms, function (form) {
    $('#submit_edit').on('click', function () {
        if (form.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
        } else {
            Update();
        }
        form.classList.add('was-validated');
    })
        $('#submit_edit_product').on('click', function () {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            } else {
                Update_Product();
            }
            form.classList.add('was-validated');
        })
       
    }, false);
})

//-------------------------Delete Category, Product-----------------------
var URLDelete = "";
$('#URLDelete')
    .keypress(function () {
        URLDelete = $(this).val();
    })
    .keypress();

function Contains(text_one, text_two) {
    if (text_one.indexOf(text_two) != -1)
        return true;
}
function deleteAlert(id, code) {
    swal({
        title: "Bạn chắn chắn muốn xóa ?",
        type: "warning",
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonText: "Xác nhận",
        confirmButtonColor: "#ec6c62",
    },
        function () {
            var categorys = {};
            categorys.ID = id;
            
            $.ajax({
                url: URLDelete,
                data: JSON.stringify(categorys),
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json'
            })
                .done(function (data) {
                    sweetAlert
                        ({
                            title: "Đã xóa!",
                            type: "success"
                        },
                            function () {
                                $("table tbody").find('input[name="record"]').each(function () {
                                    if (Contains($(this).val(), code)) {
                                        $(this).parents("tr").remove();
                                    }
                                });
                            })
                })
            /*   .error(function (data) {
                   swal("OOps", "Chúng tôi không thể kết nối đến server!", "error");
               })*/
        })
}

//-----------------------Edit status------------------------------------------
var URDEditStatus = "";
$('#URDEditStatus')
    .keypress(function () {
        URDEditStatus = $(this).val();
    })
    .keypress();
function EditStatus(id) {
    sweetAlert
        ({
            title: "Cập nhật trạng thái thành công!",
            type: "success"
        },
            function () {
                var categorys = {};
                categorys.id = id;
                $.ajax({
                    url: URDEditStatus,
                    data: JSON.stringify(categorys),
                    type: "POST",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json'
                })
            });

}

//-----------------------Định dạng giá-------------------------------------
$('#product-price').keydown(function (e) {
    setTimeout(() => {
        let parts = $(this).val().split(".");
        let v = parts[0].replace(/\D/g, ""),
            dec = parts[1]
        let calc_num = Number((dec !== undefined ? v + "." + dec : v));
        // use this for numeric calculations
        // console.log('number for calculations: ', calc_num);
        let n = new Intl.NumberFormat('en-EN').format(v);
        n = dec !== undefined ? n + "." + dec : n;
        $(this).val(n);
    })
})

$('#edit_product_price').keydown(function (e) {
    setTimeout(() => {
        let parts = $(this).val().split(".");
        let v = parts[0].replace(/\D/g, ""),
            dec = parts[1]
        let calc_num = Number((dec !== undefined ? v + "." + dec : v));
        // use this for numeric calculations
        // console.log('number for calculations: ', calc_num);
        let n = new Intl.NumberFormat('en-EN').format(v);
        n = dec !== undefined ? n + "." + dec : n;
        $(this).val(n);
    })
})




//-------------------------Get Category -------------------------------

$('#URLFindCategory')
    .keypress(function () {
        URLFindCategory = $(this).val();
    })
    .keypress();
function GetCategory(ele, id) {
    row = $(ele).closest('tr');
    $.ajax({
        type: 'POST',
        url: URLFindCategory,
        data: { "category_id": id },
        success: function (response) {
            $('#edit_id').val(response.ID);
            $('#Edit_name').val(response.Name);

            $('#Edit_Modal .close').css('display', 'none');
            $('#Edit_Modal').modal('show');
        }
    })
}

//------------------------UPDATE CATEGORY-----------------------------
$('#URLUpdateCategory')
    .keypress(function () {
        URLUpdateCategory = $(this).val();
    })
    .keypress();

function Update() {
    var table = $('#example').DataTable();
    var group = {};
    group.id = $('#edit_id').val();
    group.name = $('#Edit_name').val();

    $.ajax({
        url: URLUpdateCategory,
        type: "Post",
        data: JSON.stringify(group),
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (response) {
            $('#Edit_Modal .close').css('display', 'none');
            $('#Edit_Modal').modal('hide');
            table.cell(row, 1).data($('#Edit_name').val());
            table.draw();
            sweetAlert
                ({
                    title: "Cập nhật thành công !",
                    type: "success"
                })
        }
    });
}
//------------ Load dropdown form add product----------------------------------

var URLgetCategory = "";
$('#URLgetCategory')
    .keypress(function () {
        URLgetCategory = $(this).val();
    })
    .keypress();
$.ajax({
    type: "GET",
    url: URLgetCategory,
    data: "{}",
    success: function (data) {
        var s = '<option value="" disabled="disabled" selected="selected">Select product category</option>';
        for (var i = 0; i < data.length; i++) {
            s += '<option value="' + data[i].categoryID + '">' + data[i].categoryName + '</option>';
        }
        $("#CategoryDropdown").html(s);
    }
});
$.ajax({
    type: "GET",
    url: URLgetCategory,
    data: "{}",
    success: function (data) {
        var s = '<option value="-1" >Select product category</option>';
        for (var i = 0; i < data.length; i++) {
            s += '<option value="' + data[i].categoryID + '">' + data[i].categoryName + '</option>';
        }
        $("#filter_Category").html(s);
    }
});

//----------------------FILTER PRODUCT------------------------------------------------
$('#URLProductList')
    .keypress(function () {
        URLProductList = $(this).val();
    })
    .keypress();


$("#filter_Category").change(function () {
    var group_id = $("#filter_GroupProduct").val();
    var category_id = $("#filter_Category").val();
    GetList(group_id, category_id)
});

function GetList(group_id, category_id) {
    $.ajax({
        url: URLProductList,
        data: {
            category_id: category_id,
        }
    }).done(function (result) {
        $('#dataContainer').html(result);
        $('#example').DataTable()

    }).fail(function (XMLHttpRequest, textStatus, errorThrown) {
        console.log(textStatus)
        console.log(errorThrown)
        alert("Something Went Wrong, Try Later");
    });
}
//----------------------LOAD FORM EDIT PRODUCT---------------------------------------

$('#URLFindProduct')
    .keypress(function () {
        URLFindProduct = $(this).val();
    })
    .keypress();

function GetProduct(ele, id) {
    row = $(ele).closest('tr');
    $.ajax({
        type: 'POST',
        url: URLFindProduct,
        data: { "Product_id": id },
        success: function (response) {
            $('#edit_id').val(response.ID);
            $('#edit_name').val(response.Name);
            $('#edit_quantity').val(response.Quantity);
            $('#edit_product_price').val(response.Price);
            $('textarea#edit_description').html(response.Description);
            document.images['edit_output'].src = "/assets/images/" + response.Image;
            var category_id = response.CategoryID;
           
            $.ajax({
                type: "GET",
                url: URLgetCategory,
                data: "{}",
                success: function (data) {
                    var s = '<option value="" disabled="disabled">Chọn danh mục</option>';
                    for (var i = 0; i < data.length; i++) {
                        if (category_id == data[i].categoryID) {
                            s += '<option value="' + data[i].categoryID + '"  selected="selected">' + data[i].categoryName + '</option>';
                        } else {
                            s += '<option value="' + data[i].categoryID + '" >' + data[i].categoryName + '</option>';

                        }
                    }
                    $("#edit_Category").html(s);
                }
            });
            $('#EditProduct .close').css('display', 'none');
            $('#EditProduct').modal('show');
        }
    })
}
//-------------------------------UPDATE PRODUCT--------------------------------
/*
$('#URLUpdateProduct')
    .keypress(function () {
        URLUpdateProduct = $(this).val();
    })
    .keypress();

function Update_Product() {
    var table = $('#example').DataTable();
    var product = {};
    product.ID = $('#edit_id').val();
    product.name = $('#edit_name').val();
    product.Quantity = $('#edit_quantity').val();
    product.Description = $('#edit_description').val();
    product.Price = Number($('#edit_product_price').val().replace(/,/g, ''));
    product.CategoryID = $('#edit_Category').val();

    var file = $('#edit_file').val();
    console.log(file);
    $.ajax({
        url: URLUpdateProduct,
        type: "Post",
        data: {
            Products : JSON.stringify(product),
            file : file,
        },
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        success: function (response) {
            var group_id = $("#filter_GroupProduct").val();
            var category_id = $("#filter_Category").val();
            $.ajax({
                url: URLProductList,
                data: {
                    group_id: group_id,
                    category_id: category_id,
                }
            }).done(function (result) {
                $('#dataContainer').html(result);
                $('#example').DataTable()
                $('#EditProduct .close').css('display', 'none');
                $('#EditProduct').modal('hide');

                sweetAlert
                    ({
                        title: "Cập nhật thành công !",
                        type: "success"
                    })
            }).fail(function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus)
                console.log(errorThrown)
                alert("Something Went Wrong, Try Later");
            });
        }
    });
}*/
CKEDITOR.replace("description", {
    extraPlugins: 'easyimage',
    cloudServices_tokenUrl: 'https://example.com/cs-token-endpoint',
    cloudServices_uploadUrl: 'https://your-organization-id.cke-cs.com/easyimage/upload/'
});  
CKEDITOR.replace("edit_description", {
    extraPlugins: 'easyimage',
    cloudServices_tokenUrl: 'https://example.com/cs-token-endpoint',
    cloudServices_uploadUrl: 'https://your-organization-id.cke-cs.com/easyimage/upload/'
});  
