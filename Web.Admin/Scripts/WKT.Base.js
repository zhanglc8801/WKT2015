var Browser = {
    'isIE': (navigator.userAgent.indexOf('MSIE') >= 0) && (navigator.userAgent.indexOf('Opera') < 0),
    'isFirefox': navigator.userAgent.indexOf('Firefox') >= 0,
    'isOpera': navigator.userAgent.indexOf('Opera') >= 0
};
var CommonRootPath = "";
function GetBool(val) {
    if (val == "True") {
        return true;
    }
    else {
        return false;
    }
}

function resizeImage(obj, MaxW, MaxH) {
    var imageObject = obj;
    var stateIE = imageObject.readyState;
    var stateFF = imageObject.complete;
    if (Browser.isIE) {
        if (stateIE != "complete") {
            setTimeout("resizeImage(" + imageObject + "," + MaxW + "," + MaxH + ")", 50);
            return;
        }
    } else {
        if (!stateFF) {
            setTimeout("resizeImage(" + imageObject + "," + MaxW + "," + MaxH + ")", 50);
            return;
        }
    }
    var oldImage = new Image();
    oldImage.src = imageObject.src;
    var dW = oldImage.width;
    var dH = oldImage.height;
    if (dW > MaxW || dH > MaxH) {
        a = dW / MaxW; b = dH / MaxH;
        if (b > a) a = b;
        dW = dW / a; dH = dH / a;
    }
    if (dW > 0 && dH > 0) {
        imageObject.width = dW;
        imageObject.height = dH;
    }
}

function InitNoneImage(obj) {
    obj.src = CommonRootPath + "/Content/img/none1.jpg";
}

function GetNoneImage(url) {
    if (url.length < 1)
        return CommonRootPath + "/Content/img/none1.jpg";
    return url;
}

function GetDLLText(id, value) {
    var val = '';
    $("#" + id).children("option").each(function () {
        if ($(this).val() == value) {
            val = $(this).text();
            return;
        }
    });
    return val;
}

