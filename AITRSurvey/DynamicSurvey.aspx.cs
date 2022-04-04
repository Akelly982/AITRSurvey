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

        int currentQuestion = -1;
        int terminator = -1;
        int questionGap = -1;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //testing console
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
                myCommand = new SqlCommand("SELECT QID,QTID_FK,questionText,NextQID FROM Questions", myConn);

                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();   //capture your data 

                // prepare your data table 
                DataTable questionDT = new DataTable();

                questionDT.Columns.Add("QID", System.Type.GetType("System.Int32"));
                questionDT.Columns.Add("QTID_FK", System.Type.GetType("System.Int32"));
                questionDT.Columns.Add("questionText", System.Type.GetType("System.String"));
                questionDT.Columns.Add("NextQID", System.Type.GetType("System.Int32"));



                // row by row add to the data table 
                DataRow currentRow;
                while (myReader.Read())
                {
                    currentRow = questionDT.NewRow();

                    // can use column name or index
                    currentRow["QID"] = myReader["QID"];
                    currentRow["QTID_FK"] = myReader["QTID_FK"];
                    currentRow["questionText"] = myReader["questionText"].ToString();
                    currentRow["NextQID"] = myReader["NextQID"];

                    questionDT.Rows.Add(currentRow);
                }

                //close db connection
                myConn.Close();  // dont forget to close your db




                // -----------------------------------------------------------------------
                // PHASE 2 --- get Terminator value, first Question ID and QuestionGap ---
                // -----------------------------------------------------------------------

                // data table class docs
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-6.0

                terminator = (int)questionDT.Rows[0]["QID"];
                questionGap = (int)questionDT.Rows[0]["NextQID"];
                currentQuestion = questionGap;

                DevConsoleLbl.Text = "terminator: " + terminator.ToString() + "  /  questionGap: " + questionGap.ToString();




                // -----------------------------------------------------------------------
                // PHASE 3 --- get our last dataTable questionValues     ---------------
                // -----------------------------------------------------------------------

                //get the questionValues table from the db
                myConn.Open(); // establish the connection to the db

                myCommand = new SqlCommand("SELECT QVID, QID_FK, text, NextQID FROM QuestionValues", myConn);
                myReader = myCommand.ExecuteReader();   //capture your data 

                // prepare your data table 
                DataTable questionValuesDT = new DataTable();

                questionValuesDT.Columns.Add("QVID", System.Type.GetType("System.Int32"));
                questionValuesDT.Columns.Add("QID_FK", System.Type.GetType("System.Int32"));
                questionValuesDT.Columns.Add("text", System.Type.GetType("System.String"));
                questionValuesDT.Columns.Add("NextQID", System.Type.GetType("System.Int32"));



                // row by row add to the data table 

                //currentRow already set above
                while (myReader.Read())
                {
                    currentRow = questionValuesDT.NewRow();

                    // can use column name or index
                    currentRow["QVID"] = myReader["QVID"];
                    currentRow["QID_FK"] = myReader["QID_FK"];
                    currentRow["text"] = myReader["text"].ToString();
                    currentRow["NextQID"] = myReader["NextQID"];

                    questionValuesDT.Rows.Add(currentRow);
                }

                //close db connection
                myConn.Close();  // dont forget to close your db


                // send to dev console
                if(devConsoleVisibility != false) 
                {
                    devQuestionGridView.DataSource = questionDT;
                    devQuestionValuesGridView.DataSource = questionValuesDT;
                    devQuestionGridView.DataBind();        // on bind the grid view object is refreshed to show the data
                    devQuestionValuesGridView.DataBind();

                    //testing my datatable class
                    DataTableHandler dth = new DataTableHandler(questionValuesDT);
                    DataTable testDt = dth.getRowsByColumnNameAndIntValue("QID_FK", 200);
                    devQuestionValuesGridView.DataSource = testDt;
                    devQuestionValuesGridView.DataBind();
                }
               




            }


            //while(currentQuestion != terminator)
            //{
            //    runQuestion();
            //    currentQuestion = 


            //}


        }



        public void runQuestion() 
        {
            
        
        }




    }
}