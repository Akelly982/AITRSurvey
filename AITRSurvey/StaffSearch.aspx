<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffSearch.aspx.cs" Inherits="AITRSurvey.StaffSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Staff Search</title>
    <link rel="stylesheet" href="AkStyle.css" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="spacer2"></div>
        <div class="flexCentered">
            <div class="topBar">
                <div class="topBarItemMid">
                    <asp:Label ID="pageTitlelbl" class="title" runat="server" Text="Staff Search"></asp:Label>
                </div>
                <div class="topBarItemRight">
                    <asp:Button ID="LogoutBtn" class="btnSizeStandard btnColorWarning" runat="server" Text="Log Out" OnClick="LogoutBtn_Click" />
                </div>
                
            </div>
        </div>

        <div class="spacer1"></div>
        <div class="flexCentered">
            <div class="staffSearchEmailLabelContainer">
                <asp:Label ID="StaffEmail" class="textColorLight" runat="server" Text="email"></asp:Label>
                <div id="DevConsole" class="staffSearchDevConsoleContainer" runat="server">
                    <asp:Label ID="devConsolelbl" class="textColorLight" runat="server" Text="Dev Console Lbl"></asp:Label>
                </div>
            </div>
        </div>
        

        <div class="staffSearchContainer">
            <div class="spacer1"></div>
            <%-- dynamic question data  --%>                
            <div class="staffSearchInnerContainer">
                <div class="flexCentered">
                    <asp:Label ID="IndexLabel" class="title" runat="server" Text="Question Index"></asp:Label>
                </div>
                <div id="DynamicQuestionData" class="staffSearchQuestionData" runat="server">
            
                </div>
            </div>
            <div class="spacer1"></div>

            <%-- groupRespondent search --%>            <%-- dynamic qid selector --%>
            <div class="staffSearchInnerContainer">
                <div class="flexCentered">
                        <asp:Label ID="groupRespondentTitleLabel" class="title" runat="server" Text="Find a respondent group:"></asp:Label>
                    </div>
                <div class="qidSelector">
                    <asp:RadioButtonList ID="QidSelectorRadioButtonList" runat="server" BorderStyle="None" RepeatDirection="Horizontal" RepeatLayout="Flow" Height="10px" RepeatColumns="15" style="margin-bottom: 0px">
                    </asp:RadioButtonList>
                </div>  
                <div class="staffSearchTextRow">
                    <asp:Label ID="Label2" runat="server" Text="Where Response Like: "></asp:Label>
                    <asp:TextBox ID="GroupRespondentResponseTextBox" runat="server"></asp:TextBox>
                    <asp:Button ID="GroupRespondentsSubmitBtn" runat="server" Text="Submit" OnClick="GroupRespondentsSubmitBtn_Click" />
                </div>
                <div class="staffSearchTextRow">
                    <asp:Label ID="GroupRespondentLbl" runat="server" Text=""></asp:Label>
                </div>
                <div class="staffSearchTextRow">
                    <asp:Label class="textColorError" ID="ErrorGroupRespondentLbl" runat="server" Text=""></asp:Label>
                    <asp:GridView class="staffSearchGridView" ID="GroupRespondentGridView" runat="server"></asp:GridView> 
                </div>
            </div>
            <div class="spacer1"></div>

            <%-- find respondent search --%>
            <div class="staffSearchInnerContainer">
                <div id="findRespondentData" runat="server">
                    <div class="flexCentered">
                        <asp:Label ID="FindRespondentTitle" class="title" runat="server" Text="Find a respondent:"></asp:Label>
                    </div>
                    <div class="staffSearchTextRow">
                        <asp:Label ID="FindRespondentTextBoxLbl" runat="server" Text="RID:"></asp:Label>
                        <asp:TextBox ID="FindRespondentTextBox" runat="server"></asp:TextBox>
                        <asp:Button ID="FindRespondentSubmitButton" runat="server" Text="Submit" OnClick="FindRespondentSubmitButton_Click" />
                        <asp:Button ID="FindRespondentShowAllSubmitButton" runat="server" Text="Show All" OnClick="FindRespondentShowAllSubmitButton_Click"/>
                    </div>
                    <div class="staffSearchTextRow">
                        <asp:Label ID="FindRespondentLbl" runat="server" Text=""></asp:Label>
                    </div>
                    <asp:Label class="textColorError" ID="ErrorFindRespondentLbl" runat="server" Text=""></asp:Label>
                    <asp:GridView class="staffSearchGridView" ID="FindRespondentGridView" runat="server"></asp:GridView>
                </div>
            </div>
            <div class="spacer3"></div>
            <%-- Hidden content for old JavaScript --%>            <%--<div runat="server"> 
                <input id="idListHolder" type="hidden" value="dynamic data" runat="server"/> 
                <input id="itemTypeListHolder" type="hidden" value="dynamic data" runat="server"/> 
            </div>--%>
        </div>
        <%-- Ensure script loads after page data --%>        <%--<div>
            <script src="StaffSearch.js" type="text/javascript"></script>
        </div>--%>
       </form>
</body>
</html>
