<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WordOnline.aspx.cs" Inherits="HanFang360.InterfaceService.WordOnline" %>
<%@ Register assembly="ZSOfficeX, Version=2.0.0.1, Culture=neutral, PublicKeyToken=340a75c0b5d1a305" namespace="ZSOfficeX" tagprefix="ZS" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<div style="height:700px;widht:100%;">
    <%if(lblMsg.Text == ""){ %>
    <!--**************   卓正OFFICE组件 客户端代码开始    ************************-->
	<script language="JavaScript" event="OnInit()" for="ZSOfficeCtrl">
		// 控件打开文档前触发，用来初始化界面样式
	</script>
	<script language="JavaScript" event="OnDocumentOpened(str, obj)" for="ZSOfficeCtrl">
		// 控件打开文档后立即触发，添加自定义菜单，自定义工具栏，禁止打印，禁止另存，禁止保存等等
		
	</script>
	<script language="JavaScript" event="OnDocumentClosed()" for="ZSOfficeCtrl">
		
	</script>
	<script language="JavaScript" event="OnUserMenuClick(index, caption)" for="ZSOfficeCtrl">
		// 添加您的自定义菜单项事件响应
		
	</script>
	<script language="JavaScript" event="OnCustomToolBarClick(index, caption)" for="ZSOfficeCtrl">
		// 添加您的自定义工具栏按钮事件响应
	</script>	
    <!--**************   卓正OFFICE组件 客户端代码结束    ************************-->
    <ZS:ZSOfficeCtrl ID="ZSOfficeCtrl" runat="server" />
    <%} %>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</div>
</body>
</html>
