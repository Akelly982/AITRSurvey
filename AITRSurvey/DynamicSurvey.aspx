<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynamicSurvey.aspx.cs" Inherits="AITRSurvey.DynamicSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <asp:Label ID="QuestiionNumLbl" runat="server" Text="Q99"></asp:Label>
            <asp:Label ID="TitleLbl" runat="server" Text="Survey"></asp:Label>
        </div>
        <div id="DevConsole" runat="server">
            <asp:Label ID="DevConsoleLbl" runat="server" Text=""></asp:Label>
            <asp:GridView ID="devQuestionGridView" runat="server"></asp:GridView>
            <asp:GridView ID="devQuestionValuesGridView" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
