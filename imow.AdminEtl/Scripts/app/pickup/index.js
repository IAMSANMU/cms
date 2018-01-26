$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns;
        if (tabType === "list" || tabType==="error") {
            btns = [
                { type: "refresh" },
                { type: "add", action: "/Pickup/Add",width:"800px",height:"400px" },
                { type: "active", text: "发短信", confirmMsg: "是否重发短信?", action: "/Pickup/SendSms" },
                { type: "logicDelete", action: "/Pickup/Delete" },
                { type: "active", text: "签收", confirmMsg: "是否签收选中快递?", action: "/Pickup/Signs" }
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
            url: "/Pickup/List",
            type: "POST",
            dataType:"json"
        },
        "columns": [
            { data: "" },
            { data: "" },
            { data: "Express" },
            { data: "Number" },
            { data: "Name" },
            { data: "Tel" },
            { data: "Code" },
            {
                data: "Status",
                render: function (data, type, row) {
                    return row.StatusStr;
                }
            },
            { data: "IsError",render : function(data,type,row) {
                return data ? row.ErrorInfo : "否";
            }
            },
            { data: "AddTime" },
            { data: "PickupTime" },
            {
                data: "PickupType",
                render: function (data) {
                    if (data) {
                        //自提/后台代签收/代签收
                        if (data == 1) {
                            return "自提";
                        }else if (data == 2) {
                            return "后台代签";
                        } else if (data == 3) {
                            return "代签";
                        }
                    }
                    return data;
                 }
            }
        ],
        "order": [[9, "desc"]],
        "columnDefs": [
            {
                "render": function (data, type, row) {
                    var html = [];
                    if (row.Status < 4) {
                        //创建按钮
                        html.push("<div class=\"checkbox \"><input type=\"checkbox\" class=\"i-checks\" value=\"" +
                            row.Id +
                            "\"></input></div>");
                    }
                    return html.join("");

                },
                "targets": 0,
                "orderable": false
            },
            {
                "render": function (data, type, row) {
                    //创建按钮
                    var html = [];
                    if ($.baseConfig.tab === "list" || $.baseConfig.tab === "error") {
                        //判断权限
                        if (row.Status < 4 || row.Status==6) {
                            html.push("<button type=\"button\" class=\"btn btn-default btn-xs btnEdit\" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i> 编辑</button>");
                        }
                        if (row.Status < 5 || row.Status == 6) {
                            html.push("<button type=\"button\" class=\"btn btn-primary btn-xs btnSure \" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i>签收</button>");
                        }
                        if (row.Status != 5 && row.Status != 6) {
                            html.push("<button type=\"button\" class=\"btn btn-warning btn-xs btnBack \" data-id=\"" +
                                row.Id +
                                "\"><i class=\"fa fa-edit\"></i>拒收</button>");
                        }
                        if (row.Status < 4 && !row.IsError) {
                            html.push("<button type=\"button\" class=\"btn btn-danger btn-xs btnError\" data-id=\"" + row.Id + "\"><i class=\"fa fa-edit\"></i>问题件</button>");
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
        $.openWindow("/Pickup/Edit/?id=" + id, "修改信息","800px","400px");
    }).on("click", ".btnSure", function () {
        var id = $(this).attr("data-id");
        $.post("/Pickup/Sign",{ id: id }, function (result) {
            if (result.Success) {
                $.alert("操作成功",
                    function () {
                        $("[name='refresh']").click();
                    });
            } else {
                $.alertError(result.Message);
            }
        }, "json");


    }).on("click", ".btnError", function () {
        var id = $(this).attr("data-id");
        $.prompt("问题件原因",
            function(msg) {
                $.post("/Pickup/Error", { id: id, msg: msg }, function(result) {
                    if (result.Success) {
                        $.alert("操作成功",
                            function() {
                                $("[name='refresh']").click();
                            });
                    } else {
                        $.alertError(result.Message);
                    }
                }, "json");

            });


    }).on("click", ".btnBack", function () {
        var id = $(this).attr("data-id");
        $.prompt("拒收原因",
            function (msg) {
                $.post("/Pickup/Back", { id: id, msg: msg }, function (result) {
                    if (result.Success) {
                        $.alert("操作成功",
                            function () {
                                $("[name='refresh']").click();
                            });
                    } else {
                        $.alertError(result.Message);
                    }
                }, "json");

            });


    });
    if ($.baseConfig.tab === "over") {
        $("#tbList").DataTable().order([9, 'asc']).draw();
    }


});