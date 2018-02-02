$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Staff/Modify", action: "/Admin/Staff/Add" },
                { type: "logicDelete", perm: "/Staff/Modify", action: "/Admin/Staff/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Staff/Modify", action: "/Admin/Staff/Restore" }
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
            url: "/Admin/Staff/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[6, "desc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "CName" },
            { data: "EName" },
            { data: "Tel" },
            {
                data: "Origo"
            },
            {
                data: "SchoolId", render: function (data, type, row) {
                    return row.SchoolEntity.Name;
                }
            },
            {
                data: "IsCommand", render: function (data) {
                    return data ? "推荐" : "";
                }
            },
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
                        if ($.hasPerm("/Staff/Modify")) {
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
        $.openWindow("/Admin/Staff/Edit?id=" + id, "修改信息");

    });


});