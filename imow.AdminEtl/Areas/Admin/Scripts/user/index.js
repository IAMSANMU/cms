$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list") {
            btns = [
                { type: "refresh" },
                { type: "add", perm: "/Admin/Modify", action: "/Admin/Admin/Add" },
                { type: "active", perm: "/Admin/Modify", action: "/Admin/Admin/Active" },
                { type: "unactive", perm: "/Admin/Modify", action: "/Admin/Admin/UnActive" },
                { type: "logicDelete", perm: "/Admin/Modify", action: "/Admin/Admin/Delete" }
            ];
        } else {
            btns = [
                { type: "refresh" },
                { type: "restore", perm: "/Admin/Modify", action: "/Admin/Admin/Restore" }
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
            url: "/Admin/Admin/GetUserList",
            type: "POST",
            dataType: "json"
        },
        "order": [[2, "desc"]],
        "columns": [
            { data: "" },
            { data: "" },
            { data: "UserName" },
            { data: "RealName" },
            { data: "LastLoginTime" },
            { data: "Sex" },
            { data: "Mobile" },
            { data: "Email" },
            { data: "QQ" },
            { data: "IsStop" },
            { data: "UpdateTime" }
        ],
        "columnDefs": [
            {
                "render": function(data, type, row) {
                    //创建按钮
                    var html = [];
                    html.push("<div class=\"checkbox \"><input type=\"checkbox\" class=\"i-checks\" value=\"" +
                        row.Id +
                        "\"></input></div>");
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
                        //判断权限
                        if ($.hasPerm("/Admin/Modify")) {
                            html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnEdit\" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i> 编辑</button>");
                        }
                        if ($.hasPerm("/Admin/Modify")) {
                            html.push(" <button type=\"button\" class=\"btn btn-danger btn-xs btnEditPwd\" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i> 修改密码</button>");
                        }
                    }
                    return html.join("");
                },
                "targets": 1,
                "orderable": false
            }, {
                // 性别
                "render": function(data, type, row) {
                    return data == 0 ? "未知" : (data == 1 ? "男" : "女");
                },
                "targets": 5
            }, {
                "render": function(data) {
                    return data ? "是" : "否";
                },
                "targets": [9]
            }
        ]
    }).on("click",".btnEdit", function () {
        var id = $(this).attr("data-id");
        $.openWindow("/Admin/Admin/Edit/?id=" + id, "修改信息");
    }).on("click",".btnEditPwd", function () {
        var id = $(this).attr("data-id");
        $.openSmall("/Admin/Admin/ModifyPwd/?id=" + id, "修改信息");
    });

});