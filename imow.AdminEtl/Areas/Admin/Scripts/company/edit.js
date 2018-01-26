$(function() {
    $("#saveForm").validate({
        rules: {
            Name: {
                required: true
            }
        }
    });
    var option = {};
    //判断是增加数据
    if ($("#id").length ===0 ) {
        option= {
            text:"架构类型无法修改,是否确认增加"
        }
    }

    $("#btnSave").bindSubmit(option);

    $('.i-checks').ickbox();
});