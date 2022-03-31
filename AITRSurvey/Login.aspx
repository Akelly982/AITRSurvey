<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AITRSurvey.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="text-align: center">
                <asp:Label ID="TitleLabel" runat="server" style="font-weight: 700; " Text="Staff Login"></asp:Label>
            </div>
            <div>
                <asp:Button ID="ReturnBtn" runat="server" Text="Return" OnClick="ReturnBtn_Click" />
            </div>
            <br />
            <asp:Label ID="UsernameLabel" runat="server" Text="Username:"></asp:Label>
            <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label>
            <asp:TextBox ID="PasswordTextbox" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="LoginSubmitButton" runat="server" Text="Submit" Width="191px" OnClick="LoginSubmitButton_Click" />
            <br />
            <asp:Label ID="ErrMsg" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
