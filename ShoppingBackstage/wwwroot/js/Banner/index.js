$(document).ready(function () {
    initializeSortable();
    LoadPhotoSwipe();
});

// 列表
function loadList() {
    $.ajax({
        url: '/Banner/List',
        type: "POST",
        success: function (response) {
            $('#list').html(response);
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
    const FileObject = document.getElementById('FileObject');
    console.log(FileObject)
    initFilePond(FileObject)
}

// 載入排序套件
function initializeSortable() {
    var el = document.getElementById('sortable');
    if (el) {
        Sortable.create(el, {
            handle: '.moveBanner', // 只允許通過移動按鈕拖曳
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
                    url: '/Banner/UpdateOrder',
                    data: JSON.stringify(order),
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
        });
    }
}

// 新增輪播圖
$(document).on('click', '.addBanner', function () {
    $.ajax({
        type: "POST",
        url: '/Banner/Input',
    }).done(function (response) {
        $(".modal-title").html("新增輪播圖");
        $("#bannerModalBody").html(response);
        $("#bannerModal").modal({ backdrop: "static" }).modal('show');

        LoadFilePond();
    });
})

// 編輯輪播圖
$(document).on('click', '.editBanner', function () {
    const i0001_id = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Banner/Input',
        data: { id: i0001_id }
    }).done(function (response) {
        $(".modal-title").html("編輯輪播圖");
        $("#bannerModalBody").html(response);
        $("#bannerModal").modal({ backdrop: "static" }).modal('show');
        LoadFilePond();
    });
})

// 提交表單
$(document).on('submit', '#Save', function (e) {
    e.preventDefault(); // 阻止表單直接提交
    var formData = new FormData(this);

    $.ajax({
        type: "POST",
        url: '/Banner/Save',
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        if (response.success) {
            $("#bannerModal").modal('hide');
            console.log("test")
            loadList();
        } else {
            $("#bannerModalBody").html(response); // 沒成功就顯示錯誤訊息
            console.log("test1")
            LoadFilePond();
        }
    });
});

// 提交搜尋表單
$(document).on('change, input', '#selected, #text', function () {
    var form = $('#Search')[0]; // 獲取原生的 DOM 表單元素
    var formData = new FormData(form); // 使用 FormData 來構建表單數據

    $.ajax({
        type: "POST",
        url: '/Banner/Search',
        data: formData,
        contentType: false,
        processData: false
    }).done(function (response) {
        $('#list').html(response);
    }).fail(function (xhr, status, error) {
        console.error("Error: " + error);
    });
});

$(document).on('click', '.previewBanner', function () {
    $.ajax({
        type: "POST",
        url: '/Banner/Preview',
    }).done(function (response) {
        loadList();
    });
})

// 刪除帳號
$(document).on('click', '.deleteBanner', function () {
    const a0001_id = $(this).data('id');
    var userConfirmed = confirm("確定要刪除這個Banner嗎?");
    if (userConfirmed) {
        $.ajax({
            type: "POST",
            url: '/Banner/Delete',
            data: { id: a0001_id }
        }).done(function (response) {
            loadList();
        });
    }
})

$(document).on('click', '.closeModal', function () {
    $(this).closest('.modal').modal('hide');
});