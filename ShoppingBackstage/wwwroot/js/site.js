// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    function bindCaptchaRefresh() {
        $(".jq_captcha").off("click").on("click", function () {
            var my = $(this);
            var url = $('#ResetCaptchaUrl').val();
            console.log(url);
            $.ajax({
                url: url,
                contentType: "application/json",
                dataType: "html",
                success: function (response) {
                    if (response && response.indexOf("jq_captcha") >= 0) {
                        my.closest("div").html(response);
                        bindCaptchaRefresh(); // 重新綁定事件
                    }
                }
            });
        });
    }

    bindCaptchaRefresh(); // 初次綁定事件
});