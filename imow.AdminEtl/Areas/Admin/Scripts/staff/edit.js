$(function () {
    $("#saveForm").validate({
        rules: {
        }
    });

    $(".i-radios").iradio();
    $(".i-checks").ickbox();

    $("#btnSave").bindSubmit();

    $(".fileinput-button").imgUpload("/Admin/Upload");
});