$(function() {
    $("#saveForm").validate({
        rules: {
            Link: {
                required: true
            }
        }
    });
    $("#btnSave").bindSubmit();
    $(".fileinput-button").imgUpload("/Admin/Upload");
});