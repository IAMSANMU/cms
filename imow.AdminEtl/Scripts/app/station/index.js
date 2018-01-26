$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", action: "/Station/Add" },
                { type: "logicDelete", action: "/Station/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", action: "/Station/Restore" }
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
            url: "/Station/List",
            type: "POST",
            dataType: "json"
        },
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Name" },
            { data: "Express" },
            { data: "LinkMan" },
            { data: "Mobile" },
            { data: "Price" }
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
                        html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnEdit\" data-id=\"" + row.Id + "\"><i class=\"fa fa-edit\"></i> 编辑</button>");
                    }
                    return html.join("");
                },
                "targets": 1,
                "orderable": false
            }
        ]
    }).on("click", ".btnEdit", function () {
        var id = $(this).attr("data-id");
        $.openSmall("/Station/Edit/?id=" + id, "修改信息");
    }).on("click", ".btnSure", function () {
        var id = $(this).attr("data-id");
        $.openSmall("/Station/Sure/?id=" + id, "修改信息");
    });

});