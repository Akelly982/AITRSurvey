<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleSurveyCB.aspx.cs" Inherits="AITRSurvey.SampleSurveyCB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Sample Survey" style="font-weight: 700; margin-right:25px"></asp:Label>
            <asp:Label ID="activeQuestrionID" runat="server" Text="Q1"></asp:Label>
        </div>
        <br />
        <div>
            <asp:Label ID="Label2" runat="server" Text="Select from below all your favourite snacks." Width="800px"></asp:Label>
        </div>
        <br />
        <div>
            <asp:CheckBoxList ID="userSelectionCB" runat="server"></asp:CheckBoxList>
        </div>
        <br />
        <div>
            <asp:Button ID="SubmitButton" runat="server" Text="Submit" Width="200px" />
        </div>
    </form>
</body>
</html>
