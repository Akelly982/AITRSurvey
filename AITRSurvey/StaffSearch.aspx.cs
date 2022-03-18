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
        protected void Page_Load(object sender, EventArgs e)
        {
            initDb();

            // post back is false only the first time the page loads
            if (!IsPostBack)
            {

                initMemberRadioButton();
                initGenderRadioButton();
                initNewspapersCheckBox();
                initBanksCheckBox();
                initBankServicesCheckBox();
            }
        }


        void initMemberRadioButton()
        {
            String[] arr = { "All Users", "Members", "Non Members" };


            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                MemberRadioButtonList.Items.Add(li);
                MemberRadioButtonList.SelectedIndex = 0;
            }
        }

        void initGenderRadioButton()
        {
            String[] arr = { "Other", "Male", "Female" };


            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                GenderRadioButtonList.Items.Add(li);
                GenderRadioButtonList.SelectedIndex = 0;
            }
        }

        void initBanksCheckBox()
        {
            String[] arr = { "CentralWealth", "BankNorth", "EastPac" };


            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                BanksCheckBoxList.Items.Add(li);
            }
        }

        void initBankServicesCheckBox()
        {
            String[] arr = { "Loans", "Mortage", "Credit Cards" };


            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                BankServicesCheckBoxList.Items.Add(li);
            }
        }

        void initNewspapersCheckBox()
        {
            String[] arr = { "Daily News", "Morning Rise News", "Open Source News" };


            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                NewspapersCheckBoxList.Items.Add(li);
            }
        }




        void initDb()
        {
            //sequence
                // open connection
                // query the data 
                // read the data 
                // set the data 
                // close connection

            // data list reference code from javapoint.com
            // https://www.javatpoint.com/asp-net-datalist#:~:text=The%20ASP.NET%20DataList%20control,or%20a%20table%20from%20database.


            // below is based of inclass code from Kriss_M
            // using our db
            SqlConnection myConn;
            myConn = new SqlConnection();

            // connection using a standard const 
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;


            myConn.Open(); // establish the connection to the db

            SqlCommand myCommand;
            myCommand = new SqlCommand("SELECT * FROM tempTable", myConn);    // setup your CMD and identify your connection

            SqlDataReader myReader;
            myReader = myCommand.ExecuteReader();   //capture your data 


            // prepare your data table 
            DataTable dt = new DataTable();                   // dont forget int32

            //V1
            //dt.Columns.Add("ID", System.Type.GetType("System.String"));
            //dt.Columns.Add("FirstName", System.Type.GetType("System.String"));
            //dt.Columns.Add("Lastname", System.Type.GetType("System.String"));


            //V2
            // add dt Column names dynamically on first loop of myReader 
            // myReader needs to be at a row position to read column names else you get an error.
            // This is why it is within the below loop.


            bool firstLoop = true;
            // row by row add to the data table 
            DataRow currentRow;
            while (myReader.Read())
            {
                currentRow = dt.NewRow();

                if (firstLoop)
                {
                    
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        dt.Columns.Add(myReader.GetName(i), System.Type.GetType("System.String"));

                        //testing
                        /*Response.Write("<br />");
                        Response.Write(i.ToString() + " / " + myReader.GetName(i));
                        Response.Write("<br />");*/
                    }
                    firstLoop = false;
                }

                // currentRow is in connection to you dt object names / columns / rows from above
                // myReader data is in connection to your db names / columns / rows
                currentRow["ID"] = myReader[0].ToString();
                currentRow["FirstName"] = myReader[1].ToString();
                currentRow[2] = myReader["LastName"].ToString();


                dt.Rows.Add(currentRow);
            }


            //GridView1 is our grid view ID from the designer
            DataGridView.DataSource = dt;
            DataGridView.DataBind();  // on bind the grid view object is refreshed to show the data 

            myConn.Close();  // dont forget to close your db

            
        }

    }
}