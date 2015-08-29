// 发送系统通知
function SendSysMessage() {
    var templateID = $("#TemplateID").val();
    var CID = $("#hiddenCID").val();
    var FlowLogID = GetDLLText("selCIDLogID", CID);
    var ReveiverList = [];
    $('input[name="chbAuthor"]:checked').each(function() {
        ReveiverList.push($(this).val());
    });
    $('div[name="divSelExpert"]').each(function () {
        ReveiverList.push($(this).attr("id").replace("divSelUserDiv_", ""));
    });
    if (ReveiverList.length == 0) {
        alert('请选择接收人');
        return false;
    }
   var IsEmail = true;
   if ($("#chbSendMail").attr("checked")) {
       IsEmail = true;
   }
   var EmailTitle = $("#txtEmailTitle").val();
   $("#UEContent").val(UE.getEditor('UEContent').getContent())
   var EmailBody = $("#UEContent").val();
   if ($.trim(EmailTitle) == "") {
       alert('请填写邮件标题');
       $("#txtEmailTitle").focus();
       return false;
   }
   if ($.trim(EmailBody) == "") {
       alert('请填写邮件内容');
       return false;
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
           return false;
       }
   }

   var BillObj = { "FlowLogID": FlowLogID, "ActionID": ActionID, "CID": CID, "ReveiverList": ReveiverList.join(), "IsEmail": true, "EmailTitle": EmailTitle, "EmailBody": EmailBody, "IsSMS": IsSMS, "SMSBody": SMSBody, "TemplateID": templateID };
   $.ajax({
       beforeSend: function () {

       },
       type: 'POST',
       url: RootPath + '/FlowSet/SendSysMessageAjax/?rnd=' + Math.random(),
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
                               if (!obj.FlowConfig.IsMultiPerson && obj.FlowAction.ActionID != -1) {
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
                   alert('消息发送成功!');
               }
               else {
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