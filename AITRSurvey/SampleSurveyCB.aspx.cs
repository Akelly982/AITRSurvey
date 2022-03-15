using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AITRSurvey
{
    public partial class SampleSurveyCB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // post back is false only the first time the page loads
            if (!IsPostBack)
            {
                String[] snacksArr = {"Snickers","Ice-Cream","Jelly","Chocolate",
                                   "Milky-Way","Starburst","Twinkies","Cake"};


                // Add all items to the selection list
                for (int i = 0; i < snacksArr.Length; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = snacksArr[i];
                    li.Value = i.ToString();    // value has to be in the form of a String
                    
                    userSelectionCB.Items.Add(li);
                }
                
            }


        }
    }
}