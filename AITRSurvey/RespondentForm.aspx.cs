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
    public partial class RespondentForm : System.Web.UI.Page
    {

        const string SUCCESSFULL = "successfull";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                ageRangeRbData();
                genderRBData();
                isMemberRBData();
            }


            if(IsMemberRBList.SelectedIndex == 0)
            {
                MemberDataSection.Visible = false;
            }
            else
            {
                MemberDataSection.Visible = true;
            }


        }



        public void ageRangeRbData()
        {
            String[] arr = {"0-17","18-25","26-40","41-59","60+"};

            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                AgeRangeRBList.Items.Add(li);
                AgeRangeRBList.SelectedIndex = 0;
            }
        }

        public void genderRBData()
        {
            String[] arr = { "Male", "Female", "Other"};

            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                GenderRBList.Items.Add(li);
                GenderRBList.SelectedIndex = 0;
            }
        }

        public void isMemberRBData()
        {
            String[] arr = { "Anonymous", "Member"};

            // Add all items to the selection list
            for (int i = 0; i < arr.Length; i++)
            {
                ListItem li = new ListItem();
                li.Text = arr[i];
                li.Value = i.ToString();    // value has to be in the form of a String

                IsMemberRBList.Items.Add(li);
                IsMemberRBList.SelectedIndex = 0; // 0 is anonymous

            }
        }

        protected void IsMemberRBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update
            
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {
            ErrMsgLabel.Text = "";

            if (IsMemberRBList.SelectedIndex == 1)  //if member selected
            {
                //ErrMsgLabel.Text = "Member";
                memberRespondent();
            }
            else
            {
                //ErrMsgLabel.Text = "Anonymous user";
                anonymousRespondent();
            }
        }






        public void memberRespondent()
        {

            //check data fields
            //Check anonymous data fields
            string result = checkAnonymousDataFields();
            if (result != SUCCESSFULL)
            {
                ErrMsgLabel.Text = result;  //relay issue back to user
                return;
            }

            // check member data fields
            result = checkMemberDataFields();
            if (result != SUCCESSFULL)
            {
                ErrMsgLabel.Text = result;  //relay issue back to user
                return;
            }
            
            //Test
            //ErrMsgLabel.Text = SUCCESSFULL + " member data check";

            // Retrieve User inputs
            //In SQL Table firstName and MID_FK (member id foreign key) default to 
            //   firstname default = "anonymous"
            //   MID_FK default = 1
            string ageRange = AgeRangeRBList.SelectedItem.Text;
            string gender = GenderRBList.SelectedItem.Text;
            string state = StateTextBox.Text;
            string suburb = SuburbTextBox.Text;
            string postcode = PostCodeTextBox.Text;
            string firstName = FirstNameTextBox.Text; // is set for member users

            string lastName = LastNameTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string dob = DateOfBirthTextBox.Text;
            string streetAddress = StreetAddressTextBox.Text;
            string email = EmailTextBox.Text;


            //Db Insert Member Data
              // lastaName, phoneNumber, dateOfBirth, streetAddress, email
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;
            myConn.Open(); // establish the connection to the db

            SqlCommand myCommand;
            myCommand = new SqlCommand("INSERT INTO Member (lastName,phoneNumber,dateOfBirth,streetAddress,email)" +
                                        " OUTPUT INSERTED.MID" +
                                        " VALUES(@lastName, @phoneNumber, @dateOfBirth, @streetAddress, @email)", myConn);

            myCommand.Parameters.AddWithValue("@lastName", lastName);
            myCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
            myCommand.Parameters.AddWithValue("@dateOfBirth", dob);
            myCommand.Parameters.AddWithValue("@streetAddress", streetAddress);
            myCommand.Parameters.AddWithValue("@email", email);
            
            //sql query Output INSERTED.id returns a dataset holding 1 row stating the value of given column 
            int mid = (int)myCommand.ExecuteScalar(); //must cast what the object should be
            ErrMsgLabel.Text = "member db insert succesfull / id:" + mid.ToString();




            //Db Insert Respondent Data
                // due to member we need the MID_FK and FirstName set as well.
            myCommand = new SqlCommand("INSERT INTO Respondent(firstName, postcode, gender, ageRange, suburb, state, MID_FK)" +
                                        " OUTPUT INSERTED.RID" +
                                        " VALUES(@firstName, @postcode, @gender, @ageRange, @suburb, @state, @MID_FK)", myConn);

            myCommand.Parameters.AddWithValue("@MID_FK", mid);
            myCommand.Parameters.AddWithValue("@firstName", firstName);
            myCommand.Parameters.AddWithValue("@postcode", postcode);
            myCommand.Parameters.AddWithValue("@gender", gender);
            myCommand.Parameters.AddWithValue("@ageRange", ageRange);
            myCommand.Parameters.AddWithValue("@suburb", suburb);
            myCommand.Parameters.AddWithValue("@state", state);


            //sql query Output INSERTED.id returns a dataset holding 1 row stating the value of given column 
            int resId = (int)myCommand.ExecuteScalar(); //must cast what the object should be




            //close db connection
            myConn.Close();

            //Test
            ErrMsgLabel.Text = "member db insert succesfull / resId:" + resId.ToString() + " / memberId: " + mid.ToString();

            //update session with respondent
            AppSession.setRespondentId(resId);

            //launch Survey
            startSurvey();
        }



        public void anonymousRespondent()
        {
            
            //check data fields
            string result = checkAnonymousDataFields();
            if(result != SUCCESSFULL)
            {
                ErrMsgLabel.Text = result;  //relay issue back to user
                return;
            }

            //Test
            //ErrMsgLabel.Text = SUCCESSFULL + " anonymous data check";

            
            // Retrieve User inputs
            //In SQL Table firstName and MID_FK (member id foreign key) default to 
            //   firstname default = "anonymous"
            //   MID_FK default = 1
            string ageRange = AgeRangeRBList.SelectedItem.Text;
            string gender = GenderRBList.SelectedItem.Text;
            string state = StateTextBox.Text;
            string suburb = SuburbTextBox.Text;
            string postcode = PostCodeTextBox.Text;

            //Test
            //ErrMsgLabel.Text = ageRange + "/" + gender + "/" + state + "/" + suburb + "/" + postcode;

            // anonymous Insert db
            SqlConnection myConn = new SqlConnection();

            myConn.ConnectionString = AppConstants.DB_CONNECT_STR;

            myConn.Open(); // establish the connection to the db

            SqlCommand myCommand;
            myCommand = new SqlCommand("INSERT INTO Respondent (postcode,gender,ageRange,suburb,state) " +
                "OUTPUT INSERTED.RID " +
                "VALUES (@postcode,@gender,@ageRange,@suburb,@state)", myConn);

            myCommand.Parameters.AddWithValue("@postcode", postcode);
            myCommand.Parameters.AddWithValue("@gender", gender);
            myCommand.Parameters.AddWithValue("@ageRange", ageRange);
            myCommand.Parameters.AddWithValue("@suburb", suburb);
            myCommand.Parameters.AddWithValue("@state", state);


            //sql query Output INSERTED.id returns a dataset holding 1 row stating the value of given column 
            int resId = (int)myCommand.ExecuteScalar(); //must cast what the object should be

            //close db connection
            myConn.Close();

            ErrMsgLabel.Text = "Anonymous db insert succesfull / id:" + resId.ToString();

            //update session with respondent
            AppSession.setRespondentId(resId);


            //launch Survey
            startSurvey();
        }












        public void startSurvey()
        {
            Response.Redirect("DynamicSurvey.aspx");
        }




        // added in backwords so last error message comes up as first error message
        public string checkAnonymousDataFields()
        {
            string result = SUCCESSFULL;
            //skip radio button fields they should be preset if not set by user.

            //state
            string state = StateTextBox.Text.Trim();
            if (state.Length == 0)
            {
                result = "Error with state.";
            }

            //suburb 
            string suburb = SuburbTextBox.Text.Trim();
            if (suburb.Length == 0)
            {
                result = "Error with suburb.";
            }

            //postcode
            string postcode = PostCodeTextBox.Text.Trim();
            if (postcode.Length == 0)
            {
                result = "Error with postcode.";
            }


            return result;
        }


        public string checkMemberDataFields()
        {
            string result = SUCCESSFULL;

            //street address
            string streetAddress = StreetAddressTextBox.Text.Trim();
            if (streetAddress.Length == 0)
            {
                result = "Error with street address field.";
            }

            //email
            string email = EmailTextBox.Text.Trim();
            if (email.Length == 0)
            {
                result = "Error with email field.";
            }

            //DOB
            string dob = DateOfBirthTextBox.Text.Trim();
            if (dob.Length == 0)
            {
                result = "Error with date of birth field. (dd-mm-yyyy)";
            }

            //phone number
            string phoneNumber = PhoneNumberTextBox.Text.Trim();
            if (phoneNumber.Length == 0)
            {
                result = "Error with phone number field.";
            }

            //lastname
            string lastName = LastNameTextBox.Text.Trim();
            if (lastName.Length == 0)
            {
                result = "Error with last name field.";
            }

            //firstname
            string firstName = FirstNameTextBox.Text.Trim();
            if (firstName.Length == 0)
            {
                result = "Error with first name field.";
            }


            return result;
        }


    }
}