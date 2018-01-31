$(function () {
    $(".ibox").eq(0).remove();
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "add", perm: "/Photo/Modify", action: "/Admin/Photo/Add",width:"800px",height:"300px" }
            ];
        } else {
            btns = [
                { type: "restore", perm: "/Photo/Modify", action: "/Admin/Photo/Restore" }
            ];
        }
        $.baseConfig.btns = btns;
        $.baseConfig.tab = tabType;
    }
    initConfig();

    $("#tbBars").ectoolbar({
        btns: $.baseConfig.btns,
        searchGrid: "#tbList"
    });

    $(".btnEdit").click(function () {
        var id = $(this).data("id");
        $.openWindow("/Admin/Photo/Edit?id=" + id, "修改信息","800px","300px");
    });
    $(".btnRestore").click(function () {
        var id = $(this).data("id");
        $.confirm("是否确认还原相册",function() {
            $.post("/Admin/Photo/Restore", { id: id }, function(result) {
                if (result.Success) {
                    $.close();
                    location.reload();
                } else {
                    $.alertError(result.Message);
                }

            }, "json");
        });
    });
    $(".btnDelete").click(function () {
        var id = $(this).data("id");
        $.confirm("是否确认删除相册", function () {
            $.post("/Admin/Photo/Delete", { id: id }, function (result) {
                if (result.Success) {
                    $.close();
                    location.reload();
                } else {
                    $.alertError(result.Message);
                }
            }, "json");
        });
    });


});