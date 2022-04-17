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
        <div style="text-align: center">
            <asp:Label ID="devConsolelbl" runat="server" Text="Dev Console Lbl"></asp:Label>
        </div>
        <br />
        <%-- dynamic question data  --%>
        <div id="DynamicQuestionData" runat="server">
            
        </div>
        <br />
        <%-- groupRespondent search --%>
        <%-- dynamic qid selector --%>
        <div id="DynamicQidSelector" runat="server">
            
        </div>
        <br />
        <div>
            <asp:Label ID="Label2" runat="server" Text="Where Response Like: "></asp:Label>
            <asp:TextBox ID="GroupRespondentResponseTextBox" runat="server"></asp:TextBox>
            <asp:Button ID="GroupRespondentsSubmitBtn" runat="server" Text="Submit" OnClick="GroupRespondentsSubmitBtn_Click" />
            <br />
            <asp:Label ID="GroupRespondentLbl" runat="server" Text=""></asp:Label>
            <br/>
            <asp:Label ID="ErrorGroupRespondentLbl" runat="server" Text=""></asp:Label>
            <asp:GridView ID="GroupRespondentGridView" runat="server"></asp:GridView> 
        </div>
            
        <br/>
        <%-- find respondent search --%>
        <div id="findRespondentData" runat="server">
            <br />
            <asp:Label ID="FindRespondentTitle" runat="server" Text="Find a respondent:"></asp:Label>
            <div>
                <asp:Label ID="FindRespondentTextBoxLbl" runat="server" Text="RID:"></asp:Label>
                <asp:TextBox ID="FindRespondentTextBox" runat="server"></asp:TextBox>
                <asp:Button ID="FindRespondentSubmitButton" runat="server" Text="Submit" OnClick="FindRespondentSubmitButton_Click" />
                <asp:Button ID="FindRespondentShowAllSubmitButton" runat="server" Text="Show All" OnClick="FindRespondentShowAllSubmitButton_Click"/>
            </div>
            <asp:Label ID="FindRespondentLbl" runat="server" Text=""></asp:Label>
            <br/>
            <asp:Label ID="ErrorFindRespondentLbl" runat="server" Text=""></asp:Label>
            <asp:GridView ID="FindRespondentGridView" runat="server"></asp:GridView>
        </div>
        <br/>
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
