$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Context/Modify", action: "/Admin/Context/Add" },
                { type: "logicDelete", perm: "/Context/Modify", action: "/Admin/Context/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Context/Modify", action: "/Admin/Context/Restore" }
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
            url: "/Admin/Context/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[4, "desc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Section.Name" },
            { data: "Title" },
            {
                data: "IsShow",
                render: function (data) {
                    return data ? "显示" : "隐藏";
                }
            },
            {
                data: "Status",
                render: function (data) {
                    var msg = "";
                    if (data === 1) {
                        msg = "未审核";
                    } else if (data ===2) {
                        msg = "审核通过";
                    } else if (data === 3) {
                        msg = "审核不通过";
                    }
                    return msg;
                }
            },
            {
                data: "IsTop",
                render: function (data) {
                    return data ? "置顶" : "";
                }
            },
            { data: "CreateTime" },
            { data: "Author" }
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
                        if ($.hasPerm("/Context/Modify")) {
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
        $.openWindow("/Admin/Context/Edit?id=" + id, "修改信息");

    });


});