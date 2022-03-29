using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AITRSurvey
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginSubmitButton_Click(object sender, EventArgs e)
        {
            
            if (EmailREV.IsValid)     // regular expression validator for EmailTextBox
            {
                // validate user inputs
                String passText = PasswordTextbox.Text;
                String emailText = EmailTextBox.Text;

                userDbCheck(emailText,passText);
            }

        }



         void userDbCheck(String username, String password)
        {

        }

    }
}