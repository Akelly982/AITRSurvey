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

        protected void Page_Load(object sender, EventArgs e)
        {

            // post back is false only the first time the page loads
            if (!IsPostBack)
            {
                devConsolelbl.Visible = showDevView;  //testing sql generator

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
                initDb();

            }


            //using our FormData maintain the question list
            // Note this is done because apparently 
            // Dynamically generated controls do not maintain state through postback.
            displayFormQuestions(StaffSearchHandler.QuestionDt, StaffSearchHandler.QuestionValuesDth, StaffSearchHandler.Terminator);
            updateListHolders(StaffSearchHandler.IdList,StaffSearchHandler.ItemTypeList);
            
            //Test update console whil idList
            string str = "";
            List<string> idList = StaffSearchHandler.IdList;
            foreach (string id in idList)
            {
                str += id + " / ";
            }
            devConsolelbl.Text = str;

        }








        // ----------------
        //  functions
        // ---------------
        void updateListHolders(List<string> myIdList, List<string> myItemTypeList)
        {
            //create csv as value in dom element
            //idListHolder.Value = 

            idListHolder.Value = createCsvStringFromListString(myIdList);
            itemTypeListHolder.Value = createCsvStringFromListString(myItemTypeList);
            
           
        }


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
                csvStr = stringRemoveLastChar(csvStr);
               
            }
            else
            {
                csvStr = "Error Empty";
            }

            return csvStr;
        }


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



        //NOTES 
        // - You can not use InnerHTML and Contorls.Add togeathe you get an 
        //   error on the InnerHTML being "Cannot get inner content of DynamicForm because the contents are not literal."
        //   LABELS with </br> written into have it run as code
        public void displayFormQuestions(DataTable questionDt, DataTableHandler QuestionValuesDth, int terminator)
        {
            // ------------------------------------------------
            // Display each Question and child selection items
            // ------------------------------------------------

            //list to hold all question id connections
            List<string> idList = new List<string>();
            List<string> typeList = new List<string>();

            List<TextBox> controlsList = new List<TextBox>();

            bool showQuestionType = true;
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
                    DataTable childDt = QuestionValuesDth.getDataTableByColumnNameAndIntValue("QID_FK", (int)row["QID"]);

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
                DynamicForm.Controls.Add(outerDiv);
            }

            DynamicForm.DataBind();


            //update StaffSearchHandler
            StaffSearchHandler.IdList = idList;
            StaffSearchHandler.ItemTypeList = typeList;
            StaffSearchHandler.ControlTbList = controlsList;
            
        }


        


        




        void initDb()
        {

            // data list reference code from javapoint.com
            // https://www.javatpoint.com/asp-net-datalist#:~:text=The%20ASP.NET%20DataList%20control,or%20a%20table%20from%20database.

            // init db submissions
            SqlConnection myConn;
            myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            myConn.Open(); // establish the connection to the db


            SqlCommand myCommand;
            myCommand = new SqlCommand("SELECT s.SID,s.QID_FK,s.RID_FK,s.response,s.dateAdded,s.ipAddress FROM Respondent r, Questions q, Submission s WHERE s.RID_FK = r.RID AND s.QID_FK = q.QID", myConn);    // setup your CMD and identify your connection
            
            
            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();   //capture your data 
            // prepare your data table 
            DataTable dt = new DataTable();

            //setup table
            dt.Columns.Add("SID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("QID_FK", System.Type.GetType("System.Int32"));
            dt.Columns.Add("RID_FK", System.Type.GetType("System.Int32"));
            dt.Columns.Add("response", System.Type.GetType("System.String"));
            dt.Columns.Add("dateAdded", System.Type.GetType("System.String"));
            dt.Columns.Add("ipAddress", System.Type.GetType("System.String"));


            // row by row add to the data table 
            DataRow currentRow;
            while (myReader.Read())
            {
                currentRow = dt.NewRow();

                currentRow["SID"] = myReader["SID"].ToString();
                currentRow["QID_FK"] = myReader["QID_FK"].ToString();
                currentRow["RID_FK"] = myReader["RID_FK"].ToString();
                currentRow["response"] = myReader["response"];
                currentRow["dateAdded"] = myReader["dateAdded"];
                currentRow["ipAddress"] = myReader["ipAddress"];

                dt.Rows.Add(currentRow);
            }


            //GridView1 is our grid view ID from the designer
            DataGridView.DataSource = dt;
            DataGridView.DataBind();  // on bind the grid view object is refreshed to show the data 

            myConn.Close();  // dont forget to close your db

            
        }


        protected void LogoutBtn_Click(object sender, EventArgs e)
        {
            AppSession.clearStaff();  // reset staff from session
            Response.Redirect("Login.aspx");
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {

            List<TextBox> controlTbList = StaffSearchHandler.ControlTbList;
            List<string> idList = StaffSearchHandler.IdList;
            int index = 0;

            foreach (TextBox tbc in controlTbList)
            {
                //Get current questionId
                string qidStr = idList[index];
                
                

            }

            //CheckBoxList cbl = new CheckBoxList();
            //TextBox tb = new TextBox();
            //int cblCounter = 0;
            //int tbCounter = 0;
            //string whereStmtBuild = "";
            //String stmt = "";
            //List<WebControl> wcList = StaffSearchHandler.ControlList;
            //foreach (WebControl wc in wcList)
            //{

            //    if (wc.GetType() == cbl.GetType())
            //    {
            //        //cblCounter++;
            //        cbl = wc;
            //        foreach (ListItem item in wc.)
            //        {
            //            if (item.Selected)
            //            {
            //                response += item.Text + ",";
            //            }
            //        }

            //    }
            //    else
            //    {
            //        tbCounter++;
            //    }
            //}


        }
    }
}