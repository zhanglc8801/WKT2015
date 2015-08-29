var OtherAction = { width: 130, items:
            [
            { text: '学术不端检测', click: OtherActionClick, icon: 'search' },
            { text: '相似文献查询', click: OtherActionClick, icon: 'search2' }
            ]
};
var NullAction = { width: 160, items:
            [
            { text: '' }
            ]
};
var TagAction = {
    width: 120, items:
            [
            { text: '红旗', click: TagClick, icon: 'redflag' },
            { text: '其他旗帜', icon: 'whiteflag', children: [
                { text: '绿旗', click: TagClick, icon: 'greenflag' },
                { text: '橙旗', click: TagClick, icon: 'orangeflag' },
                { text: '蓝旗', click: TagClick, icon: 'blueflag' },
                { text: '粉旗', click: TagClick, icon: 'pinkflag' },
                { text: '青旗', click: TagClick, icon: 'cyanflag' },
                { text: '黄旗', click: TagClick, icon: 'yellowflag' },
                { text: '紫旗', click: TagClick, icon: 'purpleflag' },
                { text: '灰旗', click: TagClick, icon: 'grayflag' }
             ]
            },
            { text: '取消旗帜', click: TagClick },
            { line: true },
            { text: '加急稿件', click: TagClick, icon: 'up' },
            { text: '取消加急', click: TagClick, icon: 'busy' }
            ]
};

$(function () {
    // 菜单
    $("#divTopmenu").ligerMenuBar({ items: [
                { text: '稿件流程操作', menu: NullAction },
                { text: '消息通知操作', menu: NullAction },
                { text: '其他相关操作', menu: OtherAction },
                { text: '标记为', menu: TagAction }
            ]
    });
    LoadActionList(CStatusID);
});

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
    var cArray = new Array();
    var cObj = { "CID": CID, "Flag": Flag };
    cArray.push(cObj);
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
    var cArray = new Array();
    var cObj = { "CID": CID, "IsQuick": IsQuick };
    cArray.push(cObj);
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
        case "学术不端检测":
            window.open('http://check.cnki.net/');
            break;
        case "相似文献查询":
            window.open('http://www.baidu.com/');
            var key = "http://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=" + encodeURIComponent(""+Title+"") + "&rsv_pq=ac9e09ba00002f4f&rsv_t=ccd81Dw%2B564K2uWrLcRRfFUpEEgFevKP9SCchx5Lgdq%2BSDBhJ3zFv23HLs8&rsv_enter=1&rsv_sug3=7&rsv_sug4=81&rsv_sug1=6&rsv_sug2=0&inputT=2934";
            window.open(key);
            break;
    }
}

