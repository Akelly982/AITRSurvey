using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AITRSurvey
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///     redirect user to our survey 
        /// </summary>
        protected void RunSurveyBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("DynamicSurvey.aspx");
        }


        /// <summary>
        ///     redirect user to our login page for staff users 
        /// </summary>
        protected void AdminLoginBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}