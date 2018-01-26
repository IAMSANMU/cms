
$.extend(true, $.fn.dataTable.defaults, {
    serverSide: true,
    lengthChange: false,
    searching: false,
    select: 'single',
    info: false,
    autoWidth: false,
    processing: true,
    language: {
        "paginate": {
            "first": "首页",
            "last": "尾页",
            "previous": "上一页",
            "next": "下一页"
        },
        "emptyTable": "查无数据!",
        "loadingRecords": "请等待，数据正在加载中......",
        "processing": "请等待，数据正在加载中......"
    }
    
});
$.fn.extend({
    preDataTableEvent: function (option) {
        var instance = null;
        var searchModel = null;
        var addTosearchModel = function (elm) {
            var searchColum, searchOperator, searchValue,searchNot,searchNotAuto;
            var $elm = $(elm);
            searchValue = $elm.val();
            if (searchValue) {
                searchColum = $elm.attr('searchColum');
                searchOperator = $elm.attr('searchOperator');
                searchNot = $elm.attr('searchNot') || false;
                searchNotAuto = $elm.attr('searchNotAuto') || false;
                searchModel.push({
                    Operator: searchOperator,
                    Value: searchValue,
                    Column: searchColum,
                    Not: searchNot,
                    NotAuto: searchNotAuto
                });
            }
        }
        var self = $(this);

        var startSearch = $('#search-content [startSearch]');
        if (startSearch.length > 0) {
            searchModel = [];
            startSearch.each(function (index, elm) {
                addTosearchModel(elm);
            });
        }
        Ladda.bind('#search', {
            callback: function (ins) {
                searchModel = [];
                instance = ins;
                var serachInput = $('#search-content [searchColum]');
                serachInput.each(function (index, elm) {
                    addTosearchModel(elm);
                });
                self.DataTable().draw();
            }
        });

        document.onkeydown = function (e) {
            var ev = document.all ? window.event : e;
            if (ev.keyCode === 13) {
                $("#search").click();
            }
        }

        var isselect = !!option.select;
        if (isselect) {
            var selected = [];
            self.find('tbody')
                .on('click', 'tr',
                    function (e) {
                        if (e.target.nodeName === 'TD') {
                            var id = this.id;
                            var index = $.inArray(id, selected);

                            if (index === -1) {
                                selected.push(id);
                            } else {
                                selected.splice(index, 1);
                            }

                            $(this).toggleClass('selected');
                        }
                    });
        }
        self.on('xhr.dt', function () {
            if (instance) {
                instance.stop();
                instance = null;
            }
        });
        return self.on('preXhr.dt', function (e, settings, data) {

            if (searchModel && searchModel.length > 0)
                data.SearchModels = searchModel;
            if (data.order.length) {
                data.ordertype = data.order[0].dir;
                data.order = data.columns[data.order[0].column].data;
            }
            if (data.length > 0) {
                data.PageIndex = data.start === 0 ? 1 : (data.start / data.length + 1);
                data.PageSize = data.length;
            }
            data.length = {};
            data.start = {};
            data.columns = {};
            data.search = {};
        });
    }
});

$.fn.extend({
    preDatepickerEvent: function (e) {
        var self = $(this);
        return self.on('apply.daterangepicker', function (e, dataPick) {
            $(this).val(dataPick.startDate.format(dataPick.locale.format) + ' - ' + dataPick.endDate.format(dataPick.locale.format));
            var startTime = dataPick.startDate.format('YYYY-MM-DD hh:mm:ss.S');
            var endTime = dataPick.endDate.format('YYYY-MM-DD hh:mm:ss.S');
            $('#startTime').val(startTime);
            $('#endTime').val(endTime);
        }).on('cancel.daterangepicker', function (e, dataPick) {
            $('#startTime').val('');
            $('#endTime').val('');
            $(this).val('');
        });
    }
});
