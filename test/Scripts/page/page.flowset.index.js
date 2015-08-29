var flownodegrid;
var flowactiongrid;
var windows = [];
$(function () {
    $("#divLayout").ligerLayout({ leftWidth: 380, allowLeftCollapse: false });

    $("#divToolbar").ligerToolBar({ items: [
            { text: '新建审稿状态', click: MenuClick, icon: 'add' },
            { text: '编辑审稿状态', click: MenuClick, icon: 'modify' },
            { text: '删除审稿状态', click: MenuClick, icon: 'delete' }
        ]
    });

    flownodegrid = $("#divFlowStatusList").ligerGrid({
        columns: [
            { display: 'ID', name: 'StatusID', width: '10%', align: 'center' },
            { display: '状态名称', name: 'StatusName', width: '35%', align: 'left' },
            { display: '显示名称', name: 'DisplayName', width: '30%', align: 'left' },
            { display: '排序值', name: 'SortID', width: '15%', align: 'center' },
            { display: '状态', name: 'Status', width: '10%', align: 'center', render: function (item) {
                return item.Status == 1 ? "<img src='" + RootPath + "/Content/icons/ok.png' style='cursor:pointer;' onclick='ChangeStatusStatus(" + item.StatusID + ",0)' alt='启用' title='启用'/>" : "<img style='cursor:pointer;' src=" + RootPath + "'/Content/icons/stop.png' onclick='ChangeStatusStatus(" + item.StatusID + ",1)' alt='禁用' title='禁用'/>";
            }
            }
            ], width: '99.5%', height: '98%', rownumbers: false, usePager: false, url: RootPath + '/FlowSet/GetFlowStatusListGridAjax?rnd=' + Math.random(),
        onSelectRow: function (data, rowindex, rowobj) {
            LoadStatusActionList(data.StatusID);
            $("#divStatusTitle").show();
            $("#bStatusTitle").text(data.StatusName);
        }
    });

    $("#divActionToolbar").ligerToolBar({ items: [
            { text: '新建审稿操作', click: ActionMenuClick, icon: 'add' }
        ]
    });
});
var windows = [];
function MenuClick(item) {
    switch (item.text) {
        case "新建审稿状态":
            windows['adddialog'] = $.ligerDialog.open({ height: 380, url: RootPath + '/FlowSet/AddFlowStatus?rnd=' + Math.random(), title: '新增审稿状态', width: 650, slide: false, buttons: [
                        { text: '确定', onclick: function (item, dialog) {
                            dialog.frame.Save(flownodegrid, dialog);
                        }
                        },
                        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                        ]
            });
            break;
        case "编辑审稿状态":
            EditFlowStatus();
            break;
        case "删除审稿状态":
            DeleteFlowStatus();
            break;
    }
}

