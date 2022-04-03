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
    public partial class DynamicSurvey : System.Web.UI.Page
    {
        bool devConsoleVisibility = true;

        int terminator = -1;
        int questionGap = -1;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DevConsole.Visible = devConsoleVisibility;

                // ----------------------------------------------------------
                // PHASE 1 --- get our questionsGap and Terminator values ---
                // ----------------------------------------------------------

                //db get the list of questions
                //rember the first row is our teminator

                SqlConnection myConn = new SqlConnection();
                myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
                myConn.Open(); // establish the connection to the db

                SqlCommand myCommand;
                myCommand = new SqlCommand("SELECT TOP 1 QID,QTID_FK,questionText,NextQID FROM Questions", myConn);

                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();   //capture your data 

                // prepare your data table 
                DataTable dt = new DataTable();

                dt.Columns.Add("QID", System.Type.GetType("System.Int32"));
                dt.Columns.Add("QTID_FK", System.Type.GetType("System.Int32"));
                dt.Columns.Add("questionText", System.Type.GetType("System.String"));
                dt.Columns.Add("NextQID", System.Type.GetType("System.Int32"));



                // row by row add to the data table 
                DataRow currentRow;
                while (myReader.Read())
                {
                    currentRow = dt.NewRow();

                    // can use column name or index
                    currentRow["QID"] = myReader["QID"];
                    currentRow["QTID_FK"] = myReader["QTID_FK"];
                    currentRow["questionText"] = myReader["questionText"].ToString();
                    currentRow["NextQID"] = myReader["NextQID"];

                    dt.Rows.Add(currentRow);
                }

                //close db connection
                myConn.Close();  // dont forget to close your db



                // ------------------------------------
                // PHASE 2 --- get Terminator value ---
                // ------------------------------------

                // data table class docs
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-6.0

                terminator = (int)dt.Rows[0]["QID"];
                questionGap = (int)dt.Rows[0]["NextQID"];

                DevConsoleLbl.Text = "terminator: " + terminator.ToString() + "  /  questionGap: " + questionGap.ToString();



            }


        }
    }
}