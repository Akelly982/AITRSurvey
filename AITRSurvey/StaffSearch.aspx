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
        <div style="text-align: center">
                <asp:Label ID="StaffUsername" runat="server" Text="user"></asp:Label>
                <asp:Label ID="StaffFirstName" runat="server" Text="first"></asp:Label>
                <asp:Label ID="StaffLastName" runat="server" Text="last"></asp:Label>
                <asp:Label ID="StaffEmail" runat="server" Text="email"></asp:Label>
                <asp:Button ID="LogoutBtn" runat="server" Text="Log Out" OnClick="LogoutBtn_Click" />
        </div>
        <br />
        <%-- dynamic form --%>
        <div id="DynamicForm" runat="server">
            
        </div>
        <br />
        <div style="text-align: center">
            <asp:Button ID="SubmitBtn" runat="server" Text="Submit" OnClick="SubmitBtn_Click" />
            <br />
            <asp:Label ID="devConsolelbl" runat="server" Text="Dev Console Lbl"></asp:Label>
        </div>
        <div style="text-align: center">
            <asp:GridView ID="DataGridView" runat="server"></asp:GridView> 
        </div>
        <div runat="server"> 
            <input id="idListHolder" type="hidden" value="dynamic data" runat="server"/> 
            <input id="itemTypeListHolder" type="hidden" value="dynamic data" runat="server"/> 
        </div>
        <%-- Ensure script loads after page data --%>
        <div>
            <script src="StaffSearch.js" type="text/javascript"></script>
        </div>
       </form>
</body>
</html>
