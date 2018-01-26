$(function () {
    $("#form").validate({
        rules: {
            password: {
                required: true
            },
            name: {
                required: true
            }
        }
    });
    $("#btn_login").click(function() {
        if ($("#form").valid()) {
            var data = $("#form").serialize();

            $.post("/Admin/Login/Login", data, function (result) {
                if (result.code === 0) {
                    if (result.data.code === 0) {
                        location.href = "/Admin/Home/Index";
                    } else if (result.data.code === 4) {
                        $.alert("账号不可用");
                    } else {
                        $.alert("账号密码不匹配,登录失败");
                    }
                } else {
                    $.alert("系统错误:"+result.data.message);
                }
            }, "json");
        }
    });
    if (parent.frames.length > 0) {
        parent.window.location.href = window.location.href;
    }
    document.onkeydown = function(e){ 
        var ev = document.all ? window.event : e;
        if(ev.keyCode===13) {
            $("#btn_login").click();
        }
    }
});
