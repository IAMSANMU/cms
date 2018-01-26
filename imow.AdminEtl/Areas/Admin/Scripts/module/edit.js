$(function() {
    $("#saveForm").validate({
        rules: {
            Name: {
                required: true
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

    $('.i-checks').ickbox();
});