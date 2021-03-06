﻿$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/School/Modify", action: "/Admin/School/Add" },
                { type: "logicDelete", perm: "/School/Modify", action: "/Admin/School/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/School/Modify", action: "/Admin/School/Restore" }
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
            url: "/Admin/School/List",
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
                data: "Addr"
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
                        if ($.hasPerm("/School/Modify")) {
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
        $.openWindow("/Admin/School/Edit?id=" + id, "修改信息");

    });


});