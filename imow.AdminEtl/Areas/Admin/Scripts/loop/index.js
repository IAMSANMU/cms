$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Loop/Modify", action: "/Admin/Loop/Add" },
                { type: "logicDelete", perm: "/Loop/Modify", action: "/Admin/Loop/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Loop/Modify", action: "/Admin/Loop/Restore" }
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
            url: "/Admin/Loop/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[5, "asc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Title" },
            {
                data: "Img", render: function (data) {
                    var html = '<a href="' + data + '" target="_blank" ><img src="' + data + '" class="headPhoto "></img></a>';
                    return html;
                }
            },
            {
                data: "Link"
            },
            { data: "OrderNum" },
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
                        if ($.hasPerm("/Loop/Modify")) {
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
        $.openWindow("/Admin/Loop/Edit?id=" + id, "修改信息");

    });


});