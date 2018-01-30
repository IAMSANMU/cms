$(function() {
    $("#saveForm").validate({
        rules: {
            Name: {
                required: true
            }
        }
    });

    $("#btnSave").bindSubmit();
});