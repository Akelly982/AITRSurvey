<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AITRSurvey.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="AkStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="topBar">
            <div class="flexCentered">
                <asp:Button class="btnSizeStandard btnColorWarning" ID="ReturnBtn" runat="server" Text="Return" OnClick="ReturnBtn_Click" />
            </div>
            <div class="flexCentered">
                <asp:Label class="title" ID="TitleLabel" runat="server" style="font-weight: 700; " Text="Staff Login"></asp:Label>
            </div>
        </div>
        <div class="centerPanelContainer">
            <div class="centerPanel">
                <div class="loginPanelItem">
                    <asp:Label ID="UsernameLabel" runat="server" Text="Username:"></asp:Label>
                    <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
                </div>
                <div class="loginPanelItem">
                    <asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label>
                    <asp:TextBox ID="PasswordTextbox" runat="server"></asp:TextBox>
                </div>
                <div class="loginPanelItem">
                    <asp:Button class="btnSizeStandard btnColorSuccess" ID="LoginSubmitButton" runat="server" Text="Submit" OnClick="LoginSubmitButton_Click" />
                </div>
                <div class="loginPanelItem">
                    <asp:Label class="loginErrorMsg textColorError" ID="ErrMsg" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
