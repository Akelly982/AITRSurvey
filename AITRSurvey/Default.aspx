<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AITRSurvey.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <link rel="stylesheet" href="AkStyle.css" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="spacer4"></div>
        <div class="flexCentered">
            <div class="topBar">
                <div class="topBarItemMid">
                    <asp:Label ID="titleLabel" class="title" runat="server" Text="Home" style="text-align: left"></asp:Label>
                </div>
                <div class="topBarItemRight" >
                        <asp:Button ID="AdminLoginBtn" class="btnSizeStandard btnColorInfo" runat="server" Text="Admin Login" OnClick="AdminLoginBtn_Click" />
                </div>
            </div>
        </div>

        <div class="spacer2"></div>
        <div class="flexCentered">
            <div class="defaultContainer">
                <asp:Button ID="RunSurveyBtn" class="btnSizeStandard btnColorSuccess" runat="server" Text="runSurvey" OnClick="RunSurveyBtn_Click" />
            </div>
        </div>

    </form>
</body>
</html>
