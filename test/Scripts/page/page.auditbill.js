// 提交审稿单
function SubmitAuditBill() {
    var CID = $("#hiddenCID").val();
    var FlowLogID = GetDLLText("selCIDLogID", CID);
    var ReveiverList = [];
    $('input[name="chbAuthor"]:checked').each(function () {
        ReveiverList.push($(this).val());
    });
    $('div[name="divSelExpert"]').each(function () {
        ReveiverList.push($(this).attr("id").replace("divSelUserDiv_", ""));
    });

    $("input[name='chkEmailList']:checked").each(function () {
        ReveiverList.push($(this).val());
    });

    if (ReveiverList.length == 0) {
        alert('请选择接收人');
        $("#btnSubmit").hideLoading();
        $("#btnSubmit").val("提  交");
        return false;
    }
    var IsEmail = false;
    if ($("#chbSendMail").attr("checked")) {
        IsEmail = true;
    }
    var EmailTitle = $("#txtEmailTitle").val();
    $("#fckContent").val(UE.getEditor('fckContent').getContent())
    var EmailBody = $("#fckContent").val();
    if (IsEmail) {
        if ($.trim(EmailTitle) == "") {
            alert('请填写邮件标题');
            $("#txtEmailTitle").focus();
            $("#btnSubmit").hideLoading();
            $("#btnSubmit").val("提  交");
            return false;
        }
        if ($.trim(EmailBody) == "") {
            alert('请填写邮件内容');
            $("#btnSubmit").hideLoading();
            $("#btnSubmit").val("提  交");
            return false;
        }
    }
    var IsSMS = false;
    if ($("#chbSendSMS").attr("checked")) {
        IsSMS = true;
    }
    var SMSBody = $("#txtSMSBody").val();
    if (IsSMS) {
        if ($.trim(SMSBody) == "") {
            alert('请填写短信内容');
            $("#txtSMSBody").focus();
            $("#btnSubmit").hideLoading();
            $("#btnSubmit").val("提  交");
            return false;
        }
    }
    var DealAdvice = EmailBody;
    var CPath = contributePath;
    var CFileName = cFileName;
    var FFileName = fFileName;
    var FigurePathParam = figurePath;
    var BillObj = { "FlowLogID": FlowLogID, "StatusID": StatusID, "ActionID": ActionID, "ActionType": ActionType, "CID": CID, "ReveiverList": ReveiverList.join(), "AuditedDate": $("#txtAuditedDate").val(), "IsEmail": IsEmail, "EmailTitle": EmailTitle, "EmailBody": EmailBody, "IsSMS": IsSMS, "SMSBody": SMSBody, "DealAdvice": DealAdvice, "CPath": CPath, "CFileName": CFileName,"FFileName":FFileName, "FigurePath": FigurePathParam, "OtherPath": "", "IsContinueSubmit": IsContinueSubmit == 1 ? true : false };
    $.ajax({
        beforeSend: function () {
            $("#btnSubmit").showLoading();
            $("#btnSubmit").val("正在提交...");
        },
        type: 'POST',
        url: RootPath + '/FlowSet/SubmitAuditBillAjax/?rnd=' + Math.random(),
        dataType: "json",
        async: false,
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ auditBillEntity: BillObj }),
        success: function (e) {
            if (e.result == 'success') {
                $("#selContributionList option[value=" + CID + "]").remove();
                $("#selCIDLogID option[value=" + FlowLogID + "]").remove();
                if ($("#selContributionList option").length > 0) {
                    $('#selContributionList')[0].options[0].selected = true;
                    $("#hiddenCID").val($('#selContributionList').val());
                    $.ajax({
                        beforeSend: function () { },
                        type: 'POST',
                        url: RootPath + '/FlowSet/GetContributeAuthor?ActionID=' + ActionID + "&StatusID=" + StatusID + "&singleCID=" + $('#selContributionList').val() + ":" + $('#selCIDLogID').val() + "," + '&rnd=' + Math.random(),
                        dataType: "json",
                        async: false,
                        contentType: 'application/json; charset=utf-8',
                        data: "",
                        success: function (e) {
                            if (e != null) {
                                var obj = e;
                                if (!obj.FlowConfig.IsMultiPerson && obj.FlowAction.ActionRoleID != 3 && obj.FlowAction.ActionID != -1) {
                                    var index = 1;
                                    var div = document.getElementById("AuthorList");
                                    div.innerHTML = "";
                                    for (var i = 0; i < obj.FlowAuthorList.length; i++) {
                                        if (index % 5 == 0) {
                                            div.innerHTML += "<input type=\"radio\" name=\"chbAuthor\" value=\"" + obj.FlowAuthorList[i].AuthorID + "\"/>" + obj.FlowAuthorList[i].RealName + "<br />";
                                        }
                                        else {
                                            div.innerHTML += "<input type='radio' name='chbAuthor' value='" + obj.FlowAuthorList[i].AuthorID + "'/>" + obj.FlowAuthorList[i].RealName;
                                        }
                                        index++;
                                    }

                                }
                            }
                        },
                        error: function (xhr) {
                            alert('数据源访问错误' + '\n' + xhr.responseText);
                        }
                    });
                    alert('提交成功!');
                    $("#btnSubmit").hideLoading();
                    $("#btnSubmit").val("提  交");
                }
                else {
                    //alert("ok");
                    // window.parent.f_addTab('ContributionArea', '执行审稿操作', RootPath + "/Contribution/ContributionArea/" + "&rnd=" + Math.random());
                    window.parent.RealoadTab('AuditBill'); 
                    window.parent.RealoadTab('ContributionArea');
                    window.parent.f_removeSelectedTabItem();
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

// 提交审稿单并设置年期
function SubmitAuditBillSetYear(AuthorID, Year, Volume, Issue, JChannelID) {
    var CID = $("#hiddenCID").val();
    var FlowLogID = GetDLLText("selCIDLogID", CID);
    var ReveiverList = [];
    $('input[name="chbAuthor"]:checked').each(function () {
        ReveiverList.push($(this).val());
    });
    $('div[name="divSelExpert"]').each(function () {
        ReveiverList.push($(this).attr("id").replace("divSelUserDiv_", ""));
    });

    $("input[name='chkEmailList']:checked").each(function () {
        ReveiverList.push($(this).val());
    });

    if (ReveiverList.length == 0) {
        alert('请选择接收人');
        $("#btnSubmit").hideLoading();
        $("#btnSubmit").val("提  交");
        return false;
    }
    var IsEmail = false;
    if ($("#chbSendMail").attr("checked")) {
        IsEmail = true;
    }
    var EmailTitle = $("#txtEmailTitle").val();
    $("#fckContent").val(UE.getEditor('fckContent').getContent())
    var EmailBody = $("#fckContent").val();
    if (IsEmail) {
        if ($.trim(EmailTitle) == "") {
            alert('请填写邮件标题');
            $("#txtEmailTitle").focus();
            $("#btnSubmit").hideLoading();
            $("#btnSubmit").val("提  交");
            return false;
        }
        if ($.trim(EmailBody) == "") {
            alert('请填写邮件内容');
            $("#btnSubmit").hideLoading();
            $("#btnSubmit").val("提  交");
            return false;
        }
    }
    var IsSMS = false;
    if ($("#chbSendSMS").attr("checked")) {
        IsSMS = true;
    }
    var SMSBody = $("#txtSMSBody").val();
    if (IsSMS) {
        if ($.trim(SMSBody) == "") {
            alert('请填写短信内容');
            $("#txtSMSBody").focus();
            $("#btnSubmit").hideLoading();
            $("#btnSubmit").val("提  交");
            return false;
        }
    }
    var DealAdvice = EmailBody;
    var CPath = contributePath;
    var CFileName = cFileName;
    var FFileName = fFileName;
    var FigurePathParam = figurePath;
    var BillObj = { "FlowLogID": FlowLogID, "StatusID": StatusID, "ActionID": ActionID, "ActionType": ActionType, "CID": CID, "ReveiverList": ReveiverList.join(), "AuditedDate": $("#txtAuditedDate").val(), "IsEmail": IsEmail, "EmailTitle": EmailTitle, "EmailBody": EmailBody, "IsSMS": IsSMS, "SMSBody": SMSBody, "DealAdvice": DealAdvice, "CPath": CPath, "CFileName": CFileName, "FFileName": FFileName, "FigurePath": FigurePathParam, "OtherPath": "", "IsContinueSubmit": IsContinueSubmit == 1 ? true : false };
    $.ajax({
        beforeSend: function () {
            $("#btnSubmit").showLoading();
            $("#btnSubmit").val("正在提交...");
        },
        type: 'POST',
        url: RootPath + '/FlowSet/SubmitAuditBillAjax/?rnd=' + Math.random(),
        dataType: "json",
        async: false,
        contentType: 'application/json; charset=utf-8',
        data: $.toJSON({ auditBillEntity: BillObj }),
        success: function (e) {
            if (e.result == 'success') {
                //设置年期
                $.ajax({
                    beforeSend: function () {

                    },
                    type: 'POST',
                    url: RootPath + '/Contribution/SetYearVolumnIssueAjax/?rnd=' + Math.random(),
                    data: { CID: CID, AuthorID: AuthorID, Year: Year, Volume: Volume, Issue: Issue, JChannelID: JChannelID },
                    success: function (data) {
                        var e = eval("(" + data + ")");
                        if (e.result == 'success') {
                            $("#selContributionList option[value=" + CID + "]").remove();
                            $("#selCIDLogID option[value=" + FlowLogID + "]").remove();
                            if ($("#selContributionList option").length > 0) {
                                $('#selContributionList')[0].options[0].selected = true;
                                $("#hiddenCID").val($('#selContributionList').val());
                                $.ajax({
                                    beforeSend: function () { },
                                    type: 'POST',
                                    url: RootPath + '/FlowSet/GetContributeAuthor?ActionID=' + ActionID + "&StatusID=" + StatusID + "&singleCID=" + $('#selContributionList').val() + ":" + $('#selCIDLogID').val() + "," + '&rnd=' + Math.random(),
                                    dataType: "json",
                                    async: false,
                                    contentType: 'application/json; charset=utf-8',
                                    data: "",
                                    success: function (e) {
                                        if (e != null) {
                                            var obj = e;
                                            if (!obj.FlowConfig.IsMultiPerson && obj.FlowAction.ActionRoleID != 3 && obj.FlowAction.ActionID != -1) {
                                                var index = 1;
                                                var div = document.getElementById("AuthorList");
                                                div.innerHTML = "";
                                                for (var i = 0; i < obj.FlowAuthorList.length; i++) {
                                                    if (index % 5 == 0) {
                                                        div.innerHTML += "<input type=\"radio\" name=\"chbAuthor\" value=\"" + obj.FlowAuthorList[i].AuthorID + "\"/>" + obj.FlowAuthorList[i].RealName + "<br />";
                                                    }
                                                    else {
                                                        div.innerHTML += "<input type='radio' name='chbAuthor' value='" + obj.FlowAuthorList[i].AuthorID + "'/>" + obj.FlowAuthorList[i].RealName;
                                                    }
                                                    index++;
                                                }

                                            }
                                        }
                                    },
                                    error: function (xhr) {
                                        alert('数据源访问错误' + '\n' + xhr.responseText);
                                    }
                                });
                                alert('提交成功！设置年期成功！ ');
                                $("#btnSubmit").hideLoading();
                                $("#btnSubmit").val("提  交");
                            }
                            else {
                                window.parent.RealoadTab('AuditBill');
                                window.parent.RealoadTab('ContributionArea');
                                window.parent.f_removeSelectedTabItem();
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