// 编辑审稿状态
function EditFlowStatus() {
    var rows = flownodegrid.getSelectedRow();
    if (rows == null) { alert('请选择要编辑的审稿状态'); return; }
    var StatusID = rows.StatusID;
    windows['editdialog'] = $.ligerDialog.open({ height: 380, url: RootPath + '/FlowSet/EditFlowStatus?StatusID=' + StatusID + "&rnd=" + Math.random(), title: '编辑审稿状态', width: 650, slide: false, buttons: [
                { text: '确定', onclick: function (item, dialog) {
                    dialog.frame.Save(flownodegrid, dialog);
                }
                },
                { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
    });
}

// 删除审稿状态
function DeleteFlowStatus() {
    var rows = flownodegrid.getSelectedRow();
    if (rows == null) { alert('请选择要删除的审稿状态'); return; }
    var StatusID = rows.StatusID;
    if (confirm('您确认要删除选中的审稿状态吗？')) {
        $.ajax({
            beforeSend: function () {
            },
            type: 'POST',
            url: RootPath + '/FlowSet/DelFlowStatusAjax/?rnd=' + Math.random(),
            data: { "StatusID": StatusID },
            success: function (data) {
                var e = eval("(" + data + ")");
                if (e.result == 'success') {
                    alert('删除成功');
                    flownodegrid.loadData();
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
}

// 修改审稿状态的状态
function ChangeStatusStatus(StatusID, Status) {
    if (confirm('您确认要修改审稿状态的状态吗？')) {
        $.ajax({
            beforeSend: function () {
            },
            type: 'POST',
            url: RootPath + '/FlowSet/EditStatusAjax/?rnd=' + Math.random(),
            data: { "StatusID": StatusID, "Status": Status },
            success: function (data) {
                var e = eval("(" + data + ")");
                if (e.result == 'success') {
                    alert('修改状态成功');
                    flownodegrid.loadData();
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
}
/********************************************************Action 操作*************************************************************/
function LoadStatusActionList(StatusID) {
    flowactiongrid = $("#divFlowActionList").ligerGrid({
        columns: [
            { display: '编号', name: 'ActionID', width: '50', align: 'center' },
            { display: '操作名称', name: 'ActionName', width: '80', align: 'left' },
            { display: '操作类型', name: 'ActionType', width: '80', align: 'center', render: function (rowdata, rowindex, value) {
                if (parseInt(value) == 1) {
                    return "改变状态";
                }
                else if (parseInt(value) == 2) {
                    return "原路返回";
                }
                else if (parseInt(value) == 3) {
                    return "通知消息";
                }
                else if (parseInt(value) == 4) {
                    return "原路撤回";
                }
                }
            },
            { display: '审稿状态', name: 'StatusName', width: '80', align: 'left' },
            { display: '排序值', name: 'SortID', width: '60', align: 'center' },
            { display: '操作状态', name: 'ActionStatus', width: '80', align: 'center', render: function (rowdata, rowindex, value) {
                var h = "<a href='javascript:SetFlowActionAuth(" + rowdata.ActionID + "," + StatusID + ")'>设置权限</a>";
                return h;
            }
            },
             { display: '操作', isSort: false, width: '100', render: function (rowdata, rowindex, value) {
                 var h = "";
                 h += "<a href='javascript:EditAction(" + rowdata.ActionID + "," + StatusID + ")'>修改</a> ";
                 h += "<a href='javascript:DeleteAction(" + rowdata.ActionID + "," + StatusID + ")'>删除</a> ";
                 return h;
             }
             }
            ], width: '99.5%', height: '98%', rownumbers: false, usePager: false, checkbox: false,
        url: RootPath + '/FlowSet/GetFlowActionListAjax?rnd=' + Math.random(), parms: { StatusID: StatusID }
    });
 }

 // 新建审稿操作
 function ActionMenuClick(item) {
     var rows = flownodegrid.getSelectedRow();
     if (rows == null) { alert('请先选择左侧的审稿状态'); return; }
     var StatusID = rows.StatusID;
     windows['addactiondialog'] = $.ligerDialog.open({ height: 380, url: RootPath + '/FlowSet/FlowActionSet?StatusID=' + StatusID + "&rnd=" + Math.random(), title: '新增审稿操作', width: 650, slide: false, buttons: [
                { text: '确定', onclick: function (item, dialog) {
                    dialog.frame.SaveAndUpdate(flowactiongrid, dialog);
                }
                },
                { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
     });
    }

    // 编辑审稿操作
    function EditAction(ActionID, StatusID) {
        windows['editactiondialog'] = $.ligerDialog.open({ height: 380, url: RootPath + '/FlowSet/FlowActionSet?ActionID=' + ActionID + '&StatusID=' + StatusID + "&rnd=" + Math.random(), title: '编辑审稿操作', width: 650, slide: false, buttons: [
                { text: '确定', onclick: function (item, dialog) {
                    dialog.frame.SaveAndUpdate(flowactiongrid, dialog);
                }
                },
                { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
        });
    }

 // 删除审稿操作
 function DeleteAction(ActionID, StatusID) {
     if (confirm('确定要删除当前审稿操作吗?')) {
         $.ajax({
             beforeSend: function () {
             },
             type: 'POST',
             url: RootPath + '/FlowSet/DelFlowActionAjax/?rnd=' + Math.random(),
             data: { "ActionID": ActionID, "StatusID": StatusID },
             cache: false,
             traditional: true,
             success: function (data) {
                 var e = eval("(" + data + ")");
                 if (e.result == 'success') {
                     alert('删除成功');
                     flowactiongrid.loadData();
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
 }

 // 设置操作权限
 function SetFlowActionAuth(ActionID, StatusID){
     windows['setauthdialog'] = $.ligerDialog.open({ height: 590, url: RootPath + '/FlowSet/FlowAuthSet?ActionID=' + ActionID + "&StatusID=" + StatusID + "&rnd=" + Math.random(), title: '设置操作权限', width: 650, slide: false, buttons: [
                { text: '确定', onclick: function (item, dialog) {
                    dialog.close();
                }
                },
                { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
     });
 }