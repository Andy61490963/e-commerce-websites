$(document).ready(function () {
    initializeSortable();
});

// 篩選
function loadFunction() {
    $.ajax({
        url: '/About/Function',
        type: "POST",
        success: function (response) {
            $('#function').html(response);
            initializeSortable();
        },
        error: function () {
            console.log("Error");
        }
    });
}

// 列表
function loadList() {
    $.ajax({
        url: '/About/List',
        type: "POST",
        success: function (response) {
            $('#list').html(response);
            initializeSortable();
        },
        error: function () {
            console.log("Error");
        }
    });
}

// 載入排序套件
function initializeSortable() {
    var el = document.getElementById('sortable');
    if (el) {
        Sortable.create(el, {
            handle: '.moveAbout', // 只允許通過移動按鈕拖曳
            animation: 150,
            start: function(event, ui) {
                // 紀錄開始拖曳的索引位置
                initialIndex = ui.item.index();
                console.log('Start: ' + initialIndex);
            },
            onEnd: function (evt) {
                var order = this.toArray();
                // 發送新排序到後端
                $.ajax({
                    type: "POST",
                    url: '/About/UpdateOrder',
                    data: JSON.stringify(order),
                    contentType: "application/json",
                    dataType: "json",
                    success: function (response) {
                        if (response.success) {
                            loadList();
                            loadFunction();
                            alert('排序更新成功');
                        } else {
                            alert('排序更新失敗');
                        }
                    }
                });
            }
        });
    }
}

// 新增
$(document).on('click', '.addAbout', function () {
    $.ajax({
        type: "POST",
        url: '/About/Input',
    }).done(function (response) {
        $(".modal-title").html("新增");
        $("#aboutModalBody").html(response);
        $("#aboutModal").modal({ backdrop: "static" }).modal('show');
    });
})

// 編輯
$(document).on('click', '.editAbout', function () {
    const i0001_id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/About/Input',
        data: { id: i0001_id }
    }).done(function (response) {
        $(".modal-title").html("編輯");
        $("#aboutModalBody").html(response);
        $("#aboutModal").modal({ backdrop: "static" }).modal('show');
    });
})

// 提交表單
$(document).on('submit', '#Save', function (e) {
    e.preventDefault(); // 阻止表單直接提交
    var formData = new FormData(this);

    $.ajax({
        type: "POST",
        url: '/About/Save',
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            $("#aboutModal").modal('hide');
            loadList();
            loadFunction();
        } else {
            $("#aboutModalBody").html(response); // 沒成功就顯示錯誤訊息
        }
    });
});

// 提交搜尋表單
$(document).on('change, input', '#selected, #text', function () {
    var form = $('#Search')[0]; // 獲取原生的 DOM 表單元素
    var formData = new FormData(form); // 使用 FormData 來構建表單數據

    $.ajax({
        type: "POST",
        url: '/About/Search',
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        $('#list').html(response);
    }).fail(function (xhr, status, error) {
        console.error("Error: " + error);
    });
});

// 刪除
$(document).on('click', '.deleteAbout', function () {
    const a0001_id = $(this).data('id');
    var userConfirmed = confirm("確定要刪除這個About嗎?");
    if (userConfirmed) {
        $.ajax({
            type: "POST",
            url: '/About/Delete',
            data: { id: a0001_id }
        }).done(function (response) {
            loadList();
            loadFunction();
        });
    }
})

$(document).on('click', '.closeModal', function () {
    $(this).closest('.modal').modal('hide');
});