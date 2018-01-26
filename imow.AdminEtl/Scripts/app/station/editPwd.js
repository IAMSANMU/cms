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

    var old = $("[name='OldPwd']");
    if (old.length > 0) {
        $("#btnSave").bindSubmit({
            success: function (result) {
                $.alert("修改密码成功,需重新登录",
                    function () {
                        $.closeWindow(function () {
                            location.href = "/Login/Logout";
                        });
                });
            }
        });
    } else {
        $("#btnSave").bindSubmit();
    }

});