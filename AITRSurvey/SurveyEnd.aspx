<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyEnd.aspx.cs" Inherits="AITRSurvey.SurveyEnd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey End</title>
    <link rel="stylesheet" href="AkStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="spacer4"></div>
        <div class="flexCentered">
            <div class="surveyEndContainer">
                 <asp:Label class="title surveyEndContainerItem" ID="titleLabel" runat="server" Text="Thank you for participating in our survey."></asp:Label>
                 <br/>
                 <asp:Button class="btnColorSuccess surveyEndContainerItem" ID="returnBtn" runat="server" Text="Return to home" OnClick="returnBtn_Click" />
            </div>
        </div>
    </form>
</body>
</html>
