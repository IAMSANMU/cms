$(function () {
    $("#saveForm").validate({
        rules: {
            Pwd: {
                required: true,
                rangelength: [6, 64]
            },
            PwdSure: {
                required: true,
                rangelength: [6, 64],
                equalTo: "#Pwd"
            }
        }
    });

    $.ajaxForm('#btnSave', $("#saveForm"), {
        success: function (result) {
            $.alert("保存成功", function () {
                $.closeWindow(function () {
                    parent.location.reload();
                });
            });
        }
    });
});