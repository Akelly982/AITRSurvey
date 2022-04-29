<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynamicSurvey.aspx.cs" Inherits="AITRSurvey.DynamicSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey</title>
    <link rel="stylesheet" href="AkStyle.css" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="spacer3"></div>
        <div class="flexCentered">
            <div class="surveyTitle">
                <asp:Label class="titleSmall" ID="TitleLbl" runat="server" Text="Survey"></asp:Label>
            </div>
        </div>

        <div class="spacer2"></div>
        <div class="flexCentered">
            <div class="surveyQuestion">
                <%-- Question Text --%>
                <div class="surveyQuestionText title">
                    <asp:Label ID="QuestionTextLbl" runat="server" Text="Question Text, Question Text, Question Text"></asp:Label>
                </div>
                 <%-- Dynamic question  --%>
                <div id="SurveyContainer" runat="server">
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
                            <asp:RadioButtonList ID="userSelectionRB" runat="server" style="text-align: left" OnSelectedIndexChanged="userSelectionRB_SelectedIndexChanged"></asp:RadioButtonList>
                        </div>
                        <br />
                        <div>
                            <asp:Button ID="SubmitButtonRB" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonRB_Click"/>
                        </div>
                    </div>
                    <%-- TextBox Default item --%>
                    <div id="ItemTextBox" runat="server">
                        <div style="text-align: center">
                            <asp:TextBox ID="userResultTB" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="SubmitButtonTB" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonTB_Click" />
                        </div>
                    </div>
                    <%-- Additional textBox to run with specific reg expressions --%>
                    <%-- TextBox Phone item --%>
                    <div id="ItemTextBoxPhone" runat="server">
                        <div style="text-align: center">
                            <asp:TextBox ID="userResultTBPhone" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="SubmitButtonTBPhone" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonPhoneTB_Click" />
                        </div>
                    </div>
                    <%-- TextBox Email item --%>
                    <div id="ItemTextBoxEmail" runat="server">
                        <div style="text-align: center">
                            <asp:TextBox ID="userResultTBEmail" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="SubmitButtonTBEmail" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonEmailTB_Click" />
                        </div>
                    </div>
                    <%-- TextBox Date item --%>
                    <div id="ItemTextBoxDate" runat="server">
                        <div style="text-align: center">
                            <asp:TextBox ID="userResultTBDate" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="SubmitButtonTBDate" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonDateTB_Click" />
                        </div>
                    </div>
                    <%-- TextBox PostCode item --%>
                    <div id="ItemTextBoxPostCode" runat="server">
                        <div style="text-align: center">
                            <asp:TextBox ID="userResultTBPostCode" runat="server" Height="200px" Width="400px" MaxLength="300" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="SubmitButtonTBPostCode" runat="server" Text="Submit" Width="200px" OnClick="SubmitButtonPostCodeTB_Click" />
                        </div>
                    </div>
                </div>
                <div style="text-align: center" class="validatorContainer">
                    <div class="spacer0"></div>
                    <%-- default textbox not empty validator --%>
                    <asp:RequiredFieldValidator ID="textBoxRequiredFieldValidator" runat="server" ControlToValidate="userResultTB" Enabled="False" ErrorMessage="Input field is empty"></asp:RequiredFieldValidator>
                    <%-- regex textbox validators and their empty validator --%>
                    <%-- phone --%>
                    <asp:RequiredFieldValidator ID="textBoxPhoneEmptyREV" runat="server" ControlToValidate="userResultTBPhone" Enabled="False" ErrorMessage="Input field is empty"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="textBoxPhoneREV" runat="server" ErrorMessage="Error Phone number: 123-123-1234" ControlToValidate="userResultTBPhone" ValidationExpression="[0-9]{3}-[0-9]{3}-[0-9]{4}"></asp:RegularExpressionValidator>
                    <%-- email --%>
                    <asp:RequiredFieldValidator ID="textBoxEmailEmptyREV" runat="server" ControlToValidate="userResultTBEmail" Enabled="False" ErrorMessage="Input field is empty"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="textBoxEmailREV" runat="server" ErrorMessage="Error Email: name@email.com" ControlToValidate="userResultTBEmail" ValidationExpression="([a-zA-Z0-9])+\@+([a-zA-Z0-9])+(.com)"></asp:RegularExpressionValidator>
                    <%-- date --%>
                    <asp:RequiredFieldValidator ID="textBoxDateEmptyREV" runat="server" ControlToValidate="userResultTBDate" Enabled="False" ErrorMessage="Input field is empty"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="textBoxDateREV" runat="server" ErrorMessage="Error Date: yyyy-mm-dd" ControlToValidate="userResultTBDate" ValidationExpression="[0-9]{4}-([0-1]{1})([0-9]{1})-([0-3]{1})([0-9]{1})"></asp:RegularExpressionValidator>
                    <%-- post code --%>
                    <asp:RequiredFieldValidator ID="textBoxPostCodeEmptyREV" runat="server" ControlToValidate="userResultTBPostCode" Enabled="False" ErrorMessage="Input field is empty"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="textBoxPostCodeREV" runat="server" ErrorMessage="Error PostCode: 1234" ControlToValidate="userResultTBPostCode" ValidationExpression="[0-9]{4}"></asp:RegularExpressionValidator>
                    <div class="spacer1"></div>
                </div>
            </div>
        </div>

        <div class="spacer2"></div>
        <div class="flexCentered">
            <div class="surveyDevConsole" id="DevConsole" runat="server">
                <asp:Label class="title" runat="server" Text="Active Question"></asp:Label>
                <asp:Label class="title" ID="QuestionNumLbl" runat="server" Text="Q99"></asp:Label>
                <br/>
                <asp:Label ID="DevMessageLbl" runat="server"></asp:Label>
                <asp:GridView class="surveyGridView" ID="devQuestionGridView" runat="server"></asp:GridView>
                <asp:GridView class="surveyGridView" ID="devQuestionValuesGridView" runat="server"></asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
