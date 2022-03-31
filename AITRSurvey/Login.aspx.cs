using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;            // object / default data
using System.Data.SqlClient;  //execute cmd 
using System.Configuration;    // allows you to call from the web.config file

namespace AITRSurvey
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginSubmitButton_Click(object sender, EventArgs e)
        {
            
    
            // validate user inputs
            String passText = PasswordTextbox.Text;
            String usernameText = UsernameTextBox.Text;  // email is validated using Regualar Expression Validation componenet

            //connect to db
            SqlConnection myConn = new SqlConnection();

            // connection using a standard const 
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;


            myConn.Open(); // establish the connection to the db

            string cmdStr = String.Format("SELECT *" +
                                            "FROM Staff " +
                                        "WHERE username = '{0}' AND password = '{1}'",usernameText,passText);
            //ErrMsg.Text = cmdStr;

            SqlCommand myCommand;
            myCommand = new SqlCommand(cmdStr, myConn);    // setup your CMD and identify your connection

            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();   //capture your data 

            // prepare your data table 
            DataTable dt = new DataTable();                   // dont forget int32

            //V1
            dt.Columns.Add("SID", System.Type.GetType("System.String"));
            dt.Columns.Add("username", System.Type.GetType("System.String"));
            dt.Columns.Add("password", System.Type.GetType("System.String"));
            dt.Columns.Add("email", System.Type.GetType("System.String"));
            dt.Columns.Add("firstName", System.Type.GetType("System.String"));
            dt.Columns.Add("lastName", System.Type.GetType("System.String"));



            // row by row add to the data table 
            DataRow currentRow;
            while (myReader.Read())
            {
                currentRow = dt.NewRow();

                // currentRow is in connection to you dt object names / columns / rows from above
                // myReader data is in connection to your db names / columns / rows
                currentRow["SID"] = myReader["SID"].ToString();
                currentRow["username"] = myReader["username"].ToString();
                currentRow[2] = myReader["password"].ToString(); //can use index or column name
                currentRow["email"] = myReader[3].ToString();
                currentRow["firstName"] = myReader["firstName"].ToString();
                currentRow["LastName"] = myReader["LastName"].ToString();


                dt.Rows.Add(currentRow);
            }


            if(dt.Rows.Count != 1)
            {
                ErrMsg.Text = "Could not find the user with username and password, Try again..";
                myConn.Close();  // dont forget to close your db
            }
            else
            {

                //create staff object and save to session
                string SID = dt.Rows[0]["SID"].ToString();
                string username = dt.Rows[0]["username"].ToString();
                string firstName = dt.Rows[0]["firstName"].ToString();
                string lastName = dt.Rows[0]["lastName"].ToString();
                string email = dt.Rows[0]["email"].ToString();

                Staff st = new Staff(SID, username, email, firstName, lastName);

                AppSession.setStaff(st);

          
                myConn.Close();  // dont forget to close your db

                //redirect 
                Response.Redirect("StaffSearch.aspx");
            }


           


        }

        protected void ReturnBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
    }
}