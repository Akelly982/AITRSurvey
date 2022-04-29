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
        //Show DEV TEST CONSOLE
        bool devConsoleVisibility = false;

        //Constants resembeling our questionTypes and their ID's within the database
        const int ITEM_TEXTBOX = 1;
        const int ITEM_RADIOBUTTON = 2;
        const int ITEM_CHECKBOX = 3;
        const int ITEM_TEXTBOX_PHONE = 4;
        const int ITEM_TEXTBOX_EMAIL = 5;
        const int ITEM_TEXTBOX_DATE = 6;
        const int ITEM_TEXTBOX_POSTCODE = 7;


        protected void Page_Load(object sender, EventArgs e)
        {

            // inital data retrieval
                // I get our sql tables and save them as data tables
            if (!IsPostBack)
            {
                //testing console
                DevConsole.Visible = devConsoleVisibility;


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
                if (devConsoleVisibility != false)
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
                // -----------  PHASE 3.5  Additional Handler Updates --------------------- 
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



        /// <summary>
        ///     access httpContext.Current.Request.ServerVariables and get our IP address
        /// </summary>
        /// <returns> a string consiting of our ip address </returns>
        public string getIpAddress()
        {
            //w3schools
            //https://www.w3schools.com/asp/coll_servervariables.asp

            //return (string)HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            return (string)HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

        }



        //happens after button load
        /// <summary>
        ///     OUR MAIN LOOP
        ///     Here we look for childQuestions and if their are none we run our parent questions.
        /// </summary>
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
                int questionGroupIndex = childQuestionList.Count - 1; //-1 will give us the index
                // if childQuestionGroup == 0 || -1 it will be cought above and below should never be out of range. 
                List<int> questionGroup = childQuestionList[questionGroupIndex];

                //find the last child of the question group - int/qid
                int questionIndex = questionGroup.Count - 1;

                if (questionIndex == -1) //no questions left (index 0 - 1 == -1)
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

                    //if questionGroup is now empty 
                    //pop the question group
                    if(childQuestionList[questionGroupIndex].Count == 0)
                    {
                        childQuestionList.RemoveAt(questionGroupIndex);
                    }
                    else
                    {
                        //still has children to process
                    }
                    
                }


                //test
                //devStrChildUpdate();

            }



        }



        // ---------------------------------------
        // ---------- Run Questions --------------
        // ---------------------------------------
        /// <summary>
        ///     Here we get our parent question and run it through runQuestion just like any other question
        ///     This is detached as a function from the pageLoad complete so that I can call the
        ///     runParentQuestion function in 2 usecases
        ///         1: their are no Questions in our ChildQuestionList.
        ///         2: their is a questionGroup but it is has no question children remaining.
        /// </summary>
        /// <param name="parentQuestionId"> An integer holding the QID</param>
        /// <param name="terminator">An integer holding our terminator value used to show end points to sub questions</param>
        /// <param name="questionDth">The sql questions table held as a datatable within my dataTableHandler class </param>
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

                Response.Redirect("SurveyEnd.aspx");

                //temp placeholder 
                //assignQuestionTitles("END OF SURVEY", parentQuestionId);
                //DevMessageLbl.Text = " END OF SURVEY / " + parentQuestionId.ToString();
                //hideAllQuestionsAndClear(); // hide the previous question


            }
        }

        /// <summary>
        ///     using the currentRow / currentQuestionRow from the questionsDT we show the respective question type and data to the user
        ///     This is the bread and butter of our application
        ///         clear/hide previous question data 
        ///         show user current question data
        ///         determine question type
        ///             fill question type with its required data
        ///         
        ///         wait for user response as specified by question type
        /// </summary>
        /// <param name="currentRow"> A dataTable Row from our questions DataTable with our current question data</param>
        public void runQuestion(DataRow currentRow)
        {
            devConsoleGridUpdate(currentRow);

            //set asctiveQuestionRow  both in our static class and for use here
            SurveyHandler.ActiveQuestionRow = currentRow;
            DataRow activeQuestionRow = currentRow;

            //hide previous question whatever it may be
            hideAllQuestionsAndClear();

            //turn off all validator controls from the designer
            turnOffAllValidators();

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
            //4 == TextBox_Phone
            //5 == TextBox_Email
            //6 == TextBox_Date
            //7 == TextBox_PostCode


            //Determine question Type
            switch ((int)activeQuestionRow["QTID_FK"])
            {
                case ITEM_TEXTBOX:
                    //TextBox
                    viewTextBox(activeQuestionRow);
                    break;

                case ITEM_RADIOBUTTON:
                    //RadioBtn
                    viewRadioButton(activeQuestionRow);
                    break;

                case ITEM_CHECKBOX:
                    //CheckBox
                    viewCheckBox(activeQuestionRow);
                    break;

                case ITEM_TEXTBOX_PHONE:
                    //TextBox
                    viewTextBoxPhone(activeQuestionRow);
                    break;

                case ITEM_TEXTBOX_EMAIL:
                    //TextBox
                    viewTextBoxEmail(activeQuestionRow);
                    break;

                case ITEM_TEXTBOX_DATE:
                    //TextBox
                    viewTextBoxDate(activeQuestionRow);
                    break;

                case ITEM_TEXTBOX_POSTCODE:
                    //TextBox
                    viewTextBoxPostCode(activeQuestionRow);
                    break;

                default:
                    DevMessageLbl.Text = "ERROR / QID : " + ((int)activeQuestionRow["QTID"]).ToString() + "/ QTID : " + ((int)activeQuestionRow["QTID"]).ToString();
                    break;
            }

        }


        /// <summary>
        ///   hides all question types and clear for potential reuse later
        /// </summary>
        public void hideAllQuestionsAndClear()
        {
            //turn all visiblitiys off
            ItemTextBox.Visible = false;
            ItemRadioBtn.Visible = false;
            ItemCheckBox.Visible = false;
            ItemTextBoxDate.Visible = false;
            ItemTextBoxEmail.Visible = false;
            ItemTextBoxPhone.Visible = false;
            ItemTextBoxPostCode.Visible = false;

            //clear textbox fields
            userResultTB.Text = "";
            userResultTBDate.Text = "";
            userResultTBEmail.Text = "";
            userResultTBPhone.Text = "";
            userResultTBPostCode.Text = "";
            //empty item lists rb / cb
            userSelectionCB.Items.Clear();
            userSelectionRB.Items.Clear();
        }


        /// <summary>
        ///     turn off all validators
        /// </summary>
        public void turnOffAllValidators()
        {
            //checks is textbox is not empty
            textBoxRequiredFieldValidator.Enabled = false;
    
            // checks with regular expressions
            //phone
            textBoxPhoneREV.Enabled = false;
            textBoxPhoneEmptyREV.Enabled = false;
            //email
            textBoxEmailREV.Enabled = false;
            textBoxEmailEmptyREV.Enabled = false;
            //date
            textBoxDateREV.Enabled = false;
            textBoxDateEmptyREV.Enabled = false;
            //postcode 
            textBoxPostCodeREV.Enabled = false;
            textBoxPostCodeEmptyREV.Enabled = false;


            //validator visibility to avoid style issues
                //otherwise you get issuess in your style
            textBoxRequiredFieldValidator.Visible = false;
            textBoxPhoneREV.Visible = false;
            textBoxPhoneEmptyREV.Visible = false;
            textBoxEmailREV.Visible = false;
            textBoxEmailEmptyREV.Visible = false;
            textBoxDateREV.Visible = false;
            textBoxDateEmptyREV.Visible = false;
            textBoxPostCodeREV.Visible = false;
            textBoxPostCodeEmptyREV.Visible = false;

        }


        /// <summary>
        ///    gets information regarding our active question row and display to devConsole
        ///    should show our active questions question and question values tables for help
        ///    understanding our system's thought process
        /// </summary>
        /// <param name="activeRow"> Our active data row  holding our question data</param>
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


        /// <summary>
        ///     all questions have a questionId and a questionText
        ///     this sets the labels displaying it to the user.
        /// </summary>
        /// <param name="questionText"> a string representing the question text</param>
        /// <param name="questionId"> holds the questions QID </param>
        public void assignQuestionTitles(string questionText, int questionId)
        {
            QuestionNumLbl.Text = "Q" + questionId.ToString();
            //assign questText to QestionTextLabel
            QuestionTextLbl.Text = questionText;
        }




        //view for the selected question to be shown within the SurveyContainer
        /// <summary>
        ///     show the given questionItem to the user
        ///     and activate validators
        ///         not empty
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewTextBox(DataRow activeQuestionRow)
        {
            //show question
            ItemTextBox.Visible = true;

            //show validator and enable
            textBoxRequiredFieldValidator.Visible = true;
            textBoxRequiredFieldValidator.Enabled = true;
        }



        /// <summary>
        ///     using our active row qid we search for its connected questionValues
        ///     we get these as a listItemCollection and add them to our RadioButtonList.
        ///     
        ///     Side Note: it is important for radio buttons that the value is not the same as other list items
        ///     otherwise you get an error where no matter what you select its the top choice.
        ///     To get around this for multiple radiobuttons with the same NextQId I index to a list and compare
        ///     it againts our radioButtionList later.    <---- you will find remenants of this is the SurveyHandler class
        ///     
        /// 
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewRadioButton(DataRow activeQuestionRow)
        {

            //set question internal data
            //get data 
            int qId = (int)activeQuestionRow["QID"];
            ListItemCollection lic = SurveyHandler.QuestionValuesDth.getListItemCollectionByColumnNameAndIntValue("QID_FK", qId);



            // ADD DATA ERROR HANDELING   
                // below testing is important as it displays an issue within the rblist 
                // that does not exist in the checkbox list controls
            ////testing 
            //// NOTE 
            //// radioButton values need to be different from each other otherwise the result 
            //// will always be index 0, i assume it isSelected is connected to the item.value despite it being a string
            //// Below
            //    //(A) works
            //    //(B) will cause selection to always be index 0
            ////(A)
            //for (int i = 0; i<5; i++)
            //{
            //    ListItem li = new ListItem();
            //    li.Text = "option " + i.ToString();
            //    li.Value = i.ToString();

            //    userSelectionRB.Items.Add(li);
            //}
            ////(B)
            //for (int i = 0; i < 5; i++)
            //{
            //    ListItem li = new ListItem();
            //    li.Text = "option " + i.ToString();
            //    li.Value = "99";

            //    userSelectionRB.Items.Add(li);
            //}


            //add data
            List<string> rbNextQIDList = new List<string>(); 
            int index = 0;   // <-- first index is 0

            foreach (ListItem li in lic)
            {
                //save ou nextQID to list
                rbNextQIDList.Add(li.Value);
                ListItem item = new ListItem();

                //replace value with index location in list
                item.Text = li.Text;
                item.Value = index.ToString();

                //add to LIC
                userSelectionRB.Items.Add(item);

                //increment index
                index++;
            }

            //once complete add rbNextQIDList to handler class
            SurveyHandler.RadioButtonNextQidList = rbNextQIDList;

            //preset value to an index so it is not unselected by default
            userSelectionRB.SelectedIndex = 0;

            //show question
            ItemRadioBtn.Visible = true;
        }

        /// <summary>
        ///     using our active row qid we search for its connected questionValues
        ///     we get these as a listItemCollection and add them to our CheckBox.
        ///     
        ///     Note: checkbox does not have issue with multiple same values (hence it is more staight forward)
        ///  
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewCheckBox(DataRow activeQuestionRow)
        {

            //set question internal data
            //get data 
            int qId = (int)activeQuestionRow["QID"];
            ListItemCollection lic = SurveyHandler.QuestionValuesDth.getListItemCollectionByColumnNameAndIntValue("QID_FK", qId);
            //add data
            foreach (ListItem li in lic) {
                userSelectionCB.Items.Add(li);
            }

            //preset value to an index so it is not unselected by default
            userSelectionCB.SelectedIndex = 0;

            //show question
            ItemCheckBox.Visible = true;
        }




        //--------------------------------------
        // additonal text box views with regex 
        //--------------------------------------

        /// <summary>
        ///     show the given questionItem to the user
        ///     and activate validators
        ///         not empty
        ///         regex
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewTextBoxPhone(DataRow activeQuestionRow)
        {
            //show question
            ItemTextBoxPhone.Visible = true;

            //show connected validators and enable
            textBoxPhoneEmptyREV.Visible = true;
            textBoxPhoneREV.Visible = true;
            textBoxPhoneEmptyREV.Enabled = true;
            textBoxPhoneREV.Enabled = true;
        }

        /// <summary>
        ///     show the given questionItem to the user
        ///     and activate validators
        ///         not empty
        ///         regex
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewTextBoxEmail(DataRow activeQuestionRow)
        {
            //show question
            ItemTextBoxEmail.Visible = true;

            //show connected validators and enable
            textBoxEmailEmptyREV.Visible = true;
            textBoxEmailREV.Visible = true;
            textBoxEmailEmptyREV.Enabled = true;
            textBoxEmailREV.Enabled = true;
        }

        /// <summary>
        ///     show the given questionItem to the user
        ///     and activate validators
        ///         not empty
        ///         regex
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewTextBoxDate(DataRow activeQuestionRow)
        {
            //show question
            ItemTextBoxDate.Visible = true;

            //show connected validators and enable
            textBoxDateEmptyREV.Visible = true;
            textBoxDateREV.Visible = true;
            textBoxDateEmptyREV.Enabled = true;
            textBoxDateREV.Enabled = true;
        }

        /// <summary>
        ///     show the given questionItem to the user
        ///     and activate validators
        ///         not empty
        ///         regex
        /// </summary>
        /// <param name="activeQuestionRow"> dataRow of our active question from the question data table</param>
        public void viewTextBoxPostCode(DataRow activeQuestionRow)
        {
            //show question
            ItemTextBoxPostCode.Visible = true;

            //show connected validators and enable
            textBoxPostCodeEmptyREV.Visible = true;
            textBoxPostCodeREV.Visible = true;
            textBoxPostCodeEmptyREV.Enabled = true;
            textBoxPostCodeREV.Enabled = true;
        }






        // ---------------------------------
        // ---- EVENTS ---------------------
        // ---------------------------------


        /// <summary>
        ///     submit for standard text box
        ///     Note: all textboxes share the same submitTextBoxFunction
        /// </summary>
        protected void SubmitButtonTB_Click(object sender, EventArgs e)
        {
            //validation is done with validator controls in designer

            //submit question data to db
            string response = userResultTB.Text;  // user enter data TextBox
            submitTextBox(response);

        }

        /// <summary>
        ///     submit for standard text box
        ///     Note: all textboxes share the same submitTextBoxFunction
        /// </summary>
        protected void SubmitButtonPhoneTB_Click(object sender, EventArgs e)
        {
            //validation is done with validator controls in designer

            //submit question data to db
            string response = userResultTBPhone.Text;  // user enter data TextBox
            submitTextBox(response);

        }

        /// <summary>
        ///     submit for standard text box
        ///     Note: all textboxes share the same submitTextBoxFunction
        /// </summary>
        protected void SubmitButtonEmailTB_Click(object sender, EventArgs e)
        {
            //validation is done with validator controls in designer

            //submit question data to db
            string response = userResultTBEmail.Text;  // user enter data TextBox
            submitTextBox(response);

        }

        /// <summary>
        ///     submit for standard text box
        ///     Note: all textboxes share the same submitTextBoxFunction
        /// </summary>
        protected void SubmitButtonDateTB_Click(object sender, EventArgs e)
        {
            //validation is done with validator controls in designer

            //submit question data to db
            string response = userResultTBDate.Text;  // user enter data TextBox
            submitTextBox(response);

        }

        /// <summary>
        ///     submit for standard text box
        ///     Note: all textboxes share the same submitTextBoxFunction
        /// </summary>
        protected void SubmitButtonPostCodeTB_Click(object sender, EventArgs e)
        {
            //validation is done with validator controls in designer

            //submit question data to db
            string response = userResultTBPostCode.Text;  // user enter data TextBox
            submitTextBox(response);

        }



        //shared between all text box's
        /// <summary>
        ///     takes the users text box response input and submits it to the database in connection to
        ///     activeQID, usersRID, and their given usersIpAddress
        ///     
        ///     if NextQID != terminator
        ///         determins if the textbox question is a parent question or not 
        ///             if not the NextQID is added to the childQuestionList as a QuestionGroup with one NextQID int value
        /// </summary>
        /// <param name="response"> A string representing the users input </param>
        public void submitTextBox(string response)
        {
            questionSubmissionToDataBase((int)SurveyHandler.ActiveQuestionRow["QID"],
                                         SurveyHandler.RespondentId,
                                         response,
                                         SurveyHandler.IpAddress);


            //----------- check for child questions -----------------------
            //if question textbox is not a parent question add nextQID as a child item
            int terminator = SurveyHandler.Terminator;
            DataRow activeRow = SurveyHandler.ActiveQuestionRow;
            if ((string)activeRow["isParent"] == "False")
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


        /// <summary>
        ///     loops through the checkbox items to find the users selection if any or multiple.
        ///     with this we create a CSV string of all the users selections creating their response string
        ///     this is then submitted to the submissions table within the database. 
        ///     In connection to activeQID, usersRID and usersIpAdress.  
        ///     
        ///     for each item selected if NextQID is not the terminator
        ///     add item selected NextQID to QuestionGroup
        ///     if questionGroup not empty
        ///     append QuestionGroup to ChildQuestionList
        ///    
        /// </summary>
        protected void SubmitButtonCB_Click(object sender, EventArgs e)
        {

            //submit question data to db ---------------------------
                // as csv answear1,answear2,answear3
            //create csv response
            String response = "";
            foreach (ListItem item in userSelectionCB.Items)
            {
                if (item.Selected)
                {
                    response += item.Text + ",";
                }
            }

            if (response.Length != 0)  //somthing was added
            { 
                // someItem,someItem,
                //  - remove the comma from the last item entered
                response = stringRemoveLastChar(response);
            }
            else
            {
                // with a check box it is possible to select nothing 
                // if their is nothing selected just respond with...
                response = "None"; 
            }

            questionSubmissionToDataBase((int)SurveyHandler.ActiveQuestionRow["QID"],
                                            SurveyHandler.RespondentId,
                                            response,
                                            SurveyHandler.IpAddress);

            //----------- check for child questions -----------------------
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
                        foreach (int num in childQuestionGroup) {
                            if (num == nextQid)
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


            //if not an empty list of ints add to survey handler question list
            checkChildQuestionList(childQuestionGroup);

            //Test
            devStrChildUpdate();

        }


        /// <summary>
        ///     loops through the Radio Button items to find the users selection.
        ///         Should only be one item
        ///     with this we create a CSV string of all the users selections creating their response string
        ///     this is then submitted to the submissions table within the database. 
        ///     In connection to activeQID, usersRID and usersIpAdress.  
        ///     
        ///     Note: 
        ///         items in Radio button list have text and value
        ///             text: represtents the user shown choice
        ///             value: value represents a string index position for List<string> radioButtonNextQidList from surveyHandler
        ///         This is how i get around multiple rb items sharing the same NextQID value.
        ///         so for the selected item within the radio button list get value an use it
        ///         as the index position in radioButtonNextQIDList
        ///         
        ///    once you have sorted out the above
        ///    
        ///    loop
        ///     if selected item != terminator value 
        ///         add NextQID to questionGroup
        ///         if questionGroup != empty 
        ///         append questionGroup to ChildQuestionList
        ///    
        /// </summary>
        protected void SubmitButtonRB_Click(object sender, EventArgs e)
        {
            //submit question data to db
            // as csv answear1 but should only result to one answear
            //create csv response

            String response = "";
            foreach (ListItem item in userSelectionRB.Items)
            {
                if (item.Selected)
                {
                    response += item.Text + ",";
                }
            }
            if (response.Length != 0)  //somthing was added
            {
                // someItem,someItem,
                //  - remove the comma from the last item entered
                response = stringRemoveLastChar(response);
            }
            else
            {
                // With a radio button this should not be possible
                // however if their is nothing selected just respond with...
                response = "None";
            }

            questionSubmissionToDataBase((int)SurveyHandler.ActiveQuestionRow["QID"],
                                            SurveyHandler.RespondentId,
                                            response,
                                            SurveyHandler.IpAddress);



            ////----------- check for child questions -----------------------
            int terminator = SurveyHandler.Terminator;
            List<string> rbActuralValues = SurveyHandler.RadioButtonNextQidList; //dont forget about this RB NextQID value list 

            List<int> childQuestionGroup = new List<int>();
            // loop though the users choices
            foreach (ListItem item in userSelectionRB.Items)
            {
                if (item.Selected)  //add question / questions to childQuestionList
                {

                    int nextQid = Int32.Parse(rbActuralValues[Int32.Parse(item.Value)]);
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


            //if not an empty list of ints add to survey handler question list
            checkChildQuestionList(childQuestionGroup);

            //Test
            devStrChildUpdate();

        }


        /// <summary>
        ///     Take child question group and if not empty add it to our overall childQuestionList
        /// </summary>
        /// <param name="childQuestionGroup"> A list of integers representing child questions of the active question </param>
        public void checkChildQuestionList(List<int> childQuestionGroup)
        {
            //if not empty
            if (childQuestionGroup.Count != 0)
            {
                //add to handler list of child question lists
                SurveyHandler.ChildQuestionsList.Add(childQuestionGroup);
            }
        }


        /// <summary>
        ///     takes the shared child question list and writes it out the the devConsole
        /// </summary>
        public void devStrChildUpdate()
        {
            string devStr = "";
            foreach (List<int> questionGroup in SurveyHandler.ChildQuestionsList)  //list
            {
                foreach (int question in questionGroup)  //integer
                {
                    devStr += " /" + question.ToString();
                }
            }
            DevMessageLbl.Text = devStr;
        }


        /// <summary>
        ///     using the required data submit the question results to the database
        /// </summary>
        /// <param name="questionId"> the relevent Question ID QID</param>
        /// <param name="respondentId">the given Respondent ID RID</param>
        /// <param name="response">the users response as a string pottentially a csv string if multiple answears </param>
        /// <param name="ipAddress"> the users IP address </param>
        public void questionSubmissionToDataBase(int questionId, int respondentId, string response, string ipAddress)
        {
            //setup connection for db
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            SqlCommand myCommand;
            //open connection
            myConn.Open();

            //cmd
            // dont forget your spaces in the sql cmd
            myCommand = new SqlCommand("INSERT INTO Submission (QID_FK,RID_FK,response,ipAddress)" +
                                       " VALUES (@QID_FK, @RID_FK, @response, @ipAddress)", myConn);
            // add params
            myCommand.Parameters.AddWithValue("@QID_FK", questionId);
            myCommand.Parameters.AddWithValue("@RID_FK", respondentId);
            myCommand.Parameters.AddWithValue("@response", response);
            myCommand.Parameters.AddWithValue("@ipAddress", ipAddress);

            //sql exec not expecting a response 
            myCommand.ExecuteNonQuery();

            //close db connection
            myConn.Close();
        }


        /// <summary>
        ///     remove 1 character from the end of a string
        /// </summary>
        /// <param name="str"> the string to alter</param>
        /// <returns></returns>
        public string stringRemoveLastChar(string str)
        {
            List<char> lc = str.ToList<char>();  //convert to char list
            int lastIndex = str.Length - 1;  //find last index
            lc.RemoveAt(lastIndex); //remove last index

            //convert back to string 
            //str = lc.ToString();  // this return the object type as a string
            str = "";
            foreach (char c in lc)
            {
                str += c;
            }

            int num = str.Length;
            return str;
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


        // runs after submit btn hits -- Lifecycle i guess
        // still results to index 0 no matter user selection
        protected void userSelectionRB_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevMessageLbl.Text = "userSelection RB Selected Index : " + userSelectionRB.SelectedIndex.ToString();
        }
    }
}