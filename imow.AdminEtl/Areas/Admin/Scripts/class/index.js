$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Class/Modify", action: "/Admin/Class/Add" },
                { type: "logicDelete", perm: "/Class/Modify", action: "/Admin/Class/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Class/Modify", action: "/Admin/Class/Restore" }
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
            url: "/Admin/Class/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[7, "desc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Name" },
            { data: "Type" },
            {
                data: "SchoolId",
                render: function(data, type, row) {
                    return row.SchoolEntity.Name;
                }
            },
            {
                data: "IsMain",
                render: function (data, type, row) {
                    return data ? "主修" : "辅修";
                }
            },
            { data: "CreateTime" },
            { data: "UpdateTime" }
        ],
        "columnDefs": [
            {
                "render": function(data, type, row) {
                    //创建按钮
                    var html = [];
                    html.push("<div class=\"checkbox \"><input type=\"checkbox\" class=\"i-checks\" value=\"" +row.Id +"\"></input></div>");
                    return html.join("");
                },
                "targets": 0,
                "orderable": false
            },
            {
                "render": function(data, type, row) {
                    //创建按钮
                    var html = [];
                    if ($.baseConfig.tab === "list") {
                        if ($.hasPerm("/Class/Modify")) {
                            html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnEdit\" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i> 编辑</button>");
                        }
                    }
                    return html.join("");
                },
                "targets": 1,
                "orderable": false
            }
        ]
    }).on("click", ".btnEdit", function () {
        var id = $(this).data("id");
        $.openWindow("/Admin/Class/Edit?id=" + id, "修改信息");

    });


});