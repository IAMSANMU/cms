$(function() {
    $("a[data-toggle='tab']")
        .on("shown.bs.tab",function(e) {
            console.log(e.target);
        });
});