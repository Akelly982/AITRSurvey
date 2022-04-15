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
    public partial class StaffSearch : System.Web.UI.Page
    {
        bool showDevView = true;
        string initialFindRespondentId = "75";
        string initalGroupRespondentQid = "100"; //first parent question 

        protected void Page_Load(object sender, EventArgs e)
        {

            // post back is false only the first time the page loads
            if (!IsPostBack)
            {
                devConsolelbl.Visible = this.showDevView;  //testing sql generator

                //update staff user labels
                Staff st = AppSession.getStaff();
                if (st == null) //staff not set
                {
                    string s = "Staff Not Set";
                    StaffUsername.Text = s;
                    StaffFirstName.Text = s;
                    StaffLastName.Text = s;
                    StaffEmail.Text = s;
                }
                else
                {
                    StaffUsername.Text = st.Username;
                    StaffFirstName.Text = st.FirstName;
                    StaffLastName.Text = st.LastName;
                    StaffEmail.Text = st.Email;
                }
                
                getFormData();
                setFindRespondentGridView(this.initialFindRespondentId);
                setGroupRespondentGridView(this.initalGroupRespondentQid, "");

            }


            //using our FormData maintain the question list
            // Note this is done because apparently 
            // Dynamically generated controls do not maintain state through postback;

            displayQuestionData(StaffSearchHandler.QuestionDt,StaffSearchHandler.QuestionValuesDth);
            createQuestionSelector(StaffSearchHandler.QuestionDt);
            


        }






        

        // ----------------
        //  Fun..ctions
        // ---------------

        //parent for convertings lists to string csv's
        void updateListHolders(List<string> myIdList, List<string> myItemTypeList)
        {
            //create csv as value in dom element
            //idListHolder.Value = 

            idListHolder.Value = createCsvStringFromListString(myIdList);
            itemTypeListHolder.Value = createCsvStringFromListString(myItemTypeList);
            
           
        }

        //converts a list of strings into a single csv string
        string createCsvStringFromListString(List<string> listString)
        {
            //create temp str
            string csvStr = "";
            foreach (string str in listString)
            {
                csvStr += str + ",";
            }

            //remove last comma from str
            if (csvStr.Length != 0)
            {
                csvStr = stringRemoveLastNumChars(csvStr,1);
               
            }
            else
            {
                csvStr = "Error Empty";
            }

            return csvStr;
        }

        // remove x num chars from the end of a string returning a string
        public string stringRemoveLastNumChars(string str, int numCharRemove)
        {
            List<char> lc = str.ToList<char>();  //convert to char list
            int lastIndex = str.Length - 1;  //find last index

            // for numCharRemove remove from end of char lsit
            for (int i = 0; i < numCharRemove; i++)
            {
                lc.RemoveAt(lastIndex); //remove at index
                lastIndex--;
            }

            
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

        //gets question data tables and terminator value
        void getFormData()
        {
            //DynamicForm


            //init db connection
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            SqlCommand myCommand;


            //get db dt question, questionValues

            // ----------
            // Questions
            // ----------
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


            // -----------------
            // Question Values
            // -----------------
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


            //get terminator
            int terminator = (int)questionDt.Rows[0]["QID"];


            //QuestionDt  remove the terminator row
            questionDt.Rows.RemoveAt(0);

            //set questionValues to dtHandler
            DataTableHandler questionValuesDth = new DataTableHandler(questionValuesDt);

            //set all to handler class
            StaffSearchHandler.Terminator = terminator;
            StaffSearchHandler.QuestionDt = questionDt;
            StaffSearchHandler.QuestionValuesDth = questionValuesDth;
        
        }


        // show question information at top of page so the the end user knows what they are asking for
        void displayQuestionData(DataTable questionDt, DataTableHandler questionValuesDth)
        {
            bool showQuestionType = true;

            foreach (DataRow row in questionDt.Rows)
            {

                //WITH CHILD ITEMS
                //<div> //outerDiv
                //      <Label> QID 100 / what is your favourite color ? </label>
                //    < div > //innerDivDiv
                //      <Label> Possible answaer: green,red,blue </label>
                //    </ div >
                //    <br />   //note this br is hidden in a empty label
                //</div>

                //WITHOUT CHILD ITEMS  (TEXT BOX Question Type)
                //<div> //outerDiv
                //      <Label> QID 100 / what is your favourite color ? </label>
                //    <br />   //note this br is hidden in a empty label
                //</div>


                //SETUP PARENT
                // create generic html element 
                // create outer html
                System.Web.UI.HtmlControls.HtmlGenericControl outerDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                outerDiv.Attributes.Add("runat", "server");

                Label questionLbl = new Label();
                //add question text
                string questionStr = "QID " + ((int)row["QID"]).ToString() + " / " + (string)row["questionText"];
                if (showQuestionType)
                {
                    questionStr += " / QType: " + ((int)row["QTID_FK"]).ToString();
                }
                questionLbl.Text = questionStr;

                //add to outer div
                outerDiv.Controls.Add(questionLbl);

                //create inner html
                System.Web.UI.HtmlControls.HtmlGenericControl innerDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                innerDiv.Attributes.Add("runat", "server");

                if ((int)row["QTID_FK"] == 2 || (int)row["QTID_FK"] == 3) //if should have question values being type check box or radio button
                {
                    //SETUP CHILD
                    // get our child dt
                    DataTable childDt = questionValuesDth.getDataTableByColumnNameAndIntValue("QID_FK", (int)row["QID"]);

                    string answearStr = "Possible Answears: ";
                    //create TextBox and show possible answears
                    foreach (DataRow childRow in childDt.Rows)
                    {

                        answearStr += (string)childRow["text"] + " /";

                    }
                    // remove the last " /" in string
                    stringRemoveLastNumChars(answearStr, 2);
                    // attach string to label
                    Label answearlb = new Label();
                    answearlb.Text = answearStr + " <br/>";

                    //attach to innerDiv
                    innerDiv.Controls.Add(answearlb);
                }


                //ATTACH THE DATA
                // innerDiv -> outerDiv -> parentDiv 
                //attach innerDiv to outerDiv
                outerDiv.Controls.Add(innerDiv);

                //add the final break after innerDiv within outerDiv
                //you can cheat by hiding it in a label
                Label lb = new Label();
                lb.Text = "<br/>";
                outerDiv.Controls.Add(lb);

                //Append outerDiv to parent Div
                DynamicQuestionData.Controls.Add(outerDiv);

            }

        }

       
        //create the GroupRespondent QID Radio button selector 
        void createQuestionSelector(DataTable questionDt)
        {
            //create question selector
            System.Web.UI.HtmlControls.HtmlGenericControl questionsCheckBoxDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            questionsCheckBoxDiv.Attributes.Add("runat", "server");

            //create checkBox
            RadioButtonList questionRbl = new RadioButtonList();   //needs text / value
            questionRbl.Attributes.Add("runat", "server");

            foreach (DataRow row in questionDt.Rows)
            {

                ListItem li = new ListItem();
                li.Text = (string)row["QID"].ToString();
                li.Value = (string)row["QID"].ToString();
                questionRbl.Items.Add(li);

            }
            //add question checkBox to
            questionsCheckBoxDiv.Controls.Add(questionRbl);
            //Append outerDiv to parent Div
            DynamicQidSelector.Controls.Add(questionsCheckBoxDiv);

            StaffSearchHandler.QuestionRbl = questionRbl;
        }


        void OLD_createQuestionData(DataTable questionDt, DataTableHandler questionValuesDth)
        {

            bool showQuestionType = true;
            //list to hold all question id connections
            List<string> idList = new List<string>();
            List<string> typeList = new List<string>();
            List<TextBox> controlsList = new List<TextBox>();

            foreach (DataRow row in questionDt.Rows)
            {


                //GET ID / item type
                int currentId = ((int)row["QID"]);
                int currentQTID = ((int)row["QTID_FK"]);
                idList.Add(currentId.ToString()); // save ID
                typeList.Add(currentQTID.ToString());


                //SETUP PARENT
                // create generic html element 
                // create outer html
                System.Web.UI.HtmlControls.HtmlGenericControl outerDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                outerDiv.Attributes.Add("runat", "server");

                Label questionLbl = new Label();
                //add question text
                string questionStr = "Q" + ((int)row["QID"]).ToString() + " / " + (string)row["questionText"];
                if (showQuestionType)
                {
                    questionStr += " / QType: " + ((int)row["QTID_FK"]).ToString();
                }
                questionLbl.Text = questionStr;

                //add to outer div
                outerDiv.Controls.Add(questionLbl);


                //create inner html
                System.Web.UI.HtmlControls.HtmlGenericControl innerDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                innerDiv.Attributes.Add("runat", "server");

                //SETUP QUESTION RESULTS
                // determin our userSelected data points
                if ((int)row["QTID_FK"] == 2 || (int)row["QTID_FK"] == 3)  //if radio btn or checkBox
                {

                    // get our child dt
                    DataTable childDt = questionValuesDth.getDataTableByColumnNameAndIntValue("QID_FK", (int)row["QID"]);

                    //create checkBox
                    //CheckBoxList cbl = new CheckBoxList();   //needs text / value
                    //cbl.Attributes.Add("runat", "server");
                    //foreach (DataRow childRow in childDt.Rows)
                    //{
                    //    ListItem li = new ListItem();
                    //    li.Text = (string)childRow["text"];
                    //    li.Value = (string)childRow["QVID"].ToString();

                    //    cbl.Items.Add(li);
                    //}
                    //cbl.ID = currentId.ToString();

                    string answearStr = "Possible Answears: ";
                    //create TextBox and show possible answears
                    foreach (DataRow childRow in childDt.Rows)
                    {

                        answearStr += (string)childRow["text"] + " /";

                    }

                    Label answearlb = new Label();
                    answearlb.Text = answearStr + "<br/>";

                    TextBox tb = new TextBox();
                    tb.Attributes.Add("runat", "server");
                    tb.ID = currentId.ToString();

                    //add control to control list
                    controlsList.Add(tb);

                    //attach to innerDiv
                    innerDiv.Controls.Add(answearlb);
                    innerDiv.Controls.Add(tb);
                }
                else
                {
                    TextBox tb = new TextBox();
                    tb.Attributes.Add("runat", "server");
                    tb.ID = currentId.ToString();

                    //add control to control list
                    controlsList.Add(tb);

                    //attach to inner div
                    innerDiv.Controls.Add(tb);
                }

                //attach innerDiv to outerDiv
                outerDiv.Controls.Add(innerDiv);

                //Append outerDiv to parent Div
                DynamicQidSelector.Controls.Add(outerDiv);

                //update StaffSearchHandler
                StaffSearchHandler.IdList = idList;
                StaffSearchHandler.ItemTypeList = typeList;
                StaffSearchHandler.ControlTbList = controlsList;

            }
        }

        //set data source for findRespondent grid view  ---- show all stmt---
        void setShowAllFindRespondentGridView()
        {

            //-------------------------------------
            // initalize FindRespondent GridView 
            //-------------------------------------

            // data list reference code from javapoint.com
            // https://www.javatpoint.com/asp-net-datalist#:~:text=The%20ASP.NET%20DataList%20control,or%20a%20table%20from%20database.

            // init db submissions
            SqlConnection myConn;
            myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            myConn.Open(); // establish the connection to the db


            SqlCommand myCommand;
            myCommand = new SqlCommand("SELECT s.SID, s.RID_FK, q.QID, q.questionText, s.response, s.dateAdded, s.ipAddress FROM Submission s, Questions q WHERE s.QID_FK = q.QID", myConn);    // setup your CMD and identify your connection

            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();   //capture your data 

            // prepare your data table
            // using schema we can adapt for changes in columns
            DataTable schemaDt = myReader.GetSchemaTable();  //gives you a data table consisting of column data
            DataTable dt = new DataTable();

            foreach (DataRow row in schemaDt.Rows)
            {
                dt.Columns.Add((string)row["ColumnName"], System.Type.GetType("System.String")); // setup all columns as datatype string
            }

            // row by row add to the data table 
            // be aware of receiving different datatypes, convert all to string
            int columnsCount = dt.Columns.Count;
            DataRow currentRow;
            while (myReader.Read())
            {

                currentRow = dt.NewRow();

                for (int i = 0; i < columnsCount; i++)
                {
                    //for each row column if data type is not of type string convert to string
                    if (myReader[i].GetType() != System.Type.GetType("System.String"))
                    {
                        currentRow[i] = myReader[i].ToString();
                    }
                    else
                    {
                        currentRow[i] = myReader[i];
                    }

                }

                dt.Rows.Add(currentRow);
            }


            //GridView1 is our grid view ID from the designer
            FindRespondentGridView.DataSource = dt;
            FindRespondentGridView.DataBind(); //dont forget to bind with changing dataSource

            myConn.Close();  // dont forget to close your db

        }

        // set data source for findRespondent grid view
        void setFindRespondentGridView(string findRespondentId)
        {

            //-------------------------------------
            // initalize FindRespondent GridView 
            //-------------------------------------

            // data list reference code from javapoint.com
            // https://www.javatpoint.com/asp-net-datalist#:~:text=The%20ASP.NET%20DataList%20control,or%20a%20table%20from%20database.

            // init db submissions
            SqlConnection myConn;
            myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            myConn.Open(); // establish the connection to the db


            SqlCommand myCommand;
            myCommand = new SqlCommand("SELECT s.SID, s.RID_FK, q.QID, q.questionText, s.response, s.dateAdded, s.ipAddress FROM Submission s, Questions q WHERE s.QID_FK = q.QID AND RID_FK = " + findRespondentId , myConn);    // setup your CMD and identify your connection
            
            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();   //capture your data 
            
            // prepare your data table
            // using schema we can adapt for changes in columns
            DataTable schemaDt = myReader.GetSchemaTable();  //gives you a data table consisting of column data
            DataTable dt = new DataTable();

            foreach(DataRow row in schemaDt.Rows)
            {
                dt.Columns.Add((string)row["ColumnName"], System.Type.GetType("System.String")); // setup all columns as datatype string
            }

            // row by row add to the data table 
            // be aware of receiving different datatypes, convert all to string
            int columnsCount = dt.Columns.Count;
            DataRow currentRow;
            while (myReader.Read())
            {
                
                currentRow = dt.NewRow();

                for (int i = 0; i < columnsCount; i++)
                {
                    //for each row column if data type is not of type string convert to string
                    if (myReader[i].GetType() != System.Type.GetType("System.String"))
                    {
                        currentRow[i] = myReader[i].ToString();
                    }
                    else
                    {
                        currentRow[i] = myReader[i];
                    }

                }

                dt.Rows.Add(currentRow);
            }


            //GridView1 is our grid view ID from the designer
            FindRespondentGridView.DataSource = dt;
            FindRespondentGridView.DataBind(); //dont forget to bind with changing dataSource

            myConn.Close();  // dont forget to close your db

        }

        //set data source for group respondent grid view
        void setGroupRespondentGridView(string qid, string response)
        {

            //-------------------------------------
            // initalize GroupRespondent GridView 
            //-------------------------------------

            // init db submissions
            SqlConnection myConn;
            myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            myConn.Open(); // establish the connection to the db

            SqlCommand myCommand;
            myCommand = new SqlCommand("SELECT s.SID, s.RID_FK, q.QID, q.questionText, s.response, " +
                                        "s.dateAdded, s.ipAddress FROM Submission s, Questions q " +
                                        "WHERE s.QID_FK = q.QID " +
                                        "AND q.QID = " + qid +
                                        " AND s.response LIKE '%" + response + "'", myConn);    // setup your CMD and identify your connection
            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();   //capture your data 

            // prepare your data table
            // using schema we can adapt for changes in columns
            DataTable schemaDt = myReader.GetSchemaTable();  //gives you a data table consisting of column data
            DataTable dt = new DataTable();

            foreach (DataRow row in schemaDt.Rows)
            {
                dt.Columns.Add((string)row["ColumnName"], System.Type.GetType("System.String")); // setup all columns as datatype string
            }

            // row by row add to the data table 
            // be aware of receiving different datatypes, convert all to string
            int columnsCount = dt.Columns.Count;
            DataRow currentRow; //preset above
            while (myReader.Read())
            {

                currentRow = dt.NewRow();

                for (int i = 0; i < columnsCount; i++)
                {
                    //for each row column if data type is not of type string convert to string
                    if (myReader[i].GetType() != System.Type.GetType("System.String"))
                    {
                        currentRow[i] = myReader[i].ToString();
                    }
                    else
                    {
                        currentRow[i] = myReader[i];
                    }

                }

                dt.Rows.Add(currentRow);
            }


            //GridView1 is our grid view ID from the designer
            GroupRespondentGridView.DataSource = dt;
            GroupRespondentGridView.DataBind(); //dont forget to bind with changing dataSource

            myConn.Close();  // dont forget to close your db

        }






        // -----------------------------
        // EVENTS
        // -----------------------------

        protected void LogoutBtn_Click(object sender, EventArgs e)
        {
            AppSession.clearStaff();  // reset staff from session
            Response.Redirect("Login.aspx");
        }


        protected void OLD_SubmitBtn_Click(object sender, EventArgs e)
        {
            



            // w3Schools https://www.w3schools.com/sql/sql_like.asp

            // Textbox and idList should be in the same index order as each other
            //List<TextBox> controlTbList = StaffSearchHandler.ControlTbList;
            //List<string> idList = StaffSearchHandler.IdList;

            //string columnLikeStmt = " AND";
            //// textbox list will be in
            //for (int i = 0; i < idList.Count; i++)
            //{
            //    // AND column LIKE '%%'  <-- single quotes important for sql 
            //    //QID = idList[i];

            //    //if text is not empty
            //    if(controlTbList[i].Text != "")
            //    {                   // qid
            //        columnLikeStmt += " s.response" + " = " + "\'%" +controlTbList[i].Text + "%\' OR";
            //    }
                
            //}

            //devConsolelbl.Text = selectFromWhere + qidStmt + columnLikeStmt;



        }

        

        protected void FindRespondentSubmitButton_Click(object sender, EventArgs e)
        {
            string inputRid = "";
            if(FindRespondentTextBox.Text.Length != 0){
                inputRid = FindRespondentTextBox.Text;
            }

            //update gridView
            setFindRespondentGridView(inputRid);

        }

        protected void GroupRespondentsSubmitBtn_Click(object sender, EventArgs e)
        {

            //get qid
            RadioButtonList questionRbl = StaffSearchHandler.QuestionRbl;
            //check for selected QID <-- radio button so should be only one
            string selectedQid = "";
            foreach (ListItem item in questionRbl.Items)
            {
                if (item.Selected)
                {
                    selectedQid = item.Text;
                }
            }

            string response = "";  
            //get specified user response check if applicable
            if (GroupRespondentResponseTextBox.Text.Length != 0)
            {
                response = GroupRespondentResponseTextBox.Text;
            }
            //response can be left empty, should show all results

            //update grid view
            setGroupRespondentGridView(selectedQid, response);


            

        }

        protected void FindRespondentShowAllSubmitButton_Click(object sender, EventArgs e)
        {
            //update gridView
            setShowAllFindRespondentGridView();
        }
    }
}