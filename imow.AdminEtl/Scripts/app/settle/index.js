$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns = [
            { type: "refresh" },
            { type: "settle", text: "结算今日", click: settle },
            { type: "settleYes", text: "结算往日",click: function() {
                $("[name='settleYes']").datepicker('show');
            } }
        ];
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
            url: "/settle/List",
            type: "POST",
            dataType: "json"
        },
        "columns": [
            { data: "Express" },
            {
                data: "StartTime",
                render: function (date, type, row) {
                    return row.StartTime.substring(0, 10);
                }
            },
            { data: "InExTotal" },
            { data: "InExMoney" },
            { data: "OutUserTotal" },
            { data: "OutUserMoney" },
            { data: "InExError" },
            { data: "InExErrorMoney" },
            { data: "CreateTime" }
        ],
        "order": [[8, "desc"]],
        "footerCallback": function (tfoot, data, start, end, display) {
            var api = this.api();
            var json = api.context[0].json.info;
            var tds = $(tfoot).find("td");
            tds.eq(1).text(json.InExTotal);
            tds.eq(2).text(json.InExMoney);
            tds.eq(3).text(json.OutUserTotal);
            tds.eq(4).text(json.OutUserMoney);
            tds.eq(5).text(json.InExError);
            tds.eq(6).text(json.InExErrorMoney);
        }
    });
    function settle() {
        $.confirm("是否确认结算？若已结算，则会覆盖",
            function () {
                $.post("/Settle/SettleToday", function (result) {
                    if (result.Success) {
                        $.alert("结算成功", function () {
                            location.reload();
                        });
                    } else {
                        $.alertError(result.Message);
                    }
                }, "json");

            });
    }


    $("[name='settleYes']").datepicker({
        endDate: new Date()

    }).on("changeDate", function(e) {
        var day = e.format();
        $.confirm("是否确认结算"+day+"？若已结算，则会覆盖",
            function () {
                $.post("/Settle/Settle",{start:day,end:day}, function (result) {
                    if (result.Success) {
                        $.alert("结算成功", function () {
                            location.reload();
                        });
                    } else {
                        $.alertError(result.Message);
                    }
                }, "json");

            });


    });
});