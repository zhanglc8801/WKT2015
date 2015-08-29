var windows = [];
$(function () {
    $("#divRoleLayout").ligerLayout({ leftWidth: 320, allowLeftCollapse: false });

    // tool bar
    $("#divMenuToolbar").ligerToolBar({ items: [
            { text: '赋权', click: MenuToolClick, icon: 'save' }
        ]
    });
    $("#divAuthorToolbar").ligerToolBar({ items: [
            { text: '添加用户到该角色', click: AuthorRoleSetClick, icon: 'add' },
            { line: true },
            { text: '从该角色移除用户', click: AuthorRoleSetClick, icon: 'delete' },
            { line: true },
            { text: '设置例外', click: AuthorRoleSetClick, icon: 'settings' }
        ]
    });

    // 菜单设置,菜单赋权
    function MenuToolClick(item) {
        var notes = windows['menu'].getAllChecked();
        var menuIDArray = [];
        for (var i = 0; i < notes.length; i++) {
            menuIDArray.push(notes[i].data.key);
        }
        if (menuIDArray.length == 0) {
            alert('请选择要赋权的菜单');
            return;
        }
        // roleid
        var RoleID = 0;
        if ($("#spanCurRoleID").length > 0) {
            RoleID = parseInt($("#spanCurRoleID").text());
        }
        if (RoleID == 0) {
            alert('请先选择左侧的角色信息');
            return;
        }
        $.ajax({
            beforeSend: function () {

            },
            type: 'POST',
            url: RootPath + '/menu/setmenuright/?rnd=' + Math.random(),
            data: { "RoleID": RoleID, "IDAarry": menuIDArray },
            cache: false,
            traditional: true,
            success: function (data) {
                var e = eval("(" + data + ")");
                
                if (e.result == 'success') {
                    alert('设置成功');
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

    // 用户角色设置
    function AuthorRoleSetClick(item) {
        var RoleID = 0;
        if ($("#spanCurRoleID").length > 0) {
            RoleID = parseInt($("#spanCurRoleID").text());
        }
        if (RoleID == 0) {
            alert('请先选择左侧的角色信息');
            return;
        }
        switch (item.text) {
            case "添加用户到该角色":
                windows['seldialog'] = $.ligerDialog.open({ height: 420, url: RootPath + '/member/seldialog', title: '选择成员', width: 580, slide: false, buttons: [
                    { text: '确定', onclick: function (item, dialog) {
                        rows = dialog.frame.memberSelDialog.getSelectedRows();
                        if (rows == "") { alert('请选择要添加的成员'); return; }
                        var data = [];
                        for (i = 0; i < rows.length; i++) {
                            data.push(rows[i].AuthorID);
                        }
                        AddAuthorToRole(RoleID, data);
                        dialog.close();
                    }
                    },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                    ]
                });
                break;
            case "从该角色移除用户":
                RemoveAuthorFromRole(RoleID);
                break;
            case "设置例外":
                var rows = windows['author'].getSelectedRows();
                if (rows == "") { alert('请选择要设置例外的成员'); return; }
                var data = []; // 设置例外的成员
                for (i = 0; i < rows.length; i++) {
                    data.push(rows[i].AuthorID);
                }
                windows['excptionmenudialog'] = $.ligerDialog.open({ height: 600, url: RootPath + '/Menu/SetExceptionMenu?RoleID=' + RoleID + "&AuthorID=" + data[0] + "&rnd=" + Math.random(), title: '设置例外权限', width: 600, slide: false, buttons: [
                    { text: '确定', onclick: function (item, dialog) {
                        var menuidRows = dialog.frame.setexmenutree.getAllChecked();
                        var menuIDArray = [];
                        for (var i = 0; i < menuidRows.length; i++) {
                            menuIDArray.push(menuidRows[i].data.key);
                        }
                        if (menuIDArray.length == 0) {
                            alert('不能全部取消授权，请设置');
                            return;
                        }
                        SetAuthorExceptionMenuRight(RoleID, data, menuIDArray);
                        dialog.close();
                    }
                    },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                    ]
                });
                return;
                break;
        }
    }

    // 角色菜单
    function RoleMenuClick(item) {
        if (item.text == "增加") {
            windows['adddialog'] = $.ligerDialog.open({ height: 315, url: RootPath + '/Role/Add', title: '新增角色', width: 430, slide: false });
            return;
        }
        var rows = windows['role'].getSelectedRows();
        if (rows == "") { alert('请选择行'); return; }
        var data = [];
        for (i = 0; i < rows.length; i++) {
            if (parseInt(rows[i].RoleID) > 5) {
                data.push(rows[i].RoleID);
            }
        }
        switch (item.text) {
            case "修改":
                if (data.length > 0) {
                    windows['editdialog'] = $.ligerDialog.open({ height: 315, url: RootPath + '/Role/Edit?RoleID=' + data[0], title: '编辑角色', width: 430, slide: false });
                }
                break;
            case "删除":
                if (confirm('您确认要删除选中的角色吗？')) {
                    DelRole(data);
                }
                break;
        }
    }

    // tab
    var tArtist2 = new TabClickGroup("next", "on", ["liBaseInfo", "liMenuAuth"], ["divBaseInfo", "divMenuAuth"]);

    // 角色列表
    windows['role'] =
            $("#divRole").ligerGrid({
                columns: [
                { display: '角色ID', name: 'RoleID', align: 'center', width: 70 },
                { display: '角色名称', name: 'RoleName', width: 160, render: function (item) {
                    if (parseInt(item.RoleID) > 5) {
                        return item.RoleName;
                    }
                    else {
                        return item.RoleName + "(*)";
                    }
                }
                }
                ], url: RootPath + '/role/GetRoleListAjax?rnd=' + Math.random(), width: '99%', height: '100%', pageSize: 100, usePager: false, rownumbers: true,
                toolbar: { items: [
                    { text: '增加', click: RoleMenuClick, icon: 'add' },
                    { line: true },
                    { text: '修改', click: RoleMenuClick, icon: 'modify' },
                    { line: true },
                    { text: '删除', click: RoleMenuClick, icon: 'delete' }
                    ]
                },
                onSelectRow: function (data, rowindex, rowobj) {
                    $("#noticeTip").html('当前角色ID：[ <span id="spanCurRoleID"></span> ]&nbsp;&nbsp;当前角色名称：[ <span id="spanCurRoleName"></span> ]');
                    $("#spanCurRoleID").text(data.RoleID);
                    $("#spanCurRoleName").text(data.RoleName);
                    $("#divRight").show();
                    LoadAuthorList(data.RoleID);
                    LoadMenu(data.RoleID);
                }
            });
});

// 载入指定角色的成员列表
function LoadAuthorList(RoleID) {
    windows['author'] = null;
    windows['author'] =
            $("#divAuthorList").ligerGrid({
                columns: [
                { display: 'ID', name: 'AuthorID', align: 'left', width: 100, minWidth: 80 },
                { display: '登录名', name: 'LoginName', minWidth: 260 },
                { display: '真实姓名', name: 'RealName', minWidth: 300 }
                ], url: RootPath + '/Member/GetMemberListByRole?RoleID=' + RoleID + "&rnd=" + Math.random(), checkbox: true, width: '99%', height: '100%', pageSize: 30, rownumbers: true
            });
}

// 载入菜单
function LoadMenu(RoleID) {
    //树
    $("#ulMenuTree").html("");
    windows['menu'] = $("#ulMenuTree").ligerTree({
        url: RootPath + '/menu/GetMenuAjaxByRole?RoleID=' + RoleID + "&rnd=" + Math.random(),
        checkbox: true,
        slide: false,
        nodeWidth: 180,
        attribute: ['nodename', 'url']
    });
}

// 删除角色
function DelRole(RoleIDArray) {
    if (RoleIDArray.length > 0) {
        $.ajax({
            beforeSend: function () {
            },
            type: 'POST',
            url: RootPath + '/role/delrole/?rnd=' + Math.random(),
            data: { "IDAarry": RoleIDArray },
            cache: false,
            traditional: true,
            success: function (data) {
                var e = eval("(" + data + ")");
                if (e.result == 'success') {
                    alert('删除成功');
                    windows['role'].loadData();
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

// 给指定的角色添加成员
function AddAuthorToRole(RoleID, AuthorIDArray) {
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/member/AddAurhorRole/?rnd=' + Math.random(),
        data: { "RoleID": RoleID, "IDAarry": AuthorIDArray },
        cache: false,
        traditional: true,
        success: function (data) {
            var e = eval("(" + data + ")");
            if (e.result == 'success') {
                alert('添加成功');
                windows['author'].loadData();
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

// 从角色中移除成员
function RemoveAuthorFromRole(RoleID) {
    if (confirm('确认要从当前角色中移除该成员吗？')) {
        var rows = windows['author'].getSelectedRows();
        if (rows == "") { alert('请选择要移除的成员'); return; }
        var data = []; // 要移除的作者列表
        for (i = 0; i < rows.length; i++) {
            data.push(rows[i].AuthorID);
        }
        $.ajax({
            beforeSend: function () {

            },
            type: 'POST',
            url: RootPath + '/member/DelAurhorRole/?rnd=' + Math.random(),
            data: { "RoleID": RoleID, "IDAarry": data },
            cache: false,
            traditional: true,
            success: function (data) {
                var e = eval("(" + data + ")");
                if (e.result == 'success') {
                    alert('移除成功');
                    windows['author'].loadData();
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

// 给指定的成员设置例外
function SetAuthorExceptionMenuRight(RoleID, data, MenuIDArray) {
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/member/SetAurhorException/?rnd=' + Math.random(),
        data: { "RoleID": RoleID, "AuthorIDArray": data, "IDAarry": MenuIDArray },
        cache: false,
        traditional: true,
        success: function (data) {
            var e = eval("(" + data + ")");
            if (e.result == 'success') {
                alert('设置成功');
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