$(function () {
    $('.i-checks').ickbox();
    $("#saveForm").validate({
        rules: {
            Name: {
                required: true,
                rangelength: [0, 32]
            },
            RoleCode: {
                rangelength: [0, 32]
            },
            Remark: {
                rangelength: [0, 256]
            }
        }
    });
    function initTree() {
        var roleId = $("#roleId").val();
        var url = "/Role/GetTreeJson";
        if (roleId !== "") {
            url = url + "?roleId=" + roleId;
        }

        $("#jstree").jstree({
            "checkbox": {
                "keep_selected_style": false
            },
            "plugins": ["checkbox", "types"],
            'types': {
                'default': {
                    'icon': 'fa fa-folder'
                }
            },
            'core': {
                'data': {
                    'url': url,
                    "method": "post",
                    "dataType": "json"
                }
            }

        });
    }
    initTree();
    $.ajaxForm('#btnSave', $("#saveForm"), {
        data: function (orignData) {
            var nodes = $("#jstree").jstree().get_checked(true);
            var codes = [];
            $.each(nodes, function (i, node) {
                Array.prototype.push.apply(codes, node.parents);
                codes.push(node.id);
            });
            $.unique(codes);
            var result = orignData + "&codes=" + codes.join(",");
            return result;
        },
        success: function (result) {
            $.alert("保存成功", function () {
                $.closeWindow(function () {
                    parent.location.reload();
                });
            });
        }
    });
});
