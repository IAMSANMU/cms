$(function () {
    $("#saveForm").validate({
        rules: {
            Tel: {
                required: true
            },
            Number: {
                required: true
            }
        }
    });
    $("#btnSave").bindSubmit();
});
