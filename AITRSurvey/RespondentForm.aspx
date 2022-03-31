<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RespondentForm.aspx.cs" Inherits="AITRSurvey.RespondentForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center">
            <asp:Label ID="titleLabel" runat="server" Text="Respondent Form" style="text-align: left"></asp:Label>
        </div>
        <div>
            <div>
                <asp:Label ID="Label1" runat="server" Text="Gender"></asp:Label>
                <asp:RadioButtonList ID="GenderRBList" runat="server"></asp:RadioButtonList>
            </div>
            <div>
                <asp:Label ID="Label3" runat="server" Text="Age Range:"></asp:Label>
                <asp:RadioButtonList ID="AgeRangeRBList" runat="server"></asp:RadioButtonList>
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="Post Code:"></asp:Label>
                <asp:TextBox ID="PostCodeTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="Label4" runat="server" Text="Suburb:"></asp:Label>
                <asp:TextBox ID="SuburbTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="Label5" runat="server" Text="State:"></asp:Label>
                <asp:TextBox ID="StateTextBox" runat="server"></asp:TextBox>
            </div>


            <%-- If wants to be member --%>
            <div>
                <asp:Label ID="Label6" runat="server" Text="Do you wish to participate as a member?"></asp:Label>
                <asp:RadioButtonList ID="IsMemberRBList" runat="server" OnSelectedIndexChanged="IsMemberRBList_SelectedIndexChanged" AutoPostBack="True"></asp:RadioButtonList>
                <asp:Label ID="isMemberTrueLabel" runat="server" Text=""></asp:Label>
            </div>

            <div id="MemberDataSection" runat="server">
                <div>
                    <asp:Label ID="Label7" runat="server" Text="First Name:"></asp:Label>
                    <asp:TextBox ID="FirstNameTextBox" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="Label8" runat="server" Text="Last Name:"></asp:Label>
                    <asp:TextBox ID="LastNameTextBox" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="Label9" runat="server" Text="Phone Number:"></asp:Label>
                    <asp:TextBox ID="PhoneNumberTextBox" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="Label10" runat="server" Text="Date Of Birth:"></asp:Label>
                    <asp:TextBox ID="DateOfBirthTextBox" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="Label11" runat="server" Text="Email:"></asp:Label>
                    <asp:TextBox ID="EmailTextBox" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="Label12" runat="server" Text="Street Address:"></asp:Label>
                    <asp:TextBox ID="StreetAddressTextBox" runat="server"></asp:TextBox>
                </div>
            </div>


            <%-- Submit --%>
            <div>
                <asp:Button ID="SubmitBtn" runat="server" Text="Submit" OnClick="SubmitBtn_Click" />
            </div>
        </div>
    </form>
</body>
</html>
