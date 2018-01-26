$(function () {
    $('.i-checks').ickbox();
    $("#saveForm").validate({
        rules: {
            RealName: {
                required: true,
                maxlength: 32
            },
            Mobile: {
                maxlength: 11
            },
            UserName: {
                required: true,
                rangelength: [4, 32]
            },
            Pwd:{
                required: true,
                rangelength: [6, 64]
            },
            PwdSure: {
                required: true,
                rangelength: [6, 64],
                equalTo:"#Pwd"
            }

        }
    });


    $.ajaxForm('#btnSave', $("#saveForm"), {
        data: function(orignData) {
            var ckbox = $("#roles").find(":checked");
            var ids = [];
            ckbox.each(function() {
                ids.push($(this).val());
            });
            var result = orignData + "&Roles=" + ids.join(",");
            return result;
        },
        success: function(result) {
            $.alert("保存成功", function() {
                $.closeWindow(function () {
                    parent.location.reload();
                });
            });
        }
    });
});
