$(function () {
    // 选择投稿人
    $("#selAuthor").click(function () {
        var common = new CommonPage();
        common.SelAuthor("txtInAuthor", "hiddenInAuthor");
    });
    // 搜索
    $("#btnSearch").click(function () {
        Search();
    });
    // 重置
    $("#btnReset").click(function () {
        $("#txtCNumber").val("");
        $("#txtTitle").val("");
        $("#txtKeyword").val("");
        if ($('#selActionStatus').length > 0) {
            $('#selActionStatus')[0].options[0].selected = true;
        }
        $("#txtAuthor").val("");
        $("#hiddenInAuthor").val("");
        $("#txtInAuthor").val("");
        $("#txtStartDate").val("");
        $("#txtEndDate").val("");
        $("#selflag").val("");
   
        
    });
    // 载入当前可以操作的审稿状态
    LoadStatusList();
})
// 如果CStatus=200则增加年卷期列
function Search(CStatus) {
    columns = [];
    if (CStatus == 200) {
        columns = [
            { display: '作者ID', name: 'AuthorID', hide: true },
            { display: '稿件ID', name: 'CID', hide: true },
            { display: '日志ID', name: 'FlowLogID', hide: true },
            { display: '稿件编号', name: 'CNumber', width: '85', align: 'center' },
            { display: '稿件标题', name: 'Title', width: '240', align: 'center', render: function (item) {
                var title = item.Title;
                title = item.IsQuick == true ? "<img src='" + RootPath + "/Content/ligerUI/skins/icons/up.gif' alt='加急稿件' title='加急稿件'/>&nbsp;" + title : title;
                if (item.Flag != "") {
                    title = "<img src=" + RootPath + "/Content/ligerUI/skins/icons/" + item.Flag + ".gif alt='旗帜标记' title='旗帜标记'/>&nbsp;" + title;
                }
                title = '<a href="javascript:void(0)" title="' + title + '" onclick="ViewDetail(' + item.CID + ',\'' + item.CNumber + '\',' + $("#hiddenStatus").val() + "," + item.FlowLogID + "," + item.AuthorID + ')">' + title + '</a>';
                return title;
            }
            },
            { display: '年', name: 'Year', width: '40', align: 'center' },
            { display: '期', name: 'Issue', width: '40', align: 'center' },
            { display: '栏目', name: 'JChannelName', width: '70', align: 'center' },
            { display: '第一作者', name: 'FirstAuthor', width: '60', align: 'center' },
            { display: '通信作者', name: 'CommunicationAuthor', width: '60', align: 'center' },
            { display: '联系方式', name: 'TelMobile', width: '100', align: 'center', render: function (item) {
                return "<a href='javascript:alert(\"" + item.Mobile + "&nbsp;" + item.Tel + "\");'>" + item.Mobile + "<span>&nbsp;</span>" + item.Tel + "</a>";
            }
            },
            { display: '发送人', name: 'SendUserName', width: '60', align: 'center' },
            { display: '接收人', name: 'RecUserName', width: '60', align: 'center' },
            { display: '稿件状态', name: 'AuditStatus', width: '80', align: 'center' },
            { display: '处理时间', name: 'HandTime', width: '130', align: 'center', render: function (item) {
                var date = eval('new ' + item.HandTime.substr(1, item.HandTime.length - 2));
                return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + "(距今<span style='color:blue;'>" + item.HandDays + "</span>天)"
            }
            },
            { display: '投稿时间', name: 'AddDate', width: '130', align: 'center', render: function (item) {
                var date = eval('new ' + item.AddDate.substr(1, item.AddDate.length - 2));
                return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + "(距今<span style='color:blue;'>" + item.Days + "</span>天)"
            }
            },
            { display: '备注', name: 'Remark', width: '100', align: 'center', render: function (item) {
                if (item.Remark != "") {
                    return "<a href='javascript:alert(\"" + item.Remark + "\")' title=\"" + item.Remark + "\">" + item.Remark + "</a>";
                }
            }
            }
         ]
    }
    else {
        columns = [
            { display: '作者ID', name: 'AuthorID', hide: true },
            { display: '稿件ID', name: 'CID', hide: true },
            { display: '日志ID', name: 'FlowLogID', hide: true },
            { display: '稿件编号', name: 'CNumber', width: '85', align: 'center' },
            { display: '稿件标题', name: 'Title', width: '240', align: 'center', render: function (item) {
                var title = item.Title;
                title = item.IsQuick == true ? "<img src='" + RootPath + "/Content/ligerUI/skins/icons/up.gif' alt='加急稿件' title='加急稿件'/>&nbsp;" + title : title;
                if (item.Flag != "") {
                    title = "<img src='" + RootPath + "/Content/ligerUI/skins/icons/" + item.Flag + ".gif' alt='旗帜标记' title='旗帜标记'/>&nbsp;" + title;
                }
                if (item.IsRetractions) {
                    title = '<a href="javascript:void(0)" style=\"color:red;\" onclick="ViewDetail(' + item.CID + ',\'' + item.CNumber + '\',' + $("#hiddenStatus").val() + "," + item.FlowLogID + "," + item.AuthorID + ')">(作者申请撤稿)' + title + '</a>';
                }
                else {
                    title = '<a href="javascript:void(0)" title="' + title + '" onclick="ViewDetail(' + item.CID + ',\'' + item.CNumber + '\',' + $("#hiddenStatus").val() + "," + item.FlowLogID + "," + item.AuthorID + ')">' + title + '</a>';
                }

                return title;
            }
            },
            { display: '第一作者', name: 'FirstAuthor', width: '70', align: 'center' },
            { display: '通讯作者', name: 'CommunicationAuthor', width: '70', align: 'center' },
            { display: '联系方式', name: 'TelMobile', width: '100', align: 'center', render: function (item) {
                return "<a href='javascript:$.ligerDialog.success(\"" + item.Mobile + "&nbsp;" + item.Tel + "\");'>" + item.Mobile + "<span>&nbsp;</span>" + item.Tel + "</a>"
            }
            },
        //               { display: '催审时间', name: 'SendDate', width: '80', align: 'center', type: 'date', format: 'yyyy-MM-dd hh:mm:ss',
        //                   render: function (item) {
        //                       if (item.SendDate != "") {
        //                           return item.SendDate.toString("yyyy-MM-dd")
        //                       }
        //                   }

        //                },
            {display: '发送人', name: 'SendUserName', width: '70', align: 'center' },
            { display: '接收人', name: 'RecUserName', width: '70', align: 'center' },
            { display: '稿件状态', name: 'AuditStatus', width: '70', align: 'center' },
            { display: '审稿费', name: 'IsPayAuditFee', width: '60', align: 'center', hide: true, render: function (item) {
                if (item.IsPayAuditFee) {
                    return "<img src='" + RootPath + "/Content/ligerUI/skins/icons/ok.gif' alt='已交审稿费' title='已交审稿费'/>"
                }
                else {
                    return "<img src='" + RootPath + "/Content/ligerUI/skins/icons/delete.gif' alt='未交审稿费' title='未交审稿费'/>"
                }
            }
            },
            { display: '版面费', name: 'IsPayPageFee', width: '60', align: 'center', hide: true, render: function (item) {
                if (item.IsPayPageFee) {
                    return "<img src='" + RootPath + "/Content/ligerUI/skins/icons/ok.gif' alt='已交版面费' title='已交版面费'/>"
                }
                else {
                    return "<img src='" + RootPath + "/Content/ligerUI/skins/icons/delete.gif' alt='未交版面费' title='未交版面费'/>"
                }
            }
            },
            { display: '介绍信', name: 'IntroLetterPath', width: '60', hide: true, align: 'center', render: function (item) {
                if (item.IntroLetterPath == "") {
                    return "<img src='" + RootPath + "/Content/ligerUI/skins/icons/delete.gif' style='cursor:pointer;' onclick=\"ConfirmIntro(" + item.CID + ",1)\" alt='未提交介绍信' title='未提交介绍信'/>"
                }
                else {
                    return "<img src='" + RootPath + "/Content/ligerUI/skins/icons/ok.gif' style='cursor:pointer;' onclick=\"ConfirmIntro(" + item.CID + ",0)\" alt='已提交介绍信' title='已提交介绍信'/>"
                }
            }
            },
            { display: '处理时间', name: 'HandTime', width: '130', align: 'center', render: function (item) {
                var date = eval('new ' + item.HandTime.substr(1, item.HandTime.length - 2));
                return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + "(距今<span style='color:blue;'>" + item.HandDays + "</span>天)"
            }
            },
            { display: '投稿时间', name: 'AddDate', width: '130', align: 'center', render: function (item) {
                var date = eval('new ' + item.AddDate.substr(1, item.AddDate.length - 2));
                return date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + "(距今<span style='color:blue;'>" + item.Days + "</span>天)"
            }
            },
            { display: '备注', name: 'Remark', width: '100', align: 'center', render: function (item) {
                if (item.Remark != "") {
                    return "<a href='javascript:alert(\"" + item.Remark + "\")' title=\"" + item.Remark + "\">" + item.Remark + "</a>";
                }
            }
            },
            { display: '其他标记', name: 'IsRetractions', width: '70', align: 'center', render: function (item) {
                var title = "";
                if (item.IsRetractions) {
                    title = "<a href=\"javascript:void(0);\" style=\"color:red;\" onclick=\"ViewRetractions(" + item.CID + ")\"><b>处理撤稿?</b></a>";
                }
                return title;
            }
            }

          ];
    }
    contributiongrid.set('columns', columns);
    contributiongrid.setOptions({ parms: {
        CNumber: $("#txtCNumber").val(),
        Title: $("#txtTitle").val(),
        Keyword: $("#txtKeyword").val(),
        StatusID: $("#hiddenStatus").val(),
        IsHandled: $("#hiddenWorkStatus").val(),
        FirstAuthor: $("#txtAuthor").val(),
        AuthorID: $("#hiddenInAuthor").val(),
        StartDate: $("#txtStartDate").val(),
        EndDate: $("#txtEndDate").val(),
        Flag: $("#selflag").val()
    }
    });
    contributiongrid.loadData();
}
// 载入稿件状态
function LoadStatusList() {

    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/Contribution/GetHaveRightFlowStatus/?rnd=' + Math.random(),
        data: { WorkStatus: $("#hiddenWorkStatus").val() },
        success: function (e) {
            if (e.result == 'success') {
                $("div .l-menu-inner").first().html("");

                for (var i = 0; i < e.ItemList.length; i++) {
                    $("div .l-menu-inner").first().append('<div class="l-menu-item" onclick="StatusClick(' + e.ItemList[i].StatusID + ',\'' + e.ItemList[i].StatusName + '\',' + e.ItemList[i].CStatus + ')"><div class="l-menu-item-text">' + e.ItemList[i].StatusName + '(' + e.ItemList[i].ContributionCount + ')</div></div>');
                }
                $("div .l-menu-inner:first .l-menu-item").each(function (i) {
                    $(this).hover(function () {
                        var itemtop = $(this).offset().top;
                        var top = itemtop - 137;
                        $("div .l-menu-over:first").css('top', top);
                    });
                })
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

// 点击某个状态后
function StatusClick(StatusID, StatusName, CStatus) {
    $("#hiddenStatus").val(StatusID);
    Search(CStatus);
    // 载入当前状态下的可做操作及消息通知
    LoadActionList(StatusID, CStatus);
    $("#spanCurStatus").text("当前状态：" + StatusName);
}

// 载入当前状态可做操作
function LoadActionList(StatusID, CStatus) {
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/Contribution/GetFlowActionByStatus/?rnd=' + Math.random(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ query: { "StatusID": StatusID} }),
        success: function (e) {
            if (e.result == 'success') {
                $("div .l-menu-inner").eq(1).html(""); // 审稿操作
                $("div .l-menu-inner").eq(2).html(""); // 消息通知
                var isHaveMsgAction = false; // 是否有消息通知操作

                for (var i = 0; i < e.ItemList.length; i++) {
                    // 审稿操作
                    if (parseInt(e.ItemList[i].ActionType) != 3) {
                        $("div .l-menu-inner").eq(1).append('<div class="l-menu-item" onclick="FlowAction(' + e.ItemList[i].ActionID + ',' + StatusID + ')"><div class="l-menu-item-text">' + e.ItemList[i].ActionName + '</div></div>');
                    }
                    else {
                        isHaveMsgAction = true;
                        $("div .l-menu-inner").eq(2).append('<div class="l-menu-item" onclick="SendMessage(' + e.ItemList[i].ActionID + ',' + StatusID + ')"><div class="l-menu-item-text">' + e.ItemList[i].ActionName + '</div></div>');
                    }
                }
                if (CStatus == 200) {
                    $("div .l-menu-inner").eq(1).append('<div class="l-menu-item" onclick="SetYearIssue()"><div class="l-menu-item-text">设置年卷期</div></div>');
                }
                $("div .l-menu-inner:eq(1) .l-menu-item").each(function (i) {
                    $(this).hover(function () {
                        var itemtop = $(this).offset().top;
                        var top = itemtop - 137;
                        $("div .l-menu-over:first").css('top', top);
                    });
                })
                $("div .l-menu-inner:eq(2) .l-menu-item").each(function (i) {
                    $(this).hover(function () {
                        var itemtop = $(this).offset().top;
                        var top = itemtop - 137;
                        $("div .l-menu-over:first").css('top', top);
                    });
                })
                if (!isHaveMsgAction) {
                    $("#divTopmenu .l-menubar-item:eq(2)").hide();
                }
                else {
                    $("#divTopmenu .l-menubar-item:eq(2)").show();
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

// 工作状态
function WorkActionClick(item) {
    var WorkStatus = 2;
    if (item.text == "全部") {
        WorkStatus = 2;
        $("#spanWorkStatus").text("全部稿件");
    }
    else if (item.text == "已处理") {
        WorkStatus = 1;
        $("#spanWorkStatus").text("已处理稿件");
    }
    else {
        WorkStatus = 0;
        $("#spanWorkStatus").text("待处理稿件")
    }
    $("#hiddenWorkStatus").val(WorkStatus);
    // 载入当前可以操作的审稿状态
    LoadStatusList();
    //contributiongrid.clearGrid();
    $("#spanCurStatus").text('');
}

// 标记
function TagClick(item) {
    var tagtype = item.text;
    switch (tagtype) {
        case "红旗":
            SetContributionFlag("redflag");
            break;
        case "绿旗":
            SetContributionFlag("greenflag");
            break;
        case "橙旗":
            SetContributionFlag("orangeflag");
            break;
        case "蓝旗":
            SetContributionFlag("blueflag");
            break;
        case "粉旗":
            SetContributionFlag("pinkflag");
            break;
        case "青旗":
            SetContributionFlag("cyanflag");
            break;
        case "黄旗":
            SetContributionFlag("yellowflag");
            break;
        case "紫旗":
            SetContributionFlag("purpleflag");
            break;
        case "灰旗":
            SetContributionFlag("grayflag");
            break;
        case "取消旗帜":
            SetContributionFlag("");
            break;
        case "加急稿件":
            SetContributionQuick(true);
            break;
        case "取消加急":
            SetContributionQuick(false);
            break;
    }
}

// 设置稿件旗帜
function SetContributionFlag(Flag) {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择要设置的稿件'); return; }
    var cArray = new Array();
    for (i = 0; i < rows.length; i++) {
        var cObj = { "CID": rows[i].CID, "Flag": Flag };
        cArray.push(cObj);
    }
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/Contribution/SetContributionFlagAjax/?rnd=' + Math.random(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ cEntityList: cArray }),
        success: function (e) {
            if (e.result == 'success') {
                alert('设置成功');
                contributiongrid.loadData();
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

// 设置稿件加急标志
function SetContributionQuick(IsQuick) {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择要设置的稿件'); return; }
    var cArray = new Array();
    for (i = 0; i < rows.length; i++) {
        var cObj = { "CID": rows[i].CID, "IsQuick": IsQuick };
        cArray.push(cObj);
    }
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/Contribution/SetContributionQuickAjax/?rnd=' + Math.random(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ cEntityList: cArray }),
        success: function (e) {
            if (e.result == 'success') {
                alert('设置成功');
                contributiongrid.loadData();
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

// 其他操作
function OtherActionClick(item) {
    var commandtype = item.text;
    switch (commandtype) {
        case "学术不端检测(中国知网)":
            window.open('http://check.cnki.net/amlc2/');
            break;
	    case "学术不端检测(万方数据)":
            window.open('http://check.wanfangdata.com.cn/');
            break;
        case "相似文献查询":
            var key = "http://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=" + encodeURIComponent(contributiongrid.getSelectedRows()[0].Title) + "&rsv_pq=ac9e09ba00002f4f&rsv_t=ccd81Dw%2B564K2uWrLcRRfFUpEEgFevKP9SCchx5Lgdq%2BSDBhJ3zFv23HLs8&rsv_enter=1&rsv_sug3=7&rsv_sug4=81&rsv_sug1=6&rsv_sug2=0&inputT=2934";
            window.open(key);
            break;
        case "稿签打印":
            PrintSign();
            break;
        case "审稿单打印":
            PrintAuditBill();
            break;
        case "邮寄信封打印":
            PrintEnvelope();
            break;
        case "导出Excel":
            ImportExcel();
            break;
        case "合并稿件状态":
            MergeStatus();
            break;
        case "稿件送排":
            SendTypesetting();
            break;
        case "继续送交":
            ContinueSend();
            break;
        case "继续送复审":
            ContinueReSend();
            break;
    }
}

// 稿签打印
function PrintSign() {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择需要打印稿签的稿件！'); return; }
    var CIDS = '';
    for (i = 0; i < rows.length; i++) {
        CIDS += rows[i].CID + ",";
    }
    window.open(RootPath + '/Contribution/PrintSign?CIDS=' + CIDS + '&rnd=' + Math.random(), "稿签打印", "");
//    $.ligerDialog.open({ height: 500, url: , title: '稿签打印', width: 700, slide: false, buttons: [
//        { text: '确定', onclick: function (item, dialog) {
//            dialog.frame.EnvelopePrint();
//        }
//        },
//        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
//        ]
//    });
}

// 打印审稿单
function PrintAuditBill() {
    var rows = contributiongrid.getSelectedRows();
    var CIDArray = [];
    if (rows == "") { alert('请选择要打印审稿单的稿件！'); return; }
    var selCID = rows[0].CID;
    window.open(RootPath + '/FlowSet/CreateReviewBillContent?CID=' + selCID + '&IsView=true&rnd=' + Math.random(), "打印审稿单", "");
}

// 导出EXCEL
function ImportExcel() {
    if ($(".l-grid-row").length < 1) {
        alert('没有数据不能导出，请先进行查询！')
        return;
    }
    if (confirm('确认要导出当前稿件列表吗？')) {
        html = "";
        html += '<input type="hidden" name="CNumber" value="' + $("#txtCNumber").val() + '"/>';
        html += '<input type="hidden" name="Title" value="' + $("#txtTitle").val() + '"/>';
        html += '<input type="hidden" name="Keyword" value="' + $("#txtKeyword").val() + '"/>';
        html += '<input type="hidden" name="StatusID" value="' + $("#hiddenStatus").val() + '"/>';
        html += '<input type="hidden" name="FirstAuthor" value="' + $("#txtAuthor").val() + '"/>';
        html += '<input type="hidden" name="AuthorID" value="' + $("#hiddenInAuthor").val() + '"/>';
        html += '<input type="hidden" name="StartDate" value="' + $("#txtStartDate").val() + '"/>';
        html += '<input type="hidden" name="EndDate" value="' + $("#txtEndDate").val() + '"/>';
        html += '<input type="hidden" name="Flag" value="' + $("#selflag").val() + '"/>';
     
        $('form[name="action_command"]').html(html).attr("action", RootPath + '/Contribution/ContributionRenderToExcel/').attr("method", "post").submit();
    }
}

// 审稿操作
function FlowAction(ActionID, StatusID) {
    var rows = contributiongrid.getSelectedRows();
    var AuthorID = rows[0].AuthorID;

    var CIDArray = [];
    if (rows == "") { alert('请选择需要审核的稿件！'); return; }
    var str = RootPath + "/FlowSet/ValidateIsPayNotice?actionID=" + ActionID + "&rnd=" + Math.random();
    $.ajax({
        type: "POST",
        url: str,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.flag=="1") {
                BatchCheck(rows, data.payType);
                LoadStatusList(); // 更新稿件状态
            }
            else if (data.flag == "2") {
                FContributeCreate(rows, data.payType);
                LoadStatusList(); // 更新稿件状态
            }
            else {
                for (i = 0; i < rows.length; i++) {
                    CIDArray.push(rows[i].CID + ":" + rows[i].FlowLogID);
                }
                window.parent.f_addTab('AuditBill', '执行审稿操作', RootPath + '/FlowSet/AuditBill?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&CGrid" + contributiongrid + "&AuthorID=" + AuthorID + "&rnd=" + Math.random());
                LoadStatusList(); // 更新稿件状态
            }
        },
        error: function (data) {
            alert(data);
        }
    });
    //    $.ligerDialog.open({ height: 800, url: RootPath + '/FlowSet/AuditBill?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&rnd=" + Math.random(), title: '执行审稿操作', width: 1020, slide: false, buttons: [
    //        { text: '确定', onclick: function (item, dialog) {
    //            if (confirm('您确定要提交吗？')) {
    //                dialog.frame.SubmitAuditBill(contributiongrid, dialog);
    //                LoadStatusList(); // 更新稿件状态
    //            }
    //        }
    //        },
    //        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
    //        ]
    //    });
}
//通知交审稿费/版面费对话框
function BatchCheck(rows, payType) {
    if (rows.length == 1) {
        $.ligerDialog.open({
            height: 500,
            width: 700,
            url: RootPath + '/Finance/PayNotice?PayType=' + payType + '&NoticeID=0&CID=' + rows[0].CID,
            title: '交费通知单',
            slide: false,
            buttons: [{
                text: '确认', onclick: function (item, dialog) {
                    dialog.frame.Save(contributiongrid, dialog, rows[0].AuthorID);
                    contributiongrid.loadData();
                }
            }, { text: '关闭', onclick: function (item, dialog) { dialog.close(); } }]
        });
    }
    else {
        var array = "";
        var users = "";
        for (var i = 0; i < rows.length; i++) {
            var para = payType + ",0," + rows[i].CID + "," + rows[i].AuthorID;
            if (i == (rows.length - 1)) {
                array += para;
            }
            else {
                array += para + "|";
            }
        }
        $.ligerDialog.open({
            height: 500,
            width: 700,
            url: RootPath + '/Finance/BatchPayNotice?Array=' + array,
            title: '批量交费通知单',
            slide: false,
            buttons: [{
                text: '确认', onclick: function (item, dialog) {
                    dialog.frame.BatchSave(contributiongrid, dialog, users);
                    contributiongrid.loadData();
                }
            }, { text: '关闭', onclick: function (item, dialog) { dialog.close(); } }]
        });
    }

}
//入款审稿费/版面费对话框
function FContributeCreate(rows, payType) {
    if (rows.length > 1) {
        alert("不支持批量入款操作！");
        return;
    }
    $.ligerDialog.open({
        height: 450,
        width: 600,
        url: RootPath + '/Finance/FContributeCreate?FeeType=' + payType + '&PKID=0',
        title: '入款',
        slide: false,
        buttons: [{
            text: '确认', onclick: function (item, dialog) {
                dialog.frame.Save(contributiongrid, dialog, rows[0].CID, rows[0].NoticeID);
                contributiongrid.loadData();
            }
        }, { text: '关闭', onclick: function (item, dialog) { dialog.close(); } }]
    });
}

// 消息通知
function SendMessage(ActionID, StatusID) {
    var rows = contributiongrid.getSelectedRows();
    var CIDArray = [];
    if (rows == "") { alert('请选择稿件！'); return; }
    for (i = 0; i < rows.length; i++) {
        CIDArray.push(rows[i].CID + ":" + rows[i].FlowLogID);
    }

    window.parent.f_addTab('SendMessage', '发送消息通知', RootPath + '/FlowSet/SendMessage?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&rnd=" + Math.random());


//    $.ligerDialog.open({ height: 620, url: RootPath + '/FlowSet/SendMessage?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&rnd=" + Math.random(), title: '消息通知操作', width: 800, slide: false, buttons: [
//        { text: '确定', onclick: function (item, dialog) {
//            dialog.frame.SendSysMessage(contributiongrid, dialog);
//        }
//        },
//        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
//        ]
//    });
}

// 打印信封
function PrintEnvelope() {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择需要打印的稿件！'); return; }
    var authorID = '';
    for (i = 0; i < rows.length; i++) {
        authorID += rows[i].AuthorID + ",";
    }
    $.ligerDialog.open({ height: 400, url: RootPath + '/Contribution/PrintEnvelope?AuthorIDStr=' + authorID + '&rnd=' + Math.random(), title: '邮寄信封打印', width: 600, slide: false, buttons: [
        { text: '确定', onclick: function (item, dialog) {
            dialog.frame.EnvelopePrint();
            //dialog.close();
        }
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}

// 查看撤稿申请
function ViewRetractions(CID) {
    $.ligerDialog.open({ height: 400, url: RootPath + '/Contribution/ViewRetraction?CID=' + CID + '&rnd=' + Math.random(), title: '撤稿申请处理', width: 600, slide: false, buttons: [
        { text: '同意', onclick: function (item, dialog) {
            $.ajax({
                beforeSend: function () {

                },
                type: 'POST',
                url: RootPath + '/Contribution/DealWithdrawal/?rnd=' + Math.random(),
                data: { CID: CID },
                success: function (data) {
                    var e = eval("(" + data + ")");
                    if (e.result == 'success') {
                        contributiongrid.loadData();
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
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}

function ViewDetail(cID, cNumber, StatusID, FlowLogID,AuthorID) {
    if (cID.text != undefined) {
        var rows = contributiongrid.getSelectedRows();
        if (rows == "") { alert('请选择需要查看的稿件！'); return; }
        cID = rows[0].CID;
        cNumber = rows[0].cNumber;
    }
    window.parent.f_addTab('View' + cNumber, '稿件详细信息[' + cNumber + ']', RootPath + '/ContributionInfo/ViewDetail?CID=' + cID + "&StatusID=" + StatusID + "&FlowLogID=" + FlowLogID + "&AuthorID=" +AuthorID+ "&rnd=" + Math.random());
}

// 删除稿件
function DelContribution() {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择要删除的稿件'); return; }
    if (!confirm('此稿件将在整个审稿系统中(包含专家与作者平台)隐藏并不可处理!\r\n删除后您可以在稿件搜索中查找到此稿件(以灰色显示)并可以撤销其删除状态\r\n您确认要删除选择的稿件吗？')) {
        return false;
    }
    var cArray = new Array();
    for (i = 0; i < rows.length; i++) {
        var cObj = { "CID": rows[i].CID };
        cArray.push(cObj);
    }
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/Contribution/DeleteContributionAjax/?rnd=' + Math.random(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ cEntityList: cArray }),
        success: function (e) {
            if (e.result == 'success') {
                alert('删除成功');
                contributiongrid.loadData();
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

// 确认已交介绍信
function ConfirmIntro(CID, flag) {
    var msg = "您确认要设置稿件为已交标记吗？";
    if (flag == 0) {
        msg = "您确认要设置稿件为未交标记吗？";
    }
    if (confirm(msg)) {
        $.ajax({
            beforeSend: function () {

            },
            type: 'POST',
            url: RootPath + '/Contribution/SetIntroLetter/?rnd=' + Math.random(),
            data: { CID: CID, flag: flag },
            success: function (data) {
                var e = eval("(" + data + ")");
                if (e.result == 'success') {
                    alert('设置成功');
                    contributiongrid.loadData();
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

//获取所有备注
function AllRemarkDetails() {
  
}


// 设置年卷期
function SetYearIssue() {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择要设置的稿件'); return; }
    var CID = rows[0].CID;
    var AuthorID = rows[0].AuthorID;
    $.ligerDialog.open({ height: 300, url: RootPath + '/Contribution/SetYearIssue?rnd=' + Math.random(), title: '设置年卷期', width: 400, slide: false, buttons: [
        { text: '确定', onclick: function (item, dialog) {
            Year = dialog.frame.Year;
            if (Year == 0) {
                alert('请选择年');
                return false;
            }
            Volume = dialog.frame.Volume;
            Issue = dialog.frame.Issue;
            if (Issue == 0) {
                alert('请选择期');
                return false;
            }
            JChannelID = dialog.frame.JChannelID;
//            if (JChannelID == 0) {
//                alert('请选择栏目');
//                return false;
//            }
            $.ajax({
                beforeSend: function () {

                },
                type: 'POST',
                url: RootPath + '/Contribution/SetYearVolumnIssueAjax/?rnd=' + Math.random(),
                data: { CID: CID, AuthorID: AuthorID, Year: Year, Volume: Volume, Issue: Issue, JChannelID: JChannelID },
                success: function (data) {
                    var e = eval("(" + data + ")");
                    if (e.result == 'success') {
                        Search(200);
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
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}

// 合并稿件状态
function MergeStatus() {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择要合并的稿件'); return; }
    if (rows.length > 1) {
        alert('只能选择一篇稿件进行状态合并'); return;
    }
    var CID = rows[0].CID;
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/Contribution/JudgeIsMoreStatusAjax/?rnd=' + Math.random(),
        data: { CID: CID },
        success: function (data) {
            var e = eval("(" + data + ")");
            if (e.result == 'success') {
                $.ligerDialog.open({ height: 400, url: RootPath + '/Contribution/MergeStatus?CID=' + CID + '&rnd=' + Math.random(), title: '合并稿件状态', width: 500, slide: false, buttons: [
                { text: '确定', onclick: function (item, dialog) {
                    FlowLogID = dialog.frame.FlowLogID;
                    if (FlowLogID == 0) {
                        alert('请选择要合并的主状态');
                        return false;
                    }
                    $.ajax({
                        beforeSend: function () {

                        },
                        type: 'POST',
                        url: RootPath + '/Contribution/MergeStatusAjax/?rnd=' + Math.random(),
                        data: { CID: CID, FlowLogID: FlowLogID },
                        success: function (data) {
                            var e = eval("(" + data + ")");
                            if (e.result == 'success') {
                                alert('合并成功');
                                Search();
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
                },
                { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ]
                });
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

function SendTypesetting() {
    var rows = contributiongrid.getSelectedRows();
    if (rows == "") { alert('请选择要送排的稿件'); return; }
    var CID = rows[0].CID;
    $.ajax({
        beforeSend: function () {

        },
        type: 'POST',
        url: RootPath + '/FangzApi/SingleSendTypesetting/?rnd=' + Math.random(),
        data: { CID: CID },
        success: function (data) {
            var e = eval("(" + data + ")");
            if (e.result == 'success') {
                alert('送排成功');
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

// 继续送交
function ContinueSend() {
    var StatusID = $("#hiddenStatus").val();
    FlowAction(0, StatusID);
}
// 继续送专家复审
function ContinueReSend() {
    var StatusID = $("#hiddenStatus").val();
    FlowAction(-1, StatusID);
}