// 载入当前状态可做操作
function LoadActionList(StatusID) {
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
                $("div .l-menu-inner").eq(0).html(""); // 审稿操作
                $("div .l-menu-inner").eq(1).html(""); // 消息通知
                var isHaveMsgAction = false; // 是否有消息通知操作

                for (var i = 0; i < e.ItemList.length; i++) {
                    // 审稿操作
                    if (parseInt(e.ItemList[i].ActionType) != 3) {
                        $("div .l-menu-inner").eq(0).append('<div class="l-menu-item" onclick="FlowAction(' + e.ItemList[i].ActionID + ',' + StatusID + ')"><div class="l-menu-item-text">' + e.ItemList[i].ActionName + '</div></div>');
                    }
                    else {
                        isHaveMsgAction = true;
                        $("div .l-menu-inner").eq(1).append('<div class="l-menu-item" onclick="SendMessage(' + e.ItemList[i].ActionID + ',' + StatusID + ')"><div class="l-menu-item-text">' + e.ItemList[i].ActionName + '</div></div>');
                    }
                }
                
                $("div .l-menu-inner:eq(0) .l-menu-item").each(function (i) {
                    $(this).hover(function () {
                        var itemtop = $(this).offset().top;
                        var top = itemtop - 137;
                        $("div .l-menu-over:first").css('top', top);
                    });
                })
                $("div .l-menu-inner:eq(1) .l-menu-item").each(function (i) {
                    $(this).hover(function () {
                        var itemtop = $(this).offset().top;
                        var top = itemtop - 137;
                        $("div .l-menu-over:first").css('top', top);
                    });
                })
                if (!isHaveMsgAction) {
                    $("#divTopmenu .l-menubar-item:eq(1)").hide();
                }
                else {
                    $("#divTopmenu .l-menubar-item:eq(1)").show();
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

// 审稿操作
function FlowAction(ActionID, StatusID) {
    var ActionType = 0;
    var CIDArray = [];
    CIDArray.push(CID + ":" + FlowLogID);
    var AuthorID = getQueryString("AuthorID");
    //window.parent.f_addTab('AuditBill', '执行审稿操作', RootPath + '/FlowSet/AuditBill?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&rnd=" + Math.random());
  //====
    var str = RootPath + "/FlowSet/ValidateIsPayNotice?actionID=" + ActionID + "&rnd=" + Math.random();
    $.ajax({
        type: "POST",
        url: str,
        data: "{}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.flag == "1") {
                PayNotice(AuthorID, data.payType);
                //LoadStatusList(); // 更新稿件状态
            }
            else if (data.flag == "2") {
                FContributeCreate(CID, data.payType);
                //LoadStatusList(); // 更新稿件状态
            }
            else {
                
                window.parent.f_addTab('AuditBill', '执行审稿操作', RootPath + '/FlowSet/AuditBill?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&AuthorID="+AuthorID+"&rnd=" + Math.random());
                //LoadStatusList(); // 更新稿件状态
            }
        },
        error: function (data) {
            alert(data);
        }
    });

//    $.ligerDialog.open({ height: 620, url: RootPath + '/FlowSet/AuditBill?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&rnd=" + Math.random(), title: '执行审稿操作', width: 800, slide: false, buttons: [
//        { text: '确定', onclick: function (item, dialog) {
//            dialog.frame.SubmitAuditBill(null, dialog);
//            InitFlowLog();
//        }
//        },
//        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
//        ]
//    });
}

//通知交审稿费/版面费对话框
function PayNotice(authorid, payType) {
    //http://localhost:2901/Finance/PayNotice?PayType=1&NoticeID=0&CID=34
    $.ligerDialog.open({
        height: 450,
        width: 600,
        url: RootPath + '/Finance/PayNotice?PayType=' + payType + '&NoticeID=0&CID=' + CID,
        title: '交费通知单',
        slide: false,
        buttons: [{ text: '确认', onclick: function (item, dialog) {
            dialog.mask();
            dialog.frame.Save(null, dialog, authorid);
            //contributiongrid.loadData();
        }
        }, { text: '关闭', onclick: function (item, dialog) { dialog.close(); } }]
    });
}
//入款审稿费/版面费对话框
function FContributeCreate(cid, payType) {
    //http://localhost:2901/Finance/FContributeCreate?FeeType=2&PKID=0
    $.ligerDialog.open({
        height: 450,
        width: 600,
        url: RootPath + '/Finance/FContributeCreate?FeeType=' + payType + '&PKID=0',
        title: '入款',
        slide: false,
        buttons: [{ text: '确认', onclick: function (item, dialog) {
            dialog.frame.Save(null, dialog, cid, 0);
            //contributiongrid.loadData();
        }
        }, { text: '关闭', onclick: function (item, dialog) { dialog.close(); } }]
    });
}

// 消息通知
function SendMessage(ActionID,StatusID) {
    var CIDArray = [];
    CIDArray.push(CID + ":" + FlowLogID);
    window.parent.f_addTab('SendMessage', '发送消息通知', RootPath + '/FlowSet/SendMessage?ActionID=' + ActionID + "&StatusID=" + StatusID + "&CIDS=" + CIDArray.join() + "&rnd=" + Math.random());

    
}