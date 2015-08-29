$(function () {
    GetFlowStepSortID();
    LoadAllFlowNode();
    // 下一步骤全选
    $("#btnSelAllNextNodes").click(function () {
        $("#selNextNodes option").each(function () {
            $(this).attr("selected", true);
        })
    });
    // 上移
    $("#btnUp").click(function () {
        var selCount = $("#selNextNodes option:selected").length;
        if (selCount == 0) {
            alert("调整步骤的顺序时，请选择其中一项！");
            return;
        }
        else if (selCount > 1) {
            alert("调整步骤的顺序时，只能选择其中一项！");
            return;
        }
        var selectedIndex = $('#selNextNodes')[0].selectedIndex;
        if (selectedIndex != 0) {
            var my_option = document.createElement("option");
            my_option.text = $('#selNextNodes')[0].options[selectedIndex].text;
            my_option.value = $('#selNextNodes')[0].options[selectedIndex].value;

            $('#selNextNodes')[0].options.add(my_option, selectedIndex - 1);
            $('#selNextNodes')[0].remove(selectedIndex + 1);
            $('#selNextNodes')[0].options[selectedIndex - 1].selected = true;
        }
    });
    // 下移
    $("#btnDown").click(function () {
        var selCount = $("#selNextNodes option:selected").length;
        if (selCount == 0) {
            alert("调整步骤的顺序时，请选择其中一项！");
            return;
        }
        else if (selCount > 1) {
            alert("调整步骤的顺序时，只能选择其中一项！");
            return;
        }
        var selectedIndex = $('#selNextNodes')[0].selectedIndex;
        if (selectedIndex != ($("#selNextNodes option").length-1)) {
            var my_option = document.createElement("option");
            my_option.text = $('#selNextNodes')[0].options[selectedIndex].text;
            my_option.value = $('#selNextNodes')[0].options[selectedIndex].value;

            $('#selNextNodes')[0].options.add(my_option, selectedIndex + 2);
            $('#selNextNodes')[0].remove(selectedIndex);
            $('#selNextNodes')[0].options[selectedIndex + 1].selected = true;
        }
    });
    // 可用步骤全选
    $("#btnSelAllEnableNodes").click(function () {
        $("#selEnableNodes option").each(function () {
            $(this).attr("selected", true);
        })
    });
    // 添加到下一步骤
    $("#btnAddNextNodes").click(function () {
        if ($("#selEnableNodes option:selected").length > 0) {
            $("#selEnableNodes option:selected").each(function () {
                $("#selNextNodes").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option");
                $(this).remove();
            })
        }
        else {
            alert("请选择要添加的步骤！");
        }
    });
    // 从下一步骤中移除
    $("#btnRemoveNextNodes").click(function () {
        if ($("#selNextNodes option:selected").length > 0) {
            $("#selNextNodes option:selected").each(function () {
                $("#selEnableNodes").append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option");
                $(this).remove();
            })
        }
        else {
            alert("请选择要移除的步骤！");
        }
    });
});

// 获取审稿步骤序号
function GetFlowStepSortID() {
    $.ajax({
        beforeSend: function () {
        },
        type: 'POST',
        url: RootPath + '/FlowSet/GetFlowSetNum/?rnd=' + Math.random(),
        success: function (data) {
            var e = eval("(" + data + ")");
            if (e.result == 'success') {
                $("#txtSortID").val(e.ReturnObject);
            }
            else if (e.result == 'failure') {
                alert(e.msg);
            }
            else if (e.result == 'error') {
                alert(e.msg);
            }
        },
        error: function (xhr) {
            alert('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
}

// 获取已有步骤信息
function LoadAllFlowNode() {
    $.ajax({
        beforeSend: function () {
        },
        type: 'POST',
        url: RootPath + '/FlowSet/GetFlowSetListDataAjax/?rnd=' + Math.random(),
        success: function (data) {
            var e = eval("(" + data + ")");
            if (e.result == 'success') {
                $("#selEnableNodes").empty();
                for (var i = 0; i < e.ReturnObject.length; i++) {
                    $("#selEnableNodes").append('<option value="' + e.ReturnObject[i].FlowSetID + '">' + e.ReturnObject[i].NodeName + '</option>');
                }
            }
            else if (e.result == 'failure') {
                alert(e.msg);
            }
            else if (e.result == 'error') {
                alert(e.msg);
            }
        },
        error: function (xhr) {
            alert('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
}

// 保存
function Save(flownodegrid, dialog) {
    var SortID = $("#txtSortID").val();
    var NodeName = $("#txtNodeName").val();
    if ($.trim(NodeName) == "") {
        alert("请填写步骤名称");
        $("#txtNodeName").focus();
        return false;
    }
    var DisplayName = $("#txtDisplayName").val();
    if ($.trim(DisplayName) == "") {
        alert("请填写显示名称");
        $("#txtDisplayName").focus();
        return false;
    }
    var GroupID = $("#selGroup").val();
    var ArrayNextNodes = [];
    $("#selNextNodes option").each(function () {
        ArrayNextNodes.push($(this).val());
    })
    var FlowSetObj = { "NodeName": NodeName, "DisplayName": DisplayName, "SortID": SortID, "GroupID": GroupID, "NextNodes": ArrayNextNodes.join() };

    var IsAllowBack = $("#selIsAllowBack").val();
    var IsMultiPerson = $("#selIsMultiPerson").val();
    var MultiPattern = $("#selMultiPattern").val();
    var TimeoutDay = $("#txtTimeoutDay").val();
    var TimeoutPattern = $("input:[name=TimeoutPattern]:radio:checked").val();
    var IsSMSRemind = false;
    if ($("#chbIsSMSRemind").attr("checked") == "checked") {
        IsSMSRemind = true;
    }
    var IsEmailRemind = false;
    if ($("#chbIsEmailRemind").attr("checked") == "checked") {
        IsEmailRemind = true;
    }
    var RangeDay = $("#txtRangeDay").val();
    var RemindCount = $("#txtRemindCount").val();
    var IsRetraction = false;
    if ($("#chbIsRetraction").attr("checked") == "checked") {
        IsRetraction = true;
    }
    var FlowConfigObj = { "IsAllowBack": IsAllowBack, "IsMultiPerson": IsMultiPerson == 1 ? true : false, "MultiPattern": MultiPattern, "TimeoutDay": TimeoutDay, "TimeoutPattern": TimeoutPattern, "IsSMSRemind": IsSMSRemind, "IsEmailRemind": IsEmailRemind, "RangeDay": RangeDay, "RemindCount": RemindCount, "IsRetraction": IsRetraction };

    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        async: false, 
        url: RootPath + '/FlowSet/AddFlowSetAjax/?rnd=' + Math.random(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ FlowNode: FlowSetObj,FlowConfig:FlowConfigObj }),
        success: function (e) {
            if (e.result == 'success') {
                msgdialog = $.ligerDialog.waitting('添加审稿流程步骤成功!');
                setTimeout(function () {
                    msgdialog.close();
                }, 2000);
                flownodegrid.loadData();
                dialog.close();                     
            }
            else if (e.result == 'failure') {
                alert(e.msg);
            }
            else if (e.result == 'error') {
                alert(e.msg);
            }
        },
        error: function (xhr) {
            alert('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
}