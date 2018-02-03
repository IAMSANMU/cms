
var $os = function () {
    var ua = navigator.userAgent,
        isWindowsPhone = /(?:Windows Phone)/.test(ua),
        isSymbian = /(?:SymbianOS)/.test(ua) || isWindowsPhone,
        isAndroid = /(?:Android)/.test(ua),
        isFireFox = /(?:Firefox)/.test(ua),
        isChrome = /(?:Chrome|CriOS)/.test(ua),
        isTablet = /(?:iPad|PlayBook)/.test(ua) || (isAndroid && !/(?:Mobile)/.test(ua)) || (isFireFox && /(?:Tablet)/.test(ua)),
        isPhone = /(?:iPhone)/.test(ua) && !isTablet,
        isWx = /(?:MicroMessenger)/.test(ua),
        isPc = !isPhone && !isAndroid && !isSymbian;
    return {
        isTablet: isTablet,
        isPhone: isPhone,
        isAndroid: isAndroid,
        isPc: isPc,
        isWx: isWx
    };
}
$(function () {
    function initSource() {
        var source = "";
        var os = $os();
        if (os.isPc) {
            source = "电脑";
        } else if (os.isWx) {
            source = "微信";
        } else if (os.isWx) {
            source = "手机";
        } else {
            source = navigator.userAgent;
        }
        $(".order_source").val(source);
    }
    initSource();
    //判断浏览器类型
    $(".orderForm").each(function () {
        $(this).validate({
            rules: {
                Name: {
                    required: true
                },
                Tel: {
                    required: true
                }
            }
        });
    });
    $(".orderForm .btnOrder").click(function () {
        var form = $(this).parents("form:first");
        var pass = form.valid();
        var data = form.serialize();
        if (pass) {
            var url = form.attr("action");
            $.post(url,
                data,
                function (result) {
                    if (result.Success) {
                        layer.alert("恭喜您预约成功<br/>将会有专人联系您");
                        form[0].reset();
                    } else {
                        layer.alert(result.Message);
                    }
                },
                "json");
        }
    });

    $('.spin-icon').click(function () {
        $(".theme-config-box").toggleClass("show");
    });

})