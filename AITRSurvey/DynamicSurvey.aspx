<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynamicSurvey.aspx.cs" Inherits="AITRSurvey.DynamicSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <asp:Label ID="TitleLbl" runat="server" Text="Survey : "></asp:Label>
            <asp:Label ID="QuestionNumLbl" runat="server" Text="Q99"></asp:Label>
        </div>

        <%-- Question Text --%>
        <div>
            <asp:Label ID="QuestionTextLbl" runat="server" Text="Question Text, Question Text, Question Text " Width="800px"></asp:Label>
        </div>
        <%-- Dynamic question  --%>
        <div id="SurveyContainer" runat="server">
            <%-- TextBox Item --%>
            <div id="ItemTextBox" runat="server">
                <div style="text-align: center">
                    <asp:TextBox ID="userResultTB" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                </div>
                <br />
                <div style="text-align: center">
                    <asp:Button ID="SubmitButtonTB" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonTB_Click" />
                </div>
            </div>
            <%-- CheckBox Item --%>
            <div id="ItemCheckBox" runat="server">
                <div>
                    <asp:CheckBoxList ID="userSelectionCB" runat="server"></asp:CheckBoxList>
                </div>
                <br />
                <div>
                    <asp:Button ID="SubmitButtonCB" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonCB_Click"/>
                </div>
            </div>
            <br />
            <%-- Radio Button item --%>
            <div id="ItemRadioBtn" runat="server">
                <div>
                    <asp:RadioButtonList ID="userSelectionRB" runat="server" style="text-align: left"></asp:RadioButtonList>
                </div>
                <br />
                <div>
                    <asp:Button ID="SubmitButtonRB" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonRB_Click"/>
                </div>
            </div>
        </div>

        <div id="DevConsole" runat="server">
            <asp:Label ID="DevMessageLbl" runat="server"></asp:Label>
            <asp:GridView ID="devQuestionGridView" runat="server"></asp:GridView>
            <asp:GridView ID="devQuestionValuesGridView" runat="server"></asp:GridView>
            <asp:Button ID="HiddenSubmit" runat="server" OnClick="HiddenSubmit_Click" Text="HiddenSubmit" />
        </div>
        <div id="SurveyQuestionHolder" runat="server">
            
        </div>
    </form>
</body>
</html>
