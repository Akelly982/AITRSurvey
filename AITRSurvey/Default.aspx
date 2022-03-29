<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AITRSurvey.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <asp:Label ID="titleLabel" runat="server" Text="Home" style="text-align: left"></asp:Label>
        </div>
        <div style="text-align: right">
            <asp:Button ID="AdminLoginBtn" runat="server" Text="adminLogin" style="text-align: center" OnClick="AdminLoginBtn_Click" />
        </div>
        <div style="text-align: center; height: 500px; display: flex; align-items: center; justify-content: center">
            <asp:Button ID="RunSurveyBtn" runat="server" Text="runSurvey" OnClick="RunSurveyBtn_Click" />
        </div>
    </form>
</body>
</html>
