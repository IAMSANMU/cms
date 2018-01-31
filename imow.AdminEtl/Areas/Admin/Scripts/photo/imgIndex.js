$(function () {
    $(".ibox").eq(0).remove();
    function initConfig() {
        var id = $("#photoId").val();
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "add", perm: "/Photo/Modify", action: "/Admin/Photo/AddImg?id=" + id, width: "800px", height: "300px" },
                {
                    type: "back",
                    action: "",
                    click: function () {
                        location.href = "/Admin/Photo/Index";
                    },
                    text: "返回相册"
                }
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
        $.openWindow("/Admin/Photo/EditImg?id=" + id, "修改信息","800px","300px");
    });
    
    $(".btnDelete").click(function () {
        var id = $(this).data("id");
        $.confirm("是否确认删除", function () {
            $.post("/Admin/Photo/DeleteImg", { id: id }, function (result) {
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