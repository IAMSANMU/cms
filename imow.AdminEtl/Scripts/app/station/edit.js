$(function () {
    $("#saveForm").validate({
        rules: {
            Name: {
                required: true
            },
            Price: {
                required: true
            }
        }
    });
    $("#btnSave").bindSubmit({
        before: function () {
            var name = $("#expressId option:selected").text();
            $("#expressName").val(name);
            return true;
        }
    });
});
