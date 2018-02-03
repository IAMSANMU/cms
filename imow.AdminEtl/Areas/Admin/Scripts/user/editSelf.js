$(function () {
    $('.i-radios').iradio();
    $("#saveForm").validate({
        rules: {
            "RealName": {
                required: true,
                maxlength: 32
            },
            "Mobile": {
                maxlength: 11
            }
        }
    });
    $("#btnSave").bindSubmit();

    $(".fileinput-card-button").imgUpload("/Admin/Upload");

});
