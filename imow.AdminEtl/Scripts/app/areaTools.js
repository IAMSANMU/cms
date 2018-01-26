$(function () {
    $.extend({
        loadAreaData: function(target,url,param,valId,callback) {
            $.post(url, param, function (result) {
                if (!result.Success) {
                    $.alertError(result.Message);
                } else {
                    var data = result.Data;
                    if (data.length === 0) {
                        target.hide();
                    } else {
                        var html = [];
                        html.push("<option value=''>请选择</option>");
                        for (var i = 0; i < data.length; i++) {
                            var selected = data[i].AreaID === valId ?"selected":"";
                            html.push("<option "+selected+" value='" + data[i].AreaID + "'>" + data[i].AreaName + "</option>");
                        }
                        target.html(html.join(""));
                        target.show();
                        if (valId) {
                            target.change();
                        }
                    }
                }
            }, "json");
        }
    });

    
    /**
     * 
     * @param {} valId 默认选中id
     * @param {} callback 回调
     * @returns {} 
     */
    $.fn.loadProvince = function (valId) {
        var target = $(this);
        $.loadAreaData(target, "/Admin/Area/GetProvince", {}, valId);
    }
    /**
     * 
     * @param {} pid 上一级id
     * @param {} valId  默认选中id
     * @returns {} 
     */
    $.fn.loadCity = function (pid,valId) {
        var target = $(this);
        $.loadAreaData(target, "/Admin/Area/GetCity", { id: pid }, valId);
    }
    $.fn.loadArea = function (pid,valId) {
        var target = $(this);
        $.loadAreaData(target, "/Admin/Area/GetArea", { id: pid }, valId);
    }
    $.fn.loadStree = function (pid,valId) {
        var target = $(this);
        $.loadAreaData(target, "/Admin/Area/GetStree", { id: pid }, valId);
    }

    /**
     * 加载省市区街道
     * @returns {} 
     */
    $.fn.loadAllArea = function () {
        var selects = $(this).find("select");
        var province = selects.eq(0);
        var city = selects.eq(1);
        var area = selects.eq(2);
        var stree = selects.eq(3);
        var pId = province.data("valid");
        var cId = city.data("valid");
        var aId = area.data("valid");
        var sId = stree.data("valid");

        //绑定事件
        province.change(function () {
            var prevId = $(this).val(); //上一级选中的值Id
            if (prevId) {
                city.loadCity(prevId, cId);
            } else {
                city.empty();
                area.empty();
                stree.empty();
            }
        });
        city.change(function () {
            var prevId = $(this).val();//上一级选中的值Id
            if (prevId) {
                area.loadArea(prevId, aId);
            } else {
                area.empty();
                stree.empty();
            }
        });
        area.change(function () {
            var prevId = $(this).val();//上一级选中的值Id
            if (prevId) {
                stree.loadStree(prevId, sId);
            } else {
                stree.empty();
            }
        });
        province.loadProvince(pId);
    }
})
   

