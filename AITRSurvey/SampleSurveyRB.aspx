<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleSurveyRB.aspx.cs" Inherits="AITRSurvey.SampleSurveyRB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Sample Survey" style="font-weight: 700; padding-right:25px"></asp:Label>
            <asp:Label ID="activeQuestrionID" runat="server" Text="Q1"></asp:Label>
        </div>
        <br />
        <div>
            <asp:Label ID="Label2" runat="server" Text="What is your favourite Colour." Width="800px"></asp:Label>
        </div>
        <br />
        <div>
            <asp:RadioButtonList ID="userSelectionRB" runat="server" style="text-align: left"></asp:RadioButtonList>
        </div>
        <br />
        <div>
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" Width="200px" />
        </div>
    </form>
</body>
</html>
