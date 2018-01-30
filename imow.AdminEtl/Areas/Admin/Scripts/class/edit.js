$(function() {
    $("#saveForm").validate({
        rules: {
            Name: {
                required: true
            },
            Type: {
                required: true
            }
        }
    });

    $("#btnSave").bindSubmit({
        before: function() {
            var html = $('.summernote').summernote("code");
            html = $.base64.encode(html, "UTF-8");
            $("#info").val(html);
            return true;
        }
    });
    $(".i-checks").ickbox();
    $('.summernote').Textarea({ type: "class" });
    $(".fileinput-button").imgUpload("/Admin/Upload");
});