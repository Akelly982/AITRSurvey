<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleSurveyTB.aspx.cs" Inherits="AITRSurvey.SampleSurveyTB" %>

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
        <div style="text-align: center">
            <asp:Label ID="questionText" runat="server" Text="Given how AIT classes are run at the moment do you enjoy having the choice of online/onsite mixed classes?" Width="800px"></asp:Label>
        </div>
        <br />
        <div style="text-align: center">
            <asp:TextBox ID="userResultTB" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
        </div>
        <br />
        <div style="text-align: center">
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" Width="200px" />
        </div>
    </form>
</body>
</html>