function CommonPage() {
    this.title = "";
    this.url = "";
}
// 选择编辑部成员
CommonPage.prototype.SelMember = function (txtControlID, valueControlID) {
    $.ligerDialog.open({ height: 420, url: CommonRootPath + '/member/seldialog', title: '选择成员', width: 580, slide: false, buttons: [
        { text: '确定', onclick: function (item, dialog) {
            rows = dialog.frame.memberSelDialog.getSelectedRows();
            if (rows == "") { alert('请选择成员'); return; }
            if ($("#" + txtControlID).is("input")) {
                $("#" + txtControlID).val(rows[0].RealName);
            }
            else {
                $("#" + txtControlID).text(rows[0].RealName);
            }
            $("#" + valueControlID).val(rows[0].AuthorID);
            dialog.close();
        }
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}
// 选择作者
CommonPage.prototype.SelAuthor = function (txtControlID, valueControlID) {
    $.ligerDialog.open({ height: 420, url: CommonRootPath + '/author/seldialog', title: '选择作者', width: 580, slide: false, buttons: [
        { text: '确定', onclick: function (item, dialog) {
            rows = dialog.frame.memberSelDialog.getSelectedRows();
            if (rows == "") { alert('请选择作者'); return; }
            if ($("#" + txtControlID).is("input")) {
                $("#" + txtControlID).val(rows[0].LoginName);
            }
            else {
                $("#" + txtControlID).text(rows[0].LoginName);
            }
            $("#" + valueControlID).val(rows[0].AuthorID);
            dialog.close();
        }
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}
// 选择专家
CommonPage.prototype.SelExpert = function (containerID) {
    $.ligerDialog.open({ height: 500, url: CommonRootPath + '/expert/seldialog', title: '选择专家', width: 760, slide: false, buttons: [
        { text: '确定', onclick: function (item, dialog) {
            rows = dialog.frame.checkedExpert;
            if (rows == "") { alert('请选择专家'); return; }
            for (i = 0; i < rows.length; i++) {
                $("#" + containerID).append('<div class="fsg_hy2" name="divSelExpert" id="divSelUserDiv_' + rows[i].AuthorID + '">' + rows[i].AuthorName + '&nbsp;&nbsp;<a onclick="SelExpertDelete(\'' + rows[i].AuthorID + '\');" style="cursor: pointer;">x</a></div>');
            }
            dialog.close();
        }
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}

// 选择英文专家
CommonPage.prototype.SelEnExpert = function (containerID) {
    $.ligerDialog.open({ height: 500, url: CommonRootPath + '/expert/seldialogen', title: '选择专家', width: 760, slide: false, buttons: [
        { text: '确定', onclick: function (item, dialog) {
            rows = dialog.frame.checkedExpert;
            if (rows == "") { alert('请选择专家'); return; }
            for (i = 0; i < rows.length; i++) {
                $("#" + containerID).append('<div class="fsg_hy2" name="divSelExpert" id="divSelUserDiv_' + rows[i].AuthorID + '">' + rows[i].AuthorName + '&nbsp;&nbsp;<a onclick="SelExpertDelete(\'' + rows[i].AuthorID + '\');" style="cursor: pointer;">x</a></div>');
            }
            dialog.close();
        }
        },
        { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
        ]
    });
}

function SelExpertDelete(ExpertID) {
    jQuery("#divSelUserDiv_" + ExpertID).remove();
}

// 选择作者
CommonPage.prototype.AutoAuthor = function (txtControlID, valueControlID) {
    $('#' + txtControlID).bind("input.autocomplete", function () {
        $(this).trigger('keydown.autocomplete');
    });
    $('#' + txtControlID).autocomplete({
        source: function (request, response) {
            //define a function to call your Action (assuming UserController)
            $.ajax({
                url: CommonRootPath + '/Author/AuthorSearchList', type: "GET", dataType: "json",
                //query will be the param used by your action method
                data: { RealName: encodeURIComponent($('#' + txtControlID).val()) },
                term: extractLast(request.term),
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.RealName, value: item.AuthorID };
                    }))
                }
            })
        },
        search: function () {
            // custom minLength
            var term = extractLast(this.value);
            if (term.length < 1) {
                if (valueControlID != "") {
                    $("#" + valueControlID).val('');
                }
                return false;
            }
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (event, ui) {
            this.value = ui.item.label;
            if (valueControlID != "") {
                $("#" + valueControlID).val(ui.item.value);
            }
            return false;
        }
    });
}
function split(val) {
    return val.split(/,\s*/);
}
function extractLast(term) {
    return split(term).pop();
}
//日期格式化
Date.prototype.pattern = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份        
        "d+": this.getDate(), //日        
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时        
        "H+": this.getHours(), //小时        
        "m+": this.getMinutes(), //分        
        "s+": this.getSeconds(), //秒        
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度        
        "S": this.getMilliseconds() //毫秒        
    };
    var week = {
        "0": "\u65e5",
        "1": "\u4e00",
        "2": "\u4e8c",
        "3": "\u4e09",
        "4": "\u56db",
        "5": "\u4e94",
        "6": "\u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "\u661f\u671f" : "\u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
};

function GetDate(obj, format) {
    if (obj == null || obj.length < 1) return '';
    var jsonDate = obj.replace(/\/Date\((\d+)\)\//gi, "$1");
    return (new Date(jsonDate * 1)).pattern(format);
}

function GetWeek(obj) {
    if (obj == null || obj.length < 1) return '';
    var jsonDate = obj.replace(/\/Date\((\d+)\)\//gi, "$1");
    var week = (new Date(jsonDate * 1)).getDay();
    var weekstr = "";
    switch (week) {
        case 1:
            weekstr = "一";
            break;
        case 2:
            weekstr = "二";
            break;
        case 3:
            weekstr = "三";
            break;
        case 4:
            weekstr = "四";
            break;
        case 5:
            weekstr = "五";
            break;
        case 6:
            weekstr = "六";
            break;
        case 0:
            weekstr = "日";
            break;
    }
    return weekstr;
}

function MoneyConvert(money, isprecision) {
    money = decimal(money, 2);
    var s = money; //获取小数型数据
    s += "";
    if (s.indexOf(".") == -1) s += ".0"; //如果没有小数点，在后面补个小数点和0
    if (/\.\d$/.test(s)) s += "0";   //正则判断
    while (/\d{4}(\.|,)/.test(s))  //符合条件则进行替换
    {
        s = s.replace(/(\d)(\d{3}(\.|,))/, "$1,$2"); //每隔3位添加一个
    }
    if (isprecision == undefined || !isprecision) {// 不带小数位
        var a = s.split(".");
        if (a[1] == "00") {
            s = a[0];
        }
    }
    return s;
}

function MoneyFormat(money) {
    money = decimal(money, 2);
    var s = money; //获取小数型数据
    s += "";
    if (s.indexOf(".") == -1) s += ".0"; //如果没有小数点，在后面补个小数点和0
    if (/\.\d$/.test(s)) s += "0";   //正则判断
    return s;
}

function decimal(num, v) {
    var vv = Math.pow(10, v);
    return Math.round(num * vv) / vv;
}

function DownLoad(basePath, path, fileName) {
    var browserMatch = uaMatch(userAgent.toLowerCase());
    if (browserMatch.browser) {
        browser = browserMatch.browser;
        version = browserMatch.version;
    }
    $.ligerDialog.open({
        height: 300,
        width: 400,
        url: basePath + '/Upload/Index?path=' + encodeURIComponent(path) + '&fileName=' + encodeURIComponent(fileName) + "&Browser=" + browser,
        title: '附件',
        slide: false,
        buttons: [{ text: '关闭', onclick: function (item, dialog) { dialog.close(); } }]
    });
}

//获取浏览器版本
var userAgent = navigator.userAgent,
				rMsie = /(msie\s|trident.*rv:)([\w.]+)/,
				rFirefox = /(firefox)\/([\w.]+)/,
				rOpera = /(opera).+version\/([\w.]+)/,
				rChrome = /(chrome)\/([\w.]+)/,
				rSafari = /version\/([\w.]+).*(safari)/;
var browser;
var version;
var ua = userAgent.toLowerCase();
function uaMatch(ua) {
    var match = rMsie.exec(ua);
    if (match != null) {
        return { browser: "IE", version: match[2] || "0" };
    }
    var match = rFirefox.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rOpera.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rChrome.exec(ua);
    if (match != null) {
        return { browser: match[1] || "", version: match[2] || "0" };
    }
    var match = rSafari.exec(ua);
    if (match != null) {
        return { browser: match[2] || "", version: match[1] || "0" };
    }
    if (match != null) {
        return { browser: "", version: "0" };
    }
}

//根据注册的EMail地址打开邮箱登录页
function LoginMailURL(url) {
    var BaseURL = url.substr(url.indexOf('@') + 1, url.length - url.indexOf('@'));
    if (BaseURL == "163.com") {
        window.location.href = "http://mail.163.com";
    }
    if (BaseURL == "vip.163.com") {
        window.location.href = "http://vip.163.com";
    }
    if (BaseURL == "126.com") {
        window.location.href = "http://mail.126.com";
    }
    if (BaseURL == "qq.com") {
        window.location.href = "http://mail.qq.com";
    }
    if (BaseURL == "sina.com") {
        window.location.href = "http://mail.sina.com";
    }
    if (BaseURL == "vip.sina.com") {
        window.location.href = "http://vip.sina.com";
    }
    if (BaseURL == "hotmail.com") {
        window.location.href = "http://www.hotmail.com";
    }
    if (BaseURL == "live.com") {
        window.location.href = "http://login.live.com";
    }
    if (BaseURL == "gmail.com") {
        window.location.href = "http://mail.google.com";
    }
    if (BaseURL == "sohu.com") {
        window.location.href = "http://mail.sohu.com";
    }
    if (BaseURL == "tom.com") {
        window.location.href = "http://mail.tom.com";
    }
    if (BaseURL == "aliyun.com") {
        window.location.href = "http://mail.aliyun.com";
    }
    else {

    }
}
