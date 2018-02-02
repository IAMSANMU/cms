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

    $("#btnSave").bindSubmit();
    $(".i-checks").ickbox();
    $(".fileinput-button").imgUpload("/Admin/Upload");
});