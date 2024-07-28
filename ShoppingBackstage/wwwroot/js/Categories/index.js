$(document).ready(function () {
    initializeSortable();
    LoadPhotoSwipe();
});

// 刷新列表
function loadList(){
    loadleftList();
    loadRightList();
}

// 左列表
function loadleftList() {
    $.ajax({
        url: '/Categories/CategoriesManagement/LeftList',
        type: "POST",
        success: function (response) {
            $('#leftList').html(response);
            initializeSortable();
            LoadPhotoSwipe();
        },
        error: function () {
            console.log("Error");
        }
    });
}

// 右列表
function loadRightList() {
    $.ajax({
        url: '/Categories/CategoriesManagement/RightList',
        type: "POST",
        success: function (response) {
            $('#rightList').html(response);
            initializeSortable();
            LoadPhotoSwipe();
        },
        error: function () {
            console.log("Error");
        }
    });
}

// 載入FilePond套件
function LoadFilePond() {
    const layoutFileObject = document.getElementById('LayoutFileObject');
    const bannerFileObject = document.getElementById('BannerFileObject');
    initFilePond(layoutFileObject, bannerFileObject);
}

// 載入排序套件
function initializeSortable() {
    var leftEl = document.getElementById('leftSortable');
    var rightEl = document.getElementById('rightSortable');
    function sendOrder(type, order) {
        $.ajax({
            type: "POST",
            url: '/Categories/CategoriesManagement/UpdateOrder',
            data: JSON.stringify({ type: type, order: order }),
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    loadList();
                    alert('排序更新成功');
                } else {
                    alert('排序更新失敗');
                }
            }
        });
    }

    if (leftEl) {
        Sortable.create(leftEl, {
            handle: '.moveLayout',
            animation: 150,
            onEnd: function (evt) {
                var order = this.toArray();
                sendOrder('left', order);
            }
        });
    }

    if (rightEl) {
        Sortable.create(rightEl, {
            handle: '.moveBanner',
            animation: 150,
            onEnd: function (evt) {
                var order = this.toArray();
                sendOrder('right', order);
            }
        });
    }
}

// 新增
$(document).on('click', '.addCategories', function () {
    $.ajax({
        type: "POST",
        url: '/Categories/CategoriesManagement/Input',
    }).done(function (response) {
        $(".modal-title").html("新增商品類別");
        $("#categoriesModalBody").html(response);
        $("#categoriesModal").modal({ backdrop: "static" }).modal('show');
        LoadFilePond();
    });
})

// 編輯
$(document).on('click', '.editCategories', function () {
    const id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Categories/CategoriesManagement/Input',
        data: { id: id }
    }).done(function (response) {
        $(".modal-title").html("編輯商品類別");
        $("#categoriesModalBody").html(response);
        $("#categoriesModal").modal({ backdrop: "static" }).modal('show');
        LoadFilePond();
    });
})

// 儲存
$(document).on('submit', '#Save', function (e) {
    e.preventDefault(); // 阻止表單直接提交
    var formData = new FormData(this);

    $.ajax({
        type: "POST",
        url: '/Categories/CategoriesManagement/Save',
        data: formData,
        contentType: false,
        processData: false
        
    }).done(function (response) {
        if (response.success) {
            $("#categoriesModal").modal('hide');
            loadList();
        } else {
            $("#categoriesModalBody").html(response);
            LoadFilePond();
        }
    });
});

// 刪除帳號
$(document).on('click', '.deleteCategories', function () {
    const id = $(this).data('id');
    var userConfirmed = confirm("確定要刪除這個商品類別嗎?");
    if (userConfirmed) {
        $.ajax({
            type: "POST",
            url: '/Categories/CategoriesManagement/Delete',
            data: { id: id }
        }).done(function (response) {
            loadList();
        });
    }
})

$(document).on('click', '.closeModal', function () {
    $(this).closest('.modal').modal('hide');
});