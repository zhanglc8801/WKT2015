$(function () {
    $("#selIsAllowBack").val(curIsAllowBack);
    $("#selIsMultiPerson").val(GetBool(curIsMultiPerson) ? 1 : 0);
    $("#selMultiPattern").val(curMultiPattern);
    $("input:[name=TimeoutPattern]").attr("checked", GetBool(curTimeoutPattern));
    $('input[name="IsSMSRemind"]:checkbox').attr('checked', GetBool(curIsSMSRemind));
    $('input[name="IsEmailRemind"]:checkbox').attr('checked', GetBool(curIsEmailRemind));
    $('input[name="IsRetraction"]:checkbox').attr('checked', GetBool(curIsRetraction));
    $("#selCStatus").val(curCStatus);
});

// 保存
function Save(flownodegrid, dialog) {
    var StatusID = $("#hiddenStatusID").val();
    var StatusName = $("#txtStatusName").val();
    if ($.trim(StatusName) == "") {
        alert("请填写状态名称");
        $("#txtNodeName").focus();
        return false;
    }
    var DisplayName = $("#txtDisplayName").val();
    if ($.trim(DisplayName) == "") {
        alert("请填写显示名称");
        $("#txtDisplayName").focus();
        return false;
    }
    var RoleID = $("#selRoleList").val();
    var ActionRoleID = $("#selActionRoleList").val();
    var CStatus = $("#selCStatus").val();
    var SortID = $("#txtSortID").val();

    var FlowStatusObj = { "StatusID": StatusID, "StatusName": StatusName, "DisplayName": DisplayName, "RoleID": RoleID, "ActionRoleID": ActionRoleID, "CStatus": CStatus, "SortID": SortID };

    var FlowConfigID = $("#hiddenFlowConfigID").val();
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
    var FlowConfigObj = { "FlowConfigID": FlowConfigID, "StatusID": StatusID, "IsAllowBack": IsAllowBack, "IsMultiPerson": IsMultiPerson == 1 ? true : false, "MultiPattern": MultiPattern, "TimeoutDay": TimeoutDay, "TimeoutPattern": TimeoutPattern, "IsSMSRemind": IsSMSRemind, "IsEmailRemind": IsEmailRemind, "RangeDay": RangeDay, "RemindCount": RemindCount, "IsRetraction": IsRetraction };

    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        async: false, 
        url: RootPath + '/FlowSet/EditFlowStatusAjax/?rnd=' + Math.random(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ FlowStatus: FlowStatusObj, FlowConfig: FlowConfigObj }),
        success: function (e) {
            if (e.result == 'success') {
                msgdialog = $.ligerDialog.waitting('编辑审稿状态成功!');
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