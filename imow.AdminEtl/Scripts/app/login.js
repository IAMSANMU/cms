var os = function() {  
    var ua = navigator.userAgent,  
        isWindowsPhone = /(?:Windows Phone)/.test(ua),  
        isSymbian = /(?:SymbianOS)/.test(ua) || isWindowsPhone,   
        isAndroid = /(?:Android)/.test(ua),   
        isFireFox = /(?:Firefox)/.test(ua),   
        isChrome = /(?:Chrome|CriOS)/.test(ua),  
        isTablet = /(?:iPad|PlayBook)/.test(ua) || (isAndroid && !/(?:Mobile)/.test(ua)) || (isFireFox && /(?:Tablet)/.test(ua)),  
        isPhone = /(?:iPhone)/.test(ua) && !isTablet,  
        isPc = !isPhone && !isAndroid && !isSymbian;  
    return {  
        isTablet: isTablet,  
        isPhone: isPhone,  
        isAndroid : isAndroid,  
        isPc : isPc  
    };  
}




if (!os().isPc) {
    location.href = location.href + "/H5";
}

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

            $.post("/Login/Login", data, function (result) {
                if (result.code === 0) {
                    if (result.data.code === 0) {
                        location.href = "/Home/Index";
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
