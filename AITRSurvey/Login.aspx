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
            <asp:Label ID="Label1" runat="server" style="font-weight: 700; " Text="Staff Login"></asp:Label>
            </div>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="Password:"></asp:Label>
            <asp:TextBox ID="PasswordTextbox" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="LoginSubmitButton" runat="server" Text="Submit" Width="191px" />
        </div>
    </form>
</body>
</html>
