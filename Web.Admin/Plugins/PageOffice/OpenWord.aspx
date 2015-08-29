<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpenWord.aspx.cs" Inherits="OpenWord" %>

<%@ Register Assembly="PageOffice, Version=2.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript">
        var savePath = "";
        function Save() {
            document.getElementById("PageOfficeCtrl1").WebSave();
            savePath = document.getElementById("PageOfficeCtrl1").CustomSaveResult;
        }
        function Print() {
            document.getElementById("PageOfficeCtrl1").PrintPreview();
        }
    </script>
    <form id="form1" runat="server">
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    <div style="width:auto; height:1000px;" >
        <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server" CustomToolbar="True" onload="PageOfficeCtrl1_Load">
        </po:PageOfficeCtrl>
    </div>
    </form>
</body>
</html>