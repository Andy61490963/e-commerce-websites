// 身份列表
function loadList() {
    $.ajax({
        url: '/Account/AccountManagement/List',
        type: "POST",
        success: function (response) {
            $('#list').html(response);
        },
        error: function () {
            console.log("Error");
        }
    });
}


// 新增帳號
$(document).on('click', '.addAccount', function () {
    $.ajax({
        type: "POST",
        url: '/Account/AccountManagement/Input',
    }).done(function (response) {
        $(".modal-title").html("新增帳號");
        $("#accountModalBody").html(response);
        $("#accountModal").modal({ backdrop: "static" }).modal('show');
    });
})

// 編輯帳號
$(document).on('click', '.editAccount', function () {
    const a0001_id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Account/AccountManagement/Input',
        data: { id: a0001_id }
    }).done(function (response) {
        $(".modal-title").html("編輯帳號");
        $("#accountModalBody").html(response);
        $("#accountModal").modal({ backdrop: "static" }).modal('show');
    });
})

// 新增、編輯事件
$(document).on('submit', '#Save', function (e) {
    e.preventDefault(); // 阻止表單直接提交
    var formData = new FormData(this);

    $.ajax({
        type: "POST",
        url: '/Account/AccountManagement/Save',
        data: formData,
        contentType: false,
        processData: false

        // 目前只能處裡單一錯誤訊息(帳號重複)
    }).done(function (response) {
        if (response.success) {
            $("#accountModal").modal('hide');
            loadList();
        } else {
            $("#accountModalBody").html(response); // 沒成功就顯示錯誤訊息
        }
    });
});

// 刪除帳號
$(document).on('click', '.deleteAccount', function () {
    const a0001_id = $(this).data('id');
    var userConfirmed = confirm("確定要刪除這個帳號嗎?");
    if (userConfirmed) {
        $.ajax({
            type: "POST",
            url: '/Account/AccountManagement/Delete',
            data: { id: a0001_id }
        }).done(function (response) {
            loadList();
        });
    }
})

$(document).on('click', '.closeModal', function () {
    $(this).closest('.modal').modal('hide');
});