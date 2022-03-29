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
            <br />
            <asp:Label ID="EmailLabel" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="EmailREV" runat="server" ErrorMessage=" - Invalid Email" ControlToValidate="EmailTextBox" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="PasswordLabel" runat="server" Text="Password:"></asp:Label>
            <asp:TextBox ID="PasswordTextbox" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="LoginSubmitButton" runat="server" Text="Submit" Width="191px" OnClick="LoginSubmitButton_Click" />
        </div>
    </form>
</body>
</html>
