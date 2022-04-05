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

        //int currentQuestionId = -1;
        //int terminator = -1;
        //DataTableHandler questionDth;
        //DataTableHandler questionValuesDth;
        //DataRow activeQuestionRow;

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                //testing console
                DevConsole.Visible = devConsoleVisibility;
                SurveyQuestionHolder.Visible = false;  // hide my template survey questions

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
                // PHASE 2 --- get Terminator value, first Question ID -------------------
                // -----------------------------------------------------------------------

                // data table class docs
                // https://docs.microsoft.com/en-us/dotnet/api/system.data.datatable?view=net-6.0

                int tempTerminator = (int)questionDt.Rows[0]["QID"];
                int tempCurrentQuestionId = (int)questionDt.Rows[0]["NextQID"];

                DevMessageLbl.Text = "terminator: " + tempTerminator.ToString();

                //set to static survey handler
                SurveyHandler.Terminator = tempTerminator;
                SurveyHandler.CurrentQuestionId = tempCurrentQuestionId;



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
                SurveyHandler.QuestionDth = new DataTableHandler(questionDt);
                SurveyHandler.QuestionValuesDth = new DataTableHandler(questionValuesDt);

                // Survey handler further setup
                SurveyHandler.RunningChildQuestions = false;
            }


        }



        //happens after button load
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // PHASE 4 -- Start the gameplay loop    ---------------
            // -----------------------------------------------------

            //setup data from our static class
            //on first run these will be set in firstLoop
            int currentQuestionId = SurveyHandler.CurrentQuestionId;
            int terminator = SurveyHandler.Terminator;
            DataTableHandler questionDth = SurveyHandler.QuestionDth;
            DataTableHandler questionValuesDth = SurveyHandler.QuestionValuesDth;
            bool runningChildQuestions = SurveyHandler.RunningChildQuestions;

            // create our gameplay loop
            if (!runningChildQuestions)
            {
                if (currentQuestionId != terminator)
                {

                    //get current question dt row
                    DataRow currentRow = questionDth.getRowByColumnNameAndIntValue("QID", currentQuestionId);

                    //run question
                    runQuestion(currentRow);

                    SurveyHandler.CurrentQuestionId = (int)currentRow["NextQID"];

                }
                else
                {
                    // end survey

                    //temp placeholder 
                    DevMessageLbl.Text = " END OF SURVEY / " + currentQuestionId.ToString();
                    hideAllQuestions(); // hide the previous question
                }
            }
            
            

        }



        public void runQuestion(DataRow currentRow) 
        {
            devConsoleGridUpdate(currentRow);

            //set asctiveQuestionRow  both in our static class and for use here
            SurveyHandler.ActiveQuestionRow = currentRow;
            DataRow activeQuestionRow = currentRow;

            //hide previous question whatever it may be
            hideAllQuestions();

            //set questionText and questiong number (QID)
            // this data uses shared controls / elements so it is always visable
            string qText = (string)activeQuestionRow["questionText"];
            int qId = (int)activeQuestionRow["QID"];
            assignQuestionTitles(qText, qId);

            //QTID
            // Determin Question Type ID
                //1 == TextBox
                //2 == RadioBtn
                //3 == CheckBox

            //Determine question Type
            switch ((int)activeQuestionRow["QTID_FK"])
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

        public void hideAllQuestions()
        {
            ItemTextBox.Visible = false;
            ItemRadioBtn.Visible = false;
            ItemCheckBox.Visible = false;
        }

        public void devConsoleGridUpdate(DataRow activeRow)
        {
            
            int qid = (int)activeRow["QID"];
            //update
            devQuestionGridView.DataSource = SurveyHandler.QuestionDth.getDataTableByColumnNameAndIntValue("QID", qid);
            devQuestionValuesGridView.DataSource = SurveyHandler.QuestionValuesDth.getDataTableByColumnNameAndIntValue("QID_FK", qid);
            //bind
            devQuestionGridView.DataBind();
            devQuestionValuesGridView.DataBind();
        }

        public void assignQuestionTitles(string questionText, int questionId)
        {
            QuestionNumLbl.Text = "Q" + questionId.ToString();
            //assign questText to QestionTextLabel
            QuestionTextLbl.Text = questionText;
        }




        //Build question to be shown within the SurveyContainer
        public void viewTextBox(DataRow activeQuestionRow)
        {
            //show question
            ItemTextBox.Visible = true;
        }

        public void viewRadioButton(DataRow activeQuestionRow)
        {
            //clear previous data 
            userSelectionRB.Items.Clear();

            //set question internal data
            //get data 
            int qId = (int)activeQuestionRow["QID"];
            ListItemCollection lic = SurveyHandler.QuestionValuesDth.getListItemCollectionByColumnNameAndIntValue("QID_FK", qId);
            //add data
            foreach (ListItem li in lic)
            {
                userSelectionRB.Items.Add(li);
            }

            //show question
            ItemRadioBtn.Visible = true;
        }

        public void viewCheckBox(DataRow activeQuestionRow)
        {
            
            //clear previous data 
            userSelectionCB.Items.Clear();

            //set question internal data
                //get data 
            int qId = (int)activeQuestionRow["QID"];
            ListItemCollection lic = SurveyHandler.QuestionValuesDth.getListItemCollectionByColumnNameAndIntValue("QID_FK", qId);
                //add data
            foreach (ListItem li in lic){
                userSelectionCB.Items.Add(li);
            }

            //show question
            ItemCheckBox.Visible = true;
        }


        // ---------------------------------
        // ---- EVENTS ---------------------
        // ---------------------------------
        protected void SubmitButtonTB_Click(object sender, EventArgs e)
        {
            //validate user response

            //submit activeQuestionRow 

            //nextQuestionCheck();
        }

        protected void SubmitButtonCB_Click(object sender, EventArgs e)
        {
            //validate user response

            //submit activeQuestionRow 


            //get the data we require
            DataRow activeQuestionRow = SurveyHandler.ActiveQuestionRow;
            int terminator = SurveyHandler.Terminator;
            int activeQID = (int)activeQuestionRow["QID"];
            DataTableHandler questionDth = SurveyHandler.QuestionDth;

            //turn on running child rows
            SurveyHandler.RunningChildQuestions = true;
            // loop though the users choices
            foreach (ListItem item in userSelectionCB.Items)
            {
                if (item.Selected)
                {
                    int nextQid = Int32.Parse(item.Value);
                    //V1
                    //check our active row to see if it has any child questions
                    if (nextQid != terminator)
                    {
                        // Get our next data row
                        DataRow nextRow = questionDth.getRowByColumnNameAndIntValue("QID", nextQid);
                        //Recursion --> start this proccess again --> FEED THE BEAST
                        runQuestion(nextRow);
                    }
                    else
                    {
                        //continue 
                    }
                }
            }

            //turn off runningChildQuestion
            SurveyHandler.RunningChildQuestions = false;

            //OLD
            //nextQuestionCheck();

        }

        protected void SubmitButtonRB_Click(object sender, EventArgs e)
        {
            //validate user response

            //submit activeQuestionRow 

            nextQuestionCheck();
        }




        // check to see if we can move to a new question within the current question
        public void nextQuestionCheck()
        {
            //get the data we require
            DataRow activeQuestionRow = SurveyHandler.ActiveQuestionRow;
            int terminator = SurveyHandler.Terminator;
            DataTableHandler questionDth = SurveyHandler.QuestionDth;

            //V1
            //check our active row to see if it has any child questions
            if ((int)activeQuestionRow["NextQID"] != terminator)
            {
                // Get our next data row
                int nextQid = (int)activeQuestionRow["NextQID"];
                DataRow nextRow = questionDth.getRowByColumnNameAndIntValue("QID", nextQid);
                //Recursion --> start this proccess again --> FEED THE BEAST
                runQuestion(nextRow);
            }
            else
            {
                //continue 
            }


            ////V2
            //// setup for the next loop
            //SurveyHandler.CurrentQuestionId = (int)activeQuestionRow["NextQID"];  //move to next question
            ////TEST
            //DevMessageLbl.Text = "CurrentQuestionID: " + ((int)activeQuestionRow["NextQID"]).ToString();


        }
    }
}