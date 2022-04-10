<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyEnd.aspx.cs" Inherits="AITRSurvey.SurveyEnd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <div style=" height: 500px; display: flex; align-items: center; justify-content: center">
                <div style="text-align: center;">
                    <asp:Label ID="titleLabel" runat="server" Text="Thank you for participating in our survey."></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="returnBtn" runat="server" Text="Return to home" OnClick="returnBtn_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
