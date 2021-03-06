﻿$(function () {
    function initConfig() {
        var pid = $("#pid").val();
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Company/Modify", action: "/Admin/Company/Add?pid=" + pid, height: "60%", width: "60%" },
                { type: "logicDelete", perm: "/Company/Modify", action: "/Admin/Company/Delete", confirmMsg: "删除会自动删除所有子集,你确定删除吗?" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Company/Modify", action: "/Admin/Company/Restore", confirmMsg: "是否确定还原,并还原所有子模块?" }
            ];
        }
        if (pid != 0) {
            btns.push({
                type: "back",
                action: "",
                click: function () {
                    location.href = "/Admin/Company/Index?pid=" + $("#ppid").val() + "&tab=" + tabType;
                },
                text: "返回上级"
            });
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
            url: "/Admin/Company/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[5, "asc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Name" },
            { data: "Levels" },
            { data: "IsDel" },
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
                    if ($.baseConfig.tab === "list") {
                        if ($.hasPerm("/Company/Modify")) {
                            html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnEdit\" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i> 编辑</button>");
                        }
                    }
                    html.push(" <button type=\"button\" class=\"btn btn-danger btn-xs btnChild\" data-tab="+$.baseConfig.tab+" data-id=\"" +
                        row.Id +
                        "\"><i class=\"fa fa-edit\"></i>子模块</button>");
                    return html.join("");
                },
                "targets": 1,
                "orderable": false
            }, {
                "render": function(data) {
                    return data ? "是" : "否";
                },
                "targets": [4]
            }, {
                "render": function (data) {
                    return data === 1 ? "公司" : (data === 2 ? "部门" : "岗位");
                },
                "targets": [3]
            }
        ]
    }).on("click", ".btnEdit", function () {
        var id = $(this).data("id");
        $.openSmall("/Admin/Company/Edit?id=" + id, "修改信息");

    }).on("click", ".btnChild", function () {
        var pid = $(this).data("id");
        var tab = $(this).data("tab");
        location.href = "/Admin/Company/Index?pid=" + pid + "&tab=" + tab;
    });


});