$(function () {
    $("#saveForm").submit(function() {
        $("#search").fastClick();
        return false;
    });
    $("#search").fastClick(function() {
        var number = $("#number").val();
        if (number) {
            $("#msg").load("/api/search", { n: number });
        }
    });
})

