$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Order/Modify", action: "/Admin/Order/Choose",text:"选择回访",confirmMsg:"是否确认?" }
            ];
        } else {
            btns = [
                { type: "refresh" }
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
            url: "/Admin/Order/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[6, "desc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Name" },
            { data: "Tel" },
            {
                data: "SchoolEntity.Name"
            },
            {
                data: "ClassEntity.Name"
            },
            { data: "CreateTime" }
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
                    if ($.hasPerm("/Order/Modify")) {
                        html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnEdit\" data-id=\"" +
                            row.Id +
                            "\"><i class=\"fa fa-edit\"></i>详情</button>");
                    }
                    return html.join("");
                },
                "targets": 1,
                "orderable": false
            }
        ]
    }).on("click", ".btnEdit", function () {
        var id = $(this).data("id");
        $.openWindow("/Admin/Order/View?id=" + id, "详情");
    });


});