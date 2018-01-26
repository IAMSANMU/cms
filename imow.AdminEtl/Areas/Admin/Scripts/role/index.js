$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Role/Modify", action: "/Admin/Role/Add", width: "60%" },
                { type: "logicDelete", perm: "/Role/Modify", action: "/Admin/Role/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore",perm:"/Role/Modify", action: "/Role/Restore" }
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
    $("#tbList").tbInit();
    $("#tbList").DataTable({
        "ajax": {
            url: "/Admin/Role/List",
            type: "POST",
            dataType:"json"
        },
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Name" },
            { data: "Remark" },
            { data: "CreateTime" }
        ],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    //创建按钮
                    var html = [];
                    html.push("<div class=\"checkbox \"><input type=\"checkbox\" class=\"i-checks\" value=\"" + row.Id + "\"></input></div>");
                    return html.join("");
                },
                "targets": 0,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    //创建按钮
                    var html = [];
                    if ($.baseConfig.tab === "list") {
                        //判断权限
                        if ($.hasPerm("/Role/Modify")) {
                            html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnEdit\" data-id=\"" + row.Id + "\"><i class=\"fa fa-edit\"></i> 编辑</button>");
                        }
                    }
                    return html.join("");
                },
                "targets": 1,
                "orderable": false
            }
        ]
    }).on("click",".btnEdit", function () {
        var id = $(this).attr("data-id");
        $.openSmall("/Admin/Role/Edit/?id=" + id, "修改信息");
    });

});