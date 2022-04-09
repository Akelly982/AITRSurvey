﻿using System;
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
        //Show DEV TEST CONSOLE
        bool devConsoleVisibility = true;

        

        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                //testing console
                DevConsole.Visible = devConsoleVisibility;
                SurveyQuestionHolder.Visible = false;  // hide my template survey questions


                //setup connection for db
                SqlConnection myConn = new SqlConnection();
                myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
                SqlCommand myCommand;

                // ----------------------------------------------------------
                // PHASE 0 --- generate a new Respondent id  ----------------
                // ----------------------------------------------------------

                //open connection
                myConn.Open();

                //cmd
                // dont forget your spaces in the sql cmd
                myCommand = new SqlCommand("INSERT INTO Respondent (dateAdded)" +
                                            " OUTPUT INSERTED.RID" +
                                            " VALUES(getDate())", myConn);

                //sql query Output INSERTED.id returns a dataset holding 1 row stating the value of given column 
                int rid = (int)myCommand.ExecuteScalar(); //must cast what the object should be
                
                //save generated 
                SurveyHandler.RespondentId = rid;

                //close db connection
                myConn.Close();

                //testing
                DevMessageLbl.Text = "new respondent id: " + rid.ToString();

                


                // ----------------------------------------------------------
                // PHASE 1 --- get our first question and Terminator values ---
                // ----------------------------------------------------------

                //db get the list of questions
                //rember the first row is our teminator


                myConn.Open(); // establish the connection to the db

                myCommand = new SqlCommand("SELECT QID,QTID_FK,questionText,NextQID,isParent FROM Questions", myConn);

                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();   //capture your data 

                // prepare your data table 
                DataTable questionDt = new DataTable();

                questionDt.Columns.Add("QID", System.Type.GetType("System.Int32"));
                questionDt.Columns.Add("QTID_FK", System.Type.GetType("System.Int32"));
                questionDt.Columns.Add("questionText", System.Type.GetType("System.String"));
                questionDt.Columns.Add("NextQID", System.Type.GetType("System.Int32"));
                questionDt.Columns.Add("isParent", System.Type.GetType("System.String"));  
           

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
                    currentRow["isParent"] = myReader["isParent"];    //db data type is bit, but successfully comes through as string as "False" or "True"

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

                DevMessageLbl.Text += " / terminator: " + tempTerminator.ToString();

                //set to static survey handler
                SurveyHandler.Terminator = tempTerminator;
                //SurveyHandler.CurrentQuestionId = tempCurrentQuestionId;
                SurveyHandler.ParentQuestionId = tempCurrentQuestionId;



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

                // ----------------------------------------------------------------
                // -----------  PHASE 3.5  Additional Updates --------------------- 
                // ----------------------------------------------------------------

                //init child question list of lists
                List<List<int>> childQuestionList = new List<List<int>>();
                SurveyHandler.ChildQuestionsList = childQuestionList;

                //init IpAddress
                SurveyHandler.IpAddress = getIpAddress();

                //test
                DevMessageLbl.Text += (" / " + SurveyHandler.IpAddress);

            }


        }

        public string getIpAddress()
        {
            //w3schools
            //https://www.w3schools.com/asp/coll_servervariables.asp

            //return (string)HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            return (string)HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        }



        //happens after button load
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            // -----------------------------------------------------
            // PHASE 4 -- Start the gameplay loop    ---------------
            // -----------------------------------------------------

            //setup data from our static class
            //on first run these will be set in firstLoop
            int parentQuestionId = SurveyHandler.ParentQuestionId;
            int terminator = SurveyHandler.Terminator;
            DataTableHandler questionDth = SurveyHandler.QuestionDth;
            DataTableHandler questionValuesDth = SurveyHandler.QuestionValuesDth;
            List<List<int>> childQuestionList = SurveyHandler.ChildQuestionsList;

            // create our gameplay loop
            if (childQuestionList.Count == 0)
            {

                runParentQuestion(parentQuestionId, terminator, questionDth);

            }
            else
            {
                //Run Child Question's
                // If the question is here their should be no terminator values

                //find the last child of questionGroup - List<int>
                int questionGroupIndex = childQuestionList.Count -1; //-1 will give us the index
                // if childQuestionGroup == 0 || -1 it will be cought above and below should never be out of range. 
                List<int> questionGroup = childQuestionList[questionGroupIndex];

                //find the last child of the question group - int/qid
                int questionIndex = questionGroup.Count - 1;

                if(questionIndex == -1) //no questions left (index 0 - 1 == -1)
                {
                    //Pop/Remove questionGroup
                    SurveyHandler.ChildQuestionsList.RemoveAt(questionGroupIndex);

                    //their are no child questions left so run the next parent question
                    runParentQuestion(parentQuestionId, terminator, questionDth);
                   
                }
                else
                {
                    int childQid = questionGroup[questionIndex];

                    //run question
                    //get current question dt row
                    DataRow childRow = questionDth.getRowByColumnNameAndIntValue("QID", childQid);
                    runQuestion(childRow);

                    //Pop/Remove question from questionGroup
                    SurveyHandler.ChildQuestionsList[questionGroupIndex].RemoveAt(questionIndex);
                }


                //test
                //devStrChildUpdate();

            }
            
            

        }



        // ---------------------------------------
        // ---------- Run Questions --------------
        // ---------------------------------------


        public void runParentQuestion(int parentQuestionId, int terminator, DataTableHandler questionDth)
        {
            
            if (parentQuestionId != terminator)
            {
                //get current question dt row
                DataRow currentRow = questionDth.getRowByColumnNameAndIntValue("QID", parentQuestionId);
                //run question
                runQuestion(currentRow);
                SurveyHandler.ParentQuestionId = (int)currentRow["NextQID"];
            }
            else
            {
                // end survey
                //temp placeholder 
                assignQuestionTitles("END OF SURVEY", parentQuestionId);
                DevMessageLbl.Text = " END OF SURVEY / " + parentQuestionId.ToString();
                hideAllQuestionsAndClear(); // hide the previous question
            }
        }


        public void runQuestion(DataRow currentRow) 
        {
            devConsoleGridUpdate(currentRow);

            //set asctiveQuestionRow  both in our static class and for use here
            SurveyHandler.ActiveQuestionRow = currentRow;
            DataRow activeQuestionRow = currentRow;

            //hide previous question whatever it may be
            hideAllQuestionsAndClear();

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

        public void hideAllQuestionsAndClear()
        {
            ItemTextBox.Visible = false;
            ItemRadioBtn.Visible = false;
            ItemCheckBox.Visible = false;

            userResultTB.Text = "";
            userSelectionCB.Items.Clear();
            userSelectionRB.Items.Clear();
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

            //submit question data to db


            //if question textbox is not a parent question add nextQID as a child item
            int terminator = SurveyHandler.Terminator;
            DataRow activeRow = SurveyHandler.ActiveQuestionRow;    //This row will be from the Questions Table not QuestionValues Table
            if((string)activeRow["isParent"] == "False")
            {
                //check for terminators
                if ((int)activeRow["NextQID"] != terminator) 
                {
                    //follow same setup as CB and RB questions
                        //questionGroup
                        //questions   <-- in this case it will be only one question not multiple 
                    List<int> questionGroup = new List<int>();
                    questionGroup.Add((int)activeRow["NextQID"]);

                    //update handler
                    SurveyHandler.ChildQuestionsList.Add(questionGroup);

                    //test
                    devStrChildUpdate();
                }

            }


            
            

        }

        protected void SubmitButtonCB_Click(object sender, EventArgs e)
        {
            //validate user response

            //submit question data to db



            ////V1
            //nextQuestionCheck();


            ////V2
            ////get the data we require
            //DataRow activeQuestionRow = SurveyHandler.ActiveQuestionRow;
            //int terminator = SurveyHandler.Terminator;
            //int activeQID = (int)activeQuestionRow["QID"];
            //DataTableHandler questionDth = SurveyHandler.QuestionDth;
            //// loop though the users choices
            //foreach (ListItem item in userSelectionCB.Items)
            //{
            //    if (item.Selected)
            //    {
            //        int nextQid = Int32.Parse(item.Value);
            //        //V1
            //        //check our active row to see if it has any child questions
            //        if (nextQid != terminator)
            //        {
            //            // Get our next data row
            //            DataRow nextRow = questionDth.getRowByColumnNameAndIntValue("QID", nextQid);
            //            //Recursion --> start this proccess again --> FEED THE BEAST
            //            runQuestion(nextRow);
            //        }
            //        else
            //        {
            //            //continue 
            //        }
            //    }
            //}


            //V3
            //get the data we require
            int terminator = SurveyHandler.Terminator;

            List<int> childQuestionGroup = new List<int>();
            // loop though the users choices
            foreach (ListItem item in userSelectionCB.Items)
            {
                if (item.Selected)  //add question / questions to childQuestionList
                {
                    int nextQid = Int32.Parse(item.Value);
                    //check our active row to see if it has any child questions
                    if (nextQid != terminator)  //check for terminators
                    {
                        //ensure nextQid is unique to the list
                            // can have multiple child questions returning the same question
                            // ensure you do not have duplicates
                        bool inListAlready = false;
                        foreach (int num in childQuestionGroup){
                            if(num == nextQid)
                            {
                                inListAlready = true;
                            }
                        }

                        // if question not already in list add to list
                        if (!inListAlready)
                        {
                            childQuestionGroup.Add(nextQid);  //add method adds at end of list 
                        }
                        
                    }
                    else
                    {
                        //continue 
                    }
                }
            }

            //Add our list of childQuestions to the Su
            SurveyHandler.ChildQuestionsList.Add(childQuestionGroup);

            //Test
            devStrChildUpdate();

        }

        protected void SubmitButtonRB_Click(object sender, EventArgs e)
        {
            //validate user response

            //submit question data to db

            int terminator = SurveyHandler.Terminator;

            List<int> childQuestionGroup = new List<int>();
            // loop though the users choices
            foreach (ListItem item in userSelectionRB.Items)
            {
                if (item.Selected)  //add question / questions to childQuestionList
                {
                    int nextQid = Int32.Parse(item.Value);
                    //check our active row to see if it has any child questions
                    if (nextQid != terminator)  //check for terminators
                    {
                        //radio btn will only ever return one child question at most
                        childQuestionGroup.Add(nextQid);  //add method adds at end of list 
                    }
                    else
                    {
                        //continue 
                    }
                }
            }

            //Add our list of childQuestions to the Su
            SurveyHandler.ChildQuestionsList.Add(childQuestionGroup);

            //Test
            devStrChildUpdate();
            
        }



        public void devStrChildUpdate()
        {
            string devStr = "";
            foreach (List<int> questionGroup in SurveyHandler.ChildQuestionsList)  //list
            {
                foreach (int question in questionGroup) // integer
                {
                    devStr += " /" + question.ToString();
                }
            }
            DevMessageLbl.Text = devStr;
        }






        //Generic function
        // check to see if we can move to a new question within the current question
        // this is handled by the parent loop in most cases
        public void nextQuestionCheck()
        {
            ////get the data we require
            //DataRow activeQuestionRow = SurveyHandler.ActiveQuestionRow;
            //int terminator = SurveyHandler.Terminator;
            //DataTableHandler questionDth = SurveyHandler.QuestionDth;

            ////V1
            ////check our active row to see if it has any child questions
            //if ((int)activeQuestionRow["NextQID"] != terminator)
            //{
            //    // Get our next data row
            //    int nextQid = (int)activeQuestionRow["NextQID"];
            //    DataRow nextRow = questionDth.getRowByColumnNameAndIntValue("QID", nextQid);
            //    //Recursion --> start this proccess again --> FEED THE BEAST
            //    runQuestion(nextRow);
            //}
            //else
            //{
            //    //continue 
            //}


            ////V2
            //// setup for the next loop
            //SurveyHandler.CurrentQuestionId = (int)activeQuestionRow["NextQID"];  //move to next question
            ////TEST
            //DevMessageLbl.Text = "CurrentQuestionID: " + ((int)activeQuestionRow["NextQID"]).ToString();


        }

        protected void HiddenSubmit_Click(object sender, EventArgs e)
        {
            // hidden submit
        }
    }
}