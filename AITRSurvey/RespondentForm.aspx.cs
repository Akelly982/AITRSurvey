using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AITRSurvey
{
    public partial class RespondentForm : System.Web.UI.Page
    {
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
                IsMemberRBList.SelectedIndex = 0;

            }
        }

        protected void IsMemberRBList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update
            isMemberTrueLabel.Text = "Thank you for your membership";
            
        }

        protected void SubmitBtn_Click(object sender, EventArgs e)
        {

        }
    }
}