$(function () {
    function initConfig() {
        var tabType = $("#_tab").val();
        var btns = [
                { type: "refresh" }
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
            url: "/SMS/List",
            type: "POST",
            dataType: "json"
        },
        "order": [[4, "desc"]],
        "columns": [
            { data: "Number" },
            { data: "Name" },
            { data: "Tel" },
            { data: "Msg" },
            { data: "CreateTime" },
            { data: "Url" },
            { data: "Remark" }
        ]
    });
});