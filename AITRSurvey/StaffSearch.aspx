<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffSearch.aspx.cs" Inherits="AITRSurvey.StaffSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
               <asp:Label ID="Label1" runat="server" Text="Staff Search"></asp:Label>
        </div>
        <br />
        <div style="text-align: center">
            <asp:GridView ID="DataGridView" runat="server"></asp:GridView> 
        </div>
        <div>
            <asp:Label ID="Label6" runat="server" Text="Members"></asp:Label>
            <asp:RadioButtonList ID="MemberRadioButtonList" runat="server"></asp:RadioButtonList>
            <br />
            <asp:Label ID="Label2" runat="server" Text="FirstName"></asp:Label>
            <asp:TextBox ID="FirstNameTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label3" runat="server" Text="LastName"></asp:Label>
            <asp:TextBox ID="LastNameTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label4" runat="server" Text="State"></asp:Label>
            <asp:TextBox ID="StateTextBox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="Label5" runat="server" Text="Gender"></asp:Label>
            <asp:RadioButtonList ID="GenderRadioButtonList" runat="server"></asp:RadioButtonList>
            <asp:Label ID="Label7" runat="server" Text="Suburb"></asp:Label>
            <asp:TextBox ID="Suburb" runat="server"></asp:TextBox>
            <br />
        </div>
       </form>
</body>
</html>
