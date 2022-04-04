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

        int currentQuestionId = -1;
        int terminator = -1;
        int questionGap = -1;

        DataTableHandler questionDth;
        DataTableHandler questionValuesDth;

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
                DataTable questionDt = new DataTable();

                questionDt.Columns.Add("QID", System.Type.GetType("System.Int32"));
                questionDt.Columns.Add("QTID_FK", System.Type.GetType("System.Int32"));
                questionDt.Columns.Add("questionText", System.Type.GetType("System.String"));
                questionDt.Columns.Add("NextQID", System.Type.GetType("System.Int32"));



                // row by row add to the data table 
                DataRow currentRow;
                while (myReader.Read())
                {
                    currentRow = questionDt.NewRow();

                    // can use column name or index
                    currentRow["QID"] = myReader["QID"];
                    currentRow["QTID_FK"] = myReader["QTID_FK"];
                    currentRow["questionText"] = myReader["questionText"].ToString();
                    currentRow["NextQID"] = myReader["NextQID"];

                    questionDt.Rows.Add(currentRow);
                }

                //close db connection
                myConn.Close();  // dont forget to close your db




                // -----------------------------------------------------------------------
                // PHASE 2 --- get Terminator value, first Question ID and QuestionGap ---
                // -----------------------------------------------------------------------

                // data table class docs
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-6.0

                terminator = (int)questionDt.Rows[0]["QID"];
                questionGap = (int)questionDt.Rows[0]["NextQID"];
                currentQuestionId = questionGap;

                DevMessageLbl.Text = "terminator: " + terminator.ToString() + "  /  questionGap: " + questionGap.ToString();




                // -----------------------------------------------------------------------
                // PHASE 3 --- get our last dataTable questionValues     ---------------
                // -----------------------------------------------------------------------

                //get the questionValues table from the db
                myConn.Open(); // establish the connection to the db

                myCommand = new SqlCommand("SELECT QVID, QID_FK, text, NextQID FROM QuestionValues", myConn);
                myReader = myCommand.ExecuteReader();   //capture your data 

                // prepare your data table 
                DataTable questionValuesDt = new DataTable();

                questionValuesDt.Columns.Add("QVID", System.Type.GetType("System.Int32"));
                questionValuesDt.Columns.Add("QID_FK", System.Type.GetType("System.Int32"));
                questionValuesDt.Columns.Add("text", System.Type.GetType("System.String"));
                questionValuesDt.Columns.Add("NextQID", System.Type.GetType("System.Int32"));



                // row by row add to the data table 

                //currentRow already set above
                while (myReader.Read())
                {
                    currentRow = questionValuesDt.NewRow();

                    // can use column name or index
                    currentRow["QVID"] = myReader["QVID"];
                    currentRow["QID_FK"] = myReader["QID_FK"];
                    currentRow["text"] = myReader["text"].ToString();
                    currentRow["NextQID"] = myReader["NextQID"];

                    questionValuesDt.Rows.Add(currentRow);
                }

                //close db connection
                myConn.Close();  // dont forget to close your db


                // send to dev console
                if(devConsoleVisibility != false) 
                {
                    devQuestionGridView.DataSource = questionDt;
                    devQuestionValuesGridView.DataSource = questionValuesDt;
                    devQuestionGridView.DataBind();        // on bind the grid view object is refreshed to show the data
                    devQuestionValuesGridView.DataBind();

                    //testing my datatable class
                    //DataTableHandler dth = new DataTableHandler(questionValuesDT);
                    //DataTable testDt = dth.getRowsByColumnNameAndIntValue("QID_FK", 200);
                    //devQuestionValuesGridView.DataSource = testDt;
                    //devQuestionValuesGridView.DataBind();
                }


                //set datatables to the DynamicSurvey class
                this.questionDth = new DataTableHandler(questionDt);
                this.questionValuesDth = new DataTableHandler(questionValuesDt);

            }


            // -----------------------------------------------------
            // PHASE 4 -- Start the gameplay loop    ---------------
            // -----------------------------------------------------

            // create our gameplay loop
            while (currentQuestionId != terminator)
            {

                //get current question dt row
                DataRow currentRow = questionDth.getRowByColumnNameAndIntValue("QID", currentQuestionId);

                //run question
                runQuestion(currentRow);

                //increment to NextQID based on current row
                currentQuestionId = (int)currentRow["NextQID"];


            }


        }



        public void runQuestion(DataRow activeQuestionRow) 
        {

            //QTID
            // Question Type ID
                //1 == TextBox
                //2 == RadioBtn
                //3 == CheckBox

            //Determine question Type
            switch ((int)activeQuestionRow["QTID"])
            {
                case 1:
                    //TextBox
                    viewTextBox(activeQuestionRow);
                    break;

                case 2:
                    //RadioBtn
                    viewRadioButton(activeQuestionRow);
                    break;

                case 3:
                    //CheckBox
                    viewCheckBox(activeQuestionRow);
                    break;

                default:
                    DevMessageLbl.Text = "ERROR / QID : " + ((int)activeQuestionRow["QTID"]).ToString() + "/ QTID : " + ((int)activeQuestionRow["QTID"]).ToString();
                    break;
            }
            

        
        }

        public void viewTextBox(DataRow activeQuestionRow)
        {

        }

        public void viewRadioButton(DataRow activeQuestionRow)
        {

        }

        public void viewCheckBox(DataRow activeQuestionRow)
        {
            
        }




    }
}